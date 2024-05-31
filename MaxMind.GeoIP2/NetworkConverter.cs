/******************************************************************
 * File: NetworkConverter.cs
 * Copyright (c) 2024 Vyacheslav Kotrachev
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 ******************************************************************/

using MaxMind.Db;
using System;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MaxMind.GeoIP2
{
    internal class NetworkConverter : JsonConverter<Network?>
    {
        public override void Write(Utf8JsonWriter writer, Network? value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value?.ToString());
        }

        public override Network? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            if (value == null)
            {
                return null;
            }
            // It'd probably be nice if we added an appropriate constructor
            // to Network.
            var parts = value.Split('/');
            if (parts.Length != 2 || !int.TryParse(parts[1], out var prefixLength))
            {
                throw new JsonException("Network not in CIDR format: " + value);
            }

            return new Network(IPAddress.Parse(parts[0]), prefixLength);
        }
    }
}
