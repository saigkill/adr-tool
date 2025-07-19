// <copyright file="AdrEntryTest.cs" company="Sascha Manns">
// Copyright (c) 2025 Sascha Manns.
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the “Software”), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial
// portions of the Software.
// 
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN
// ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

using adr_tool;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace adr_tool.Tests;

[TestClass]
[TestSubject(typeof(AdrEntry))]
public sealed class AdrEntryTest
{
  [TestMethod]
  [DataRow(TemplateType.Adr, "Record Architecture Decisions", new string[] { },
    "docs\\adr\\0001-record-architecture-decisions.md")]
  [DataRow(TemplateType.New, "New Decision", new string[] { "Link1", "Link2" }, "docs\\adr\\0001-new-decision.md")]
  public void Write_ShouldCreateCorrectFile(TemplateType templateType, string title, string[] supersededLinks,
    string expectedFilePath)
  {
    // Arrange
    AdrSettings.Current.DocFolder = "docs\\adr";
    AdrSettings.Current.TemplateFolder = "templates";
    var adrEntry = new AdrEntry(templateType) { Title = title, SupersededLinks = supersededLinks };

    // Act
    adrEntry.Write();

    // Assert
    Assert.IsTrue(File.Exists(expectedFilePath));
    File.Delete(expectedFilePath); // Clean up
  }

  [TestMethod]
  [DataRow("Valid Title", "valid-title")]
  [DataRow("Title With Spaces", "title-with-spaces")]
  [DataRow("Special@Characters!", "special@characters!")]
  public void SanitizeFileName_ShouldReturnCorrectFileName(string input, string expected)
  {
    // Act
    var result = typeof(AdrEntry).GetMethod("SanitizeFileName",
        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
      .Invoke(null, new object[] { input });

    // Assert
    Assert.AreEqual(expected, result);
  }

  [TestMethod]
  [DataRow("docs\\adr", 1, new string[] { })]
  [DataRow("docs\\adr", 2, new string[] { "0001-existing.md" })]
  public void GetNextFileNumber_ShouldReturnCorrectNumber(string docFolder, int expected, string[] existingFiles)
  {
    // Arrange
    Directory.CreateDirectory(docFolder);
    foreach (var file in existingFiles)
    {
      File.Create(Path.Combine(docFolder, file)).Dispose();
    }

    // Act
    var result = typeof(AdrEntry).GetMethod("GetNextFileNumber",
        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
      .Invoke(null, new object[] { docFolder });

    // Assert
    Assert.AreEqual(expected, result);

    // Clean up
    Directory.Delete(docFolder, true);
  }

  [TestMethod]
  [DataRow(TemplateType.Adr, "docs\\adr\\0001-record-architecture-decisions.md")]
  [DataRow(TemplateType.New, "docs\\adr\\0001-new-decision.md")]
  public void Launch_ShouldOpenFile(TemplateType templateType, string expectedFilePath)
  {
    // Arrange
    AdrSettings.Current.DocFolder = "docs\\adr";
    AdrSettings.Current.TemplateFolder = "templates";
    var adrEntry = new AdrEntry(templateType);
    adrEntry.Write();

    // Act
    adrEntry.Launch();

    // Assert
    Assert.IsTrue(File.Exists(expectedFilePath));
    File.Delete(expectedFilePath); // Clean up
  }
}
