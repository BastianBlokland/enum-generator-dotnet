using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Enum.Generator.Core.Definition;
using Enum.Generator.Core.Mapping.Exceptions;
using Enum.Generator.Core.Builder;
using Enum.Generator.Core.Utilities;

namespace Enum.Generator.Core.Mapping
{
    /// <summary>
    /// Utility for mapping json text to a enum-definition.
    /// </summary>
    public static class JsonMapper
    {
        /// <summary>
        /// Map an enum from json text.
        /// </summary>
        /// <exception cref="Exceptions.JsonParsingFailureException">
        /// Thrown when invalid json text is supplied.
        /// </exception>
        /// <exception cref="Exceptions.MappingFailureException">
        /// Thrown when a error occurs during mapping.
        /// </exception>
        /// <param name="context">Context to use during mapping</param>
        /// <param name="jsonText">Json text to parse enum from</param>
        /// <param name="enumName">Identifier to give to the enum we parse</param>
        /// <param name="enumComment">Optional comment to add to the enum</param>
        /// <returns>Newly created immutable enum definition</returns>
        public static EnumDefinition MapEnum(
            this Context context,
            string jsonText,
            string enumName,
            string enumComment = null)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (string.IsNullOrEmpty(jsonText))
                throw new ArgumentException($"Invalid json: '{jsonText}'", nameof(jsonText));
            if (string.IsNullOrEmpty(enumName))
                throw new ArgumentException($"Invalid enumName: '{enumName}'", nameof(enumName));

            // Deserialize the json.
            JToken jsonToken;
            try
            {
                jsonToken = JsonConvert.DeserializeObject<JToken>(jsonText);
            }
            catch (Exception e)
            {
                throw new JsonParsingFailureException(e);
            }

            return context.MapEnum(jsonToken, enumName, enumComment);
        }

        private static EnumDefinition MapEnum(
            this Context context,
            JToken jsonToken,
            string enumName,
            string enumComment)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (jsonToken == null)
                throw new ArgumentNullException(nameof(jsonToken));
            if (string.IsNullOrEmpty(enumName))
                throw new ArgumentException($"Invalid enumName: '{enumName}'", nameof(enumName));

            try
            {
                var builder = new EnumBuilder(enumName);
                builder.Comment = enumComment;

                var tokens = jsonToken.SelectTokens(context.CollectionJPath);
                if (tokens == null || !tokens.Any())
                {
                    context.Logger?.LogWarning($"No tokens found at JPath: '{context.CollectionJPath}'");
                }
                else
                {
                    foreach (var token in tokens)
                        context.MapEntry(builder, token);
                }

                context.Logger?.LogDebug($"Enum '{enumName}' with '{builder.EntryCount}' entries mapped");
                return builder.Build();
            }
            catch (Exception e)
            {
                throw new MappingFailureException(e);
            }
        }

        private static void MapEntry(this Context context, EnumBuilder builder, JToken entryToken)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            if (entryToken == null)
                throw new ArgumentNullException(nameof(entryToken));

            var parsedName = ParseName();
            if (string.IsNullOrEmpty(parsedName))
            {
                context.Logger?.LogTrace("Skipping entry without name");
                return;
            }

            context.Logger?.LogDebug($"Parsed entry-name: {parsedName}");

            var parsedValue = ParseValue();
            context.Logger?.LogDebug($"Parsed entry-value: {parsedValue}");

            var parsedComment = ParseComment();
            if (!string.IsNullOrEmpty(parsedComment))
                context.Logger?.LogDebug($"Parsed entry-comment: {parsedComment}");

            builder.PushEntry(parsedName, parsedValue, parsedComment);

            string ParseName()
            {
                var nameToken = entryToken.SelectToken(context.EntryNameJPath);
                if (nameToken == null)
                {
                    context.Logger?.LogError($"Unable to find entry-name at path: '{context.EntryNameJPath}'");
                    return null;
                }

                var name = nameToken.Value<string>();
                context.Logger?.LogTrace($"Entry name-token found: '{name}'");

                // Create identifier out of the name.
                if (!IdentifierCreator.TryCreateIdentifier(name, out var nameId))
                {
                    context.Logger?.LogError($"Unable to convert name '{name}' into a valid identifier");
                    return null;
                }

                // Unduplicate name.
                var duplicationCount = 1;
                var unduplicatedName = nameId;
                while (builder.HasEntry(unduplicatedName))
                {
                    context.Logger?.LogTrace($"Duplicate name '{unduplicatedName}', adding number to unduplicate");
                    unduplicatedName = $"{nameId}_{duplicationCount++}";
                }

                return unduplicatedName;
            }

            int ParseValue()
            {
                var valueToken = string.IsNullOrEmpty(context.EntryValueJPath) ?
                    null :
                    entryToken.SelectToken(context.EntryValueJPath);
                var value = valueToken?.Value<int?>();
                if (value == null)
                    context.Logger?.LogTrace($"No value found at: '{context.EntryValueJPath}' using count as value");

                return value ?? builder.EntryCount;
            }

            string ParseComment()
            {
                var commentToken = string.IsNullOrEmpty(context.EntryCommentJPath) ?
                    null :
                    entryToken.SelectToken(context.EntryCommentJPath);
                return commentToken?.Value<string>();
            }
        }
    }
}
