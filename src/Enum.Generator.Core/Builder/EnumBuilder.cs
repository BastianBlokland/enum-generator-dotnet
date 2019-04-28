using System;
using System.Collections.Immutable;
using System.Linq;

using Enum.Generator.Core.Definition;
using Enum.Generator.Core.Utilities;

namespace Enum.Generator.Core.Builder
{
    /// <summary>
    /// Builder for creating a enum-definition.
    /// </summary>
    public sealed class EnumBuilder
    {
        private readonly ImmutableArray<EnumEntry>.Builder entries = ImmutableArray.CreateBuilder<EnumEntry>();
        private readonly string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumBuilder"/> class
        /// </summary>
        /// <exception cref="Exceptions.InvalidEnumNameException">
        /// Thrown when name is not a valid identifier.
        /// </exception>
        /// <param name="name">Name of the enum</param>
        public EnumBuilder(string name)
        {
            if (!IdentifierValidator.Validate(name))
                throw new Exceptions.InvalidEnumNameException(this.name);

            this.name = name;
        }

        /// <summary>
        /// Optional comment about this enum.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Add a entry to the enum.
        /// </summary>
        /// <exception cref="Exceptions.InvalidEnumEntryNameException">
        /// Thrown when name is not a valid identifier.
        /// </exception>
        /// <exception cref="Exceptions.DuplicateEnumEntryNameException">
        /// Thrown when name is not unique.
        /// </exception>
        /// <exception cref="Exceptions.DuplicateEnumEntryValueException">
        /// Thrown when value is not unique.
        /// </exception>
        /// <param name="name">Name of the entry</param>
        /// <param name="value">Value of the entry</param>
        /// <param name="comment">Optional comment about the entry</param>
        public void PushEntry(string name, int value, string comment = null)
        {
            if (!IdentifierValidator.Validate(name))
                throw new Exceptions.InvalidEnumEntryNameException(this.name, name);

            if (this.entries.Any(e => e.Name == name))
                throw new Exceptions.DuplicateEnumEntryNameException(this.name, name);

            if (this.entries.Any(e => e.Value == value))
                throw new Exceptions.DuplicateEnumEntryValueException(this.name, value);

            this.entries.Add(new EnumEntry(name, value, comment));
        }

        /// <summary>
        /// Build a immutable <see cref="EnumDefinition"/> from the current state of the builder.
        /// </summary>
        /// <exception cref="Exceptions.EmptyEnumException">
        /// Thrown when enum has no values.
        /// </exception>
        /// <returns>Newly created immutable enum-definition</returns>
        public EnumDefinition Build()
        {
            if (this.entries.Count == 0)
                throw new Exceptions.EmptyEnumException(this.name);

            return new EnumDefinition(this.name, this.entries.ToImmutableArray(), this.Comment);
        }
    }
}
