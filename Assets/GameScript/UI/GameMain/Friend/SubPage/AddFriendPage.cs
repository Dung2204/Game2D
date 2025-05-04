using ccU3DEngine;
using UnityEngine;
using System.Collections;

public class AddFriendPage : UIFramwork
{
    private UIInput mNameInput;
    private GameObject mAddSureBtn;
    private GameObject mAddCancelBtn;

    private string mLastInputValue;
    private int mLastResult;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mNameInput = f_GetObject("NameInput").GetComponent<UIInput>(); 
        mAddSureBtn = f_GetObject("AddSureBtn");
        mAddCancelBtn = f_GetObject("AddCancelBtn");
        f_RegClickEvent(mAddSureBtn,f_AddFriendSure);
        f_RegClickEvent(mAddCancelBtn, f_AddFriendCancel);
        f_RegClickEvent("MaskClose", f_MaskClose);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        mNameInput.value = string.Empty;
mNameInput.defaultText = "Vui lòng nhập tên của người bạn muốn thêm";
        mLastInputValue = string.Empty;
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }

    private void f_MaskClose(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.AddFriendPage, UIMessageDef.UI_CLOSE);
    }

    private void f_AddFriendSure(GameObject go, object value1, object value2)
    {
        string tNowInputValue = mNameInput.value;
        int byteNum = ccMath.f_GetStringBytesLength(tNowInputValue);
        if (!Data_Pool.m_BlockWordPool.f_CheckValidity(ref tNowInputValue))
        {
UITool.Ui_Trip("Không được chứa các ký tự đặc biệt");
            mNameInput.value = tNowInputValue;
            return;
        }
        if (mLastInputValue == tNowInputValue && !string.IsNullOrEmpty(tNowInputValue))
        {
            UITool.Ui_Trip(f_GetDescByResult(mLastResult));
            return;
        }
        else if (byteNum < GameParamConst.RoleNameByteMinNum || byteNum > GameParamConst.RoleNameByteMaxNum)
        {
            mLastResult = (int)eMsgOperateResult.cOR_Error_StringLength;
            UITool.Ui_Trip(f_GetDescByResult(mLastResult));
            return;
        }
        else if (tNowInputValue == Data_Pool.m_UserData.m_szRoleName)
        {
            mLastResult = (int)eMsgOperateResult.cOR_Error_AddSelf;
            UITool.Ui_Trip(f_GetDescByResult(mLastResult));
            return;
        }
        else if (Data_Pool.m_FriendPool.f_CheckIsInBlackList(tNowInputValue))
        {
            mLastResult = (int)eMsgOperateResult.cOR_Error_InBlacklist;
            UITool.Ui_Trip(f_GetDescByResult(mLastResult));
            return;
        }
        else if (Data_Pool.m_FriendPool.f_CheckIsInApplyList(tNowInputValue))
        {
            mLastResult = (int)eMsgOperateResult.cOR_Error_InApplyList;
            UITool.Ui_Trip(f_GetDescByResult(mLastResult));
            return;
        }
        else if (Data_Pool.m_FriendPool.f_CheckIsFriend(tNowInputValue))
        {
            mLastResult = (int)eMsgOperateResult.cOR_Error_AddRepeat;
            UITool.Ui_Trip(f_GetDescByResult(mLastResult));
            return;
        }
        else if (Data_Pool.m_FriendPool.f_CheckIsFull())
        {
            mLastResult = (int)eMsgOperateResult.cOR_Error_AddNumFull;
            UITool.Ui_Trip(f_GetDescByResult(mLastResult));
            return;
        }
        mLastInputValue = mNameInput.value;
        SocketCallbackDT tCallBackDT = new SocketCallbackDT();
        tCallBackDT.m_ccCallbackSuc = f_AddFriendSuc;
        tCallBackDT.m_ccCallbackFail = f_AddFriendFail;
        //发送添加好友协议
        Data_Pool.m_FriendPool.f_ApplyFriendByName(mLastInputValue, tCallBackDT);
    }

    private void f_AddFriendCancel(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.AddFriendPage, UIMessageDef.UI_CLOSE);
    }

    private void f_AddFriendSuc(object result)
    {
        mLastResult = (int)result;
        UITool.Ui_Trip(f_GetDescByResult(mLastResult));
    }

    private void f_AddFriendFail(object result)
    {
        mLastResult = (int)result;
        UITool.Ui_Trip(f_GetDescByResult(mLastResult));
    }

    /// <summary>
    /// 根据返回结果，得到相应描述
    /// </summary>
    private string f_GetDescByResult(int result)
    {
        string tDesc = string.Empty;
        if (result == (int)eMsgOperateResult.OR_Succeed)
        {
tDesc = "Đã gửi yêu cầu kết bạn";
        }
        else if (result == (int)eMsgOperateResult.cOR_Error_StringLength)
        {
tDesc = "Tên của người cần thêm quá dài hoặc quá ngắn";
        }
        else if (result == (int)eMsgOperateResult.cOR_Error_AddSelf)
        {
tDesc = "Không thể thêm bản thân";
        }
        else if (result == (int)eMsgOperateResult.cOR_Error_AddRepeat)
        {
tDesc = string.Format("{0} đã là bạn rồi", mNameInput.value);
        }
        else if (result == (int)eMsgOperateResult.cOR_Error_AddNumFull)
        {
tDesc = "Danh sách bạn bè của bạn đã đầy";
        }
        else if (result == (int)eMsgOperateResult.cOR_Error_InBlacklist)
        {
tDesc = string.Format("{0} bị đưa vào danh sách đen", mNameInput.value);
        }
        else if (result == (int)eMsgOperateResult.cOR_Error_InApplyList)
        {
tDesc = string.Format("{0} danh sách chờ", mNameInput.value);
        }
        //服务器返回的
        else if (result == (int)eMsgOperateResult.eOR_UserNotFound)
        {
tDesc = "Không tìm thấy người chơi";
        }
        else if (result == (int)eMsgOperateResult.eOR_UserOffline)
        {
tDesc = string.Format("{0} đang ngoại tuyến", mNameInput.value);
        }
        else if (result == (int)eMsgOperateResult.eOR_InPeerBlack)
        {
tDesc = string.Format("Bạn bị đưa vào danh sách đen vì {0}", mNameInput.value);
        }
        else if (result == (int)eMsgOperateResult.eOR_PeerInBlack)
        {
tDesc = string.Format("{0} bị đưa vào danh sách đen", mNameInput.value);
        }
        else if (result == (int)eMsgOperateResult.eOR_FriendListIsFull)
        {
tDesc = string.Format("Danh sách bạn bè của {0} đã đầy", mNameInput.value);
        }
        return tDesc;
    }
}
