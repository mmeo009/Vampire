using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawn : MonoBehaviour
{
    void Start()
    {
        Managers.Player.CreatePlayer(1, "111111P");
    }
}