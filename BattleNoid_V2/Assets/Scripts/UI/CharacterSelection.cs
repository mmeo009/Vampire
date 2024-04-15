using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterSelection : MonoBehaviour
{
    public ImageSequence[] imageSequences; // 각 캐릭터의 이미지 시퀀스 배열
    private int currentCharacterIndex = 0; // 현재 선택된 캐릭터 인덱스

    private void Start()
    {
        // 처음에는 첫 번째 캐릭터의 이미지 시퀀스를 표시
        ShowCharacterImages(currentCharacterIndex);
    }

    // 버튼을 클릭할 때 호출되는 함수들
    public void OnCharacter1ButtonClicked()
    {
        ShowCharacterImages(0); // 첫 번째 캐릭터의 이미지 시퀀스를 표시
    }

    public void OnCharacter2ButtonClicked()
    {
        ShowCharacterImages(1); // 두 번째 캐릭터의 이미지 시퀀스를 표시
    }

    // 해당 캐릭터의 이미지 시퀀스를 표시하는 함수
    private void ShowCharacterImages(int characterIndex)
    {
        // 현재 캐릭터의 이미지 시퀀스를 중지하고 비활성화
        if (currentCharacterIndex < imageSequences.Length)
        {
            imageSequences[currentCharacterIndex].StopCoroutine(DisplayImages());
            imageSequences[currentCharacterIndex].gameObject.SetActive(false);
        }

        // 새로운 캐릭터의 이미지 시퀀스를 활성화하고 시작
        currentCharacterIndex = characterIndex;
        imageSequences[currentCharacterIndex].gameObject.SetActive(true); 
        imageSequences[currentCharacterIndex].StartCoroutine(DisplayImages());
    }

    // 이미지 시퀀스를 표시하는 코루틴 함수
    IEnumerator DisplayImages()
    {
        Image[] images = imageSequences[currentCharacterIndex].images;

        foreach (Image image in images)
        {
            image.fillAmount = 0f; // 이미지의 fillAmount 초기화
            image.enabled = true; // 이미지 활성화

            // 이미지를 서서히 채워지는 애니메이션
            while (image.fillAmount < 1f)
            {
                image.fillAmount += Time.deltaTime * imageSequences[currentCharacterIndex].fillSpeed;
                yield return null;
            }

            // 일정 시간 동안 이미지 유지
            yield return new WaitForSeconds(imageSequences[currentCharacterIndex].displayDuration);
        }
    }
}
