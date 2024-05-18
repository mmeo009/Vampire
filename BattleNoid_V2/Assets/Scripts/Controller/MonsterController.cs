using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Supporter;
using DG.Tweening;

public class MonsterController : MonoBehaviour
{

    [SerializeField] private MonsterStats monster;
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
        MaintainDistance();

        if (isFreeze == true)
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

            if(bullet.bulletType == BulletType.Freeze)
            {
                Freeze(bullet.damage);
            }
            else
            {
                ChangeMonsterStats(OperationType.Minus, StatType.CurrentHP, bullet.damage);
            }

            bullet.DestroyBullet();

            ContactPoint cp = other.GetComponent<Collision>().GetContact(0);

            Quaternion rot = Quaternion.LookRotation(-cp.normal);

            var sparkEffect = Instantiate(Managers.Data.Load<GameObject>("Sparks") , cp.point, rot);

            Destroy(other.gameObject);
            Destroy(sparkEffect, 0.5f);

        }
    }

    private void Move()
    {
        if(monster != null && Player != null)
        {
            float moveSpeed = monster.moveSpeed;

            Collider[] nearbyMonsters = Physics.OverlapSphere(transform.position, 2);
            List<MonsterController> nearbyMonsterController = new List<MonsterController>();

            foreach(Collider collider in nearbyMonsters)
            {
                nearbyMonsterController.Add(collider.transform.GetComponent<MonsterController>());
            }

            if (nearbyMonsters.Length > 0)
            {
                // TODO : 몬스터 끼리 멀어지기
            }

            if (knockBackTimer > 0)
            {
                knockBackTimer -= Time.deltaTime;

                if (moveSpeed > 0)
                {
                    moveSpeed = - moveSpeed * monster.knockBackAmount;
                }

                if (knockBackTimer < 0)
                {
                    moveSpeed = Mathf.Abs(moveSpeed * 0.5f);
                }
            }
            Vector3 direction = (Player.transform.position - transform.position).normalized;

            if (isFreeze == false)
            {
                if (Vector3.Distance(Player.transform.position, transform.position) >= monster.attackRange)
                {
                    transform.position += direction * moveSpeed * Time.deltaTime;
                    isAttack = false;
                }
                else
                {
                    isAttack = true;
                }

                Vector3 targetDiraction = (Player.transform.position - this.transform.position).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(targetDiraction);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, monster.rotationSpeed * Time.deltaTime);
            }

        }
    }
    private void MaintainDistance()
    {
        Vector3 separation = Vector3.zero;
        int neighborCount = 0;

        foreach (var other in Managers.Monster.monsters)
        {
            if (other != this.gameObject)
            {
                float distance = Vector3.Distance(transform.position, other.transform.position);
                if (distance < 2f)
                {
                    Vector3 diff = transform.position - other.transform.position;
                    separation += diff.normalized / distance;
                    neighborCount++;
                }
            }
        }

        if (neighborCount > 0)
        {
            separation /= neighborCount;
        }

        transform.position += separation * 1f * Time.deltaTime;
    }
    private void Attack()
    {
        if (monster.attackType == 1)
        {
            if (GetMyRange() == 1)
            {
                Managers.Player.SetStats(OperationType.Minus, StatType.CurrentHP, monster.attackDamage);
                attackTimer = monster.attackSpeed;
            }
        }
        else if(monster.attackType == 2)
        {
            if (GetMyRange() == 1)
            {
                GameObject temp = Managers.Data.Instantiate("Bullet", null, true);
                BulletController bc = Util.GetOrAddComponent<BulletController>(temp);
                Managers.Data.bullets.Add(bc);
                bc.transform.position = attackPivot.transform.position;
                bc.bulletType = BulletType.Enemy;
                bc.direction = transform.forward;
                bc.moveSpeed = 3f;
                bc.range = monster.attackRange;

                attackTimer = monster.attackSpeed;
            }
        }
        else if(monster.attackType == 3)
        {
            Collider[] colls = Physics.OverlapSphere(transform.position, 2);
            foreach(Collider col in colls)
            {
                if(col.tag == "Player")
                {
                    Managers.Player.SetStats(OperationType.Minus, StatType.CurrentHP, monster.attackDamage);
                }
            }
            MonsterDie();
        }
    }

    public void Freeze(float amount)
    {
        isFreeze = true;
        freezeTimer = amount;
    }


    public void GetMonsterStats(MonsterStats _monster)
    {
        Player = Managers.Player.player.playerController;

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
        Managers.Monster.spawnedMonsterAmount--;
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
                // Debug.Log($"{gameObject.name}의 공격 범위안에 플레이어 있음");
                return 1;
            }
            else
            {
                // Debug.Log($"{gameObject.name}의 시야 범위안에 플레이어 있음");
                return 2;
            }
        }
        else
        {
            // Debug.Log("플레이어 없소");
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
