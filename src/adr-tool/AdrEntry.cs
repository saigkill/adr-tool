using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;

using adr_tool.Common;

namespace adr_tool;

internal class AdrEntry
{
  private readonly string _docFolder;

  //private readonly string _templatePath;

  private readonly TemplateType _templateType;

  private string _fileName;

  public AdrEntry(TemplateType templateType)
  {
    this._docFolder = AdrSettings.Current.DocFolder;
    this._templateType = templateType;
    //this._templatePath = $"{AdrSettings.Current.TemplateFolder}\\{templateType.ToString()}.md";
    this._fileName = string.Empty;
  }

  public string Title { get; set; } = "Record Architecture Decisions";
  public string[] SupersededLinks { get; set; } = new string[0];
  public string[] AdditionalLinks { get; set; } = new string[0];

  public AdrEntry Write()
  {
    if (this._templateType == TemplateType.Adr)
    {
      this.WriteAdr();
    }
    else
    {
      this.WriteNew();
    }

    return this;
  }

  private void WriteNew()
  {
    var fileNumber = Directory.Exists(this._docFolder)
      ? GetNextFileNumber(this._docFolder)
      : 1;
    _fileName = Path.Join(
      _docFolder,
      $"{fileNumber.ToString(CultureInfo.InvariantCulture).PadLeft(4, '0')}-{SanitizeFileName(this.Title)}.md");

    CreateDocumentsFolderIfNotExists();

    WriteAdrFile(fileNumber);

    LinkSupersedes(SupersededLinks);

    LinksAdditional(AdditionalLinks);
  }

  private void WriteAdr()
  {
    var fileNumber = Directory.Exists(this._docFolder)
      ? GetNextFileNumber(this._docFolder)
      : 1;
    _fileName = Path.Join(
      this._docFolder,
      $"{fileNumber.ToString(CultureInfo.InvariantCulture).PadLeft(4, '0')}-{SanitizeFileName(this.Title)}.md");

    CreateDocumentsFolderIfNotExists();

    WriteInitialAdrFile(fileNumber);
  }

  private void LinkSupersedes(string[] links)
  {
    foreach (var link in links)
    {
      AddAdrLink.LinkAdr(link, "Superseded By", _fileName);
      AdrRemoveStatus.Remove("Accepted", link);
      AddAdrLink.LinkAdr(_fileName, "Supersedes", link);
    }
  }

  private void LinksAdditional(string[] links)
  {
    foreach (var link in links)
    {
      string[] parts = link.Split(':');
      if (parts.Length != 3)
      {
        Console.WriteLine("Invalid link format");
        continue;
      }
      string target = parts[0];
      string forwardLink = parts[1];
      string reverseLink = parts[2];

      AddAdrLink.LinkAdr(_fileName, forwardLink, target);
      AddAdrLink.LinkAdr(target, reverseLink, _fileName);
    }
  }

  private void WriteInitialAdrFile(int fileNumber)
  {
    using var writer = File.CreateText(this._fileName);
    writer.WriteLine($"# {fileNumber}. {this.Title}");
    writer.WriteLine();
    writer.WriteLine(DateTime.Today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
    writer.WriteLine();
    writer.WriteLine("## Status");
    writer.WriteLine();
    writer.WriteLine("Accepted");
    writer.WriteLine();
    writer.WriteLine("## Context");
    writer.WriteLine();
    writer.WriteLine("We need to record the architectural decisions made on this project.");
    writer.WriteLine();
    writer.WriteLine("## Decision");
    writer.WriteLine();
    writer.WriteLine("We will use Architecture Decision Records, as described by Michael Nygard in this article: http://thinkrelevance.com/blog/2011/11/15/documenting-architecture-decisions");
    writer.WriteLine();
    writer.WriteLine("## Consequences");
    writer.WriteLine();
    writer.WriteLine("See Michael Nygard's article, linked above.");

  }

  private void WriteAdrFile(int fileNumber)
  {
    using var writer = File.CreateText(_fileName);
    writer.WriteLine($"# {fileNumber}. {this.Title}");
    writer.WriteLine();
    writer.WriteLine(DateTime.Today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
    writer.WriteLine();
    writer.WriteLine("## Status");
    writer.WriteLine();
    writer.WriteLine("Proposed");
    writer.WriteLine();
    writer.WriteLine("## Context");
    writer.WriteLine();
    writer.WriteLine("{context}");
    writer.WriteLine();
    writer.WriteLine("## Decision");
    writer.WriteLine();
    writer.WriteLine("{decision}");
    writer.WriteLine();
    writer.WriteLine("## Consequences");
    writer.WriteLine();
    writer.WriteLine("{consequences}");

  }

  private void CreateDocumentsFolderIfNotExists()
  {
    if (!Directory.Exists(this._docFolder))
    {
      Directory.CreateDirectory(this._docFolder);
    }
  }

  public AdrEntry Launch()
  {
    try
    {
      Process.Start(this._fileName);
    }
    catch
    {
      // hack because of this: https://github.com/dotnet/corefx/issues/10361
      if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
      {
        var url = this._fileName.Replace("&", "^&");
        Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
      }
      else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
      {
        Process.Start("xdg-open", this._fileName);
      }
      else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
      {
        Process.Start("open", this._fileName);
      }
      else
      {
        throw;
      }
    }

    return this;
  }

  private static int GetNextFileNumber(string docFolder)
  {
    int fileNumOut = 0;
    var files =
      from file in new DirectoryInfo(docFolder).GetFiles("*.md", SearchOption.TopDirectoryOnly)
      let fileNum = file.Name.Substring(0, 4)
      where int.TryParse(fileNum, CultureInfo.InvariantCulture, out fileNumOut)
      select fileNumOut;
    var enumerable = files.ToList();
    var maxFileNum = enumerable.Any() ? enumerable.Max() : 0;
    return maxFileNum + 1;
  }

  private static string SanitizeFileName(string title)
  {
    return title
      .Replace(' ', '-')
      .ToLower(CultureInfo.InvariantCulture);
  }
}
