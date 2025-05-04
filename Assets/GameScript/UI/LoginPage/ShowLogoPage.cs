using UnityEngine;
using System.Collections;
using ccU3DEngine;
/// <summary>
/// 展示公司Logo页面
/// </summary>
public class ShowLogoPage : UIFramwork {
    private UISprite m_texLogo;//公司Logo图片
    private float m_timetick;//时间控制
    private bool isPageAct = true;//当前页面是否激活状态
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("LogPWBtn1", f_InuptShowLogPW, 1);
        f_RegClickEvent("LogPWBtn2", f_InuptShowLogPW, 2);
        f_RegClickEvent("LogPWBtn3", f_InuptShowLogPW, 3);
        f_RegClickEvent("LogPWBtn4", f_InuptShowLogPW, 4);
        //是否打开Log
        bool tBShowLog = LocalDataManager.f_GetLocalDataIfNotExitSetData<bool>(LocalDataType.Bool_ShowLog, true);
        if (tBShowLog)
        {
            glo_Main.GetInstance().f_StartLog();
            //UITool.Ui_Trip("打开Log");
        }
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.LoginBg);
		StaticValue.m_MusicName = "LoginBG";
    }
    /// <summary>
    /// 页面开启
    /// </summary>
    /// <param name="e"></param>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
#if W2
        //f_GetObject("GridBg").SetActive(false);
#endif
        
        CheckNet();
        LoadRes();
        
    }
    /// <summary>
    /// 页面关闭
    /// </summary>
    protected override void UI_CLOSE(object e)
    {
        _NeedCloseSound = false;
        base.UI_CLOSE(e);
        isPageAct = false;
    }
    /// <summary>
    /// 加载Logo图
    /// </summary>
    private void LoadRes()
    {
        m_texLogo = f_GetObject("GridBg").GetComponent<UISprite>();
        m_texLogo.color = new Color(1, 1, 1, 1);
    }

    /// <summary>
    /// 检测网络是否连通
    /// </summary>
    /// <returns></returns>
    void CheckNet()
    {
        switch (Application.internetReachability)
        {
            case NetworkReachability.NotReachable:
                // ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Không thể kết nối đến server, hãy kiểm tra lại đường truyền！");
				glo_Main.GetInstance().m_GameMessagePool.f_Broadcast(MessageDef.OnLoginQueueEvent, LoginQueueType.LoginQueueType_FakeQueue);
                break;
        }
    }
    /// <summary>
    /// 控制Logo图片（显示完logo图显示登录界面）
    /// </summary>
    protected override void f_Update()
    {
        base.f_Update();
        if (!isPageAct)
            return;
        m_timetick += Time.deltaTime;
        if (m_timetick >= 1)
        {
            isPageAct = false;
            
            ccUIManage.GetInstance().f_SendMsg(UINameConst.ShowLogoPage, UIMessageDef.UI_CLOSE);
			ccUIManage.GetInstance().f_SendMsg(UINameConst.ResLoadPage, UIMessageDef.UI_OPEN, false);
		}
    }
    #region 密码开启Log部分

    private const string TrueShowLogPW = "132431";
    private const string ConstShowLogFlag = "ShowLogFlag";

    private string showLogPW = string.Empty;

    public void f_InuptShowLogPW(GameObject go, object value1, object value2)
    {
        int tIdx = (int)value1;
        showLogPW += tIdx.ToString();
        if (showLogPW == TrueShowLogPW)
        {
            showLogPW = string.Empty;
            bool tBShowLog = LocalDataManager.f_GetLocalDataIfNotExitSetData<bool>(LocalDataType.Bool_ShowLog, false);
            if (tBShowLog)
            {
                LocalDataManager.f_SetLocalData<bool>(LocalDataType.Bool_ShowLog, false);
                UITool.Ui_Trip("Tắt Nhật ký");
            }
            else
            {
                LocalDataManager.f_SetLocalData<bool>(LocalDataType.Bool_ShowLog, true);
                UITool.Ui_Trip("Bật Nhật ký");
                glo_Main.GetInstance().f_StartLog();
            }
        }
    }

    #endregion
}
