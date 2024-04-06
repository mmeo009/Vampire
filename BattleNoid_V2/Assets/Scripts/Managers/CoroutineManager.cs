using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    // ��ư �Ŵ������� ȣ���� �ε� �ڷ�ƾ
    public static void LoadSceneWithLoadingBar(string sceneName)
    {
        StartCoroutine(LoadingSceneAndFillLoadingBarCoroutine(sceneName));
    }

    // ���� �ε��ϰ� �ε� �ٸ� ä��� Coroutine
    private static IEnumerator LoadingSceneAndFillLoadingBarCoroutine(string sceneName)
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
            Debug.LogError("�ε��ٸ� ã�� �� �����ϴ�.");
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
                    yield break;
                }
            }
        }
    }
}
