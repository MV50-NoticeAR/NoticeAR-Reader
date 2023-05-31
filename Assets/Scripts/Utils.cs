using System.Collections.Generic;
using UnityEngine;

/// <sumary>
/// Constantes du projet
/// </sumary>
public struct CONSTANTS
{
    public const bool DEBUG = true;
}

/// <sumary>
/// Class permettant d'étendre d'autre classes en leur ajoutant des méthodes utiles
/// </sumary>
public static class Mixin 
{
    /// <sumary>
    /// Détruit tous les enfants d'un GameObject (à partir de son transform)
    /// </sumary>
    public static void ClearChildren(this Transform transform)
    {
        foreach (Transform child in transform) UnityEngine.Object.Destroy(child.gameObject);
    }

    /// <sumary>
    /// Convertie un Texture2D vers un Sprite
    /// </sumary>
    public static Sprite ToSprite(this Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
}

public struct Schematic
{
    public string name;
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
    public int model;
    public string color;
    public Vector3 position;
    public Quaternion rotation;
}
