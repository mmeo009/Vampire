using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Supporter;
using Unity.VisualScripting;
using System.Diagnostics.Tracing;

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
                case StatType.CurrentXP:
                    player.currentXp -= amount;
                    if (player.currentXp <= 0)
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
                    if (player.moveSpeed <= 0)
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
                    if (player.attackDamage <= 0)
                    {
                        player.attackDamage = 0;
                    }
                    break;
                case StatType.AttackRange:
                    player.attackRange -= amount;
                    if (player.attackRange <= 0)
                    {
                        player.attackRange = 0;
                    }
                    break;
            }
        }
        else if (operation == OperationType.Set)
        {
            switch (statType)
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
        else if (operation == OperationType.Reset && resetData != null)
        {
            player.code = resetData.code;
            player.hp = resetData.baseHp;
            player.attackDamage = resetData.baseDamage;
            player.attackSpeed = resetData.baseAttackSpeed;
            player.attackRange = resetData.baseRange;
            player.moveSpeed = resetData.baseMoveSpeed;
            player.rotationSpeed = resetData.baseRotSpeed;
            player.bulletSpeed = resetData.bulletSpeed;
            player.leftAttackAmount = 0;
            player.rightAttackAmount = 0;
            player.forwardAttackAmount = 1;
            player.backwardAttackAmount = 0;
            player.xp = 5;
            player.currentXp = 0;
            player.currentHp = player.hp;
        }
        else
        {
            Debug.LogWarning("이상하다요");
        }

    }

    public void Attack()
    {
        if (player.code == "111111P")
        {
            for(int i  = 0; i < player.forwardAttackAmount; i++)
            {
                ShotBulletAsDirection(BulletDirection.forward, i + 1);
            }
            for(int i = 0; i< player.leftAttackAmount; i++)
            {
                ShotBulletAsDirection(BulletDirection.left, i + 1);
            }

            for(int i = 0; i < player.rightAttackAmount; i++)
            {
                ShotBulletAsDirection(BulletDirection.right, i + 1);
            }

            for (int i = 0; i < player.backwardAttackAmount; i++)
            {
                ShotBulletAsDirection(BulletDirection.back, i + 1);
            }
        }
    }


    private Vector3 GetDotPos(Vector3 direction, int dotAmount, int dotNum)
    {
        Vector3 playerPosition = player.playerController.transform.position;
        Quaternion playerRotation = player.playerController.transform.rotation;

        if (dotAmount <= 1)
        {
            return Vector3.zero;
        }

        Vector3 end = playerPosition + (playerRotation * direction) * 0.5f;
        float segmentLength = 1f / (dotAmount - 1);

        for (int i = 0; i < dotAmount; i++)
        {
            Vector3 point = end - (playerRotation * direction) * (segmentLength * i);

            if (i + 1 == dotNum)
            {
                return point - playerPosition;

            }
        }

        return Vector3.zero;
    }

    private void ShotBulletAsDirection(BulletDirection direction, int amount)
    {
        GameObject temp = Managers.Data.Instantiate("Bullet", null, true);
        BulletController bc = Util.GetOrAddComponent<BulletController>(temp);
        bullets.Add(bc);
        Vector3 dot;

        if (direction == BulletDirection.forward)
        {
            dot = GetDotPos(Vector3.right, player.forwardAttackAmount, amount);
            bc.direction = player.playerController.transform.forward;

        }
        else if (direction == BulletDirection.left)
        {
            dot = GetDotPos(Vector3.forward, player.leftAttackAmount, amount);
            bc.direction = -player.playerController.transform.right;
        }
        else if (direction == BulletDirection.right)
        {
            dot = GetDotPos(Vector3.forward, player.rightAttackAmount, amount);
            bc.direction = player.playerController.transform.right;
        }
        else if (direction == BulletDirection.back)
        {
            dot = GetDotPos(Vector3.right, player.backwardAttackAmount, amount);
            bc.direction = -player.playerController.transform.forward;
        }
        else
        {
            dot = Vector3.zero;
            Debug.LogError("Wrong Direction");
            bc.DestroyBullet();
        }

        temp.transform.position = player.playerController.transform.position + new Vector3(dot.x, 1.3f, dot.z);
        player.playerController.cubeVector = player.playerController.transform.position + new Vector3(dot.x, 1.3f, dot.z);
        bc.bulletType = direction;
        bc.moveSpeed = player.bulletSpeed;
        bc.damage = player.attackDamage;
    }

    public void UseFirstSkill()
    {
        if (player.code == "111111P")
        {

        }
    }
    public void UseSecondSkill()
    {
        if (player.code == "111111P")
        {

        }
    }
}
