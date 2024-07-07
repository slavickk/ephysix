using System.Threading.Tasks;
using NUnit.Framework;

namespace ParserLibrary.Tests;

[TestFixture]
public class PipelineTests
{
    // NOTE: DummySender is the ParserLibrary.DummySender (Sender-based),
    // and FileReceiver is the Plugins.FileReceiver (IReceiver-based, in the Plugins assembly)
    // TODO: rework the plugin system for consistency (either keep it IReceiver/ISender-based, or Receiver/Sender-based)
    private readonly string? _pipelineDefinition = @"
pipelineDescription: Pipeline example
steps:
- isBridge: true
  isHandleSenderError: true
  saveErrorSendDirectory: Data/Restore
  iDStep: Step_0
  iDPreviousStep: ''
  iDResponsedReceiverStep: ''
  description: Check registration extract
  ireceiver: !Plugins.FileReceiver
    fileName: input/items_file.txt
  sender: !FileSender
    saveDirectory: output
";

    private readonly string[] _inputJsonDocs = {
        @"{""id"": 1, ""name"": ""John""}",
        @"{""id"": 2, ""name"": ""Jane""}"
    };
    
    [Test]
    public void TestPipelineDeserialization()
    {
        // Arrange
        var pipeline = Pipeline.loadFromString(_pipelineDefinition, typeof(Plugins.FileReceiver).Assembly);

        // Act
        // NONE, just checking that the pipeline is deserialized correctly

        // Assert
        Assert.NotNull(pipeline.steps);
        Assert.AreEqual(1, pipeline.steps.Length);
        Assert.AreEqual("Step_0", pipeline.steps[0].IDStep);
        
        // check the receiver and sender
        Assert.IsInstanceOf<Plugins.FileReceiver>(pipeline.steps[0].ireceiver);
        Assert.IsInstanceOf<FileSender>(pipeline.steps[0].sender);
    }
    
    [OneTimeSetUp]
    public void Setup()
    {
        // Create the input directory and a file
        System.IO.Directory.CreateDirectory("input");
        System.IO.File.WriteAllLines("input/items_file.txt", _inputJsonDocs);
        
        // Clean up the output directory
        if (System.IO.Directory.Exists("output")) 
            System.IO.Directory.Delete("output", true);
    }

    [Test]
    public async Task TestPipelineExecution()
    {
        // Arrange
        var pipeline = Pipeline.loadFromString(_pipelineDefinition, typeof(Plugins.FileReceiver).Assembly);

        // Act
        await pipeline.run();

        // Assert
        
        Assert.IsTrue(System.IO.Directory.Exists("output"));
        
        Assert.IsTrue(System.IO.File.Exists("output/1.json"));
        var output1 = System.IO.File.ReadAllText("output/1.json");
        Assert.AreEqual(@"{""id"": 1, ""name"": ""John""}", output1);
        
        Assert.IsTrue(System.IO.File.Exists("output/2.json"));
        var output2 = System.IO.File.ReadAllText("output/2.json");
        Assert.AreEqual(@"{""id"": 2, ""name"": ""Jane""}", output2);
    }
}
