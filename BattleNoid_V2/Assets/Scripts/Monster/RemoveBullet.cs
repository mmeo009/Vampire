using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{

    public GameObject sparkEffect;

    void OnCollisionEnter(Collision coll)
    {
        // 충돌한 게임오브젝트의 태그값 비교
        if (coll.collider.CompareTag("BULLET"))
        {

            ContactPoint cp = coll.GetContact(0);

            Quaternion rot = Quaternion.LookRotation(-cp.normal);

            Instantiate(sparkEffect, cp.point, rot);

            Destroy(coll.gameObject);

            GameObject spark = Instantiate(sparkEffect, cp.point, rot);
            //  파티클 삭제
            Destroy(spark, 0.5f);
        }
    }
}
