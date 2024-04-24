using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InfiniteScroll : MonoBehaviour
{
    
    public Transform content; // Content transform
    public Image[] images; // 이미지 배열
    public int itemCount = 50; // 초기 리스트 아이템 개수
    public float itemHeight = 100f; // 리스트 아이템의 높이

    private List<GameObject> itemList = new List<GameObject>(); // 생성된 리스트 아이템들

    void Start()
    {
        // 초기 리스트 아이템 생성
        for (int i = 0; i < itemCount; i++)
        {
            // 이미지 배열에서 랜덤으로 이미지 선택
            Image randomImage = images[Random.Range(0, images.Length)];

          
        }
    }

    void Update()
    {
        // Scroll View의 위치를 기준으로 리스트 아이템의 배치를 업데이트
        float contentPosY = content.localPosition.y;
        for (int i = 0; i < itemList.Count; i++)
        {
            // 아이템이 화면을 벗어난 경우, 맨 위에 재배치
            if (contentPosY + itemList[i].GetComponent<RectTransform>().anchoredPosition.y < -itemHeight)
            {
                itemList[i].GetComponent<RectTransform>().anchoredPosition += new Vector2(0, itemList.Count * itemHeight);
            }
            // 아이템이 화면 밑에 벗어난 경우, 맨 아래로 재배치
            else if (contentPosY + itemList[i].GetComponent<RectTransform>().anchoredPosition.y > 0)
            {
                itemList[i].GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, itemList.Count * itemHeight);
            }
        }
    }
}
