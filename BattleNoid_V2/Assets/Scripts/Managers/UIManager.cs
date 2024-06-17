using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Supporter;
using System.Linq;
using TMPro;

[System.Serializable]
public class UIManager
{
    // UI 윈도우
    public GameObject OptionWindowPrefab;
    public GameObject CardWindowPrefab;
    public GameObject CardPrefab;

    public GameObject optionWindow;
    public GameObject cardWindow;

    public void TestLoadEnum(DataType type)
    {
        Debug.Log(type.ToString());
    }
    public void ButtonAction(ActionType type, string taskString = null, string playerCode = null)
    {
        if (type == ActionType.SceneMove)
        {
            if (taskString != null)
            {
                // 씬 로딩 로직은 여기에 추가
                SceneManager.LoadScene("LoadingScene");
                optionWindow = null;
                if(playerCode != null)
                {
                    CoroutineManager.LoadSceneWithLoadingBar(taskString);
                }
                else
                {
                    CoroutineManager.LoadSceneWithLoadingBar(taskString);
                }
            }
            else
            {
                Debug.LogError("씬 이름이 기입되지 않았습니다.");
                taskString = "StartScene";
                SceneManager.LoadScene(taskString);
            }
        }
        else if (type == ActionType.ExitGame)
        {
            // 에디터에서는 동작 안함
            Debug.Log("Exit 스크립트는 에디터에서 작동안해요 아마도여");

            // 빌드된 게임에서는 동작함 봉붕어에서 가져옴
            Application.Quit();
        }
        else if (type == ActionType.PauseGame && taskString != null)
        {

            // 시간이 정지하지 않았다면
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                if (taskString == "OptionsWindow")
                {
                    Debug.Log("버튼누름");
                    if (optionWindow == null)
                    {
                        
                        optionWindow = LoadWindow(OptionWindowPrefab);
                    }
                    optionWindow.SetActive(true);
                }
                else if (taskString == "CardWindow")
                {
                    if (cardWindow == null)
                    {
                        cardWindow = LoadWindow(CardWindowPrefab);
                    }
                    cardWindow.SetActive(true);
                }
            }
            else
            {       // 시간이 정지 해 있다면
                if (taskString == "OptionsWindow")
                {
                    Debug.Log("버튼누름");
                    optionWindow.SetActive(false);
                    if (cardWindow!= null)
                    {
                        if(cardWindow.activeInHierarchy == false)
                        {
                            Debug.Log("버튼누름2");
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

    public void InitSkillUI(Image image, string skillName)
    {
        var _sprite = Managers.Data.Load<Sprite>(skillName);

        if (_sprite != null)
        {
            image.sprite = _sprite;
        }
        else
        { 
            Debug.LogError($"{skillName}라는 스킬의 이미지를 찾을 수 없어");
        }

        image.type = Image.Type.Filled;
        image.fillMethod = Image.FillMethod.Radial360;
        image.fillOrigin = (int)Image.Origin360.Top;
        image.fillClockwise = false;
    }

    public class ButtonUI
    {
        public TMP_Text text;
        public Button button;
        public Image image;
        public ButtonUI(GameObject buttonObject) 
        {
            text = buttonObject.GetComponent<TMP_Text>();
            button = buttonObject.GetComponent<Button>();
            image = buttonObject.GetComponent<Image>();
        }
    }
}