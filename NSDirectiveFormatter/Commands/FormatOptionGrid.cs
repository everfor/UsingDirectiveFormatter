namespace UsingDirectiveFormatter.Commands
{
    using System.ComponentModel;
    using Microsoft.VisualStudio.Shell;
    using System.Collections.ObjectModel;
    using UsingDirectiveFormatter.Contracts;

    /// <summary>
    /// FormatOptionGrid
    /// </summary>
    /// <seealso cref="Microsoft.VisualStudio.Shell.DialogPage" />
    public class FormatOptionGrid : DialogPage
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
        /// The sort groups
        /// </summary>
        private Collection<SortGroup> sortGroups = new Collection<SortGroup>();

        /// <summary>
        /// Gets or sets the sort order option.
        /// </summary>
        /// <value>
        /// The sort order option.
        /// </value>
        [Category("Options")]
        [DisplayName("Inside Namespace")]
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

        /// <summary>
        /// Gets or sets the sort groups.
        /// </summary>
        /// <value>
        /// The sort groups.
        /// </value>
        [Category("Options")]
        [DisplayName("4. Sort Groups")]
        [Description("Namespace groups that have relative orders defined by user: sorting will only happen within groups.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [TypeConverter(typeof(SortGroupCollectionConverter))]
        public Collection<SortGroup> SortGroups
        {
            get
            {
                return sortGroups;
            }

            set
            {
                sortGroups = value;
            }
        }
    }
}
