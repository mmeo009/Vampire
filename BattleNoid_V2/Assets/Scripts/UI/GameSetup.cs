using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetup : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        Screen.SetResolution(1920, 1080, true);
    }
}
