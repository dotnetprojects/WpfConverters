using System;
using System.Security;
using System.Windows;

namespace DotNetProjects.WPF.Converters.Behaviors.CopyPaste.Commands
{
	/// <summary>
	/// Abstract class with base code for all textbox's standard commands, which uses clipboard.
	/// </summary>
	internal abstract class ClipboardCommandBase : CommandBase
	{
		private IClipboardSecurityExceptionNotify _clipboardSecurityExceptionNotify;

        protected ClipboardCommandBase(UIElement UIElement, IClipboardSecurityExceptionNotify clipboardSecurityExceptionNotify)
            : base(UIElement)
		{
			if (clipboardSecurityExceptionNotify == null)
				throw new ArgumentNullException("clipboardSecurityExceptionNotify");

			this._clipboardSecurityExceptionNotify = clipboardSecurityExceptionNotify;
		}

		/// <summary>
		/// Method for handle SecurityException with clipboard.
		/// </summary>
		/// <param name="exception"></param>
		/// <param name="actionType"></param>
		protected void OnClipboardSecurityException(SecurityException exception, StdCommandActionType actionType)
		{
			this._clipboardSecurityExceptionNotify.OnClipboardSecurityExceptionEvent(
				new ClipboardSecurityExceptionEventArgs(exception, actionType));
		}

        protected override void OnDisposing()
        {
            this._clipboardSecurityExceptionNotify = null;
            base.OnDisposing();

        }
	}
}