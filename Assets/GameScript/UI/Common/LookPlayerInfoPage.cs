using ccU3DEngine;
using UnityEngine;
using System.Collections;

public class LookPlayerInfoPage : UIFramwork
{
    private UI2DSprite mIcon;
    private UILabel mName;
    private UILabel mLevelLabel;
    private UILabel mPowerLabel;
    private UILabel mGuildLabel;
    private UILabel mVipLabel;
    private UILabel mUserId;

    private GameObject mDeleteFriendBtn;
    private GameObject mAddFriendBtn;
    private GameObject mBlacklistBtn;
    private GameObject mPrivateChatBtn;
    private GameObject mLookFormationBtn;
    private GameObject mBattleBtn;
    private UISprite mIconBorder;

    public BasePlayerPoolDT mData
    {
        get;
        private set;
    }

    /// <summary>
    /// 是否已经是好友
    /// </summary>
    private bool mIsFriend = false;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mIcon = f_GetObject("Icon").GetComponent<UI2DSprite>();
        mName = f_GetObject("Name").GetComponent<UILabel>();
        mLevelLabel = f_GetObject("LevelLabel").GetComponent<UILabel>();
        mPowerLabel = f_GetObject("PowerLabel").GetComponent<UILabel>();
        mGuildLabel = f_GetObject("GuildLabel").GetComponent<UILabel>();
        mVipLabel = f_GetObject("VipLabel").GetComponent<UILabel>();
        mUserId = f_GetObject("UserIdLabel").GetComponent<UILabel>();
        mDeleteFriendBtn = f_GetObject("DeleteFriendBtn");
        mAddFriendBtn = f_GetObject("AddFriendBtn");
        mBlacklistBtn = f_GetObject("BlacklistBtn");
        mPrivateChatBtn = f_GetObject("PrivateChatBtn");
        mLookFormationBtn = f_GetObject("LookFormationBtn");
        mBattleBtn = f_GetObject("BattleBtn");
        mIconBorder = f_GetObject("IconBorder").GetComponent<UISprite>();
        f_RegClickEvent(mDeleteFriendBtn, f_DeleteFriendBtnHandle);
        f_RegClickEvent(mAddFriendBtn, f_AddFriendBtnHandle);
        f_RegClickEvent(mBlacklistBtn, f_BlacklistBtnHandle);
        f_RegClickEvent(mPrivateChatBtn, f_PrivateChatBtnHandle);
        f_RegClickEvent(mLookFormationBtn, f_LookFormationBtnHandle);
        f_RegClickEvent(mBattleBtn, f_BattleBtnHandle);
        f_RegClickEvent("MaskClose", f_MaskClose);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        if (e == null || !(e is BasePlayerPoolDT))
MessageBox.ASSERT("LookPlayerInfoPage must be passed as parameter BasePlayerPoolDT");
        mData = (BasePlayerPoolDT)e;
        mIcon.sprite2D = UITool.f_GetIconSpriteBySexId(mData.m_iSex);
        string tName = mData.m_szName;
        int iFrame = mData.m_iFrameId;
        mIconBorder.spriteName = UITool.f_GetImporentColorName(iFrame, ref tName);
        mName.text = tName;
        mLevelLabel.text = mData.m_iLv.ToString();
        mPowerLabel.text = UITool.f_CountToChineseStr(mData.m_iBattlePower);
        mGuildLabel.text = mData.m_szLegion;
        mVipLabel.text = string.Format("VIP {0}", mData.m_iVip);
        mIsFriend = Data_Pool.m_FriendPool.f_CheckIsFriend(mData.iId);
        mUserId.text = mData.iId.ToString();
        mDeleteFriendBtn.SetActive(mIsFriend);
        mAddFriendBtn.SetActive(!mIsFriend);
        f_GetObject("BtnGrid").GetComponent<UIGrid>().Reposition();
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }

    //关闭处理
    private void f_MaskClose(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LookPlayerInfoPage, UIMessageDef.UI_CLOSE);
    }

    /// <summary>
    /// 删除好友
    /// </summary>
    private void f_DeleteFriendBtnHandle(GameObject go, object value1, object value2)
    {
        if (!mIsFriend)
        {
UITool.Ui_Trip(string.Format("{0} không phải bạn", mData.m_szName));
            return;
        }
        else if (mData.iId == Data_Pool.m_UserData.m_iUserId)
        {
UITool.Ui_Trip("Không thể tự xóa bản thân");
            return;
        }
string tContent = string.Format("Xác nhận xóa {0}？", mData.m_szName);
PopupMenuParams tParam = new PopupMenuParams("Nhắc nhở", tContent, "Đồng ý", f_SureDeleteFriend, "Hủy bỏ");
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, tParam);

    }
    //确定删除好友
    private void f_SureDeleteFriend(object value)
    {
        SocketCallbackDT tCallBackDT = new SocketCallbackDT();
        tCallBackDT.m_ccCallbackSuc = f_DeleteFriendSuc;
        tCallBackDT.m_ccCallbackFail = f_DeleteFriendFail;
        //发送删除好友协议
        Data_Pool.m_FriendPool.f_RemoveFriend(mData.iId, tCallBackDT);
    }

    /// <summary>
    /// 添加好友
    /// </summary>
    private void f_AddFriendBtnHandle(GameObject go, object value1, object value2)
    {
        if (mIsFriend)
        {
UITool.Ui_Trip(string.Format("{0} đã là bạn", mData.m_szName));
            return;
        }
        else if (mData.iId == Data_Pool.m_UserData.m_iUserId)
        {
UITool.Ui_Trip("Không thể tự thêm bản thân");
            return;
        }
        else if (Data_Pool.m_FriendPool.f_CheckIsFull())
        {
UITool.Ui_Trip("Danh sách bạn đã đầy");
            return;
        }
        else if (Data_Pool.m_FriendPool.f_CheckIsInBlackList(mData.iId))
        {
UITool.Ui_Trip(string.Format("{0} nằm trong danh sách đen", mData.m_szName));
            return;
        }
        else if (Data_Pool.m_FriendPool.f_CheckIsInApplyList(mData.iId))
        {
UITool.Ui_Trip(string.Format("Đã gửi lời mời cho {0}", mData.m_szName));
            return;
        }
        SocketCallbackDT tCallBackDT = new SocketCallbackDT();
        tCallBackDT.m_ccCallbackSuc = f_AddFriendSuc;
        tCallBackDT.m_ccCallbackFail = f_AddFriendFail;
        //发送添加好友协议
        Data_Pool.m_FriendPool.f_ApplyFriend(mData.iId, tCallBackDT);
    }

    /// <summary>
    /// 加入黑名单
    /// </summary>
    private void f_BlacklistBtnHandle(GameObject go, object value1, object value2)
    {
        bool tIsBlacklist = Data_Pool.m_FriendPool.f_CheckIsInBlackList(mData.iId);
        if (tIsBlacklist)
        {
UITool.Ui_Trip(string.Format("{0} đã nằm trong danh sách", mData.m_szName));
            return;
        }
        else if (Data_Pool.m_FriendPool.f_CheckIsInApplyList(mData.iId))
        {
UITool.Ui_Trip(string.Format("{0} đã thêm"));
            return;
        }
string tContent = string.Format("Xác nhận thêm {0} vào danh sách đen?", mData.m_szName);
PopupMenuParams tParam = new PopupMenuParams("Nhắc nhở", tContent, "Đồng ý", f_SureBlacklist, "Hủy bỏ");
        ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, tParam);
    }

    /// <summary>
    /// 确定加入黑名单
    /// </summary>
    private void f_SureBlacklist(object value)
    {
        SocketCallbackDT tCallBackDT = new SocketCallbackDT();
        tCallBackDT.m_ccCallbackSuc = f_BlacklistSuc;
        tCallBackDT.m_ccCallbackFail = f_BlacklistFail;
        //发送加入黑名单协议
        Data_Pool.m_FriendPool.f_Blacklist(mData.iId, tCallBackDT);
    }

    /// <summary>
    /// 私聊
    /// </summary>
    private void f_PrivateChatBtnHandle(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LookPlayerInfoPage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ChatManage, UIMessageDef.UI_OPEN, new PrivateData() { UserTarget = mData.iId, RoomTarget = 0 });
    }

    /// <summary>
    /// 查看阵容
    /// </summary>
    private void f_LookFormationBtnHandle(GameObject go, object value1, object value2)
    {
        ViewPlayerLineUpPageParam param = new ViewPlayerLineUpPageParam();
        param.userId = mData.iId;
        param.szName = mData.m_szName;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ViewPlayerLineUpPage, UIMessageDef.UI_OPEN, param);
    }

    /// <summary>
    /// 切磋
    /// </summary>
    private void f_BattleBtnHandle(GameObject go, object value1, object value2)
    {
		UITool.Ui_Trip("Khiêu chiến");
    }


    #region 协议回调处理

    private void f_DeleteFriendSuc(object result)
    {
UITool.Ui_Trip("Đã xóa");
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LookPlayerInfoPage, UIMessageDef.UI_CLOSE);
    }

    private void f_DeleteFriendFail(object result)
    {
UITool.UI_ShowFailContent("Thất bại，code：" + result);
    }

    private void f_AddFriendSuc(object resutlt)
    {
UITool.Ui_Trip("Đã gửi lời mời");
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LookPlayerInfoPage, UIMessageDef.UI_CLOSE);
    }

    private void f_AddFriendFail(object result)
    {
string tDesc = UITool.f_GetFriendServerResultDesc("Adversary", (int)result);
        if (!string.IsNullOrEmpty(tDesc))
        {
            UITool.Ui_Trip(tDesc);
        }
        else
        {
UITool.UI_ShowFailContent("Thất bại，code：" + result);
        }
    }

    private void f_BlacklistSuc(object result)
    {
UITool.Ui_Trip("Đã thêm");
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LookPlayerInfoPage, UIMessageDef.UI_CLOSE);
    }

    private void f_BlacklistFail(object result)
    {
UITool.UI_ShowFailContent("Thất bại，code：" + result);
    }

    #endregion

}
