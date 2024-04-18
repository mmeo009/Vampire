using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelUpUI : MonoBehaviour
{
    public Image levelUpImage;
    public float fadeDuration = 1f;
    public float displayDuration = 2f;

    void Start()
    {
        // UI 요소를 투명하게 만듭니다.
        levelUpImage.canvasRenderer.SetAlpha(0f);

        // 페이드 인 애니메이션 시작
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        // 애니메이션 시간 초기화
        float timer = 0f;

        // 페이드 인 애니메이션
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            levelUpImage.CrossFadeAlpha(1f, fadeDuration, false);
            yield return null;
        }

        // 일정 시간 대기 후 페이드 아웃 애니메이션 시작
        yield return new WaitForSeconds(displayDuration);
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        // 애니메이션 시간 초기화
        float timer = 0f;

        // 페이드 아웃 애니메이션
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            levelUpImage.CrossFadeAlpha(0f, fadeDuration, false);
            yield return null;
        }

        // 애니메이션 완료 후 UI 요소 비활성화
        gameObject.SetActive(false);
    }
}
