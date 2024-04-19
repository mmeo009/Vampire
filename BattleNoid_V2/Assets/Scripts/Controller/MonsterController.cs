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
    [SerializeField] private bool isAttack = false;

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

        if (isAttack == true)
        {
            if(attackTimer > 0) 
            {
                attackTimer -= Time.deltaTime;
                if (attackTimer <= 0)
                {
                    Attack();
                }
            }
        }


        if (this.transform.position.y != 0)
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
                isAttack = false;
            }
            else
            {
                monsterRigidbody.velocity = Vector3.zero;
                isAttack = true;
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
        if (monster.attackType == 1)
        {
            Vector3 targetDiraction = (Player.transform.position - this.transform.position).normalized;

            if (GetMyRange() == 1)
            {
                Managers.Player.SetStats(OperationType.Minus, StatType.CurrentHP, monster.attackDamage);
                attackTimer = monster.attackSpeed;
            }
        }
        else if(monster.attackType == 2)
        {
            attackPivot.GetComponent<Transform>();
            //TODO 총알 빵야 추가
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
        attackTimer = _monster.attackSpeed;
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
    public int GetMyRange()
    {
        Vector3 myPos = transform.position + new Vector3(0, 1.3f, 0);
        Vector3 toPlayer = Player.transform.position - myPos;
        toPlayer.Normalize();

        float _angle = Vector3.Angle(transform.forward, toPlayer);

        if (_angle < monster.viewingAngle * 0.5f)
        {
            if (Vector3.Distance(myPos, Player.transform.position + new Vector3(0, 1.3f, 0)) <= monster.attackRange)
            {
                Debug.Log($"{gameObject.name}의 공격 범위안에 플레이어 있음");
                return 1;
            }
            else
            {
                Debug.Log($"{gameObject.name}의 시야 범위안에 플레이어 있음");
                return 2;
            }
        }
        else
        {
            Debug.Log("플레이어 없소");
            return 0;
        }
    }
    private void OnDrawGizmos()
    {
        if(monster != null)
        {
            DrawLine(monster.viewingAngle);
        }
    }
    
    void DrawLine(float angle)
    {
        Gizmos.color = Color.blue;
        float halfPlusAngle = angle * 0.5f + transform.eulerAngles.y;
        float halfMinusAngle = -angle * 0.5f + transform.eulerAngles.y;
        Vector3 angleRight = new Vector3(Mathf.Sin(halfPlusAngle * Mathf.Deg2Rad), 0, Mathf.Cos(halfPlusAngle * Mathf.Deg2Rad));
        Vector3 angleLeft = new Vector3(Mathf.Sin(halfMinusAngle * Mathf.Deg2Rad), 0, Mathf.Cos(halfMinusAngle * Mathf.Deg2Rad));

        Vector3 end1 = transform.position + new Vector3(0, 1.3f, 0) + angleRight * monster.attackRange;
        Vector3 end2 = transform.position + new Vector3(0, 1.3f, 0) + angleLeft * monster.attackRange;
        Gizmos.DrawLine(transform.position + new Vector3(0, 1.3f, 0), end1);
        Gizmos.DrawLine(transform.position + new Vector3(0, 1.3f, 0), end2);
        Gizmos.DrawLine(transform.position + new Vector3(0, 1.3f, 0), transform.position + new Vector3(0, 1.3f, 0) + transform.forward * monster.attackRange);

        if(Player != null)
        {
            Gizmos.color = Color.yellow;
            if(GetMyRange() == 1)
            {
                Gizmos.color = Color.red;
            }
            Gizmos.DrawLine(transform.position + new Vector3(0, 1.3f, 0), Player.transform.position);
        }

    }

}
