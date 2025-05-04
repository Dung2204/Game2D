using UnityEngine;
using System.Collections.Generic;

public class DialogPool
{
    public Dictionary<int, List<DungeonDialogDT>> mDataDic;
    public Dictionary<int, bool> mNeedDic;

    public DialogPool()
    {
        f_Init();
    }

    private void f_Init()
    {
        mDataDic = new Dictionary<int, List<DungeonDialogDT>>();
        mNeedDic = new Dictionary<int, bool>();
        System.Array tInitData = System.Enum.GetValues(typeof(EM_DialogcCondition));
        for (int i = 0; i < tInitData.Length; i++)
        {
            mDataDic.Add((int)tInitData.GetValue(i), new List<DungeonDialogDT>());
            mNeedDic.Add((int)tInitData.GetValue(i),false);
        }
    }

    /// <summary>
    /// 更新对话数据                 第一次胜利后不需要再播放
    /// </summary>
    /// <param name="battleType"></param>
    /// <param name="tollgateId"></param>
    public void f_UpdateDialogDataByFirstWin(int battleType,int tollgateId)
    {
        //保存通过对话数据
        PlayerPrefs.SetInt(string.Format(GameParamConst.DialogTollgateIdx, Data_Pool.m_UserData.m_iUserId,battleType), tollgateId);
    }

    /// <summary>
    /// 更新单前关卡的对话数据  在挑战关卡前调用
    /// </summary>
    /// <param name="tollgateId"></param>
    public void f_UpdateCurDialogData(EM_Fight_Enum battleType,int chapterId,int tollgateId)
    {
        foreach (int itemKey in mDataDic.Keys)
        {
            mDataDic[itemKey].Clear();
            mNeedDic[itemKey] = false;
        }
        if (battleType != EM_Fight_Enum.eFight_DungeonMain
            && battleType != EM_Fight_Enum.eFight_DungeonElite
            && battleType != EM_Fight_Enum.eFight_Legend
            )
        {
            return;
        }
        
        //检测是否是单前能打的关卡
        bool tIsFightTollgate = Data_Pool.m_DungeonPool.f_CheckIsFightTollgate(chapterId,tollgateId);
        if (CmdParam.m_IgnoreDialogLimit)
        {
            List<NBaseSCDT> tmp = glo_Main.GetInstance().m_SC_Pool.m_DungeonDialogSC.f_GetAll();
            DungeonDialogDT tmpItem;
            for (int i = 0; i < tmp.Count; i++)
            {
                tmpItem = (DungeonDialogDT)tmp[i];
                if (tmpItem.iTollgateId == tollgateId)
                {
                    if (mDataDic.ContainsKey(tmpItem.iCondition))
                    {
                        mDataDic[tmpItem.iCondition].Add(tmpItem);
                        mNeedDic[tmpItem.iCondition] = true;
                    }
                    else
MessageBox.ASSERT(string.Format("Unknown conditional table DungeonDialog TollgateId:{0} type:{1}", tollgateId, tmpItem.iCondition));
                }
            }
            return;
        }
        if (tIsFightTollgate)
        {
            //本地保存通过Idx，如果Idx相同 则表示之前已打过该关卡
            int tId = PlayerPrefs.GetInt(string.Format(GameParamConst.DialogTollgateIdx,Data_Pool.m_UserData.m_iUserId,(int)battleType), -1); 
            if (tId == tollgateId)
                return;
            List<NBaseSCDT> tmp = glo_Main.GetInstance().m_SC_Pool.m_DungeonDialogSC.f_GetAll();
            DungeonDialogDT tmpItem;
            for (int i = 0; i < tmp.Count; i++)
            {
                tmpItem = (DungeonDialogDT)tmp[i];
                if (tmpItem.iTollgateId == tollgateId)
                {
                    if (mDataDic.ContainsKey(tmpItem.iCondition))
                    {
                        mDataDic[tmpItem.iCondition].Add(tmpItem);
                        mNeedDic[tmpItem.iCondition] = true;
                    }
                    else
MessageBox.ASSERT(string.Format("Unknown conditional table DungeonDialog TollgateId:{0} type:{1}", tollgateId, tmpItem.iCondition));
                }
            }
        }
    }

    /// <summary>
    /// 检查单前是否需要播放对话
    /// </summary>
    /// <returns></returns>
    public bool f_CheckCurNeedDialog(EM_DialogcCondition condition)
    {
        return mNeedDic[(int)condition];
    }

    /// <summary>
    /// 检查单前是否需要播放对话 有类型参数
    /// </summary>
    public bool f_CheckCurNeedDialog(EM_DialogcCondition condition, int conditionParam)
    {
        if (mNeedDic[(int)condition])
        {
            List<DungeonDialogDT> tmp = f_GetCurNeedDialog(condition, conditionParam);
            return tmp.Count != 0;
        }
        return false;
    }

    /// <summary>
    /// 获取对话数据
    /// </summary>
    public List<DungeonDialogDT> f_GetCurNeedDialog(EM_DialogcCondition condition)
    {
        return mDataDic[(int)condition];
    }

    /// <summary>
    /// 获取对话数据 有类型参数
    /// </summary>
    public List<DungeonDialogDT> f_GetCurNeedDialog(EM_DialogcCondition condition, int conditionParam)
    {
        return mDataDic[(int)condition].FindAll(delegate (DungeonDialogDT item) { return item.iConditionParam == conditionParam; });
    }

}
