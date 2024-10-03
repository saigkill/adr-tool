using System.IO;

using adr;

using JetBrains.Annotations;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;

namespace adr_tool.Tests
{
  [TestClass]
  [TestSubject(typeof(AdrSettings))]
  public class AdrSettingsTest
  {
    [TestMethod]
    public void TestCurrent_NotNull()
    {
      // Arrange & Act
      var settings = AdrSettings.Current;

      // Assert
      Assert.IsNotNull(settings);
    }

    [TestMethod]
    public void TestCurrent_Singleton()
    {
      // Arrange & Act
      var settings1 = AdrSettings.Current;
      var settings2 = AdrSettings.Current;

      // Assert
      Assert.AreSame(settings1, settings2);
    }

    [TestMethod]
    public void TestWrite_CreatesFile()
    {
      // Arrange
      var settings = AdrSettings.Current;
      settings.DocFolder = "test_docs";
      settings.TemplateFolder = "test_templates";

      // Act
      settings.Write();

      // Assert
      Assert.IsTrue(File.Exists("adr.config.json"));
    }

    [TestMethod]
    public void TestWrite_FileContents()
    {
      // Arrange
      var settings = AdrSettings.Current;
      settings.DocFolder = "test_docs";
      settings.TemplateFolder = "test_templates";

      // Act
      settings.Write();

      // Assert
      var json = File.ReadAllText("adr.config.json");
      dynamic value = JsonConvert.DeserializeObject(json);
      Assert.AreEqual("test_docs", (string)value.path);
      Assert.AreEqual("test_templates", (string)value.templates);
    }

    [TestMethod]
    public void TestRead_FileExists()
    {
      // Arrange
      var json = "{\"path\":\"existing_docs\",\"templates\":\"existing_templates\"}";
      File.WriteAllText("adr.config.json", json);

      // Act
      var settings = AdrSettings.Current;

      // Assert
      Assert.AreEqual("existing_docs", settings.DocFolder);
    }

    [TestMethod]
    public void TestDocFolder_SetAndGet()
    {
      // Arrange
      var settings = AdrSettings.Current;
      var expected = "new_docs";

      // Act
      settings.DocFolder = expected;

      // Assert
      Assert.AreEqual(expected, settings.DocFolder);
    }

    [TestMethod]
    public void TestTemplateFolder_SetAndGet()
    {
      // Arrange
      var settings = AdrSettings.Current;
      var expected = "new_templates";

      // Act
      settings.TemplateFolder = expected;

      // Assert
      Assert.AreEqual(expected, settings.TemplateFolder);
    }
  }
}
