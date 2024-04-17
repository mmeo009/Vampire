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

        // 축을 가져옴
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.z = Input.GetAxisRaw("Vertical");

        // 대각선 가속 방지
        moveInput.Normalize();

        //이동 항향 벡터를 기반으로 회전 각도를 계산
        if (moveInput != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveInput);

            //회전을 부드럽게 적용하기 위한 Slerp 를 사용
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

        // 가로선
        DrawLine(playerPosition, playerRotation * Vector3.right, Color.red);
        // 세로선
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
