
using UnityEngine;
using UnityEngine.SceneManagement;


public class ButtonManager
{
    public enum ActonType
    {
        SceneMove,
        ExitGame,
        PauseGame,
        SaveGame,
        LoadGame,
        DefaultAction
    }
    // �ɼ� �����츦 ������ ģ��
    public UnityEngine.GameObject OptionsWindow;

    public void Test()
    {
        LoadGameScene(ActonType.ExitGame);
    }

    //��ü������ ���� �̵� ���� ��  ����� ������� ������
    public void LoadGameScene(ActonType type, string SceneName = null)
    {
        if(type == ActonType.SceneMove)
        {
            if(SceneName != null)
            {
                // �� �ε� ������ ���⿡ �߰�
                SceneManager.LoadScene(SceneName);
            }
            else
            {
                Debug.LogError("�� �̸��� ���Ե��� �ʾҽ��ϴ�.");
                SceneName = "StartScene";
                SceneManager.LoadScene(SceneName);
            }

        }
        else if (type == ActonType.ExitGame)
        {
            // �����Ϳ����� ���۾���
            Debug.Log("Exit ��ũ��Ʈ�� �����Ϳ��� �۵����ؿ� �Ƹ�����");

            // ����� ���ӿ����� ������ ���ؾ�� ������
            Application.Quit();
        }
        else if (type == ActonType.PauseGame)
        {
            // �ð��� �������� �ʾҴٸ�
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                OptionsWindow.SetActive(true);
            }
            else
            {       // �ð��� ���� �� �ִٸ�
                Time.timeScale = 1;
                OptionsWindow.SetActive(false);
            }
        }
        else if(type == ActonType.SaveGame)
        {
           // Managers.Data.SaveGameData();         //Todo : ���� ���
        }
        else if(type == ActonType.LoadGame) 
        {
            // Managers.Data.LoadGameData();        //Todo : ������ ������ �ҷ����� ���
        }
    }
}
