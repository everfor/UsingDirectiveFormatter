namespace UsingDirectiveFormatter.Commands
{
    using System.ComponentModel;
    using Microsoft.VisualStudio.Shell;
    using System.Collections.ObjectModel;
    using Microsoft.VisualStudio.Settings;
    using UsingDirectiveFormatter.Contracts;
    using UsingDirectiveFormatter.Utilities;
    using Microsoft.VisualStudio.Shell.Settings;

    /// <summary>
    /// FormatOptionGrid
    /// </summary>
    /// <seealso cref="DialogPage" />
    public class FormatOptionGrid : DialogPage
    {
        /// <summary>
        /// The collection name
        /// </summary>
        private static readonly string CollectionName = "UsingDirectiveFormatterVSIX";

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

        /// <summary>
        /// Called by Visual Studio to store the settings of a dialog page in local storage, typically the registry.
        /// </summary>
        public override void SaveSettingsToStorage()
        {
            base.SaveSettingsToStorage();

            var userSettingStore = GetUserSettingStore();

            if (!userSettingStore.CollectionExists(CollectionName))
            {
                userSettingStore.CreateCollection(CollectionName);
            }

            SaveToStore(userSettingStore, nameof(SortGroups), this.SortGroups,
                new SortGroupCollectionConverter());
        }

        /// <summary>
        /// Called by Visual Studio to load the settings of a dialog page from local storage, generally the registry.
        /// </summary>
        public override void LoadSettingsFromStorage()
        {
            base.LoadSettingsFromStorage();

            var userSettingStore = GetUserSettingStore();

            if (!userSettingStore.CollectionExists(CollectionName))
            {
                return;
            }
            
            this.sortGroups = LoadFromStore(userSettingStore, nameof(sortGroups), new SortGroupCollectionConverter())
                as Collection<SortGroup> ?? this.SortGroups;
        }

        /// <summary>
        /// Gets the user setting store.
        /// </summary>
        /// <returns></returns>
        private static WritableSettingsStore GetUserSettingStore()
        {
            // Custom saving/loading for sort group collections, since visual studio is broken and cannot save this
            // https://stackoverflow.com/questions/32751040/store-array-in-options-using-dialogpage
            var settingsManager = new ShellSettingsManager(ServiceProvider.GlobalProvider);
            var userSettingStore = settingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);

            return userSettingStore;
        }

        /// <summary>
        /// Saves to store.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="value">The value.</param>
        /// <param name="converter">The converter.</param>
        private static void SaveToStore(WritableSettingsStore store, string entry,
            object value, TypeConverter converter)
        {
            ArgumentGuard.ArgumentNotNull(store, "store");
            ArgumentGuard.ArgumentNotNullOrEmpty(entry, "entry");
            ArgumentGuard.ArgumentNotNull(value, "value");
            ArgumentGuard.ArgumentNotNull(converter, "converter");

            store.SetString(CollectionName, entry,
                converter.ConvertTo(value, typeof(string)) as string);
        }

        /// <summary>
        /// Loads from store.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="entry">The entry.</param>
        /// <param name="converter">The converter.</param>
        /// <returns></returns>
        private static object LoadFromStore(WritableSettingsStore store, string entry, TypeConverter converter)
        {
            ArgumentGuard.ArgumentNotNull(store, "store");
            ArgumentGuard.ArgumentNotNullOrEmpty(entry, "entry");
            ArgumentGuard.ArgumentNotNull(converter, "converter");

            return converter.ConvertFrom(store.GetString(CollectionName, entry));
        }
    }
}
