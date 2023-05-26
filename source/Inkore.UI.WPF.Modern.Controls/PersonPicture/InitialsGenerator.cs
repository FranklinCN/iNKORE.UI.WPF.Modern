﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Linq;

namespace Inkore.UI.WPF.Modern.Controls
{
    /// <summary>
    /// Value indicating the general character set for a given character.
    /// </summary>
    enum CharacterType
    {
        /// <summary>
        /// Indicates we could not match the character set.
        /// </summary>
        Other = 0,

        /// <summary>
        /// Member of the Latin character set.
        /// </summary>
        Standard = 1,

        /// <summary>
        /// Member of a symbolic character set.
        /// </summary>
        Symbolic = 2,

        /// <summary>
        /// Member of a character set which supports glyphs.
        /// </summary>
        Glyph = 3
    };

    /// <summary>
    /// PersonPicture Control. Displays the Profile Picture, or in its absence Initials,
    /// for a given Contact.
    /// </summary>
    class InitialsGenerator
    {
        /// <summary>
        /// Helper function which takes a DisplayName, as generated by
        /// Windows.ApplicationModel.Contacts, and returns a initials representation.
        /// </summary>
        /// <param name="contactDisplayName>The DisplayName of the person</param>
        /// <returns>
        /// String containing the initials representation of the given DisplayName.
        /// </returns>
        public static string InitialsFromDisplayName(string contactDisplayName)
        {
            CharacterType type = GetCharacterType(contactDisplayName);

            // We'll attempt to make initials only if we recognize a name in the Standard character set.
            if (type == CharacterType.Standard)
            {
                string displayName = contactDisplayName;

                StripTrailingBrackets(ref displayName);

                string[] words = Split(displayName, ' ');

                if (words.Length == 1)
                {
                    // If there's only a single long word, we'll show one initial.
                    string firstWord = words.First();

                    string result = GetFirstFullCharacter(firstWord);

                    return result.ToUpper();
                }
                else if (words.Length > 1)
                {
                    // If there's at least two words, we'll show two initials.
                    // 
                    // NOTE: Based on current implementation, we could be showing punctuation.
                    // For example, "John -Smith" would be "J-".
                    string firstWord = words.First();
                    string lastWord = words.Last();

                    string result = GetFirstFullCharacter(firstWord);
                    result += GetFirstFullCharacter(lastWord);

                    return result.ToUpper();
                }
                else
                {
                    // If there's only spaces in the name, we'll get a Vector size of 0.
                    return string.Empty;
                }
            }
            else
            {
                // Return empty string. In our code-behind we will produce a generic glyph as a result.
                return string.Empty;
            }
        }

