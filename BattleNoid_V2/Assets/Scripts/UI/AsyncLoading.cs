using System.Collections; // IEnumerator �� ��Ÿ �ڷ�ƾ ���� ����� ����ϱ� ���� ���ӽ����̽�
using UnityEngine; // Unity ���� ���� ����� ����ϱ� ���� ���ӽ����̽�
using UnityEngine.UI; // UI ���(�̹��� ��)�� ����ϱ� ���� ���ӽ����̽�
using UnityEngine.SceneManagement; // ���� �����ϱ� ���� ���ӽ����̽�

public class AsyncLoading : MonoBehaviour
{
    public Image loadingBar; // �ε� �� �̹����� ����Ű�� ����
    public float loadingTime = 3f; // �ε� �ð� (��)

    void Start()
    {
        StartCoroutine(FillLoadingBar()); // �ε� �ٸ� ä��� �ڷ�ƾ�� �����մϴ�.
    }

    IEnumerator FillLoadingBar()
    {
        float timer = 0f; // Ÿ�̸� ������ �ʱ�ȭ�մϴ�.

        while (timer < loadingTime) // �ε� �ð��� ���� ������ �ݺ��մϴ�.
        {
            timer += Time.deltaTime; // ��� �ð��� ���մϴ�.

            // �ε� �ٸ� ������ ä���� �� �ֵ��� ����մϴ�.
            float progress = Mathf.Clamp01(timer / loadingTime); // �ε� ���� ä���� ������ 0�� 1 ���̷� �����մϴ�.
            loadingBar.fillAmount = progress; // �ε� ���� ä���� ������ �����մϴ�.

            yield return null; // ���� �����ӱ��� ����մϴ�.
        }

        // �ε��� �Ϸ�Ǹ� �ش� ������ �Ѿ�� ������ �߰��� �� �ֽ��ϴ�.
        // SceneManager.LoadScene("YourNextSceneNameHere"); // ���� ���� �ε��մϴ�.
    }
}
