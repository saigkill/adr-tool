// Architecture Decision Record Tool
// Copyright (C) 2025+ Sascha Manns
// 
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License along with this program. If not, see <https://www.gnu.org/licenses/>.

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace adr_tool;

public sealed class AdrEntry(TemplateType templateType)
{
  private readonly string _docFolder = AdrSettings.Current.DocFolder;

  private readonly string _templatePath = $"{AdrSettings.Current.TemplateFolder}//{templateType.ToString()}.md";

  private string _fileName = string.Empty;

  public string Title { get; set; } = "Record Architecture Decisions";
  public string[] SupersededLinks { get; set; } = [];

  public AdrEntry Write()
  {
    if (templateType == TemplateType.Adr)
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
    if (!Directory.Exists(this._docFolder))
    {
      Directory.CreateDirectory(this._docFolder);
    }
    using var writer = File.CreateText(_fileName);
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

  private void WriteAdr()
  {
    var fileNumber = Directory.Exists(this._docFolder)
      ? GetNextFileNumber(this._docFolder)
      : 1;
    _fileName = Path.Combine(
      _docFolder,
      $"{fileNumber.ToString().PadLeft(4, '0')}-{SanitizeFileName(this.Title)}.md");
    if (!Directory.Exists(this._docFolder))
    {
      Directory.CreateDirectory(this._docFolder);
    }
    using var writer = File.CreateText(_fileName);
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
    writer.WriteLine("We will use Architecture Decision Records, as described by Michael Nygard in this article: http://thinkrelevance.com/blog/2011/11/15/documenting-architecture-decisions");
    writer.WriteLine();
    writer.WriteLine("## Consequences");
    writer.WriteLine();
    writer.WriteLine("See Michael Nygard's article, linked above.");
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
    var enumerable = files.ToList();
    var maxFileNum = enumerable.Any() ? enumerable.Max() : 0;
    return maxFileNum + 1;
  }

  private static string SanitizeFileName(string title)
  {
    return title
      .Replace(' ', '-')
      .ToLower();
  }
}
