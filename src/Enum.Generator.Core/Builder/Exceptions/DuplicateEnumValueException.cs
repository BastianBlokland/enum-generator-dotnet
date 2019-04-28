using System;

namespace Enum.Generator.Core.Builder.Exceptions
{
    /// <summary>
    /// Exception for when a enum value conflicts with another enum value.
    /// </summary>
    public sealed class DuplicateEnumValueException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateEnumValueException"/> class
        /// </summary>
        /// <param name="enumName">Name of the enum with the duplicated value</param>
        /// <param name="value">Value that is duplicated</param>
        public DuplicateEnumValueException(string enumName, int value)
            : base(message: $"Enum '{enumName}' has duplicated value: '{value}'")
        {
            this.EnumName = enumName;
            this.Value = value;
        }

        /// <summary>
        /// Name of the enum with the duplicated value.
        /// </summary>
        public string EnumName { get; }

        /// <summary>
        /// Value that was duplicated.
        /// </summary>
        public int Value { get; }
    }
}
