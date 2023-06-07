using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class JsonLoader
{
    /// <summary>
    /// Convert the given JSON path into a Schematic
    /// </summary>
    public static Schematic Load(string filename)
    {
        string path = Path.Combine(CONSTANTS.DEBUG == true ? Application.streamingAssetsPath : Application.persistentDataPath, "schematics", filename);
        if (!File.Exists(path)) throw new Exception($"Cannot find the file at {path}");

        string dataAsJSON = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<Schematic>(dataAsJSON);
    }

    /// <summary>
    /// Fetch all available JSON files and convert them into Schematics
    /// </summary>
    public static List<Schematic> FetchAllSchematics()
    {
        string path = Path.Combine(CONSTANTS.DEBUG == true ? Application.streamingAssetsPath : Application.persistentDataPath, "schematics");
        if (!Directory.Exists(path)) throw new Exception($"Cannot find the schematics directory at {path}");

        string[] files = Directory.GetFiles(path, "*.json");
        List<Schematic> schematics = new();
        foreach (string file in files)
        {
            schematics.Add(JsonLoader.Load(file));
        }

        return schematics;
    }
}
