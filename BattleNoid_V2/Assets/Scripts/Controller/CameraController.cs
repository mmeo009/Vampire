using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniGLTF.Extensions.VRMC_vrm;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera miniMapCamera;
    [SerializeField] private Camera mainCamera;
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
        
        if(miniMapCamera == null)
        {
            miniMapCamera = GameObject.FindWithTag("MiniMapCamera")?.GetComponent<Camera>();
            if (miniMapCamera == null)
            {
                var minimap = Managers.Data.Instantiate("MiniMapCamera", null, false);
                minimap.transform.LookAt(Player.transform.position);
                miniMapCamera = minimap.GetComponent<Camera>();
            }
        }
    }

    private void FollowPlayer()
    {
        if (Player != null && mainCamera != null)
        {
            Vector3 playerPos = Player.transform.position;
            mainCamera.transform.position = new Vector3(playerPos.x, playerPos.y + 8, playerPos.z - 8);
        }
        if(Player != null && miniMapCamera != null)
        {
            Vector3 playerPos = Player.transform.position;
            miniMapCamera.transform.position = new Vector3(playerPos.x, playerPos.y + 20, playerPos.z);
        }
    }
}
