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
