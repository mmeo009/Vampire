using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Supporter;

public class ButtonManager
{
    // �ɼ� �����츦 ������ ģ��
    public UnityEngine.GameObject OptionsWindow;

    public void TestLoadEnum(DataType type)
    {
        Debug.Log(type.ToString());
    }
    //��ü������ ���� �̵� ���� ��  ����� ������� ������
    public void LoadGameScene(ActonType type, string SceneName = null)
    {

        if (type == ActonType.SceneMove)
        {
            if(SceneName != null)
            {
                // �� �ε� ������ ���⿡ �߰�
                SceneManager.LoadScene("LoadingScene");
                CoroutineManager.LoadSceneWithLoadingBar(SceneName);
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
            Managers.Data.SaveGmaeData(Managers.Data.gameData);
        }
        else if(type == ActonType.LoadGame) 
        {
            Managers.Data.LoadGameData();
        }
    }
}
