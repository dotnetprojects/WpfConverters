using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DotNetProjects.WPF.Converters.Panels
{
    /// <summary>
    /// Circle Panel, arranges child element within a circle.
    /// from: http://www.codebullets.com/custom-circle-panel-wpf-part-1-216
    /// </summary>
    public class CirclePanel : Panel
    {
        /// <summary>
        /// Maximal Height of an child element.
        /// </summary>
        public const double MaxChildHeight = 23;

        /// <summary>
        /// Register Dependency Property
        /// </summary>
        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register(
        "Radius",
        typeof(double),
        typeof(CirclePanel),
        new FrameworkPropertyMetadata(
           (double)100,
           FrameworkPropertyMetadataOptions.AffectsRender,
           new PropertyChangedCallback(RadiusChanged)));

        /// <summary>
        /// Initializes a new instance of the <see cref="CirclePanel"/> class.
        /// </summary>
        public CirclePanel()
        {
            // Set a default value for the inner circle radius
            this.Radius = 100d;
        }

        /// <summary>
        /// Gets or sets the current scale.
        /// </summary>
        /// <value>The scale.</value>
        public double Radius
        {
            get { return (double)this.GetValue(RadiusProperty); }
            set
            {
                this.SetValue(RadiusProperty, value);
            }
        }

        /// <summary>
        /// Radiuses the changed.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void RadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CirclePanel)d).InvalidateArrange();
            ((CirclePanel)d).InvalidateVisual();
        }

        /// <summary>
        /// Measures the override.
        /// </summary>
        /// <param name="availableSize">Size of the available.</param>
        /// <returns></returns>
        protected override Size MeasureOverride(Size availableSize)
        {

            Size elementsAvailableSize = new Size(double.PositiveInfinity, MaxChildHeight);

            double maxElementWidth = 0;

            foreach (UIElement child in Children)
            {
                child.Measure(elementsAvailableSize);
                maxElementWidth = Math.Max(child.DesiredSize.Width, maxElementWidth);
            }

            double panelWith = 2 * this.Radius + 2 * maxElementWidth;

            return new Size(panelWith, panelWith);
        }

        /// <summary>
        /// When overridden in a derived class, positions child elements and determines a size for a <see cref="T:System.Windows.FrameworkElement"/> derived class.
        /// </summary>
        /// <param name="finalSize">The final area within the parent that this element should use to arrange itself and its children.</param>
        /// <returns>The actual size used.</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (this.Children != null && this.Children.Count > 0)
            {
                ArrangeElements();
            }

            return finalSize;
        }

        /// <summary>
        /// Arranges this instance.
        /// </summary>
        private void ArrangeElements()
        {
            double degree = 0;
            double degreeStep = (double)360 / this.Children.Count;

            double mX = this.DesiredSize.Width / 2;
            double mY = this.DesiredSize.Height / 2;

            foreach (UIElement child in Children)
            {
                double angle = Math.PI * degree / 180.0;
                double x = Math.Cos(angle) * this.Radius;
                double y = Math.Sin(angle) * this.Radius;

                child.RenderTransform = new RotateTransform(degree, 0, 0);
                child.Arrange(new Rect(mX + x, mY + y, child.DesiredSize.Width, child.DesiredSize.Height));

                degree += degreeStep;
            }
        }
    }
}
