using System.IO;

using JetBrains.Annotations;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace adr_tool.Tests
{
  [TestClass]
  [TestSubject(typeof(GenerateHelper))]
  public class GenerateHelperTest
  {
    [TestMethod]
    [DataRow("C:\\Users\\sasch\\source\\repos\\adr-tool\\adr-tool.Tests\\TestFiles\\ValidTitle.md", "Sample Title")]
    [DataRow("C:\\Users\\sasch\\source\\repos\\adr-tool\\adr-tool.Tests\\TestFiles\\NoTitle.md", "No title found")]
    [DataRow("C:\\Users\\sasch\\source\\repos\\adr-tool\\adr-tool.Tests\\TestFiles\\InvalidFormat.md",
      "No title found")]
    public void GetTitle_TestCases(string filePath, string expectedTitle)
    {
      // Arrange
      // Act
      var result = GenerateHelper.GetTitle(filePath);

      // Assert
      Assert.AreEqual(expectedTitle, result);
    }

    [TestInitialize]
    public void Setup()
    {
      // Create test files
      Directory.CreateDirectory("C:\\Users\\sasch\\source\\repos\\adr-tool\\adr-tool.Tests\\TestFiles");

      File.WriteAllText("C:\\Users\\sasch\\source\\repos\\adr-tool\\adr-tool.Tests\\TestFiles\\ValidTitle.md",
        "# 1. Sample Title");
      File.WriteAllText("C:\\Users\\sasch\\source\\repos\\adr-tool\\adr-tool.Tests\\TestFiles\\NoTitle.md",
        "This file has no title");
      File.WriteAllText("C:\\Users\\sasch\\source\\repos\\adr-tool\\adr-tool.Tests\\TestFiles\\InvalidFormat.md",
        "# Sample Title without number");
    }

    [TestCleanup]
    public void Cleanup()
    {
      // Clean up test files
      Directory.Delete("C:\\Users\\sasch\\source\\repos\\adr-tool\\adr-tool.Tests\\TestFiles", true);
    }
  }
}
