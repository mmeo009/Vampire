using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Supporter;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public Enums.ActonType actonType;
    public string mythod;
    public Button button;
    void Start()
    {
        button = Util.GetOrAddComponent<Button>(this.gameObject);
        button.onClick.AddListener(() => Managers.Button.LoadGameScene(actonType, mythod));
    }

}
