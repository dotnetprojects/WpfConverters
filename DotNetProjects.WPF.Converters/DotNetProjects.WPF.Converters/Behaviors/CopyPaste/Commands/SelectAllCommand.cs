using System.Windows.Controls;

namespace DotNetProjects.WPF.Converters.Behaviors.CopyPaste.Commands
{
	/// <summary>
	/// Command for TextBox's Context Menu. 
	/// Do select All Text in TextBox. 
	/// Can't be executed if all text is already selected or text is empty.
	/// </summary>
	internal class SelectAllCommand : CommandBase
	{
        private TextBox TextBox;

		public SelectAllCommand(TextBox textBox) : base(textBox)
		{
		    this.TextBox = textBox;
		}

		public override void Execute()
		{
			this.TextBox.SelectAll();
			this.TextBox.Focus();
		}

		public override bool CanExecute()
		{
			return !string.IsNullOrEmpty(this.TextBox.Text) &&
			       this.TextBox.Text.Length != this.TextBox.SelectedText.Length;
		}
	}
}