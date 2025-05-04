using UnityEngine;
using System.Collections;
using ccU3DEngine;

public class GameNoticePage : UIFramwork
{
    private GameNoticePoolDT _GameNoticePoolDT;

    private GameObject _Mask;   //锁屏
    private GameObject _BtnSuc;
    private GameObject _BtnExit;

    private UISprite _BtnSucSprite;
    private BoxCollider _BtnSucBoxCollider;

    private UILabel _Title;
    private UILabel _LabelContent;
    private UILabel _SucLabel;

    private UIScrollView _TextScrollView;

    private int _StarTime;
    private int _OverTime;
    private int _ShowTime;
    private int Time_UpdateBtnSuc;
    private int _Time;

    private bool _isGray;

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        _GameNoticePoolDT = (GameNoticePoolDT)e;

        f_Init();

        _StarTime = _GameNoticePoolDT.m_iStarTime;
        _OverTime = _GameNoticePoolDT.m_iEndTime;
        _Time = GameSocket.GetInstance().f_GetServerTime();
        if (_StarTime > _Time)
        {
            _CloseThis(null, null, null);
            return;
        }
        if (_OverTime <= _Time)
        {
            _CloseThis(null, null, null);
            return;
        }
        Data_Pool.m_GuidancePool.m_NowOpenUIName = UINameConst.GameNoticePage;
        //if (Data_Pool.m_GuidancePool.IsEnter)
        //{
        //    Data_Pool.m_GuidancePool.f_SetCurClickButtonNull();
        //}

        _UpdateMain();
    }

    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BtnExit", _ExitGame);
        f_RegClickEvent("BtnSuc", _CloseThis);
        f_RegClickEvent("BtnBlack", _CloseThis);
    }

    #region 按钮消息
    private void _ExitGame(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_SDKCmponent.f_Exit();    
    }

    private void _CloseThis(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GameNoticePage, UIMessageDef.UI_CLOSE);
        ccTimeEvent.GetInstance().f_UnRegEvent(Time_UpdateBtnSuc);
        Data_Pool.m_GameNoticePool._StarTime();
        ccTimeEvent.GetInstance().f_UnRegEvent(Time_UpdateBtnSuc);
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        Data_Pool.m_GuidancePool.f_SetCurClickButton(Data_Pool.m_GuidancePool.GuidanceBtnName, Data_Pool.m_GuidancePool.m_GuidanceCallback);
    }
    #endregion


    private void f_Init()
    {
        _Mask = f_GetObject("ShadeMask");
        _BtnSuc = f_GetObject("BtnSuc");
        _BtnExit = f_GetObject("BtnExit");

        _Title = f_GetObject("Title").GetComponent<UILabel>();
        _LabelContent = f_GetObject("LabelContent").GetComponent<UILabel>();
        _SucLabel = f_GetObject("SucLabel").GetComponent<UILabel>();

        _BtnSucSprite = _BtnSuc.GetComponent<UISprite>();
        _BtnSucBoxCollider = _BtnSuc.GetComponent<BoxCollider>();

        _TextScrollView = f_GetObject("ScrollView").GetComponent<UIScrollView>();
    }

    private void _UpdateMain()
    {
        _Mask.SetActive(_GameNoticePoolDT.m_iIsLockGame == 1 || _GameNoticePoolDT.m_iQuitGame == 1);
        if (_GameNoticePoolDT.m_szTitle != null)
            _Title.text = _GameNoticePoolDT.m_szTitle;
        _LabelContent.text = _GameNoticePoolDT.m_szContext;
        _TextScrollView.ResetPosition();

        bool IsShowExitBtn = _GameNoticePoolDT.m_iQuitGame == 1;

        _BtnExit.SetActive(IsShowExitBtn);
        _BtnSuc.SetActive(!IsShowExitBtn);

        if (!IsShowExitBtn)
        {
            Time_UpdateBtnSuc = ccTimeEvent.GetInstance().f_RegEvent(0.5f, true, null, _UpdateTime);
        }
    }

    private void _UpdateTime(object obj)
    {
        _ShowTime = _OverTime - GameSocket.GetInstance().f_GetServerTime();
        _isGray = _ShowTime <= 0;

        UITool.f_SetSpriteGray(_BtnSucSprite, !_isGray);

        _BtnSucBoxCollider.enabled = _isGray;

        if (_isGray)
        {
            _Mask.SetActive(false);
_SucLabel.text = "Confirm";
            ccTimeEvent.GetInstance().f_UnRegEvent(Time_UpdateBtnSuc);
        }
        else
            _SucLabel.text = _ShowTime.ToString();
    }

}
