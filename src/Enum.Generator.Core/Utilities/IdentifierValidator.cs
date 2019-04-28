using System.Linq;

namespace Enum.Generator.Core.Utilities
{
    /// <summary>
    /// Utilities for validating identifiers.
    /// </summary>
    public static class IdentifierValidator
    {
        /// <summary>
        /// Validate if given string can be used as an identifier.
        /// </summary>
        /// <param name="identifier">String to validate</param>
        /// <returns>'True' if valid, otherwise 'False'</returns>
        public static bool Validate(string identifier)
        {
            // Identifier cannot be empty.
            if (string.IsNullOrEmpty(identifier))
                return false;

            // Identifier has to start with a letter or underscore.
            if (!char.IsLetter(identifier[0]) && identifier[0] != '_')
                return false;

            // Validate if all characters are valid.
            return identifier.All(ValidateCharacter);

            bool ValidateCharacter(char c)
            {
                // Validate the unicode category, following the c# rules of allowing these categories:
                // Lu, Ll, Lt, Lm, Lo, Nl, Mn, Mc, Nd, Pc, and Cf
                var charCat = char.GetUnicodeCategory(c);
                return
                    charCat == System.Globalization.UnicodeCategory.UppercaseLetter ||
                    charCat == System.Globalization.UnicodeCategory.LowercaseLetter ||
                    charCat == System.Globalization.UnicodeCategory.TitlecaseLetter ||
                    charCat == System.Globalization.UnicodeCategory.ModifierLetter ||
                    charCat == System.Globalization.UnicodeCategory.OtherLetter ||
                    charCat == System.Globalization.UnicodeCategory.LetterNumber ||
                    charCat == System.Globalization.UnicodeCategory.NonSpacingMark ||
                    charCat == System.Globalization.UnicodeCategory.SpacingCombiningMark ||
                    charCat == System.Globalization.UnicodeCategory.DecimalDigitNumber ||
                    charCat == System.Globalization.UnicodeCategory.ConnectorPunctuation ||
                    charCat == System.Globalization.UnicodeCategory.Format;
            }
        }
    }
}
