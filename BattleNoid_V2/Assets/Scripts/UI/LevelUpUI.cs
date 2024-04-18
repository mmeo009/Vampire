using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelUpUI : MonoBehaviour
{
    // ��Ÿ�� �̹���
    public Image levelUpImage;

    // �ʱ� �̹��� ũ��� ���� �̹��� ũ��
    public float initialScale = 0.5f; // �̹����� ��Ÿ�� �� �ʱ� ũ��
    public float finalScale = 1.0f;   // �̹����� ��Ÿ�� �� ���� ũ��

    // ���̵� �� �� ���̵� �ƿ� ���� �ð�
    public float fadeDuration = 1f; // ���̵� �� �Ǵ� ���̵� �ƿ� ���� �ð�
    public float displayDuration = 2f;   // �̹����� ȭ�鿡 ǥ�õǴ� �ð�

    void Start()
    {
        // �̹��� �ʱ�ȭ
        levelUpImage.gameObject.SetActive(false);
        levelUpImage.transform.localScale = Vector3.one * initialScale;

        // ���� �ð� �Ŀ� UI ��Ҹ� ��Ÿ���� ���� �Լ� ȣ��
        StartCoroutine(ShowWithDelay());
    }

    IEnumerator ShowWithDelay()
    {
        // ���� �ð� ��� �Ŀ� �̹��� Ȱ��ȭ
        yield return new WaitForSeconds(displayDuration);

        // �̹��� Ȱ��ȭ
        levelUpImage.gameObject.SetActive(true);

        // �̹����� ������ ���� ȿ��
        StartCoroutine(ImageAppearance());

        // ���̵� �ƿ� �ִϸ��̼�
        StartCoroutine(FadeOut());
    }

    IEnumerator ImageAppearance()
    {
        // �̹��� ũ�⸦ ���� Ű��ϴ�.
        float scale = initialScale;
        while (scale < finalScale)
        {
            scale += Time.deltaTime * 2f; // �̹��� ũ�⸦ �ø��� �ӵ��� ������ �� �ֽ��ϴ�.
            levelUpImage.transform.localScale = Vector3.one * scale;
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        // �ִϸ��̼� �ð� �ʱ�ȭ
        float timer = 0f;

        // ���̵� �ƿ� �ִϸ��̼�
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            Color tempColor = levelUpImage.color;
            tempColor.a = alpha;
            levelUpImage.color = tempColor;
            yield return null;
        }

        // �ִϸ��̼� �Ϸ� �� UI ��� ��Ȱ��ȭ
        gameObject.SetActive(false);
    }
}
