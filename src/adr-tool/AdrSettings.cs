// Architecture Decision Record Tool
// Copyright (C) 2025+ Sascha Manns
// 
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License along with this program. If not, see <https://www.gnu.org/licenses/>.

using Newtonsoft.Json;

namespace adr_tool;

public sealed class AdrSettings
{
  private const string DefaultFileName = "adr.config.json";

  private static AdrSettings _instance = new AdrSettings();

  private AdrSettings()
  {
  }

  public static AdrSettings Current
  {
    get
    {
      if (_instance == null)
      {
        _instance = Read(new AdrSettings());
      }

      return _instance;
    }
  }

  public string DocFolder { get; set; } = string.Empty;

  public string TemplateFolder { get; set; } = string.Empty;

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
      settings.DocFolder = "docs//adr";
      settings.TemplateFolder = "";
      return settings;
    }

    using var stream = File.OpenText(DefaultFileName);
    var serializer = new JsonSerializer
    {
      Formatting = Formatting.Indented,
      NullValueHandling = NullValueHandling.Ignore
    };

    var value = serializer.Deserialize(stream, typeof(object)) as dynamic;
    settings.DocFolder = value?.path ?? string.Empty;
    settings.TemplateFolder = value?.template ?? string.Empty;
    return settings;
  }
}
