using ccU3DEngine;
using UnityEngine;
class GuidanceProcess : ccMachineStateBase
{
    private int _iCreateArrBtn = -99;
    private GuidanceDT _GuidanceDt;
    private GuidanceManage tManage;
    private ccUIBase _NowUi;
    private string[] _UiNameAndBtnName;
    private GameObject _tArr;
    private UIScrollView _Scrollview;
    float[] posFloat = new float[2];

    private float idleTime = 10f;
    private float NowTime = 0f;

    //private int CheckUiID = 90000;
    private GuidanceDialogDT tCheckUiDt;
    public GuidanceProcess(GuidanceManage tManage) : base((int)EM_Guidance.GuidancePlay)
    {
        this.tManage = tManage;
        _UiNameAndBtnName = new string[3];
    }
    public override void f_Enter(object Obj)
    {
        base.f_Enter(Obj);
        idleTime = Data_Pool.m_GuidancePool.Is_AllEscTime;
        NowTime = 0f;
        _GuidanceDt = (GuidanceDT)Obj;
        _UiNameAndBtnName = _GuidanceDt.szBtnName.Split(';');

        if (_GuidanceDt.szArrPos != "0")
            posFloat = ccMath.f_String2ArrayFloat(_GuidanceDt.szArrPos, ",");
        else
        {
            posFloat[0] = 0;
            posFloat[1] = 0;
        }
Debug.LogError("Indication Id： " + _GuidanceDt.iId);
        //TsuCode - tracking Tân thủ
        GameParamDT param = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(109);
        if (param != null)
        {
            int idTuttorial_Completion = param.iParam1;
            if(idTuttorial_Completion == _GuidanceDt.iId)
            {
                glo_Main.GetInstance().m_SDKCmponent.f_Tutorial_Completion(); //TsuCode - tracking tân thủ kết thúc - tutorital  completion
            }
        }
        //
        if (_iCreateArrBtn == -99)
        {
            _iCreateArrBtn = ccTimeEvent.GetInstance().f_RegEvent(0.17f, true, null, CreateArrBtn);
        }
    }

    private void CreateArrBtn(object Obj)
    {
        _NowUi = ccUIManage.GetInstance().f_GetUIHandler(_UiNameAndBtnName[0]);

        if (_NowUi == null)
        {
            return;
        }
        else
        {
            if (_GuidanceDt.iId == 25)
            {
                _DelaySetBtn(1f);
                return;
            }
            else if (_GuidanceDt.iId == 2004)
            {
                if (Data_Pool.m_GuidancePool.IsUpdateArena)
                {
                    _DelaySetBtn(0.1f);
                }
                else
                    return;
            }
            else if (_GuidanceDt.iId == 2014)
            {
                if (Data_Pool.m_GuidancePool.IsOpponent)
                {
                    ccTimeEvent.GetInstance().f_UnRegEvent(_iCreateArrBtn);
                    _iCreateArrBtn = -99;
                }
                else
                    return;
            }
            else if (_GuidanceDt.iId == 2008)
            {
                _DelaySetBtn(0.5f);
            }
            ccTimeEvent.GetInstance().f_UnRegEvent(_iCreateArrBtn);
            _iCreateArrBtn = -99;
        }
        _SetBtn(null);
        MessageBox.DEBUG("Đã set button:" + _UiNameAndBtnName[2] + "    ID của DT" + _GuidanceDt.iId);
MessageBox.DEBUG("Current Process Status" + Data_Pool.m_GuidancePool.m_GuidanceType.ToString());

    }

    private int delayId = 0;
    void _DelaySetBtn(float time)
    {
        ccTimeEvent.GetInstance().f_UnRegEvent(_iCreateArrBtn);
        _iCreateArrBtn = -99;
        ccTimeEvent.GetInstance().f_RegEvent(time, false, null, _SetBtn);
    }

    private GameObject curBtn = null;
    void _SetBtn(object boj)
    {
Debug.LogError("Button pressed： "+_UiNameAndBtnName[1]);
        Transform btn = _NowUi.transform.Find("Panel/" + _UiNameAndBtnName[1]);
        if (!btn)
        {
            return;
        }
        curBtn = _NowUi.transform.Find("Panel/" + _UiNameAndBtnName[1]).gameObject;
        _tArr = tManage.f_CreateArr(curBtn, new Vector2(posFloat[0], posFloat[1]), 1, _GuidanceDt.szArrPos != "0", _GuidanceDt.iParticle != 0, ref _Scrollview, _GuidanceDt.szText);
        //要点击按钮名字修改一下，防止同名的按钮可以点击
        curBtn.name = _UiNameAndBtnName[2] + "_guidance";
        Data_Pool.m_GuidancePool.f_SetCurClickButton(_UiNameAndBtnName[2] + "_guidance", f_Change);
        //如果已经找到了按钮就取消
        //ccTimeEvent.GetInstance().f_UnRegEvent(delayId);
        //_iCreateArrBtn = -99;
    }
    void f_Change(object obj)
    {
        if (_Scrollview != null)
            _Scrollview.enabled = true;

        Data_Pool.m_GuidancePool.IGuidanceID++;
        Data_Pool.m_GuidancePool.f_SaveNewbie(_GuidanceDt);
        //名字修改回去以防有其他问题
        if (curBtn && curBtn.name == (string)obj)
        {
            curBtn.name = _UiNameAndBtnName[2];
        }
        if (_UiNameAndBtnName[2] == "Btn_Challenges" || _UiNameAndBtnName[2] == "ChallengeBtn" ||
            _UiNameAndBtnName[2] == "ClickItem" || _UiNameAndBtnName[2] == " BtnGrab" ||
            (_UiNameAndBtnName[2] == "BtnChallenge" && (_UiNameAndBtnName[0] == "RunningManChallengePage" || _UiNameAndBtnName[0] == "PatrolLandPage"))
            || _UiNameAndBtnName[2] == "Btn_Common" || (_UiNameAndBtnName[0] == "ArenaPageNew" && _UiNameAndBtnName[2] == "RoleParent"))    //挑战场景需要设置为NU
        {
            Data_Pool.m_GuidancePool.f_SetCurClickButtonNull();
            f_SetComplete((int)EM_Guidance.GuidanceEnd);
        }
        else
            f_SetComplete((int)EM_Guidance.GuidanceRead);
    }

    public override void f_Execute()
    {
        base.f_Execute();

        //隐藏跳过按钮（策划要求）
        //NowTime += Time.deltaTime;
        //if (NowTime >= idleTime)
        //{
        //    NowTime = 0f;
        //    tManage.Btn_Esc.SetActive(true);
        //}

        for (int i = 0; i < tManage.szCheckUi.Length; i++)
        {
            if (ccUIManage.GetInstance().f_CheckUIIsOpen(tManage.szCheckUi[i]))
            {
                f_SetComplete((int)EM_Guidance.GuidancePass);
                break;
            }
        }


        //while (glo_Main.GetInstance().m_SC_Pool.m_GuidanceDialogSC.f_GetSC(CheckUiID) != null)
        //{
        //    tCheckUiDt = glo_Main.GetInstance().m_SC_Pool.m_GuidanceDialogSC.f_GetSC(CheckUiID) as GuidanceDialogDT;
        //    if (ccUIManage.GetInstance().f_CheckUIIsOpen(tCheckUiDt.szDialog))
        //    {
        //        ccTimeEvent.GetInstance().f_Pause();
        //        Data_Pool.m_GuidancePool.f_SetCurClickButtonNull();
        //        IsInit = false;
        //        break;
        //    }
        //    IsInit = true;
        //    CheckUiID++;
        //}
    }
}

