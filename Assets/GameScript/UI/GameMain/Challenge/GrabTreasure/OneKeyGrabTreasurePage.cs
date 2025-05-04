using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;

public class OneKeyGrabTreasurePage : UIFramwork
{
    private UIWrapComponent _Result;

    //public UIWrapComponent mResult
    //{
    //    get
    //    {
    //        if (_Result == null)
    //        {
    //            //tmpTreasure = Data_Pool.m_GrabTreasurePool.m_GrabTreasureOneKeyResult;
    //            _Result = new UIWrapComponent(852 , 1 , 5 , 1 , f_GetObject("ItemParent") , f_GetObject("Item") , tmpTreasure , null , null);
    //        }

    //        return _Result;
    //    }
    //    set { _Result = value; }
    //}

    //private int Time_UpdateItem = 0;
    private int Index = 0;
    private float fIntervalTime = 0.2f;
    private float fShowItemTime = 0.45f;

    private bool bTweenOver = false;
    private int mixTreasureCount;
    private int Time_UpdateItem = 0;

    private UIScrollBar tUIBard;
    private TreasureDT curSelectTreasure;

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        Data_Pool.m_GuidancePool.IsOpenOneKeyGrabTreasure = true;
        curSelectTreasure = (TreasureDT)e;
        bTweenOver = false;
        Index = 0;
        Time_UpdateItem = 0;
        int lv = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        int exp = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Exp);
        StaticValue.m_sLvInfo.f_UpdateInfo(lv, exp);
        tUIBard = f_GetObject("Bar").GetComponent<UIScrollBar>();
        f_GetObject("SucDesc").SetActive(false);
        UpdateMain();
        SendOneKey();
        //for (int i = 0 ; Data_Pool.m_GrabTreasurePool.m_GrabTreasureOneKeyResult.Count > i ; i++)
        //{
        //    SC_GrabTreasure tSC_GrabTreasure = Data_Pool.m_GrabTreasurePool.m_GrabTreasureOneKeyResult [i];
        //    GrabTreasurePoolDT poolDT = new GrabTreasurePoolDT();
        //    poolDT.iId = i;
        //    poolDT.star = tSC_GrabTreasure.star;
        //    poolDT.awardID = tSC_GrabTreasure.awardId;
        //    poolDT.resourceType = tSC_GrabTreasure.awardNode.resourceType;
        //    poolDT.resourceId = tSC_GrabTreasure.awardNode.resourceId;
        //    poolDT.resourceNum = tSC_GrabTreasure.awardNode.resourceNum;
        //    poolDT.treaFragID = tSC_GrabTreasure.treaFragId;
        //    tmpTreasure.Add(poolDT);
        //}

        //tmpTreasure = Data_Pool.m_GrabTreasurePool.m_GrabTreasureOneKeyResult;
    }

    protected override void f_InitMessage()
    {
        base.f_InitMessage();

        f_RegClickEvent("CloseBtn", On_Close);
        f_RegClickEvent("Alphe", On_Close);
    }

    private void UpdateMain()
    {
        UITool.f_SetIconSprite(f_GetObject("GoodsIcon").GetComponent<UI2DSprite>(), EM_ResourceType.Good, 201);
        //f_GetObject("GoodsIcon").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSprite();
        f_GetObject("ResidueNum").GetComponent<UILabel>().text = Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(201).ToString();

        for(int i = 0; i < f_GetObject("ItemParent").transform.childCount; i++)
            Destroy(f_GetObject("ItemParent").transform.GetChild(i).gameObject);

        //UpdateItem(Data_Pool.m_GrabTreasurePool.m_GrabTreasureOneKeyResult[Index]);
        //Time_UpdateItem = ccTimeEvent.GetInstance().f_RegEvent(fIntervalTime , true ,  , UpdateItem);

    }

    private void UpdateItem(object obj)
    {
        if(obj == null)
        {
            return;
        }
        bTweenOver = true;



        GameObject Item = NGUITools.AddChild(f_GetObject("ItemParent"), f_GetObject("Item"));

        GrabSweepResultItem tmpItem = Item.GetComponent<GrabSweepResultItem>();
        OneKeyFragmentInfo tmpGrab = (OneKeyFragmentInfo)obj;
        Item.SetActive(true);
        f_GetObject("ItemParent").GetComponent<UIGrid>().Reposition();
        int tLv = StaticValue.m_sLvInfo.m_iAddLv;
        int moneyCount = GameFormula.f_VigorCost2Money(tLv, 2);
        int addExp;
        int exCount = GameFormula.f_VigorCost2Exp(tLv, 2, out addExp);
        string strAddExp = addExp > 0 ? "[FFF700FF]（+" + addExp + "）" : "";

        string Hint = string.Empty;
        if(tmpGrab.treaFragId > 0)
        {
            ResourceCommonDT commonDt = new ResourceCommonDT();
            commonDt.f_UpdateInfo((byte)EM_ResourceType.TreasureFragment, tmpGrab.treaFragId, 1);
            string name = commonDt.mName;
            UITool.f_GetImporentColorName(commonDt.mImportant, ref name);
            Hint = CommonTools.f_GetTransLanguage(787) + name;
        }
        if(Hint == string.Empty)
        {
            Hint = string.Format(CommonTools.f_GetTransLanguage(788));
        }
        //AwardPoolDT awardItem = new AwardPoolDT();
        //awardItem.f_UpdateByInfo(tmpGrab.awardNode.resourceType, tmpGrab.awardNode.resourceId, tmpGrab.awardNode.resourceNum);
        tmpItem.SetData(Index + 1, Hint, moneyCount, exCount + strAddExp, (EM_ResourceType)tmpGrab.awardNode.resourceType,
            tmpGrab.awardNode.resourceId, tmpGrab.awardNode.resourceNum, tmpGrab.star <= 0 ? true : false, true);

        Index++;
        if(Index < 3)
        {
            tUIBard.value = 0.01f;
        }
        else
        {
            tUIBard.value = 0.5f;
            tUIBard.value = 1f;
        }

        if(Index >= Data_Pool.m_GrabTreasurePool.m_GrabTreasureOneKeyResult.Count)
        {
            SendOneKey();
            ccTimeEvent.GetInstance().f_RegEvent(1f, false, null, UpdateBar);
            return;
        }
        Time_UpdateItem= ccTimeEvent.GetInstance().f_RegEvent(fShowItemTime, false, Data_Pool.m_GrabTreasurePool.m_GrabTreasureOneKeyResult[Index], UpdateItem);

    }

    private void On_TweenOver()
    {
        tUIBard.f_RightArray();
        tUIBard.value = 0.99f;
        bTweenOver = false;
        mixTreasureCount = Data_Pool.m_TreasurePool.f_CheckTreasureCanMixMaxCount(curSelectTreasure.iId);


        f_GetObject("SucDesc").SetActive(mixTreasureCount >= 1);

        if(mixTreasureCount >= 1)
        {
            string TreasureName = UITool.f_GetImporentForName(curSelectTreasure.iImportant, curSelectTreasure.szName);

            f_GetObject("SucDesc").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(789), TreasureName);
        }
    }


    private void On_Close(GameObject go, object obj1, object obj2)
    {

        if(bTweenOver)
            return;
        ccTimeEvent.GetInstance().f_UnRegEvent(Time_UpdateItem);
        Data_Pool.m_GuidancePool.IsOpenOneKeyGrabTreasure = false;
        Data_Pool.m_GrabTreasurePool.m_GrabTreasureOneKeyResult.Clear();
        ccUIManage.GetInstance().f_SendMsg(UINameConst.OneKeyGrabTreasurePage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GrabTreasurePage, UIMessageDef.UI_OPEN, GrabTreasurePage.curSelectTreasure.iId);
    }

    private void SendOneKey()
    {
        if(Data_Pool.m_TreasurePool.f_CheckTreasureCanMixMaxCount(curSelectTreasure.iId) >= 1)
        {
            On_TweenOver();
            return;
        }
        if(!UITool.f_IsEnoughMoney(EM_MoneyType.eUserAttr_Vigor, 2, false, false,this))
        {
            if(GrabTreasurePage.mbIsAutoGoods)
            {
                if(Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(201) > 0)
                {
                    SocketCallbackDT tmp = new SocketCallbackDT();
                    tmp.m_ccCallbackFail = TreasureOneKeyGoodsFail;
                    tmp.m_ccCallbackSuc = TreasureOneKeyGoodsSuc;
                    Data_Pool.m_BaseGoodsPool.f_Use(Data_Pool.m_BaseGoodsPool.f_GetForData5(201).iId, 1, 0, tmp);
                }
                else
                {
                    bTweenOver = false;
                    return;
                }
            }
            bTweenOver = false;
            return;
        }

        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT TreasureOneKey = new SocketCallbackDT();
        TreasureOneKey.m_ccCallbackSuc = TreasureOneKeySuc;
        TreasureOneKey.m_ccCallbackFail = TreasureOneKetFail;
        Data_Pool.m_GrabTreasurePool.f_GrabTreasureOneKey(curSelectTreasure.iId, TreasureOneKey);
    }

    private void TreasureOneKeyGoodsSuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        f_GetObject("ResidueNum").GetComponent<UILabel>().text = Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(201).ToString();
        SendOneKey();
    }
    private void TreasureOneKeyGoodsFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(790) + obj.ToString());
    }

    private void TreasureOneKeySuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if(Index >= Data_Pool.m_GrabTreasurePool.m_GrabTreasureOneKeyResult.Count)
            return;
        UpdateItem(Data_Pool.m_GrabTreasurePool.m_GrabTreasureOneKeyResult[Index]);
    }
    private void TreasureOneKetFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        bTweenOver = true;
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(791) + obj.ToString());
    }
    private void UpdateBar(object obj)
    {
        tUIBard.value = 1f;
    }
}
