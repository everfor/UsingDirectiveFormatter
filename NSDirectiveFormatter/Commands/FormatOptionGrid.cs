namespace UsingDirectiveFormatter.Commands
{
    using System.ComponentModel;
    using Microsoft.VisualStudio.Shell;

    /// <summary>
    /// FormatOptionGrid
    /// </summary>
    /// <seealso cref="Microsoft.VisualStudio.Shell.DialogPage" />
    internal class FormatOptionGrid : DialogPage
    {
        /// <summary>
        /// The inside namespace
        /// </summary>
        private bool insideNamespace = true;

        /// <summary>
        /// The sort order
        /// </summary>
        private SortStandard sortOrder = SortStandard.Length;

        /// <summary>
        /// The chained sort order
        /// </summary>
        private SortStandard chainedSortOrder = SortStandard.None;

        /// <summary>
        /// Gets or sets the sort order option.
        /// </summary>
        /// <value>
        /// The sort order option.
        /// </value>
        [Category("Options")]
        [DisplayName("1. Inside Namespace")]
        [Description("Place using's inside namespace")]
        public bool InsideNamespace
        {
            get
            {
                return insideNamespace;
            }

            set
            {
                insideNamespace = value;
            }
        }

        /// <summary>
        /// Gets or sets the sort order option.
        /// </summary>
        /// <value>
        /// The sort order option.
        /// </value>
        [Category("Options")]
        [DisplayName("2. Sort by")]
        [Description("Sort standard")]
        public SortStandard SortOrderOption
        {
            get
            {
                return sortOrder;
            }

            set
            {
                sortOrder = value;
            }
        }

        /// <summary>
        /// Gets or sets the chained sort order option.
        /// </summary>
        /// <value>
        /// The chained sort order option.
        /// </value>
        [Category("Options")]
        [DisplayName("3. Then by")]
        [Description("Sort standard (chained)")]
        public SortStandard ChainedSortOrderOption
        {
            get
            {
                return chainedSortOrder;
            }

            set
            {
                chainedSortOrder = value;
            }
        }
    }

    /// <summary>
    /// SortOrder
    /// </summary>
    public enum SortStandard
    {
        /// <summary>
        /// The length
        /// </summary>
        Length = 0,

        /// <summary>
        /// The length descending
        /// </summary>
        LengthDescending,

        /// <summary>
        /// The alphabetical
        /// </summary>
        Alphabetical,

        /// <summary>
        /// The alphabetical descending
        /// </summary>
        AlphabeticalDescending,

        /// <summary>
        /// The none
        /// </summary>
        None
    }
}
