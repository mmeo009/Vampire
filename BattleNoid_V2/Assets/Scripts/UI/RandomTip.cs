using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class RandomTip : MonoBehaviour
{
    public TextMeshProUGUI TipText;
    public string[] tips;
     
    // Start is called before the first frame update
    void Start()
    {
        ShowRandomTip();
    }


   void ShowRandomTip()
    {
        if(tips.Length > 0)
        {
            int randomIndex = Random.Range(0, tips.Length);
            TipText.text = tips[randomIndex];
        }
    }
}
