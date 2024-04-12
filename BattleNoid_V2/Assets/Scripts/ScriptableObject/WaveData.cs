using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWave", menuName = "ScriptableObject/wave")]
public class WaveData : ScriptableObject
{
    // 스테이지 정보
    [Header("스테이지 정보")] public StageData stageData = new StageData();
    // 다음 웨이브 까지 기다리는 시간
    [Header("다음 웨이브 까지 기다리는 시간")] public int timeToNextWave;
    // 보스 스테이지 인지
    [Header("보스일 경우 체크")] public bool isBossStage = false;
    // 이번 웨이브에 생성하는 몬스터
    [Header("생성할 몬스터 리스트")] public List<Monster> monsters = new List<Monster>();
    [System.Serializable]
    public class Monster
    {
        // 몬스터 인덱스 번호
        [Header("몬스터 인덱스 번호")] public int monsterIndex;
        // 몬스터 생성 수
        [Header("이번 웨이브에 생성할 양")] public int monsterAmount;
        // 추가 체력
        [Header("기본보다 강화될 체력")] public float additionalHp;
        // 추가 데미지
        [Header("기본보다 강화될 공격력")] public float additionalDamage;
        // 추가 이동 속도
        [Header("기본보다 강화될 이동 속도")] public float additionalMoveSpeed;
    }
    [System.Serializable]
    public class StageData
    {
        // 스테이지 번호
        [Header("스테이지 번호")] public int stageNumber;
        // 웨이브 숫자
        [Header("웨이브 숫자")] public int waveNumber;
    }
}
