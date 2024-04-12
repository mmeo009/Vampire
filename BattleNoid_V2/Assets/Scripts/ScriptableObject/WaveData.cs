using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWave", menuName = "ScriptableObject/wave")]
public class WaveData : ScriptableObject
{
    public StageData stageData = new StageData();           // 스테이지 정보
    public int timeToNextWave;                              // 다음 웨이브 까지 기다리는 시간
    public bool isBossStage = false;                        // 보스 스테이지 인지
    public List<Monster> monsters = new List<Monster>();    // 이번 웨이브에 생성하는 몬스터
    [System.Serializable]
    public class Monster
    {
        public int monsterIndex;                            // 몬스터 인덱스 번호
        public int monsterAmount;                           // 몬스터 생성 수
        public float additionalHp;                          // 추가 체력
        public float additionalDamage;                      // 추가 데미지
        public float additionalMoveSpeed;                   // 추가 이동 속도
    }
    [System.Serializable]
    public class StageData
    {
        public int stageNumber;                             // 스테이지 번호
        public int waveNumber;                              // 웨이브 숫자
    }
}
