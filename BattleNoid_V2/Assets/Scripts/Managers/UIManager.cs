using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Supporter;

[System.Serializable]
public class UIManager
{
    // UI ������
    public GameObject optionWindowPrefab;
    public GameObject cardWindowPrefab;

    public GameObject optionWindow;
    public GameObject cardWindow;

    public void TestLoadEnum(DataType type)
    {
        Debug.Log(type.ToString());
    }

    // ��ü������ ��� �� �̵� ���� �� ���⿡ ������� ������
    public void ButtonAction(ActionType type, string taskString = null)
    {
        if (type == ActionType.SceneMove)
        {
            if (taskString != null)
            {
                // �� �ε� ������ ���⿡ �߰�
                SceneManager.LoadScene("LoadingScene");
                optionWindow = null;
                CoroutineManager.LoadSceneWithLoadingBar(taskString);
            }
            else
            {
                Debug.LogError("�� �̸��� ���Ե��� �ʾҽ��ϴ�.");
                taskString = "StartScene";
                SceneManager.LoadScene(taskString);
            }
        }
        else if (type == ActionType.ExitGame)
        {
            // �����Ϳ����� ���� ����
            Debug.Log("Exit ��ũ��Ʈ�� �����Ϳ��� �۵����ؿ� �Ƹ�����");

            // ����� ���ӿ����� ������ ���ؾ�� ������
            Application.Quit();
        }
        else if (type == ActionType.PauseGame && taskString != null)
        {

            // �ð��� �������� �ʾҴٸ�
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                if (taskString == "OptionsWindow")
                {
                    if(optionWindow == null)
                    {
                        optionWindow = LoadWindow(optionWindowPrefab);
                    }
                    optionWindow.SetActive(true);
                }
                else if (taskString == "CardWindow")
                {
                    if (cardWindow == null)
                    {
                        cardWindow = LoadWindow(cardWindowPrefab);
                    }
                    cardWindow.SetActive(true);
                }
            }
            else
            {       // �ð��� ���� �� �ִٸ�
                if (taskString == "OptionsWindow")
                {
                    optionWindow.SetActive(false);
                    if (cardWindow!= null)
                    {
                        if(cardWindow.activeInHierarchy == false)
                        {
                            Time.timeScale = 1;
                        }
                        Time.timeScale = 1;
                    }
                    Time.timeScale = 1;
                }
                else if (taskString == "CardWindow")
                {
                    cardWindow.SetActive(false);
                    if (optionWindow != null)
                    {
                        if (optionWindow.activeInHierarchy == false)
                        {
                            Time.timeScale = 1;
                        }
                        Time.timeScale = 1;
                    }
                    Time.timeScale = 1;
                }
            }
            Debug.Log(Time.timeScale);
        }
        else if (type == ActionType.SaveGame)
        {
            Managers.Data.SaveGameData(Managers.Data.gameData);
        }
        else if (type == ActionType.LoadGame)
        {
            Managers.Data.LoadGameData();
        }
        else if(type == ActionType.LoadWaveData)
        {
            Managers.Monster.GetWaveDatas();
        }
    }

    private GameObject LoadWindow(GameObject gameObjectPrefab)
    {
        if(gameObjectPrefab.name == "OptionsWindow")
        {
            if (optionWindow == null)
            {
                if (gameObjectPrefab != null)
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
            else
            {
                return null;
            }
        }
        else if(gameObjectPrefab.name == "CardWindow")
        {
            if (cardWindow == null)
            {
                if (gameObjectPrefab != null)
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
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }


    }
}