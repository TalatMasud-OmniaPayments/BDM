namespace Omnia.Pie.Vtm.Framework.Interface.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ReceiptOutput
    {
        private int OutputWidth { get; }
        private bool IsRightToLeft { get; }
        private bool IsMarkupEnabled { get; }

        public ReceiptOutput(int outputWidth, bool isRightToLeft, bool isMarkupEnabled)
        {
            OutputWidth = outputWidth;
            IsRightToLeft = isRightToLeft;
            IsMarkupEnabled = isMarkupEnabled;
        }

        /// <summary>
        /// Align text to the center of the row (add spaces to the begining of the string). Wrap string longer receipt row width.
        /// </summary>
        /// <param name="text">String that should be align to the center of the row</param>
        /// <returns></returns>
        public string WriteRowCenter(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";
            StringBuilder result = new StringBuilder();

            var chunks = SplitOn(text, OutputWidth);
            var lastChunk = chunks.Last();

            foreach (var line in chunks)
            {
                var lineWithoutMarkupChars = line.Replace("^B^", "").Replace("^/B^", "");
                var pad = (OutputWidth + lineWithoutMarkupChars.Length) / 2;
                pad += line.Contains("^B^") ? 3 : 0;
                pad += line.Contains("^/B^") ? 4 : 0;
                if (line != lastChunk)
                {
                    result.AppendLine(lineWithoutMarkupChars.Length < OutputWidth ? line.PadLeft(pad) : line);
                }
                else
                {
                    result.Append(lineWithoutMarkupChars.Length < OutputWidth ? line.PadLeft(pad) : line);
                }
            }
            return result.ToString();
        }

        /// <summary>
        /// Align text to the left or right(depend on flow direction) of the row. Wrap string longer receipt row width.
        /// </summary>
        /// <param name="text">String that should be align to the center of the row</param>
        /// <returns></returns>
        public string WriteRow<T>(T text)
        {
            if (string.IsNullOrWhiteSpace(text?.ToString()))
                return "";
            var result = new StringBuilder();

            var chunks = SplitOn(text.ToString(), OutputWidth - 1);
            var lastChunk = chunks.Last();

            foreach (var line in chunks)
            {
                // -1 to prevent extra space between receipt lines
                var value = IsRightToLeft ? line.PadLeft(OutputWidth - 1) : line;
                if (line != lastChunk)
                {
                    result.AppendLine(value);
                }
                else
                {
                    result.Append(value);
                }
            }
            return result.ToString();
        }

        /// <summary>
        /// Write columns text to the row. Allign column text to the left for the left to right language (Arabic) or to the right for the right to left language (English)
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public string WriteRow<T>(params T[] columns)
        {
            if (columns == null || columns.Length == 0)
                return "";
            columns = IsRightToLeft ? columns.Reverse().ToArray() : columns;
            var columnWidth = OutputWidth / columns.Length;

            var lastItem = columns.Last()?.ToString() ?? "";
            var firstItem = columns.First()?.ToString() ?? "";
            var items = columns.Select(x => x?.ToString() ?? "");

            var result = new StringBuilder();
            var itemNumber = 0;
            var itemsTotalLength = items.Sum(x => x.Length);
            foreach (var item in items)
            {
                var trimedItem = item.Length > columnWidth && itemsTotalLength >= OutputWidth ? item.Substring(0, columnWidth) : item;
                //Use the Left-to-right mark for LTR rendering, \u200E.
                if (IsRightToLeft)
                {
                    result.Append("\u200E");
                }
                if (item == firstItem)
                {
                    result.Append(trimedItem);

                }
                else if (item == lastItem)
                {
                    var padFromPrevious = OutputWidth >= result.Length && result.Length > 0 ? OutputWidth - result.Length : columnWidth;
                    // -1 to prevent extra space between receipt lines
                    padFromPrevious -= 1;
                    result.Append(trimedItem.PadLeft(padFromPrevious));
                }
                else
                {
                    var padFromPrevious = IsRightToLeft ?
                        columnWidth * (itemNumber + 1) >= result.Length + 2 && result.Length > 0 ?
                            columnWidth * (itemNumber + 1) - result.Length - (trimedItem.Length >= columnWidth - 2 ? 0 : 2) :
                        columnWidth * (itemNumber + 1) >= result.Length && result.Length > 0 ?
                            columnWidth * (itemNumber + 1) - result.Length :
                            columnWidth :

                        columnWidth * itemNumber + trimedItem.Length + 2 >= result.Length && result.Length > 0 ?
                            columnWidth * (itemNumber) - result.Length + trimedItem.Length + (trimedItem.Length >= columnWidth - 2 ? 0 : 2) :
                        columnWidth * itemNumber + trimedItem.Length >= result.Length && result.Length > 0 ?
                            columnWidth * (itemNumber) - result.Length + trimedItem.Length :
                            columnWidth;
                    result.Append(trimedItem.PadLeft(padFromPrevious));
                }
                itemNumber++;
            }

            return result.ToString();
        }

        public string WriteRow(params Column[] columns)
        {
            var engagedWidth = columns.Sum(c => c.Width);

            if (engagedWidth > OutputWidth)
                throw new ArgumentException();

            var freeWidth = OutputWidth - 1 - engagedWidth;
            var freeColumns = columns.Count(c => c.Width == 0);


            return WriteRow(string.Join("", columns.Select(c => c.FormattedText(c.Width != 0 ? c.Width : (byte)(freeWidth / freeColumns)))));
        }

        /// <summary>
        /// Write columns text to the header row. Allign column text to the center
        /// </summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public string WriteHeaderRow<T>(params T[] columns)
        {
            columns = IsRightToLeft ? columns.Reverse().ToArray() : columns;
            var columnWidth = OutputWidth / columns.Length;

            var items = columns.Select(x => x.ToString());

            var result = new StringBuilder();
            var itemNumber = 0;
            foreach (var item in items)
            {
                var trimedItem = item.Length > columnWidth ? item.Substring(0, columnWidth) : item;
                //Use the Left-to-right mark for LTR rendering, \u200E.
                if (IsRightToLeft)
                {
                    result.Append("\u200E");
                }
                result.Append(columnWidth * (2 * itemNumber + 1) + trimedItem.Length > 2 * result.Length ?
                    trimedItem.PadLeft((int)Math.Ceiling((columnWidth * (2 * itemNumber + 1M) - 2 * result.Length + trimedItem.Length) / 2)) : trimedItem);
                itemNumber++;
            }

            return result.ToString();
        }

        public string Bold(string text) => IsMarkupEnabled ? $"^B^{text}^/B^" : text;

        public string WriteRowSeparator()
        {
            // -1 to prevent extra space between receipt lines
            return new string('-', OutputWidth - 1);
        }

        private IEnumerable<string> SplitOn(string initial, int maxCharacters)
        {
            int pos, next;
            if (maxCharacters < 1)
                yield return initial;

            // Parse each line of text
            for (pos = 0; pos < initial.Length; pos = next)
            {
                // Find end of line
                int eol = initial.IndexOf(Environment.NewLine, pos);
                if (eol == -1)
                    next = eol = initial.Length;
                else
                    next = eol + Environment.NewLine.Length;

                // Copy this line of text, breaking into smaller lines as needed
                if (eol > pos)
                {
                    do
                    {
                        int len = eol - pos;
                        if (len > maxCharacters)
                            len = BreakLine(initial, pos, maxCharacters);
                        yield return initial.Substring(pos, len);

                        // Trim whitespace following break
                        pos += len;
                        while (pos < eol && Char.IsWhiteSpace(initial[pos]))
                            pos++;
                    } while (eol > pos);
                }
            }
        }

        private int BreakLine(string text, int pos, int max)
        {
            // Find last whitespace in line
            int i = max;
            while (i >= 0 && !Char.IsWhiteSpace(text[pos + i]))
                i--;

            // If no whitespace found, break at maximum length
            if (i < 0)
                return max;

            // Find start of whitespace
            while (i >= 0 && Char.IsWhiteSpace(text[pos + i]))
                i--;

            // Return length of text before whitespace
            return i + 1;
        }
    }

    public class Column
    {
        public string Text { get; }
        public byte Width { get; }
        public TextAlignment Alignment { get; }

        public Column(string text, byte width = 0, TextAlignment alignment = TextAlignment.Right)
        {
            Text = text ?? string.Empty;
            Width = width;
            Alignment = alignment;
        }

        public string FormattedText(byte? width)
        {
            var w = width ?? Width;
            return Text.Length == w
                ? Text
                : Text.Length > w
                    ? Text.Substring(0, w)
                    : Align(w);
        }

        private string Align(byte width)
        {
            switch (Alignment)
            {
                case TextAlignment.Left:
                    return Text.PadRight(width, ' ');
                case TextAlignment.Right:
                    return Text.PadLeft(width, ' ');
                case TextAlignment.Center:
                case TextAlignment.Justify:
                    return Text.PadLeft((width + Text.Length) / 2, ' ').PadRight(width, ' ');
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static Column Empty => new Column(string.Empty);

        public static Column Create(string text, byte width = 0, TextAlignment alignment = TextAlignment.Right)
        {
            return new Column(text, width, alignment);
        }

        public static Column Create(string text, TextAlignment alignment)
        {
            return new Column(text, 0, alignment);
        }
    }

    public enum TextAlignment
    {
        Left = 0,
        Right = 1,
        Center = 2,
        Justify = 3
    }
}
