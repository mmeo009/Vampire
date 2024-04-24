using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InfiniteScroll : MonoBehaviour
{
    
    public Transform content; // Content transform
    public Image[] images; // �̹��� �迭
    public int itemCount = 50; // �ʱ� ����Ʈ ������ ����
    public float itemHeight = 100f; // ����Ʈ �������� ����

    private List<GameObject> itemList = new List<GameObject>(); // ������ ����Ʈ �����۵�

    void Start()
    {
        // �ʱ� ����Ʈ ������ ����
        for (int i = 0; i < itemCount; i++)
        {
            // �̹��� �迭���� �������� �̹��� ����
            Image randomImage = images[Random.Range(0, images.Length)];

          
        }
    }

    void Update()
    {
        // Scroll View�� ��ġ�� �������� ����Ʈ �������� ��ġ�� ������Ʈ
        float contentPosY = content.localPosition.y;
        for (int i = 0; i < itemList.Count; i++)
        {
            // �������� ȭ���� ��� ���, �� ���� ���ġ
            if (contentPosY + itemList[i].GetComponent<RectTransform>().anchoredPosition.y < -itemHeight)
            {
                itemList[i].GetComponent<RectTransform>().anchoredPosition += new Vector2(0, itemList.Count * itemHeight);
            }
            // �������� ȭ�� �ؿ� ��� ���, �� �Ʒ��� ���ġ
            else if (contentPosY + itemList[i].GetComponent<RectTransform>().anchoredPosition.y > 0)
            {
                itemList[i].GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, itemList.Count * itemHeight);
            }
        }
    }
}
