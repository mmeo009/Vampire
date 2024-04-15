using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Supporter;

[System.Serializable]
public class ButtonManager
{
    // UI윈도우
    public UnityEngine.GameObject OptionsWindowPrefab;
    public UnityEngine.GameObject CardWindowPrefab;

    public UnityEngine.GameObject OptionsWindow;
    public UnityEngine.GameObject CardWindow;

    public void TestLoadEnum(DataType type)
    {
        Debug.Log(type.ToString());
    }
    //대체적으로 모든씬 이동 로직 및  여기다 적어놓을 예정임
    public void LoadGameScene(ActonType type, string taskString = null)
    {

        if (type == ActonType.SceneMove)
        {
            if (taskString != null)
            {
                // 씬 로딩 로직은 여기에 추가
                SceneManager.LoadScene("LoadingScene");
                CoroutineManager.LoadSceneWithLoadingBar(taskString);
            }
            else
            {
                Debug.LogError("씬 이름이 기입되지 않았습니다.");
                taskString = "StartScene";
                SceneManager.LoadScene(taskString);
            }

        }
        else if (type == ActonType.ExitGame)
        {
            // 에디터에서는 동작안함
            Debug.Log("Exit 스크립트는 에디터에서 작동안해요 아마도여");

            // 빌드된 게임에서는 동작함 봉붕어에서 가져옴
            Application.Quit();
        }
        else if (type == ActonType.PauseGame && taskString != null)
        {
            // 시간이 정지하지 않았다면
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
            {       // 시간이 정지 해 있다면

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
