namespace UsingDirectiveFormatter.Contracts
{
    using System;
    using System.Text.RegularExpressions;
    using UsingDirectiveFormatter.Utilities;

    /// <summary>
    /// SortGroupExtension
    /// </summary>
    public static class SortGroupExtension
    {
        /// <summary>
        /// Validates the specified value.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static bool Validate(this SortGroup group, string value)
        {
            ArgumentGuard.ArgumentNotNull(group, "group");

            if (value == null)
            {
                return false;
            }

            switch (group.Standard)
            {
                case SortGroupStandard.StartsWith:
                    return value.StartsWith(group.Filter, StringComparison.OrdinalIgnoreCase);
                case SortGroupStandard.EndsWith:
                    return value.StartsWith(group.Filter, StringComparison.OrdinalIgnoreCase);
                case SortGroupStandard.Contains:
                    return value.IndexOf(group.Filter, StringComparison.OrdinalIgnoreCase) != -1;
                case SortGroupStandard.MatchRegex:
                    return (new Regex(group.Filter)).IsMatch(value);
                case SortGroupStandard.NotStartsWith:
                    return !value.StartsWith(group.Filter, StringComparison.OrdinalIgnoreCase);
                case SortGroupStandard.NotEndsWith:
                    return !value.EndsWith(group.Filter, StringComparison.OrdinalIgnoreCase);
                case SortGroupStandard.NotContains:
                    return value.IndexOf(group.Filter, StringComparison.OrdinalIgnoreCase) == -1;
                case SortGroupStandard.NotMatchRegex:
                    return !(new Regex(group.Filter)).IsMatch(value);
                default:
                    return false;
            }
        }
    }
}
