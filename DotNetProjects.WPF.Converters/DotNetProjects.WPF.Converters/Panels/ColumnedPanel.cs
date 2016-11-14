using System;
using System.Windows;
using System.Windows.Controls;

namespace DotNetProjects.WPF.Converters.Panels
{
    /// <span class="code-SummaryComment"><summary></span>
    /// A column based layout panel, that automatically
    /// wraps to new column when required. The user
    /// may also create a new column before an element
    /// using the 
    /// from: http://www.codeproject.com/Articles/37348/Creating-Custom-Panels-In-WPF
    /// <span class="code-SummaryComment"></summary></span>
    public class ColumnedPanel : Panel
    {

        #region Ctor

        static ColumnedPanel()
        {
            //tell DP sub system, this DP, will affect
            //Arrange and Measure phases
            FrameworkPropertyMetadata metadata =
                new FrameworkPropertyMetadata();
            metadata.AffectsArrange = true;
            metadata.AffectsMeasure = true;
            ColumnBreakBeforeProperty =
                DependencyProperty.RegisterAttached(
                    "ColumnBreakBefore",
                    typeof (bool), typeof (ColumnedPanel),
                    metadata);
        }

        #endregion

        #region DPs

        /// <span class="code-SummaryComment"><summary></span>
        /// Can be used to create a new column with the ColumnedPanel
        /// just before an element
        /// <span class="code-SummaryComment"></summary></span>
        public static DependencyProperty ColumnBreakBeforeProperty;

        public static void SetColumnBreakBefore(UIElement element,
            Boolean value)
        {
            element.SetValue(ColumnBreakBeforeProperty, value);
        }

        public static Boolean GetColumnBreakBefore(UIElement element)
        {
            return (bool) element.GetValue(ColumnBreakBeforeProperty);
        }

        #endregion

        #region Measure Override

        // From MSDN : When overridden in a derived class, measures the 
        // size in layout required for child elements and determines a
        // size for the FrameworkElement-derived class
        protected override Size MeasureOverride(Size constraint)
        {
            Size currentColumnSize = new Size();
            Size panelSize = new Size();

            foreach (UIElement element in base.InternalChildren)
            {
                element.Measure(constraint);
                Size desiredSize = element.DesiredSize;

                if (GetColumnBreakBefore(element) ||
                    currentColumnSize.Height + desiredSize.Height >
                    constraint.Height)
                {
                    // Switch to a new column (either because the 
                    //element has requested it or space has run out).
                    panelSize.Height = Math.Max(currentColumnSize.Height,
                        panelSize.Height);
                    panelSize.Width += currentColumnSize.Width;
                    currentColumnSize = desiredSize;

                    // If the element is too high to fit using the 
                    // maximum height of the line,
                    // just give it a separate column.
                    if (desiredSize.Height > constraint.Height)
                    {
                        panelSize.Height = Math.Max(desiredSize.Height,
                            panelSize.Height);
                        panelSize.Width += desiredSize.Width;
                        currentColumnSize = new Size();
                    }
                }
                else
                {
                    // Keep adding to the current column.
                    currentColumnSize.Height += desiredSize.Height;

                    // Make sure the line is as wide as its widest element.
                    currentColumnSize.Width =
                        Math.Max(desiredSize.Width,
                            currentColumnSize.Width);
                }
            }

            // Return the size required to fit all elements.
            // Ordinarily, this is the width of the constraint, 
            // and the height is based on the size of the elements.
            // However, if an element is higher than the height given
            // to the panel,
            // the desired width will be the height of that column.
            panelSize.Height = Math.Max(currentColumnSize.Height,
                panelSize.Height);
            panelSize.Width += currentColumnSize.Width;
            return panelSize;

        }

        #endregion

        #region Arrange Override

        //From MSDN : When overridden in a derived class, positions child
        //elements and determines a size for a FrameworkElement derived
        //class.

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            int firstInLine = 0;

            Size currentColumnSize = new Size();

            double accumulatedWidth = 0;

            UIElementCollection elements = base.InternalChildren;
            for (int i = 0; i < elements.Count; i++)
            {

                Size desiredSize = elements[i].DesiredSize;

                //need to switch to another column
                if (GetColumnBreakBefore(elements[i]) ||
                    currentColumnSize.Height +
                    desiredSize.Height >
                    arrangeBounds.Height)
                {
                    arrangeColumn(accumulatedWidth,
                        currentColumnSize.Width,
                        firstInLine, i, arrangeBounds);

                    accumulatedWidth += currentColumnSize.Width;
                    currentColumnSize = desiredSize;

                    //the element is higher then the constraint - 
                    //give it a separate column 
                    if (desiredSize.Height > arrangeBounds.Height)
                    {
                        arrangeColumn(accumulatedWidth,
                            desiredSize.Width, i, ++i, arrangeBounds);
                        accumulatedWidth += desiredSize.Width;
                        currentColumnSize = new Size();
                    }
                    firstInLine = i;
                }
                else //continue to accumulate a column
                {
                    currentColumnSize.Height += desiredSize.Height;
                    currentColumnSize.Width =
                        Math.Max(desiredSize.Width,
                            currentColumnSize.Width);
                }
            }

            if (firstInLine < elements.Count)
                arrangeColumn(accumulatedWidth,
                    currentColumnSize.Width,
                    firstInLine, elements.Count,
                    arrangeBounds);

            return arrangeBounds;
        }

        #endregion

        #region Private Methods

        /// <span class="code-SummaryComment"><summary></span>
        /// Arranges a single column of elements
        /// <span class="code-SummaryComment"></summary></span>
        private void arrangeColumn(double x,
            double columnWidth, int start,
            int end, Size arrangeBounds)
        {
            double y = 0;
            double totalChildHeight = 0;
            double widestChildWidth = 0;
            double xOffset = 0;

            UIElementCollection children = InternalChildren;
            UIElement child;

            for (int i = start; i < end; i++)
            {
                child = children[i];
                totalChildHeight += child.DesiredSize.Height;
                if (child.DesiredSize.Width > widestChildWidth)
                    widestChildWidth = child.DesiredSize.Width;
            }

            //work out y start offset within a given column
            y = ((arrangeBounds.Height - totalChildHeight)/2);


            for (int i = start; i < end; i++)
            {
                child = children[i];
                if (child.DesiredSize.Width < widestChildWidth)
                {
                    xOffset = ((widestChildWidth -
                                child.DesiredSize.Width)/2);
                }

                child.Arrange(new Rect(x + xOffset, y,
                    child.DesiredSize.Width, columnWidth));
                y += child.DesiredSize.Height;
                xOffset = 0;
            }
        }

        #endregion

    }
}
