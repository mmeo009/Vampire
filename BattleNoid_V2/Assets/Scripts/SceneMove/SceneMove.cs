
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneMove : MonoBehaviour
{
    //대체적으로 모든씬 이동로직은 여기다 적어놓을 예정임
    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene_001");
    }
}
