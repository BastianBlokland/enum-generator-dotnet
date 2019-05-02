using System.Collections.Generic;
using Xunit;

using EnumGenerator.Core.Utilities;

namespace EnumGenerator.Tests.Builder
{
    public sealed class IdentifierCreatorTests
    {
        public static IEnumerable<object[]> GetTestIdentifiersData()
        {
            foreach (var tuple in GetTestIdentifiers())
                yield return new object[] { tuple.input, tuple.identifier };
        }

        [Theory]
        [MemberData(nameof(GetTestIdentifiersData))]
        public void ValidIdentifierIsGenerated(string input, string identifier)
        {
            Assert.True(IdentifierCreator.TryCreateIdentifier(input, out var generatedIdentifier));
            Assert.True(IdentifierValidator.Validate(generatedIdentifier));
            Assert.Equal(identifier, generatedIdentifier);
        }

        private static IEnumerable<(string input, string identifier)> GetTestIdentifiers()
        {
            yield return (input: "TestIdentifierInPascalCasing", identifier: "TestIdentifierInPascalCasing");
            yield return (input: "testIdentifierInCamelCasing", identifier: "TestIdentifierInCamelCasing");
            yield return (input: "test_identifier_in_snake_casing", identifier: "TestIdentifierInSnakeCasing");
            yield return (input: "test.identifier.with.dots", identifier: "TestIdentifierWithDots");
            yield return (input: "test-identifier-with-dashes", identifier: "TestIdentifierWithDashes");
            yield return (input: "test/identifier/with/slashes", identifier: "TestIdentifierWithSlashes");
            yield return (input: "test identifier with spaces", identifier: "TestIdentifierWithSpaces");
            yield return (input: "0 - Test", identifier: "_0Test");
            yield return (input: "Test0-Number", identifier: "Test0Number");
        }
    }
}
