using System.IO;

using JetBrains.Annotations;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace adr_tool.Tests;

[TestClass]
[TestSubject(typeof(GenerateHelper))]
public class GenerateHelperTest
{
  private readonly string _tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

  [TestMethod]
  public void GetTitle_TestCases(string filePath, string expectedTitle)
  {
    // Arrange
    // Act
    string[] fileNames = ["ValidTitle.md", "NoTitle.md", "InvalidFormat.md"];
    foreach (string fileName in fileNames)
    {
      GenerateHelper.GetTitle(fileName);
      File.Move(fileName, Path.Combine(_tempDirectory, fileName));
    }
  }

  [TestInitialize]
  public void Setup()
  {
    // Create test files
    Directory.CreateDirectory(_tempDirectory);

    File.WriteAllText(Path.Combine(_tempDirectory, "ValidTitle.md"),
      "# 1. Sample Title");
    File.WriteAllText(Path.Combine(_tempDirectory, "NoTitle.md"),
      "This file has no title");
    File.WriteAllText(Path.Combine(_tempDirectory, "InvalidFormat.md"),
      "# Sample Title without number");
  }

  [TestCleanup]
  public void Cleanup()
  {
    // Clean up test files
    Directory.Delete(_tempDirectory, true);
  }
}
