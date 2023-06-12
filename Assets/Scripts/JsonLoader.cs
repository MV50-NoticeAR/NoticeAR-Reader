using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;


public static class JsonLoader
{
    /// <summary>
    /// Fetch all available JSON files and convert them into Schematics
    /// </summary>
    public static List<Schematic> FetchAllSchematics()
    {
        TextAsset[] jsonFiles = Resources.LoadAll<TextAsset>(@$"Schematics");
        List<Schematic> schematics = new();

        foreach (TextAsset jsonFile in jsonFiles)
        {
            Schematic tmp = JsonConvert.DeserializeObject<Schematic>(jsonFile.text);
            tmp.filename = jsonFile.name;
            schematics.Add(tmp);
        }

        return schematics;
    }

    public static Schematic FetchSchematic(string filename)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>($@"Schematics/{filename.Replace(".json", "")}");
        Schematic tmp = JsonConvert.DeserializeObject<Schematic>(jsonFile.text);
        tmp.filename = filename;

        return tmp;
    }
}
