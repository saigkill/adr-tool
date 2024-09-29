using adr;

using adr_tool.Strategies;

using Microsoft.Extensions.CommandLineUtils;

namespace adr_tool
{
  internal static class Program
  {
    private const string HelpOption = "-?|-h|--help";

    private static void Main(string[] args)
    {
      var generateStrategy = new GenerateOutput();
      var app = new CommandLineApplication();
      app.Name = "adr";
      app.Description = "A simply tool to handle architecture decision records.";

      app.HelpOption(HelpOption);

      app.Command("init", (command) =>
      {
        command.Description = "Creates the directory for adr's and writes first adr.";
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
        command.Description = "";
        command.OnExecute(() =>
        {
          string adrBinDir = AdrSettings.Current.DocFolder;
          string[] files = Directory.GetFiles(adrBinDir, "*.md");
          foreach (var file in files)
          {
            Console.WriteLine(file);
          }

          return 0;
        });
      });

      app.Command("new", (command) =>
      {
        command.Description = "";
        var title = command.Argument("title", "");
        //var supersedes = command.Option("-s|--supersedes", "", CommandOptionType.MultipleValue);
        command.HelpOption(HelpOption);

        command.OnExecute(() =>
        {
          new AdrEntry(TemplateType.New) { Title = title.Value ?? "" }
              .Write()
              .Launch();
          return 0;
        });
      });

      app.Command("link", (command) =>
      {
        command.Description = "";
        command.OnExecute(() =>
        {
          return 0;
        });
      });

      app.Command("generate", (command) =>
      {
        command.Description = "Generate some outputs like toc or graph.";
        var toc = command.Argument("toc", "");
        toc.Description = "Generate a table of contents";
        var graph = command.Argument("graph", "");
        graph.Description = "Generate a graph of the architecture decision records.";
        var intro = command.Option("-i|--intro", "", CommandOptionType.SingleValue);
        intro.Description = "Write some things, that can be used as intro.";
        var outro = command.Option("-o|--outro", "", CommandOptionType.SingleValue);
        outro.Description = "Write some things, that can be used as outro.";
        var linkPrefix = command.Option("-p|--link-prefix", "", CommandOptionType.SingleValue);
        linkPrefix.Description = "Prefix for links in the generated output.";

        command.OnExecute(() =>
        {
          if (toc != null)
          {
            generateStrategy.OutputStrategy(new GenerateToc(intro.ToString(), outro.ToString(), linkPrefix.ToString()));
            generateStrategy.Generate();
          }
          //else if (graph != null)
          //{
          //  generateStrategy.OutputStrategy(new GenerateGraph(linkPrefix.ToString(), null));
          //  generateStrategy.Generate();
          //}

          return 0;
        });
      });

      app.OnExecute(() =>
      {
        app.ShowHelp();
        return 0;
      });
      app.Execute(args);
    }
  }
}
