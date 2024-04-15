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
        Entity_Player.Param playerData = Managers.Data.GetDataFromDictionary(Managers.Data.playerDictionary, index, code);

        if (string.IsNullOrEmpty(code))
        {
            code = playerData.code;
        }

        GameObject playerObject = Managers.Data.Instantiate(code, null, true);
        player.playerController = playerObject.GetComponent<PlayerController>();
        SetStats(OperationType.Reset, StatType.None, 0, playerData);
        player.playerController.LoadData();
        CameraController cc = Util.GetOrAddComponent<CameraController>(player.playerController.gameObject);
        cc.FindCamera();
    }

    public void SetStats(OperationType operation, StatType statType, float amount, Entity_Player.Param resetData = null)
    {
        if (operation == OperationType.Plus)
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
        else if (operation == OperationType.Minus)
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
                    // Other cases omitted for brevity
            }
        }
        // Other cases omitted for brevity
    }

    public void Attack()
    {
        if (player.code == "111111P")
        {
            ShotAsDirection(BulletDirection.forward);
        }
        Debug.Log("»§¾ß");
    }

    private void ShotAsDirection(BulletDirection direction)
    {
        GameObject temp = Managers.Data.Instantiate("Bullet", null, true);
        BulletController bc = Util.GetOrAddComponent<BulletController>(temp);
        temp.transform.position = player.playerController.transform.position + new Vector3(0, 1.3f, 0);
        bc.bulletType = direction;
        bc.moveSpeed = player.bulletSpeed;
        bc.damage = player.attackDamage;

        if (direction == BulletDirection.forward) bc.direction = player.playerController.transform.forward;
        else if (direction == BulletDirection.left) bc.direction = -player.playerController.transform.right;
        // Other conditions omitted for brevity

        bullets.Add(bc);
    }
}
