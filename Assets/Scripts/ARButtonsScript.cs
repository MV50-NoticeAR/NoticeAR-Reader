using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class ARButtonsScript : MonoBehaviour
{
    [SerializeField]
    private GameObject bricksButton, bricksList, stepDownButton, stepUpButton, scrollContainer;
    [SerializeField]
    private ScrollRect scrollRect;
    [SerializeField]
    private TextMeshProUGUI stepCountText;
    [SerializeField]
    private Slider stepCountBar;
    [SerializeField]
    private string MENU_SCENE = "MenuScene";

    public GameObject DisplayModel;
    public GameObject brickCountPrefab;

    private bool brickListDisplay, stepProgressDisplay;

    private void Start()
    {
        brickListDisplay = true;
        stepProgressDisplay = true;

        ChangeBrickListDisplay();
        LoadBrickList();

        stepCountBar.maxValue = DisplayModel.GetComponent<ModelDisplay>().StepMax;
        UpdateSteps();
    }

    public void ChangeBrickListDisplay()
    {
        brickListDisplay = !brickListDisplay;
        bricksList.SetActive(brickListDisplay);
        bricksButton.SetActive(!brickListDisplay);
    }

    public void LoadBrickList()
    {
        // Recupere le nombre de chaque briques
        Dictionary<string, int> numberOfEachBricks = DisplayModel.GetComponent<ModelDisplay>().GetNumberOfEachBricks();
        int step = 0;

        Camera renderCamera = new GameObject("RenderCamera").AddComponent<Camera>();

        foreach (KeyValuePair<string, int> kvp in numberOfEachBricks) 
        {
            float scaling = 50f;
            GameObject brick = Instantiate(Resources.Load<GameObject>(@$"Bricks/{kvp.Key}"), transform);
            brick.transform.localPosition = Vector3.zero;
            brick.transform.localScale = new Vector3(scaling, scaling, scaling);
            brick.transform.localRotation = Quaternion.Euler(-33.33f, 33.33f, 0);

            // Modifie la camera pour le rendu des briques
            renderCamera.transform.position = brick.transform.position - Vector3.forward * 3f;
            renderCamera.orthographic = true;
            renderCamera.orthographicSize = 3f;
            renderCamera.clearFlags = CameraClearFlags.Color;

            // Create a RenderTexture to capture the model's render
            RenderTexture renderTexture = new RenderTexture(512, 512, 24);
            renderCamera.targetTexture = renderTexture;

            // Render the model to the RenderTexture
            renderCamera.Render();

            // Create a new sprite from the RenderTexture
            Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
            RenderTexture.active = renderTexture;
            texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            texture.Apply();

            Sprite previewSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one);

            GameObject preview = Instantiate(brickCountPrefab, scrollContainer.transform);
            preview.name = $"Sprite of {kvp.Key}";
            preview.transform.localScale = Vector3.one * 0.4f;
            //preview.transform.localPosition = new Vector3(-30f, 1f * (--step * 180) + 230, 0f);

            TextMeshProUGUI text = preview.GetComponentInChildren<TextMeshProUGUI>();
            text.SetText($"🧩 {kvp.Key}\n   × {kvp.Value}");

            Image imageComp = preview.transform.Find("Image").GetComponent<Image>();
            imageComp.sprite = previewSprite;

            Destroy(renderTexture);
            brick.SetActive(false);
            Destroy(brick);
        }

        Destroy(renderCamera.gameObject);
    }

    public void UpdateSteps()
    {
        int currentStep = DisplayModel.GetComponent<ModelDisplay>().Step;
        int maxStep = DisplayModel.GetComponent<ModelDisplay>().StepMax;

        // step down button
        if (currentStep == 0) stepDownButton.SetActive(false);
        if (currentStep > 0) stepDownButton.SetActive(true);

        // step up button
        if (currentStep == maxStep) stepUpButton.SetActive(false);
        if (currentStep < maxStep) stepUpButton.SetActive(true);

        stepCountText.SetText($"Etape {currentStep}/{maxStep}");
        stepCountBar.value = currentStep;
    }

    public void StepUp()
    {
        DisplayModel.GetComponent<ModelDisplay>().NextStep();
        UpdateSteps();
    }

    public void StepDown()
    {
        DisplayModel.GetComponent<ModelDisplay>().PrevStep();
        UpdateSteps();
    }

    public void BackToMenu()
    {
        StartCoroutine(LoadSceneRoutine(MENU_SCENE));
    }

    System.Collections.IEnumerator LoadSceneRoutine(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

    }
}
