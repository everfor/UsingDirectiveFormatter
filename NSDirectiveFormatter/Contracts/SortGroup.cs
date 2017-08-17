namespace UsingDirectiveFormatter.Contracts
{
    using System;
    using UsingDirectiveFormatter.Utilities;

    /// <summary>
    /// SortGroup
    /// </summary>
    public class SortGroup
    {
        #region Fields

        /// <summary>
        /// Gets the standard.
        /// </summary>
        /// <value>
        /// The standard.
        /// </value>
        public SortGroupStandard Standard
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the filter.
        /// </summary>
        /// <value>
        /// The filter.
        /// </value>
        public string Filter
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortGroup"/> class.
        /// </summary>
        [Obsolete("Designer Only", true)]
        public SortGroup()
        {
            this.Standard = SortGroupStandard.MatchRegex;
            this.Filter = ".";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortGroup"/> class.
        /// </summary>
        /// <param name="standard">The standard.</param>
        /// <param name="filter">The filter.</param>
        public SortGroup(SortGroupStandard standard, string filter)
        {
            ArgumentGuard.ArgumentNotNullOrWhiteSpace(filter, "filter");

            this.Standard = standard;
            this.Filter = filter;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{this.Standard.ToString()}({this.Filter})";
        }

        #endregion
    }
}
