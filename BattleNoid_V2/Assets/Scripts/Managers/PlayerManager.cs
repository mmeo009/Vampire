using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Supporter;
using Unity.VisualScripting;

[System.Serializable]
public class PlayerManager
{
    public PlayerStats player;
    public HashSet<BulletController> bullets = new HashSet<BulletController>();
    public void CreatePlayer(int index, string code)
    {

        Entity_Player.Param _playerData = Managers.Data.GetDataFromDictionary(Managers.Data.playerDictionary, index, code);
        if(string.IsNullOrEmpty(code))
        {
             code = _playerData.code;
        }
         GameObject playerObject = Managers.Data.Instantiate(code,null,true);
        player.playerController = playerObject.GetComponent<PlayerController>();
        SetStats(Operation.Reset, StatType.None, 0, _playerData);
        player.playerController.LoadData();
        CameraController cc = Util.GetOrAddComponent<CameraController>(player.playerController.gameObject);
        cc.player = player.playerController;
        cc.FindCamera();
    }
    public void SetStats(Operation operation, StatType statType, float amount, Entity_Player.Param resetData = null)
    {
        if(operation == Operation.Plus)
        {
            if (statType == StatType.CurrentHP) player.currentHp += amount;
            else if (statType == StatType.MAXHP) player.hp += amount;
            else if (statType == StatType.CurrentSP) player.currentXp += amount;
            else if (statType == StatType.MAXSP) player.xp += amount;
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
            else if (statType == StatType.CurrentSP) { player.currentXp -= amount; if (player.currentXp <= 0) player.currentXp = 0; }
            else if (statType == StatType.MAXSP) { player.xp -= amount; if (player.xp <= 0) player.xp = 0; }
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
            else if (statType == StatType.CurrentSP) player.currentXp = amount;
            else if (statType == StatType.MAXSP) player.xp = amount;
            else if (statType == StatType.MoveSpeed) player.moveSpeed = amount;
            else if (statType == StatType.AttackSpeed) player.attackSpeed = amount;
            else if (statType == StatType.AttackDamage) player.attackDamage = amount;
            else if (statType == StatType.AttackRange) player.attackRange = amount;
            else Debug.LogWarning("플레이어의 무엇을 바꾸고 싶으시나");
        }
        else if(operation == Operation.Reset && resetData != null)
        {
            player.hp = resetData.baseHp;
            player.attackDamage = resetData.baseDamage;
            player.attackSpeed = resetData.baseAttackSpeed;
            player.moveSpeed = resetData.baseMoveSpeed;
            player.rotationSpeed = resetData.baseRotSpeed;
            player.bulletSpeed = resetData.bulletSpeed;
            player.leftAttackAmount = 0;
            player.rightAttackAmount = 0;
            player.forwardAttackAmount = 1;
            player.backwardAttackAmount = 0;
            player.xp = 5;
            player.currentXp = 0;
            player.currentHp = 0;
        }
        else
        {
            Debug.LogWarning("미안해");
        }
    }

    public void Attack()
    {
        if(player.code == "111111P")
        {
            ShotAsDirection(BulletDirection.forward);
        }
        Debug.Log("빵야");
    }
    private void ShotAsDirection(BulletDirection direction)
    {
        GameObject temp = Managers.Data.Instantiate("Bullet",null,true);
        BulletController bc = Util.GetOrAddComponent<BulletController>(temp);
        temp.transform.position = player.playerController.transform.position + new Vector3(0,1.3f,0);
        bc.bulletType = direction;
        bc.moveSpeed = player.bulletSpeed;
        bc.damage = player.attackDamage;

        if (direction == BulletDirection.forward) bc.direction = player.playerController.transform.forward;
        else if (direction == BulletDirection.left) bc.direction = -player.playerController.transform.right;
        else if (direction == BulletDirection.right) bc.direction = player.playerController.transform.right;
        else if (direction == BulletDirection.back) bc.direction = -player.playerController.transform.forward;
        else Debug.LogWarning("방향오류");

        bullets.Add(bc);
    }
}
