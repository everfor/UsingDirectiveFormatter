//------------------------------------------------------------------------------
// <copyright file="FormatCommand.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace UsingDirectiveFormatter.Commands
{
    using EnvDTE;
    using System;
    using EnvDTE80;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Shell;
    using System.ComponentModel.Design;
    using UsingDirectiveFormatter.Contracts;
    using Microsoft.VisualStudio.Shell.Interop;

    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class FormatCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("2bb0aad7-e323-43dc-883c-cab65d5684c7");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly Package package;

        /// <summary>
        /// The DTE
        /// </summary>
        private DTE2 Dte
        {
            get;
            set;
        }

        /// <summary>
        /// The document
        /// </summary>
        private Document document
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormatCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private FormatCommand(Package package)
        {
            this.package = package ?? throw new ArgumentNullException("package");

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new OleMenuCommand(this.MenuItemCallback, menuCommandID);
                menuItem.BeforeQueryStatus += MenuItem_BeforeQueryStatus;
                commandService.AddCommand(menuItem);
            }

            this.Dte = this.ServiceProvider.GetService(typeof(SDTE)) as DTE2;
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static FormatCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(Package package)
        {
            Instance = new FormatCommand(package);
        }

        /// <summary>
        /// Handles the BeforeQueryStatus event of the MenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void MenuItem_BeforeQueryStatus(object sender, EventArgs e)
        {
            var command = (OleMenuCommand)sender;

            command.Visible = false;
            command.Enabled = false;

            this.document = this.Dte.ActiveDocument;

            if (this.document == null || 
                !this.document.IsCSharpCode())
            {
                return;
            }

            command.Visible = true;
            command.Enabled = true;
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            var options = ((FormatCommandPackage)this.package).GetOptions();
            this.document
                .ToIWpfTextView(this.Dte)
                .TextBuffer
                .Format(new List<SortStandard> { options.SortOrderOption, options.ChainedSortOrderOption }, options.InsideNamespace);
        }
    }
}
