using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;

public class GameManager : MonoBehaviour
{
    public GameManager instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        
        Schematic schem = LoadSchematic();
        Debug.Log(schem.name);
        Debug.Log(schem.picture);
        Debug.Log(schem.author);
        Debug.Log(schem.description);
        Debug.Log(schem.version);
        Debug.Log(schem.steps[0].name);
        Debug.Log(schem.steps[0].description);
        Debug.Log(schem.steps[0].pieces[0].model);
        Debug.Log(schem.steps[0].pieces[0].color);
        Debug.Log(schem.steps[0].pieces[0].position);
        Debug.Log(schem.steps[0].pieces[0].rotation);
    }

    public Schematic LoadSchematic()
    {
        string json = System.IO.File.ReadAllText(@"test.json");
        Schematic schem = JsonConvert.DeserializeObject<Schematic>(json);
        return schem;
    }
}
