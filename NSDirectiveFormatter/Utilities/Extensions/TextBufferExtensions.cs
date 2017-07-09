namespace Microsoft.VisualStudio.Text
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

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

            bool lastLineEmpty = false;
            var emptyLines = new List<ITextSnapshotLine>();

            for (int l = 0; l < snapShot.LineCount; l++, cursor = tail)
            {
                var line = snapShot.GetLineFromLineNumber(l);
                var lineText = line.GetText();
                var lineTextTrimmed = lineText.TrimStart();

                tail += line.LengthIncludingLineBreak;

                if (string.IsNullOrWhiteSpace(lineTextTrimmed))
                {
                    emptyLines.Add(line);
                    lastLineEmpty = true;
                }
                else
                {
                    if (!lastLineEmpty)
                    {
                        emptyLines.Clear();
                    }

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
                        if (nsReached)
                        {
                            indent = lineText.Substring(0, lineText.IndexOf(UsingNamespaceDirectivePrefix));
                        }
                    }
                    else if (lineTextTrimmed.StartsWith(NamespaceDeclarationPrefix, StringComparison.Ordinal))
                    {
                        prensSpan = new Span(0, cursor);
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
                    else if (lineTextTrimmed.Equals(";", StringComparison.Ordinal) ||
                        lineTextTrimmed.StartsWith("/", StringComparison.Ordinal))
                    {
                        continue;
                    }
                    else
                    {
                        nsSpan =
                            new Span(nsInnerStartPos,
                            cursor - nsInnerStartPos - emptyLines.Aggregate(0, (len, eline) => len + eline.LengthIncludingLineBreak));
                        break;
                    }
                }
            }

            usingDirectives = usingDirectives.AsEnumerable().OrderBy(s => s.Length).Select(s => indent + s).ToList();

            var insertPos = nsReached ? startPos : 0;
            var insertString = string.Join("\r\n", usingDirectives);

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
