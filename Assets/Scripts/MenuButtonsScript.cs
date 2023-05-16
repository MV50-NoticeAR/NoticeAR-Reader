using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonsScript : MonoBehaviour
{
    public int PageNumber;

    [SerializeField]
    private string NOTICE_PLAYERPREF = "NoticeName";
    [SerializeField]
    private Button[] Buttons;
    [SerializeField]
    private string[] NoticesFiles;
    [SerializeField]
    private string[] ButtonsFiles;

    private void Start()
    {
        PageNumber = 0;
        RefreshButtons();
        ButtonsFiles = new string[Buttons.Length];
    }

    public void SetNotice(string Value)
    {
        PlayerPrefs.SetString(NOTICE_PLAYERPREF, Value);
    }

    private void RefreshButtons()
    {
        for(int i = PageNumber*Buttons.Length;i< (PageNumber+1) * Buttons.Length-1; ++i)
        {
            ButtonsFiles[i] = null;
            if (NoticesFiles.Length > i)
            {
                ButtonsFiles[i] = NoticesFiles[i];
            }
        }
    }
}
