using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterSelection : MonoBehaviour
{
    public ImageSequence character1ImageSequence; // 첫 번째 캐릭터의 이미지 시퀀스
    public ImageSequence character2ImageSequence; // 두 번째 캐릭터의 이미지 시퀀스
    public Button character1Button; // 첫 번째 캐릭터 버튼
    public Button character2Button; // 두 번째 캐릭터 버튼
    public ButtonController startButton;

    private ImageSequence currentCharacterSequence; // 현재 선택된 캐릭터의 이미지 시퀀스

    private void Start()
    {
        // 처음에는 첫 번째 캐릭터의 이미지 시퀀스를 표시
        ShowCharacterImages(character1ImageSequence);
    }

    private void ShowCharacterImages(ImageSequence sequence)
    {
        // 모든 코루틴 중지
        StopAllCoroutines();

        // 모든 이미지 시퀀스 비활성화
        HideImages(character1ImageSequence);
        HideImages(character2ImageSequence);

        // 선택된 캐릭터의 이미지 시퀀스 활성화
        currentCharacterSequence = sequence;
        ShowImages(currentCharacterSequence);
        StartCoroutine(DisplayImages(currentCharacterSequence));
    }

    // 첫 번째 캐릭터 버튼을 클릭할 때 호출되는 함수
    public void OnCharacter1ButtonClicked()
    {
        ShowCharacterImages(character1ImageSequence);
        character2Button.interactable = true; // 두 번째 캐릭터 버튼 활성화
        character1Button.interactable = false; // 첫 번째 캐릭터 버튼 비활성화
        startButton.playerCode = "111111P";
        startButton.AddButtonTesk();
    }

    // 두 번째 캐릭터 버튼을 클릭할 때 호출되는 함수
    public void OnCharacter2ButtonClicked()
    {
        ShowCharacterImages(character2ImageSequence);
        character1Button.interactable = true; // 첫 번째 캐릭터 버튼 활성화
        character2Button.interactable = false; // 두 번째 캐릭터 버튼 비활성화
        startButton.playerCode = "111112P";
        startButton.AddButtonTesk();
    }



    // 이미지 숨김 처리 함수
    private void HideImages(ImageSequence sequence)
    {
        foreach (var image in sequence.images)
        {
            image.enabled = false;
        }
    }

    // 이미지 활성화 처리 함수
    private void ShowImages(ImageSequence sequence)
    {
        foreach (var image in sequence.images)
        {
            image.enabled = true;
            image.fillAmount = 0f; // 이미지의 fillAmount 초기화
        }
    }

    // 이미지 시퀀스를 표시하는 코루틴 함수
    IEnumerator DisplayImages(ImageSequence sequence)
    {
        foreach (Image image in sequence.images)
        {
            // 이미지를 서서히 채워지는 애니메이션
            while (image.fillAmount < 1f)
            {
                image.fillAmount += Time.deltaTime * sequence.fillSpeed;
                yield return null;
            }

            // 일정 시간 동안 이미지 유지
            yield return new WaitForSeconds(sequence.displayDuration);
        }
    }
}
