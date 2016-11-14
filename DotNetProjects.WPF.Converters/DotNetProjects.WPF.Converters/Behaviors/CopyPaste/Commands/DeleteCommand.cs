using System.Windows.Controls;

namespace DotNetProjects.WPF.Converters.Behaviors.CopyPaste.Commands
{
	/// <summary>
	/// Command for TextBox's Context Menu. 
	/// Do delete of selected text. 
	/// Can't be executed if not text is selected.
	/// </summary>
	internal class DeleteCommand : CommandBase
	{
        private TextBox TextBox;

		public DeleteCommand(TextBox textBox) : base(textBox)
		{
		    this.TextBox = textBox;
		}

		public override void Execute()
		{
			this.TextBox.SelectedText = string.Empty;
			this.TextBox.Focus();
		}

		public override bool CanExecute()
		{
			return this.TextBox.IsEnabled && !this.TextBox.IsReadOnly && this.TextBox.SelectionLength > 0;
		}
	}
}