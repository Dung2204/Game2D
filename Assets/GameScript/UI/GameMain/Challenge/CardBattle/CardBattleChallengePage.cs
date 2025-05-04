using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;

public class CardBattleChallengePage : SubPanelBase
{
    public GameObject mEnemyItemParent;
    public GameObject mEnemyItem;
    private List<BasePoolDT<long>> m_EnemyList;
    private UIWrapComponent _enemyListWrapComponent;
    private UIWrapComponent mEnemyListWrapComponent
    {
        get
        {
            if (_enemyListWrapComponent == null)
            {
                m_EnemyList = Data_Pool.m_CardBattlePool.EnemyList;
                //需要保证滚动Item不能少于展示Item，不然拖拽时有可能改变了Item的数据
                _enemyListWrapComponent = new UIWrapComponent(210, 1, 160, 3, mEnemyItemParent, mEnemyItem, m_EnemyList, f_OnEnemyItemUpdate, null);
            }
            return _enemyListWrapComponent;
        }
    }

    public GameObject mBtnRefreshEnemy;
    public GameObject mBtnRefreshEnemyGray;
    public UILabel mBtnRefreshEnemyGrayLabel;
    public UILabel mChallengeTimes;
    public UILabel mChallengeEndTime;
    public GameObject mBtnHelp;

    public GameObject mBtnClothArray;
    public GameObject[] mAtkCloth;
    public UIGrid mClothGrid;

    public Transform mRoleParent;
    private GameObject mRole;
    public UILabel mSelfName;
    //个人战绩
    public UILabel mSelfRecord;

    private List<CardBattleClothPoolDT> m_AtkCloth;

    public override void f_Init(UIFramwork parentUI)
    {
        base.f_Init(parentUI);
        f_RegClickEvent(mBtnRefreshEnemy, f_OnBtnRefreshEnemyClick, true, false);
        f_RegClickEvent(mBtnClothArray, f_OnBtnClothArrayClick);
        f_RegClickEvent(mBtnHelp, f_OnBtnHlepClick);
        mRole = null;
    }

    public override void f_Open()
    {
        base.f_Open();
        m_AtkCloth = Data_Pool.m_CardBattlePool.AtkClothList;
        f_UpdateCloth();
mChallengeTimes.text = string.Format("Today's challenge：[72ff00]{0} [-]times", Data_Pool.m_CardBattlePool.LeftChallengeTimes);
        //mChallengeEndTime.text = string.Format("[ff8439]Thời gian kết thúc: [72ff00]{0:d2}:{1:d2}:{2:d2}[-][-]", Data_Pool.m_CardBattlePool.BattleEndTime / (60 * 60) % 24 + 8, Data_Pool.m_CardBattlePool.BattleEndTime / 60 % 60, 0);
        mChallengeEndTime.text = string.Format("Thời gian kết thúc: [72ff00]{0:d2}:{1:d2}:{2:d2}[-]", Data_Pool.m_CardBattlePool.BattleEndTime / (60 * 60) % 24 + 7, Data_Pool.m_CardBattlePool.BattleEndTime / 60 % 60, 0); //TsuCode - time +8 -> +7
        f_UpdateSlefInfo();
        f_UpdateBySecond();
        //刷新对手列表
        f_OnBtnRefreshEnemyClick(null, false, true);
    }

    public override void f_Close()
    {
        base.f_Close();
        if (mRole != null)
        {
            UITool.f_DestoryStatelObject(mRole);
            mRole = null;
        }
    }

