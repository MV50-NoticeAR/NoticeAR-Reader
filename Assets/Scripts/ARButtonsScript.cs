using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class ARButtonsScript : MonoBehaviour
{
    [SerializeField]
    private GameObject bricksButton, bricksList, stepDownButton, stepUpButton;
    [SerializeField]
    private TextMeshProUGUI stepCountText;
    [SerializeField]
    private Slider stepCountBar;
    [SerializeField]
    private string MENU_SCENE = "MenuScene";

    public GameObject DisplayModel;

    private bool brickListDisplay, stepProgressDisplay;

    private void Start()
    {
        brickListDisplay = true;
        stepProgressDisplay = true;
        ChangeBrickListDisplay();
        stepCountBar.maxValue = DisplayModel.GetComponent<ModelDisplay>().StepMax;
        UpdateSteps();
    }

    public void ChangeBrickListDisplay()
    {
        brickListDisplay = !brickListDisplay;
        if (brickListDisplay)
        {
            bricksList.SetActive(true);
            bricksButton.SetActive(false);
        }
        else
        {
            bricksList.SetActive(false);
            bricksButton.SetActive(true);
        }
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

    IEnumerator LoadSceneRoutine(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

    }
}
