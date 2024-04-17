using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{

    [SerializeField] private float attackTimer;
    [SerializeField] private Vector3 moveInput = Vector3.zero;
    [SerializeField] private Rigidbody playerRigidbody;

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
        CoolDown();
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
        if(attackTimer <= 0)
        {
            Attack();
            attackTimer = Managers.Player.player.attackSpeed;
        }
    }
    private void Attack()
    {
        Managers.Player.Attack();
    }
}
