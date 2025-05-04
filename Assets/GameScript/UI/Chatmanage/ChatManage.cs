using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System;
using System.Collections.Generic;
using System.Linq;
public struct PrivateData
{
    public long UserTarget;
    public long RoomTarget;
}
public class ChatManage : UIFramwork
{
    private float ChatTimeMax = 10.0f;
    private float ChatTimeNow = 0f;
    private int Time_ChatTime = 0;

    private bool bCanSend = true;
    private PrivateData CurrentPrivateData;
    //private long CurrentPrivateUserTarget = -1;
    //private long CurrentPrivateRoomTarget = -1;
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        Init();
        ShowAllChat();
        Data_Pool.m_ChatPool.isUIShow = true;
        Chat_Input.value = "";
        if (e != null)
        {
            if (e is PrivateData)
            {
                ChatType = EM_ChatChan.eChan_Private;
                CurrentPrivateData = (PrivateData)e;
            }
            else
            {
                ChatType = (EM_ChatChan)e;
            }
        }
        
        switch (ChatType)
        {
            default:
            case EM_ChatChan.eChan_World:
                Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.ChatWordNewMsg);
                f_GetObject("WorldBtn").GetComponent<UIToggle>().value = true;
                f_GetObject("control").SetActive(true);
                break;
            case EM_ChatChan.eChan_Legion:
                Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.ChatLegionNewMsg);
                f_GetObject("GroupBtn").GetComponent<UIToggle>().value = true;
                f_GetObject("control").SetActive(true);
                break;
            case EM_ChatChan.eChan_System:
                Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.ChatTeamNewMsg);
                f_GetObject("control").SetActive(false);
                break;
            case EM_ChatChan.eChan_Private:
                Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.ChatPrivateNewMsg);
                f_GetObject("PrivateBtn").GetComponent<UIToggle>().value = true;
                ShowUIPrivate();
                break;
        }

        
    }

    private void ShowUIPrivate(object obj = null)
    {
        GameObject privateGO = f_GetObject("ChatPrivate");
        bool isShowChatList = CurrentPrivateData.Equals(default(PrivateData));
        f_GetObject("control").SetActive(!isShowChatList);
        GameObject parentChatAll = privateGO.transform.Find("ChatAll/ChatParent").gameObject;
        privateGO.transform.Find("ChatAll").gameObject.SetActive(!isShowChatList);
        privateGO.transform.Find("TargetInfo").gameObject.SetActive(!isShowChatList);
        f_GetObject("BtnPrivateBack").gameObject.SetActive(!isShowChatList);
        privateGO.transform.Find("ChatList").gameObject.SetActive(isShowChatList);
        GameObject noDataGO = privateGO.transform.Find("ChatList/noData").gameObject;
        GameObject itemTemplate = privateGO.transform.Find("ChatList/itemTemplate").gameObject;
        itemTemplate.SetActive(false);
        GameObject parentChatList = privateGO.transform.Find("ChatList/ChatListParent").gameObject;

        if (isShowChatList)
        {
            bool hasNoRoom = Data_Pool.m_ChatPool.ChatRoomList == null || Data_Pool.m_ChatPool.ChatRoomList.Count <= 0;
            noDataGO.SetActive(hasNoRoom);
            parentChatList.SetActive(!hasNoRoom);
            if (!hasNoRoom) {
                for (int i = 0; i < parentChatList.transform.childCount; i++)
                {
                    Destroy(parentChatList.transform.GetChild(i).gameObject);
                }
                Data_Pool.m_ChatPool.ChatRoomList.ForEach((o) => {
                    UpdateChatListItem(Instantiate(itemTemplate, parentChatList.transform),o);
                });
                parentChatList.GetComponent<UIGrid>().repositionNow = true;
            }
        }
        else {
            BasePlayerPoolDT tBasePlayerPoolDT = ((BasePlayerPoolDT)Data_Pool.m_GeneralPlayerPool.f_GetForId(CurrentPrivateData.UserTarget));
            string tNname = tBasePlayerPoolDT.m_szName;
            privateGO.transform.Find("TargetInfo/Chat_Icon").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSpriteBySexId(tBasePlayerPoolDT.m_iSex);
            privateGO.transform.Find("TargetInfo/Chat_Case").GetComponent<UISprite>().spriteName = UITool.f_GetImporentColorName(tBasePlayerPoolDT.m_iFrameId, ref tNname);
            privateGO.transform.Find("TargetInfo/Chat_Name").GetComponent<UILabel>().text = tNname;
            for (int i = 0; i < parentChatAll.transform.childCount; i++)
            {
                Destroy(parentChatAll.transform.GetChild(i).gameObject);
            }
            parentChatAll.GetComponent<UIGrid>().repositionNow = true;
            ChatRoomInfo info = Data_Pool.m_ChatPool.ChatRoomList.FirstOrDefault(o => o.roomMembers.Contains(CurrentPrivateData.UserTarget));
            ChatSocket.GetInstance().f_GetRoomMessage(info.roomId);
        }
    }

    private void UpdateChatListItem(GameObject go, ChatRoomInfo roomInfo)
    {
        go.SetActive(true);
        long targetId = roomInfo.roomMembers.ToList().FirstOrDefault(o => o != Data_Pool.m_UserData.m_iUserId);
        BasePlayerPoolDT tBasePlayerPoolDT = ((BasePlayerPoolDT)Data_Pool.m_GeneralPlayerPool.f_GetForId(targetId));
        string tNname = tBasePlayerPoolDT.m_szName;
        go.transform.Find("icon").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSpriteBySexId(tBasePlayerPoolDT.m_iSex);
        go.transform.Find("frame").GetComponent<UISprite>().spriteName = UITool.f_GetImporentColorName(tBasePlayerPoolDT.m_iFrameId, ref tNname);
        go.transform.Find("name").GetComponent<UILabel>().text = tNname;
        go.transform.Find("lastUpdate").GetComponent<UILabel>().text = roomInfo.lastUpdate.ToString();

        ccUIEventListener.Get(go.transform.Find("goto").gameObject).f_RegClick((_go,_o1,_o2)=> {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.ChatManage, UIMessageDef.UI_OPEN, new PrivateData() { UserTarget = targetId, RoomTarget = roomInfo.roomId });
        }, null, null);
    }

    protected override void UI_HOLD(object e)
    {
        base.UI_HOLD(e);
        CurrentPrivateData = default(PrivateData);
    }

    void ReceiveMessage(object obj)
    {

        ChatPoolDT tmp = (ChatPoolDT)obj;
        if (!CurrentPrivateData.Equals(default(PrivateData)) && CurrentPrivateData.RoomTarget != tmp.m_RoomId && CurrentPrivateData.UserTarget != tmp.m_Id) {
            return;
        }
        AddChat(tmp);
    }
    /// <summary>
    /// 添加聊天
    /// </summary>
    void AddChat(ChatPoolDT tmp)
    {
        Chat_LabelEmoji.Add(tmp.m_Char, (EM_ChatChan)tmp.m_Chan, Data_Pool.m_UserData.m_iUserId == tmp.m_Id ? null : tmp);
        Data_Pool.m_ChatPool.lRecord.Add((int)(tmp.iId));
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        Data_Pool.m_ChatPool.isUIShow = false;
        CurrentPrivateData = default(PrivateData);

    }
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("Back", UI_ExitBtn);
        f_RegClickEvent("MaskClose", UI_ExitBtn);
        f_RegClickEvent("WorldBtn", UpdateWorld);
        f_RegClickEvent("SystemBtn", UpdateSystem);
        f_RegClickEvent("GroupBtn", UpdateGroup);
        f_RegClickEvent("PrivateBtn", UpdatePrivate);
        f_RegClickEvent("PrivateBtn", UpdatePrivate);
        f_RegClickEvent("BtnPrivateBack", OnPrivateBack);
        f_RegClickEvent("EmojiBtn", UI_OpenEmoji);
        f_RegClickEvent("Send", Chat_Send);

        f_RegClickEvent("SeleEmoji", UI_CloseEmoji);


        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.RECEIVEMESSAGE, ReceiveMessage, this);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_ChatRoomUpdate, ShowUIPrivate, this);
    }
    private void OnPrivateBack(GameObject gameObject_0, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ChatManage, UIMessageDef.UI_OPEN, new PrivateData() { UserTarget = 0, RoomTarget = 0 });
    }


    #region 红点提示
    protected override void InitRaddot()
    {
        base.InitRaddot();
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.ChatWordNewMsg, f_GetObject("WorldBtn"), ReddotCallback_Show_Btn_World);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.ChatLegionNewMsg, f_GetObject("GroupBtn"), ReddotCallback_Show_Btn_Group);
        Data_Pool.m_ReddotMessagePool.f_Reg(EM_ReddotMsgType.ChatPrivateNewMsg, f_GetObject("PrivateBtn"), ReddotCallback_Show_Btn_Private);
        UpdateReddotUI();
    }
    protected override void On_Destory()
    {
        base.On_Destory();
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.RECEIVEMESSAGE);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_ChatRoomUpdate);
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.ChatWordNewMsg, f_GetObject("WorldBtn"));
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.ChatLegionNewMsg, f_GetObject("GroupBtn"));
        Data_Pool.m_ReddotMessagePool.f_UnReg(EM_ReddotMsgType.ChatPrivateNewMsg, f_GetObject("PrivateBtn"));
    }
    protected override void UpdateReddotUI()
    {
        base.UpdateReddotUI();
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.ChatWordNewMsg);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.ChatLegionNewMsg);
        Data_Pool.m_ReddotMessagePool.f_ForceUpdateUI(EM_ReddotMsgType.ChatPrivateNewMsg);
    }
    private void ReddotCallback_Show_Btn_World(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnWorld = f_GetObject("WorldBtn");
        UITool.f_UpdateReddot(BtnWorld, iNum, new Vector3(92, -20, 0), 2500);
    }
    private void ReddotCallback_Show_Btn_Group(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtnGroup = f_GetObject("GroupBtn");
        UITool.f_UpdateReddot(BtnGroup, iNum, new Vector3(92, -20, 0), 2500);
    }
    private void ReddotCallback_Show_Btn_Private(object Obj)
    {
        int iNum = (int)Obj;
        GameObject BtPrivate = f_GetObject("PrivateBtn");
        UITool.f_UpdateReddot(BtPrivate, iNum, new Vector3(92, -20, 0), 2500);
    }
    #endregion
    #region UI
    //Transform ChatParent;
    //Transform Chat;
    //UI2DSprite Chat_Icon;
    //UILabel Chat_Name;
    ////UILabel Chat_Vip;
    //UILabel Chat_Chat;
    //UIInput LabelInput;
    GameObject Chat;
    UI2DSprite Chat_Icon;
    UILabel Chat_Vip;
    UILabel Chat_Name;
    UILabel Chat_Label;
    UIInput Chat_Input;
    EM_ChatChan ChatType;
    UIAtlas EmojiAtlas;
    int ChatNum;
    BetterList<string> EmojiData;

    EmojiExtension Chat_LabelEmoji;
    #endregion
    /// <summary>
    /// 初始化
    /// </summary>
    private void Init()
    {
        Chat = f_GetObject("Chat");
        Chat_Input = f_GetObject("Input").GetComponent<UIInput>();
        Chat_LabelEmoji = f_GetObject("Chat").GetComponent<EmojiExtension>();
        EmojiAtlas = f_GetObject("Emoji").GetComponent<UIAtlas>();
        ChatNum = Data_Pool.m_ChatPool.f_GetAll().Count;
        ChatType = EM_ChatChan.eChan_World;
        if (EmojiData == null)
        {
            EmojiData = EmojiAtlas.GetListOfSprites();
            for (int EmojiIndex = 0; EmojiIndex < EmojiData.size; EmojiIndex++)
            {
                if (EmojiData[EmojiIndex].Length >= 4)
                    continue;
                GameObject Emoji = Instantiate(f_GetObject("EmojiSprite"));
                Emoji.SetActive(true);
                Emoji.transform.parent = f_GetObject("EmojiParent").transform;
                Emoji.transform.localScale = Vector3.one;
                Emoji.GetComponent<UISprite>().spriteName = EmojiData[EmojiIndex];
                f_RegClickEvent(Emoji, AddEmoji, EmojiData[EmojiIndex]);
            }
        }
        f_GetObject("EmojiParent").GetComponent<UIGrid>().enabled = true;
    }
    private void ShowAllChat()
    {
        for (int index = 0; index < Data_Pool.m_ChatPool.f_GetAll().Count; index++)
        {
            if (Data_Pool.m_ChatPool.lRecord.Contains((int)(((ChatPoolDT)(Data_Pool.m_ChatPool.f_GetAll()[index])).iId)))
                continue;
            AddChat((ChatPoolDT)(Data_Pool.m_ChatPool.f_GetAll()[index]));
        }
    }
    #region   UI事件
    void AddEmoji(GameObject go, object obj1, object obj2)
    {
        string name = (string)obj1;
        Chat_Input.value += "#" + name + "@";
        f_GetObject("SeleEmoji").SetActive(false);
    }
    private void UI_ExitBtn(GameObject go, object obj1, object obj2)
    {
        //通知HoldPool弹出当前页
        //ccUIHoldPool.GetInstance().f_UnHold();
        //关闭自身
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ChatManage, UIMessageDef.UI_CLOSE);
    }
    private void UI_Mail(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ChatManage, UIMessageDef.UI_OPEN);
    }
    void UI_OpenEmoji(GameObject go, object obj1, object obj2)
    {
        f_GetObject("SeleEmoji").SetActive(true);
    }
    void UI_CloseEmoji(GameObject go, object obj1, object obj2)
    {
        f_GetObject("SeleEmoji").SetActive(false);
    }
    void Chat_Send(GameObject go, object obj1, object obj2)
    {
        string content = Chat_Input.value;
        if (Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Level) < 20)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1147));
            return;
        }

        if (content.Length == 0)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1148));
            return;
        }
        //else if (content.Contains("@") && content.Contains("#"))
        //{
        //    MessageBox.DEBUG(CommonTools.f_GetTransLanguage(1149));
        //}
        else if (!Data_Pool.m_BlockWordPool.f_CheckValidity(ref content))
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1150));
            Chat_Input.value = content;
            return;
        }
        else if (ccMath.f_GetStringBytesLength(content) > 80)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1151));
            return;
        }
        else if (glo_Main.GetInstance().m_CmdController.f_CheckAndExecuteCmd(content.Trim()))
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1152));
            return;
        }
        else if (AdministratorTools.f_ChatMsg(content, null))
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.ChatManage, UIMessageDef.UI_CLOSE);
            return;
        }
        else if (!bCanSend)
        {
            UITool.Ui_Trip(string.Format(CommonTools.f_GetTransLanguage(1153), ChatTimeMax));
            return;
        }

        Chat_LabelEmoji.Add(content, ChatType);
        switch (ChatType)
        {
            case EM_ChatChan.eChan_World:
                ChatSocket.GetInstance().f_SendMessage(0, ChatType, content);
                break;
            case EM_ChatChan.eChan_Legion:
                ChatSocket.GetInstance().f_SendMessage(0, ChatType, content);
                break;
            case EM_ChatChan.eChan_Team:
            case EM_ChatChan.eChan_Private:
                ChatSocket.GetInstance().f_SendMessage(CurrentPrivateData.UserTarget, ChatType, content, CurrentPrivateData.RoomTarget);
                break;
        }

        if (bCanSend)
        {
            Invoke("_ChatTime", 10f);
            bCanSend = false;
        }
        //Time_ChatTime = ccTimeEvent.GetInstance().f_RegEvent(0.2f , true , null , _ChatTime);
        Chat_Input.value = null;
    }

    //Chat_LabelEmoji.Add("", Chat_Input.value, ChatType);
    void AddText(string text, EM_ChatChan Chat)
    {
        Chat_LabelEmoji.Add(text, Chat);
    }
    private void UpdateSystem(GameObject gameObject_0, object obj1, object obj2)
    {
        ChatType = EM_ChatChan.eChan_System;
        f_GetObject("control").SetActive(false);
    }
    void UpdateWorld(GameObject go, object obj1, object obj2)
    {
        ChatType = EM_ChatChan.eChan_World;
        f_GetObject("control").SetActive(true);
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.ChatWordNewMsg);
    }
    void UpdateGroup(GameObject go, object obj1, object obj2)
    {
        if (LegionMain.GetInstance().m_LegionInfor.m_iLegionId == 0)
        {
            f_GetObject("WorldBtn").GetComponent<UIToggle>().value = true;
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(2257));
            return;
        }
        ChatType = EM_ChatChan.eChan_Legion;
        f_GetObject("control").SetActive(true);
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.ChatLegionNewMsg);
    }
    void UpdatePrivate(GameObject go, object obj1, object obj2)
    {
        ChatType = EM_ChatChan.eChan_Private;
        Data_Pool.m_ReddotMessagePool.f_MsgReset(EM_ReddotMsgType.ChatPrivateNewMsg);
        f_GetObject("control").SetActive(true);
        ShowUIPrivate();
    }
    #endregion

    public void _ChatTime()
    {
        bCanSend = true;
        //        ChatTimeNow += 0.2f;
        //
        //
        //        if (ChatTimeNow >= ChatTimeMax)
        //        {
        //            ChatTimeNow = 0f;
        //            ccTimeEvent.GetInstance().f_UnRegEvent(Time_ChatTime);
        //        }
    }

}