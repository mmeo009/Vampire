using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Supporter;

public class BulletController : MonoBehaviour
{
    public BulletDirection bulletType;
    public Vector3 direction;
    public float moveSpeed;
    public float timer;
    public float damage;
    private void OnEnable()
    {
        timer = Managers.Player.player.attackRange;
    }
    private void FixedUpdate()
    {
        Move();
        timer -= Time.fixedDeltaTime;
        if(timer <= 0)
        {
            DestroyBullet();
        }
    }
    private void Move()
    {
        float _moveSpeed = (moveSpeed) * Time.fixedDeltaTime;
        transform.Translate(direction * _moveSpeed);
    }
    public void DestroyBullet()
    {
        Managers.Player.bullets.Remove(this as BulletController);
        Managers.Pool.Destroy(this.gameObject);
    }
}
