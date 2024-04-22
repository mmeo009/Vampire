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
        // 삼각형의 세 꼭지점 계산
        Vector3 pointA = startPos + Quaternion.Euler(0, -90, 0) * (Vector3.forward * width / 2);
        Vector3 pointB = startPos + Quaternion.Euler(0, 90, 0) * (Vector3.forward * width / 2);
        Vector3 pointC = startPos + (Vector3.forward * length);

        // 삼각형 그리기
        Gizmos.color = Color.green;
        Gizmos.DrawLine(pointA, pointB);
        Gizmos.DrawLine(pointB, pointC);
        Gizmos.DrawLine(pointC, pointA);

        // 주어진 점 그리기
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(target, 0.1f);

        // 주어진 점이 삼각형 내부에 있는지 확인
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

    // 삼각형 내부에 주어진 점이 있는지 확인하는 함수
    private bool IsPointInRange(Vector3 startPos, float width, float length, Vector3 target)
    {
        // 주어진 각도에 따라 삼각형의 세 점 계산
        Vector3 pointA = startPos + Quaternion.Euler(0, -90, 0) * (Vector3.forward * width / 2);
        Vector3 pointB = startPos + Quaternion.Euler(0, 90, 0) * (Vector3.forward * width / 2);
        Vector3 pointC = startPos + (Vector3.forward * length);

        // 주어진 점이 세 변을 지나는지 확인
        bool sideAB = IsOnSameSide(pointA, pointB, target, startPos);
        bool sideBC = IsOnSameSide(pointB, pointC, target, startPos);
        bool sideCA = IsOnSameSide(pointC, pointA, target, startPos);

        // 주어진 점이 세 변을 지나는 경우 삼각형 내부에 있음
        return sideAB && sideBC && sideCA;
    }

    // 주어진 두 점과 시작점 사이의 점이 같은 쪽에 있는지 확인하는 함수
    private bool IsOnSameSide(Vector3 pointA, Vector3 pointB, Vector3 target, Vector3 startPos)
    {
        Vector3 dir = Vector3.Cross((pointB - pointA), (startPos - pointA));
        float dot = Vector3.Dot(dir, target - pointA);
        return dot >= 0;
    }
}
