using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ARButtonsScript : MonoBehaviour
{
    [SerializeField]
    private GameObject bricksButton, bricksList, stepCount, stepProgress;
    private bool brickListDisplay, stepProgressDisplay;

    private void Start()
    {
        brickListDisplay = true;
        stepProgressDisplay = true;
        changeBrickListDisplay();
        changeStepProgressDisplay();
    }

    public void changeBrickListDisplay()
    {
        Debug.Log("ok");
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

    public void changeStepProgressDisplay()
    {
        stepProgressDisplay = !stepProgressDisplay;
        if (stepProgressDisplay)
        {
            stepProgress.SetActive(true);
            stepCount.SetActive(false);
        }
        else
        {
            stepProgress.SetActive(false);
            stepCount.SetActive(true);
        }
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
