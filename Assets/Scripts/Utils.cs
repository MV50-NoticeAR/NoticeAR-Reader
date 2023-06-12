using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Constantes du projet
/// </summary>
public struct CONSTANTS
{
    public const bool DEBUG = true;
}

/// <summary>
/// Class permettant d'etendre d'autre classes en leur ajoutant des methodes utiles
/// </summary>
public static class Mixin 
{
    /// <summary>
    /// Detruit tous les enfants d'un GameObject (a partir de son transform)
    /// </summary>
    public static void ClearChildren(this Transform transform)
    {
        foreach (Transform child in transform) Object.Destroy(child.gameObject);
    }
}

public struct Schematic
{
    public string name;
    public string filename;
    public string description;
    public string picture;
    public string author;
    public string version;
    public List<Step> steps;
}

public struct Step 
{
    public List<Piece> pieces;
    public string name;
    public string description;
}

public struct Piece
{
    public string model;
    public string color;
    public Vector3 position;
    public Quaternion rotation;
}
