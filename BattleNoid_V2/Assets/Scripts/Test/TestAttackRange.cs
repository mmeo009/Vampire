using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAttackRange : MonoBehaviour
{
    public float angle = 90f;
    public GameObject testPlayer;
    public float nowAngle = 0;
    public Vector3 toPlayer;


    void Update()
    {
        Vector3 myPos = transform.position + new Vector3(0,1.3f,0);
        toPlayer = testPlayer.transform.position - myPos;
        toPlayer.Normalize();

        float _angle = Vector3.Angle(transform.forward, toPlayer);
        nowAngle = _angle;

        if (_angle < angle * 0.5f )
        {
            if(Vector3.Distance(myPos, testPlayer.transform.position) <= 1)
            {
                Debug.Log("적이 나의 안에 있소");
            }
            else
            {
                Debug.Log("적이 시야에 만 있소");
            }    
        }
        else
        {
            Debug.Log("적이 밖에 있소");
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position + new Vector3(0, 1.3f, 0), testPlayer.transform.position);
        DrawLine();
    }
    void DrawLine()
    {
        Gizmos.color = Color.magenta;
        float halfPlusAngle = angle * 0.5f + transform.eulerAngles.y;
        float halfMinusAngle = -angle * 0.5f + transform.eulerAngles.y;
        Vector3 angleRight = new Vector3(Mathf.Sin(halfPlusAngle * Mathf.Deg2Rad), 0, Mathf.Cos(halfPlusAngle * Mathf.Deg2Rad));
        Vector3 angleLeft = new Vector3(Mathf.Sin(halfMinusAngle * Mathf.Deg2Rad), 0, Mathf.Cos(halfMinusAngle * Mathf.Deg2Rad));

        Vector3 end1 = transform.position + new Vector3(0, 1.3f, 0) + angleRight * 1f;
        Vector3 end2 = transform.position + new Vector3(0, 1.3f, 0) + angleLeft * 1f;
        Gizmos.DrawLine(transform.position + new Vector3(0, 1.3f, 0), end1);
        Gizmos.DrawLine(transform.position + new Vector3(0, 1.3f, 0), end2);
        Gizmos.DrawLine(transform.position + new Vector3(0, 1.3f, 0), transform.position + new Vector3(0, 1.3f, 0) + transform.forward * 1f);
    }

}
