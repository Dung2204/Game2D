using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;

public class UseGoodOnekeyPage : UIFramwork
{
    private UI2DSprite mIcon;
    private UISprite mFrame;
    private UILabel mGoodDesc;
    private UILabel mGoodName;
    private UILabel mHaveNum;
    private UILabel mUseNum;


    private int goodId;
    private BaseGoodsDT goodTemplate;
    private int haveNum;
    private int useNum;
    List<BasePoolDT<long>> goodPoolDtList;
    private int curPoolDtListIdx;
    private int curRequestNum;
    private int totalRequestNum;
    
    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mIcon = f_GetObject("Icon").GetComponent<UI2DSprite>();
        mFrame = f_GetObject("Frame").GetComponent<UISprite>();
        mGoodDesc = f_GetObject("GoodDesc").GetComponent<UILabel>();
        mGoodName = f_GetObject("GoodName").GetComponent<UILabel>();
        mHaveNum = f_GetObject("HaveNum").GetComponent<UILabel>();
        mUseNum = f_GetObject("UseNum").GetComponent<UILabel>();

        f_RegClickEvent("BtnAddOne", f_BtnAddOne);
        f_RegClickEvent("BtnAddTen", f_BtnAddTen);
        f_RegClickEvent("BtnMinusOne", f_BtnMinusOne);
        f_RegClickEvent("BtnMinusTen", f_BtnMinusTen);
        f_RegClickEvent("BtnClose", f_BtnClose);
        f_RegClickEvent("BtnUse", f_BtnUse);
        f_RegClickEvent("MaskClose", f_BtnClose);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        goodId = (int)e;
        goodTemplate = (BaseGoodsDT)glo_Main.GetInstance().m_SC_Pool.m_BaseGoodsSC.f_GetSC(goodId);
        if (goodTemplate == null)
MessageBox.ASSERT("Item used using interface parameter does not exist, Id:" + goodId);
        mGoodDesc.text = goodTemplate.szReadme;
        string goodName = goodTemplate.szName;
        mFrame.spriteName = UITool.f_GetImporentColorName(goodTemplate.iImportant, ref goodName);
        mGoodName.text = goodName;
        UITool.f_SetIconSprite(mIcon, EM_ResourceType.Good, goodId);
        haveNum = Data_Pool.m_BaseGoodsPool.f_GetHaveNumByTemplate(goodId);
mHaveNum.text = string.Format("[f1b049]Hiện có：[-][fffaec]{0}", haveNum);
        useNum = 0;
        mUseNum.text = useNum.ToString();
        f_BtnAddOne(null, null, null);
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }

    private void f_BtnUse(GameObject go, object value1, object value2)
    {
        if (useNum == 0)
        {
            f_BtnClose(null,null,null);
            return;
        }
        goodPoolDtList = Data_Pool.m_BaseGoodsPool.f_GetAllForData5(goodId);
        if (goodPoolDtList.Count <= 0)
            return;
        curPoolDtListIdx = 0;
        curRequestNum = 0;
        totalRequestNum = 0;
        f_RequestUseGood(goodPoolDtList[curPoolDtListIdx]);
    }

    private void f_RequestUseGood(object value)
    {
        BaseGoodsPoolDT tItem = (BaseGoodsPoolDT)value;
        if (useNum - totalRequestNum <= tItem.m_iNum)
        {
            curRequestNum = useNum - totalRequestNum;     
        }
        else
        {
            curRequestNum = tItem.m_iNum;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_CallbackUseGood;
        socketCallbackDt.m_ccCallbackFail = f_CallbackUseGood;
        Data_Pool.m_BaseGoodsPool.f_Use(tItem.iId, curRequestNum, 0, socketCallbackDt);
    }

    private void f_RequestUseGoodDelay(object tItem, float delayTime)
    {
        ccTimeEvent.GetInstance().f_RegEvent(delayTime,false,tItem, f_RequestUseGood);
    }

    private void f_CallbackUseGood(object result)
    {
        curPoolDtListIdx++;
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            totalRequestNum += curRequestNum; 
            if (totalRequestNum == useNum)
            {
                UITool.f_OpenOrCloseWaitTip(false);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.UseGoodOnekeyPage, UIMessageDef.UI_CLOSE);
            }
            else
            {
                if (curPoolDtListIdx < goodPoolDtList.Count)
                    f_RequestUseGoodDelay(goodPoolDtList[curPoolDtListIdx], 0.01f);
                else
                {
                    UITool.f_OpenOrCloseWaitTip(false);
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.UseGoodOnekeyPage, UIMessageDef.UI_CLOSE);
UITool.Ui_Trip("Lỗi");
MessageBox.ASSERT("Error using a lot, request failed");
                }
            }
        }
        else
        {
            UITool.f_OpenOrCloseWaitTip(false);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.UseGoodOnekeyPage, UIMessageDef.UI_CLOSE);
UITool.UI_ShowFailContent("Request error,code:" + result);
        }
    }

    private void f_BtnClose(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.UseGoodOnekeyPage, UIMessageDef.UI_CLOSE);
    }

    private bool f_AddOne()
    {
        if (useNum + 1 <= haveNum)
        {
            if (goodId == GameParamConst.EnergyGoodId)
            {
                int energyNum = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Energy);
                energyNum += goodTemplate.iEffectData * (useNum + 1);
                int mEnergyLimit = UITool.f_GetNowVipPrivilege((int)EM_VipPrivilege.eVip_EnergyLimit); //GameParamConst.EnergyMax
                if (energyNum > mEnergyLimit)
                {
UITool.Ui_Trip("Đã ở mức tối đa");
                    return false;
                }
            }
            else if (goodId == GameParamConst.VigorGoodId)
            {
                int vigorNum = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Vigor);
                vigorNum += goodTemplate.iEffectData * (useNum + 1);
                if (vigorNum > GameParamConst.VigorMax)
                {
UITool.Ui_Trip("Đã ở mức tối đa");
                    return false;
                }
            }
            useNum++;
            mUseNum.text = useNum.ToString();
            return true;
        }
        return false;
    }

    private void f_BtnAddOne(GameObject go, object value1, object value2)
    {
        f_AddOne();
    }

    private void f_BtnAddTen(GameObject go, object value1, object value2)
    {
        for (int i = 0; i < 10; i++)
        {
            if (!f_AddOne())
                break;
        }
    }

    private void f_BtnMinusOne(GameObject go, object value1, object value2)
    {
        if (useNum - 1 > 0)
        {
            useNum--;
            mUseNum.text = useNum.ToString();
        }
    }

    private void f_BtnMinusTen(GameObject go, object value1, object value2)
    {
        for (int i = 0; i < 10; i++)
        {
            f_BtnMinusOne(null, null, null);
        }
    }

}
