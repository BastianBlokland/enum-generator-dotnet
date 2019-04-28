using System.Collections.Generic;
using Xunit;

using Enum.Generator.Core.Utilities;

namespace Enum.Generator.Tests.Builder
{
    public sealed class IdentifierValidatorTests
    {
        public static IEnumerable<object[]> GetTestIdentifiersData()
        {
            foreach (var tuple in GetTestIdentifiers())
                yield return new object[] { tuple.identifier, tuple.valid };
        }

        [Theory]
        [MemberData(nameof(GetTestIdentifiersData))]
        public void IdentifierIsValidatedAccordingToRules(string identifier, bool valid) =>
            Assert.Equal(valid, IdentifierValidator.Validate(identifier));

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
        }
    }
}
