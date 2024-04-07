using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public GameObject VolumeWindow;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickstopbutton();
        }
    }

    public void OnClickstopbutton()
    {
        Time.timeScale = 0;
        VolumeWindow.SetActive(true);
    }

    public void OnClickContiButtom()
    {
        Time.timeScale = 1;
        VolumeWindow.SetActive(false);
    }
}
