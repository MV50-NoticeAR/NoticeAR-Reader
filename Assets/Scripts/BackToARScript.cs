using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToARScript : MonoBehaviour
{
    [SerializeField]
    private GameObject layout;
    [SerializeField]
    private string AR_SCENE = "MainScene";

    public void BackToAR()
    {
        // already went to the AR scene else select a default notice
        if (PlayerPrefs.HasKey(CONSTANTS.PLAYER_PREF_SCHEMATIC_KEY)) ChangeSceneToAR();
        else
        {
            ToAssembleButtonScript script = layout.transform.GetChild(0).GetChild(0).gameObject.GetComponent<ToAssembleButtonScript>();
            if (script != null)
            {
                script.ChangeSceneToAR();
            } 
        }
    }

    private void ChangeSceneToAR()
    {
        SetNotice(PlayerPrefs.GetString(CONSTANTS.PLAYER_PREF_SCHEMATIC_KEY));
        StartCoroutine(LoadSceneRoutine(AR_SCENE));
    }

    private void SetNotice(string value)
    {
        if (PlayerPrefs.GetString(CONSTANTS.PLAYER_PREF_SCHEMATIC_KEY) != value)
            PlayerPrefs.SetInt(CONSTANTS.PLAYER_PREF_STEP_KEY, 1);

        PlayerPrefs.SetString(CONSTANTS.PLAYER_PREF_SCHEMATIC_KEY, value);
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
