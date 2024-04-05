using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    Entity_Enemy.Param myData;

    public int hp;
    public float moveSpeed;

    public void LoadMyData(int id, string code)
    {
        myData = Managers.Data.GetDataFromDictionary(Managers.Data.enemyDictionary, id, code);
        hp = ((int)myData.baseHp);
        moveSpeed = myData.baseMoveSpeed;
    }
    public void Move()
    {
        
    }
}
