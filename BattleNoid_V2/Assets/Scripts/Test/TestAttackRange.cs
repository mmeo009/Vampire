using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class TestAttackRange : MonoBehaviour
{
    public float angle = 90f;
    public GameObject testPlayer;
    public float nowAngle = 0;
    public Vector3 toPlayer;
    public bool IsAttack;
    public float dot;

    void Update()
    {
        
        //IsAttack = IsEnemyInsideOBB(testPlayer.transform.position, transform.position, transform.rotation, 1, 2);
        AttackMelee(transform.forward, new Vector3(testPlayer.transform.position.x, 0, testPlayer.transform.position.z), 1);

    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position + new Vector3(0, 1.3f, 0), testPlayer.transform.position);
        //DrawLine();
        DrawOBB(transform.position, transform.rotation, 1, 2);

    }
    void AttackMelee(Vector3 startPos, Vector3 targetPos, float attackRange)
    {
        Vector3 toTarget = targetPos - transform.position;
        dot = Vector3.Dot(startPos, toTarget.normalized);

        if (dot <= 1)
        {
            if(Vector3.Distance(startPos, targetPos) <= attackRange)
            {
                Debug.Log("으앗!");
            }
            else
            {
                Debug.Log("뜨앗!");
            }
        }
        else
        {
            Debug.Log("으잇!");
        }

    }

    void Check()
    {
        Vector3 myPos = transform.position + new Vector3(0, 1.3f, 0);
        toPlayer = testPlayer.transform.position - myPos;
        toPlayer.Normalize();

        float _angle = Vector3.Angle(transform.forward, toPlayer);
        nowAngle = _angle;

        if (_angle < angle * 0.5f)
        {
            if (Vector3.Distance(myPos, testPlayer.transform.position) <= 1)
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

    public void DrawOBB(Vector3 start, Quaternion rotation, float width, float length)
    {
        Gizmos.color = Color.green;

        float halfWidth = width / 2f;

        Vector3[] points = new Vector3[]
        {
        start + rotation * new Vector3(-halfWidth, 1.3f, 0f),
        start + rotation * new Vector3(halfWidth, 1.3f, 0f),
        start + rotation * new Vector3(halfWidth, 1.3f, length),
        start + rotation * new Vector3(-halfWidth, 1.3f, length)
        };

        // Draw the OBB
        Gizmos.DrawLine(points[0], points[1]);
        Gizmos.DrawLine(points[1], points[2]);
        Gizmos.DrawLine(points[2], points[3]);
        Gizmos.DrawLine(points[3], points[0]);
    }
    public bool IsEnemyInsideOBB(Vector3 point, Vector3 start, Quaternion rotation, float width, float length)
    {
        float halfWidth = width / 2f;
        float halfLength = length / 2f;

        Vector3 center = start + rotation * new Vector3(0f, 0f, halfLength);

        Vector3 localPoint = Quaternion.Inverse(rotation) * (point - center);


        if (Mathf.Abs(localPoint.x) <= halfWidth && Mathf.Abs(localPoint.z) <= halfLength)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
