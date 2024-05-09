using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawn : MonoBehaviour
{
    void Start()
    {
        Managers.Player.CreatePlayer(0, "111112P");
    }
}
