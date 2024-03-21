using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public void ExitGame()
    {

        // 에디터에서는 동작안함
        Debug.Log("게임 종료 스크립트는 에디터에서 동작하지 않습니다.");

        // 빌드된 게임에서는 동작함 봉붕어에서 가져옴
        Application.Quit();

    }
}
