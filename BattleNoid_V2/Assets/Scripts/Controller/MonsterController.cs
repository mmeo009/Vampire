using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Supporter;

public class MonsterController : MonoBehaviour
{
    Entity_Enemy.Param myData;

    [SerializeField] private MonsterStats monster;
    public PlayerController player;
    public Rigidbody rb;

    public void Update()
    {
        Move();
    }
    public void AddStats(StatType stat, float amount = 0, MonsterStats ms = null)
    {
        if(stat == StatType.None)
        {
            player = Managers.Player.player.playerController;
            rb = GetComponent<Rigidbody>();
            return;
        }
        if (amount > 0)
        {
            if (stat == StatType.MoveSpeed) monster.hp += amount;
            else if (stat == StatType.AttackDamage) monster.attackDamage += amount;
            else if (stat == StatType.MAXHP) monster.hp += amount;
            else return;
        }
        else
        {
            return;
        }
    }

    public void Move()
    {
        Vector3 direction = (player.transform.position - this.transform.position).normalized;

        rb.MovePosition(this.transform.position + direction * monster.moveSpeed * Time.deltaTime);

        Vector3 targetDiraction = (player.transform.position - this.transform.position).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(targetDiraction);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, monster.rotationSpeed * Time.deltaTime);
    }

    private void GetDmg(float amount)
    {
        monster.hp -= amount;
        if(monster.hp <= 0)
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
