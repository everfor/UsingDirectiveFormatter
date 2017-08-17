namespace System.Collections.Generic
{
    using System;
    using System.Linq;
    using UsingDirectiveFormatter.Contracts;
    using UsingDirectiveFormatter.Utilities;

    /// <summary>
    /// GenericsExtensions
    /// </summary>
    public static class GenericsExtensions
    {
        /// <summary>
        /// The using namespace directive prefix
        /// </summary>
        private static readonly string UsingNamespaceDirectivePrefix = "using";

        /// <summary>
        /// Orders the by sort standards.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="standards">The standards.</param>
        /// <returns></returns>
        public static IEnumerable<string> OrderBySortStandards(this IEnumerable<string> collection,
            IList<SortStandard> standards)
        {
            ArgumentGuard.ArgumentNotNull(collection, "collection");
            ArgumentGuard.ArgumentNotNull(standards, "standards");

            standards = standards.Where(s => s != SortStandard.None).ToList();

            if (standards.Count > 0)
            {
                return standards.Aggregate((IOrderedEnumerable<string>) null, (orderedCollection, standard) => 
                {
                    if (orderedCollection == null)
                    {
                        return collection.OrderBySortStandard(standard);
                    }

                    return orderedCollection.OrderBySortStandard(standard, true);
                });
            }

            return collection;
        }

        /// <summary>
        /// Orders the by sort standard.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="standard">The standard.</param>
        /// <param name="chained">if set to <c>true</c> [chained].</param>
        /// <returns></returns>
        public static IOrderedEnumerable<string> OrderBySortStandard(this IEnumerable<string> collection,
                    SortStandard standard, bool chained = false)
        {
            ArgumentGuard.ArgumentNotNull(collection, "collection");

            switch (standard)
            {
                case SortStandard.Length:
                    return chained ?
                        ((IOrderedEnumerable<string>)collection).ThenBy(s => s.Length) :
                        collection.OrderBy(s => s.Length);
                case SortStandard.LengthDescending:
                    return chained ?
                        ((IOrderedEnumerable<string>)collection).ThenByDescending(s => s.Length) :
                        collection.OrderByDescending(s => s.Length);
                case SortStandard.Alphabetical:
                    // Replace the ending ; since it can mess up the string comparison
                    return chained ?
                        ((IOrderedEnumerable<string>)collection).ThenBy(s => s.Replace(";", "")) :
                        collection.OrderBy(s => s.Replace(";", ""));
                case SortStandard.AlphabeticalDescending:
                    // Replace the ending ; since it can mess up the string comparison
                    return chained ?
                        ((IOrderedEnumerable<string>)collection).ThenByDescending(s => s.Replace(";", "")) :
                        collection.OrderByDescending(s => s.Replace(";", ""));
                case SortStandard.None:
                    throw new ArgumentOutOfRangeException();
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Groups the by sort groups.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="groups">The groups.</param>
        /// <returns></returns>
        public static IList<string> GroupBySortGroups(this IList<string> collection, IList<SortGroup> groups)
        {
            ArgumentGuard.ArgumentNotNull(collection, "collection");
            ArgumentGuard.ArgumentNotNull(groups, "groups");

            if (!groups.Any())
            {
                return collection;
            }

            var dict = new Dictionary<SortGroup, IList<string>>();
            foreach (var group in groups)
            {
                dict[group] = new List<string>();
            }

            foreach (var value in collection)
            {
                foreach (var group in groups)
                {
                    if (group.Validate(value.Replace(UsingNamespaceDirectivePrefix, string.Empty).Trim()))
                    {
                        dict[group].Add(value);
                        break;
                    }
                }
            }

            var result = new List<string>();
            foreach (var group in groups)
            {
                result.AddRange(dict[group]);
            }
            result.AddRange(collection);

            return result.Distinct().ToList();
        }
    }
}