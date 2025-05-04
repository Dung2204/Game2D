using UnityEngine;
using System.Collections;
using ccU3DEngine;
/// <summary>
/// 商店界面
/// </summary>
public class ShopStorePage : UIFramwork
{
    private int playerLevel;//玩家等级
    /// <summary>
    /// 界面打开
    /// </summary>
    /// <param name="e"></param>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        playerLevel = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level);
        f_GetObject("ScrollView").GetComponent<UIScrollView>().ResetPosition();
        InitUIData();
    }

    protected override void UI_HOLD(object e)
    {
        base.UI_HOLD(e);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.MainMenu, UIMessageDef.UI_CLOSE);
    }

    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.MainMenu, UIMessageDef.UI_OPEN);
    }
    /// <summary>
    /// 解锁和UI控制
    /// </summary>
    private void InitUIData()
    {
        //军团解锁等级
        SetIsLock(f_GetObject("BtnLegion"), playerLevel < UITool.f_GetSysOpenLevel(EM_NeedLevel.LegionLevel) ? true : false, UITool.f_GetSysOpenLevel(EM_NeedLevel.LegionLevel));
        SetIsLock(f_GetObject("BtnAwakening"), playerLevel < UITool.f_GetSysOpenLevel(EM_NeedLevel.AwakeShopLevel) ? true : false, UITool.f_GetSysOpenLevel(EM_NeedLevel.AwakeShopLevel));
        SetIsLock(f_GetObject("BtnCardShop"), playerLevel < 0 ? true : false, 0);
        SetIsLock(f_GetObject("BtnReputation"), playerLevel < UITool.f_GetSysOpenLevel(EM_NeedLevel.ArenaLevel) ? true : false, UITool.f_GetSysOpenLevel(EM_NeedLevel.ArenaLevel));
        SetIsLock(f_GetObject("BtnEquip"), playerLevel < UITool.f_GetSysOpenLevel(EM_NeedLevel.RunningManLvel) ? true : false, UITool.f_GetSysOpenLevel(EM_NeedLevel.RunningManLvel));
        SetIsLock(f_GetObject("BtnBattleFeat"), playerLevel < UITool.f_GetSysOpenLevel(EM_NeedLevel.RebelArmyLevel) ? true : false, UITool.f_GetSysOpenLevel(EM_NeedLevel.RebelArmyLevel));
        SetIsLock(f_GetObject("BtnCrossServerBattle"), playerLevel < UITool.f_GetSysOpenLevel(EM_NeedLevel.CrossServerBattle) ? true : false, UITool.f_GetSysOpenLevel(EM_NeedLevel.CrossServerBattle));
        //TsuCode - ChaosBattle
        SetIsLock(f_GetObject("BtnChaosBattle"), playerLevel < UITool.f_GetSysOpenLevel(EM_NeedLevel.ChaosBattle) ? true : false, UITool.f_GetSysOpenLevel(EM_NeedLevel.ChaosBattle));
        //
    }
    /// <summary>
    /// 设置UI
    /// </summary>
    /// <param name="item">按钮item</param>
    /// <param name="isLock">是否解锁</param>
    /// <param name="lockLevel">解锁等级</param>
    private void SetIsLock(GameObject item, bool isLock, int openLevel)
    {
        Color grayColor = new Color(0, 0.2f, 0.2f, 1);
        UITool.f_SetTextureGray(item.GetComponent<UITexture>(), isLock);
        //item.GetComponent<UISprite>().color = isLock ? grayColor : Color.white;
        item.transform.Find("ShopIcon").GetComponent<UITexture>().color = isLock ? grayColor : Color.white;
        GameObject SpriteTitle = item.transform.Find("SpriteTitle").gameObject;
        if (SpriteTitle.GetComponent<UISprite>() != null)
            SpriteTitle.GetComponent<UISprite>().color = isLock ? grayColor : Color.white;
        else
            SpriteTitle.GetComponent<UILabel>().color = isLock ? new Color(1, 1, 1, 0.72f) : new Color(1, 1, 0.56f, 1);
        //item.transform.FindChild("Lock").gameObject.SetActive(isLock);
        //item.transform.FindChild("Lock/SpriteLock/LabelLock").GetComponent<UILabel>().text = openLevel + "级可以开启";
    }
    /// <summary>
    /// 初始化消息
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BlackBG", OnBtnCloseClick);
        f_RegClickEvent("BtnClose", OnBtnCloseClick);
        f_RegClickEvent("BtnCardShop", OnBtnCardShopClick);
        f_RegClickEvent("BtnAwakening", OnBtnAwakeningClick);
        f_RegClickEvent("BtnReputation", OnBtnReputationClick);
        f_RegClickEvent("BtnLegion", OnBtnLegionClick);
        f_RegClickEvent("BtnEquip", OnBtnEquipClick);
        f_RegClickEvent("BtnBattleFeat", OnBtnBattleFeatClick);
        f_RegClickEvent("BtnCrossServerBattle", OnBtnCrossServerBattleClick);
        //TsuCode - ChaosBattle
        f_RegClickEvent("BtnChaosBattle", OnBtnChaosBattleClick);
        //
        f_RegClickEvent("BtnPropsShop", OnBtnPropsShopClick);
    }
    /// <summary>
    /// 点击返回按钮
    /// </summary>
    private void OnBtnCloseClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShopStorePage, UIMessageDef.UI_CLOSE);
    }
    /// <summary>
    /// 点击神将商店
    /// </summary>
    private void OnBtnCardShopClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShopCommonPage, UIMessageDef.UI_OPEN, EM_ShopType.Card);
    }
    private void OnBtnPropsShopClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShopPropsPage, UIMessageDef.UI_OPEN);
    }
    /// <summary>
    /// 点击领悟商店
    /// </summary>
    private void OnBtnAwakeningClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        if (playerLevel >= UITool.f_GetSysOpenLevel(EM_NeedLevel.AwakeShopLevel))
        {
            ccUIHoldPool.GetInstance().f_Hold(this);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.ShopCommonPage, UIMessageDef.UI_OPEN, EM_ShopType.Awake);
        }
        else
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, string.Format(CommonTools.f_GetTransLanguage(1210), UITool.f_GetSysOpenLevel(EM_NeedLevel.AwakeShopLevel)));
        }
    }
    /// <summary>
    /// 点击声望商店
    /// </summary>
    public void OnBtnReputationClick(GameObject go, object obj1, object obj2)
    {
        if (playerLevel >= UITool.f_GetSysOpenLevel(EM_NeedLevel.ArenaLevel))
        {
            ccUIHoldPool.GetInstance().f_Hold(this);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.ShopMutiCommonPage, UIMessageDef.UI_OPEN, EM_ShopMutiType.Reputation);
        }
        else
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, string.Format(CommonTools.f_GetTransLanguage(1211), UITool.f_GetSysOpenLevel(EM_NeedLevel.ArenaLevel)));
        }
    }

    /// <summary>
    /// 点击神装商店
    /// </summary>
    public void OnBtnEquipClick(GameObject go, object obj1, object obj2)
    {
        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < UITool.f_GetSysOpenLevel(EM_NeedLevel.RunningManLvel))
        {
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1212), UITool.f_GetSysOpenLevel(EM_NeedLevel.RunningManLvel)));
            return;
        }
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShopMutiCommonPage, UIMessageDef.UI_OPEN, EM_ShopMutiType.RunningMan);
    }

    public void OnBtnCrossServerBattleClick(GameObject go, object value1, object value2)
    {
        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < UITool.f_GetSysOpenLevel(EM_NeedLevel.CrossServerBattle))
        {
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1213), UITool.f_GetSysOpenLevel(EM_NeedLevel.CrossServerBattle)));
            return;
        }
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShopMutiCommonPage, UIMessageDef.UI_OPEN, EM_ShopMutiType.CrossServerBattle);
    }

    /// <summary>
    /// 点击军团商店
    /// </summary>
    public void OnBtnLegionClick(GameObject go, object obj1, object obj2)
    {
        if (playerLevel >= UITool.f_GetSysOpenLevel(EM_NeedLevel.LegionLevel))
        {
            if (LegionMain.GetInstance().m_LegionInfor.m_iLegionId == 0)
            {
                ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1214));
            }
            else
            {
                UITool.f_OpenOrCloseWaitTip(true);
                LegionMain.GetInstance().m_LegionInfor.f_ExecuteAfterLegionInfo(true, true, f_Callback_LegionSelfInfo);
            }
        }
        else
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, string.Format(CommonTools.f_GetTransLanguage(1215), UITool.f_GetSysOpenLevel(EM_NeedLevel.LegionLevel)));
        }
    }
    /// <summary>
    /// 成功请求完自己军团信息
    /// </summary>
    /// <param name="result"></param>
    private void f_Callback_LegionSelfInfo(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            ccUIHoldPool.GetInstance().f_Hold(this);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.ShopMutiCommonPage, UIMessageDef.UI_OPEN, EM_ShopMutiType.Legion);
        }
        else
        {
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(1216) + result);
        }
    }
    /// <summary>
    /// 点击战功商店
    /// </summary>
    public void OnBtnBattleFeatClick(GameObject go, object obj1, object obj2)
    {
        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < UITool.f_GetSysOpenLevel(EM_NeedLevel.RebelArmyLevel))
        {
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1217), UITool.f_GetSysOpenLevel(EM_NeedLevel.RebelArmyLevel)));
            return;
        }
        else
        {
            ccUIHoldPool.GetInstance().f_Hold(this);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.ShopMutiCommonPage, UIMessageDef.UI_OPEN, EM_ShopMutiType.BattleFeatShop);
        }
    }
    //TsuCode - ChaosBattle
    public void OnBtnChaosBattleClick(GameObject go, object value1, object value2)
    {
        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < UITool.f_GetSysOpenLevel(EM_NeedLevel.ChaosBattle))
        {
UITool.Ui_Trip(string.Format("Lv.{0} opens Test Market", UITool.f_GetSysOpenLevel(EM_NeedLevel.ChaosBattle)));
            return;
        }
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShopMutiCommonPage, UIMessageDef.UI_OPEN, EM_ShopMutiType.ChaosBattle);
    }
    //
}
