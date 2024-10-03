using System.IO;

using adr;

using JetBrains.Annotations;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace adr_tool.Tests;

[TestClass]
[TestSubject(typeof(AdrEntry))]
public class AdrEntryTest
{
  [TestMethod]
  [DataRow(TemplateType.Adr)]
  [DataRow(TemplateType.New)]
  public void AdrEntry_Constructor_ShouldInitializeProperties(TemplateType templateType)
  {
    // Arrange
    AdrSettings.Current.DocFolder = "testDocFolder";
    AdrSettings.Current.TemplateFolder = "testTemplateFolder";

    // Act
    var adrEntry = new AdrEntry(templateType);

    // Assert
    Assert.AreEqual("testDocFolder",
      adrEntry.GetType()
        .GetField("_docFolder", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
        .GetValue(adrEntry));
    Assert.AreEqual($"{AdrSettings.Current.TemplateFolder}\\{templateType.ToString()}.md",
      adrEntry.GetType().GetField("_templatePath",
        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(adrEntry));
    Assert.AreEqual(templateType,
      adrEntry.GetType().GetField("_templateType",
        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(adrEntry));
  }

  [TestMethod]
  public void AdrEntry_Launch_ShouldStartProcess()
  {
    // Arrange
    var adrEntry = new AdrEntry(TemplateType.Adr);
    adrEntry.GetType()
      .GetField("_fileName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
      .SetValue(adrEntry, "testFileName.md");

    // Act
    var result = adrEntry.Launch();

    // Assert
    Assert.IsNotNull(result);
  }

  [TestMethod]
  [DataRow("Test Title", "test-title")]
  [DataRow("Another Title", "another-title")]
  public void AdrEntry_SanitizeFileName_ShouldReturnSanitizedFileName(string title, string expected)
  {
    // Act
    var result = typeof(AdrEntry)
        .GetMethod("SanitizeFileName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
        .Invoke(null, new object[] { title });

    // Assert
    Assert.AreEqual(expected, result);
  }

  [TestMethod]
  public void AdrEntry_GetNextFileNumber_ShouldReturnNextFileNumber()
  {
    // Arrange
    var docFolder = "testDocFolder";
    Directory.CreateDirectory(docFolder);
    File.Create(Path.Combine(docFolder, "0001-test.md")).Dispose();

    // Act
    var result = typeof(AdrEntry)
        .GetMethod("GetNextFileNumber", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
        .Invoke(null, new object[] { docFolder });

    // Assert
    Assert.AreEqual(2, result);

    // Cleanup
    Directory.Delete(docFolder, true);
  }

  [TestMethod]
  public void AdrEntry_CreateDocumentsFolderIfNotExists_ShouldCreateFolder()
  {
    // Arrange
    var adrEntry = new AdrEntry(TemplateType.Adr);
    var docFolder = "testDocFolder";
    adrEntry.GetType()
      .GetField("_docFolder", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
      .SetValue(adrEntry, docFolder);

    // Act
    adrEntry.GetType()
      .GetMethod("CreateDocumentsFolderIfNotExists",
        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
      .Invoke(adrEntry, new object[] { });

    // Assert
    Assert.IsTrue(Directory.Exists(docFolder));

    // Cleanup
    Directory.Delete(docFolder, true);
  }
}
