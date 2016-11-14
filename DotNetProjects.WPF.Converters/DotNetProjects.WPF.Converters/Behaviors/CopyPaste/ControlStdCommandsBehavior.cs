using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media.Imaging;
using DotNetProjects.WPF.Converters.Behaviors.CopyPaste.Commands;

namespace DotNetProjects.WPF.Converters.Behaviors.CopyPaste
{
	/// <summary>
	/// Behavior add to context menu of text box six default commands: Undo, Copy, Cut, Delete, Paste and Select All.
	/// Code based on http://www.codepaste.net/ckwbjm
	/// </summary>
    public class ControlStdCommandsBehavior : Behavior<Control>, ICloneableBehavior, IClipboardSecurityExceptionNotify
	{
        public object CloneBehavior()
        {
            return new ControlStdCommandsBehavior();
        }

        #region Static Members

        private static readonly Type TriggerType = typeof(ControlStdCommandsBehavior);

		public static readonly DependencyProperty UndoMenuHeaderProperty =
			DependencyProperty.Register("UndoMenuHeader", typeof (object), TriggerType,
			                            new PropertyMetadata("Undo",
			                                                 (sender, args) =>
                                                             ((ControlStdCommandsBehavior)sender).
			                                                 	OnMenuHeaderPropertyChanged(StdCommandActionType.Undo, args)));

		public static readonly DependencyProperty CutMenuHeaderProperty =
			DependencyProperty.Register("CutMenuHeader", typeof(object), TriggerType,
			                            new PropertyMetadata("Cut",
			                                                 (sender, args) =>
                                                             ((ControlStdCommandsBehavior)sender).
			                                                 	OnMenuHeaderPropertyChanged(StdCommandActionType.Cut, args)));

		public static readonly DependencyProperty CopyMenuHeaderProperty =
			DependencyProperty.Register("CopyMenuHeader", typeof(object), TriggerType,
			                            new PropertyMetadata("Copy",
			                                                 (sender, args) =>
                                                             ((ControlStdCommandsBehavior)sender).
			                                                 	OnMenuHeaderPropertyChanged(StdCommandActionType.Copy, args)));

		public static readonly DependencyProperty PasteMenuHeaderProperty =
			DependencyProperty.Register("PasteMenuHeader", typeof(object), TriggerType,
			                            new PropertyMetadata("Paste",
			                                                 (sender, args) =>
                                                             ((ControlStdCommandsBehavior)sender).
			                                                 	OnMenuHeaderPropertyChanged(StdCommandActionType.Paste, args)));

		public static readonly DependencyProperty DeleteMenuHeaderProperty =
			DependencyProperty.Register("DeleteMenuHeader", typeof(object), TriggerType,
			                            new PropertyMetadata("Delete",
			                                                 (sender, args) =>
                                                             ((ControlStdCommandsBehavior)sender).
			                                                 	OnMenuHeaderPropertyChanged(StdCommandActionType.Delete, args)));

		public static readonly DependencyProperty SelectAllMenuHeaderProperty =
			DependencyProperty.Register("SelectAllMenuHeader", typeof(object), TriggerType,
			                            new PropertyMetadata("Select All",
			                                                 (sender, args) =>
                                                             ((ControlStdCommandsBehavior)sender).
			                                                 	OnMenuHeaderPropertyChanged(StdCommandActionType.SelectAll, args)));

		#endregion

		#region Private fields

		private ContextMenu _contextMenu;
		private readonly Dictionary<StdCommandActionType, MenuItem> _clipboardActions
			= new Dictionary<StdCommandActionType, MenuItem>();

		#endregion

		/// <summary>
		/// Header of Undo MenuItem. Default value is 'Undo'.
		/// </summary>
		public object UndoMenuHeader
		{
			get { return this.GetValue(UndoMenuHeaderProperty); }
			set { this.SetValue(UndoMenuHeaderProperty, value); }
		}

		/// <summary>
		/// Header of Cut MenuItem. Default value is 'Cut'.
		/// </summary>
		public object CutMenuHeader
		{
			get { return this.GetValue(CutMenuHeaderProperty); }
			set { this.SetValue(CutMenuHeaderProperty, value); }
		}

