using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToAssembleButtonScript : MonoBehaviour
{
    [SerializeField]
    private string AR_SCENE = "MainScene";

    public string ButtonFile = null;

    public void ChangeSceneToAR()
    {
        if (ButtonFile != null)
        {
            SetNotice(ButtonFile);
            StartCoroutine(LoadSceneRoutine(AR_SCENE));
        }
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
