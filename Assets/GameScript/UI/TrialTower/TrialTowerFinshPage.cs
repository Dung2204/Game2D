using ccU3DEngine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialTowerFinshPage : UIFramwork
{

    private GameObject mMaskClose;
    
    private GameObject mLoseObj;
    private UITexture mResultTexture;
    

    private bool mCurResult;

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
        mLoseObj = f_GetObject("LosePage");
        mResultTexture = f_GetObject("ResultTexture").GetComponent<UITexture>();
        // mResultTexture.enabled = false;

        //特效
        //mWinTitleParent = f_GetObject("WinTitleParent").transform;

        //模型
        mRoleParent = f_GetObject("RoleParent").transform;

        //监听事件
        f_RegClickEvent(mMaskClose, f_MaskClose);
        f_RegClickEvent("BattleData", f_OpenBattleDataPage);
        f_RegClickEvent("BtnGetMoreCard", f_BattleLoseProcess, EM_BattleLoseProcess.GetMoreCard);
        f_RegClickEvent("BtnCardIntensify", f_BattleLoseProcess, EM_BattleLoseProcess.CardIntensify);
        f_RegClickEvent("BtnEquipIntensify", f_BattleLoseProcess, EM_BattleLoseProcess.EquipIntensify);
        //f_RegClickEvent("BtnLineupChange", f_BattleLoseProcess, EM_BattleLoseProcess.LineupChange);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        mCurResult = Data_Pool.m_TrialTowerPool.FinshRet;
        mLoseObj.SetActive(!mCurResult);

        //加载模型
        if (mRole == null)
        {
            UITool.f_CreateRoleByCardId(Data_Pool.m_CardPool.mRolePoolDt.m_iTempleteId, ref mRole, mRoleParent, 501);
            Transform RoleTran = mRole.transform;
            RoleTran.localScale = Vector3.one * 150;
            RoleTran.localPosition = new Vector3(0, -180, 0);
        }

        //播放音效
        bool isFail = !mCurResult;
        AudioMusicType audioType = isFail ? AudioMusicType.BattleFail : AudioMusicType.BattleVictory;
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(audioType, isFail);
		if(isFail)
		{
			f_GetObject("ResultTexture").SetActive(false);
		}
        //加载结果光效
        // EM_MagicId magicId = isFail ? EM_MagicId.eFightFinishFail : EM_MagicId.eFightFinishSuc;
        // string animationName = isFail ? "a_zhandoushibai" : "a_zhandoushengli";
        // UITool.f_CreateMagicById((int)magicId, ref resultMagic, mResultTexture.transform, 501, animationName, null);
        // resultMagic.transform.localScale = new Vector3(100, 100, 100);
        // resultMagic.GetComponent<SkeletonAnimation>().state.AddAnimation(0, animationName + "_loop", true, 0);
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
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.GameMain);
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
    
}
