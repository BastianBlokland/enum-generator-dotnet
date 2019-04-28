using System;

namespace Enum.Generator.Core.Builder.Exceptions
{
    /// <summary>
    /// Exception for when a enum has no values.
    /// </summary>
    public sealed class EmptyEnumException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyEnumException"/> class.
        /// </summary>
        /// <param name="enumName">Name of the enum that has no values</param>
        public EmptyEnumException(string enumName)
            : base(message: $"Enum '{enumName}' has no values")
        {
            this.EnumName = enumName;
        }

        /// <summary>
        /// Name of the enum that has no values.
        /// </summary>
        public string EnumName { get; }
    }
}
