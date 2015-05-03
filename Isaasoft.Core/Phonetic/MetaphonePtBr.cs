using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Isaasoft.Core.Phonetic
{
    public static class MetaphonePtBr
    {
        public static readonly char[] Vowel = null;

        static MetaphonePtBr()
        {
            Vowel = new char[] { 'A', 'E', 'I', 'O', 'U' };
        }

        public static string MetaphoneComplex(string text)
        {
            var originalText = text;

            text = TextPattern(text);
            text = OnlyText(text);

            text = text.ToUpper();

            text = text.Replace("Ç", "SS");

            text = text.Replace("LH", "1");
            text = text.Replace("RR", "2");
            text = text.Replace("NH", "3");
            text = text.Replace("XC", "SS");

            text = text.Replace("TH", "T");
            text = text.Replace("PH", "F");

            var lastIndex = text.Length - 1;
            var position = 0;
            var metaKey = string.Empty;

            while (position <= lastIndex)
            {
                var isLastPosition = position == lastIndex;
                var isBeginOfWord = (position == 0 || (position > 0 && text[position - 1] == ' '));
                var isEndOfWord = isLastPosition || text[position + 1] == ' ';

                var current = text[position];
                var next = !isLastPosition ? text[position + 1] : default(char?);
                var prev = position > 0 ? text[position - 1] : default(char?);

                if (isBeginOfWord && Vowel.Contains(current))
                {
                    metaKey += current;
                    position++;
                }
                else if (Vowel.Contains(current) || new[] { 'Y' }.Contains(current))
                {
                    switch (current)
                    {
                        case 'A':
                            metaKey += 'A';
                            break;
                        case 'E':
                        case 'I':
                        case 'Y':
                            metaKey += 'I';
                            break;
                        case 'O':
                        case 'U':
                            metaKey += 'U';
                            break;
                    }

                    position++;
                }
                else if (new[] { '1', '2', '3', 'B', 'D', 'F', 'J', 'K', 'L', 'M', 'P', 'T', 'V' }.Contains(current))
                {
                    metaKey += current;

                    if (!isLastPosition && next == current)
                        position += 2;
                    else
                        position++;
                }
                else
                {
                    switch (current)
                    {
                        case 'G':
                            switch (next)
                            {
                                case 'E':
                                case 'I':
                                    metaKey += 'J';
                                    break;
                                case 'R':
                                    metaKey += "GR";
                                    break;
                                default:
                                    metaKey += "G";
                                    break;
                            }

                            position += 1;
                            break;
                        case 'U':
                            if (prev != null)
                            {
                                if (Vowel.Contains(prev.Value))
                                    metaKey += 'L';
                            }

                            position++;
                            break;
                        case 'R':
                            if (isBeginOfWord || (isLastPosition || next == ' '))
                                metaKey += '2';
                            else
                                metaKey += 'R';

                            position++;
                            break;
                        case 'Z':
                            if (isLastPosition || next == ' ')
                                position += 2;
                            else if (next == 'Z')
                            {
                                metaKey += 'Z';
                                position += 2;
                            }
                            else
                            {
                                metaKey += 'Z';
                                position++;
                            }
                            break;
                        case 'N':
                            if (isLastPosition)
                            {
                                metaKey += 'M';
                                position++;
                            }
                            else if (next == 'N')
                            {
                                metaKey += 'N';
                                position += 2;
                            }
                            else
                            {
                                metaKey += 'N';
                                position += 1;
                            }
                            break;
                        case 'S':
                            var nextOfNext = position + 2 <= lastIndex ? text[position + 2] : default(char?);

                            if (isLastPosition || next == ' ')
                                position += 2;
                            else if (next == 'S')
                            {
                                metaKey += 'S';
                                position += 2;
                            }
                            else if (isBeginOfWord)
                            {
                                metaKey += 'S';
                                position += 1;
                            }
                            else if (prev != null && Vowel.Contains(prev.Value) || next != null && Vowel.Contains(next.Value))
                            {
                                metaKey += 'Z';
                                position += 1;
                            }
                            else if (next == 'C' && nextOfNext != null && new[] { 'E', 'I' }.Contains(nextOfNext.Value))
                            {
                                metaKey += "S";
                                position += 3;
                            }
                            else if (next == 'C' && nextOfNext != null && new[] { 'A', 'O', 'U' }.Contains(nextOfNext.Value))
                            {
                                metaKey += "SC";
                                position += 3;
                            }
                            else
                            {
                                metaKey += "S";
                                position += 1;
                            }
                            break;
                        case 'X':
                            var prenOfPrev = position - 2 >= 0 ? text[position - 2] : default(char?);

                            if (prev == 'E' && (prenOfPrev == null || prenOfPrev == ' '))
                                metaKey += 'Z';
                            else if (prev == 'I' && (prenOfPrev == null || prenOfPrev == ' '))
                                metaKey += 'X';
                            else if (next == 'I')
                                metaKey += 'Z';
                            else if (prev != null && Vowel.Contains(prev.Value) && (prenOfPrev == null || prenOfPrev == ' '))
                                metaKey += "KS";
                            else
                                metaKey += 'X';

                            position++;
                            break;
                        case 'C':
                            if (next != null && new[] { 'E', 'I' }.Contains(next.Value))
                            {
                                metaKey += 'S';
                                position += 2;
                            }
                            else if (next == 'H')
                            {
                                metaKey += 'X';
                                position += 2;
                            }
                            else
                            {
                                metaKey += 'K';
                                position++;
                            }
                            break;
                        case 'H':
                            if (next != null && Vowel.Contains(next.Value))
                            {
                                metaKey += next;
                                position += 2;
                            }
                            else
                                position++;

                            break;
                        case 'Q':
                            if (next == 'U')
                                position += 2;
                            else
                                position++;

                            metaKey += 'K';
                            break;
                        case 'W':
                            if (next != null && Vowel.Contains(next.Value))
                                metaKey += 'V';
                            else
                                metaKey += 'U';

                            position += 2;
                            break;
                        default:
                            position++;
                            break;
                    }
                }
            }

            return metaKey;
        }

        public static string MetaphoneSimple(string text)
        {
            var originalText = text;

            text = TextPattern(text);
            text = OnlyText(text);

            text = text.ToUpper();

            text = text.Replace("Ç", "SS");

            text = text.Replace("LH", "1");
            text = text.Replace("NH", "3");
            text = text.Replace("RR", "2");
            text = text.Replace("XC", "SS");

            text = text.Replace("TH", "T");
            text = text.Replace("PH", "F");

            var lastIndex = text.Length - 1;
            var position = 0;
            var metaKey = string.Empty;

            while (position <= lastIndex)
            {
                var isLastPosition = position == lastIndex;
                var isBeginOfWord = (position == 0 || (position > 0 && text[position - 1] == ' '));
                var isEndOfWord = isLastPosition || text[position + 1] == ' ';

                var current = text[position];
                var next = !isLastPosition ? text[position + 1] : default(char?);
                var prev = position > 0 ? text[position - 1] : default(char?);

                if (isBeginOfWord && Vowel.Contains(current))
                {
                    metaKey += current;
                    position++;
                }
                else if (new[] { '1', '2', '3', 'B', 'D', 'F', 'J', 'K', 'L', 'M', 'P', 'T', 'V' }.Contains(current))
                {
                    metaKey += current;

                    if (!isLastPosition && next == current)
                        position += 2;
                    else
                        position++;
                }
                else
                {
                    switch (current)
                    {
                        case 'G':
                            switch (next)
                            {
                                case 'E':
                                case 'I':
                                    metaKey += 'J';
                                    break;
                                case 'R':
                                    metaKey += "GR";
                                    break;
                                default:
                                    metaKey += "G";
                                    break;
                            }

                            position += 2;

                            break;
                        case 'U':
                            if (prev != null)
                            {
                                if (Vowel.Contains(prev.Value))
                                    metaKey += 'L';
                            }
                            position++;
                            break;
                        case 'R':
                            if (isBeginOfWord || (isLastPosition || next == ' '))
                                metaKey += '2';
                            else
                                metaKey += 'R';

                            position++;
                            break;
                        case 'Z':
                            if (isLastPosition || next == ' ')
                                position += 2;
                            else if (next == 'Z')
                            {
                                metaKey += 'Z';
                                position += 2;
                            }
                            else
                            {
                                metaKey += 'Z';
                                position++;
                            }
                            break;
                        case 'N':
                            if (isLastPosition)
                            {
                                metaKey += 'M';
                                position++;
                            }
                            else if (next == 'N')
                            {
                                metaKey += 'N';
                                position += 2;
                            }
                            else
                            {
                                metaKey += 'N';
                                position += 1;
                            }
                            break;
                        case 'S':
                            var nextOfNext = position + 2 <= lastIndex ? text[position + 2] : default(char?);

                            if (isLastPosition || next == ' ')
                                position += 2;
                            else if (next == 'S')
                            {
                                metaKey += 'S';
                                position += 2;
                            }
                            else if (isBeginOfWord)
                            {
                                metaKey += 'S';
                                position += 1;
                            }
                            else if (prev != null && Vowel.Contains(prev.Value) || next != null && Vowel.Contains(next.Value))
                            {
                                metaKey += 'Z';
                                position += 1;
                            }
                            else if (next == 'C' && nextOfNext != null && new[] { 'E', 'I' }.Contains(nextOfNext.Value))
                            {
                                metaKey += "S";
                                position += 3;
                            }
                            else if (next == 'C' && nextOfNext != null && new[] { 'A', 'O', 'U' }.Contains(nextOfNext.Value))
                            {
                                metaKey += "SC";
                                position += 3;
                            }
                            else
                            {
                                metaKey += "S";
                                position += 1;
                            }
                            break;
                        case 'X':
                            var prenOfPrev = position - 2 >= 0 ? text[position - 2] : default(char?);

                            if (prev == 'E' && (prenOfPrev == null || prenOfPrev == ' '))
                                metaKey += 'Z';
                            else if (prev == 'I' && (prenOfPrev == null || prenOfPrev == ' '))
                                metaKey += 'X';
                            else if (next == 'I')
                                metaKey += 'Z';
                            else if (prev != null && Vowel.Contains(prev.Value) && (prenOfPrev == null || prenOfPrev == ' '))
                                metaKey += "KS";
                            else
                                metaKey += 'X';

                            position++;
                            break;
                        case 'C':
                            if (next == 'E' || next == 'I')
                            {
                                metaKey += 'S';
                                position += 2;
                            }
                            else if (next == 'H')
                            {
                                metaKey += 'X';
                                position += 2;
                            }
                            else
                            {
                                metaKey += 'K';
                                position++;
                            }
                            break;
                        case 'H':
                            if (next != null && Vowel.Contains(next.Value))
                            {
                                metaKey += next;
                                position += 2;
                            }
                            else
                                position++;

                            break;
                        case 'Q':
                            if (next == 'U')
                                position += 2;
                            else
                                position++;

                            metaKey += 'K';
                            break;
                        case 'W':
                            if (next != null && Vowel.Contains(next.Value))
                                metaKey += 'V';
                            else
                                metaKey += 'U';

                            position += 2;
                            break;
                        default:
                            position++;
                            break;
                    }
                }
            }

            return metaKey;
        }

        public static string OnlyText(string text)
        {
            return Regex.Replace(text, "[0-9]", "", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }

        public static string TextPattern(string text)
        {
            var value = MetaphonePtBr.TextISO88598(text);

            value = MetaphonePtBr.OnlyLettersAndNumbers(value);
            value = MetaphonePtBr.JustABlankSpace(value);
            value = MetaphonePtBr.NoWhiteSpaceAtTheBeginning(value);
            value = MetaphonePtBr.NoWhiteSpaceAtTheEnd(value);

            return value;
        }

        public static string JustABlankSpace(string text)
        {
            return Regex.Replace(text, "\\s{2,}", " ", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }

        public static string NoWhiteSpaceAtTheBeginning(string text)
        {
            if (text.StartsWith(" "))
            {
                var index = 0;

                for (; index < text.Length && text[index] == ' '; index++) ;

                return text.Substring(index);
            }

            return text;
        }

        public static string NoWhiteSpaceAtTheEnd(string text)
        {
            if (text.EndsWith(" "))
            {
                var index = text.Length - 1;

                for (; index < text.Length && text[index] == ' '; index--) ;

                return text.Substring(0, index + 1);
            }

            return text;
        }

        public static string OnlyLettersAndNumbers(string text)
        {
            return Regex.Replace(text, "[^0-9a-z]", " ", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        }

        public static string TextISO88598(string text)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("iso-8859-8").GetBytes(text);

            return System.Text.Encoding.UTF8.GetString(bytes);
        }
    }
}
