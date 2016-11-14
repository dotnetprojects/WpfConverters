using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace DotNetProjects.WPF.Converters.Panels
{
    public class VirtualizingTilePanel : VirtualizingPanel, IScrollInfo
    {
        public VirtualizingTilePanel()
        {
            // For use in the IScrollInfo implementation
            this.RenderTransform = this._trans;
        }

        // Dependency property that controls the size of the child elements
        public static readonly DependencyProperty ChildSizeProperty
           = DependencyProperty.RegisterAttached("ChildSize", typeof(double), typeof(VirtualizingTilePanel),
              new FrameworkPropertyMetadata(200.0d, FrameworkPropertyMetadataOptions.AffectsMeasure |
              FrameworkPropertyMetadataOptions.AffectsArrange));

        // Accessor for the child size dependency property
        public double ChildSize
        {
            get { return (double)this.GetValue(ChildSizeProperty); }
            set { this.SetValue(ChildSizeProperty, value); }
        }

        /// <summary>
        /// Measure the children
        /// </summary>
        /// <param name="availableSize">Size available</param>
        /// <returns>Size desired</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            this.UpdateScrollInfo(availableSize);

            // Figure out range that's visible based on layout algorithm
            int firstVisibleItemIndex, lastVisibleItemIndex;
            this.GetVisibleRange(out firstVisibleItemIndex, out lastVisibleItemIndex);

            // We need to access InternalChildren before the generator to work around a bug
            UIElementCollection children = this.InternalChildren;
            IItemContainerGenerator generator = this.ItemContainerGenerator;

            // Get the generator position of the first visible data item
            GeneratorPosition startPos = generator.GeneratorPositionFromIndex(firstVisibleItemIndex);

            // Get index where we'd insert the child for this position. If the item is realized
            // (position.Offset == 0), it's just position.Index, otherwise we have to add one to
            // insert after the corresponding child
            int childIndex = (startPos.Offset == 0) ? startPos.Index : startPos.Index + 1;

            using (generator.StartAt(startPos, GeneratorDirection.Forward, true))
            {
                for (int itemIndex = firstVisibleItemIndex; itemIndex <= lastVisibleItemIndex; ++itemIndex, ++childIndex)
                {
                    bool newlyRealized;

                    // Get or create the child
                    UIElement child = generator.GenerateNext(out newlyRealized) as UIElement;
                    if (newlyRealized)
                    {
                        // Figure out if we need to insert the child at the end or somewhere in the middle
                        if (childIndex >= children.Count)
                        {
                            base.AddInternalChild(child);
                        }
                        else
                        {
                            base.InsertInternalChild(childIndex, child);
                        }
                        generator.PrepareItemContainer(child);
                    }
                    else
                    {
                        // The child has already been created, let's be sure it's in the right spot
                        Debug.Assert(child == children[childIndex], "Wrong child was generated");
                    }

                    // Measurements will depend on layout algorithm
                    child.Measure(this.GetChildSize());
                }
            }

            // Note: this could be deferred to idle time for efficiency
            this.CleanUpItems(firstVisibleItemIndex, lastVisibleItemIndex);

            return availableSize;
        }

        /// <summary>
        /// Arrange the children
        /// </summary>
        /// <param name="finalSize">Size available</param>
        /// <returns>Size used</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            IItemContainerGenerator generator = this.ItemContainerGenerator;

            this.UpdateScrollInfo(finalSize);

            for (int i = 0; i < this.Children.Count; i++)
            {
                UIElement child = this.Children[i];

                // Map the child offset to an item offset
                int itemIndex = generator.IndexFromGeneratorPosition(new GeneratorPosition(i, 0));

                this.ArrangeChild(itemIndex, child, finalSize);
            }

            return finalSize;
        }

        /// <summary>
        /// Revirtualize items that are no longer visible
        /// </summary>
        /// <param name="minDesiredGenerated">first item index that should be visible</param>
        /// <param name="maxDesiredGenerated">last item index that should be visible</param>
        private void CleanUpItems(int minDesiredGenerated, int maxDesiredGenerated)
        {
            UIElementCollection children = this.InternalChildren;
            IItemContainerGenerator generator = this.ItemContainerGenerator;

            for (int i = children.Count - 1; i >= 0; i--)
            {
                GeneratorPosition childGeneratorPos = new GeneratorPosition(i, 0);
                int itemIndex = generator.IndexFromGeneratorPosition(childGeneratorPos);
                if (itemIndex < minDesiredGenerated || itemIndex > maxDesiredGenerated)
                {
                    generator.Remove(childGeneratorPos, 1);
                    this.RemoveInternalChildRange(i, 1);
                }
            }
        }

        /// <summary>
        /// When items are removed, remove the corresponding UI if necessary
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected override void OnItemsChanged(object sender, ItemsChangedEventArgs args)
        {
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Move:
                    this.RemoveInternalChildRange(args.Position.Index, args.ItemUICount);
                    break;
            }
        }

        #region Layout specific code
        // I've isolated the layout specific code to this region. If you want to do something other than tiling, this is
        // where you'll make your changes

        /// <summary>
        /// Calculate the extent of the view based on the available size
        /// </summary>
        /// <param name="availableSize">available size</param>
        /// <param name="itemCount">number of data items</param>
        /// <returns></returns>
        private Size CalculateExtent(Size availableSize, int itemCount)
        {
            int childrenPerRow = this.CalculateChildrenPerRow(availableSize);

            // See how big we are
            return new Size(childrenPerRow * this.ChildSize,
                this.ChildSize * Math.Ceiling((double)itemCount / childrenPerRow));
        }

        /// <summary>
        /// Get the range of children that are visible
        /// </summary>
        /// <param name="firstVisibleItemIndex">The item index of the first visible item</param>
        /// <param name="lastVisibleItemIndex">The item index of the last visible item</param>
        private void GetVisibleRange(out int firstVisibleItemIndex, out int lastVisibleItemIndex)
        {
            int childrenPerRow = this.CalculateChildrenPerRow(this._extent);

            firstVisibleItemIndex = (int) Math.Floor(this._offset.Y / this.ChildSize) * childrenPerRow;
            lastVisibleItemIndex = (int) Math.Ceiling((this._offset.Y + this._viewport.Height) / this.ChildSize) * childrenPerRow - 1;

            ItemsControl itemsControl = ItemsControl.GetItemsOwner(this);
            int itemCount = itemsControl.HasItems ? itemsControl.Items.Count : 0;
            if (lastVisibleItemIndex >= itemCount)
                lastVisibleItemIndex = itemCount-1;
            
        }

        /// <summary>
        /// Get the size of the children. We assume they are all the same
        /// </summary>
        /// <returns>The size</returns>
        private Size GetChildSize()
        {
            return new Size(this.ChildSize, this.ChildSize);
        }

        /// <summary>
        /// Position a child
        /// </summary>
        /// <param name="itemIndex">The data item index of the child</param>
        /// <param name="child">The element to position</param>
        /// <param name="finalSize">The size of the panel</param>
        private void ArrangeChild(int itemIndex, UIElement child, Size finalSize)
        {
            int childrenPerRow = this.CalculateChildrenPerRow(finalSize);

            int row = itemIndex / childrenPerRow;
            int column = itemIndex % childrenPerRow;

            child.Arrange(new Rect(column * this.ChildSize, row * this.ChildSize, this.ChildSize, this.ChildSize));
        }

        /// <summary>
        /// Helper function for tiling layout
        /// </summary>
        /// <param name="availableSize">Size available</param>
        /// <returns></returns>
        private int CalculateChildrenPerRow(Size availableSize)
        {
            // Figure out how many children fit on each row
            int childrenPerRow;
            if (availableSize.Width == Double.PositiveInfinity)
                childrenPerRow = this.Children.Count;
            else
                childrenPerRow = Math.Max(1, (int)Math.Floor(availableSize.Width / this.ChildSize));
            return childrenPerRow;
        }

        #endregion

        #region IScrollInfo implementation
        // See Ben Constable's series of posts at http://blogs.msdn.com/bencon/


        private void UpdateScrollInfo(Size availableSize)
        {
            // See how many items there are
            ItemsControl itemsControl = ItemsControl.GetItemsOwner(this);
            int itemCount = itemsControl.HasItems ? itemsControl.Items.Count : 0;

            Size extent = this.CalculateExtent(availableSize, itemCount);
            // Update extent
            if (extent != this._extent)
            {
                this._extent = extent;
                if (this._owner != null)
                    this._owner.InvalidateScrollInfo();
            }

            // Update viewport
            if (availableSize != this._viewport)
            {
                this._viewport = availableSize;
                if (this._owner != null)
                    this._owner.InvalidateScrollInfo();
            }
        }

        public ScrollViewer ScrollOwner
        {
            get { return this._owner; }
            set { this._owner = value; }
        }

        public bool CanHorizontallyScroll
        {
            get { return this._canHScroll; }
            set { this._canHScroll = value; }
        }

        public bool CanVerticallyScroll
        {
            get { return this._canVScroll; }
            set { this._canVScroll = value; }
        }

        public double HorizontalOffset
        {
            get { return this._offset.X; }
        }

        public double VerticalOffset
        {
            get { return this._offset.Y; }
        }

        public double ExtentHeight
        {
            get { return this._extent.Height; }
        }

        public double ExtentWidth
        {
            get { return this._extent.Width; }
        }

        public double ViewportHeight
        {
            get { return this._viewport.Height; }
        }

        public double ViewportWidth
        {
            get { return this._viewport.Width; }
        }

        public void LineUp()
        {
            this.SetVerticalOffset(this.VerticalOffset - 10);
        }

        public void LineDown()
        {
            this.SetVerticalOffset(this.VerticalOffset + 10);
        }

        public void PageUp()
        {
            this.SetVerticalOffset(this.VerticalOffset - this._viewport.Height);
        }

        public void PageDown()
        {
            this.SetVerticalOffset(this.VerticalOffset + this._viewport.Height);
        }

        public void MouseWheelUp()
        {
            this.SetVerticalOffset(this.VerticalOffset - 10);
        }

        public void MouseWheelDown()
        {
            this.SetVerticalOffset(this.VerticalOffset + 10);
        }

        public void LineLeft()
        {
            throw new InvalidOperationException();
        }

        public void LineRight()
        {
            throw new InvalidOperationException();
        }

        public Rect MakeVisible(Visual visual, Rect rectangle)
        {
            return new Rect();
        }

        public void MouseWheelLeft()
        {
            throw new InvalidOperationException();
        }

        public void MouseWheelRight()
        {
            throw new InvalidOperationException();
        }

        public void PageLeft()
        {
            throw new InvalidOperationException();
        }

        public void PageRight()
        {
            throw new InvalidOperationException();
        }

        public void SetHorizontalOffset(double offset)
        {
            throw new InvalidOperationException();
        }

        public void SetVerticalOffset(double offset)
        {
            if (offset < 0 || this._viewport.Height >= this._extent.Height)
            {
                offset = 0;
            }
            else
            {
                if (offset + this._viewport.Height >= this._extent.Height)
                {
                    offset = this._extent.Height - this._viewport.Height;
                }
            }

            this._offset.Y = offset;

            if (this._owner != null)
                this._owner.InvalidateScrollInfo();

            this._trans.Y = -offset;

            // Force us to realize the correct children
            this.InvalidateMeasure();
        }

        private TranslateTransform _trans = new TranslateTransform();
        private ScrollViewer _owner;
        private bool _canHScroll = false;
        private bool _canVScroll = false;
        private Size _extent = new Size(0, 0);
        private Size _viewport = new Size(0, 0);
        private Point _offset;

        #endregion

    }
}
