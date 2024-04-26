using UnityEngine;

public class OptionMenu : MonoBehaviour
{
    public GameObject optionPanel;

    void Start()
    {
        optionPanel.SetActive(false); // �ɼǸ޴� �ȸԾ ����ϰ� ���°Ŷ� ���߿� �����̶� �̾߱��ϰ� �����
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
