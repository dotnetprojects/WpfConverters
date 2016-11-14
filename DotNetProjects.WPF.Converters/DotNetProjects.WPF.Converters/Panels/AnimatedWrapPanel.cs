using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace DotNetProjects.WPF.Converters.Panels
{
    /// <summary>
    /// from http://tech.pro/tutorial/736/wpf-tutorial-creating-a-custom-panel-control
    /// </summary>
  public class AnimatedWrapPanel : Panel
  {
    private TimeSpan _AnimationLength = TimeSpan.FromMilliseconds(200);

    protected override Size MeasureOverride(Size availableSize)
    {
      Size infiniteSize = new Size(double.PositiveInfinity, double.PositiveInfinity);
      double curX = 0, curY = 0, curLineHeight = 0;
      foreach (UIElement child in this.Children)
      {
        child.Measure(infiniteSize);

        if (curX + child.DesiredSize.Width > availableSize.Width)
        { //Wrap to next line
          curY += curLineHeight;
          curX = 0;
          curLineHeight = 0;
        }

        curX += child.DesiredSize.Width;
        if(child.DesiredSize.Height > curLineHeight)
          curLineHeight = child.DesiredSize.Height;
      }

      curY += curLineHeight;

      Size resultSize = new Size();
      resultSize.Width = double.IsPositiveInfinity(availableSize.Width) ? curX : availableSize.Width;
      resultSize.Height = double.IsPositiveInfinity(availableSize.Height) ? curY : availableSize.Height;

      return resultSize;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
      if (this.Children == null || this.Children.Count == 0)
        return finalSize;

      TranslateTransform trans = null;
      double curX = 0, curY = 0, curLineHeight = 0;

      foreach (UIElement child in this.Children)
      {
        trans = child.RenderTransform as TranslateTransform;
        if (trans == null)
        {
          child.RenderTransformOrigin = new Point(0, 0);
          trans = new TranslateTransform();
          child.RenderTransform = trans;
        }

        if (curX + child.DesiredSize.Width > finalSize.Width)
        { //Wrap to next line
          curY += curLineHeight;
          curX = 0;
          curLineHeight = 0;
        }

        child.Arrange(new Rect(0, 0, child.DesiredSize.Width, child.DesiredSize.Height));

        trans.BeginAnimation(TranslateTransform.XProperty, new DoubleAnimation(curX, this._AnimationLength), HandoffBehavior.Compose);
        trans.BeginAnimation(TranslateTransform.YProperty, new DoubleAnimation(curY, this._AnimationLength), HandoffBehavior.Compose);

        curX += child.DesiredSize.Width;
        if (child.DesiredSize.Height > curLineHeight)
          curLineHeight = child.DesiredSize.Height;       
      }

      return finalSize;
    } 
  }
}
