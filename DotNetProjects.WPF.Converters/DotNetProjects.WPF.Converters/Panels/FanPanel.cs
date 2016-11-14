using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace DotNetProjects.WPF.Converters.Panels
{
    /// <summary>
    /// from (http://www.codeproject.com/Articles/15705/FishEyePanel-FanPanel-Examples-of-custom-layout-pa)
    /// </summary>
    public partial class FanPanel : System.Windows.Controls.Panel
    {
        public FanPanel()
        {
            this.Background = Brushes.Red;                    // Good for debugging
            this.Background = Brushes.Transparent;            // Make sure we get mouse events
            this.MouseEnter += new MouseEventHandler(this.FanPanel_MouseEnter);
            this.MouseLeave += new MouseEventHandler(this.FanPanel_MouseLeave);
        }

        private Size ourSize;
        private bool foundNewChildren = false;
        private double scaleFactor = 1;

        public static readonly RoutedEvent RefreshEvent = EventManager.RegisterRoutedEvent("Refresh", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FanPanel));
        public event RoutedEventHandler Refresh
        {
            add { this.AddHandler(RefreshEvent, value); }
            remove { this.RemoveHandler(RefreshEvent, value); }
        }

        public static readonly RoutedEvent AnimationCompletedEvent = EventManager.RegisterRoutedEvent("AnimationCompleted", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FanPanel));
        public event RoutedEventHandler AnimationCompleted
        {
            add { this.AddHandler(AnimationCompletedEvent, value); }
            remove { this.RemoveHandler(AnimationCompletedEvent, value); }
        }

        public bool IsWrapPanel
        {
            get { return (bool)this.GetValue(IsWrapPanelProperty); }
            set { this.SetValue(IsWrapPanelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsWrapPanel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsWrapPanelProperty =
            DependencyProperty.Register("IsWrapPanel", typeof(bool), typeof(FanPanel), new UIPropertyMetadata(false, IsWrapPanelChanged));

        public static void IsWrapPanelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Wrap Panel Changed");
            FanPanel me = (FanPanel)d;
            me.InvalidateArrange();
        }

        public int AnimationMilliseconds
        {
            get { return (int)this.GetValue(AnimationMillisecondsProperty); }
            set { this.SetValue(AnimationMillisecondsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AnimationMilliseconds.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AnimationMillisecondsProperty =
            DependencyProperty.Register("AnimationMilliseconds", typeof(int), typeof(FanPanel), new UIPropertyMetadata(1250));

        protected override Size MeasureOverride(Size availableSize)
        {
            // Allow children as much room as they want - then scale them
            Size size = new Size(Double.PositiveInfinity, Double.PositiveInfinity);
            foreach (UIElement child in this.Children)
            {
                child.Measure(size);
            }

            // EID calls us with infinity, but framework doesn't like us to return infinity
            if (double.IsInfinity(availableSize.Height) || double.IsInfinity(availableSize.Width))
                return new Size(600, 600);
            else
                return availableSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            this.RaiseEvent(new RoutedEventArgs(FanPanel.RefreshEvent, null));
            if (this.Children == null || this.Children.Count == 0)
                return finalSize;

            this.ourSize = finalSize;
            this.foundNewChildren = false;

            foreach (UIElement child in this.Children)
            {
                // If this is the first time we've seen this child, add our transforms
                if (child.RenderTransform as TransformGroup == null)
                {
                    this.foundNewChildren = true;
                    child.RenderTransformOrigin = new Point(0.5, 0.5);
                    TransformGroup group = new TransformGroup();
                    child.RenderTransform = group;
                    group.Children.Add(new ScaleTransform());
                    group.Children.Add(new TranslateTransform());
                    group.Children.Add(new RotateTransform());
                }

                // Don't allow our children any clicks in icon form
                child.IsHitTestVisible = this.IsWrapPanel;
                child.Arrange(new Rect(0, 0, child.DesiredSize.Width, child.DesiredSize.Height));

                // Scale the children so they fit in our size
                double sf = (Math.Min(this.ourSize.Width, this.ourSize.Height) * 0.4) / Math.Max(child.DesiredSize.Width, child.DesiredSize.Height);
                this.scaleFactor = Math.Min(this.scaleFactor, sf);
            }

            this.AnimateAll();

            return finalSize;
        }

        void FanPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!this.IsWrapPanel)
            {
                System.Diagnostics.Debug.WriteLine("Mouse Enter");
                this.InvalidateArrange();
            }
        }

        void FanPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!this.IsWrapPanel)
            {
                System.Diagnostics.Debug.WriteLine("Mouse Leave");
                this.InvalidateArrange();
            }
        }

        private void AnimateAll()
        {
            System.Diagnostics.Debug.WriteLine("AnimateAll()");
            if (!this.IsWrapPanel)
            {
                if (!this.IsMouseOver)
                {
                    // Rotate all the children into a stack
                    double r = 0;
                    int sign = +1;
                    foreach (UIElement child in this.Children)
                    {
                        if (this.foundNewChildren)
                            child.SetValue(Panel.ZIndexProperty, 0);

                        this.AnimateTo(child, r, 0, 0, this.scaleFactor);
                        r += sign * 15;         // +-15 degree intervals
                        if (Math.Abs(r) > 90)
                        {
                            r = 0;
                            sign = -sign;
                        }
                    }
                }
                else
                {
                    // On mouse over explode out the children and don't rotate them
                    Random rand = new Random();
                    foreach (UIElement child in this.Children)
                    {
                        child.SetValue(Panel.ZIndexProperty, rand.Next(this.Children.Count));
                        double x = (rand.Next(16) - 8) * this.ourSize.Width / 32;
                        double y = (rand.Next(16) - 8) * this.ourSize.Height / 32;
                        this.AnimateTo(child, 0, x, y, this.scaleFactor);
                    }
                }
            }
            else
            {
                // Pretend to be a wrap panel
                double maxHeight = 0, x = 0, y = 0;
                foreach (UIElement child in this.Children)
                {
                    if (child.DesiredSize.Height > maxHeight)               // Row height
                        maxHeight = child.DesiredSize.Height;
                    if (x + child.DesiredSize.Width > this.ourSize.Width)
                    {
                        x = 0;
                        y += maxHeight;
                    }

                    if (y > this.ourSize.Height - maxHeight)
                        child.Visibility = Visibility.Hidden;
                    else
                        child.Visibility = Visibility.Visible;

                    this.AnimateTo(child, 0, x, y, 1);
                    x += child.DesiredSize.Width;
                }
            }
        }

        private void AnimateTo(UIElement child, double r, double x, double y, double s)
        {
            TransformGroup group = (TransformGroup)child.RenderTransform;
            ScaleTransform scale = (ScaleTransform)group.Children[0];
            TranslateTransform trans = (TranslateTransform)group.Children[1];
            RotateTransform rot = (RotateTransform)group.Children[2];

            rot.BeginAnimation(RotateTransform.AngleProperty, this.MakeAnimation(r, this.anim_Completed));
            trans.BeginAnimation(TranslateTransform.XProperty, this.MakeAnimation(x));
            trans.BeginAnimation(TranslateTransform.YProperty, this.MakeAnimation(y));
            scale.BeginAnimation(ScaleTransform.ScaleXProperty, this.MakeAnimation(s));
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, this.MakeAnimation(s));
        }

        private DoubleAnimation MakeAnimation(double to)
        {
            return this.MakeAnimation(to, null);
        }

        private DoubleAnimation MakeAnimation(double to, EventHandler endEvent)
        {
            DoubleAnimation anim = new DoubleAnimation(to, TimeSpan.FromMilliseconds(this.AnimationMilliseconds));
            anim.AccelerationRatio = 0.2;
            anim.DecelerationRatio = 0.7;
            if (endEvent != null)
                anim.Completed += endEvent;
            return anim;
        }

        void anim_Completed(object sender, EventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(FanPanel.AnimationCompletedEvent, e));
        }
    }
}
