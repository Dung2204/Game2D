using ccU3DEngine;
using UnityEngine;

public class LegionBattleGatePage : UIFramwork
{
    private LegionBattleGateItem[] mGateItems;

    private UILabel mTotalStarNum;
    private UILabel mChallengeTimes;
    private UISprite mGateNameSprite;
    private UIScrollView mScrollView;

    private LegionBattleGatePageParam curData;
    private int flag;
	private int resetScroll = 0;

    private readonly string[] GateSpriteName = new string[5] { "", "Label_GateInside", "Label_GateMain", "Label_GateRight", "Label_GateLeft" };

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mGateItems = new LegionBattleGateItem[7];
        for (int i = 0; i < mGateItems.Length; i++)
        {
            mGateItems[i] = f_GetObject(string.Format("GateItem{0}", i)).GetComponent<LegionBattleGateItem>();
            mGateItems[i].f_ResetItem();
            f_RegClickEvent(mGateItems[i].mClickItem, f_OnGateItemClick,i);
        }
        mTotalStarNum = f_GetObject("TotalStarNum").GetComponent<UILabel>();
        mChallengeTimes = f_GetObject("ChallengeTimes").GetComponent<UILabel>();
        mGateNameSprite = f_GetObject("GateNameSprite").GetComponent<UISprite>();
        mScrollView = f_GetObject("ScrollView").GetComponent<UIScrollView>();
        f_RegClickEvent("Mask", f_OnBtnBackClick);
        f_RegClickEvent("BtnBack", f_OnBtnBackClick);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_LEGION_BATTLE_DEFENCE_LIST_UPDATE, f_EeventCallback_UpdateDefenceList, this);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_LEGION_FORCE_CLOSE, f_ProcessByMsg_ForceClose, this);
        if (e == null && (e is LegionBattleGatePageParam))
        {
//UITool.Ui_Trip("Parameter error");
MessageBox.ASSERT("Invalid port parameter");
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionBattleGatePage, UIMessageDef.UI_CLOSE);
            ccUIHoldPool.GetInstance().f_UnHold();
            return;
        }
        f_LoadTexture();
        curData = (LegionBattleGatePageParam)e;
        flag = curData.m_bSelfLegion ? 0 : 1;
        f_ResetGateItems();
        f_UpdateByInfo(curData.m_GateNode);
        if (curData.m_bSelfRequest)
        {
            UITool.f_OpenOrCloseWaitTip(true);
            SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
            socketCallbackDt.m_ccCallbackSuc = f_Callback_LegionBattleDefenceList;
            socketCallbackDt.m_ccCallbackFail = f_Callback_LegionBattleDefenceList;
            LegionMain.GetInstance().m_LegionBattlePool.f_LegionBattleDefenceList(flag, (int)curData.m_GateNode.m_GateType, socketCallbackDt);
        }
    }

    private const string szCenterBgFile = "UI/TextureRemove/Legion/Tex_LegionBattleGateBg";
    private void f_LoadTexture()
    {
        f_GetObject("TextureBg").GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(szCenterBgFile);
        //f_GetObject("TextureBgRight").GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(szCenterBgFile);
        //f_GetObject("TextureBgLeft").GetComponent<UITexture>().mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(szCenterBgFile);
    }

    private void f_Callback_LegionBattleDefenceList(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result != (int)eMsgOperateResult.OR_Succeed)
        {
UITool.Ui_Trip("Lỗi");
            MessageBox.ASSERT("LegionBattleDefenceList error,code:" + result);
        }
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_LEGION_BATTLE_DEFENCE_LIST_UPDATE, f_EeventCallback_UpdateDefenceList, this);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_LEGION_FORCE_CLOSE, f_ProcessByMsg_ForceClose, this);
    }

    private void f_UpdateByInfo(LegionBattleGateNode gateNode,bool resetScrollView = true)
    {
        int tStarNum = 0;
        for (int i = 0; i < gateNode.m_DefenderNodes.Length; i++)
        {
            if (i < mGateItems.Length)
            {
                mGateItems[i].f_UpdateByInfo(gateNode.m_DefenderNodes[i]);
                tStarNum += gateNode.m_DefenderNodes[i].m_iStarNum;
            }
        }
        mTotalStarNum.text = string.Format("Sao：{0}/{1}", tStarNum, gateNode.m_iStarMaxNum);
mChallengeTimes.text = string.Format("Lượt：[72ff00]{0}[-]", LegionMain.GetInstance().m_LegionBattlePool.m_iTimesLimit - LegionMain.GetInstance().m_LegionBattlePool.m_iTimes);
        int tType = (int)gateNode.m_GateType;
        mGateNameSprite.spriteName = tType >= 0 && tType < GateSpriteName.Length ? GateSpriteName[tType] : GateSpriteName[0];
        // mGateNameSprite.MakePixelPerfect();
        if (resetScrollView)
        {
            mScrollView.ResetPosition();
        }
    }

    private void f_ResetGateItems()
    {
        for (int i = 0; i < mGateItems.Length; i++)
        {
            mGateItems[i].f_ResetItem();
        }
    }

    private void f_OnBtnBackClick(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionBattleGatePage, UIMessageDef.UI_CLOSE);
        //ccUIHoldPool.GetInstance().f_UnHold();
    }

    private void f_OnGateItemClick(GameObject go, object value1, object value2)
    {
        int tIdx = (int)value1;
        if (curData.m_bSelfLegion)
        {
UITool.Ui_Trip("Không thể tự khiêu chiến bản thân");
            return;
        }
        int tNow = GameSocket.GetInstance().f_GetServerTime();
        LegionMain.GetInstance().m_LegionBattlePool.f_SetBattleTime(tNow);
        if (tIdx >= curData.m_GateNode.m_DefenderNodes.Length)
        {
//UITool.Ui_Trip("Phát sinh lỗi");
            MessageBox.ASSERT("LegionBattle DefenderNodes error, array is over len!");
            return;
        }
        else if (LegionMain.GetInstance().m_LegionBattlePool.m_iBeginTime <= LegionMain.GetInstance().m_LegionInfor.m_iIOTime + LegionConst.LEGION_BATTLE_CHALLENGE_TIME_LIMIT)
        {
UITool.Ui_Trip("Thời gian gia nhập quân đoàn chưa đủ 24h");
            return;
        }
        else if (tNow < LegionMain.GetInstance().m_LegionBattlePool.m_iBeginTime
            || tNow > LegionMain.GetInstance().m_LegionBattlePool.m_iEndTime)
        {
UITool.Ui_Trip("Quân đoàn chiến đã kết thúc");
            return;
        }
        else if (LegionMain.GetInstance().m_LegionBattlePool.m_iTimes >= LegionMain.GetInstance().m_LegionBattlePool.m_iTimesLimit)
        {
UITool.Ui_Trip("Đã hết thời gian khiêu chiến");
            return;
        }
        else if (curData.m_GateNode.m_DefenderNodes[tIdx].m_iStarNum >= curData.m_GateNode.m_DefenderNodes[tIdx].m_iStarMaxNum)
        {
UITool.Ui_Trip("Đã bị phá hủy");
            return;
        }
        //打开军团战挑战界面
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionBattleChallengePage, UIMessageDef.UI_OPEN, curData.m_GateNode.m_DefenderNodes[tIdx]);
    }
	
	void Update()
	{
		if(resetScroll < 2)
		{
			mScrollView.ResetPosition();
			resetScroll++;
		}
	}

    private void f_EeventCallback_UpdateDefenceList(object value)
    {
        f_UpdateByInfo(curData.m_GateNode, false);
    }

    private void f_ProcessByMsg_ForceClose(object value)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LegionBattleGatePage, UIMessageDef.UI_CLOSE);
    }
}


public class LegionBattleGatePageParam
{
    /// <summary>
    /// 军团城门数据
    /// </summary>
    /// <param name="gateData">军团城门节点数据</param>
    /// <param name="selfRequest">true：军团城门界面先打开，然后自己想服务器请求数据</param>
    public LegionBattleGatePageParam(bool selfLegion, LegionBattleGateNode gateData, bool selfRequest)
    {
        m_bSelfLegion = selfLegion;
        m_GateNode = gateData;
        m_bSelfRequest = selfRequest;
    }

    public bool m_bSelfLegion
    {
        get;
        private set;
    }

    public LegionBattleGateNode m_GateNode
    {
        get;
        private set;
    }

    public bool m_bSelfRequest
    {
        get;
        private set;
    }
}
