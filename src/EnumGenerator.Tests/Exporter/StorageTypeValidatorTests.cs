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
            yield return (StorageType.Byte, byte.MinValue, valid: true);
            yield return (StorageType.Sbyte, sbyte.MinValue, valid: true);
            yield return (StorageType.Short, short.MinValue, valid: true);
            yield return (StorageType.Ushort, ushort.MinValue, valid: true);
            yield return (StorageType.Int, int.MinValue, valid: true);
            yield return (StorageType.Uint, uint.MinValue, valid: true);
            yield return (StorageType.Long, long.MinValue, valid: true);
            yield return (StorageType.Ulong, 0, valid: true);

            // Max values are valid.
            yield return (StorageType.Implicit, int.MaxValue, valid: true);
            yield return (StorageType.Byte, byte.MaxValue, valid: true);
            yield return (StorageType.Sbyte, sbyte.MaxValue, valid: true);
            yield return (StorageType.Short, short.MaxValue, valid: true);
            yield return (StorageType.Ushort, ushort.MaxValue, valid: true);
            yield return (StorageType.Int, int.MaxValue, valid: true);
            yield return (StorageType.Uint, uint.MaxValue, valid: true);
            yield return (StorageType.Long, long.MaxValue, valid: true);
            yield return (StorageType.Ulong, long.MaxValue, valid: true);

            // Too small is invalid.
            yield return (StorageType.Implicit, long.MinValue, valid: false);
            yield return (StorageType.Byte, long.MinValue, valid: false);
            yield return (StorageType.Sbyte, long.MinValue, valid: false);
            yield return (StorageType.Short, long.MinValue, valid: false);
            yield return (StorageType.Ushort, long.MinValue, valid: false);
            yield return (StorageType.Int, long.MinValue, valid: false);
            yield return (StorageType.Uint, long.MinValue, valid: false);
            yield return (StorageType.Ulong, long.MinValue, valid: false);

            // Too big in invalid.
            yield return (StorageType.Implicit, long.MaxValue, valid: false);
            yield return (StorageType.Byte, long.MaxValue, valid: false);
            yield return (StorageType.Sbyte, long.MaxValue, valid: false);
            yield return (StorageType.Short, long.MaxValue, valid: false);
            yield return (StorageType.Ushort, long.MaxValue, valid: false);
            yield return (StorageType.Int, long.MaxValue, valid: false);
            yield return (StorageType.Uint, long.MaxValue, valid: false);
        }
    }
}
