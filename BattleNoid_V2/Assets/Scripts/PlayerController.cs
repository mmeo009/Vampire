using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed;             //�̵� �ӵ�
    public float rotationSpeed;         //ȸ�� �ӵ�
    public Vector3 moveInput = Vector3.zero;
    public Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        if (rigidbody == null)
        {
             rigidbody = GetComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
    }

    public void PlayerMove()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.z = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();

        //�̵� ���� ���͸� ������� ȸ�� ������ ���
        if (moveInput != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveInput);

            //ȸ���� �ε巴�� �����ϱ� ���� Slerp �� ���
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        rigidbody.velocity = moveInput * moveSpeed;
    }
}
