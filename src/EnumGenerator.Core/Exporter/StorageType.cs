namespace EnumGenerator.Core.Exporter
{
    /// <summary>
    /// Underlying enum storage type to generate.
    /// </summary>
    public enum StorageType
    {
        /// <summary>
        /// Don't output a specific storage type. (This will let the c# compiler decide)
        /// </summary>
        Implicit = 0,

        /// <summary>
        /// Specify 'byte' as the underlying storage type for the enum.
        /// </summary>
        Byte = 1,

        /// <summary>
        /// Specify 'sbyte' as the underlying storage type for the enum.
        /// </summary>
        Sbyte = 2,

        /// <summary>
        /// Specify 'short' as the underlying storage type for the enum.
        /// </summary>
        Short = 3,

        /// <summary>
        /// Specify 'ushort' as the underlying storage type for the enum.
        /// </summary>
        Ushort = 4,

        /// <summary>
        /// Specify 'int' as the underlying storage type for the enum.
        /// </summary>
        Int = 5,

        /// <summary>
        /// Specify 'uint' as the underlying storage type for the enum.
        /// </summary>
        Uint = 6,

        /// <summary>
        /// Specify 'long' as the underlying storage type for the enum.
        /// </summary>
        Long = 7,

        /// <summary>
        /// Specify 'ulong' as the underlying storage type for the enum.
        /// </summary>
        Ulong = 8
    }
}