        /// <summary>
        /// Helper function which indicates the type of characters in a given string
        /// </summary>
        /// <param name="str">String from which to detect character-set.</param>
        /// <returns>
        /// Character set of the string: Latin, Symbolic, Glyph, or other.
        /// </returns>
        public static CharacterType GetCharacterType(string str)
        {
            // Since we're doing initials, we're really only interested in the first
            // few characters. If the first three characters aren't a glyph then
            // we don't need to count it as such because we won't be changing meaning
            // by truncating to one or two.
            CharacterType result = CharacterType.Other;

            for (int i = 0; i < 3; i++)
            {
                // Break on null character. 0xFEFF is a terminating character which appears as null.
                if ((i >= str.Length) || (str[i] == '\0') || (str[i] == 0xFEFF))
                {
                    break;
                }

                char character = str[i];
                CharacterType evaluationResult = GetCharacterType(character);

                // In mix-match scenarios, we'll want to follow this order of precedence:
                // Glyph > Symbolic > Roman
                switch (evaluationResult)
                {
                    case CharacterType.Glyph:
                        result = CharacterType.Glyph;
                        break;
                    case CharacterType.Symbolic:
                        // Don't override a Glyph state with a Symbolic State.
                        if (result != CharacterType.Glyph)
                        {
                            result = CharacterType.Symbolic;
                        }
                        break;
                    case CharacterType.Standard:
                        // Don't override a Glyph or Symbolic state with a Latin state.
                        if ((result != CharacterType.Glyph) && (result != CharacterType.Symbolic))
                        {
                            result = CharacterType.Standard;
                        }
                        break;
                    default:
                        // Preserve result's current state (if we never got data other 
                        // than "Other", it'll be set to other anyway).
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// Helper function which indicates the character-set of a given character.
        /// </summary>
        /// <param name="character">Character for which to detect character-set.</param>
        /// <returns>
        /// Character set of the string: Latin, Symbolic, Glyph, or other.
        /// </returns>
        public static CharacterType GetCharacterType(char character)
        {
            // To ensure predictable behavior, we're currently operating on an allowed list of character sets.
            //
            // Each block below is a HEX range in the official Unicode spec, which defines a set
            // of Unicode characters. Changes to the character sets would only be made by Unicode, and
            // are highly unlikely (as it would break virtually every modern text parser).
            // Definitions available here: http://www.unicode.org/charts/
            //
            // GLYPH
            //
            // IPA Extensions
            if ((character >= 0x0250) && (character <= 0x02AF))
            {
                return CharacterType.Glyph;
            }

            // Arabic
            if ((character >= 0x0600) && (character <= 0x06FF))
            {
                return CharacterType.Glyph;
            }

            // Arabic Supplement
            if ((character >= 0x0750) && (character <= 0x077F))
            {
                return CharacterType.Glyph;
            }

            // Arabic Extended-A
            if ((character >= 0x08A0) && (character <= 0x08FF))
            {
                return CharacterType.Glyph;
            }

            // Arabic Presentation Forms-A
            if ((character >= 0xFB50) && (character <= 0xFDFF))
            {
                return CharacterType.Glyph;
            }

            // Arabic Presentation Forms-B
            if ((character >= 0xFE70) && (character <= 0xFEFF))
            {
                return CharacterType.Glyph;
            }

            // Devanagari
            if ((character >= 0x0900) && (character <= 0x097F))
            {
                return CharacterType.Glyph;
            }

            // Devanagari Extended
            if ((character >= 0xA8E0) && (character <= 0xA8FF))
            {
                return CharacterType.Glyph;
            }

            // Bengali
            if ((character >= 0x0980) && (character <= 0x09FF))
            {
                return CharacterType.Glyph;
            }

            // Gurmukhi
            if ((character >= 0x0A00) && (character <= 0x0A7F))
            {
                return CharacterType.Glyph;
            }

            // Gujarati
            if ((character >= 0x0A80) && (character <= 0x0AFF))
            {
                return CharacterType.Glyph;
            }

            // Oriya
            if ((character >= 0x0B00) && (character <= 0x0B7F))
            {
                return CharacterType.Glyph;
            }

            // Tamil
            if ((character >= 0x0B80) && (character <= 0x0BFF))
            {
                return CharacterType.Glyph;
            }

            // Telugu
            if ((character >= 0x0C00) && (character <= 0x0C7F))
            {
                return CharacterType.Glyph;
            }

            // Kannada
            if ((character >= 0x0C80) && (character <= 0x0CFF))
            {
                return CharacterType.Glyph;
            }

            // Malayalam
            if ((character >= 0x0D00) && (character <= 0x0D7F))
            {
                return CharacterType.Glyph;
            }

            // Sinhala
            if ((character >= 0x0D80) && (character <= 0x0DFF))
            {
                return CharacterType.Glyph;
            }

            // Thai
            if ((character >= 0x0E00) && (character <= 0x0E7F))
            {
                return CharacterType.Glyph;
            }

            // Lao
            if ((character >= 0x0E80) && (character <= 0x0EFF))
            {
                return CharacterType.Glyph;
            }

            // SYMBOLIC
            //
            // CJK Unified Ideographs
            if ((character >= 0x4E00) && (character <= 0x9FFF))
            {
                return CharacterType.Symbolic;
            }

            // CJK Unified Ideographs Extension 
            if ((character >= 0x3400) && (character <= 0x4DBF))
            {
                return CharacterType.Symbolic;
            }

            // CJK Unified Ideographs Extension B
            if ((character >= 0x20000) && (character <= 0x2A6DF))
            {
                return CharacterType.Symbolic;
            }

            // CJK Unified Ideographs Extension C
            if ((character >= 0x2A700) && (character <= 0x2B73F))
            {
                return CharacterType.Symbolic;
            }

            // CJK Unified Ideographs Extension D
            if ((character >= 0x2B740) && (character <= 0x2B81F))
            {
                return CharacterType.Symbolic;
            }

            // CJK Radicals Supplement
            if ((character >= 0x2E80) && (character <= 0x2EFF))
            {
                return CharacterType.Symbolic;
            }

            // CJK Symbols and Punctuation
            if ((character >= 0x3000) && (character <= 0x303F))
            {
                return CharacterType.Symbolic;
            }

            // CJK Strokes
            if ((character >= 0x31C0) && (character <= 0x31EF))
            {
                return CharacterType.Symbolic;
            }

            // Enclosed CJK Letters and Months
            if ((character >= 0x3200) && (character <= 0x32FF))
            {
                return CharacterType.Symbolic;
            }

            // CJK Compatibility
            if ((character >= 0x3300) && (character <= 0x33FF))
            {
                return CharacterType.Symbolic;
            }

            // CJK Compatibility Ideographs
            if ((character >= 0xF900) && (character <= 0xFAFF))
            {
                return CharacterType.Symbolic;
            }

            // CJK Compatibility Forms
            if ((character >= 0xFE30) && (character <= 0xFE4F))
            {
                return CharacterType.Symbolic;
            }

            // CJK Compatibility Ideographs Supplement
            if ((character >= 0x2F800) && (character <= 0x2FA1F))
            {
                return CharacterType.Symbolic;
            }

            // Greek and Coptic
            if ((character >= 0x0370) && (character <= 0x03FF))
            {
                return CharacterType.Symbolic;
            }

            // Hebrew
            if ((character >= 0x0590) && (character <= 0x05FF))
            {
                return CharacterType.Symbolic;
            }

            // Armenian
            if ((character >= 0x0530) && (character <= 0x058F))
            {
                return CharacterType.Symbolic;
            }

            // LATIN
            //
            // Basic Latin
            if ((character > 0x0000) && (character <= 0x007F))
            {
                return CharacterType.Standard;
            }

            // Latin-1 Supplement
            if ((character >= 0x0080) && (character <= 0x00FF))
            {
                return CharacterType.Standard;
            }

            // Latin Extended-A
            if ((character >= 0x0100) && (character <= 0x017F))
            {
                return CharacterType.Standard;
            }

            // Latin Extended-B
            if ((character >= 0x0180) && (character <= 0x024F))
            {
                return CharacterType.Standard;
            }

            // Latin Extended-C
            if ((character >= 0x2C60) && (character <= 0x2C7F))
            {
                return CharacterType.Standard;
            }

            // Latin Extended-D
            if ((character >= 0xA720) && (character <= 0xA7FF))
            {
                return CharacterType.Standard;
            }

            // Latin Extended-E
            if ((character >= 0xAB30) && (character <= 0xAB6F))
            {
                return CharacterType.Standard;
            }

            // Latin Extended Additional
            if ((character >= 0x1E00) && (character <= 0x1EFF))
            {
                return CharacterType.Standard;
            }

            // Cyrillic
            if ((character >= 0x0400) && (character <= 0x04FF))
            {
                return CharacterType.Standard;
            }

            // Cyrillic Supplement
            if ((character >= 0x0500) && (character <= 0x052F))
            {
                return CharacterType.Standard;
            }

            // Combining Diacritical Marks
            if ((character >= 0x0300) && (character <= 0x036F))
            {
                return CharacterType.Standard;
            }

            return CharacterType.Other;
        }

        /// <summary>
        /// Helper function which takes in a string and returns a vector of pieces, separated by delimiter.
        /// </summary>
        /// <param name="source">String on which to perform the split operation.</param>
        /// <param name="delim">String on which to perform the split operation.</param>
        /// <param name="maxIterations">Maximum number of times to perform a <code>getline</code> loop.</param>
        /// <returns>A vector of pieces from the source string, separated by delimiter</returns>
        static string[] Split(string source, char delim, int maxIterations = 25)
        {
            return source.Split(new[] { delim }, maxIterations);
        }

        /// <summary>
        /// Helper function to remove bracket qualifier from the end of a display name if present.
        /// </summary>
        /// <param name="source">String on which to perform the operation.</param>
        /// <returns>A string with the content within brackets removed.</returns>
        static void StripTrailingBrackets(ref string source)
        {
            // Guidance from the world readiness team is that text within a final set of brackets
            // can be removed for the purposes of calculating initials. ex. John Smith (OSG)
            string[] delimiters = { "{}", "()", "[]" };

            if (source.Length == 0)
            {
                return;
            }

            foreach (var delimiter in delimiters)
            {
                if (source[source.Length - 1] != delimiter[1])
                {
                    continue;
                }

                var start = source.LastIndexOf(delimiter[0]);
                if (start == -1)
                {
                    continue;
                }

                source = source.Remove(start);
                return;
            }
        }

        /// <summary>
        /// Extracts the first full character from a given string, including any diacritics or combining characters.
        /// </summary>
        /// <param name="str">String from which to extract the character.</param>
        /// <returns>A wstring which represents a given character.</returns>
        static string GetFirstFullCharacter(string str)
        {
            // Index should begin at the first desireable character.
            int start = 0;

            while (start < str.Length)
            {
                char character = str[start];

                // Omit ! " # $ % & ' ( ) * + , - . /
                if ((character >= 0x0021) && (character <= 0x002F))
                {
                    start++;
                    continue;
                }

                // Omit : ; < = > ? @
                if ((character >= 0x003A) && (character <= 0x0040))
                {
                    start++;
                    continue;
                }

                // Omit { | } ~
                if ((character >= 0x007B) && (character <= 0x007E))
                {
                    start++;
                    continue;
                }

                break;
            }

            // If no desireable characters exist, we'll start at index 1, as combining
            // characters begin only after the first character.
            if (start >= str.Length)
            {
                start = 0;
            }

            // Combining characters begin only after the first character, so we should start
            // looking 1 after the start character.
            int index = start + 1;

            while (index < str.Length)
            {
                char character = str[index];

                // Combining Diacritical Marks -- Official Unicode character block
                if ((character < 0x0300) || (character > 0x036F))
                {
                    break;
                }

                index++;
            }

            // Determine number of diacritics by adjusting for our initial offset.
            int strLength = index - start;

            string result = str.SafeSubstring(start, strLength);
            return result;
        }
    };
}
