using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 夺宝结算页面
/// </summary>
public class GrabTreasureFinishPage : UIFramwork {
readonly string[] StarDescArr = new string[4] {"Rate： 0 stars（failure）",
                                                   "Đánh giá： 1 sao（Nhiều tướng tử trận）",
                                                   "Đánh giá： 2 sao（Một tướng tử trận）",
                                                   "Đánh giá： 3 sao（Không tướng tử trận）" };

    private GameObject mWinPage;
    private GameObject mLosePage;
    private Transform[] mStar;
    private UILabel mStarDesc;
    private UILabel mMoneyLabel;
    private UILabel mExpLabel;
    private UILabel mLvLabel;
    private UISlider mLvSlider;

    //特效
    private Transform mWinTitleParent;
    private Transform mExpEffectParent;

    //关卡掉落显示
    private UIGrid _awardGrid;
    private GameObject _awardItem;
    private ResourceCommonItemComponent _awardShowComponent;
    private ResourceCommonItemComponent mAwardShowComponent
    {
        get
        {
            if (_awardShowComponent == null)
                _awardShowComponent = new ResourceCommonItemComponent(_awardGrid, _awardItem);
            return _awardShowComponent;
        }
    }

    private GrabTreasureFinishParam finishParam;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mWinPage = f_GetObject("WinPage");
        mLosePage = f_GetObject("LosePage");
        mStar = new Transform[GameParamConst.StarNumPreTollgate];
        for (int i = 0; i < GameParamConst.StarNumPreTollgate; i++)
        {
            mStar[i] = f_GetObject(string.Format("Star{0}", i)).GetComponent<UISprite>().transform;
        }
        mStarDesc = f_GetObject("StarDesc").GetComponent<UILabel>();
        mMoneyLabel = f_GetObject("MoneyLabel").GetComponent<UILabel>();
        mExpLabel = f_GetObject("ExpLabel").GetComponent<UILabel>();
        mLvLabel = f_GetObject("LvLabel").GetComponent<UILabel>();
        mLvSlider = f_GetObject("LvSlider").GetComponent<UISlider>();
        _awardGrid = f_GetObject("AwardGrid").GetComponent<UIGrid>();
        _awardItem = f_GetObject("IconAndNumItem");
        mWinTitleParent = f_GetObject("WinTitleParent").transform;
        mExpEffectParent = f_GetObject("ExpEffectParent").transform;
        f_RegClickEvent("MaskClose", ReturnBtnHandle);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        finishParam = (GrabTreasureFinishParam)e;
        mWinPage.SetActive(finishParam.StarNum > 0);
        mLosePage.SetActive(finishParam.StarNum == 0);
        mStarDesc.text = StarDescArr[finishParam.StarNum];

        mMoneyLabel.text = GameFormula.f_VigorCost2Money(Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level), finishParam.VigorCost).ToString();
        int addExp;
        int exp = GameFormula.f_VigorCost2Exp(Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level), finishParam.VigorCost, out addExp);
        string strAddExp = addExp > 0 ? "[FFF700FF]（+" + addExp + "）" : "";
        mExpLabel.text = exp.ToString() + strAddExp;
