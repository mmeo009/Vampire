using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{

    [SerializeField] private float attackTimer;
    [SerializeField] private Vector3 moveInput = Vector3.zero;
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private Quaternion previousRotation = Quaternion.identity;

    public float lineLength = 1f;
    public float lineWidth = 0.1f;
    public int dotAmount = 5;
    public Vector3 cubeVector = Vector3.zero;

    void Update()
    {
        PlayerMove();
        CoolDown();

        if (this.transform.position.y != 0)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }

        if(Managers.Player.player.currentFirstCoolDown > 0)
        {
            Managers.Player.player.currentFirstCoolDown -= Time.deltaTime;
            if(Managers.Player.player.currentFirstCoolDown <= 0)
            {
                Managers.Player.player.isFirstSkillActive = true;
            }
        }

        if (Managers.Player.player.currentSecondCoolDown > 0)
        {
            Managers.Player.player.currentSecondCoolDown -= Time.deltaTime;
            if (Managers.Player.player.currentSecondCoolDown <= 0)
            {
                Managers.Player.player.isSecondSkillActive = true;
            }
        }
    }

    public void PlayerMove()
    {
        if(playerRigidbody == null)
            playerRigidbody = GetComponent<Rigidbody>();

        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.z = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();

        if (moveInput != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveInput);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Managers.Player.player.rotationSpeed * Time.deltaTime);

            previousRotation = transform.rotation;
        }
        else
        {
            transform.rotation = previousRotation;
        }

        playerRigidbody.velocity = moveInput * Managers.Player.player.moveSpeed;

        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(Managers.Player.player.isFirstSkillActive == true)
            {
                Managers.Player.player.currentFirstCoolDown = Managers.Player.player.firstCoolDown;
                Managers.Player.player.isFirstSkillActive = false;

                HashSet<MonsterController> monsters = new HashSet<MonsterController>(Managers.Monster.monsters);

                foreach (MonsterController mc in monsters)
                {
                    if (IsEnemyInsideOBB(mc.transform.position, transform.rotation, 2, 4) == true)
                    {
                        Managers.Player.UseFirstSkill(mc, 300);
                        Debug.Log(mc.transform);
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Managers.Player.player.isSecondSkillActive == true)
            {
                Managers.Player.UseSecondSkill();
                Managers.Player.player.currentSecondCoolDown = Managers.Player.player.secondCoolDown;
                Managers.Player.player.isSecondSkillActive = false;
            }
        }
    }
    private void CoolDown()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0)
        {
            Attack();
            attackTimer = Managers.Player.player.attackSpeed;
        }
    }


    void OnDrawGizmos()
    {
        DrawLine(transform.position, transform.rotation * Vector3.right, Color.red);

        DrawLine(transform.position, transform.rotation * Vector3.forward, Color.blue);

        DrawAttackPivot(cubeVector);

        DrawOBB(transform.rotation, 2, 4);
    }

    public void DrawAttackPivot(Vector3 point)
    {
        Gizmos.DrawCube(point, new Vector3(0.1f, 0.1f, 0.1f));
    }

    public void DrawLine(Vector3 start, Vector3 direction, Color color)
    {
        Gizmos.color = color;
        Vector3 end1 = start + direction * (lineLength / 2);
        Vector3 end2 = start - direction * (lineLength / 2);
        Gizmos.DrawLine(end1, end2);

        float segmentLength = lineLength / (dotAmount - 1);

        for (int i = 0; i < dotAmount; i++)
        {
            Vector3 point = end1 - direction * (segmentLength * i);
            Gizmos.DrawSphere(point, 0.1f);
        }
    }

    private void Attack()
    {
        Managers.Player.Attack();
    }
    public void DrawOBB(Quaternion rotation, float width, float length)
    {
        Gizmos.color = Color.green;

        float halfWidth = width / 2f;

        Vector3[] points = new Vector3[]
        {
        transform.position + rotation * new Vector3(-halfWidth, 1f, 0f),
        transform.position + rotation * new Vector3(halfWidth, 1f, 0f),
        transform.position + rotation * new Vector3(halfWidth, 1f, length),
        transform.position + rotation * new Vector3(-halfWidth, 1f, length)
        };

        Gizmos.DrawLine(points[0], points[1]);
        Gizmos.DrawLine(points[1], points[2]);
        Gizmos.DrawLine(points[2], points[3]);
        Gizmos.DrawLine(points[3], points[0]);
    }
    public bool IsEnemyInsideOBB(Vector3 point, Quaternion rotation, float width, float length)
    {
        float halfWidth = width / 2f;
        float halfLength = length / 2f;

        Vector3 center = this.transform.position + rotation * new Vector3(0f, 0f, halfLength);

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
