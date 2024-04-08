using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{

    private float moveSpeed;             //�̵� �ӵ�
    private float rotationSpeed;         //ȸ�� �ӵ�
    private float attackCooldown;
    [SerializeField]
    private float attackTimer;
    public Vector3 moveInput = Vector3.zero;
    public Rigidbody playerRigidbody;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
        CoolDown();
    }
    public void LoadData()
    {
        moveSpeed = Managers.Player.player.moveSpeed;
        rotationSpeed = Managers.Player.player.rotationSpeed;
        attackCooldown = Managers.Player.player.attackSpeed;
    }
    public void PlayerMove()
    {
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
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        playerRigidbody.velocity = moveInput * moveSpeed;
    }
    private void CoolDown()
    {
        attackTimer -= Time.deltaTime;
        if(attackTimer <= 0)
        {
            Attack();
            attackTimer = attackCooldown;
        }
    }
    private void Attack()
    {
        Managers.Player.Attack();
    }
}
