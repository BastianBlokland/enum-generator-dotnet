using System.Collections.Generic;
using Xunit;

using EnumGenerator.Core.Utilities;

namespace EnumGenerator.Tests.Builder
{
    public sealed class IdentifierValidatorTests
    {
        public static IEnumerable<object[]> GetTestIdentifiersData()
        {
            foreach (var tuple in GetTestIdentifiers())
                yield return new object[] { tuple.identifier, tuple.valid };
        }

        public static IEnumerable<object[]> GetNamespaceTestIdentifiersData()
        {
            foreach (var tuple in GetNamespaceTestIdentifiers())
                yield return new object[] { tuple.identifier, tuple.valid };
        }

        [Theory]
        [MemberData(nameof(GetTestIdentifiersData))]
        public void IdentifierIsValidatedAccordingToRules(string identifier, bool valid) =>
            Assert.Equal(valid, IdentifierValidator.Validate(identifier));

        [Theory]
        [MemberData(nameof(GetNamespaceTestIdentifiersData))]
        public void NamespaceIdentifierIsValidatedAccordingToRules(string identifier, bool valid) =>
            Assert.Equal(valid, IdentifierValidator.ValidateNamespace(identifier));

        private static IEnumerable<(string identifier, bool valid)> GetTestIdentifiers()
        {
            // Valid
            yield return ("Identifer", valid: true);
            yield return ("identifier", valid: true);
            yield return ("_identifier", valid: true);
            yield return ("_identifier_a", valid: true);
            yield return ("IdentifierA", valid: true);
            yield return ("IDENTIFIER", valid: true);

            // Invalid
            yield return (string.Empty, valid: false);
            yield return (" ", valid: false);
            yield return (" identifier", valid: false);
            yield return ("identifier ", valid: false);
            yield return ("identifier a", valid: false);
            yield return ("0identifier", valid: false);
            yield return ("@identifier", valid: false);
            yield return ("!identifier", valid: false);
            yield return ("$identifier", valid: false);
            yield return ("%identifier", valid: false);
            yield return ("identifier.a", valid: false);
        }

        private static IEnumerable<(string identifier, bool valid)> GetNamespaceTestIdentifiers()
        {
            // Valid
            yield return ("A", valid: true);
            yield return ("A.B", valid: true);
            yield return ("A.B.C", valid: true);

            // Invalid
            yield return (string.Empty, valid: false);
            yield return (" ", valid: false);
            yield return (".A", valid: false);
            yield return (".A.", valid: false);
            yield return ("A.", valid: false);
            yield return ("A..", valid: false);
            yield return ("A..B", valid: false);
            yield return ("A.B..C", valid: false);
        }
    }
}
