using UnityEngine;

public class ModelDisplay : MonoBehaviour
{
    public Camera camAR;
    public Camera camTest;

    public GameObject par;
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

        if (CONSTANTS.DEBUG == true) Debug.Log("Lancement de l'affichage");

        Display("3004", new Vector3(0, 0, 100), new Quaternion(0.7f, 0f, 0f, 0.7f), "#fff101");
    }

    /// <summary>
    /// Affiche la piece donnee sur la scene
    /// </summary>
    /// <param name="name">Nom de la piece a importer</param>
    /// <param name="pos">Position de la piece dans la scene</param>
    /// <param name="rot">Rotation de la piece dans la scene</param>
    /// <param name="hexColor">Couleur de la piece</param>
    void Display(string name, Vector3 pos, Quaternion rot, string hexColor)
    {
        // Loading new piece
        GameObject piece = Instantiate(Resources.Load<GameObject>(@$"Bricks/{name}"), pos, rot);
        
        // Scaling the piece
        piece.transform.localScale = new Vector3(scaling, scaling, scaling);
        
        // Setting the AR controller as a parent
        piece.transform.parent = par.transform;
        
        // Changing the color
        if (ColorUtility.TryParseHtmlString(hexColor, out Color customColor)) 
            piece.GetComponentInChildren<Renderer>().material.color = customColor;

        if (CONSTANTS.DEBUG == true) Debug.Log(customColor);
    }
}
