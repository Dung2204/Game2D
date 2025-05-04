using UnityEngine;
using System.Collections;
using ccU3DEngine;

public class CharacterSC : NBaseSC
{

    public CharacterSC()
    {
        Create("CharacterDT");
    }

    public override void f_LoadSCForData(string strBuild)
    {
        DispSaveData(strBuild);
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    private void DispSaveData(string ppSQL)
    {
        string[] ttt = ppSQL.Split(new string[] { "1#QW" }, System.StringSplitOptions.None);

        //1
        CharacterDT DataDT;
        string[] tData;
        string[] tFoddScData = ttt[1].Split(new string[] { "|" }, System.StringSplitOptions.None);
        for (int i = 0; i < tFoddScData.Length; i++)
        {
            try
            {
                if (tFoddScData[i] == "")
                {
MessageBox.DEBUG(m_strRegDTName + "String with empty record, " + i);
                    continue;
                }
                tData = tFoddScData[i].Split(new string[] { "@," }, System.StringSplitOptions.None);
                int a = 0;
                DataDT = new CharacterDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (!CheckEro.f_CheckId(DataDT.iId))
                {
MessageBox.DEBUG(m_strRegDTName + "Error String, id," + i);
                    continue;
                }
                //iId	strName	iType	strReadme	iLvUpTeamId	iUnLockNum	iUnLockActiveNum	iCarryMoney	iLimitNum		
                DataDT.strName = tData[a++];
                DataDT.iType = ccMath.atoi(tData[a++]);
                DataDT.strReadme = tData[a++];
             
                SaveItem(DataDT);
            }
            catch
            {
MessageBox.DEBUG(m_strRegDTName + "String record error, " + i);
                continue;
            }
        }
    }



}
