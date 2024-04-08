using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using Supporter;

public class MonsterController : MonoBehaviour
{
    Entity_Enemy.Param myData;

    public int hp;
    public float moveSpeed;
    public float rotationSpeed = 10;
    public PlayerController player;
    public Rigidbody rb;

    public void Update()
    {
        Move();
    }
    public void LoadMyData(int id, string code)
    {
        myData = Managers.Data.GetDataFromDictionary(Managers.Data.enemyDictionary, id, code);
        hp = ((int)myData.baseHp);
        moveSpeed = myData.baseMoveSpeed;
        player = Managers.Player.player.playerController;
        rb = Util.GetOrAddComponent<Rigidbody>(this.gameObject);
    }

    public void Move()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;

        rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);

        Vector3 targetDiraction = (player.transform.position - this.transform.position).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(targetDiraction);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
