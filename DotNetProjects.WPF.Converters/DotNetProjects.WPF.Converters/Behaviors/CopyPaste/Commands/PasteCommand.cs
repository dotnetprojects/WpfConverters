using System.Security;
using System.Windows;
using System.Windows.Controls;

namespace DotNetProjects.WPF.Converters.Behaviors.CopyPaste.Commands
{
	/// <summary>
	/// Command for TextBox's Context Menu. 
	/// Do paste text from clipboard to selected text. 
	/// Can't be executed if clipboard doesn't contains any text.
	/// </summary>
	internal class PasteCommand : ClipboardCommandBase
	{
        private TextBox TextBox;

		public PasteCommand(TextBox textBox, IClipboardSecurityExceptionNotify clipboardSecurityExceptionNotify)
			: base(textBox, clipboardSecurityExceptionNotify)
		{
		    this.TextBox = textBox;
		}

		public override void Execute()
		{
			try
			{
				this.TextBox.SelectedText = Clipboard.GetText();
			}
			catch (SecurityException ex)
			{
				this.OnClipboardSecurityException(ex, StdCommandActionType.Paste);
			}
			this.TextBox.Focus();
		}

		public override bool CanExecute()
		{
			return this.TextBox.IsEnabled && !this.TextBox.IsReadOnly && Clipboard.ContainsText();
		}
	}
}