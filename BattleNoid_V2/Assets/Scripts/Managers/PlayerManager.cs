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
        SetStats(OperationType.Reset, StatType.None, 0, _playerData);
        player.playerController.LoadData();
        CameraController cc = Util.GetOrAddComponent<CameraController>(player.playerController.gameObject);
        cc.FindCamera();
    }
    public void SetStats(OperationType operation, StatType statType, float amount, Entity_Player.Param resetData = null)
    {
        if(operation == OperationType.Plus)
        {
            switch (statType)
            {
                case StatType.CurrentHP:
                    player.currentHp += amount;
                    break;
                case StatType.MAXHP:
                    player.hp += amount;
                    break;
                case StatType.CurrentXP:
                    player.currentXp += amount;
                    break;
                case StatType.MAXXP:
                    player.xp += amount;
                    break;
                case StatType.MoveSpeed:
                    player.moveSpeed += amount;
                    break;
                case StatType.AttackSpeed:
                    player.attackSpeed += amount;
                    break;
                case StatType.AttackDamage:
                    player.attackDamage += amount;
                    break;
                case StatType.AttackRange:
                    player.attackRange += amount;
                    break;
            }
        }
        else if(operation == OperationType.Minus)
        {
            switch (statType)
            {
                case StatType.CurrentHP:
                    player.currentHp -= amount;
                    if (player.currentHp <= 0)
                    {
                        player.currentHp = 0;
                    }
                    break;
                case StatType.MAXHP:
                    player.hp -= amount;
                    if (player.hp <= 0)
                    {
                        player.hp = 0;
                    }
                    break;
                case StatType.CurrentXP:
                    player.currentXp -= amount;
                    if(player.currentXp <= 0)
                    {
                        player.currentXp = 0;
                    }
                    break;
                case StatType.MAXXP:
                    player.xp -= amount;
                    if (player.xp <= 0)
                    {
                        player.xp = 0;
                    }
                    break;
                case StatType.MoveSpeed:
                    player.moveSpeed -= amount;
                    if(player.moveSpeed <= 0)
                    {
                        player.moveSpeed = 0;
                    }
                    break;
                case StatType.AttackSpeed:
                    player.attackSpeed -= amount;
                    if (player.attackSpeed <= 0)
                    {
                        player.attackSpeed = 0;
                    }
                    break;
                case StatType.AttackDamage:
                    player.attackDamage -= amount;
                    if(player.attackDamage <= 0)
                    {
                        player.attackDamage = 0;
                    }
                    break;
                case StatType.AttackRange:
                    player.attackRange -= amount;
                    if( player.attackRange <= 0)
                    {
                        player.attackRange = 0;
                    }
                    break;
            }
        }
        else if(operation == OperationType.Set)
        {
            switch(statType)
            {
                case StatType.CurrentHP:
                    player.currentHp = amount;
                    break;
                case StatType.MAXHP:
                    player.hp = amount; 
                    break;
                case StatType.CurrentXP: 
                    player.currentXp = amount; 
                    break;
                case StatType.MAXXP:
                    player.xp = amount;
                    break;
                case StatType.MoveSpeed:
                    player.moveSpeed = amount;
                    break;
                case StatType.AttackSpeed:
                    player.attackSpeed = amount;
                    break;
                case StatType.AttackDamage:
                    player.attackDamage = amount;
                    break;
                case StatType.AttackRange:
                    player.attackRange = amount;
                    break;
            }
        }
        else if(operation == OperationType.Reset && resetData != null)
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
