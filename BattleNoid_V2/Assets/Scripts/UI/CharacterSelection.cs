using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterSelection : MonoBehaviour
{
    public ImageSequence character1ImageSequence; // ù ��° ĳ������ �̹��� ������
    public ImageSequence character2ImageSequence; // �� ��° ĳ������ �̹��� ������
    public Button character1Button; // ù ��° ĳ���� ��ư
    public Button character2Button; // �� ��° ĳ���� ��ư
    public ButtonController startButton;

    private ImageSequence currentCharacterSequence; // ���� ���õ� ĳ������ �̹��� ������

    private void Start()
    {
        // ó������ ù ��° ĳ������ �̹��� �������� ǥ��
        ShowCharacterImages(character1ImageSequence);
    }

    private void ShowCharacterImages(ImageSequence sequence)
    {
        // ��� �ڷ�ƾ ����
        StopAllCoroutines();

        // ��� �̹��� ������ ��Ȱ��ȭ
        HideImages(character1ImageSequence);
        HideImages(character2ImageSequence);

        // ���õ� ĳ������ �̹��� ������ Ȱ��ȭ
        currentCharacterSequence = sequence;
        ShowImages(currentCharacterSequence);
        StartCoroutine(DisplayImages(currentCharacterSequence));
    }

    // ù ��° ĳ���� ��ư�� Ŭ���� �� ȣ��Ǵ� �Լ�
    public void OnCharacter1ButtonClicked()
    {
        ShowCharacterImages(character1ImageSequence);
        character2Button.interactable = true; // �� ��° ĳ���� ��ư Ȱ��ȭ
        character1Button.interactable = false; // ù ��° ĳ���� ��ư ��Ȱ��ȭ
        startButton.playerCode = "111111P";
        startButton.AddButtonTesk();
    }

    // �� ��° ĳ���� ��ư�� Ŭ���� �� ȣ��Ǵ� �Լ�
    public void OnCharacter2ButtonClicked()
    {
        ShowCharacterImages(character2ImageSequence);
        character1Button.interactable = true; // ù ��° ĳ���� ��ư Ȱ��ȭ
        character2Button.interactable = false; // �� ��° ĳ���� ��ư ��Ȱ��ȭ
        startButton.playerCode = "111112P";
        startButton.AddButtonTesk();
    }



    // �̹��� ���� ó�� �Լ�
    private void HideImages(ImageSequence sequence)
    {
        foreach (var image in sequence.images)
        {
            image.enabled = false;
        }
    }

    // �̹��� Ȱ��ȭ ó�� �Լ�
    private void ShowImages(ImageSequence sequence)
    {
        foreach (var image in sequence.images)
        {
            image.enabled = true;
            image.fillAmount = 0f; // �̹����� fillAmount �ʱ�ȭ
        }
    }

    // �̹��� �������� ǥ���ϴ� �ڷ�ƾ �Լ�
    IEnumerator DisplayImages(ImageSequence sequence)
    {
        foreach (Image image in sequence.images)
        {
            // �̹����� ������ ä������ �ִϸ��̼�
            while (image.fillAmount < 1f)
            {
                image.fillAmount += Time.deltaTime * sequence.fillSpeed;
                yield return null;
            }

            // ���� �ð� ���� �̹��� ����
            yield return new WaitForSeconds(sequence.displayDuration);
        }
    }
}
