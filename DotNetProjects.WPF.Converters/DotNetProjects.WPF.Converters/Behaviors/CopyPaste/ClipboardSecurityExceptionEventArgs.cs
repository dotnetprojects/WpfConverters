using System.Security;
using System.Windows;

namespace DotNetProjects.WPF.Converters.Behaviors.CopyPaste
{
	public class ClipboardSecurityExceptionEventArgs : RoutedEventArgs
	{
		/// <summary>
		/// Exception thrown by system when application tried to get access to clipboard.
		/// </summary>
		public SecurityException Exception { get; private set; }

		/// <summary>
		/// Action type on which exception was thrown.
		/// </summary>
		public StdCommandActionType StdCommandActionType { get; private set; }

		public ClipboardSecurityExceptionEventArgs(SecurityException exception, 
												   StdCommandActionType stdCommandActionType)
		{
			this.Exception = exception;
			this.StdCommandActionType = stdCommandActionType;
		}
	}
}