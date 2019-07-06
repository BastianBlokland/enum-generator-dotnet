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

        /// <summary>
        /// Get the csharp keyword for the given storage-type.
        /// </summary>
        /// <param name="storageType">Storage-type to get the keyword for</param>
        /// <returns>Csharp keyword for the given storage-type</returns>
        public static string GetKeyword(this StorageType storageType)
        {
            switch (storageType)
            {
                case StorageType.Byte:
                    return "byte";
                case StorageType.Sbyte:
                    return "sbyte";
                case StorageType.Short:
                    return "short";
                case StorageType.Ushort:
                    return "ushort";
                case StorageType.Int:
                    return "int";
                case StorageType.Uint:
                    return "uint";
                case StorageType.Long:
                    return "long";
                case StorageType.Ulong:
                    return "ulong";
                default:
                    throw new ArgumentException($"Storage-type: '{storageType}' has no keyword", nameof(storageType));
            }
        }
    }
}
