using UnityEngine;
using System.Collections.Generic;

public class ModelDisplay : MonoBehaviour
{
    public Camera camAR;
    public Camera camTest;

    public GameObject par;
    public float scaling;

    public Material transparentMat;
    public Material baseMat;

    private Schematic schema;
    private Dictionary<int, List<GameObject>> piecesPerSteps = new();
    
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
            if (value < 1) return;
            if (value > StepMax) return;

            bool decreasing = value < __step;

            __step = value;
            Step schematicStep = schema.steps[Step - 1];

            // On retourne en arriere
            if (decreasing)
            {
                if (value + 1 <= StepMax)
                {
                    foreach (GameObject piece in piecesPerSteps[value + 1])
                    {
                        Destroy(piece);
                    }
                }

                foreach (GameObject piece in piecesPerSteps[value])
                {
                    AddFlashingScript(piece);
                }
            }

            // On incremente le nombre d'etape de 1
            else 
            {
                foreach (Piece piece in schematicStep.pieces)
                {
                    GameObject output = Display(piece.model, piece.position, piece.rotation, piece.color);

                    AddFlashingScript(output);

                    if (!piecesPerSteps.ContainsKey(value)) piecesPerSteps.Add(value, new List<GameObject>());
                    piecesPerSteps[value].Add(output);
                }

                // Si y'avait des pieces avant on enleve le flashing
                if (value - 1 > 0)
                {
                    foreach (GameObject piece in piecesPerSteps[value - 1])
                    {
                        RemoveFlashingScript(piece);
                    }
                }
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

        schema = JsonLoader.FetchSchematic("final.json");
        StepMax = schema.steps.Count;

        for (int i = 0; i < StepMax; i++)
        {
            numberOfBricks += schema.steps[i].pieces.Count;
        }

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
        GameObject resource = Resources.Load<GameObject>(@$"Bricks/{name}");
        GameObject piece = Instantiate(resource, pos, rot);
        
        // Scaling the piece
        piece.transform.localScale = new Vector3(scaling, scaling, scaling);
        
        // Setting the AR controller as a parent
        piece.transform.parent = par.transform;

        // Changing the color
        if (ColorUtility.TryParseHtmlString(hexColor, out Color customColor)) 
            piece.GetComponentInChildren<Renderer>().material.color = customColor;

        return piece;
    }

    private void AddFlashingScript(GameObject piece)
    {
        // change the material transparent for flashing 
        piece.GetComponentInChildren<Renderer>().material = transparentMat;
        piece.AddComponent<FlashingMaterialScript>();
    }

    private void RemoveFlashingScript(GameObject piece)
    {
        piece.GetComponent<FlashingMaterialScript>().RemoveScript();
        piece.GetComponentInChildren<Renderer>().material = baseMat;
    }
}
