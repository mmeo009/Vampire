using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterSelection : MonoBehaviour
{
    public ImageSequence[] imageSequences; // �� ĳ������ �̹��� ������ �迭
    private int currentCharacterIndex = 0; // ���� ���õ� ĳ���� �ε���

    private void Start()
    {
        // ó������ ù ��° ĳ������ �̹��� �������� ǥ��
        ShowCharacterImages(currentCharacterIndex);
    }

    // ��ư�� Ŭ���� �� ȣ��Ǵ� �Լ���
    public void OnCharacter1ButtonClicked()
    {
        ShowCharacterImages(0); // ù ��° ĳ������ �̹��� �������� ǥ��
    }

    public void OnCharacter2ButtonClicked()
    {
        ShowCharacterImages(1); // �� ��° ĳ������ �̹��� �������� ǥ��
    }

    // �ش� ĳ������ �̹��� �������� ǥ���ϴ� �Լ�
    private void ShowCharacterImages(int characterIndex)
    {
        // ���� ĳ������ �̹��� �������� �����ϰ� ��Ȱ��ȭ
        if (currentCharacterIndex < imageSequences.Length)
        {
            imageSequences[currentCharacterIndex].StopCoroutine(DisplayImages());
            imageSequences[currentCharacterIndex].gameObject.SetActive(false);
        }

        // ���ο� ĳ������ �̹��� �������� Ȱ��ȭ�ϰ� ����
        currentCharacterIndex = characterIndex;
        imageSequences[currentCharacterIndex].gameObject.SetActive(true); 
        imageSequences[currentCharacterIndex].StartCoroutine(DisplayImages());
    }

    // �̹��� �������� ǥ���ϴ� �ڷ�ƾ �Լ�
    IEnumerator DisplayImages()
    {
        Image[] images = imageSequences[currentCharacterIndex].images;

        foreach (Image image in images)
        {
            image.fillAmount = 0f; // �̹����� fillAmount �ʱ�ȭ
            image.enabled = true; // �̹��� Ȱ��ȭ

            // �̹����� ������ ä������ �ִϸ��̼�
            while (image.fillAmount < 1f)
            {
                image.fillAmount += Time.deltaTime * imageSequences[currentCharacterIndex].fillSpeed;
                yield return null;
            }

            // ���� �ð� ���� �̹��� ����
            yield return new WaitForSeconds(imageSequences[currentCharacterIndex].displayDuration);
        }
    }
}
