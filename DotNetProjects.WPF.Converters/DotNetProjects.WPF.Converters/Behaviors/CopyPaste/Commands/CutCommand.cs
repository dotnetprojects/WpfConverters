using System.Security;
using System.Windows;
using System.Windows.Controls;

namespace DotNetProjects.WPF.Converters.Behaviors.CopyPaste.Commands
{
	/// <summary>
	/// Command for TextBox's Context Menu. 
	/// Do cut of selected text. 
	/// Can't be executed if not text is selected.
	/// </summary>
	internal class CutCommand : ClipboardCommandBase
	{
	    private TextBox TextBox;

		public CutCommand(TextBox textBox, IClipboardSecurityExceptionNotify clipboardSecurityExceptionNotify)
			: base(textBox, clipboardSecurityExceptionNotify)
		{
		    this.TextBox = textBox;
		}

		public override void Execute()
		{
			try
			{
				Clipboard.SetText(this.TextBox.SelectedText);
			}
			catch (SecurityException ex)
			{
				this.OnClipboardSecurityException(ex, StdCommandActionType.Cut);
			}
			this.TextBox.SelectedText = string.Empty;
			this.TextBox.Focus();
		}

		public override bool CanExecute()
		{
			return this.TextBox.IsEnabled && !this.TextBox.IsReadOnly && this.TextBox.SelectionLength > 0;
		}
	}
}