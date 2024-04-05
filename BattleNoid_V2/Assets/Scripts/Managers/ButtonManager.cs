using UnityEngine;
using UnityEngine.SceneManagement;
using Supporter;


public class ButtonManager
{
    // 옵션 윈도우를 가져올 친구
    public UnityEngine.GameObject OptionsWindow;

    public void Test()
    {
        LoadGameScene(ActonType.ExitGame, null);
    }

    public void TestLoadEnum(DataType type)
    {
        Debug.Log(type.ToString());
    }
    //대체적으로 모든씬 이동 로직 및  여기다 적어놓을 예정임
    [SerializeField]
    public void LoadGameScene(ActonType type, string SceneName = null)
    {

        if (type == ActonType.SceneMove)
        {
            if(SceneName != null)
            {
                // 씬 로딩 로직은 여기에 추가
                SceneManager.LoadScene(SceneName);
            }
            else
            {
                Debug.LogError("씬 이름이 기입되지 않았습니다.");
                SceneName = "StartScene";
                SceneManager.LoadScene(SceneName);
            }

        }
        else if (type == ActonType.ExitGame)
        {
            // 에디터에서는 동작안함
            Debug.Log("Exit 스크립트는 에디터에서 작동안해요 아마도여");

            // 빌드된 게임에서는 동작함 봉붕어에서 가져옴
            Application.Quit();
        }
        else if (type == ActonType.PauseGame)
        {
            // 시간이 정지하지 않았다면
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                OptionsWindow.SetActive(true);
            }
            else
            {       // 시간이 정지 해 있다면
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
