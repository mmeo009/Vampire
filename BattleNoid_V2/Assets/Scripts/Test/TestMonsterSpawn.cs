using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMonsterSpawn : MonoBehaviour
{
    public float time = 30;
    public float timer;
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            Managers.Monster.CreateMonster(this.transform, 1, "111111A");
            timer = time;
            Debug.Log("½ºÆù");
        }
    }
}
