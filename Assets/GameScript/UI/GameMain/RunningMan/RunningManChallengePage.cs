using ccU3DEngine;
using UnityEngine;
using System.Collections;

public class RunningManChallengePage : UIFramwork
{
    private RunningManChallengeItem[] mChallengeItems;

    private UILabel mTollgateName;
    private UILabel mTollgateId;
    private Transform mRoleParent;

    private GameObject role;

    private RunningManTollgatePoolDT mData;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mChallengeItems = new RunningManChallengeItem[GameParamConst.RMModeNumPreTollgate];
        for (int i = 0; i < mChallengeItems.Length; i++)
        {
            mChallengeItems[i] = f_GetObject(string.Format("ChallengeItem{0}",i)).GetComponent<RunningManChallengeItem>();
        }
        mTollgateName = f_GetObject("TollgateName").GetComponent<UILabel>();
        mTollgateId = f_GetObject("TollgateId").GetComponent<UILabel>();
        mRoleParent = f_GetObject("RoleParent").transform;
        f_RegClickEvent("BtnClose", f_BtnClose);
        f_RegClickEvent("MaskClose", f_BtnClose);
        f_RegClickEvent("BtnClothArray", f_BtnClothArray);
        f_RegClickEvent("BtnLineup", f_LineupHandle);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        if (e == null || !(e is RunningManTollgatePoolDT))
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(842));
        f_UpdateByInfo(e as RunningManTollgatePoolDT);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_THENEXTDAY_UIPROCESS, f_ProcessNextDay, this);
        
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_THENEXTDAY_UIPROCESS, f_ProcessNextDay, this);
    }

    private void f_ProcessNextDay(object value)
    {
        if ((EM_NextDaySource)value != EM_NextDaySource.RunningManPool)
            return;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RunningManChallengePage, UIMessageDef.UI_CLOSE);
    }

    private void f_UpdateByInfo(RunningManTollgatePoolDT data)
    {
        mData = data;
        int[] moneyArr = ccMath.f_String2ArrayInt(mData.m_TollgateTemplate.szMoneys, ";");
        int[] prestigeArr = ccMath.f_String2ArrayInt(mData.m_TollgateTemplate.szPrests, ";");
        int[] passParamArr = ccMath.f_String2ArrayInt(mData.m_TollgateTemplate.szPassParams, ";");
        string passTypeDesc = Data_Pool.m_RunningManPool.f_GetPassTypeDesc(mData.m_TollgateTemplate.iPassType);
        for (int i = 0; i < mChallengeItems.Length; i++)
        {
            mChallengeItems[i].f_UpdateByInfo(moneyArr[i],prestigeArr[i],string.Format(passTypeDesc,passParamArr[i]));
            f_RegClickEvent(mChallengeItems[i].mBtnChallenge,f_TollgateChallengeHandle,i+1);
        }
        mTollgateId.text = data.m_TollgateTemplate.iId.ToString();
        mTollgateName.text = data.m_TollgateTemplate.szName;
        UITool.f_CreateRoleByModeId(mData.m_iMonsterId,ref role, mRoleParent, 3, false);
    }

    private void f_TollgateChallengeHandle(GameObject go, object value1, object value2)
    {
        int star = (int)value1;
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_RunningManChallenge;
        socketCallbackDt.m_ccCallbackFail = f_RunningManChallenge;
        Data_Pool.m_RunningManPool.f_RunningManChallenge((ushort)mData.m_iChapterId, (byte)((mData.m_iTollgateId-1)%3+1), (byte)star , mData, socketCallbackDt);
    }

    private void f_BtnClose(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RunningManChallengePage, UIMessageDef.UI_CLOSE);
    }

    private void f_BtnClothArray(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ClothArrayPage, UIMessageDef.UI_OPEN);
    }

    private void f_RunningManChallenge(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            //展示加载界面 并加载战斗场景
            ccUIManage.GetInstance().f_SendMsg(UINameConst.RunningManPage, UIMessageDef.UI_CLOSE);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.RunningManChallengePage, UIMessageDef.UI_CLOSE);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.BattleMain);
        }
        else
        {
            MessageBox.ASSERT(CommonTools.f_GetTransLanguage(844) + result);
        }
    }

    private void f_LineupHandle(GameObject go, object value1, object value2)
    {
        //打开布阵
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ClothArrayPage, UIMessageDef.UI_OPEN);
    }

}
