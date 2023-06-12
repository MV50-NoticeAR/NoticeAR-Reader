using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToAssembleButtonScript : MonoBehaviour
{
    [SerializeField]
    private string NOTICE_PLAYERPREF = "NoticeName";
    [SerializeField]
    private string AR_SCENE = "MainScene";

    public string ButtonFile = null;

    public void ChangeSceneToAR()
    {
        if (ButtonFile != null)
        {
            Debug.Log($"Changement de scene vers {ButtonFile}");
            SetNotice(ButtonFile);
            StartCoroutine(LoadSceneRoutine(AR_SCENE));
        }
    }

    private void SetNotice(string value)
    {
        PlayerPrefs.SetString(NOTICE_PLAYERPREF, value);
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
