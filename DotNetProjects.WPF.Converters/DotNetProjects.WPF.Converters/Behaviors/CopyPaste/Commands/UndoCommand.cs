using System.Windows.Controls;

namespace DotNetProjects.WPF.Converters.Behaviors.CopyPaste.Commands
{
	/// <summary>
	/// Command for TextBox's Context Menu. 
	/// Do undo of text last operation. 
	/// Can't be executed if text didn't changed.
	/// </summary>
	internal class UndoCommand : CommandBase
	{
        private TextBox TextBox;

		private string _oldText;
		private string _currentText;

		public UndoCommand(TextBox textBox)
			:base(textBox)
		{
		    this.TextBox = textBox;
			this.TextBox.TextChanged += this.TextBoxTextChanged;
			this._currentText = this.TextBox.Text;
		}

		private void TextBoxTextChanged(object sender, TextChangedEventArgs e)
		{
			this._oldText = this._currentText;
			this._currentText = this.TextBox.Text;
		}

		public override bool CanExecute()
		{
			// Null state - text is not changed. 
			// If somebody change text to empty then it will be string.Empty, but not null.
			return this.TextBox.IsEnabled && !this.TextBox.IsReadOnly && this._oldText != null;
		}

		public override void Execute()
		{
			this.TextBox.Text = this._oldText;
			this.TextBox.Focus();
		}

		protected override void OnDisposing()
		{
			base.OnDisposing();
			this.TextBox.TextChanged -= this.TextBoxTextChanged;
		}
	}
}