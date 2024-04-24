using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyWindow : MonoBehaviour
{
    public async void Start()
    {

        Managers.Data.LoadAllAsync<Object>("Prefabs", (key, count, totalCount) =>
        {

            Managers.UI.optionWindowPrefab = Managers.Data.Load<GameObject>("OptionsWindow");
        });

    }
}
