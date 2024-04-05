using System.Collections; // IEnumerator 및 기타 코루틴 관련 기능을 사용하기 위한 네임스페이스
using UnityEngine; // Unity 엔진 관련 기능을 사용하기 위한 네임스페이스
using UnityEngine.UI; // UI 요소(이미지 등)를 사용하기 위한 네임스페이스
using UnityEngine.SceneManagement; // 씬을 관리하기 위한 네임스페이스

public class AsyncLoading : MonoBehaviour
{
    public Image loadingBar; // 로딩 바 이미지를 가리키는 변수
    public float loadingTime = 3f; // 로딩 시간 (초)

    void Start()
    {
        StartCoroutine(FillLoadingBar()); // 로딩 바를 채우는 코루틴을 시작합니다.
    }

    IEnumerator FillLoadingBar()
    {
        float timer = 0f; // 타이머 변수를 초기화합니다.

        while (timer < loadingTime) // 로딩 시간이 지날 때까지 반복합니다.
        {
            timer += Time.deltaTime; // 경과 시간을 더합니다.

            // 로딩 바를 서서히 채워질 수 있도록 계산합니다.
            float progress = Mathf.Clamp01(timer / loadingTime); // 로딩 바의 채워진 정도를 0과 1 사이로 제한합니다.
            loadingBar.fillAmount = progress; // 로딩 바의 채워진 정도를 갱신합니다.

            yield return null; // 다음 프레임까지 대기합니다.
        }

        // 로딩이 완료되면 해당 씬으로 넘어가는 로직을 추가할 수 있습니다.
        // SceneManager.LoadScene("YourNextSceneNameHere"); // 다음 씬을 로드합니다.
    }
}
