using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToARScript : MonoBehaviour
{
    [SerializeField]
    private GameObject layout;
    [SerializeField]
    private string NOTICE_PLAYERPREF = "NoticeName";
    [SerializeField]
    private string AR_SCENE = "MainScene";

    public void BackToAR()
    {
        //already went to the AR scene
        if (PlayerPrefs.HasKey(NOTICE_PLAYERPREF))
        {
            this.ChangeSceneToAR();
        }
        else
        //selects a default notice
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
        SetNotice(PlayerPrefs.GetString(NOTICE_PLAYERPREF));
        StartCoroutine(LoadSceneRoutine(AR_SCENE));
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
