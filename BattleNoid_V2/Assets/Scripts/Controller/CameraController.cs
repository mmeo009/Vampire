using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera mainCamera; // 변수명 변경
    public PlayerController Player;

    private void Update()
    {
        FollowPlayer();
    }

    public void FindCamera()
    {
        Player = Managers.Player.player.playerController;
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                mainCamera = GameObject.FindWithTag("MainCamera")?.GetComponent<Camera>(); // 태그로 카메라 찾음
                if (mainCamera == null)
                {
                    Debug.LogError("Main camera not found in the scene!");
                }
            }
        }
    }

    private void FollowPlayer()
    {
        if (Player != null && mainCamera != null)
        {
            Vector3 playerPos = Player.transform.position;
            mainCamera.transform.position = new Vector3(playerPos.x, playerPos.y + 6, playerPos.z - 10);
        }
    }
}
