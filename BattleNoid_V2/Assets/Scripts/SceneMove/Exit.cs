using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public void ExitGame()
    {

        // �����Ϳ����� ���۾���
        Debug.Log("Exit ��ũ��Ʈ�� �����Ϳ��� �۵����ؿ� �Ƹ�����");

        // ����� ���ӿ����� ������ ���ؾ�� ������
        Application.Quit();

    }
}
