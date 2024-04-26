using UnityEngine;

public class OptionMenu : MonoBehaviour
{
    public GameObject optionPanel;

    void Start()
    {
        optionPanel.SetActive(false); // 옵션메뉴 안먹어서 긴급하게 쓰는거라 나중에 학현이랑 이야기하고 지우기
    }

    public void ToggleOptionPanel()
    {
        optionPanel.SetActive(!optionPanel.activeSelf);
        if (optionPanel.activeSelf)
        {
            Time.timeScale = 0f; 
        }
        else
        {
            Time.timeScale = 1f; 
        }
    }
}
