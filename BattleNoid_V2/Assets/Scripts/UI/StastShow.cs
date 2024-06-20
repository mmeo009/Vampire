using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StastShow : MonoBehaviour
{
    public GameObject textObject;
    public TMP_Text text;
    void Update()
    {
        if(Input.GetKey(KeyCode.Tab))
        {
            textObject.SetActive(true);

            text.text = $"HP : {Managers.Player.player.currentHp}/{Managers.Player.player.hp}\nEXP : {Managers.Player.player.currentXp}/{Managers.Player.player.xp}\nSPD : {Managers.Player.player.moveSpeed}\n" +
                $"DMG : {Managers.Player.player.attackDamage}\nASPD : {Managers.Player.player.attackSpeed}";
        }
        
        if(Input.GetKeyUp(KeyCode.Tab))
        {
            textObject.SetActive(false);
        }
    }
}
