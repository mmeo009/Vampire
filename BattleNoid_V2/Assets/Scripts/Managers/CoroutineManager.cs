using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class CoroutineManager : MonoBehaviour
{
    private static MonoBehaviour monoInstance;

    [RuntimeInitializeOnLoadMethod]
    private static void Initializer()
    {
        monoInstance = new GameObject($"[{nameof(CoroutineManager)}]").AddComponent<CoroutineManager>();
        DontDestroyOnLoad(monoInstance.gameObject);
    }

    public new static Coroutine StartCoroutine(IEnumerator coroutine)
    {
        return monoInstance.StartCoroutine(coroutine);
    }

    public new static void StopCoroutine(Coroutine coroutine)
    {
        monoInstance.StopCoroutine(coroutine);
    }
    public static void LoadSceneWithLoadingBar(string sceneName, string playerCode = null)
    {
        StartCoroutine(IE_LoadingScene(sceneName, playerCode));
    }
    private static IEnumerator IE_LoadingScene(string sceneName, string playerCode = null)
    {
        Image loadingBar;

        if (SceneManager.GetActiveScene().name != "LoadingScene")
        {
            AsyncOperation loadLoadingScene = SceneManager.LoadSceneAsync("LoadingScene");
            while (!loadLoadingScene.isDone)
            {
                yield return null;
            }
        }

        loadingBar = GameObject.Find("LoadingBar").GetComponent<Image>();

        if (loadingBar == null)
        {
            Debug.LogError("로딩바를 찾을 수 없습니다.");
            yield break;
        }
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;
        float timer = 0f;

        while (!op.isDone)
        {
            yield return null;

            if (op.progress <= 0.0f)
            {
                loadingBar.fillAmount = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                loadingBar.fillAmount = Mathf.Lerp(0.0f, 1f, timer);

                if (loadingBar.fillAmount >= 1f || op.isDone)
                {
                    op.allowSceneActivation = true;

                    yield return new WaitUntil(() => op.isDone);

                    if (playerCode != null)
                    {
                        Managers.Player.CreatePlayer(0, playerCode);
                        yield return null;
                    }

                    yield return StartCoroutine(IE_SetWaveData(sceneName));
                }
            }
        }
    }

    private static IEnumerator IE_SetWaveData(string sceneName)
    {

        if (SceneManager.GetActiveScene().name != sceneName)
        {
            AsyncOperation loadLoadingScene = SceneManager.LoadSceneAsync(sceneName);
            while (!loadLoadingScene.isDone)
            {
                yield return new WaitUntil(() => loadLoadingScene.isDone);
            }
        }

        if (sceneName == "GameScene_001")
        {
            var temp = new GameObject();
            temp.name = "@WaveController";
            var waveController = temp.AddComponent<WaveController>();

        }
        else
        {
            yield break;
        }
    }
}
