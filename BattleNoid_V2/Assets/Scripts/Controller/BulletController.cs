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
    private void OnEnable()
    {
        timer = Managers.Player.player.attackRange;
        Debug.Log("»ý¼º!");
    }
    private void FixedUpdate()
    {
        Attack();
        timer -= Time.fixedDeltaTime;
        if(timer <= 0)
        {
            Debug.Log("»ç¸Á!");
            Managers.Player.bullets.Remove(this as BulletController);
            Managers.Pool.Destroy(this.gameObject);
        }
    }
    private void Attack()
    {
        float _moveSpeed = (moveSpeed) * Time.fixedDeltaTime;
        transform.Translate(direction * _moveSpeed);
    }
}
