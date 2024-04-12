using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWave", menuName = "ScriptableObject/wave")]
public class WaveData : ScriptableObject
{
    // �������� ����
    [Header("�������� ����")] public StageData stageData = new StageData();
    // ���� ���̺� ���� ��ٸ��� �ð�
    [Header("���� ���̺� ���� ��ٸ��� �ð�")] public int timeToNextWave;
    // ���� �������� ����
    [Header("������ ��� üũ")] public bool isBossStage = false;
    // �̹� ���̺꿡 �����ϴ� ����
    [Header("������ ���� ����Ʈ")] public List<Monster> monsters = new List<Monster>();
    [System.Serializable]
    public class Monster
    {
        // ���� �ε��� ��ȣ
        [Header("���� �ε��� ��ȣ")] public int monsterIndex;
        // ���� ���� ��
        [Header("�̹� ���̺꿡 ������ ��")] public int monsterAmount;
        // �߰� ü��
        [Header("�⺻���� ��ȭ�� ü��")] public float additionalHp;
        // �߰� ������
        [Header("�⺻���� ��ȭ�� ���ݷ�")] public float additionalDamage;
        // �߰� �̵� �ӵ�
        [Header("�⺻���� ��ȭ�� �̵� �ӵ�")] public float additionalMoveSpeed;
    }
    [System.Serializable]
    public class StageData
    {
        // �������� ��ȣ
        [Header("�������� ��ȣ")] public int stageNumber;
        // ���̺� ����
        [Header("���̺� ����")] public int waveNumber;
    }
}
