using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Supporter;
using Unity.VisualScripting;

public class PlayerManager
{
    public PlayerStats player;
    public HashSet<BulletController> bullets = new HashSet<BulletController>();
    public void CreatePlayer(int index, string code)
    {
        PlayerStats _player = new PlayerStats();

        Entity_Player.Param _playerData = Managers.Data.GetDataFromDictionary(Managers.Data.playerDictionary, index, code);
        if(string.IsNullOrEmpty(code))
        {
             code = _playerData.code;
        }
         GameObject playerObject = Managers.Data.Instantiate(code,null,true);

        _player.playerController = playerObject.GetComponent<PlayerController>();
        _player.hp = _playerData.baseHp;
        _player.attackSpeed = 5;
        _player.moveSpeed = _playerData.baseMoveSpeed;
        _player.rotationSpeed = 10;
        _player.bulletSpeed = 1;
        _player.attackRange = _playerData.baseRange;

        player = _player;
        player.playerController.LoadData();
        CameraController cc = Util.GetOrAddComponent<CameraController>(_player.playerController.gameObject);
        cc.player = _player.playerController;
        cc.FindCamera();
    }
    public void SetStats(Operation operation ,StatType statType, float amount)
    {
        if(operation == Operation.Plus)
        {
            if (statType == StatType.CurrentHP) player.currentHp += amount;
            else if (statType == StatType.MAXHP) player.hp += amount;
            else if (statType == StatType.CurrentSP) player.currentSp += amount;
            else if (statType == StatType.MAXSP) player.sp += amount;
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

    public virtual void Attack()
    {
        ShotAsDirection(BulletDirection.forward);
        Debug.Log("빵야");
    }
    private void ShotAsDirection(BulletDirection direction)
    {
        GameObject temp = Managers.Data.Instantiate("Bullet",null,true);
        BulletController bc = Util.GetOrAddComponent<BulletController>(temp);
        temp.transform.position = player.playerController.transform.position + new Vector3(0,1.3f,0);
        bc.bulletType = direction;
        bc.moveSpeed = player.bulletSpeed;

        if (direction == BulletDirection.forward) bc.direction = player.playerController.transform.forward;
        else if (direction == BulletDirection.left) bc.direction = -player.playerController.transform.right;
        else if (direction == BulletDirection.right) bc.direction = player.playerController.transform.right;
        else if (direction == BulletDirection.back) bc.direction = -player.playerController.transform.forward;
        else Debug.LogWarning("방향오류");

        bullets.Add(bc);
    }
}
