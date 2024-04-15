using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Supporter;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonController : MonoBehaviour
{
    #region PublicVariables
    public ActionType actonType;
    public string mythod;
    public Button button;
    #endregion
    private void Start()
    {
        if(button == null)
        button = Util.GetOrAddComponent<Button>(this.gameObject);

        button.onClick.AddListener(() => Managers.Button.ButtonAction(actonType, mythod));
    }

}
