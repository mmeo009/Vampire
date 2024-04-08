using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera camera;
    public PlayerController player;

    public void Update()
    {
        FallowPlayer();
    }
    public void FindCamera()
    {
        player = Managers.Player.player.playerController;
        if(camera == null)
        {
            camera = Camera.main;
            if(camera == null )
            {
                camera = GameObject.Find("Main Camera").GetComponent<Camera>();
                if(camera == null )
                {
                    camera = new Camera();
                }
            }
        }
    }
    private void FallowPlayer()
    {
        if (player != null && camera != null)
        {
            Vector3 playerPos = player.transform.position;
            camera.transform.position = new Vector3(playerPos.x, playerPos.y + 6, playerPos.z - 10);
        }
        else
        {
            FindCamera();
        }
    }
   
}
