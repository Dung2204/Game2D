using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;

/// <summary>
/// 服务器组数据
/// 以前是单条服务器数据作为一个选项卡拖动，，现在改为1-10作为一组，然后点击它显示10个服务器列表，，所以现在将10个合为一个数据结构
/// </summary>
public class ServerGroupData: BasePoolDT<long>
{
    public string mGroupName;
    public List<NBaseSCDT> mListServer = new List<NBaseSCDT>();
}

/// <summary>
/// 选择服务器界面
/// </summary>
public class SelectServerPage : UIFramwork {
    private const int serverCountInGroup = 10;  //一组服务器个数
    private List<BasePoolDT<long>> listSelectServerData = new List<BasePoolDT<long>>();
    public static ServerInforDT mNearSelectServer1 = null;//登录时检测有没有最近登录的服务器
    public static ServerInforDT mNearSelectServer2 = null;//登录时检测有没有最近登录的服务器2
    private int selectServerIndex = 0;
    private string[] m_ServerStateName;
    private string[] m_ServerStateIcon;

    private UIGrid gridServerRoot;//全部服务器根节点
    private GameObject exampleServerItem;//服务器item
    private GameObject btnBlack;//选择服务器界面黑色背景
    private UIWrapComponent _contentWrapComponet = null;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        BindUI();
    }

    /// <summary>
    /// 注册UI事件
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent(btnBlack, OnBtnBlackClick);
    }
    /// <summary>
    /// 绑定UI
    /// </summary>
    private void BindUI()
    {
		CheckNet();
        m_ServerStateName = new string[(int)EM_ServerState.Max];
        m_ServerStateName[(int)EM_ServerState.New] = CommonTools.f_GetTransLanguage(2209);
        m_ServerStateName[(int)EM_ServerState.Unhindered] = CommonTools.f_GetTransLanguage(2210);
        m_ServerStateName[(int)EM_ServerState.Hot] = CommonTools.f_GetTransLanguage(2211);
        m_ServerStateName[(int)EM_ServerState.Maintain] = CommonTools.f_GetTransLanguage(2212);

        m_ServerStateIcon = new string[(int)EM_ServerState.Max];
        m_ServerStateIcon[(int)EM_ServerState.New] = "Border_jrdk1";
        m_ServerStateIcon[(int)EM_ServerState.Unhindered] = "Border_jrdk1";
        m_ServerStateIcon[(int)EM_ServerState.Hot] = "Border_jrdk2";
        m_ServerStateIcon[(int)EM_ServerState.Maintain] = "Border_jrdk3";

        exampleServerItem = f_GetObject("ServerItem");
        gridServerRoot = f_GetObject("ItemGrid").GetComponent<UIGrid>();
        for (int i = 0; i < serverCountInGroup; i++)
        {
            NGUITools.AddChild(gridServerRoot.gameObject, exampleServerItem);
        }       

        btnBlack = f_GetObject("BtnBlack");
        btnBlack = f_GetObject("Close");
    }
    /// <summary>
    /// 页面开启
    /// </summary>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        listSelectServerData.Clear();

        //添加最近玩过得服务器数据
        selectServerIndex = -1;
        if (mNearSelectServer1 != null || mNearSelectServer2 != null)
        {
            ServerGroupData myLoginedServerData = new ServerGroupData();
            myLoginedServerData.mGroupName = CommonTools.f_GetTransLanguage(2213);
            if (mNearSelectServer1 != null)
            {
                myLoginedServerData.mListServer.Add(mNearSelectServer1);
            }
            if (mNearSelectServer2 != null)
            {
                myLoginedServerData.mListServer.Add(mNearSelectServer2);
            }
            listSelectServerData.Add(myLoginedServerData);
            selectServerIndex = 0;
        }

        //添加服务器列表数据
        string chanelName = glo_Main.GetInstance().m_SDKCmponent.f_GetSdkChannelType();
        int index = 0;
        List<NBaseSCDT> serverInfoList = glo_Main.GetInstance().m_SC_Pool.m_ServerInforSC.f_GetAll();
        ServerGroupData serverGroupData = null;
        for (int i = 0; i < serverInfoList.Count; i++)
        {
            ServerInforDT dt = serverInfoList[i] as ServerInforDT;
            if (null == dt) continue;
            List<string> canNotLoginChanel = new List<string>(dt.szLockChanel.Split(';'));
            if (canNotLoginChanel.Contains(chanelName))
                continue;
            if (dt.szAutoOpenTime != null && dt.szAutoOpenTime != "" && !CommonTools.f_CheckServerTimeOpen(dt.szAutoOpenTime))
                continue;

            //去除未开放的
            if (dt.iServState != (int)EM_ServerState.UnOpen)
            {
                if (index % serverCountInGroup == 0)
                {
                    //serverCountInGroup个为一组
                    serverGroupData = new ServerGroupData();
                    int groupIndex = index / serverCountInGroup;
                    serverGroupData.mGroupName = string.Format(CommonTools.f_GetTransLanguage(2214), groupIndex * serverCountInGroup + 1, (groupIndex + 1) * serverCountInGroup);
                    listSelectServerData.Add(serverGroupData);
                }
                index++;
                serverGroupData.mListServer.Add(dt);
            }
        }

        //更新滑动面板
        GameObject serverGroupRoot = f_GetObject("ItemRoot");
        if (_contentWrapComponet == null)
        {
            _contentWrapComponet = new UIWrapComponent(84, 1, 600, 7, serverGroupRoot, f_GetObject("BtnTab"), listSelectServerData, OnShowServerGroupData, null);
        }
        _contentWrapComponet.f_ResetView();
        _contentWrapComponet.f_ViewGotoRealIdx(selectServerIndex, 8);

        //默认选中最近玩过地或者最后一个服务器组  
        GameObject selectItem = null;
        selectServerIndex = selectServerIndex >= 0 ? selectServerIndex : listSelectServerData.Count - 1;
        ServerGroupData selectServerGroupData = listSelectServerData[selectServerIndex] as ServerGroupData;
        for (int i = 0; i < serverGroupRoot.transform.childCount; i++)
        {
            Transform child = serverGroupRoot.transform.GetChild(i);
            UILabel labelGroupName = child.Find("Normal/Label").GetComponent<UILabel>();
            if (null == labelGroupName) continue;
            if (labelGroupName.text == selectServerGroupData.mGroupName)
            {
                selectItem = child.gameObject;
                break;
            }
        }

        OnServerGroupTabClick(selectItem, selectServerGroupData, null);
    }

    /// <summary>
    /// 初始化服务器列表信息
    /// </summary>
	void CheckNet()
	{
		// MessageBox.ASSERT("Rớt mạng");
		switch (Application.internetReachability)
		{
			case NetworkReachability.NotReachable:
TopPopupMenuParams param = new TopPopupMenuParams("Thông báo", "Kết nối mạng không ổn định, vui lòng kiểm tra lại.", "Xác nhận", TTTT);
				ccUIManage.GetInstance().f_SendMsg(UINameConst.TopPopupMenuPage, UIMessageDef.UI_OPEN, param);
				// ccUIManage.GetInstance().f_SendMsg(UINameConst.NetworkErrorPage, UIMessageDef.UI_OPEN);
				break;
		}
	}
	
	private void TTTT(object Obj)
    {

    }
	
    public void InitListServerInfoData()
    {
        
    }
    private Transform mLastSelectObj;
    /// <summary>
    /// 显示Item回调数据回调
    /// </summary>
    /// <param name="go">item物体</param>
    /// <param name="data">数据</param>
    private void OnShowServerGroupData(Transform item, BasePoolDT<long> data)
    {
        ServerGroupData serverData = data as ServerGroupData;
        if (null == serverData) return;
        bool isSelect = mLastSelectObj == item;
        item.Find("Normal").gameObject.SetActive(!isSelect);
        item.Find("Press").gameObject.SetActive(isSelect);
        UILabel labelNormalGroupName = item.Find("Normal/Label").GetComponent<UILabel>();
        labelNormalGroupName.text = serverData.mGroupName;
        UILabel labelSelectGroupName = item.Find("Press/Label").GetComponent<UILabel>();
        labelSelectGroupName.text = serverData.mGroupName;
        f_RegClickEvent(item.gameObject, OnServerGroupTabClick, data);
    }

    /// <summary>
    /// 服务器组选项卡点击事件
    /// </summary>
    /// <param name="go"></param>
    /// <param name="obj1"></param>
    /// <param name="obj2"></param>
    private void OnServerGroupTabClick(GameObject go, object obj1, object obj2)
    {
        CheckNet();
        ServerGroupData serverGroupData = obj1 as ServerGroupData;

        //设置选中状态
        if (null != mLastSelectObj)
        {
            mLastSelectObj.Find("Normal").gameObject.SetActive(true);
            mLastSelectObj.Find("Press").gameObject.SetActive(false);
        }
        if (null != go)
        {
            go.transform.Find("Normal").gameObject.SetActive(false);
            go.transform.Find("Press").gameObject.SetActive(true);
            mLastSelectObj = null;
            mLastSelectObj = go.transform;
        }
               

        //更新列表数据
        for (int i = 0; i < gridServerRoot.transform.childCount; i++)
        {
            Transform serverItem = gridServerRoot.transform.GetChild(i);
            if (i < serverGroupData.mListServer.Count)
            {
                serverItem.gameObject.SetActive(true);                
                UILabel labelServerName = serverItem.Find("LabelServerName").GetComponent<UILabel>();
                UISprite spriteServerState = serverItem.Find("SprState").GetComponent<UISprite>();
                UILabel labelServerState = serverItem.Find("LabelServerState").GetComponent<UILabel>();

                ServerInforDT serverInfo = serverGroupData.mListServer[i] as ServerInforDT;
                f_RegClickEvent(serverItem.gameObject, OnServerItemClick, serverInfo);
                EM_ServerState serverState = UITool.f_GetServerState(serverInfo);
                labelServerName.text = serverInfo.szName;
                labelServerState.text = m_ServerStateName[(int)serverState];
                spriteServerState.spriteName = m_ServerStateIcon[(int)serverState];
            }
            else
            {
                serverItem.gameObject.SetActive(false);
            }
        }
        gridServerRoot.repositionNow = true;
    }

    /// <summary>
    /// 点击了server item按钮
    /// </summary>
    private void OnServerItemClick(GameObject go, object obj1, object obj2)
    {
		CheckNet();
        ServerInforDT dt = (ServerInforDT)obj1;
        if (UITool.f_GetServerState(dt) == EM_ServerState.Preheat)
        {
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Máy chủ sẽ sớm mở, hãy đợi");
            return;
        }
        GloData.glo_strSvrIP = dt.szIP;
        GloData.glo_iSvrPort = dt.iPort;
        Data_Pool.m_UserData.f_SetServerInfo(dt.iServerId, dt.szName);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.SelectServerPage, UIMessageDef.UI_CLOSE);
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_LOGINSERVERNAME, dt);
    }

    /// <summary>
    /// 点击了选择服务器页面黑色背景
    /// </summary>
    private void OnBtnBlackClick(GameObject go, object obj1, object obj2)
    {
		CheckNet();
        ccUIManage.GetInstance().f_SendMsg(UINameConst.SelectServerPage, UIMessageDef.UI_CLOSE);
    }
}
