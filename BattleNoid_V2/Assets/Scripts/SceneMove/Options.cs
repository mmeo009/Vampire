using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour
{
    public GameObject OptionsWindow;


    public void OnClickstopbutton()
    {
        Time.timeScale = 0;
        OptionsWindow.SetActive(true);
    }

    public void OnClickContiButtom()
    {
        Time.timeScale = 1;
        OptionsWindow.SetActive(false);
    }
}
