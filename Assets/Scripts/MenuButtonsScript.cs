using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonsScript : MonoBehaviour
{
    public int PageNumber;

    [SerializeField]
    private Button[] Buttons;
    [SerializeField]
    private string[] NoticesFiles;

    private void Start()
    {
        PageNumber = 0;
        RefreshButtons();
    }

    public void PageUp()
    {
        ChangePage(1);
    }

    public void PageDown()
    {
        ChangePage(-1);
    }

    private void ChangePage(int modif)
    {
        if ((NoticesFiles.Length % Buttons.Length!=0 && PageNumber < NoticesFiles.Length/Buttons.Length+1)||(PageNumber < NoticesFiles.Length / Buttons.Length))
        {
            PageNumber += modif;
            RefreshButtons();
        }
    }
    

    private void RefreshButtons()
    {
        for(int i = PageNumber*Buttons.Length;i< (PageNumber+1) * Buttons.Length; ++i)
        {
            ToAssembleButtonScript assembleScript = Buttons[i%Buttons.Length].GetComponent<ToAssembleButtonScript>();
            assembleScript.ButtonFile = null;
            if (NoticesFiles.Length > i)
            {
                assembleScript.ButtonFile = NoticesFiles[i];
            }
        }
    }
}
