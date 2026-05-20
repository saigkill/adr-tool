// Architecture Decision Record Tool
// Copyright (C) 2025+ Sascha Manns
// 
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License along with this program. If not, see <https://www.gnu.org/licenses/>.

using Microsoft.Extensions.CommandLineUtils;

namespace adr_tool;

internal static class Program
{
  private const string HelpOption = "-?|-h|--help";

  private static void Main(string[] args)
  {
    var app = new CommandLineApplication();
    app.Name = "adr";
    app.Description = "A simply tool to handle architecture decision records.";

    app.HelpOption(HelpOption);

    app.Command("init", (command) =>
    {
      command.Description = "Initialization";
      var directory = command.Argument("[directory]", "");
      command.HelpOption(HelpOption);
      command.OnExecute(() =>
      {
        var settings = AdrSettings.Current;
        settings.DocFolder = directory.Value ?? settings.DocFolder;
        settings.Write();
        new AdrEntry(TemplateType.Adr)
          .Write()
          .Launch();
        return 0;
      });
    });

    app.Command("list", (command) =>
    {
      command.Description = "List all created ADRs.";
      command.OnExecute(() =>
      {
        var docFolder = AdrSettings.Current.DocFolder;
        if (string.IsNullOrWhiteSpace(docFolder) || !Directory.Exists(docFolder))
        {
          Console.WriteLine("No valid ADR directory found.");
          return 1;
        }

        var files = Directory.GetFiles(docFolder, "*.md", SearchOption.TopDirectoryOnly);
        if (files.Length == 0)
        {
          Console.WriteLine("No ADRs found");
          return 0;
        }

        Console.WriteLine("Founded ADRs:");
        foreach (var file in files)
        {
          Console.WriteLine($"- {Path.GetFileName(file)}");
        }
        return 0;
      });
    });

    app.Command("new", (command) =>
    {
      command.Description = "Create a new Record.";
      var title = command.Argument("title", "Enter your ADR title.");
      var supersedes = command.Option("-s|--supersedes", "Creates a new Record, but marks another Recors as Superseded.", CommandOptionType.MultipleValue);
      command.HelpOption(HelpOption);

      command.OnExecute(() =>
      {
        var adrEntry = new AdrEntry(TemplateType.New) { Title = title.Value ?? "" };

        // Supersedes-Option auswerten
        if (supersedes.HasValue())
        {
          var supersedesList = supersedes.Values
            .Select(s =>
            {
              // Versuche, die Nummer aus dem Dateinamen zu extrahieren
              var fileName = Path.GetFileNameWithoutExtension(s);
              var parts = fileName.Split('-');
              if (parts.Length > 0 && int.TryParse(parts[0], out int adrNum))
                return adrNum.ToString("D4");
              return s;
            })
            .ToList();

          // Vermerk im Titel ergänzen
          adrEntry.Title += $" (Supersedes by {string.Join(", ", supersedesList)})";
          adrEntry.SupersededLinks = supersedes.Values.ToArray();
        }

        adrEntry
          .Write()
          .Launch();
        return 0;
      });
    });

    app.Command("link", (command) =>
    {
      command.Description = "Links to ADRs";
      var adr1 = command.Argument("adr1", "First ADR-File (z.B. 0001-titel.md)");
      var adr2 = command.Argument("adr2", "Second ADR-File (z.B. 0002-titel.md)");
      command.HelpOption(HelpOption);

      command.OnExecute(() =>
      {
        var docFolder = AdrSettings.Current.DocFolder;
        if (string.IsNullOrWhiteSpace(docFolder) || !Directory.Exists(docFolder))
        {
          Console.WriteLine("No valid ADR directory found.");
          return 1;
        }

        var file1 = Path.Combine(docFolder, adr1.Value ?? "");
        var file2 = Path.Combine(docFolder, adr2.Value ?? "");

        if (!File.Exists(file1) || !File.Exists(file2))
        {
          Console.WriteLine("At least one of the specified ADR files does not exist.");
          return 1;
        }

        void AddLink(string sourceFile, string targetFile)
        {
          var targetName = Path.GetFileName(targetFile);
          var lines = File.ReadAllLines(sourceFile).ToList();

          // Suche nach "## Links" oder füge am Ende hinzu
          int linksIndex = lines.FindIndex(l => l.Trim() == "## Links");
          if (linksIndex == -1)
          {
            lines.Add("");
            lines.Add("## Links");
            linksIndex = lines.Count - 1;
          }

          // Prüfe, ob Link schon existiert
          var linkText = $"- Siehe [{targetName}]({targetName})";
          if (!lines.Skip(linksIndex + 1).Any(l => l.Contains(targetName)))
          {
            lines.Insert(linksIndex + 1, linkText);
          }

          File.WriteAllLines(sourceFile, lines);
        }

        AddLink(file1, file2);
        AddLink(file2, file1);

        Console.WriteLine($"ADRs {adr1.Value} and {adr2.Value} linked to each other.");
        return 0;
      });
    });

    //app.Command("generate", (command) =>
    //{
    //  command.Description = "";
    //  command.OnExecute(() =>
    //  {
    //    return 0;
    //  });
    //});

    app.OnExecute(() =>
    {
      app.ShowHelp();
      return 0;
    });
    app.Execute(args);
  }
}
