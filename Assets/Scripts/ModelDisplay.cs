using UnityEngine;
using System.Collections.Generic;

public class ModelDisplay : MonoBehaviour
{
    public ModelDisplay instance = null;

    public Camera camAR;
    public Camera camTest;
    public GameObject bricksList;

    public GameObject parent;
    public float scaling = 0.001f;

    public Material transparentMat;
    public Material baseMat;

    private Schematic schema;
    private Dictionary<int, List<GameObject>> piecesPerSteps = new();
    private Dictionary<string, int> numberOfEachBricks = new();    

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
            PlayerPrefs.SetInt(CONSTANTS.PLAYER_PREF_STEP_KEY, value);

            Step schematicStep = schema.steps[Step - 1];

            // On retourne en arriere
            if (decreasing)
            {
                if (value + 1 <= StepMax)
                {
                    foreach (GameObject piece in piecesPerSteps[value + 1])
                    {
                        piece.SetActive(false);
                    }
                }

                foreach (GameObject piece in piecesPerSteps[value])
                {
                    SetFlashing(piece, value > 1);
                }
            }

            // On incremente le nombre d'etape de 1
            else 
            {
                // On check si l'etape n'a pas deja ete affichee 1 fois
                if (!piecesPerSteps.ContainsKey(value))
                {
                    foreach (Piece piece in schematicStep.pieces)
                    {
                        GameObject output = Display(piece.model, piece.position, piece.rotation, piece.color);
                        if (output == null) continue;

                        if (!piecesPerSteps.ContainsKey(value)) piecesPerSteps.Add(value, new List<GameObject>());
                        piecesPerSteps[value].Add(output);
                    }
                }
                // Si c'est le cas on reaffiche les pieces
                else 
                {
                    foreach (GameObject piece in piecesPerSteps[value])
                    {
                        piece.SetActive(true);
                        SetFlashing(piece, value > 1);
                    }
                }

                // Si y'avait des pieces dans l'etape d'avant on enleve le flashing
                if (value - 1 > 0)
                {
                    foreach (GameObject piece in piecesPerSteps[value - 1])
                    {
                        SetFlashing(piece, false);
                    }
                }
            }
            
            BuildListOfBricks(value);
        }
    }

    public void NextStep() => Step++;
    public void PrevStep() => Step--;

    // Start is called before the first frame update
    void Start()
    {
        camAR.enabled = true;
        camTest.enabled = false;
    }

    void Awake()
    {
        schema = JsonLoader.FetchSchematic(PlayerPrefs.GetString(CONSTANTS.PLAYER_PREF_SCHEMATIC_KEY));
        StepMax = schema.steps.Count;

        for (int i = 0; i < StepMax; i++)
        {
            numberOfBricks += schema.steps[i].pieces.Count;
        }

        int savedStep = PlayerPrefs.GetInt(CONSTANTS.PLAYER_PREF_STEP_KEY);
        if (savedStep > 1 && savedStep <= StepMax) {
            for (int i = 1; i <= savedStep; i++)
            {
                Debug.Log($"Rebuilding step {i}/{savedStep}");
                NextStep();
            }
        }

        else NextStep();
    }

    private void BuildListOfBricks(int step)
    {
        numberOfEachBricks.Clear();
        foreach (GameObject brick in piecesPerSteps[step])
        {
            string shortName = brick.name.Replace("(Clone)", "");
            
            if (!numberOfEachBricks.ContainsKey(shortName)) numberOfEachBricks.Add(shortName, 1);
            else numberOfEachBricks[shortName]++;
        }
    }

    public Dictionary<string, int> GetNumberOfEachBricks() => numberOfEachBricks;

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
        GameObject piece;

        try {
            piece = Instantiate(resource, parent.transform);
            piece.transform.localPosition = pos * scaling;
            piece.transform.localRotation = rot;
        } catch {
            Debug.Log($"Piece {name} not found");
            return null;
        }
        
        // Scaling the piece
        piece.transform.localScale = new Vector3(scaling, scaling, scaling);

        // Ajout du script pour pouvoir faire clignoter la piece
        FlashingMaterialScript script = piece.AddComponent<FlashingMaterialScript>();
        script.transparentMat = transparentMat;

        // Changing the color
        if (ColorUtility.TryParseHtmlString(hexColor.StartsWith("#") ? hexColor : $"#{hexColor}", out Color customColor))
        {
            piece.GetComponentInChildren<Renderer>().material.color = customColor;
            script.baseColor = customColor;
        }

        SetFlashing(piece, Step > 1);

        return piece;
    }

    private void SetFlashing(GameObject piece, bool flashing)
    {
        FlashingMaterialScript script = piece.GetComponent<FlashingMaterialScript>();

        Debug.Log($"SetFlashing {piece.name} to {flashing}");

        if (flashing) script.Play();
        else script.Pause();
    }
}
