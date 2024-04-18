using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelUpUI : MonoBehaviour
{
    // 나타낼 이미지
    public Image levelUpImage;

    // 초기 이미지 크기와 최종 이미지 크기
    public float initialScale = 0.5f; // 이미지가 나타날 때 초기 크기
    public float finalScale = 1.0f;   // 이미지가 나타날 때 최종 크기

    // 페이드 인 및 페이드 아웃 지속 시간
    public float fadeDuration = 1f; // 페이드 인 또는 페이드 아웃 지속 시간
    public float displayDuration = 2f;   // 이미지가 화면에 표시되는 시간

    void Start()
    {
        // 이미지 초기화
        levelUpImage.gameObject.SetActive(false);
        levelUpImage.transform.localScale = Vector3.one * initialScale;

        // 일정 시간 후에 UI 요소를 나타내기 위해 함수 호출
        StartCoroutine(ShowWithDelay());
    }

    IEnumerator ShowWithDelay()
    {
        // 일정 시간 대기 후에 이미지 활성화
        yield return new WaitForSeconds(displayDuration);

        // 이미지 활성화
        levelUpImage.gameObject.SetActive(true);

        // 이미지가 등장할 때의 효과
        StartCoroutine(ImageAppearance());

        // 페이드 아웃 애니메이션
        StartCoroutine(FadeOut());
    }

    IEnumerator ImageAppearance()
    {
        // 이미지 크기를 점차 키웁니다.
        float scale = initialScale;
        while (scale < finalScale)
        {
            scale += Time.deltaTime * 2f; // 이미지 크기를 늘리는 속도를 조절할 수 있습니다.
            levelUpImage.transform.localScale = Vector3.one * scale;
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        // 애니메이션 시간 초기화
        float timer = 0f;

        // 페이드 아웃 애니메이션
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            Color tempColor = levelUpImage.color;
            tempColor.a = alpha;
            levelUpImage.color = tempColor;
            yield return null;
        }

        // 애니메이션 완료 후 UI 요소 비활성화
        gameObject.SetActive(false);
    }
}
