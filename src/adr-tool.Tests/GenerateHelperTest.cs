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
    [DataRow("C:\\Temp\\TestFiles\\ValidTitle.md", "Sample Title")]
    [DataRow("C:\\Temp\\TestFiles\\NoTitle.md", "No title found")]
    [DataRow("C:\\Temp\\TestFiles\\InvalidFormat.md",
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
      Directory.CreateDirectory("C:\\Temp\\TestFiles");

      File.WriteAllText("C:\\Temp\\TestFiles\\ValidTitle.md",
        "# 1. Sample Title");
      File.WriteAllText("C:\\Temp\\TestFiles\\NoTitle.md",
        "This file has no title");
      File.WriteAllText("C:\\Temp\\TestFiles\\InvalidFormat.md",
        "# Sample Title without number");
    }

    [TestCleanup]
    public void Cleanup()
    {
      // Clean up test files
      Directory.Delete("C:\\Temp\\TestFiles", true);
    }
  }
}
