using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyWindow : MonoBehaviour
{
    public void Start()
    {

        Managers.Data.LoadAllAsync<Object>("Prefabs", (key, count, totalCount) =>
        {

            Managers.UI.OptionWindowPrefab = Managers.Data.Load<GameObject>("OptionsWindow");
        });

    }
}
