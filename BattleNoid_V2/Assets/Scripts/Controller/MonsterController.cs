using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Supporter;

[RequireComponent(typeof(Rigidbody))]
public class MonsterController : MonoBehaviour
{

    [SerializeField] private MonsterStats monster;
    [SerializeField] private Rigidbody monsterRigidbody;
    [SerializeField] private GameObject attackPivot;

    public PlayerController Player;
    public void OnEnable()
    {
        monster = null;
    }
    public void Update()
    {
        Move();
    }
    public void GetMonsterStats(MonsterStats _monster)
    {
        Player = Managers.Player.player.playerController;
        monsterRigidbody = GetComponent<Rigidbody>();
        attackPivot = Util.FindChild(this.gameObject, "AttackPivot");
        if (monster == null)
        {
            monster = _monster;
        }
    }
    public void ChangeMonsterStats(OperationType operation, StatType stat, float amount = 0, MonsterStats ms = null)
    {
        if(operation == OperationType.Plus)
        {
            switch(stat)
            {
                case StatType.CurrentHP:
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
            switch (stat)
            {
                case StatType.CurrentHP:
                    monster.hp -= amount;
                    if(monster.hp <= 0)
                        MonsterDie();
                    break;
                case StatType.AttackDamage:
                    monster.attackDamage += amount;
                    if(monster.attackDamage <= 0)
                        monster.attackDamage = 0;
                    break;
                case StatType.MoveSpeed:
                    monster.moveSpeed += amount;
                    break;
            }
        }

    }

    public void Move()
    {
        if(monster != null)
        {
            Vector3 direction = (Player.transform.position - this.transform.position).normalized;

            monsterRigidbody.MovePosition(this.transform.position + direction * monster.moveSpeed * Time.deltaTime);

            Vector3 targetDiraction = (Player.transform.position - this.transform.position).normalized;

            Quaternion targetRotation = Quaternion.LookRotation(targetDiraction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, monster.rotationSpeed * Time.deltaTime);
        }
    }
    public void Attack()
    {
        if(monster.code == "111111A")
        {
            attackPivot.GetComponent<SphereCollider>()  ;
        }
    }

    private void GetDmg(float amount)
    {
        ChangeMonsterStats(OperationType.Minus,StatType.CurrentHP, amount);
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

    public void MonsterDie()
    {
        Managers.Monster.monsters.Remove(this as MonsterController);
        monster = null;
        Managers.Pool.Destroy(this.gameObject);
    }
}
