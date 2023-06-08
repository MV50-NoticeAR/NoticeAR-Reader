using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class ARButtonsScript : MonoBehaviour
{
    public int currentStep,maxSteps;

    [SerializeField]
    private GameObject bricksButton, bricksList, stepDownButton, stepUpButton;
    [SerializeField]
    private TextMeshProUGUI stepCountText;
    [SerializeField]
    private Slider stepCountBar;
    [SerializeField]
    private string MENU_SCENE = "MenuScene";
    private bool brickListDisplay, stepProgressDisplay;

    private void Start()
    {
        brickListDisplay = true;
        stepProgressDisplay = true;
        ChangeBrickListDisplay();
        stepCountBar.maxValue = maxSteps;
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
        // step down button
        if (currentStep == 0) this.stepDownButton.SetActive(false);
        if (currentStep > 0) this.stepDownButton.SetActive(true);

        // step up button
        if (currentStep == maxSteps) this.stepUpButton.SetActive(false);
        if (currentStep < maxSteps) this.stepUpButton.SetActive(true);

        stepCountText.SetText("Etape "+currentStep+"/"+maxSteps);
        stepCountBar.value = currentStep;
    }

    public void StepUp()
    {
        if (currentStep < maxSteps)
        {
            ++currentStep;
            UpdateSteps();
        }
    }

    public void StepDown()
    {
        if (currentStep > 0)
        {
            --currentStep;
            UpdateSteps();
        }
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
