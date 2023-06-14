using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public int CurrentPage
    {
        get => __currentPage;
        private set 
        {
            __currentPage = value;

            // Fleche gauche
            if (value == 0) leftButton.SetActive(false);
            else leftButton.SetActive(true);

            // Fleche droite
            if (CurrentPage == schemas.Count / maxSchematicsPerPage - 1 || schemas.Count < maxSchematicsPerPage) rightButton.SetActive(false);
            else rightButton.SetActive(true);

            // On actualise le display
            ShowSchemasInGUI();
        }
    }

    public void PageUp() => CurrentPage++;
    public void PageDown() => CurrentPage--;

    void Awake()
    {        
        // On recupere toutes les notices du dossier
        schemas = JsonLoader.FetchAllSchematics();
        if (CONSTANTS.DEBUG == true) Debug.Log($"All schemas loaded ! {schemas.Count} schemas loaded");

        // On actualise le display
        ShowSchemasInGUI();

        // On cache la fleche gauche sur la premiere page
        leftButton.SetActive(false);

        if (schemas.Count < maxSchematicsPerPage) rightButton.SetActive(false);
    }

    /// <summary>
    /// Affiche les boutons pour lancer la construction d'une notice en fonction de la page
    /// et des notices disponibles
    /// </summary>
    void ShowSchemasInGUI()
    {
        // On enleve l'affichage precedent
        verticalLayout.transform.ClearChildren();

        // On cree deux nouvelles lignes
        List<GameObject> rows = new() {
            new GameObject() { name = "HorizontalLayout 0" },
            new GameObject() { name = "HorizontalLayout 1" },
        };

        // On les centres correctement
        float sign = 1f;
        foreach (GameObject row in rows)
        {
            row.transform.SetParent(verticalLayout.transform);
            row.transform.localPosition = new Vector3(-330f, 165f * sign, 0f);
            row.transform.localScale = new Vector3(1f, 1f, 1f);
            sign = sign == 1f ? -1f : 1f;
        }

        int c = 0;          // colone en cours
        int r = 0;          // ligne en cours
        int processed = 0;  // notice en cours

        // On affiche les notices concernees
        // => En fonction de la page sur laquelle on est
        for (
            int i = CurrentPage * maxSchematicsPerPage;
            i < schemas.Count && processed < maxSchematicsPerPage;
            i++
        )
        {
            // Si la ligne precedente est pleine, on incremente r et on remet c a 0
            if (rows[r].transform.childCount == maxSchematicsPerPage / 2)
            {
                r++;
                c = 0;
            }

            Schematic schema = schemas[i];

            // On cree le bouton associe a la notice
            GameObject button = Instantiate(noticeButtonPrefab, rows[r].transform);
            button.name = $"{schema.name} schematic button";
            button.transform.localPosition = new Vector3(c * 330f, 0f, 0f);

            ToAssembleButtonScript script = button.GetComponent<ToAssembleButtonScript>();
            script.ButtonFile = schema.filename;

            // On modifie le texte du bouton
            TextMeshProUGUI textMesh = button.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            textMesh.text = $" {schema.name} (v{schema.version})\n🧩 {schema.description}\n👤 {schema.author}";

            // On update l'image du bouton (en fonction de celle parametree dans le JSON)
            if (schema.picture != null)
            {
                Image imageComp = button.transform.Find("Image").GetComponent<Image>();
                Sprite sprite = Resources.Load<Sprite>(@$"Pictures/{schema.picture.Replace(".png", "")}");
                if (sprite != null) imageComp.sprite = sprite;
            }

            c++;
            processed++;
        }
    }
}
