namespace EnvDTE
{
    using System;
    using EnvDTE80;
    using Microsoft.VisualStudio.Shell;
    using UsingDirectiveFormatter.Utilities;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Shell.Interop;
    using Microsoft.VisualStudio.TextManager.Interop;
    using VSIServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

    /// <summary>
    /// DocumentExtensions
    /// </summary>
    public static class DocumentExtensions
    {
        /// <summary>
        /// To the i vs text view.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="dte2">The dte2.</param>
        /// <returns></returns>
        public static IVsTextView ToIVsTextView(this Document document, DTE2 dte2)
        {
            ArgumentGuard.ArgumentNotNull(document, "document");
            ArgumentGuard.ArgumentNotNull(dte2, "dte2");

            IVsUIHierarchy uiHierarchy;
            uint itemId;
            IVsWindowFrame windowFrame;

            if (VsShellUtilities.IsDocumentOpen(new ServiceProvider(dte2 as VSIServiceProvider), 
                document.FullName, Guid.Empty, out uiHierarchy, out itemId, out windowFrame))
            {
                return VsShellUtilities.GetTextView(windowFrame);
            }

            return null;
        }

        /// <summary>
        /// To the i WPF text view.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="dte2">The dte2.</param>
        /// <returns></returns>
        public static IWpfTextView ToIWpfTextView(this Document document, DTE2 dte2)
        {
            ArgumentGuard.ArgumentNotNull(document, "document");
            ArgumentGuard.ArgumentNotNull(dte2, "dte2");

            return document.ToIVsTextView(dte2).ToWpfTextView();
        }

        /// <summary>
        /// Determines whether [is c sharp code].
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>
        ///   <c>true</c> if [is c sharp code] [the specified document]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsCSharpCode(this Document document)
        {
            ArgumentGuard.ArgumentNotNull(document, "document");

            return document.Language.Equals("CSharp", StringComparison.OrdinalIgnoreCase);
        }
    }
}
