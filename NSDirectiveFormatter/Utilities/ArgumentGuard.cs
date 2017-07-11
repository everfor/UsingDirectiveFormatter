namespace UsingDirectiveFormatter.Utilities
{
    using System;

    public class ArgumentGuard
    {
        /// <summary>
        /// Arguments the not null.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void ArgumentNotNull(object argument, string name)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        /// <summary>
        /// Arguments the not null or empty.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="name">The name.</param>
        public static void ArgumentNotNullOrEmpty(object argument, string name)
        {
            if (!(argument is string))
            {
                throw new ArgumentException("ArgumentNotNullOrEmpty: argument is not string", name);
            }

            if (string.IsNullOrEmpty((string)argument))
            {
                throw new ArgumentNullException(name);
            }
        }

        /// <summary>
        /// Arguments the not null or white space.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="name">The name.</param>
        public static void ArgumentNotNullOrWhiteSpace(object argument, string name)
        {
            if (!(argument is string))
            {
                throw new ArgumentException("ArgumentNotNullOrEmpty: argument is not string", name);
            }

            if (string.IsNullOrWhiteSpace((string)argument))
            {
                throw new ArgumentNullException(name);
            }
        }
    }
}
