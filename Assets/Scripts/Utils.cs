using System.Collections.Generic;
using UnityEngine;


public struct Schematic
{
    public List<Step> steps;
    public string name;
    public string picture;
    public string author;
    public string description;
    public string version;
}

public struct Step 
{
    public List<Piece> pieces;
    public string name;
    public string description;
}

public struct Piece
{
    public int model;
    public string color;
    public Vector3 position;
    public Quaternion rotation;
}
