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
        // UI ��Ҹ� �����ϰ� ����ϴ�.
        levelUpImage.canvasRenderer.SetAlpha(0f);

        // ���̵� �� �ִϸ��̼� ����
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        // �ִϸ��̼� �ð� �ʱ�ȭ
        float timer = 0f;

        // ���̵� �� �ִϸ��̼�
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            levelUpImage.CrossFadeAlpha(1f, fadeDuration, false);
            yield return null;
        }

        // ���� �ð� ��� �� ���̵� �ƿ� �ִϸ��̼� ����
        yield return new WaitForSeconds(displayDuration);
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        // �ִϸ��̼� �ð� �ʱ�ȭ
        float timer = 0f;

        // ���̵� �ƿ� �ִϸ��̼�
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            levelUpImage.CrossFadeAlpha(0f, fadeDuration, false);
            yield return null;
        }

        // �ִϸ��̼� �Ϸ� �� UI ��� ��Ȱ��ȭ
        gameObject.SetActive(false);
    }
}
