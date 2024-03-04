# Benchmarking using `dotnet-trace`

Official documentation on `dotnet-trace` can be found at
https://learn.microsoft.com/en-us/dotnet/core/diagnostics/dotnet-trace

## Collect trace data

Build the benchmarking app in the `Release` configuration before collecting trace data.

Ensure environment variables referenced in the pipeline definition are set.

Then run the app with the `dotnet-trace` tool to collect trace data:

```bash
dotnet-trace collect --format Speedscope -- bin/Release/net6.0/ConsoleAppTestUtility ReceiverTest simple
```

This command will start the application, collect trace data,
and produce a trace file in the `Speedscope` format.

`ReceiverTest` is the name of the benchmark to run.
If `simple` is specified, the benchmark will make a single call to `Pipeline.run()`,
relying on the pipeline performing enough work internally to make the benchmark meaningful.

In case of `Receiver`, you can use `MockReceiveCount` to specify the number of times
the mock message will be received. The default value is 1.

## Analyze trace data

You can use the `Speedscope` tool to analyze the trace data.
Open https://www.speedscope.app, then drag and drop the trace file
to the web page. The tool will display the trace data in a flame graph.
