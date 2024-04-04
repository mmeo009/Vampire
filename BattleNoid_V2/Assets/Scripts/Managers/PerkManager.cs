using Supporter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkManager : MonoBehaviour
{

    public PerkData teskType(Entity_Perk.Param perk)
    {
        int teskAmount = perk.taskAmount;
        string taskType = perk.taskType;
        string taskCondition = perk.taskCondition;
        PerkTeskData[] data = new PerkTeskData[teskAmount];
        PerkData perkData = new PerkData();
        if (taskType != null)
        {
            string[] keyType = taskType.TrimEnd(',').Split(',');
            string[] keyCondition = taskCondition.TrimEnd(',').Split(',');
            for (int i = 0; i < keyType.Length; i++) 
            {
                int tempNum = int.Parse(keyType[i]);
                data[i].teskNum = tempNum;
                data[i].tesk = TeskNumToEnum(tempNum);
                data[i].targetData = TeskStatStringToEnum(keyCondition[i]);
            }
            perkData.code = perk.code;
            perkData.name = perk.name;
            perkData.replicatable = perk.isReplicatable;
            perkData.perkTesks = data;
            return perkData;
        }
        return null;
    }
    public PerkTesk TeskNumToEnum(int num)
    {
        if (num == 0) return PerkTesk.None;
        else if (num == 1) return PerkTesk.PlusPercent;
        else if (num == 2) return PerkTesk.MinusPercent;
        else if (num == 3) return PerkTesk.SatAsAmount;
        else if (num == 4) return PerkTesk.PlusAndMinus;
        else if (num == 5) return PerkTesk.SetBool;
        else if (num == 6) return PerkTesk.Timer;
        else if (num == 7) return PerkTesk.If;
        else return PerkTesk.None;
    }
    public StatType TeskStatStringToEnum(string teskStat)
    {
        if (teskStat == null) return StatType.None;
        else if (teskStat == "ASPD") return StatType.AttackSpeed;
        else if (teskStat == "ADMG") return StatType.AttackDamage;
        else if (teskStat == "ARNG") return StatType.AttackRange;
        else return StatType.None;
    }
    public void Task(PerkTesk tesk)
    {
        if(tesk == PerkTesk.PlusPercent)
        {

        }
    }

    private void PlusPercent()
    {

    }
}
