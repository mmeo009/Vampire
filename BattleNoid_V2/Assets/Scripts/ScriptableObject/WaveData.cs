using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWave", menuName = "ScriptableObject/wave")]
public class WaveData : ScriptableObject
{
    public StageData stageData = new StageData();           // �������� ����
    public int timeToNextWave;                              // ���� ���̺� ���� ��ٸ��� �ð�
    public bool isBossStage = false;                        // ���� �������� ����
    public List<Monster> monsters = new List<Monster>();    // �̹� ���̺꿡 �����ϴ� ����
    [System.Serializable]
    public class Monster
    {
        public int monsterIndex;                            // ���� �ε��� ��ȣ
        public int monsterAmount;                           // ���� ���� ��
        public float additionalHp;                          // �߰� ü��
        public float additionalDamage;                      // �߰� ������
        public float additionalMoveSpeed;                   // �߰� �̵� �ӵ�
    }
    [System.Serializable]
    public class StageData
    {
        public int stageNumber;                             // �������� ��ȣ
        public int waveNumber;                              // ���̺� ����
    }
}
