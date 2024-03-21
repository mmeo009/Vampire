using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public void ExitGame()
    {

        // 에디터에서는 동작안함
        Debug.Log("Exit 스크립트는 에디터에서 작동안해요 아마도여");

        // 빌드된 게임에서는 동작함 봉붕어에서 가져옴
        Application.Quit();

    }
}
