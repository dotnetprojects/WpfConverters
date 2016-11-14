using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace DotNetProjects.WPF.Converters.Behaviors
{
    /// <summary>
    /// An attached behavior that allows forces all input to a TextBox to be numeric
    /// </summary>
    public class NumericTextBoxBehavior : Behavior<TextBox>, ICloneableBehavior
    {

        /// <summary>
        /// Called when attached to a TextBox.
        /// </summary>
        protected override void OnAttached()
        {
            this.AssociatedObject.PreviewTextInput += OnPreviewTextInput;
            DataObject.AddPastingHandler(this.AssociatedObject, OnClipboardPaste);
        }

        /// <summary>
        /// This method handles paste and drag/drop events onto the TextBox.  It restricts the character
        /// set and ensures we have consistent behavior.
        /// </summary>
        /// <param name="sender">TextBox sender</param>
        /// <param name="e">EventArgs</param>
        private void OnClipboardPaste(object sender, DataObjectPastingEventArgs e)
        {
            string text = e.SourceDataObject.GetData(e.FormatToApply) as string;

            if (!string.IsNullOrEmpty(text) && !Validate(text))
                e.CancelCommand();
        }

        /// <summary>
        /// This checks if the resulting string will match the regex expression
        /// </summary>
        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Validate(e.Text))
                e.Handled = true;
        }


        private bool Validate(string newContent)
        {
            string testString = string.Empty;

            // replace selection with new text.
            if (!string.IsNullOrEmpty(this.AssociatedObject.SelectedText))
            {
                string pre = this.AssociatedObject.Text.Substring(0, this.AssociatedObject.SelectionStart);
                string after = this.AssociatedObject.Text.Substring(this.AssociatedObject.SelectionStart + this.AssociatedObject.SelectionLength, this.AssociatedObject.Text.Length - (this.AssociatedObject.SelectionStart + this.AssociatedObject.SelectionLength));
                testString = pre + newContent + after;
            }
            else
            {
                string pre = this.AssociatedObject.Text.Substring(0, this.AssociatedObject.CaretIndex);
                string after = this.AssociatedObject.Text.Substring(this.AssociatedObject.CaretIndex, this.AssociatedObject.Text.Length - this.AssociatedObject.CaretIndex);
                testString = pre + newContent + after;
            }

            Regex regExpr = new Regex(@"^([-+]?)(\d*)([,.]?)(\d*)$");
            if (regExpr.IsMatch(testString))
                return true;

            return false;
        }

        public object CloneBehavior()
        {
            return new NumericTextBoxBehavior();
        }
    }
}
