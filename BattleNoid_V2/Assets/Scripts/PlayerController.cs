using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{

    public float moveSpeed;             //�̵� �ӵ�
    public float rotationSpeed;         //ȸ�� �ӵ�
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
}
