using System.Collections.Generic;
using Xunit;

using EnumGenerator.Core.Exporter;

namespace EnumGenerator.Tests.Builder
{
    public sealed class StorageTypeValidatorTests
    {
        public static IEnumerable<object[]> GetTestValuesData()
        {
            foreach (var tuple in GetTestValues())
                yield return new object[] { tuple.type, tuple.value, tuple.valid };
        }

        [Theory]
        [MemberData(nameof(GetTestValuesData))]
        public void ValueIsValidatedCorrectly(StorageType type, long value, bool valid) =>
            Assert.Equal(valid, StorageTypeValidator.Validate(type, value));

        private static IEnumerable<(StorageType type, long value, bool valid)> GetTestValues()
        {
            // Min values are valid.
            yield return (StorageType.Implicit, int.MinValue, valid: true);
            yield return (StorageType.Unsigned8Bit, byte.MinValue, valid: true);
            yield return (StorageType.Signed8Bit, sbyte.MinValue, valid: true);
            yield return (StorageType.Signed16Bit, short.MinValue, valid: true);
            yield return (StorageType.Unsigned16Bit, ushort.MinValue, valid: true);
            yield return (StorageType.Signed32Bit, int.MinValue, valid: true);
            yield return (StorageType.Unsigned32Bit, uint.MinValue, valid: true);
            yield return (StorageType.Signed64Bit, long.MinValue, valid: true);
            yield return (StorageType.Unsigned64Bit, 0, valid: true);

            // Max values are valid.
            yield return (StorageType.Implicit, int.MaxValue, valid: true);
            yield return (StorageType.Unsigned8Bit, byte.MaxValue, valid: true);
            yield return (StorageType.Signed8Bit, sbyte.MaxValue, valid: true);
            yield return (StorageType.Signed16Bit, short.MaxValue, valid: true);
            yield return (StorageType.Unsigned16Bit, ushort.MaxValue, valid: true);
            yield return (StorageType.Signed32Bit, int.MaxValue, valid: true);
            yield return (StorageType.Unsigned32Bit, uint.MaxValue, valid: true);
            yield return (StorageType.Signed64Bit, long.MaxValue, valid: true);
            yield return (StorageType.Unsigned64Bit, long.MaxValue, valid: true);

            // Too small is invalid.
            yield return (StorageType.Implicit, long.MinValue, valid: false);
            yield return (StorageType.Unsigned8Bit, long.MinValue, valid: false);
            yield return (StorageType.Signed8Bit, long.MinValue, valid: false);
            yield return (StorageType.Signed16Bit, long.MinValue, valid: false);
            yield return (StorageType.Unsigned16Bit, long.MinValue, valid: false);
            yield return (StorageType.Signed32Bit, long.MinValue, valid: false);
            yield return (StorageType.Unsigned32Bit, long.MinValue, valid: false);
            yield return (StorageType.Unsigned64Bit, long.MinValue, valid: false);

            // Too big in invalid.
            yield return (StorageType.Implicit, long.MaxValue, valid: false);
            yield return (StorageType.Unsigned8Bit, long.MaxValue, valid: false);
            yield return (StorageType.Signed8Bit, long.MaxValue, valid: false);
            yield return (StorageType.Signed16Bit, long.MaxValue, valid: false);
            yield return (StorageType.Unsigned16Bit, long.MaxValue, valid: false);
            yield return (StorageType.Signed32Bit, long.MaxValue, valid: false);
            yield return (StorageType.Unsigned32Bit, long.MaxValue, valid: false);
        }
    }
}
