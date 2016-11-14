using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DotNetProjects.WPF.Converters.Behaviors.CopyPaste.Commands
{
	/// <summary>
	/// Abstract class with base code for all textbox's standard commands.
	/// </summary>
	internal abstract class CommandBase : ICommand, IDisposable
	{
        internal string getText()
        {
            if (this.UIElement is TextBox) return ((TextBox)this.UIElement).Text;
            if (this.UIElement is TextBlock) return ((TextBlock)this.UIElement).Text;
            if (this.UIElement is ContentControl)
            {
                var cnt = ((ContentControl)this.UIElement).Content;
                if (cnt != null && cnt is TextBlock)
                    return ((TextBlock)cnt).Text.ToString();
                else if (cnt != null && cnt is TextBox)
                    return ((TextBox)cnt).Text.ToString();
                return ((ContentControl)this.UIElement).Content.ToString();
            }
            return null;
        }

		private bool _isDisposed = false;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="textBox">Associated object. Cannot be null.</param>
        protected CommandBase(UIElement UIElement)
		{
            if (UIElement == null)
                throw new ArgumentNullException("UIElement");

            this.UIElement = UIElement;
		}

		~CommandBase()
		{
			this.Dispose(false);
		}

		/// <summary>
		/// Associated TextBox
		/// </summary>
        protected UIElement UIElement { get; private set; }

		/// <summary>
		/// Is current command can be executed
		/// </summary>
		/// <param name="parameter">Not used.</param>
		/// <returns></returns>
		bool ICommand.CanExecute(object parameter)
		{
			return this.CanExecute();
		}

		/// <summary>
		/// Execute current command. 
		/// Check before execution that current command can be executed with <see cref="CanExecute"/> method.
		/// </summary>
		/// <param name="parameter">Not used.</param>
		void ICommand.Execute(object parameter)
		{
			if (this.CanExecute())
				this.Execute();
		}

		/// <summary>
		/// If current command can be executed. 
		/// Default realization return true.
		/// </summary>
		/// <returns></returns>
		public virtual bool CanExecute()
		{
			return true;
		}

		/// <summary>
		/// Abstract method to executed current command.
		/// </summary>
		public abstract void Execute();

		/// <summary>
		/// Subscribe to handle state change of CanExecute
		/// </summary>
		public event EventHandler CanExecuteChanged;

		public void RaiseCanExecuteChanged()
		{
			EventHandler handler = this.CanExecuteChanged;
			if (handler != null)
				handler(this, EventArgs.Empty);
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool isDisposing)
		{
			if (!this._isDisposed)
			{
				if (isDisposing)
				{
					this.OnDisposing();
					this.CanExecuteChanged = null;
                    this.UIElement = null;
				}
				this._isDisposed = true;
			}
		}

		protected virtual void OnDisposing()
		{
		}
	}
}