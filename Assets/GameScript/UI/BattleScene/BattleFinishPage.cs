using System;
using ccU3DEngine;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;

/// <summary>
/// 战斗结算显示
/// </summary>
public class BattleFinishPage : UIFramwork
{
    string[] StarDescArr = null;

    private GameObject mWinPage;
    private GameObject mLosePage;
    private UISprite[] mStar;
    private UILabel mStarDesc;
    private UILabel mMoneyLabel;
    private UILabel mExpLabel;
    private UILabel mLvLabel;
    private UISlider mLvSlider;
    private Transform mBottomLine;

    // 模型
    private Transform mRoleParent;
    private GameObject mRole;

    //特效
    private Transform mWinTitleParent;
    private Transform mExpEffectParent;

    //关卡掉落显示
    private UIGrid _awardGrid;
    private GameObject _awardItem;
    private ResourceCommonItemComponent _awardShowComponent;

    private int Time_ExpTween;
    private int Time_Star;
    private int Time_Muisc;
    private int Time_Effect;
    private int Time_Goods;

    public List<GameObject> mSucTweenList = new List<GameObject>();
    public List<GameObject> mFailTweenList = new List<GameObject>();

    private bool m_isAniDone = false;

    private bool m_IsOpen = false;
    private ResourceCommonItemComponent mAwardShowComponent
    {
        get
        {
            if (_awardShowComponent == null)
                _awardShowComponent = new ResourceCommonItemComponent(_awardGrid, _awardItem);
            return _awardShowComponent;
        }
    }

    private BattleFinishParam battleFinishParam;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        StarDescArr = new string[4] {
            CommonTools.f_GetTransLanguage(1076),
            CommonTools.f_GetTransLanguage(1077),
            CommonTools.f_GetTransLanguage(1078),
            CommonTools.f_GetTransLanguage(1079)};
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mWinPage = f_GetObject("WinPage");
        mLosePage = f_GetObject("LosePage");
        mStar = new UISprite[GameParamConst.StarNumPreTollgate];
        for (int i = 0; i < GameParamConst.StarNumPreTollgate; i++)
        {
            mStar[i] = f_GetObject(string.Format("Star{0}", i)).GetComponent<UISprite>();
        }
        
