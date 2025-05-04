using ccU3DEngine;
using UnityEngine;
using Spine.Unity;
using System.Collections.Generic;

public class ArenaFinishPageNew : UIFramwork
{
    private GameObject mMaskClose;

    private GameObject mWinObj;
    private GameObject mLoseObj;
    private UITexture mResultTexture;

    //名次变灰
    private UILabel mRankOld;
    private UILabel mRankCur;
    private GameObject mRankTable;
    private GameObject mUpTip;

    //声望、银币、经验奖励
    private Transform mAwardParent;
    private UILabel mlabelMoney;
    private UILabel mlabelPrestige;
    private UILabel mlabelExp;

    //挑战双方信息
    private UILabel mlabelMyName;
    private UILabel mlabelMyFightPower;
    private UILabel mlabelMyLevel;
    private UI2DSprite miconMy;

    private UILabel mlabelEnemyName;
    private UILabel mlabelEnemyFightPower;
    private UILabel mlabelEnemyLevel;
    private UI2DSprite miconEnemy;


    private CMsg_SC_ArenaRet mCurResult;

    // 模型
    private Transform mRoleParent;
    private GameObject mRole;

    //特效
    private Transform mWinTitleParent;
    GameObject resultMagic = null;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mMaskClose = f_GetObject("MaskClose");
        mWinObj = f_GetObject("WinPage");
        mLoseObj = f_GetObject("LosePage");
        //mResultTexture = f_GetObject("ResultTexture").GetComponent<UITexture>();
        //mResultTexture.enabled = false;

        //名次变灰
        mRankOld = f_GetObject("RankOld").GetComponent<UILabel>();
        mRankCur = f_GetObject("RankCur").GetComponent<UILabel>();
        mRankTable = f_GetObject("RankTable");
        mUpTip = f_GetObject("UpTip");

        //声望、银币、经验奖励
        mAwardParent = f_GetObject("AwardParent").transform;
        mlabelMoney = f_GetObject("MoneyLabel").GetComponent<UILabel>();
        mlabelPrestige = f_GetObject("PrestigeLabel").GetComponent<UILabel>();
        mlabelExp = f_GetObject("ExpLabel").GetComponent<UILabel>();

        //挑战双方信息
        mlabelMyName = f_GetObject("Label_MyName").GetComponent<UILabel>();
        mlabelMyFightPower = f_GetObject("Label_MyFightPower").GetComponent<UILabel>();
        mlabelMyLevel = f_GetObject("Label_MyLevel").GetComponent<UILabel>();
        miconMy = f_GetObject("Icon_My").GetComponent<UI2DSprite>();

        mlabelEnemyName = f_GetObject("Label_EnemyName").GetComponent<UILabel>();
        mlabelEnemyFightPower = f_GetObject("Label_EnemyFightPower").GetComponent<UILabel>();
        mlabelEnemyLevel = f_GetObject("Label_EnemyLevel").GetComponent<UILabel>();
        miconEnemy = f_GetObject("Icon_Enemy").GetComponent<UI2DSprite>();


        //特效
        mWinTitleParent = f_GetObject("WinTitleParent").transform;

        //模型
        //mRoleParent = f_GetObject("RoleParent").transform;

        //监听事件
        f_RegClickEvent(mMaskClose, f_MaskClose);
        f_RegClickEvent("BattleData", f_OpenBattleDataPage);
        f_RegClickEvent("BtnGetMoreCard", f_BattleLoseProcess, EM_BattleLoseProcess.GetMoreCard);
        f_RegClickEvent("BtnCardIntensify", f_BattleLoseProcess, EM_BattleLoseProcess.CardIntensify);
        f_RegClickEvent("BtnEquipIntensify", f_BattleLoseProcess, EM_BattleLoseProcess.EquipIntensify);
        f_RegClickEvent("BtnLineupChange", f_BattleLoseProcess, EM_BattleLoseProcess.LineupChange);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        mCurResult = Data_Pool.m_ArenaPool.m_ArenaRet;
        mWinObj.SetActive(mCurResult.isWin != (int)EM_ArenaResult.Lose);
        mLoseObj.SetActive(mCurResult.isWin == (int)EM_ArenaResult.Lose);

        //加载模型
        //if (mRole == null)
        //{
        //    UITool.f_CreateRoleByCardId(Data_Pool.m_CardPool.mRolePoolDt.m_iTempleteId, ref mRole, mRoleParent, 501);
        //    Transform RoleTran = mRole.transform;
        //    RoleTran.localScale = Vector3.one * 150;
        //    RoleTran.localPosition = new Vector3(0, -180, 0);
        //}

        //设置奖励物品（银币、经验、声望）
        //Vector3 awardPos = mAwardParent.localPosition;
        //awardPos.y = mCurResult.isWin == (int)EM_ArenaResult.Lose ? -196  : - 280;
        //mAwardParent.localPosition = awardPos;
        int lv = StaticValue.m_sLvInfo.m_iAddLv;
        int moneyNum = GameFormula.f_VigorCost2Money(lv, GameParamConst.ArenaVigorCost);
        mlabelMoney.text = moneyNum.ToString();
        int addExp;
        int expNum = GameFormula.f_VigorCost2Exp(lv, GameParamConst.ArenaVigorCost, out addExp);
        string strAddExp = addExp > 0 ? " [8dd91b]+" + addExp + CommonTools.f_GetTransLanguage(2186) : "";
        mlabelExp.text = "[cbc4b1]" + expNum + strAddExp;
        int frame = mCurResult.isWin == (int)EM_ArenaResult.Lose ? GameParamConst.ArenaLoseFame : GameParamConst.ArenaWinFame;
        mlabelPrestige.text = frame.ToString();

