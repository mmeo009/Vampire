using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{

    public GameObject sparkEffect;

    void OnCollisionEnter(Collision coll)
    {
        // �浹�� ���ӿ�����Ʈ�� �±װ� ��
        if (coll.collider.CompareTag("BULLET"))
        {

            ContactPoint cp = coll.GetContact(0);

            Quaternion rot = Quaternion.LookRotation(-cp.normal);

            Instantiate(sparkEffect, cp.point, rot);

            Destroy(coll.gameObject);

            GameObject spark = Instantiate(sparkEffect, cp.point, rot);
            //  ��ƼŬ ����
            Destroy(spark, 0.5f);
        }
    }
}
