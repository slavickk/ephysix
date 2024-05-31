/******************************************************************
 * File: TICBench.cs
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

using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using CCFAProtocols.TIC;

namespace CCFAProtocols.Bench;

[SimpleJob(RuntimeMoniker.Net60)]
[CategoriesColumn]
[BenchmarkCategory("TIC")]
[AllStatisticsColumn]
public class TICBench
{
    private static byte[] _ticMessageBytes;

    // private static TICMessage _ticMessageObject;
    private static string _ticMessageJson;

    [Benchmark]
    [ArgumentsSource(nameof(TICinBytes))]
    public TICMessage Deserialize(Argument<byte[]> bytes) => TICMessage.Deserialize(bytes.Get());

    [Benchmark]
    [ArgumentsSource(nameof(TICasObject))]
    public byte[] Serialize(TICMessage msg) => msg.Serialize();

    [Benchmark]
    public string DeserializeToJson() => TICMessage.DeserializeToJSON(_ticMessageBytes);

    [Benchmark]
    public byte[] SerializeFromJson() => TICMessage.SerializeFromJson(_ticMessageJson);

    [GlobalSetup]
    public void Setup()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        _ticMessageBytes = File.ReadAllBytes("Data/test200.tic")[2..];
        // _ticMessageObject = TICMessage.Deserialize(_ticMessageBytes);
        _ticMessageJson = TICMessage.DeserializeToJSON(_ticMessageBytes);
    }

    public IEnumerable<Argument<byte[]>> TICinBytes()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        yield return new Argument<byte[]>(File.ReadAllBytes("Data/test200.tic")[2..], "tic 200");
        yield return new Argument<byte[]>(TICMessage.EchoRequest.Serialize(), "echo request");
        yield return new Argument<byte[]>(TICMessage.EchoResponse.Serialize(), "echo response");
    }

    public IEnumerable<Argument<TICMessage>> TICasObject()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        yield return new Argument<TICMessage>(TICMessage.Deserialize(File.ReadAllBytes("Data/test200.tic")[2..]),
            "tic 200");
        yield return new Argument<TICMessage>(TICMessage.EchoRequest, "echo request");
        yield return new Argument<TICMessage>(TICMessage.EchoResponse, "echo response");
    }
}