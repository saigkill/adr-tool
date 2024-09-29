using System.Diagnostics;
using System.Runtime.InteropServices;

using adr_tool;

namespace adr
{
  internal class AdrEntry
  {
    private readonly string _docFolder;

    private readonly string _templatePath;

    private readonly TemplateType _templateType;

    private string _fileName;

    public AdrEntry(TemplateType templateType)
    {
      this._docFolder = AdrSettings.Current.DocFolder;
      this._templateType = templateType;
      this._templatePath = $"{AdrSettings.Current.TemplateFolder}\\{templateType.ToString()}.md";
    }

    public string Title { get; set; } = "Record Architecture Decisions";

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
      _fileName = Path.Combine(
          _docFolder,
          $"{fileNumber.ToString().PadLeft(4, '0')}-{SanitizeFileName(this.Title)}.md");

      CreateDocumentsFolderIfNotExists();

      WriteAdrFile(fileNumber);
    }

    private void WriteAdr()
    {
      var fileNumber = Directory.Exists(this._docFolder)
          ? GetNextFileNumber(this._docFolder)
          : 1;
      _fileName = Path.Combine(
          this._docFolder,
          $"{fileNumber.ToString().PadLeft(4, '0')}-{SanitizeFileName(this.Title)}.md");

      CreateDocumentsFolderIfNotExists();

      WriteInitialAdrFile(fileNumber);
    }

    private void WriteInitialAdrFile(int fileNumber)
    {
      using var writer = File.CreateText(this._fileName);
      {
        writer.WriteLine($"# {fileNumber}. {this.Title}");
        writer.WriteLine();
        writer.WriteLine(DateTime.Today.ToString("yyyy-MM-dd"));
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
        writer.WriteLine(
            "We will use Architecture Decision Records, as described by Michael Nygard in this article: http://thinkrelevance.com/blog/2011/11/15/documenting-architecture-decisions");
        writer.WriteLine();
        writer.WriteLine("## Consequences");
        writer.WriteLine();
        writer.WriteLine("See Michael Nygard's article, linked above.");
      }
    }

    private void WriteAdrFile(int fileNumber)
    {
      using var writer = File.CreateText(_fileName);
      {
        writer.WriteLine($"# {fileNumber}. {this.Title}");
        writer.WriteLine();
        writer.WriteLine(DateTime.Today.ToString("yyyy-MM-dd"));
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
          where int.TryParse(fileNum, out fileNumOut)
          select fileNumOut;
      var maxFileNum = files.Any() ? files.Max() : 0;
      return maxFileNum + 1;
    }

    private static string SanitizeFileName(string title)
    {
      return title
          .Replace(' ', '-')
          .ToLower();
    }
  }
}
