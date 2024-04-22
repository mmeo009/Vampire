using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TestAttackRange : MonoBehaviour
{
    public float angle = 90f;
    public GameObject testPlayer;
    public float nowAngle = 0;
    public Vector3 toPlayer;
    public bool IsAttack;

    public float width;
    public float length;

    public Vector3 startPos;
    public Vector3 target;


    void Update()
    {
        IsAttack = IsEnemyInsideOBB(testPlayer.transform.position, transform.position, transform.rotation, 1, 2);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position + new Vector3(0, 1.3f, 0), testPlayer.transform.position);
        //DrawLine();
        DrawOBB(transform.position, transform.rotation, 1, 2);

        DrawTriangle();
    }

    void DrawTriangle()
    {
        // �ﰢ���� �� ������ ���
        Vector3 pointA = startPos + Quaternion.Euler(0, -90, 0) * (Vector3.forward * width / 2);
        Vector3 pointB = startPos + Quaternion.Euler(0, 90, 0) * (Vector3.forward * width / 2);
        Vector3 pointC = startPos + (Vector3.forward * length);

        // �ﰢ�� �׸���
        Gizmos.color = Color.green;
        Gizmos.DrawLine(pointA, pointB);
        Gizmos.DrawLine(pointB, pointC);
        Gizmos.DrawLine(pointC, pointA);

        // �־��� �� �׸���
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(target, 0.1f);

        // �־��� ���� �ﰢ�� ���ο� �ִ��� Ȯ��
        if (IsPointInRange(startPos, width, length, target))
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(startPos, target);
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
                Debug.Log("���� ���� �ȿ� �ּ�");
            }
            else
            {
                Debug.Log("���� �þ߿� �� �ּ�");
            }
        }
        else
        {
            Debug.Log("���� �ۿ� �ּ�");
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

    // �ﰢ�� ���ο� �־��� ���� �ִ��� Ȯ���ϴ� �Լ�
    private bool IsPointInRange(Vector3 startPos, float width, float length, Vector3 target)
    {
        // �־��� ������ ���� �ﰢ���� �� �� ���
        Vector3 pointA = startPos + Quaternion.Euler(0, -90, 0) * (Vector3.forward * width / 2);
        Vector3 pointB = startPos + Quaternion.Euler(0, 90, 0) * (Vector3.forward * width / 2);
        Vector3 pointC = startPos + (Vector3.forward * length);

        // �־��� ���� �� ���� �������� Ȯ��
        bool sideAB = IsOnSameSide(pointA, pointB, target, startPos);
        bool sideBC = IsOnSameSide(pointB, pointC, target, startPos);
        bool sideCA = IsOnSameSide(pointC, pointA, target, startPos);

        // �־��� ���� �� ���� ������ ��� �ﰢ�� ���ο� ����
        return sideAB && sideBC && sideCA;
    }

    // �־��� �� ���� ������ ������ ���� ���� �ʿ� �ִ��� Ȯ���ϴ� �Լ�
    private bool IsOnSameSide(Vector3 pointA, Vector3 pointB, Vector3 target, Vector3 startPos)
    {
        Vector3 dir = Vector3.Cross((pointB - pointA), (startPos - pointA));
        float dot = Vector3.Dot(dir, target - pointA);
        return dot >= 0;
    }
}
