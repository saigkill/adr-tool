using adr;

using Microsoft.Extensions.CommandLineUtils;

namespace adr_tool
{
  internal static class Program
  {
    private const string HelpOption = "-?|-h|--help";

    private static void Main(string[] args)
    {
      var app = new CommandLineApplication();
      app.Name = "adr-tool";
      app.Description = "";

      app.HelpOption(HelpOption);

      app.Command("init", (command) =>
      {
        command.Description = "Init it";
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
          return 0;
        });
      });

      app.Command("new", (command) =>
      {
        command.Description = "";
        var title = command.Argument("title", "");
        var supercedes = command.Option("-s|--supercedes", "", CommandOptionType.MultipleValue);
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
        command.Description = "";
        command.OnExecute(() =>
        {
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
