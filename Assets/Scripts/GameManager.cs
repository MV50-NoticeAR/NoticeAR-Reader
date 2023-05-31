using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameManager instance = null;

    public GameObject verticalLayout;
    public GameObject noticeButtonPrefab;
    public GameObject leftButton;
    public GameObject rightButton;

    public List<Schematic> schemas = new();

    public int maxSchematicsPerPage = 6;

    int __currentPage = 0;
    public int currentPage
    {
        get => this.__currentPage;
        private set 
        {
            this.__currentPage = value;
            if (CONSTANTS.DEBUG) Debug.Log($"Current page {this.currentPage}");

            // Flèche gauche
            if (value == 0) this.leftButton.SetActive(false);
            if (value == 1) this.leftButton.SetActive(true);

            // Flèche droite
            if (this.currentPage == this.schemas.Count / maxSchematicsPerPage) this.rightButton.SetActive(false);
            if (this.currentPage == (this.schemas.Count / maxSchematicsPerPage) - 1) this.rightButton.SetActive(true);

            // On actualise le display
            this.ShowSchemasInGUI();
        }
    }

    public void PageUp() => this.currentPage++;
    public void PageDown() => this.currentPage--;

    void Start()
    {
        // On check qu'on a bien tous les éléments nécessaires associés via Unity
        if (this.verticalLayout == null) throw new Exception("The layout has not been associated into the script !");
        if (this.noticeButtonPrefab == null) throw new Exception("Button prefab has not been associated into the script !");
        if (this.leftButton == null) throw new Exception("The left arrow has not been associated into the script !");
        if (this.rightButton == null) throw new Exception("The right arrow has not been associated into the script !");

        // On cache la flèche gauche sur la première page
        this.leftButton.SetActive(false);
    }

    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        
        // On récupère toutes les notices du dossier
        this.schemas = JsonLoader.FetchAllSchematics();
        if (CONSTANTS.DEBUG == true) Debug.Log($"All schemas loaded ! {this.schemas.Count} schemas loaded");

        // On actualise le display
        this.ShowSchemasInGUI();
    }

    /// <sumary>
    /// Affiche les boutons pour lancer la construction d'une notice en fonction de la page
    /// et des notices disponibles
    /// </sumary>
    void ShowSchemasInGUI()
    {
        // On enlève l'affichage précédent
        this.verticalLayout.transform.ClearChildren();

        // On crée deux nouvelles lignes
        List<GameObject> rows = new() {
            new GameObject() { name = "HorizontalLayout 0" },
            new GameObject() { name = "HorizontalLayout 1" },
        };

        // On les centres correctement
        float sign = 1f;
        foreach (GameObject row in rows)
        {
            row.transform.SetParent(this.verticalLayout.transform);
            row.transform.localPosition = new Vector3(-330f, 165f * sign, 0f);
            row.transform.localScale = new Vector3(1f, 1f, 1f);
            sign = sign == 1f ? -1f : 1f;
        }

        if (CONSTANTS.DEBUG) Debug.Log($"{this.currentPage}, {this.maxSchematicsPerPage}");

        int c = 0;          // colone en cours
        int r = 0;          // ligne en cours
        int processed = 0;  // notice en cours

        // On affiche les notices concernées
        // => En fonction de la page sur laquelle on est
        for (
            int i = this.currentPage * this.maxSchematicsPerPage;
            i < this.schemas.Count && processed < maxSchematicsPerPage;
            i++
        )
        {
            // Si la ligne précédente est pleine, on incrémente r et on remet c à 0
            if (rows[r].transform.childCount == maxSchematicsPerPage / 2)
            {
                r++;
                c = 0;
            }

            if (CONSTANTS.DEBUG) Debug.Log($"{i}, {c}, {r}");
            Schematic schema = this.schemas[i];

            // On crée le bouton associé à la notice
            GameObject button = Instantiate(noticeButtonPrefab, rows[r].transform);
            button.name = $"{schema.name} schematic button";
            button.transform.localPosition = new Vector3(c * 330f, 0f, 0f);
            
            // On modifie le texte du bouton
            TextMeshProUGUI textMesh = button.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            textMesh.text = $" {schema.name} (v{schema.version})\n🧩 {schema.description}\n👤 {schema.author}";

            // On update l'image du bouton (en fonction de celle paramétrée dans le JSON)
            Image imageComp = button.transform.Find("Image").GetComponent<Image>();
            StartCoroutine(LoadImageFromFiles(imageComp, schema.picture));

            c++;
            processed++;
        }
    }

    /// <sumary>
    /// Télécharge un fichier statique vers un Sprite d'une Image
    /// </sumary>
    private IEnumerator LoadImageFromFiles(Image image, string filename) 
    {
        string path = Path.Combine(CONSTANTS.DEBUG == true ? Application.streamingAssetsPath : Application.persistentDataPath, "pictures", filename);
        if (!File.Exists(path)) throw new Exception($"Cannot find the file at {path}");

        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture($"file:///{path}"))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError) throw new Exception(uwr.error);
            else
            {
                // Get downloaded asset bundle
                var texture = DownloadHandlerTexture.GetContent(uwr);
                image.sprite = texture.ToSprite();
            }
        }
    }
}
