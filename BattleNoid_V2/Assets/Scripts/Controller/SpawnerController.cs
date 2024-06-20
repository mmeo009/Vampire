using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Supporter;

public class SpawnerController   : MonoBehaviour
{
    #region PrivateVariables
    [SerializeField] private float timer;
    [SerializeField] private int monsterIndex;
    [SerializeField] private float spawnerHp;
    #endregion
    #region PublicVariables
    public float radius = 1;
    #endregion
    private void OnEnable()
    {
        monsterIndex = Random.Range(1, Managers.Data.enemyDictionary.Count);
        spawnerHp = 100f;
        timer = 3f;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), radius);

        Gizmos.color = Color.red;

        Gizmos.DrawSphere(GetSpawnPos(transform.position, radius) + new Vector3(0,1,0), 0.2f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Bullet")
        {
            BulletController bullet = other.GetComponent<BulletController>();

            if (bullet.bulletType == BulletType.Freeze)
            {
                return;
            }
            else
            {
                spawnerHp -= bullet.damage;
                if(spawnerHp <= 0)
                {
                    Managers.Pool.Destroy(this.gameObject);
                }
            }

            Vector3 cP = other.ClosestPointOnBounds(transform.position);

            Quaternion rot = Quaternion.LookRotation(-cP);

            var sparkEffect = Instantiate(Managers.Data.Load<GameObject>("Sparks"), cP, rot);

            Destroy(sparkEffect, 0.5f);

            bullet.DestroyBullet();
        }
    }

    private Vector3 GetSpawnPos(Vector3 start,float radius)
    {
        float angle = Random.Range(0f, Mathf.PI * 2);

        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;

        Vector3 pointOnCircle = new Vector3(start.x + x, start.y, start.z + z);

        return pointOnCircle;
    }
    private void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;

            if(timer <= 0)
            {
                SpawnMonster();
            }
        }
    }
    #region PrivateMethod

    private void SpawnMonster()
    {
        if(Managers.Monster.monsters.Count >= 90)
        {
            timer = Managers.Monster.spawnedMonsterAmount * 5;
            return;
        }

        Vector3 pos = GetSpawnPos(transform.position, radius);
        Debug.Log(pos + ":" + transform.position);
        int playerLevel = Managers.Player.player.level / 3;

        int spawnAmount = Random.Range(0, 10);

        if (Managers.Monster.spawnedMonsterAmount > 30)
        {
            spawnAmount = Mathf.Clamp(spawnAmount, 1, 3);
        }

        for(int i = 0;i < spawnAmount; i++)
        {
            Managers.Monster.CreateMonster(pos, monsterIndex, null, playerLevel);
            Managers.Monster.spawnedMonsterAmount++;
        }

        timer = spawnAmount * (20 + Managers.Player.player.level);
    }
    #endregion
    #region PublicMethod

    #endregion
}
