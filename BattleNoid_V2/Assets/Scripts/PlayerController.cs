using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed;             //이동 속도
    public float rotationSpeed;         //회전 속도
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

        //이동 항향 벡터를 기반으로 회전 각도를 계산
        if (moveInput != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveInput);

            //회전을 부드럽게 적용하기 위한 Slerp 를 사용
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        rigidbody.velocity = moveInput * moveSpeed;
    }
}
