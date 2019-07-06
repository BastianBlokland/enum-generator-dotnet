using System;

namespace EnumGenerator.Core.Exporter
{
    /// <summary>
    /// Utilities for validating enum storage type.
    /// </summary>
    public static class StorageTypeValidator
    {
        /// <summary>
        /// Validate if given value can be represented with the given storage-type.
        /// </summary>
        /// <remarks>
        /// Value's larger then long.MaxValue are currently not supported as internally long's are
        /// used to represent the values.
        /// </remarks>
        /// <param name="storageType">Type to validate against</param>
        /// <param name="value">Value to validate</param>
        /// <returns>'True' if valid, otherwise 'False'</returns>
        public static bool Validate(this StorageType storageType, long value) =>
            value >= GetMinSupportedValue(storageType) &&
            value <= GetMaxSupportedValue(storageType);

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
                case StorageType.Byte:
                    return byte.MinValue;
                case StorageType.Sbyte:
                    return sbyte.MinValue;
                case StorageType.Short:
                    return short.MinValue;
                case StorageType.Ushort:
                    return ushort.MinValue;
                case StorageType.Int:
                    return int.MinValue;
                case StorageType.Uint:
                    return uint.MinValue;
                case StorageType.Long:
                    return long.MinValue;
                case StorageType.Ulong:
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
                case StorageType.Byte:
                    return byte.MaxValue;
                case StorageType.Sbyte:
                    return sbyte.MaxValue;
                case StorageType.Short:
                    return short.MaxValue;
                case StorageType.Ushort:
                    return ushort.MaxValue;
                case StorageType.Int:
                    return int.MaxValue;
                case StorageType.Uint:
                    return uint.MaxValue;
                case StorageType.Long:
                    return long.MaxValue;
                case StorageType.Ulong:
                    /*  Note: We do not support numbers larger then long.MaxValue as we internally use
                    long's to represent the values. */
                    return long.MaxValue;
                default:
                    throw new ArgumentException($"Unsupported storage-type: '{storageType}'", nameof(storageType));
            }
        }
    }
}
