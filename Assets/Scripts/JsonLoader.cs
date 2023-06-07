using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections;
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
            schematics.Add(JsonConvert.DeserializeObject<Schematic>(jsonFile.text));
        }

        return schematics;
    }
}
