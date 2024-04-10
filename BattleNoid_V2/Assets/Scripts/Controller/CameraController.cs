using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera mainCamera; // ������ ����
    public PlayerController player;

    private void Update()
    {
        FollowPlayer();
    }

    public void FindCamera()
    {
        if (player == null)
        {
            player = Managers.Player.player.playerController;
            if(player == null)
            {
                Debug.LogError("�÷��̾� ��Ʈ�ѷ��� ����!");
                return;
            }
        }

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                mainCamera = GameObject.FindWithTag("MainCamera")?.GetComponent<Camera>(); // �±׷� ī�޶� ã��
                if (mainCamera == null)
                {
                    Debug.LogError("Main camera not found in the scene!");
                }
            }
        }
    }

    private void FollowPlayer()
    {
        if (player != null && mainCamera != null)
        {
            Vector3 playerPos = player.transform.position;
            mainCamera.transform.position = new Vector3(playerPos.x, playerPos.y + 6, playerPos.z - 10);
        }
    }
}
