namespace UsingDirectiveFormatter.Commands
{
    using System;
    using System.Linq;
    using System.Globalization;
    using System.ComponentModel;
    using System.Collections.ObjectModel;
    using UsingDirectiveFormatter.Contracts;

    /// <summary>
    /// SortGroupCollectionConverter
    /// </summary>
    /// <seealso cref="TypeConverter" />
    public class SortGroupCollectionConverter : TypeConverter
    {
        /// <summary>
        /// The default string conversion
        /// </summary>
        private static readonly string DefaultStringValue = "(none)";

        /// <summary>
        /// The everything else
        /// </summary>
        private static readonly string EverythingElse = "[EverythingElse]";

        /// <summary>
        /// The delimiter
        /// </summary>
        private static readonly string Delimiter = "-";

        /// <summary>
        /// Returns whether this converter can convert the object to the specified type, using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
        /// <param name="destinationType">A <see cref="T:System.Type" /> that represents the type you want to convert to.</param>
        /// <returns>
        /// true if this converter can perform the conversion; otherwise, false.
        /// </returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// Converts the given value object to the specified type, using the specified context and culture information.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> that provides a format context.</param>
        /// <param name="culture">A <see cref="T:System.Globalization.CultureInfo" />. If null is passed, the current culture is assumed.</param>
        /// <param name="value">The <see cref="T:System.Object" /> to convert.</param>
        /// <param name="destinationType">The <see cref="T:System.Type" /> to convert the <paramref name="value" /> parameter to.</param>
        /// <returns>
        /// An <see cref="T:System.Object" /> that represents the converted value.
        /// </returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is Collection<SortGroup>)
            {
                var values = (Collection<SortGroup>)value;
                var stringValue = string.Join(Delimiter, values.Select(g => "[" + g.ToString() + "]").ToList());

                return string.IsNullOrEmpty(stringValue) ?
                    DefaultStringValue : string.Join(Delimiter, stringValue, EverythingElse);
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
