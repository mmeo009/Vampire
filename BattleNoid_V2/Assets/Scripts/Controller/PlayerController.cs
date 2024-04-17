using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{

    [SerializeField] private float attackTimer;
    [SerializeField] private Vector3 moveInput = Vector3.zero;
    [SerializeField] private Rigidbody playerRigidbody;

    public float lineLength = 1f;
    public float lineWidth = 0.1f;
    public int dotAmount = 5;
    public Vector3 cubeVector = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
        CoolDown();

        if (this.transform.position.y != 0)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
    }

    public void PlayerMove()
    {
        if(playerRigidbody == null)
            playerRigidbody = GetComponent<Rigidbody>();

        // ���� ������
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.z = Input.GetAxisRaw("Vertical");

        // �밢�� ���� ����
        moveInput.Normalize();

        //�̵� ���� ���͸� ������� ȸ�� ������ ���
        if (moveInput != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveInput);

            //ȸ���� �ε巴�� �����ϱ� ���� Slerp �� ���
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Managers.Player.player.rotationSpeed * Time.deltaTime);
        }

        playerRigidbody.velocity = moveInput * Managers.Player.player.moveSpeed;
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
        Vector3 playerPosition = Managers.Player.player.playerController.transform.position;
        Quaternion playerRotation = Managers.Player.player.playerController.transform.rotation;

        // ���μ�
        DrawLine(playerPosition, playerRotation * Vector3.right, Color.red);
        // ���μ�
        DrawLine(playerPosition, playerRotation * Vector3.forward, Color.blue);

        DrawCube(cubeVector);
    }

    public void DrawCube(Vector3 point)
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
}
