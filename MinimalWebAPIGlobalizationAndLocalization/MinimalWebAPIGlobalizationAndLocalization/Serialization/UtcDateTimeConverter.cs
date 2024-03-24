﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace MinimalWebAPIGlobalizationAndLocalization.Serialization;

public class UtcDateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.GetDateTime().ToUniversalTime();

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        => writer.WriteStringValue((value.Kind == DateTimeKind.Local ? value.ToUniversalTime() : value)
            .ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffff'Z'"));
}