namespace Microsoft.VisualStudio.Text
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using UsingDirectiveFormatter.Utilities;

    /// <summary>
    /// TextBufferExtensions
    /// </summary>
    public static class TextBufferExtensions
    {
        /// <summary>
        /// The using namespace directive prefix
        /// </summary>
        private static readonly string UsingNamespaceDirectivePrefix = "using";

        /// <summary>
        /// The namespace declaration prefix
        /// </summary>
        private static readonly string NamespaceDeclarationPrefix = "namespace";

        /// <summary>
        /// Formats the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        public static void Format(this ITextBuffer buffer)
        {
            ArgumentGuard.ArgumentNotNull(buffer, "buffer");

            var snapShot = buffer.CurrentSnapshot;

            int cursor = 0;
            int tail = 0;

            // using directive related
            var usingDirectives = new List<string>();
            int startPos = 0;

            // Namespace related flags
            bool nsReached = false;
            int nsInnerStartPos = 0;

            string indent = "";

            // Using directives before namespace (if any)
            Span? prensSpan = null;
            // Using directives inside namespace, or all usings is there's no namespace
            Span? nsSpan = null;

            bool lastLineEmptyOrComment = false;
            int spanToPreserve = 0;

            foreach (var line in snapShot.Lines)
            {
                var lineText = line.GetText();
                var lineTextTrimmed = lineText.TrimStart();

                cursor = tail;
                tail += line.LengthIncludingLineBreak;

                if (nsReached && 
                    !string.IsNullOrWhiteSpace(lineTextTrimmed) && 
                    string.IsNullOrEmpty(indent))
                {
                    indent = lineText.Substring(0, lineText.IndexOf(lineTextTrimmed));
                }

                if (string.IsNullOrWhiteSpace(lineTextTrimmed) ||
                        lineTextTrimmed.StartsWith("/", StringComparison.Ordinal))
                {
                    spanToPreserve += line.LengthIncludingLineBreak;
                    lastLineEmptyOrComment = true;
                }
                else
                {
                    if (!lastLineEmptyOrComment)
                    {
                        spanToPreserve = 0;
                    }

                    lastLineEmptyOrComment = false;

                    if (lineTextTrimmed.StartsWith(UsingNamespaceDirectivePrefix, StringComparison.Ordinal))
                    {
                        if (startPos == 0)
                        {
                            startPos = cursor;
                        }

                        if (nsInnerStartPos == 0)
                        {
                            nsInnerStartPos = cursor;
                        }

                        usingDirectives.Add(lineTextTrimmed);
                    }
                    else if (lineTextTrimmed.StartsWith(NamespaceDeclarationPrefix, StringComparison.Ordinal))
                    {
                        prensSpan = new Span(0, cursor - spanToPreserve);
                        nsReached = true;
                        startPos = tail;
                        nsInnerStartPos = tail;
                    }
                    else if (lineTextTrimmed.Equals("{", StringComparison.Ordinal))
                    {
                        if (nsReached)
                        {
                            startPos = tail;
                            nsInnerStartPos = tail;
                        }
                    }
                    else if (lineTextTrimmed.Equals(";", StringComparison.Ordinal))
                    {
                        continue;
                    }
                    else
                    {
                        nsSpan =
                            new Span(nsInnerStartPos, cursor - nsInnerStartPos - spanToPreserve);
                        break;
                    }
                }
            }

            usingDirectives = usingDirectives.AsEnumerable().OrderBy(s => s.Length).Select(s => indent + s).ToList();

            var insertPos = nsReached ? startPos : 0;
            var insertString = string.Join("\r\n", usingDirectives) + "\r\n";

            // Testing
            var edit = buffer.CreateEdit();
            edit.Insert(insertPos, insertString);
            if (nsSpan != null)
            {
                edit.Delete(nsSpan.Value);
            }
            if (prensSpan != null)
            {
                edit.Delete(prensSpan.Value);
            }
            edit.Apply();
        }
    }
}
