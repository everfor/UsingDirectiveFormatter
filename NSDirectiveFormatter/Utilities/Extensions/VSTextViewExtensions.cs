namespace Microsoft.VisualStudio.TextManager.Interop
{
    using ComponentModelHost;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Editor;
    using UsingDirectiveFormatter.Utilities;
    using Microsoft.VisualStudio.Text.Editor;

    /// <summary>
    /// VSTetxViewExtensions
    /// </summary>
    public static class VSTextViewExtensions
    {
        /// <summary>
        /// To the WPF text view.
        /// </summary>
        /// <param name="textView">The text view.</param>
        /// <returns></returns>
        public static IWpfTextView ToWpfTextView(this IVsTextView textView)
        {
            ArgumentGuard.ArgumentNotNull(textView, "textView");

            var componentModel = Package.GetGlobalService(typeof(SComponentModel)) as IComponentModel;
            var editorAdaptor = componentModel.GetService<IVsEditorAdaptersFactoryService>();

            return editorAdaptor.GetWpfTextView(textView);
        }
    }
}
