using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public void ExitGame()
    {

        // �����Ϳ����� ���۾���
        Debug.Log("���� ���� ��ũ��Ʈ�� �����Ϳ��� �������� �ʽ��ϴ�.");

        // ����� ���ӿ����� ������ ���ؾ�� ������
        Application.Quit();

    }
}