    public override void f_RegEvent()
    {
        base.f_RegEvent();
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_CardBattleClothPage_UpdateCloth, f_UpdateClothByMsg, this);
    }

    public override void f_UnregEvent()
    {
        base.f_UnregEvent();
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_CardBattleClothPage_UpdateCloth, f_UpdateClothByMsg, this);
    }

    private void f_UpdateClothByMsg(object value)
    {
        f_UpdateCloth();
    }

    private void f_UpdateCloth()
    {
        for (int i = 0; i < m_AtkCloth.Count; i++)
        {
            if (i >= mAtkCloth.Length)
                break;
            if (m_AtkCloth[i].CardTemplateId == 0 || m_AtkCloth[i].CardTemplate == null)
            {
                mAtkCloth[i].SetActive(false);
                continue;
            }
            mAtkCloth[i].SetActive(true);
            UI2DSprite icon = mAtkCloth[i].transform.Find("Icon").GetComponent<UI2DSprite>();
            UISprite frame = mAtkCloth[i].transform.Find("Frame").GetComponent<UISprite>();
            icon.sprite2D = UITool.f_GetIconSpriteByCardId(m_AtkCloth[i].CardTemplateId);
            frame.spriteName = UITool.f_GetImporentCase(m_AtkCloth[i].CardTemplate.iImportant);
        }
        mClothGrid.repositionNow = true;
        mClothGrid.Reposition();
    }

    private void f_UpdateSlefInfo()
    {
        //UITool.f_CreateRoleByCardId(Data_Pool.m_CardPool.mRolePoolDt.m_CardDT.iId, ref mRole, mRoleParent, 1);   
        mSelfName.text = UITool.f_GetImporentForName(Data_Pool.m_CardPool.mRolePoolDt.m_CardDT.iImportant, Data_Pool.m_UserData.m_szRoleName);
mSelfRecord.text = string.Format("Achievement：[72ff00] {0}[-] Win [72ff00]{1}[-] Collect", Data_Pool.m_CardBattlePool.WinTimes, Data_Pool.m_CardBattlePool.LoseTimes);
    }
    

    public void f_UpdateBySecond()
    {
        if (!mPanel.activeSelf)
            return;
        int now = GameSocket.GetInstance().f_GetServerTime();
        int refreshEnemyTime = Data_Pool.m_CardBattlePool.RefreshEnemyTime;
        if (now - refreshEnemyTime < Data_Pool.m_CardBattlePool.RefreshEnemyTimeCD)
        {
            if (mBtnRefreshEnemy.activeSelf)
                mBtnRefreshEnemy.SetActive(false);
            if (!mBtnRefreshEnemyGray.activeSelf)
                mBtnRefreshEnemyGray.SetActive(true);
mBtnRefreshEnemyGrayLabel.text = string.Format("Refresh（{0}）", Data_Pool.m_CardBattlePool.RefreshEnemyTimeCD - (now - refreshEnemyTime));
        }
        else
        {
            if (!mBtnRefreshEnemy.activeSelf)
                mBtnRefreshEnemy.SetActive(true);
            if (mBtnRefreshEnemyGray.activeSelf)
                mBtnRefreshEnemyGray.SetActive(false);
        }
    }

    private void f_OnEnemyItemUpdate(Transform tf, BasePoolDT<long> dt)
    {
        CardBattleEnemyItem tItem = tf.GetComponent<CardBattleEnemyItem>();
        tItem.f_UpdateByInfo((CardBattleEnemyPoolDT)dt);
        f_RegClickEvent(tItem.mBtnChallenge, f_OnChallengeBtnClick, dt);
    }

    private void f_OnChallengeBtnClick(GameObject go, object value1, object value2)
    {
        if (f_IsAtkClothInvalid(true))
            return;
        CardBattlePool.EM_CardBattleState state = Data_Pool.m_CardBattlePool.f_GetState();
        if (state == CardBattlePool.EM_CardBattleState.BetweenApplyBattle)
        {
UITool.Ui_Trip(string.Format("Bắt đầu lúc {0}:{1}", Data_Pool.m_CardBattlePool.BattleTime[0], Data_Pool.m_CardBattlePool.BattleTime[1] != 0 ? Data_Pool.m_CardBattlePool.BattleTime[1].ToString() : ""));
            return;
        }
        else if (Data_Pool.m_CardBattlePool.LeftChallengeTimes <= 0)
        {
UITool.Ui_Trip("Đã hết lượt khiêu chiến");
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        CardBattleEnemyPoolDT enemyInfo = (CardBattleEnemyPoolDT)value1;
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_ChallengeEnemy;
        socketCallbackDt.m_ccCallbackFail = f_Callback_ChallengeEnemy;
        Data_Pool.m_CardBattlePool.f_ChallengeEnemy(enemyInfo.iId, socketCallbackDt);
    }

    private void f_Callback_ChallengeEnemy(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
            //开启战斗
            ccUIManage.GetInstance().f_SendMsg(UINameConst.CardBattlePage, UIMessageDef.UI_CLOSE);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.LoadingPage, UIMessageDef.UI_OPEN, EM_Scene.BattleMain);
        }
        else if ((int)result == (int)eMsgOperateResult.eOR_CrossCardBattleNoRegedit)
        {
UITool.Ui_Trip("Chưa đăng ký");
        }
        else if ((int)result == (int)eMsgOperateResult.eOR_CrossCardBattleIsResult)
        {
UITool.Ui_Trip("Đang tổng kết");
        }
        else
        {
UITool.Ui_Trip("Challenge error，code:" + (int)result);
        }
    }

    private void f_OnBtnRefreshEnemyClick(GameObject go, object value1, object value2)
    {
        bool showTip = true;
        bool isOpenPageRequest = true;
        if (value1 != null && value1 is bool)
            showTip = (bool)value1;
        if (value2 != null && value2 is bool)
            isOpenPageRequest = (bool)value2;
        if (f_IsAtkClothInvalid(showTip))
            return;
        if (!isOpenPageRequest && Data_Pool.m_CardBattlePool.LeftChallengeTimes <= 0)
        {
UITool.Ui_Trip("Khiêu chiến đã kết thúc");
            return;
        }
        int now = GameSocket.GetInstance().f_GetServerTime();
        int refreshEnemyTime = Data_Pool.m_CardBattlePool.RefreshEnemyTime;
        if (now - refreshEnemyTime < Data_Pool.m_CardBattlePool.RefreshEnemyTimeCD)
        {
            if (!showTip)
                return;
            int cdTime = Data_Pool.m_CardBattlePool.RefreshEnemyTimeCD - now + refreshEnemyTime;
UITool.Ui_Trip(string.Format("Thử lại sau {0}", cdTime));
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true, true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_RefreshEnemy;
        socketCallbackDt.m_ccCallbackFail = f_Callback_RefreshEnemy;
        Data_Pool.m_CardBattlePool.f_RefreshEnemyList(socketCallbackDt);
    }

    private void f_Callback_RefreshEnemy(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result == (int)eMsgOperateResult.OR_Succeed)
        {
UITool.Ui_Trip("Làm mới thành công");
            f_UpdateBySecond();
        }
        else if ((int)result == (int)eMsgOperateResult.eOR_CrossCardBattleNoRegedit)
        {
            //UITool.Ui_Trip("跨服斗将未报名不能進行刷新");
        }
        else if ((int)result == (int)eMsgOperateResult.eOR_CrossCardBattleIsResult)
        {
            //UITool.Ui_Trip("跨服斗将今日已結算,无需刷新");
        }
        else
        {
UITool.Ui_Trip("Làm mới Error，code:" + (int)result);
        }
        mEnemyListWrapComponent.f_ResetView();
    }

    private void f_OnBtnClothArrayClick(GameObject go, object value1, object value2)
    {
        if (f_IsAtkClothInvalid(true))
            return;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CardBattleClothPage, UIMessageDef.UI_OPEN);
    }

    private bool f_IsAtkClothInvalid(bool showTip)
    {
        for (int i = 0; i < m_AtkCloth.Count; i++)
        {
            if (m_AtkCloth[i].CardTemplateId != 0)
            {
                return false;
            }
        }
        if (showTip)
UITool.Ui_Trip("Đội hình không hợp lệ");
        return true;
    }

    public void f_OnBtnHlepClick(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CommonHelpPage, UIMessageDef.UI_OPEN, 15);
    }
}
