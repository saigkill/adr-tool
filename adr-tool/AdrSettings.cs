﻿using adr_tool;

using Newtonsoft.Json;

namespace adr
{
  internal class AdrSettings
  {
    private const string DefaultFileName = "adr.config.json";

    private static AdrSettings _instance;

    private AdrSettings()
    {
    }

    public static AdrSettings Current => _instance ??= Read(new AdrSettings());

    public string DocFolder { get; set; }

    public string TemplateFolder { get; set; }

    public AdrSettings Write()
    {
      using (var stream = File.CreateText(DefaultFileName))
      {
        var value = new
        {
          path = this.DocFolder,
          templates = this.TemplateFolder
        };
        var serializer = new JsonSerializer
        {
          Formatting = Formatting.Indented,
          NullValueHandling = NullValueHandling.Ignore
        };
        serializer.Serialize(stream, value);
      }

      return this;
    }

    private static AdrSettings Read(AdrSettings settings)
    {
      if (!File.Exists(DefaultFileName))
      {
        settings.DocFolder = GlobalVariables.AdrFolder;
        settings.TemplateFolder = "";
        return settings;
      }

      using (var stream = File.OpenText(DefaultFileName))
      {
        var serializer = new JsonSerializer
        {
          Formatting = Formatting.Indented,
          NullValueHandling = NullValueHandling.Ignore
        };

        var value = (dynamic)serializer.Deserialize(stream, new { path = "", template = "" }.GetType());
        settings.DocFolder = value.path;
        settings.TemplateFolder = value.template;
        return settings;
      }
    }
  }
}
