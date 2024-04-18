using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelUpUI : MonoBehaviour
{
    public Image levelUpImage;
    public float displayDuration = 2f;

    void Start()
    {
        // 알파값을 1로 설정하여 UI 요소를 나타나게 합니다.
        Color tempColor = levelUpImage.color;
        tempColor.a = 1f;
        levelUpImage.color = tempColor;

        // 일정 시간 후에 UI 요소를 비활성화합니다.
        StartCoroutine(HideAfterDelay());
    }

    IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);

        // 일정 시간이 지난 후에 UI 요소를 비활성화합니다.
        gameObject.SetActive(false);
    }
}
