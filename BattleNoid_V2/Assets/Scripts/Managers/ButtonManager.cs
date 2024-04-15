using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Supporter;

[System.Serializable]
public class ButtonManager
{
    // UI������
    public UnityEngine.GameObject OptionsWindowPrefab;
    public UnityEngine.GameObject CardWindowPrefab;

    public UnityEngine.GameObject OptionsWindow;
    public UnityEngine.GameObject CardWindow;

    public void TestLoadEnum(DataType type)
    {
        Debug.Log(type.ToString());
    }
    //��ü������ ���� �̵� ���� ��  ����� ������� ������
    public void LoadGameScene(ActonType type, string taskString = null)
    {

        if (type == ActonType.SceneMove)
        {
            if (taskString != null)
            {
                // �� �ε� ������ ���⿡ �߰�
                SceneManager.LoadScene("LoadingScene");
                CoroutineManager.LoadSceneWithLoadingBar(taskString);
            }
            else
            {
                Debug.LogError("�� �̸��� ���Ե��� �ʾҽ��ϴ�.");
                taskString = "StartScene";
                SceneManager.LoadScene(taskString);
            }

        }
        else if (type == ActonType.ExitGame)
        {
            // �����Ϳ����� ���۾���
            Debug.Log("Exit ��ũ��Ʈ�� �����Ϳ��� �۵����ؿ� �Ƹ�����");

            // ����� ���ӿ����� ������ ���ؾ�� ������
            Application.Quit();
        }
        else if (type == ActonType.PauseGame && taskString != null)
        {
            // �ð��� �������� �ʾҴٸ�
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                if(taskString == "OptionsWindow")
                {
                    OptionsWindow = LoadWindow(OptionsWindowPrefab);
                    OptionsWindow.SetActive(true);
                }
                else if(taskString == "CardWindow")
                {
                    CardWindow = LoadWindow(CardWindowPrefab);
                    CardWindow.SetActive(true);
                }

            }
            else
            {       // �ð��� ���� �� �ִٸ�

                if (taskString == "OptionsWindow")
                {
                    OptionsWindow.SetActive(false);
                    if (CardWindow.activeInHierarchy == false)
                    {
                        Time.timeScale = 1;
                    }
                }
                else if( taskString == "CardWindow")
                {
                    CardWindow.SetActive(false);
                    if(OptionsWindow.activeInHierarchy == false)
                    {
                        Time.timeScale = 1;
                    }

                }
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

    private GameObject LoadWindow(GameObject gameObjectPrefab)
    {
        if(gameObjectPrefab != null)
        {
            GameObject temp = GameObject.Instantiate<GameObject>(gameObjectPrefab);
            temp.transform.parent = GameObject.FindGameObjectWithTag("Canvas").transform;
            temp.transform.localPosition = Vector3.zero;
            return temp;
        }
        else
        {
            return null;
        }

    }
}
