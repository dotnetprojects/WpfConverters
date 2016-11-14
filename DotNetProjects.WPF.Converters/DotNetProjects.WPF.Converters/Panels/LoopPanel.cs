/* Copyright (c) 2009, Dr. WPF
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *
 *   * Redistributions of source code must retain the above copyright
 *     notice, this list of conditions and the following disclaimer.
 * 
 *   * Redistributions in binary form must reproduce the above copyright
 *     notice, this list of conditions and the following disclaimer in the
 *     documentation and/or other materials provided with the distribution.
 * 
 *   * The name Dr. WPF may not be used to endorse or promote products
 *     derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY Dr. WPF ``AS IS'' AND ANY
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL Dr. WPF BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DotNetProjects.WPF.Converters.Panels
{
    public class LoopPanel : Panel
    {
        private static class CalculateExtensions
        {
            public static bool AreVirtuallyEqual(double d1, double d2)
            {
                if (double.IsPositiveInfinity(d1))
                    return double.IsPositiveInfinity(d2);

                if (double.IsNegativeInfinity(d1))
                    return double.IsNegativeInfinity(d2);

                if (double.IsNaN(d1))
                    return double.IsNaN(d2);

                double n = d1 - d2;
                double d = (Math.Abs(d1) + Math.Abs(d2) + 10) * 1.0e-15;
                return (-d < n) && (d > n);
            }

            public static bool AreVirtuallyEqual(Rect r1, Rect r2)
            {
                return CalculateExtensions.AreVirtuallyEqual(r1.TopLeft, r2.TopLeft) && CalculateExtensions.AreVirtuallyEqual(r1.BottomRight, r2.BottomRight);
            }

            public static bool AreVirtuallyEqual(Point p1, Point p2)
            {
                return CalculateExtensions.AreVirtuallyEqual(p1.X, p2.X) && CalculateExtensions.AreVirtuallyEqual(p1.Y, p2.Y);
            }

            public static bool LessThanOrVirtuallyEqual(double d1, double d2)
            {
                return (d1 < d2 || AreVirtuallyEqual(d1, d2));
            }

            public static bool StrictlyLessThan(double d1, double d2)
            {
                return (d1 < d2 && !AreVirtuallyEqual(d1, d2));
            }

            public static bool StrictlyGreaterThan(double d1, double d2)
            {
                return (d1 > d2 && !AreVirtuallyEqual(d1, d2));
            }
        }

        #region constructors

        #region static

        static LoopPanel()
        {
            EventManager.RegisterClassHandler(typeof(LoopPanel), FrameworkElement.RequestBringIntoViewEvent, new RequestBringIntoViewEventHandler(OnRequestBringIntoView));
        }

        #endregion

        #region public

        public LoopPanel()
        {
            this.PivotalChildIndex = -1;
        }

        #endregion

        #endregion

        #region dependency properties

        #region BringChildrenIntoView

        /// <summary>
        /// BringChildrenIntoView Dependency Property
        /// </summary>
        public static readonly DependencyProperty BringChildrenIntoViewProperty =
            DependencyProperty.Register("BringChildrenIntoView", typeof(bool), typeof(LoopPanel),
                new FrameworkPropertyMetadata((bool)false));

        /// <summary>
        /// Gets or sets the BringChildrenIntoView property.  This dependency property 
        /// indicates whether the panel should automatically adjust its Offset property to bring 
        /// its direct children into view when they raise the RequestBringIntoView event.
        /// </summary>
        public bool BringChildrenIntoView
        {
            get { return (bool)this.GetValue(BringChildrenIntoViewProperty); }
            set { this.SetValue(BringChildrenIntoViewProperty, value); }
        }

        #endregion

        #region Offset

        /// <summary>
        /// Offset Dependency Property
        /// </summary>
        public static readonly DependencyProperty OffsetProperty =
            DependencyProperty.Register("Offset", typeof(double), typeof(LoopPanel),
                new FrameworkPropertyMetadata((double)0.5d,
                    FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Gets or sets the Offset property.  This dependency property indicates the logical 
        /// offset of children (in the orientation direction) within the LoopPanel. 
        /// 
        /// The Offset property provides for logical units in the direction of the
        /// panel's orientation.  This allows the panel to support per-item scrolling.
        /// Each logical unit represents a single child.  Since children can have 
        /// varying sizes, this means a fraction of a unit (such as 0.1) will represent
        /// different lengths for different children.  All placement of children occurs
        /// around the current pivotal child.  The pivotal child's index is determined 
        /// by truncating the Offset value and applying a modulo using the child count.
        /// 
        /// Another way to think about this property is that its non-fractional portion
        /// represents the index of the pivotal child (the child around which other 
        /// children are arranged). If the value is between 0 (inclusive) 
        /// and 1 (exclusive), the first child is the pivotal child.  The fractional
        /// portion represents an offset for the pivotal child. 
        /// 
        /// The default value of 0.5 means that the first child is the pivotal child and 
        /// it is centered along its extent (the child's width for a horizontal 
        /// orientation or height for a vertical orientation).
        /// </summary>
        public double Offset
        {
            get { return (double)this.GetValue(OffsetProperty); }
            set { this.SetValue(OffsetProperty, value); }
        }

        #endregion

        #region Orientation

        /// <summary>
        /// Orientation Dependency Property
        /// </summary>
        public static readonly DependencyProperty OrientationProperty =
            StackPanel.OrientationProperty.AddOwner(typeof(LoopPanel),
                new FrameworkPropertyMetadata((Orientation)Orientation.Horizontal,
                    FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Gets or sets the Orientation property.  This dependency property 
        /// indicates the layout orientation of the looping children.
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)this.GetValue(OrientationProperty); }
            set { this.SetValue(OrientationProperty, value); }
        }

        #endregion

        #region RelativeOffset

        /// <summary>
        /// RelativeOffset Dependency Property
        /// </summary>
        public static readonly DependencyProperty RelativeOffsetProperty =
            DependencyProperty.Register("RelativeOffset", typeof(double), typeof(LoopPanel),
                new FrameworkPropertyMetadata((double)0.5d, FrameworkPropertyMetadataOptions.AffectsArrange),
                    new ValidateValueCallback(IsRelativeOffsetValid));

        /// <summary>
        /// Gets or sets the RelativeOffset property.  This dependency property 
        /// indicates where the pivotal child is positioned within the panel. 
        /// The default value of 0.5 means that the pivotal child is positioned
        /// with its edge at the center of the panel.
        /// </summary>
        public double RelativeOffset
        {
            get { return (double)this.GetValue(RelativeOffsetProperty); }
            set { this.SetValue(RelativeOffsetProperty, value); }
        }

        private static bool IsRelativeOffsetValid(object value)
        {
            double v = (double)value;
            return (!double.IsNaN(v)
                && !double.IsPositiveInfinity(v)
                && !double.IsNegativeInfinity(v)
                && v >= 0.0d && v <= 1.0d);
        }

        #endregion

        #endregion

        #region properties

        #region internal

        internal int PivotalChildIndex { get; private set; }

        #endregion

        #endregion

        #region methods

        #region public

        /// <summary>
        /// Provides a helper method for scrolling the panel by viewport units rather than
        /// adjusting the Offset property (which uses logical units)
        /// </summary>
        /// <param name="viewportUnits">The number of device independent pixels to scroll.  A positive value 
        /// scrolls the Offset forward and a negative value scrolls it backward</param>
        public void Scroll(double viewportUnits)
        {
            int childCount = this.InternalChildren.Count;
            if (childCount == 0) return;

            // determine the new Offset value required to move the specified viewport units
            double newOffset = this.Offset;
            int childIndex = this.PivotalChildIndex;
            bool isHorizontal = (this.Orientation == Orientation.Horizontal);
            bool isForwardMovement = (viewportUnits > 0);
            int directionalFactor = isForwardMovement ? 1 : -1;
            double remainingExtent = Math.Abs(viewportUnits);
            UIElement child = this.InternalChildren[childIndex];
            double childExtent = isHorizontal ? child.DesiredSize.Width : child.DesiredSize.Height;
            double fractionalOffset = (this.Offset > 0) ? this.Offset - Math.Truncate(this.Offset) : 1.0d - Math.Truncate(this.Offset) + this.Offset;
            double childRemainingExtent = isForwardMovement ? childExtent - fractionalOffset * childExtent : fractionalOffset * childExtent;
            if (CalculateExtensions.LessThanOrVirtuallyEqual(childRemainingExtent, remainingExtent))
            {
                newOffset = Math.Round(isForwardMovement ? newOffset + 1 - fractionalOffset : newOffset - fractionalOffset);
                remainingExtent -= childRemainingExtent;
                while (CalculateExtensions.StrictlyGreaterThan(remainingExtent, 0.0d))
                {
                    childIndex = isForwardMovement ? (childIndex + 1) % childCount : (childIndex == 0 ? childCount - 1 : childIndex - 1);
                    child = this.InternalChildren[childIndex];
                    childExtent = isHorizontal ? child.DesiredSize.Width : child.DesiredSize.Height;
                    if (CalculateExtensions.LessThanOrVirtuallyEqual(childExtent, remainingExtent))
                    {
                        newOffset += 1.0d * directionalFactor;
                        remainingExtent -= childExtent;
                    }
                    else
                    {
                        newOffset += remainingExtent * directionalFactor / childExtent;
                        remainingExtent = 0.0d;
                    }
                }
            }
            else
            {
                newOffset += remainingExtent * directionalFactor / childExtent;
                remainingExtent = 0.0d;
            }
            this.Offset = newOffset;
        }

        #endregion

        #region protected

        protected override Size ArrangeOverride(Size finalSize)
        {
            UIElementCollection children = this.InternalChildren;
            bool isHorizontal = (this.Orientation == Orientation.Horizontal);
            Rect childRect = new Rect();
            double childExtent = 0.0;
            int childCount = children.Count;
            Rect controlBounds = new Rect(finalSize);
            double nextEdge = 0, priorEdge = 0;
            int nextIndex = 0, priorIndex = 0;
            this.PivotalChildIndex = -1;

            // arrange pivotal child
            if (childCount > 0)
            {
                // determine pivotal child index
                double adjustedOffset = this.Offset % childCount;
                if (adjustedOffset < 0)
                {
                    adjustedOffset = (adjustedOffset + childCount) % childCount;
                }
                this.PivotalChildIndex = (int)adjustedOffset;

                nextIndex = (this.PivotalChildIndex + 1) % childCount;
                priorIndex = (this.PivotalChildIndex == 0) ? childCount - 1 : this.PivotalChildIndex - 1;

                UIElement child = (UIElement)children[this.PivotalChildIndex];
                if (isHorizontal)
                {
                    childExtent = child.DesiredSize.Width;
                    childRect.X = finalSize.Width * this.RelativeOffset - childExtent * (adjustedOffset - Math.Truncate(adjustedOffset));
                    childRect.Width = childExtent;
                    nextEdge = childRect.X + childExtent;
                    priorEdge = childRect.X;
                    childRect.Height = Math.Max(finalSize.Height, child.DesiredSize.Height);
                }
                else
                {
                    childExtent = child.DesiredSize.Height;
                    childRect.Y = finalSize.Height * this.RelativeOffset - childExtent * (adjustedOffset - Math.Truncate(adjustedOffset));
                    childRect.Height = childExtent;
                    nextEdge = childRect.Y + childExtent;
                    priorEdge = childRect.Y;
                    childRect.Width = Math.Max(finalSize.Width, child.DesiredSize.Width);
                }
                child.Arrange(childRect);
            }

            // arrange subsequent and prior children until we run out of room
            bool isNextFull = false, isPriorFull = false;
            for (int i = 1; i < childCount; i++)
            {
                // for odd iterations, arrange the next child
                // for even iterations, arrange the prior child
                bool isArrangingNext = (i % 2 == 1);

                if (isArrangingNext && isNextFull && !isPriorFull)
                {
                    isArrangingNext = false;
                }
                else if (!isArrangingNext && isPriorFull && !isNextFull)
                {
                    isArrangingNext = true;
                }

                // get the child index and adjust the appropriate counter
                int childIndex = nextIndex;
                if (isArrangingNext)
                {
                    nextIndex = (nextIndex + 1) % childCount;
                }
                else
                {
                    childIndex = priorIndex;
                    priorIndex = (priorIndex > 0) ? priorIndex - 1 : childCount - 1;
                }
                UIElement child = children[childIndex];
                bool childArranged = false;
                if (!(isNextFull && isPriorFull))
                {
                    // determine the child's arrange rect
                    if (isHorizontal)
                    {
                        childExtent = child.DesiredSize.Width;
                        if (isArrangingNext)
                        {
                            childRect.X = nextEdge;
                        }
                        else
                        {
                            childRect.X = priorEdge - childExtent;
                        }
                        childRect.Width = childExtent;
                        childRect.Height = Math.Max(finalSize.Height, child.DesiredSize.Height);
                    }
                    else
                    {
                        childExtent = child.DesiredSize.Height;
                        if (isArrangingNext)
                        {
                            childRect.Y = nextEdge;
                        }
                        else
                        {
                            childRect.Y = priorEdge - childExtent;
                        }
                        childRect.Height = childExtent;
                        childRect.Width = Math.Max(finalSize.Width, child.DesiredSize.Width);
                    }

                    // arrange the child
                    Rect intersection = Rect.Intersect(childRect, controlBounds);
                    if (!intersection.IsEmpty
                        && intersection.Width * intersection.Height > 1.0e-10)
                    {
                        if (isHorizontal)
                        {
                            if (isArrangingNext)
                            {
                                nextEdge = childRect.X + childExtent;
                            }
                            else
                            {
                                priorEdge = childRect.X;
                            }
                        }
                        else
                        {
                            if (isArrangingNext)
                            {
                                nextEdge = childRect.Y + childExtent;
                            }
                            else
                            {
                                priorEdge = childRect.Y;
                            }
                        }
                        child.Arrange(childRect);
                        childArranged = true;
                    }
                    else
                    {
                        // if there's no room for the child, set the appropriate full flag
                        if (isArrangingNext)
                        {
                            isNextFull = true;
                        }
                        else
                        {
                            isPriorFull = true;
                        }
                    }
                }

                // if there was no room for the child, arrange it to a rect with no width or height 
                // as a render optimization; preserve layout placement to ensure that keyboard 
                // navigation works as expected
                if (!childArranged)
                {
                    if (isArrangingNext && isNextFull)
                    {
                        if (isHorizontal)
                        {
                            childRect.X = nextEdge;
                            nextEdge = childRect.X + child.DesiredSize.Width;
                        }
                        else
                        {
                            childRect.Y = nextEdge;
                            nextEdge = childRect.Y + child.DesiredSize.Height;
                        }
                        childRect.Width = 0;
                        childRect.Height = 0;
                        child.Arrange(childRect);
                    }
                    else if (!isArrangingNext && isPriorFull)
                    {
                        if (isHorizontal)
                        {
                            childRect.X = priorEdge - child.DesiredSize.Width;
                            priorEdge = childRect.X;
                        }
                        else
                        {
                            childRect.Y = priorEdge - child.DesiredSize.Height;
                            priorEdge = childRect.Y;
                        }
                        childRect.Width = 0;
                        childRect.Height = 0;
                        child.Arrange(childRect);
                    }
                }
            }
            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Size desiredSize = new Size();
            this._inMeasure = true;
            try
            {
                UIElementCollection children = this.InternalChildren;
                bool isHorizontal = (this.Orientation == Orientation.Horizontal);
                Size childSize = availableSize;

                // children will be measured to their desired size in the logical direction
                if (isHorizontal)
                {
                    childSize.Width = Double.PositiveInfinity;
                }
                else
                {
                    childSize.Height = Double.PositiveInfinity;
                }

                // measure the children
                for (int i = 0, count = children.Count; i < count; ++i)
                {
                    UIElement child = children[i];
                    child.Measure(childSize);

                    Size childDesiredSize = child.DesiredSize;
                    if (isHorizontal)
                    {
                        desiredSize.Width += childDesiredSize.Width;
                        desiredSize.Height = Math.Max(desiredSize.Height, childDesiredSize.Height);
                    }
                    else
                    {
                        desiredSize.Width = Math.Max(desiredSize.Width, childDesiredSize.Width);
                        desiredSize.Height += childDesiredSize.Height;
                    }
                }

                // The available size represents a maximum constraint in the logical direction.  We
                // should never return a size larger than the available size.
                if (isHorizontal)
                {
                    desiredSize.Width = Math.Min(availableSize.Width, desiredSize.Width);
                }
                else
                {
                    desiredSize.Height = Math.Min(availableSize.Height, desiredSize.Height);
                }

                this._isMeasured = true;
            }
            finally
            {
                this._inMeasure = false;
            }
            return desiredSize;
        }

        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            // changes to the children collection require an update to the Offset 
            // so that the children don't shift when the collection changes

            // we do not want to process changes that occur as part of container generation
            if (this._inMeasure || !this._isMeasured) return;

            int childCount = this.InternalChildren.Count;
            if (visualAdded != null)
            {
                double adjustedOffset = this.Offset % childCount;
                if (adjustedOffset < 0)
                {
                    adjustedOffset += childCount;
                }
                int newPivotalChildIndex = (int)adjustedOffset;
                if (newPivotalChildIndex != this.PivotalChildIndex)
                {
                    // add necessary correction to keep Offset the same
                    this.Offset += ((int)this.Offset) / (childCount - 1) + (this.Offset < 0 ? -1 : 0);
                }
            }

            if (visualRemoved != null)
            {
                // add necessary correction to keep Offset the same
                this.Offset -= ((int)this.Offset) / childCount + (this.Offset < 0 ? -1 : 0);
            }

            base.OnVisualChildrenChanged(visualAdded, visualRemoved);
        }

        #endregion

        #region private

        private static void OnRequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            LoopPanel lp = sender as LoopPanel;
            DependencyObject target = e.TargetObject as DependencyObject;
            if (lp.BringChildrenIntoView && target != lp)
            {
                UIElement child = null;
                while (target != null)
                {
                    if ((target is UIElement)
                        && lp.InternalChildren.Contains(target as UIElement))
                    {
                        child = target as UIElement;
                        break;
                    }
                    target = VisualTreeHelper.GetParent(target);
                    if (target == lp) break;
                }

                if (child != null
                    && lp.InternalChildren.Contains(child))
                {
                    e.Handled = true;

                    // determine if the child needs to be brought into view
                    GeneralTransform childTransform = child.TransformToAncestor(lp);
                    Rect childRect = childTransform.TransformBounds(new Rect(new Point(0, 0), child.RenderSize));
                    Rect intersection = Rect.Intersect(new Rect(new Point(0, 0), lp.RenderSize), childRect);

                    // if the intersection is different than the child rect, it is either not visible 
                    // or only partially visible, so adjust the Offset to bring it into view
                    if (!CalculateExtensions.AreVirtuallyEqual(childRect, intersection))
                    {
                        if (!intersection.IsEmpty)
                        {
                            // the child is already partially visible, so just scroll it into view
                            lp.Scroll((lp.Orientation == Orientation.Horizontal)
                                ? (CalculateExtensions.AreVirtuallyEqual(childRect.X, intersection.X) ? childRect.Width - intersection.Width + Math.Min(0, lp.RenderSize.Width - childRect.Width) : childRect.X - intersection.X)
                                : (CalculateExtensions.AreVirtuallyEqual(childRect.Y, intersection.Y) ? childRect.Height - intersection.Height + Math.Min(0, lp.RenderSize.Height - childRect.Height) : childRect.Y - intersection.Y));
                        }
                        else
                        {
                            // the child is not visible at all
                            lp.Scroll((lp.Orientation == Orientation.Horizontal)
                                ? (CalculateExtensions.StrictlyLessThan(childRect.Right, 0.0d) ? childRect.X : childRect.Right - lp.RenderSize.Width + Math.Min(0, lp.RenderSize.Width - childRect.Width))
                                : (CalculateExtensions.StrictlyLessThan(childRect.Bottom, 0.0d) ? childRect.Y : childRect.Bottom - lp.RenderSize.Height + Math.Min(0, lp.RenderSize.Height - childRect.Height)));
                        }
                    }
                }
            }
        }

        #endregion

        #endregion

        #region fields

        private bool _isMeasured = false;
        private bool _inMeasure = false;

        #endregion
    }
}
