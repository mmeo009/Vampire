using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Supporter;

public class MonsterController : MonoBehaviour
{
    Entity_Enemy.Param myData;

    [SerializeField] private MonsterStats monster;
    [SerializeField] private float hp;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float attackDamage;
    [SerializeField] private float rotationSpeed = 10;
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
    public void AddStats(float _hp = 0, float damage = 0, float speed = 0)
    {
        hp += _hp;
        attackDamage += damage;
        moveSpeed += speed;
    }

    public void Move()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;

        rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);

        Vector3 targetDiraction = (player.transform.position - this.transform.position).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(targetDiraction);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void GetDmg(float amount)
    {
        hp -= amount;
        if(hp <= 0)
        {
            Managers.Monster.monsters.Remove(this as MonsterController);
            Managers.Pool.Destroy(this.gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bullet"))
        {
            BulletController bullet = other.GetComponent<BulletController>();
            GetDmg(bullet.damage);
            bullet.DestroyBullet();
        }
    }
}
