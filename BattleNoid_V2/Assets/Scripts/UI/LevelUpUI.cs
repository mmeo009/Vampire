using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelUpUI : MonoBehaviour
{
    public Image levelUpImage;
    public float displayDuration = 2f;

    void Start()
    {
        // ���İ��� 1�� �����Ͽ� UI ��Ҹ� ��Ÿ���� �մϴ�.
        Color tempColor = levelUpImage.color;
        tempColor.a = 1f;
        levelUpImage.color = tempColor;

        // ���� �ð� �Ŀ� UI ��Ҹ� ��Ȱ��ȭ�մϴ�.
        StartCoroutine(HideAfterDelay());
    }

    IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);

        // ���� �ð��� ���� �Ŀ� UI ��Ҹ� ��Ȱ��ȭ�մϴ�.
        gameObject.SetActive(false);
    }
}
