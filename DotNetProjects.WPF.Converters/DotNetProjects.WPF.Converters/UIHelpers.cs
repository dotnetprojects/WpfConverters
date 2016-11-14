using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Xps.Serialization;

namespace DotNetProjects.WPF.Converters
{
    public static class UIHelpers
    {
        /// <summary>
        /// Method to call to get the Visual Tree of a Visual element.
        /// </summary>
        /// <param name="element">The Visual element that yo want to browse to have its Visual Tree.</param>
        /// <returns>A StringBuilder object that contains the Visual Tree of the specified Visual element.</returns>
        public static string GetVisualTreeInfo(Visual element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(String.Format("Element {0} is null !", element.ToString()));
            }

            var sb = new StringBuilder();

            GetControlsList(element, 0, sb);

            return sb.ToString();
        }

        private static void GetControlsList(Visual control, int level, StringBuilder sb)
        {
            const int indent = 4;
            int ChildNumber = VisualTreeHelper.GetChildrenCount(control);

            for (int i = 0; i <= ChildNumber - 1; i++)
            {
                Visual v = VisualTreeHelper.GetChild(control, i) as Visual;

                if (v != null)
                {
                    sb.Append(new string(' ', level * indent));
                    sb.Append(v.GetType());
                    sb.Append(Environment.NewLine);

                    if (VisualTreeHelper.GetChildrenCount(v) > 0)
                    {
                        GetControlsList(v, level + 1, sb);
                    }
                }
                else
                {
                    throw new ArgumentNullException(String.Format("Element {0} is null !", v.ToString()));
                }
            }
        }

        /// <summary>
        /// Render a UIElement such that the visual tree is generated, 
        /// without actually displaying the UIElement
        /// anywhere
        /// </summary>
        public static void CreateVisualTree(this UIElement element)
        {
            var fixedDoc = new FixedDocument();
            var pageContent = new PageContent();
            var fixedPage = new FixedPage();
            fixedPage.Children.Add(element);
            (pageContent as IAddChild).AddChild(fixedPage);
            fixedDoc.Pages.Add(pageContent);

            var f = new XpsSerializerFactory();
            var w = f.CreateSerializerWriter(new MemoryStream());
            w.Write(fixedDoc);
        }

        /// <summary>
        /// Finds a parent of a given item on the visual tree.
        /// </summary>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="child">A direct or indirect child of the
        /// queried item.</param>
        /// <returns>The first parent item that matches the submitted
        /// type parameter. If not matching item can be found, a null
        /// reference is being returned.</returns>
        public static T TryFindParent<T>(this DependencyObject child)
            where T : DependencyObject
        {
            //get parent item
            DependencyObject parentObject = GetParentObject(child);

            //we've reached the end of the tree
            if (parentObject == null) return null;

            //check if the parent matches the type we're looking for
            T parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                //use recursion to proceed with next level
                return TryFindParent<T>(parentObject);
            }
        }

        /// <summary>
        /// Finds a child of the given item on the visual tree.
        /// </summary>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="parent">A parent of the queried item.</param>
        /// <returns>The first child item that matches the submitted
        /// type parameter. If not matching item can be found, a null
        /// reference is being returned.</returns>
        public static T TryFindChild<T>(this DependencyObject parent)
            where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                if (child is T)
                {
                    return (T)child;
                }
                else
                {
                    child = TryFindChild<T>(child);
                    if (child != null)
                    {
                        return (T)child;
                    }
                }
            }
            return null;
        } 

        /// <summary>
        /// This method is an alternative to WPF's
        /// <see cref="VisualTreeHelper.GetParent"/> method, which also
        /// supports content elements. Keep in mind that for content element,
        /// this method falls back to the logical tree of the element!
        /// </summary>
        /// <param name="child">The item to be processed.</param>
        /// <returns>The submitted item's parent, if available. Otherwise
        /// null.</returns>
        public static DependencyObject GetParentObject(this DependencyObject child)
        {
            if (child == null) return null;

            //handle content elements separately
            ContentElement contentElement = child as ContentElement;
            if (contentElement != null)
            {
                DependencyObject parent = ContentOperations.GetParent(contentElement);
                if (parent != null) return parent;

                FrameworkContentElement fce = contentElement as FrameworkContentElement;
                return fce != null ? fce.Parent : null;
            }

            //also try searching for parent in framework elements (such as DockPanel, etc)
            FrameworkElement frameworkElement = child as FrameworkElement;
            if (frameworkElement != null)
            {
                DependencyObject parent = frameworkElement.Parent;
                if (parent != null) return parent;
            }

            //if it's not a ContentElement/FrameworkElement, rely on VisualTreeHelper
            return VisualTreeHelper.GetParent(child);
        }

        /// <summary>
        /// Tries to locate a given item within the visual tree,
        /// starting with the dependency object at a given position. 
        /// </summary>
        /// <typeparam name="T">The type of the element to be found
        /// on the visual tree of the element at the given location.</typeparam>
        /// <param name="reference">The main element which is used to perform
        /// hit testing.</param>
        /// <param name="point">The position to be evaluated on the origin.</param>
        public static T TryFindFromPoint<T>(UIElement reference, Point point) where T : DependencyObject
        {
            DependencyObject element = reference.InputHitTest(point) as DependencyObject;
            if (element == null) return null;
            else if (element is T) return (T)element;
            else return TryFindParent<T>(element);
        }


        public enum RelativeVerticalMousePosition
        {
            Middle,
            Top,
            Bottom
        }

        public static RelativeVerticalMousePosition GetRelativeVerticalMousePosition(FrameworkElement elm, Point pt)
        {
            if (pt.Y > 0.0 && pt.Y < 25)
            {
                return RelativeVerticalMousePosition.Top;
            }
            else if (pt.Y > elm.ActualHeight - 25 && pt.Y < elm.ActualHeight)
            {
                return RelativeVerticalMousePosition.Top;
            }
            return RelativeVerticalMousePosition.Middle;
        }

        public static object GetItemFromPointInItemsControl(ItemsControl parent, Point p)
        {
            UIElement element = parent.InputHitTest(p) as UIElement;
            while (element != null)
            {
                if (element == parent)
                    return null;

                object data = parent.ItemContainerGenerator.ItemFromContainer(element);
                if (data != DependencyProperty.UnsetValue)
                {
                    return data;
                }
                else
                {
                    element = VisualTreeHelper.GetParent(element) as UIElement;
                }
            }
            return null;
        }

        public static UIElement GetItemContainerFromPointInItemsControl(ItemsControl parent, Point p)
        {
            UIElement element = parent.InputHitTest(p) as UIElement;
            while (element != null)
            {
                if (element == parent)
                    return null;

                object data = parent.ItemContainerGenerator.ItemFromContainer(element);
                if (data != DependencyProperty.UnsetValue)
                {
                    return element;
                }
                else
                {
                    element = VisualTreeHelper.GetParent(element) as UIElement;
                }
            }
            return null;
        }

        public static T GetVisualDescendent<T>(DependencyObject d) where T : DependencyObject
        {
            return GetVisualDescendents<T>(d).FirstOrDefault();
        }

        public static IEnumerable<T> GetVisualDescendents<T>(DependencyObject d) where T : DependencyObject
        {
            for (int n = 0; n < VisualTreeHelper.GetChildrenCount(d); n++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(d, n);

                if (child is T)
                {
                    yield return (T)child;
                }

                foreach (T match in GetVisualDescendents<T>(child))
                {
                    yield return match;
                }
            }

            yield break;
        }
    }
}