        mStarDesc = f_GetObject("StarDesc").GetComponent<UILabel>();
        mMoneyLabel = f_GetObject("MoneyLabel").GetComponent<UILabel>();
        mExpLabel = f_GetObject("ExpLabel").GetComponent<UILabel>();
        mLvLabel = f_GetObject("LvLabel").GetComponent<UILabel>();
        mLvSlider = f_GetObject("LvSlider").GetComponent<UISlider>();
        _awardGrid = f_GetObject("AwardGrid").GetComponent<UIGrid>();
        _awardItem = f_GetObject("IconAndNumItem");
        mRoleParent = f_GetObject("RoleParent").transform;
        mBottomLine = f_GetObject("Bottom").transform;
        //mWinTitleParent = f_GetObject("WinTitleParent").transform;
        mExpEffectParent = f_GetObject("ExpEffectParent").transform;
        f_RegClickEvent("MaskClose", ReturnBtnHandle);
        f_RegClickEvent("BtnGetMoreCard", f_BattleLoseProcess, EM_BattleLoseProcess.GetMoreCard);
        f_RegClickEvent("BtnCardIntensify", f_BattleLoseProcess, EM_BattleLoseProcess.CardIntensify);
        f_RegClickEvent("BtnEquipIntensify", f_BattleLoseProcess, EM_BattleLoseProcess.EquipIntensify);
        f_RegClickEvent("BtnLineupChange", f_BattleLoseProcess, EM_BattleLoseProcess.LineupChange);
        f_RegClickEvent("BattleData",f_OpenBattleDataPage);
    }

    private int titleEventId = 0;
    private int timeEventId = 0;
    private List<int> propTimeEventId = new List<int>();
    private int[] starTimeEventId = new[] {0, 0, 0};
    private int winEventId = 0;
    //显示战斗结算特效
    private void showEffect(BattleFinishParam param)
    {
        bool isShowStar = StaticValue.m_CurBattleConfig.m_eBattleType != EM_Fight_Enum.eFight_Legend;
		////Tu day
		
        if (param.StarNum <= 0)
        {
            GameObject failMagic = null;
            Transform failMagicParent = mLosePage.transform.Find("LoseTexture");
            // UITool.f_CreateMagicById((int)EM_MagicId.eFightFinishFail, ref failMagic, failMagicParent, 501, "Open", null, false, 100);
			// UITool.f_CreateMagicById((int)EM_MagicId.eFightFinishFail, ref failMagic, failMagicParent, 501, "a_zhandoushibai", null, false, 100);
            if (failMagic != null)
            {              
                // failMagic.GetComponent<SkeletonAnimation>().state.AddAnimation(0, "Loop", true, 0);
				failMagic.GetComponent<SkeletonAnimation>().state.AddAnimation(0, "a_zhandoushibai_loop", true, 0);
            }
            titleEventId = ccTimeEvent.GetInstance().f_RegEvent(0.3f, false, null, delegate (object value)
            {
                if(!m_IsOpen || !isShowStar) return;
                for (int i = 0; i < mStar.Length; i++)
                {
                    starTimeEventId[i] = ccTimeEvent.GetInstance().f_RegEvent(0.2f * (i + 1), false, mStar[i].transform, delegate (object data)
                    {
                        if (!m_IsOpen) return;
                        GameObject star = null;
                        Transform parent = data as Transform;
                        string aniName = "shibaixingxing";
                        UITool.f_CreateMagicById((int)EM_MagicId.eFightFinishStar, ref star, parent, 501, aniName, null, false, 100);
                        if (star != null)
                        {                          
                            star.GetComponent<SkeletonAnimation>().state.AddAnimation(0, aniName + "_loop", true, 0);
                        }

                    });
                }
            });
            for (int i = 0; i < mFailTweenList.Count; i++)
            {
                if (!m_IsOpen) return;
                mFailTweenList[i].SetActive(false);
            }

            timeEventId = ccTimeEvent.GetInstance().f_RegEvent(0.75f, false, null, delegate (object value)
            {
                if (!m_IsOpen) return;
                for (int i = 0; i < mFailTweenList.Count; i++)
                {
                    int id = ccTimeEvent.GetInstance().f_RegEvent(0.15f * (i + 1), false, mFailTweenList[i], delegate(object data)
                        {
                            if (!m_IsOpen) return;
                            GameObject go = data as GameObject;
                            go.SetActive(true);
                            m_isAniDone = true;
                            Time.timeScale = 1;
                        });
                    propTimeEventId.Add(id);
                }
            });

            return;
        }
        for (int i = 0; i < _awardGrid.transform.childCount; i++)
        {
            mSucTweenList.Add(_awardGrid.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < mSucTweenList.Count; i++)
        {
            mSucTweenList[i].SetActive(false);
        }
        GameObject winMagic = null;
        Transform winMagicParent = mWinPage.transform.Find("Texture");
        // UITool.f_CreateMagicById((int)EM_MagicId.eFightFinishSuc, ref winMagic, winMagicParent,501, "Open",null, false, 100f);
		// UITool.f_CreateMagicById((int)EM_MagicId.eFightFinishSuc, ref winMagic, winMagicParent,501, "a_zhandoushengli",null, false, 100f);
        if (winMagic != null)
        {
            // winMagic.GetComponent<SkeletonAnimation>().state.AddAnimation(0, "Loop", true, 0);
			winMagic.GetComponent<SkeletonAnimation>().state.AddAnimation(0, "a_zhandoushengli_loop", true, 0);
        }

        titleEventId = ccTimeEvent.GetInstance().f_RegEvent(0.3f, false, null, delegate (object value)
        {
            if (!m_IsOpen || !isShowStar) return;
            for (int i = 0; i < mStar.Length; i++)
            {
                starTimeEventId[i] = ccTimeEvent.GetInstance().f_RegEvent(0.2f * (i+1), false, mStar[i].transform, delegate(object data)
                {
                    if (!m_IsOpen) return;
                    GameObject star = null;
                    Transform parent = data as Transform;
                    if (null == parent)
                    {
                        ccTimeEvent.GetInstance().f_UnRegEvent(starTimeEventId[i]);
                        return;
                    }
                    int index = int.Parse(parent.name.Replace("Star", ""));
                    string aniName = param.StarNum > index ? "shenglixingxing" : "shibaixingxing";
                    UITool.f_CreateMagicById((int)EM_MagicId.eFightFinishStar, ref star, parent, 501, aniName, null, false, 100f);
                    if (star != null)
                    {
                        star.GetComponent<SkeletonAnimation>().state.AddAnimation(0, aniName + "_loop", true, 0);
                    }
                });
            }
        });
		
		////Toi day
		
        //1秒后物品出现,每0.15秒出现一个物品
        timeEventId = ccTimeEvent.GetInstance().f_RegEvent(0.75f, false, null, delegate(object value)
        {
            if (!m_IsOpen) return;
            for (int i = 0; i < mSucTweenList.Count; i++)
            {
                int id = ccTimeEvent.GetInstance().f_RegEvent(0.15f * (i + 1), false, mSucTweenList[i], delegate (object data)
                {
                    if (!m_IsOpen) return;
                    GameObject go = data as GameObject;
                    if (go.name == "StarDesc" && !isShowStar)
                    {
                        return;
                    }
                    go.SetActive(true);
                    mAwardShowComponent.f_ShowEffect();
                    if (go.name == "TopAward")
                    {
                        int maxMoney = GameFormula.f_EnergyCost2Money(StaticValue.m_sLvInfo.m_iLv,
                            battleFinishParam.EnergyCost);
                        double curMoney = 0;
                        int addExp;
                        int maxExp = GameFormula.f_EnergyCost2Exp(StaticValue.m_sLvInfo.m_iLv,
                            battleFinishParam.EnergyCost, out addExp);
                        mExpLabel.transform.parent.gameObject.SetActive(maxExp > 0);
                        float curExp = 0;
                        float curSlider = maxExp > 0 ? 0 : Data_Pool.m_UserData.mExpPercent; //mLvSlider.value;
                        int winEventId = 0;
                        winEventId = ccTimeEvent.GetInstance().f_RegEvent(0.02f, true, null,
                            delegate(object cbData)
                            {
                                if (!m_IsOpen || ((curExp >= maxExp || maxExp <= 0) && curMoney >= maxMoney && curSlider >= Data_Pool.m_UserData.mExpPercent))
                                {
                                    if (winEventId > 0)
                                    {
                                        ccTimeEvent.GetInstance().f_UnRegEvent(winEventId);
                                        m_isAniDone = true;
                                        Time.timeScale = 1;
                                    }
                                    return;
                                }
                                curExp += maxExp * 0.05f;
                                curMoney += Math.Round(maxMoney * 0.05f);
                                mMoneyLabel.text = Math.Min((int)curMoney, maxMoney).ToString();
                                mExpLabel.text = Math.Min((int)curExp, maxExp).ToString();
                                curSlider += Data_Pool.m_UserData.mExpPercent * 0.05f;
                                mLvSlider.value = Math.Min(curSlider, Data_Pool.m_UserData.mExpPercent);
                            });
                    }
                });
                propTimeEventId.Add(id);
            }
        });
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        m_IsOpen = false;
        ccTimeEvent.GetInstance().f_UnRegEvent(timeEventId);
        ccTimeEvent.GetInstance().f_UnRegEvent(titleEventId);
        ccTimeEvent.GetInstance().f_UnRegEvent(winEventId);
        for (int i = 0; i < propTimeEventId.Count; i++)
        {
            ccTimeEvent.GetInstance().f_UnRegEvent(propTimeEventId[i]);
        }
        for (int i = 0; i < starTimeEventId.Length; i++)
        {
            ccTimeEvent.GetInstance().f_UnRegEvent(starTimeEventId[i]);
        }
        Time.timeScale = 1;
        m_isAniDone = false;
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        m_IsOpen = true;
        if (mSucTweenList.Count > 5)
        {
            mSucTweenList.RemoveRange(5, mSucTweenList.Count-6);
        }
        battleFinishParam = (BattleFinishParam)e;
        mWinPage.SetActive(battleFinishParam.StarNum > 0);
        mLosePage.SetActive(battleFinishParam.StarNum == 0);
        Data_Pool.m_GuidancePool.DungeonResult = battleFinishParam.StarNum;
        Data_Pool.m_GuidancePool.f_ChangeGuidanceType(EM_GuidanceType.DungeonResult);
        mStarDesc.text = StarDescArr[battleFinishParam.StarNum];
        bool isShowStar = StaticValue.m_CurBattleConfig.m_eBattleType != EM_Fight_Enum.eFight_Legend;
        mStarDesc.gameObject.SetActive(isShowStar);
        mStar[0].transform.parent.gameObject.SetActive(isShowStar);
        if(isShowStar)
        UITool.f_UpdateStarNum(mStar, battleFinishParam.StarNum, "Icon_RMStar", "Icon_RMStar2",0,false);
        mLvLabel.text = string.Format(CommonTools.f_GetTransLanguage(1080), Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level));
        //mLvSlider.value = Data_Pool.m_DungeonPool.m_DungeonStartExp;
        //Time_ExpTween = ccTimeEvent.GetInstance().f_RegEvent(TweenSeed, true, null, _PlayTween);
        
        if (battleFinishParam.StarNum > 0)
        {
            mAwardShowComponent.f_Show(battleFinishParam.listAward);//Data_Pool.m_AwardPool.f_GetAwardPoolDTByType(info.mAwardSource);
            glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.BattleVictory, false);                                                   //失败显示空

            if (battleFinishParam.listAward.Count != 0)
            {
                f_GetObject("AwardTitle").SetActive(true);
                mBottomLine.localPosition = new Vector3(0, -110, 0);
            }
            else
            {

                f_GetObject("AwardTitle").SetActive(false);
                mBottomLine.localPosition = Vector3.zero;
            }
        }
        else
        {
            f_GetObject("AwardTitle").SetActive(false);
            mBottomLine.localPosition = Vector3.zero;
            mAwardShowComponent.f_Show(new System.Collections.Generic.List<AwardPoolDT>());
            glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.BattleFail, false);
        }

        if (battleFinishParam.StarNum > 0)
        {
            mMoneyLabel.text = GameFormula.f_EnergyCost2Money(StaticValue.m_sLvInfo.m_iLv, battleFinishParam.EnergyCost).ToString();
            int addExp;
            int exp = GameFormula.f_EnergyCost2Exp(StaticValue.m_sLvInfo.m_iLv, battleFinishParam.EnergyCost, out addExp);
            string strAddExp = addExp > 0 ? "[FFF700FF]（+" + addExp + "）" : "";
            mExpLabel.text = exp.ToString() + strAddExp;
            //UITool.f_CreateEffect(UIEffectName.UIEffectAddress2, UIEffectName.WinTitleEffect, mWinTitleParent);
            //int delayIdx = 1;
            //for (int i = 0; i < battleFinishParam.StarNum; i++)
            //{
            //Time_Star = ccTimeEvent.GetInstance().f_RegEvent(0.2f * (delayIdx++), false, mStar[i], f_ShowStarEffect);
            //Time_Muisc = ccTimeEvent.GetInstance().f_RegEvent(0.2f * (delayIdx++), false, 13 + i, f_PlayAudio);
            //}

            //Time_Effect = ccTimeEvent.GetInstance().f_RegEvent(0.2f * delayIdx, false, mExpEffectParent, f_ShowExpEffect);
            //Time_Goods = ccTimeEvent.GetInstance().f_RegEvent(0.2f * delayIdx, false, null, f_ShowGoodEffect);
        }
        else
        {
            mMoneyLabel.text = "0";
            mExpLabel.text = "0";
        }
        _LoadRole();
        showEffect(battleFinishParam);
    }
    private void f_PlayAudio(object obj)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioEffect((AudioEffectType)obj);
    }

    private void f_ShowStarEffect(object value)
    {

        Transform tf = (Transform)value;
        UITool.f_CreateEffect(UIEffectName.UIEffectAddress2, UIEffectName.WinStarEffect, tf);
    }

    private void f_ShowExpEffect(object value)
    {
        Transform tf = (Transform)value;
        UITool.f_CreateEffect(UIEffectName.UIEffectAddress2, UIEffectName.WinExpEffect, tf, 0f, 0f, false, 1f, 1.5f);
    }

    private void f_ShowGoodEffect(object value)
    {
        mAwardShowComponent.f_ShowEffect();
    }

    private void ReturnBtnHandle(GameObject go, object value1, object value2)
    {
        if (!m_isAniDone)
        {
            Time.timeScale = 3;
            return;
        }
        //ccTimeEvent.GetInstance().f_UnRegEvent(Time_ExpTween);
        if (battleFinishParam.NeedShowFirstWin)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.DungeonFirstWinPage, UIMessageDef.UI_OPEN);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.BattleFinishPage, UIMessageDef.UI_CLOSE);
        }
        else
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.BattleFinishPage, UIMessageDef.UI_CLOSE);
            //ccTimeEvent.GetInstance().f_UnRegEvent(Time_Effect);
            //ccTimeEvent.GetInstance().f_UnRegEvent(Time_Goods);
            //cTimeEvent.GetInstance().f_UnRegEvent(Time_Muisc);
            //cTimeEvent.GetInstance().f_UnRegEvent(Time_Star);

            ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.GameMain);
        }
        if (mRole != null)
        {
            UITool.f_DestoryStatelObject(mRole);
        }
    }

    private void f_BattleLoseProcess(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.BattleFinishPage, UIMessageDef.UI_CLOSE);
        UITool.f_ProcessBattleLose((EM_BattleLoseProcess)value1);
    }
    private float TweenSeed = 0.005f;
    private void _PlayTween(object obj)
    {
        mLvSlider.value += TweenSeed;
        if (mLvSlider.value >= Data_Pool.m_UserData.mExpPercent)
        {
            //ccTimeEvent.GetInstance().f_UnRegEvent(Time_ExpTween);
            mLvSlider.value = Data_Pool.m_UserData.mExpPercent;
        }
        
    }

    private void _LoadRole()
    {
        if (mRole == null)
        {
            UITool.f_CreateRoleByCardId(Data_Pool.m_CardPool.mRolePoolDt.m_iTempleteId, ref mRole, mRoleParent, 501);
            Transform RoleTran = mRole.transform;
            RoleTran.localScale = Vector3.one * 150;
            RoleTran.localPosition = new Vector3(0,-180,0);
        }
    }

    private void f_OpenBattleDataPage(GameObject go, object value1, object value2) {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.BattleDataPag, UIMessageDef.UI_OPEN);
    }
}
public class BattleFinishParam
{
    public int StarNum;
    public bool NeedShowFirstWin;
    public List<AwardPoolDT> listAward;
    public int EnergyCost;
}
