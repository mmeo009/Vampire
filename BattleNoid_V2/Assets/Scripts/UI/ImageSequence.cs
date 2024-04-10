using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageSequence : MonoBehaviour
{
    public Image[] images; // �̹��� �迭
    public float fillSpeed = 1f; // ä������ �ӵ�
    public float displayDuration = 2f; // �̹����� ǥ�õ� �ð�

    private void Start()
    {
        StartCoroutine(DisplayImages());
    }

    IEnumerator DisplayImages()
    {
        foreach (Image image in images)
        {
            image.fillAmount = 0f; // �̹����� fillAmount �ʱ�ȭ
            image.enabled = true; // �̹��� Ȱ��ȭ

            // �̹����� ������ ä������ �ִϸ��̼�
            while (image.fillAmount < 1f)
            {
                image.fillAmount += Time.deltaTime * fillSpeed;
                yield return null;
            }

            // ���� �ð� ���� �̹��� ����
            yield return new WaitForSeconds(displayDuration);



            //image.enabled = false; // �̹��� ��Ȱ��ȭ
        }
    }
}
