namespace DotNetProjects.WPF.Converters.Behaviors.CopyPaste
{
	/// <summary>
	/// Interface for handle security exceptions with clipboard.
	/// </summary>
	internal interface IClipboardSecurityExceptionNotify
	{
		void OnClipboardSecurityExceptionEvent(ClipboardSecurityExceptionEventArgs args);
	}
}