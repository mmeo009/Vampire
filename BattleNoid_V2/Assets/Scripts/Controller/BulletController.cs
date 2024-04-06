using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Supporter;

public class BulletController : MonoBehaviour
{
    public BulletDirection bulletType;
    public Vector3 direction;
    public float moveSpeed;
    private void Attack()
    {
        float _moveSpeed = (moveSpeed) * Time.fixedDeltaTime;
        transform.Translate(direction * _moveSpeed);
    }
}