mLvLabel.text = string.Format("Level {0}", Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level));
        mLvSlider.value = Data_Pool.m_UserData.mExpPercent;

        bool isGetFrag = finishParam.TreaFragId > 0;
        f_GetObject("GetFrameHintLabel").SetActive(isGetFrag);
        f_GetObject("NoGetFrameHintLabel").SetActive(!isGetFrag);
        if (finishParam.TreaFragId > 0)
        {
            List<AwardPoolDT> listAward = new List<AwardPoolDT>();
            AwardPoolDT dt = new AwardPoolDT();
            dt.f_UpdateByInfo((byte)EM_ResourceType.TreasureFragment, finishParam.TreaFragId, 1);
            listAward.Add(dt);
            mAwardShowComponent.f_Show(listAward);//Data_Pool.m_AwardPool.f_GetAwardPoolDTByType(info.mAwardSource);
            string name = dt.mTemplate.mName;
            UITool.f_GetImporentColorName(dt.mTemplate.mImportant, ref name);
f_GetObject("GetFrameHintLabel").GetComponent<UILabel>().text = "[f8be40]Assured success，received"+ name;
            Data_Pool.m_GrabTreasurePool.m_IsNeedChangeToRobotPage = false;
        }
        //没有得到碎片
        else
        {
            mAwardShowComponent.f_Show(new System.Collections.Generic.List<AwardPoolDT>());
            Data_Pool.m_GrabTreasurePool.m_IsNeedChangeToRobotPage = true;
        }
        if (finishParam.StarNum > 0)
        {
            glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.BattleVictory, false);
            UITool.f_CreateEffect(UIEffectName.UIEffectAddress2, UIEffectName.WinTitleEffect, mWinTitleParent);
            int delayIdx = 1;
            for (int i = 0; i < finishParam.StarNum; i++)
            {
                ccTimeEvent.GetInstance().f_RegEvent(0.2f * (delayIdx++), false, mStar[i], f_ShowStarEffect);
            }
            ccTimeEvent.GetInstance().f_RegEvent(0.2f * delayIdx, false, mExpEffectParent, f_ShowExpEffect);
            ccTimeEvent.GetInstance().f_RegEvent(0.2f * delayIdx, false, null, f_ShowGoodEffect);
        }
        else {
            glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.BattleFail, false);
        }
    }
    private void f_ShowStarEffect(object value)
    {
        Transform tf = (Transform)value;
        UITool.f_CreateEffect(UIEffectName.UIEffectAddress2, UIEffectName.WinStarEffect, tf);
    }

    private void f_ShowExpEffect(object value)
    {
        Transform tf = (Transform)value;
        UITool.f_CreateEffect(UIEffectName.UIEffectAddress2, UIEffectName.WinExpEffect, tf);
    }

    private void f_ShowGoodEffect(object value)
    {
        mAwardShowComponent.f_ShowEffect();
    }
    /// <summary>
    /// 点击返回界面
    /// </summary>
    private void ReturnBtnHandle(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GrabTreasureFinishPage, UIMessageDef.UI_CLOSE);
        if (finishParam.StarNum > 0)//战斗胜利有抽牌的过程
        {
            ChooseAwardParam param = new ChooseAwardParam();
            List<AwardPoolDT> listData = Data_Pool.m_AwardPool.f_GetAwardPoolDTByAwardId(Data_Pool.m_GrabTreasurePool.m_GrabTreasurePoolDT.awardID);
            List<ResourceCommonDT> listCommonData = new List<ResourceCommonDT>();
            for (int i = 0; i < listData.Count; i++)
            {
                listCommonData.Add(listData[i].mTemplate);
            }
            byte resourceType = Data_Pool.m_GrabTreasurePool.m_GrabTreasurePoolDT.resourceType;
            int resourceId = Data_Pool.m_GrabTreasurePool.m_GrabTreasurePoolDT.resourceId;
            int resourceNum = Data_Pool.m_GrabTreasurePool.m_GrabTreasurePoolDT.resourceNum;
            param.mListData = listCommonData;
            param.mChooseIndex = Data_Pool.m_AwardPool.f_GetIndexOfAwardPoolDTByAwardId(listData, resourceType, resourceId, resourceNum);
            param.mOnReturnCallback = OnChooseAwardCallback;
            ccUIHoldPool.GetInstance().f_Hold(this);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.ChooseAwardPage, UIMessageDef.UI_OPEN, param);
        }
        else
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.GameMain);
        }
    }
    /// <summary>
    /// 抽牌回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnChooseAwardCallback(object obj)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.GameMain);
    }
}
/// <summary>
/// 夺宝结算页面参数
/// </summary>
public class GrabTreasureFinishParam
{
    public int StarNum;
    public int TreaFragId;//法宝碎片，0表示没有得到碎片
    public int VigorCost;
}