        //设置挑战双方信息
        string enemyName = PlayerPrefs.GetString(ArenaPageNew.ArenaFakePlayerNameKey);
        if (enemyName != "")
        {
            mlabelEnemyName.text = enemyName;
            PlayerPrefs.SetString(ArenaPageNew.ArenaFakePlayerNameKey, "");

            int enemyPower = PlayerPrefs.GetInt(ArenaPageNew.ArenaFakePlayerPowerKey);
            mlabelEnemyFightPower.text = enemyPower.ToString();
        }
        else
        {
            mlabelEnemyName.text = mCurResult.szRivalName;
            mlabelEnemyFightPower.text = mCurResult.iRivalPower.ToString();
        }


        mlabelMyName.text = Data_Pool.m_UserData.m_szRoleName;
        mlabelMyFightPower.text = UITool.f_CountToChineseStr(Data_Pool.m_TeamPool.f_GetTotalBattlePower());
        mlabelMyLevel.text = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level).ToString();
        int tSex = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_MainCard) % 2;
        miconMy.sprite2D = UITool.f_GetIconSpriteBySexId(tSex);

        mlabelEnemyLevel.text = Data_Pool.m_ArenaPool.enemyLevel.ToString();
        miconEnemy.sprite2D = UITool.f_GetIconSpriteBySexId(Data_Pool.m_ArenaPool.enemySex);

        //设置名词变化
        bool isRankChange = mCurResult.oldRank != mCurResult.curRank || enemyName != "";
        mRankTable.gameObject.SetActive(isRankChange);
        if (isRankChange)
        {
            mRankOld.text = enemyName != "" ? CommonTools.f_GetTransLanguage(744) : mCurResult.oldRank.ToString();
            mRankCur.text = mCurResult.curRank.ToString();
            mUpTip.SetActive(mCurResult.oldRank - mCurResult.curRank > 0 || enemyName != "");
            //mRankTable.repositionNow = true;
        }

        //播放音效
        bool isFail = mCurResult.isWin == (int)EM_ArenaResult.Lose;
        AudioMusicType audioType = isFail ? AudioMusicType.BattleFail : AudioMusicType.BattleVictory;
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(audioType, isFail);

        //加载结果光效
        //      EM_MagicId magicId = isFail ? EM_MagicId.eFightFinishFail : EM_MagicId.eFightFinishSuc;
        //      string animationName = isFail ? "a_zhandoushibai" : "a_zhandoushengli";
        //      UITool.f_CreateMagicById((int)magicId, ref resultMagic, mResultTexture.transform, 501, animationName, null);
        //      resultMagic.transform.localScale = new Vector3(100, 100, 100);
        //if(isFail == true)
        //	resultMagic.GetComponent<SkeletonAnimation>().state.AddAnimation(0, "a_zhandoushibai_loop", true, 0);
        //else
        //	resultMagic.GetComponent<SkeletonAnimation>().state.AddAnimation(0, "a_zhandoushengli_loop", true, 0);
    }

    private void f_MaskClose(GameObject go, object value1, object value2)
    {
        if (mRole != null)
        {
            UITool.f_DestoryStatelObject(mRole);
            mRole = null;
        }
        if (resultMagic != null)
        {
            UITool.f_DestoryStatelObject(resultMagic);
            resultMagic = null;
        }
        if (mCurResult.isWin != (int)EM_ArenaResult.Lose)
        {
            //ccUIManage.GetInstance().f_SendMsg(UINameConst.ArenaFinishPage, UIMessageDef.UI_CLOSE);
            //ccUIManage.GetInstance().f_SendMsg(UINameConst.ArenaChooseAwardPage, UIMessageDef.UI_OPEN);
            Data_Pool.m_ArenaPool.f_ArenaChooseAward((byte)1, f_ShowAwardCallback);
        }
        else
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.GameMain);
        }
    }

    //打开战斗数据页面
    private void f_OpenBattleDataPage(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.BattleDataPag, UIMessageDef.UI_OPEN);
    }

    private void f_BattleLoseProcess(GameObject go, object value1, object value2)
    {
        if (mRole != null)
        {
            UITool.f_DestoryStatelObject(mRole);
            mRole = null;
        }
        if (resultMagic != null)
        {
            UITool.f_DestoryStatelObject(resultMagic);
            resultMagic = null;
        }
        UITool.f_ProcessBattleLose((EM_BattleLoseProcess)value1);
    }

    /// <summary>
    /// 显示抽牌界面
    /// </summary>
    /// <param name="result"></param>
    private void f_ShowAwardCallback(object result)
    {
        CMsg_SC_ArenaChooseAward ret = (CMsg_SC_ArenaChooseAward)result;
        ChooseAwardParam param = new ChooseAwardParam();
        List<ResourceCommonDT> listCommonData = new List<ResourceCommonDT>();
        for (int i = 0; i < ret.awards.Length; i++)
        {
            ResourceCommonDT commonDT = new ResourceCommonDT();
            SC_Award scAward = ret.awards[i];
            commonDT.f_UpdateInfo(scAward.resourceType, scAward.resourceId, scAward.resourceNum);
            listCommonData.Add(commonDT);
        }
        param.mListData = listCommonData;
        param.mChooseIndex = ret.idx;
        param.mOnReturnCallback = OnChooseAwardCallback;
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ChooseAwardPage, UIMessageDef.UI_OPEN, param);
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
