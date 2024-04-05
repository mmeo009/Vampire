using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Supporter;

public class PlayerManager
{
    public PlayerStats player;
    public void CreatePlayer(int index, string code)
    {
        PlayerStats _player = new PlayerStats();

        Entity_Player.Param _playerData = Managers.Data.GetDataFromDictionary(Managers.Data.playerDictionary, index, code);

        _player.hp = _playerData.baseHp;

    }
    public void SetStats(Operation operation ,StatType statType, float amount)
    {
        if(operation == Operation.Plus)
        {
            if (statType == StatType.CurrentHP) player.currentHp += amount;
            else if (statType == StatType.MAXHP) player.hp += amount;
            else if (statType == StatType.CurrentSP) player.currentSp += amount;
            else if (statType == StatType.MAXSP) player.sp += amount;
            else if (statType == StatType.CurrentXP) player.currentXp += amount;
            else if (statType == StatType.MoveSpeed) player.moveSpeed += amount;
            else if (statType == StatType.AttackSpeed) player.attackSpeed += amount;
            else if (statType == StatType.AttackDamage) player.attackDamage += amount;
            else if (statType == StatType.AttackRange) player.attackRange += amount;
            else Debug.LogWarning("플레이어의 무엇을 더하고 싶으시나");
        }
        else if(operation == Operation.Minus)
        {
            if (statType == StatType.CurrentHP) { player.currentHp -= amount; if (player.currentHp <= 0) player.currentHp = 0; }
            else if (statType == StatType.MAXHP) { player.hp -= amount; if (player.hp <= 0) player.hp = 0; }
            else if (statType == StatType.CurrentSP) { player.currentSp -= amount; if (player.currentSp <= 0) player.currentSp = 0; }
            else if (statType == StatType.MAXSP) { player.sp -= amount; if (player.sp <= 0) player.sp = 0; }
            else if (statType == StatType.CurrentXP) { player.currentXp -= amount; if (player.currentXp <= 0) player.currentXp = 0; }
            else if (statType == StatType.MoveSpeed) { player.moveSpeed -= amount; if (player.moveSpeed <= 0) player.moveSpeed = 0; }
            else if (statType == StatType.AttackSpeed) { player.attackSpeed -= amount; if (player.attackSpeed <= 0) player.attackSpeed = 0; }
            else if (statType == StatType.AttackDamage) { player.attackDamage -= amount; if (player.attackDamage <= 0) player.attackDamage = 0; }
            else if (statType == StatType.AttackRange) { player.attackRange -= amount; if (player.attackRange <= 0) player.attackRange = 0; }
            else Debug.LogWarning("플레이어의 무엇을 빼고 싶으시나");
        }
        else if(operation == Operation.Set)
        {
            if (statType == StatType.CurrentHP) player.currentHp = amount;
            else if (statType == StatType.MAXHP) player.hp = amount;
            else if (statType == StatType.CurrentSP) player.currentSp = amount;
            else if (statType == StatType.MAXSP) player.sp = amount;
            else if (statType == StatType.CurrentXP) player.currentXp = amount;
            else if (statType == StatType.MoveSpeed) player.moveSpeed = amount;
            else if (statType == StatType.AttackSpeed) player.attackSpeed = amount;
            else if (statType == StatType.AttackDamage) player.attackDamage = amount;
            else if (statType == StatType.AttackRange) player.attackRange = amount;
            else Debug.LogWarning("플레이어의 무엇을 바꾸고 싶으시나");
        }
        else
        {
            Debug.LogWarning("미안해");
        }
    }

    public void Attack()
    {

    }
}
