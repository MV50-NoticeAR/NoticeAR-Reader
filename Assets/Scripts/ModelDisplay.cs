using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;

using Dummiesman;

public class ModelDisplay : MonoBehaviour
{
    public Camera camAR;
    public Camera camTest;

    public GameObject par;

    public GameObject piece;

    public float scaling;

    // Start is called before the first frame update
    void Start()
    {
        camAR.enabled = false;
        camTest.enabled = true;
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        UnityEngine.Debug.Log("Lancement de l'affichage");

        this.Display("3004", new Vector3(0, 0, 100), new Quaternion(0.7f, 0f, 0f, 0.7f), "#fff101");
    }

    void Display(string nomPiece, Vector3 pos, Quaternion rot, string hexColor)
    {
        piece = new OBJLoader().Load("C:/Users/gauth/Documents/GitHub/API/" + nomPiece + ".obj");
        // Loading new piece
        // Positioning the piece
        piece.transform.position = pos;
        // Rotating the piece
        piece.transform.rotation = rot;
        // Scaling the piece
        piece.transform.localScale = new Vector3(scaling, scaling, scaling);
        // Setting the AR controller as a parent
        piece.transform.parent = par.transform;
        // Changing the color
        Color customColor;
        if (ColorUtility.TryParseHtmlString(hexColor, out customColor))
            piece.GetComponentInChildren<Renderer>().material.color = customColor;
        UnityEngine.Debug.Log(customColor);
    }
}
