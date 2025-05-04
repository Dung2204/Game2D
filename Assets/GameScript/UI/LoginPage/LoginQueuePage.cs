using ccU3DEngine;
using UnityEngine;

public enum LoginQueueType
{
    LoginQueueType_Close     = -1,   //关闭排队界面
    LoginQueueType_FakeQueue = 0,    //打开假排队界面
    LoginQueueType_Queue     = 1,    //打开排队界面
}

public class LoginQueuePage : UIFramwork
{
    private UILabel mlabelQueueTips;  //排队提示
    private UILabel mlabelQueuePos;   //排队位置
    private UILabel mlabelWaitTime;   //等待时间
    private int fakeWaitPos = 0;
    private int timeEventId = 0;

    private const int waitTimeforOnePeople = 5;   //服务器每一位排队玩家需要的等待时间（分）
    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mlabelQueueTips = f_GetObject("Label_QueueTips").GetComponent<UILabel>();
        mlabelQueuePos  = f_GetObject("Label_Pos").GetComponent<UILabel>();
        mlabelWaitTime  = f_GetObject("Label_WaitTime").GetComponent<UILabel>();

        f_RegClickEvent("BtnCancel", OnCancle);
    }


    /// <summary>
    /// 打开界面
    /// </summary>
    /// <param name="e">EquipSythesis类似</param>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        mlabelQueueTips.text = string.Format(CommonTools.f_GetTransLanguage(2189), Data_Pool.m_UserData.m_strServerName);
        if (timeEventId > 0)
        ccTimeEvent.GetInstance().f_UnRegEvent(timeEventId);

        //服务器排队位置        
        int nWaitPos = 0;
        if (null != e && e is int)
            nWaitPos = (int)e;
        if (nWaitPos > 0)
        {
            //预计等待时间，假时间，每一位玩家估计等待时间为5分钟，每减少一位玩家减少5分钟
            mlabelQueuePos.text = CommonTools.f_GetTransLanguage(2190) + nWaitPos;
            mlabelWaitTime.text = string.Format(CommonTools.f_GetTransLanguage(2191), nWaitPos * waitTimeforOnePeople);
        }
        else
        {
            //排队人数默认为100，每10秒减少一位玩家
            if(fakeWaitPos <= 0)
                fakeWaitPos = 100;
            mlabelQueuePos.text = CommonTools.f_GetTransLanguage(2190) + fakeWaitPos;
            mlabelWaitTime.text = string.Format(CommonTools.f_GetTransLanguage(2191), fakeWaitPos / 6 + 1);
            timeEventId = ccTimeEvent.GetInstance().f_RegEvent(10, true, null, (object obj) => {
                if (!ccUIManage.GetInstance().f_CheckUIIsOpen(UINameConst.LoginQueuePage))
                {
                    ccTimeEvent.GetInstance().f_UnRegEvent(timeEventId);
                    return;
                }
                fakeWaitPos--;
                if (fakeWaitPos <= 0)
                {
                    //减到0了还没连接上，则弹出网络连接异常，请检查网络提示
                    ccUIManage.GetInstance().f_SendMsg(UINameConst.PopupMenuPage, UIMessageDef.UI_OPEN, new PopupMenuParams(CommonTools.f_GetTransLanguage(1847), CommonTools.f_GetTransLanguage(2201)));
                    OnCancle(null, null, null);
                    return;
                }
                else
                {
                    mlabelQueuePos.text = CommonTools.f_GetTransLanguage(2190) + fakeWaitPos;
                    mlabelWaitTime.text = string.Format(CommonTools.f_GetTransLanguage(2191), fakeWaitPos / 6);
                }                               
            });
        }
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }

    private void OnCancle(GameObject go, object value1, object value2)
    {
        StaticValue.m_IsCancelQueue = true;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LoginQueuePage, UIMessageDef.UI_CLOSE);
        GameSocket.GetInstance().f_Close();
    }
}