using System;

namespace EnumGenerator.Core.Exporter
{
    /// <summary>
    /// Extensions for the <see cref="StorageType"/> enum.
    /// </summary>
    public static class StorageTypeExtensions
    {
        /// <summary>
        /// Get the mimimum value that is supported by the given storage-type.
        /// </summary>
        /// <param name="storageType">Storage-type to get the min value for</param>
        /// <returns>Minimum supported value</returns>
        public static long GetMinSupportedValue(this StorageType storageType)
        {
            switch (storageType)
            {
                case StorageType.Implicit:
                    return int.MinValue;
                case StorageType.Unsigned8Bit:
                    return byte.MinValue;
                case StorageType.Signed8Bit:
                    return sbyte.MinValue;
                case StorageType.Signed16Bit:
                    return short.MinValue;
                case StorageType.Unsigned16Bit:
                    return ushort.MinValue;
                case StorageType.Signed32Bit:
                    return int.MinValue;
                case StorageType.Unsigned32Bit:
                    return uint.MinValue;
                case StorageType.Signed64Bit:
                    return long.MinValue;
                case StorageType.Unsigned64Bit:
                    return 0;
                default:
                    throw new ArgumentException($"Unsupported storage-type: '{storageType}'", nameof(storageType));
            }
        }

        /// <summary>
        /// Get the maximum value that is supported by the given storage-type.
        /// </summary>
        /// <remarks>
        /// Value's larger then long.MaxValue are currently not supported as internally long's are
        /// used to represent the values.
        /// </remarks>
        /// <param name="storageType">Storage-type to get the max value for</param>
        /// <returns>Maximum supported value</returns>
        public static long GetMaxSupportedValue(this StorageType storageType)
        {
            switch (storageType)
            {
                case StorageType.Implicit:
                    return int.MaxValue;
                case StorageType.Unsigned8Bit:
                    return byte.MaxValue;
                case StorageType.Signed8Bit:
                    return sbyte.MaxValue;
                case StorageType.Signed16Bit:
                    return short.MaxValue;
                case StorageType.Unsigned16Bit:
                    return ushort.MaxValue;
                case StorageType.Signed32Bit:
                    return int.MaxValue;
                case StorageType.Unsigned32Bit:
                    return uint.MaxValue;
                case StorageType.Signed64Bit:
                    return long.MaxValue;
                case StorageType.Unsigned64Bit:
                    /*  Note: We do not support numbers larger then long.MaxValue as we internally use
                    long's to represent the values. */
                    return long.MaxValue;
                default:
                    throw new ArgumentException($"Unsupported storage-type: '{storageType}'", nameof(storageType));
            }
        }

        /// <summary>
        /// Get the csharp keyword for the given storage-type.
        /// </summary>
        /// <param name="storageType">Storage-type to get the keyword for</param>
        /// <returns>Csharp keyword for the given storage-type</returns>
        public static string GetCSharpKeyword(this StorageType storageType)
        {
            switch (storageType)
            {
                case StorageType.Unsigned8Bit:
                    return "byte";
                case StorageType.Signed8Bit:
                    return "sbyte";
                case StorageType.Signed16Bit:
                    return "short";
                case StorageType.Unsigned16Bit:
                    return "ushort";
                case StorageType.Signed32Bit:
                    return "int";
                case StorageType.Unsigned32Bit:
                    return "uint";
                case StorageType.Signed64Bit:
                    return "long";
                case StorageType.Unsigned64Bit:
                    return "ulong";
                default:
                    throw new ArgumentException($"Storage-type: '{storageType}' has no keyword in csharp", nameof(storageType));
            }
        }
    }
}
