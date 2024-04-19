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
    [SerializeField] private float knockBackTimer;
    [SerializeField] private float attackTimer;
    [SerializeField] private bool isFreeze = false;

    public PlayerController Player;
    public float freezeTimer;

    private void OnEnable()
    {
        monster = null;
    }
    private void Update()
    {
        Move();

        if(freezeTimer > 0)
        {
            freezeTimer -= Time.deltaTime;
            if(freezeTimer <= 0)
            {
                isFreeze = false;
            }
        }

        if(this.transform.position.y != 0)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            BulletController bullet = other.GetComponent<BulletController>();
            ChangeMonsterStats(OperationType.Minus, StatType.CurrentHP, bullet.damage);
            bullet.DestroyBullet();
        }
        else if(other.CompareTag("Monster"))
        {
            Physics.IgnoreCollision(other, other);
        }
    }

    private void Move()
    {
        if(monster != null)
        {
            float moveSpeed = monster.moveSpeed;

            if (knockBackTimer > 0)
            {
                knockBackTimer -= Time.deltaTime;

                if (moveSpeed > 0)
                {
                    moveSpeed = -moveSpeed * monster.knockBackAmount;
                }

                if (knockBackTimer < 0)
                {
                    moveSpeed = Mathf.Abs(moveSpeed * 0.5f);
                }
            }

            if (Vector3.Distance(Player.transform.position, transform.position) >= monster.attackRange)
            {
                monsterRigidbody.velocity = (Player.transform.position - transform.position).normalized * moveSpeed;
            }
            else
            {
                Attack();
            }

            if (isFreeze)
            {
                moveSpeed = 0;
            }

            Vector3 targetDiraction = (Player.transform.position - this.transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(targetDiraction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, monster.rotationSpeed * Time.deltaTime);
        }
    }
    private void Attack()
    {
        monsterRigidbody.velocity = Vector3.zero;

        if (monster.attackType == 1)
        {
            Vector3 targetDiraction = (Player.transform.position - this.transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(targetDiraction);

            if (transform.rotation == targetRotation)
            {
                if (attackTimer > 0)
                {
                    attackTimer -= Time.deltaTime;

                    if (attackTimer <= 0)
                    {
                        Managers.Player.SetStats(OperationType.Minus, StatType.CurrentHP, monster.attackDamage);
                        attackTimer = monster.attackSpeed;
                    }
                }
            }
        }
        else if(monster.attackType == 2)
        {
            attackPivot.GetComponent<Transform>();
            //TODO ÃÑ¾Ë »§¾ß Ãß°¡
        }
    }

    public void Freeze(int amount)
    {
        isFreeze = true;
        freezeTimer = amount;
    }


    public void GetMonsterStats(MonsterStats _monster)
    {
        Player = Managers.Player.player.playerController;
        monsterRigidbody = GetComponent<Rigidbody>();

        if (monster == null)
        {
            monster = _monster;
        }

        if (monster.attackType == 2)
        {
            attackPivot = Util.FindChild(this.gameObject, "AttackPivot");
        }
    }
    public void ChangeMonsterStats(OperationType operation, StatType stat, float amount = 0, MonsterStats ms = null)
    {
        if (operation == OperationType.Plus)
        {
            switch (stat)
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
        else if (operation == OperationType.Minus)
        {
            switch (stat)
            {
                case StatType.CurrentHP:
                    monster.hp -= amount;
                    if (monster.hp <= 0)
                        MonsterDie();
                    break;
                case StatType.AttackDamage:
                    monster.attackDamage += amount;
                    if (monster.attackDamage <= 0)
                        monster.attackDamage = 0;
                    break;
                case StatType.MoveSpeed:
                    monster.moveSpeed += amount;
                    break;
            }
        }

    }
    public void MonsterDie()
    {
        Managers.Monster.monsters.Remove(this as MonsterController);
        monster = null;
        Managers.Pool.Destroy(this.gameObject);
    }


}
