using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Supporter;

[RequireComponent(typeof(Rigidbody))]
public class MonsterController : MonoBehaviour
{
    private Entity_Enemy.Param myData;

    [SerializeField] private MonsterStats monster;
    public PlayerController Player;
    private Rigidbody rb;

    public void Update()
    {
        Move();
    }
    public void ChangeMonsterStats(OperationType operation, StatType stat, float amount = 0, MonsterStats ms = null)
    {
        if(operation == OperationType.None)
        {
            Player = Managers.Player.player.playerController;
            rb = GetComponent<Rigidbody>();
            return;
        }
        else if(operation == OperationType.Plus)
        {
            switch(stat)
            {
                case StatType.CurrentHP:
                case StatType.MAXHP:
                    monster.hp += amount;
                    break;
                case StatType.AttackDamage:
                    monster.attackDamage += amount;
                    break;
                case StatType.MoveSpeed:
                    monster.moveSpeed += amount;
                    break;
            }
        }
        else if(operation == OperationType.Minus) 
        {

        }

    }

    public void Move()
    {
        Vector3 direction = (Player.transform.position - this.transform.position).normalized;

        rb.MovePosition(this.transform.position + direction * monster.moveSpeed * Time.deltaTime);

        Vector3 targetDiraction = (Player.transform.position - this.transform.position).normalized;

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
