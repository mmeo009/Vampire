using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoolTime : MonoBehaviour
{
    public Text text_CoolTime;
    public Image image_fill;
    public float time_cooltime = 10;        // 쿨타임 시간 설정
    private float time_current;
    private float time_start;
    private bool isEnded = true;
    public KeyCode activationKey; // 사용자가 지정할 키

    void Start()
    {
        Init_UI();
    }

    void Update()
    {
        if (Input.GetKeyDown(activationKey) && isEnded) // 사용자가 설정한 키를 눌렀고, 쿨타임이 끝났다면
        {
            Trigger_Skill(); // 스킬 발동
        }

        if (isEnded)
            return;
        Check_CoolTime();
    }

    private void Init_UI()
    {
        image_fill.type = Image.Type.Filled;
        image_fill.fillMethod = Image.FillMethod.Radial360;
        image_fill.fillOrigin = (int)Image.Origin360.Top;
        image_fill.fillClockwise = false;
        Debug.Log("발동됨");
    }

    private void Check_CoolTime()
    {
        time_current = Time.time - time_start;
        if (time_current < time_cooltime)
        {
            Set_FillAmount(time_cooltime - time_current);
        }
        else if (!isEnded)
        {
            End_CoolTime();
        }
    }

    private void End_CoolTime()
    {
        Set_FillAmount(0);
        isEnded = true;
        text_CoolTime.gameObject.SetActive(false);
        Debug.Log("스킬 사용가능이요");
    }

    private void Trigger_Skill()
    {
        if (!isEnded)
        {
            Debug.LogError("기다려 임마");
            return;
        }

        Reset_CoolTime();
        // Debug.LogError("뿅");
    }

    private void Reset_CoolTime()
    {
        text_CoolTime.gameObject.SetActive(true);
        time_current = time_cooltime;
        time_start = Time.time;
        Set_FillAmount(time_cooltime);
        isEnded = false;
    }

    private void Set_FillAmount(float _value)
    {
        image_fill.fillAmount = _value / time_cooltime;
        string txt = _value.ToString("0.0");
        text_CoolTime.text = txt;
        // Debug.Log(txt);
    }
}
