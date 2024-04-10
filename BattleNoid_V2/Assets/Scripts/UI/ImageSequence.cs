using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageSequence : MonoBehaviour
{
    public Image[] images; // 이미지 배열
    public float fillSpeed = 1f; // 채워지는 속도
    public float displayDuration = 2f; // 이미지가 표시될 시간

    private void Start()
    {
        StartCoroutine(DisplayImages());
    }

    IEnumerator DisplayImages()
    {
        foreach (Image image in images)
        {
            image.fillAmount = 0f; // 이미지의 fillAmount 초기화
            image.enabled = true; // 이미지 활성화

            // 이미지를 서서히 채워지는 애니메이션
            while (image.fillAmount < 1f)
            {
                image.fillAmount += Time.deltaTime * fillSpeed;
                yield return null;
            }

            // 일정 시간 동안 이미지 유지
            yield return new WaitForSeconds(displayDuration);



            //image.enabled = false; // 이미지 비활성화
        }
    }
}