		/// <summary>
		/// Header of Copy MenuItem. Default value is 'Copy'.
		/// </summary>
		public object CopyMenuHeader
		{
			get { return this.GetValue(CopyMenuHeaderProperty); }
			set { this.SetValue(CopyMenuHeaderProperty, value); }
		}

		/// <summary>
		/// Header of Paste MenuItem. Default value is 'Paste'.
		/// </summary>
		public object PasteMenuHeader
		{
			get { return this.GetValue(PasteMenuHeaderProperty); }
			set { this.SetValue(PasteMenuHeaderProperty, value); }
		}

		/// <summary>
		/// Header of Delete MenuItem. Default value is 'Delete'.
		/// </summary>
		public object DeleteMenuHeader
		{
			get { return this.GetValue(DeleteMenuHeaderProperty); }
			set { this.SetValue(DeleteMenuHeaderProperty, value); }
		}

		/// <summary>
		/// Header of SelectAll MenuItem. Default value is 'Select All'.
		/// </summary>
		public object SelectAllMenuHeader
		{
			get { return this.GetValue(SelectAllMenuHeaderProperty); }
			set { this.SetValue(SelectAllMenuHeaderProperty, value); }
		}

		/// <summary>
		/// Raised if some command will get security exception at getting access to clipboard. 
		/// Subscribe to it if you want to show user friendly message.
		/// </summary>
		public event EventHandler<ClipboardSecurityExceptionEventArgs> ClipboardSecurityException;

		void IClipboardSecurityExceptionNotify.OnClipboardSecurityExceptionEvent(ClipboardSecurityExceptionEventArgs args)
		{
			var handler = this.ClipboardSecurityException;

			if (handler != null)
				handler(this, args);
		}

		
		protected override void OnAttached()
		{

			if (this.AssociatedObject != null)
			{
				// Subscribe on Loaded event of TextBox.
				this.AssociatedObject.Loaded += this.AssociatedObjectLoaded;
                this.AssociatedObject.Unloaded += this.AssociatedObjectUnLoaded;
			}

			base.OnAttached();
		}

	    private void AssociatedObjectUnLoaded(object sender, RoutedEventArgs e)
	    {
	        this.DestructClipBoard();
	    }

	    protected override void OnDetaching()
	    {
	        // Dispose all Commands (we are trying to prevent memory leaks)
	        this.DestructClipBoard();
	        base.OnDetaching();
	    }

	    private void DestructClipBoard()
	    {
            ContextMenuService.SetContextMenu(this.AssociatedObject, null);

	        foreach (MenuItem item in this._clipboardActions.Values)
	        {
	            var commandBase = (CommandBase)item.Command;

                if ( commandBase != null)
                {
                    commandBase.Dispose();
                }

                item.Command = null;
	        }
	        this._clipboardActions.Clear();

	        if (this._contextMenu != null)
	        {
	            this._contextMenu.Opened -= this.ContextMenuOpened;
	            this._contextMenu = null;
	        }
	    }

	    /// <summary>
		/// Create menu if it doesn't exist. 
		/// After first invoke of current method we unsubscribe this method. 
		/// Creation of Context Menu should be happened on first Loaded event invoke. 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AssociatedObjectLoaded(object sender, RoutedEventArgs e)
		{
			this.AssociatedObject.Loaded -= this.AssociatedObjectLoaded;
			if (this._contextMenu == null)
			{
				this.CreateMenu();
			}
		}

