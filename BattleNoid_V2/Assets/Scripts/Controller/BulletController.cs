using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Supporter;

public class BulletController : MonoBehaviour
{
    public BulletType bulletType;
    public Vector3 direction;
    public float moveSpeed;
    public float timer;
    public float damage;
    public float range;

    private void FixedUpdate()
    {
        Move();
        if(timer == -1)
        {
            timer = (range / moveSpeed);
        }
        if(timer > 0)
        {
            timer -= Time.fixedDeltaTime;
            if (timer <= 0)
            {
                DestroyBullet();
                timer = -1;
            }
        }
    }
    private void Move()
    {
        float _moveSpeed = (moveSpeed) * Time.fixedDeltaTime;
        transform.Translate(direction * _moveSpeed);
    }
    public void DestroyBullet()
    {
        Managers.Data.bullets.Remove(this as BulletController);
        Managers.Pool.Destroy(this.gameObject);
        this.transform.position = Vector3.zero;
    }
}
