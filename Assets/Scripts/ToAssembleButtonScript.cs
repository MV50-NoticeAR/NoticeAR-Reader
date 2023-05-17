using System.Collections;
using System.Collections.Generic;
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
        if (ButtonFile!=null)
        {
            SetNotice(ButtonFile);
            StartCoroutine(LoadSceneRoutine(AR_SCENE));
        }
    }

    private void SetNotice(string Value)
    {
        PlayerPrefs.SetString(NOTICE_PLAYERPREF, Value);
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