		private void CreateMenu()
		{
			// Try to get first Context Menu, which can be created by developer in XAML.
			this._contextMenu = ContextMenuService.GetContextMenu(this.AssociatedObject);
			if (this._contextMenu == null)
			{
				// If developer doesn't create ContextMenu with XAML then we create new one.
				this._contextMenu = new ContextMenu();
				ContextMenuService.SetContextMenu(this.AssociatedObject, this._contextMenu);
			}

			// Subscribe on Opened event. On Opened event we will check for all command if they can be executed.
			this._contextMenu.Opened += this.ContextMenuOpened;

		    var textbox = this.AssociatedObject as TextBox;
            if (textbox == null && this.AssociatedObject is ContentControl && ((ContentControl)this.AssociatedObject).Content is TextBox)
            {
                textbox = ((ContentControl)this.AssociatedObject).Content as TextBox;
            }
           
		    if (textbox != null)
		    {
		        this.AddMenuItem(
		            StdCommandActionType.Undo,
		            new MenuItem
		                {
		                    Header = this.UndoMenuHeader,
                            Command = new UndoCommand(textbox),
		                    Icon = GetContextMenuImage("Edit_UndoHS.png")
		                });

		        this._contextMenu.Items.Add(new Separator());

		        this.AddMenuItem(
		            StdCommandActionType.Cut,
		            new MenuItem
		                {
		                    Header = this.CutMenuHeader,
                            Command = new CutCommand(textbox, this),
		                    Icon = GetContextMenuImage("CutHS.png")
		                });
		    }
		    this.AddMenuItem(StdCommandActionType.Copy,
			            new MenuItem
			            	{
			            		Header = this.CopyMenuHeader,
			            		Command = new CopyCommand(this.AssociatedObject, this),
			            		Icon = GetContextMenuImage("CopyHS.png")
			            	});

		    if (textbox != null)
		    {
		        this.AddMenuItem(
		            StdCommandActionType.Paste,
		            new MenuItem
		                {
		                    Header = this.PasteMenuHeader,
                            Command = new PasteCommand(textbox, this),
		                    Icon = GetContextMenuImage("PasteHS.png")
		                });
		        this.AddMenuItem(
		            StdCommandActionType.Delete,
		            new MenuItem
		                {
		                    Header = this.DeleteMenuHeader,
                            Command = new DeleteCommand(textbox),
		                    Icon = GetContextMenuImage("DeleteHS.png")
		                });

		        this._contextMenu.Items.Add(new Separator());

		        this.AddMenuItem(
		            StdCommandActionType.SelectAll,
                    new MenuItem { Header = this.SelectAllMenuHeader, Command = new SelectAllCommand(textbox) });
		    }
		}

		/// <summary>
		/// Add <paramref name="menuItem"/> to context menu associated with TextBox.
		/// Add <paramref name="menuItem"/> to private dictionary with key <paramref name="actionType"/> 
		/// so you can always get MenuItem with method <see cref="GetMenuItem"/> by action type.
		/// </summary>
		/// <param name="actionType"></param>
		/// <param name="menuItem"></param>
		private void AddMenuItem(StdCommandActionType actionType, MenuItem menuItem)
		{
			this._contextMenu.Items.Add(menuItem);
			this._clipboardActions.Add(actionType, menuItem);
		}

		/// <summary>
		/// Get created MenuItem. If Menu is not created then LINQ exception will throw.
		/// </summary>
		/// <param name="actionType"></param>
		/// <returns></returns>
		private MenuItem GetMenuItem(StdCommandActionType actionType)
		{
			return this._clipboardActions.Where(x => x.Key == actionType).Select(x => x.Value).Single();
		}

		/// <summary>
		/// Invoke RaiseCanExecuteChanged method for all Standard commands of current menu.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ContextMenuOpened(object sender, RoutedEventArgs e)
		{
			foreach (MenuItem item in this._clipboardActions.Values)
			{
				var commandBase = (CommandBase)item.Command;
				commandBase.RaiseCanExecuteChanged();
			}
		}

		/// <summary>
		/// Change header text of menu item if it is already created.
		/// </summary>
		/// <param name="actionType"></param>
		/// <param name="args"></param>
		private void OnMenuHeaderPropertyChanged(StdCommandActionType actionType, DependencyPropertyChangedEventArgs args)
		{
			if (this._contextMenu != null)
			{
				this.GetMenuItem(actionType).Header = args.NewValue;
			}
		}

		public static Image GetContextMenuImage(string fileName)
		{
            string uriPath = string.Format("pack://application:,,,/JFKCommonLibrary;component/Behaviors/CopyPaste/Images/{0}", fileName);
            BitmapImage bmp = new BitmapImage(new Uri(uriPath, UriKind.Absolute));
            return new Image { Source = bmp };			
		}
	}
}