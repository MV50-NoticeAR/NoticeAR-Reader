using UnityEngine;
using System.Collections.Generic;

public class ModelDisplay : MonoBehaviour
{
    public Camera camAR;
    public Camera camTest;

    public GameObject par;
    public float scaling;

    public Material transparentMat; // used for flashing

    private Schematic schema;
    private List<GameObject> currentPieces = new();
    private int numberOfBricks = 0;

    private int __stepMax = 0;
    public int StepMax
    {
        private set => __stepMax = value;
        get => __stepMax;
    }

    private int __step = 0;
    public int Step
    {
        get => __step;
        set
        {
            if (value > StepMax) return;
            __step = value;

            Step schematicStep = schema.steps[Step - 1];
            
            if (currentPieces.Count > 0)
            {
                // On enlève l'effet clignotant sur les pièces de l'étape d'avant
                foreach (GameObject piece in currentPieces)
                {
                    RemoveFlashingScript(piece);
                }

                // On reset la liste des pièces en cours
                currentPieces = new();
            }

            foreach (Piece piece in schematicStep.pieces)
            {
                GameObject output = Display(piece.model, piece.position, piece.rotation, piece.color);
                AddFlashingScript(output);
                currentPieces.Add(output);
            }
        }
    }

    public void NextStep() => Step++;
    public void PrevStep() => Step--;

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

        // Display("3004", new Vector3(0, 0, 100), new Quaternion(0.7f, 0f, 0f, 0.7f), "#fff101");

        schema = JsonLoader.FetchSchematic("final.json");
        StepMax = schema.steps.Count;

        for (int i = 0; i < StepMax; i++)
        {
            numberOfBricks += schema.steps[i].pieces.Count;
        }

        Debug.Log($"Number of steps : {StepMax}");
        Debug.Log($"Number of pieces : {numberOfBricks}");

        NextStep();
    }

    /// <summary>
    /// Affiche la piece donnee sur la scene
    /// </summary>
    /// <param name="name">Nom de la piece a importer</param>
    /// <param name="pos">Position de la piece dans la scene</param>
    /// <param name="rot">Rotation de la piece dans la scene</param>
    /// <param name="hexColor">Couleur de la piece</param>
    private GameObject Display(string name, Vector3 pos, Quaternion rot, string hexColor)
    {
        // Loading new piece
        GameObject piece = Instantiate(Resources.Load<GameObject>(@$"Bricks/{name}"), pos, rot);
        
        // Scaling the piece
        piece.transform.localScale = new Vector3(scaling, scaling, scaling);
        
        // Setting the AR controller as a parent
        piece.transform.parent = par.transform;

        //change the material transparent for flashing 
        piece.GetComponentInChildren<Renderer>().material = transparentMat;

        // Changing the color
        if (ColorUtility.TryParseHtmlString(hexColor, out Color customColor)) 
            piece.GetComponentInChildren<Renderer>().material.color = customColor;

        return piece;
    }

    private void AddFlashingScript(GameObject piece)
    {
        piece.AddComponent<FlashingMaterialScript>();
    }

    private void RemoveFlashingScript(GameObject piece)
    {
        var comp = piece.GetComponent<FlashingMaterialScript>();
        Destroy(comp);
    }
}
