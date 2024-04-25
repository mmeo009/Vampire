using Supporter;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text coolTime;
    [SerializeField] private Image image;
    [Header("¾ê¸¸ 1,2·Î ¼öÁ¤")][Range(0,1)][SerializeField] private int skillNum;
    [SerializeField] private string skillName;

    private void OnEnable()
    {
        Util.GetOrAddComponent<Image>(this.gameObject);
        coolTime = GetComponent<TMP_Text>();
        Managers.UI.InitSkillUI(image, skillName);
    }
    private void Update()
    {
        SetFillAmount();
    }
    private void SetFillAmount()
    {
        string txt = "";

        if (skillNum == 1)
        {
            image.fillAmount = Managers.Player.player.currentFirstCoolDown / Managers.Player.player.firstCoolDown;
            txt = Managers.Player.player.currentFirstCoolDown.ToString("0.0");
        }
        else if(skillNum == 2)
        {
            image.fillAmount = Managers.Player.player.currentSecondCoolDown / Managers.Player.player.secondCoolDown;
            txt = Managers.Player.player.currentSecondCoolDown.ToString("0.0");
        }
        else
        {
            txt = "";
        }
        coolTime.text = txt;
    }
}
