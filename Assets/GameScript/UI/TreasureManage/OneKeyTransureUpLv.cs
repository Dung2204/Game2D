using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;

public class OneKeyTransureUpLv : UIFramwork
{
    OneKeyTransureUpLvParam tParam;
    TreasurePoolDT tTreasurePoolDT;


    private int MaxUpLv = 0;

    private UILabel RefineNow;
    private string srtRefineNow;

    private int ConsumeExp;
    private int ConsumeMoney;
    private int MinLvNum;
    private int ChangeRefineNum;

    private UIToggle ToggleConsume;

    private UILabel RefineNum;
    Dictionary<long, int> AddExp = new Dictionary<long, int>();
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);

        if (e == null && !(e is OneKeyTransureUpLvParam))
        {
MessageBox.ASSERT("Parameter error");
        }
        InitGUI();
        tParam = (OneKeyTransureUpLvParam)e;
        tTreasurePoolDT = tParam.m_TreasurePoolDT;
        MaxUpLv = CheckMaxUpLv();

        if (MaxUpLv == tTreasurePoolDT.m_lvIntensify)
        {
UITool.Ui_Trip("Hiện tại không đáp ứng được yêu cầu");
            ClosePage();
            return;
        }
        ChangeRefineNum = MinLvNum = tTreasurePoolDT.m_lvIntensify;

        UpdateMain();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        MaxUpLv = 0;
        RefineNow = f_GetObject("RefineNow").GetComponent<UILabel>();
        RefineNum = f_GetObject("RefineNum").GetComponent<UILabel>();
srtRefineNow = "[FFFB93]Cấp: [-]{0}";

        ConsumeExp = 0;
        ConsumeMoney = 0;

        ToggleConsume = f_GetObject("ToggleConsume").GetComponent<UIToggle>();

    }


    #region      按钮事件
    private void Btn_ChangeRefineLv(GameObject go, object obj1, object obj2)
    {
        int Addnum = (int)obj1;

        if (ChangeRefineNum + Addnum > MaxUpLv)
        {
UITool.Ui_Trip("Có thể nâng cấp lên cấp độ " + MaxUpLv + "");
            ChangeRefineNum = MaxUpLv;
        }
        else if (ChangeRefineNum + Addnum > MinLvNum)
        {
            ChangeRefineNum = MinLvNum;       
        }
        else
        {
            ChangeRefineNum += Addnum;
        }

        UpdateConsume(ChangeRefineNum);


    }

    private void Btn_ClosePage(GameObject go, object obj1, object obj2)
    {
        ClosePage();
    }

    private void ClosePage()
    {
        if (tParam.m_ccCallback != null)
            tParam.m_ccCallback(null);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.OneKeyTransureUpLv, UIMessageDef.UI_CLOSE);
    }
    #endregion


    private void UpdateMain()
    {
        ResourceCommonItem tItem = f_GetObject("ResourceCommonItem").GetComponent<ResourceCommonItem>();
        tItem.f_UpdateByInfo((int)EM_ResourceType.Treasure, tTreasurePoolDT.m_iTempleteId, 1);
        RefineNow.text = string.Format(srtRefineNow, MinLvNum);
        RefineNum.text = ChangeRefineNum.ToString();

        ToggleConsume.value = false;




    }
    /// <summary>
    /// 刷新消耗数量
    /// </summary>
    /// <param name="lv">目标等级</param>
    private void UpdateConsume(int lv)
    {
        int NeedExp = 0;
        int NowExp = tTreasurePoolDT.m_ExpIntensify;
        int Imprent = tTreasurePoolDT.m_TreasureDT.iImportant;
        TreasureUpConsumeDT tUpConsume;
        for (int i = tTreasurePoolDT.m_lvIntensify + 1; i <= lv; i++)
        {
            tUpConsume = _GetTreasureUpConsumeDT(i, Imprent);
            NeedExp += (tUpConsume.iIntensifyExp - NowExp);
            NowExp = 0;
        }

        AddExp.Clear();
        TreasurePoolDT tmpTreasurePoolDT;
        for (int i = 0; i < Data_Pool.m_TreasurePool.f_GetAll().Count; i++)
        {
            tmpTreasurePoolDT = Data_Pool.m_TreasurePool.f_GetAll()[i] as TreasurePoolDT;

            if (tmpTreasurePoolDT.m_TreasureDT.iImportant >= Imprent)
            {
                if (!ToggleConsume)
                {
                    continue;
                }
            }
            for (int j = 0; j < tmpTreasurePoolDT.m_Num; j++)
            {
                if (tmpTreasurePoolDT.m_ExpIntensify <= NeedExp)
                {
                    if (AddExp.ContainsKey(tmpTreasurePoolDT.iId))
                        AddExp[tmpTreasurePoolDT.iId]++;
                    else
                        AddExp.Add(tmpTreasurePoolDT.iId, 1);

                    NeedExp -= tmpTreasurePoolDT.m_ExpIntensify;
                }
                else { break; }
            }


        }


    }

    private int CheckMaxUpLv()
    {
        int canAddExp = 0;
        List<BasePoolDT<long>> tList = Data_Pool.m_TreasurePool.f_GetAll();
        TreasurePoolDT tTreasurePool;
        for (int i = 0; i < tList.Count; i++)
        {
            tTreasurePool = tList[i] as TreasurePoolDT;
            if (tTreasurePool == tTreasurePoolDT)
                continue;
            if (tTreasurePool.m_TreasureDT.iImportant >= tTreasurePoolDT.m_TreasureDT.iImportant)
                continue;
            canAddExp += tTreasurePool.m_Num * tTreasurePool.m_TreasureDT.iExp;
        }

        int NowLv = tTreasurePoolDT.m_lvIntensify;
        int NowExp = tTreasurePoolDT.m_ExpIntensify;

        TreasureUpConsumeDT tUppConsume = null;
        do
        {
            tUppConsume = _GetTreasureUpConsumeDT(NowLv, tTreasurePoolDT.m_TreasureDT.iImportant);
            if (tUppConsume.iIntensifyExp - NowExp >= canAddExp)
            {
                break;
            }
            NowLv++;
            NowExp = 0;

        } while (tUppConsume != null);


        return NowLv;

    }

    private TreasureUpConsumeDT _GetTreasureUpConsumeDT(int lv, int Imporent)
    {
        TreasureUpConsumeDT tUpConsusme = null;
        tUpConsusme = glo_Main.GetInstance().m_SC_Pool.m_TreasureUpConsumeSC.f_GetSC(Imporent * 1000 + lv + 1) as TreasureUpConsumeDT;
        return tUpConsusme;

    }

}


public class OneKeyTransureUpLvParam
{
    public TreasurePoolDT m_TreasurePoolDT;


    public ccCallback m_ccCallback;
}
