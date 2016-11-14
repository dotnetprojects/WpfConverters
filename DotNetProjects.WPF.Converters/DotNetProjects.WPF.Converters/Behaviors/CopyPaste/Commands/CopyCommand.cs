using System.Security;
using System.Windows;
using System.Windows.Controls;

namespace DotNetProjects.WPF.Converters.Behaviors.CopyPaste.Commands
{
	/// <summary>
	/// Command for TextBox's Context Menu. 
	/// Do copy of selected text. 
	/// Can't be executed if not text is selected.
	/// </summary>
	internal class CopyCommand : ClipboardCommandBase
	{
        public CopyCommand(UIElement UIElement, IClipboardSecurityExceptionNotify clipboardSecurityExceptionNotify)
            : base(UIElement, clipboardSecurityExceptionNotify)
		{
		}

		public override void Execute()
		{
			try
			{
			    if (this.UIElement is TextBox)
			    {
			        var textBox = this.UIElement as TextBox;
			        if (textBox.SelectionLength == 0) 
                        Clipboard.SetText(textBox.Text);
			        else 
                        Clipboard.SetText(textBox.SelectedText);

                    textBox.Focus();
			    }
			    else
			    {
                    Clipboard.SetText(this.getText());
			    }
			}
			catch (SecurityException ex)
			{
				this.OnClipboardSecurityException(ex, StdCommandActionType.Copy);
			}
			
		}

		public override bool CanExecute()
		{
            return !string.IsNullOrEmpty(this.getText());
		}
	}
}