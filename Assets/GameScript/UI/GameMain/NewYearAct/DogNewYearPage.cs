using UnityEngine;
using ccU3DEngine;
using System.Collections;

public class DogNewYearPage : UIFramwork
{
    private SocketCallbackDT _SendStep = new SocketCallbackDT();
    private SocketCallbackDT _SendInfo = new SocketCallbackDT();

    private Transform _Dog;
    private Transform[] _House;
    private Transform _DefaultPoint;

    private UILabel _Score;

    private TweenScale _DogTweenScale;
    private TweenRotation _DogTweenRotation;


    private int _TestStep = 4;


    private GameObject BigHouse1;
    private GameObject BigHouse2;

    private Animator _DogAni;
    public void f_ShowView()
    {
        UITool.f_OpenOrCloseWaitTip(true);
        _SendInfo.m_ccCallbackFail = _InfoFail;
        _SendInfo.m_ccCallbackSuc = _InfoSuc;
        Data_Pool.m_NewYearActivityPool.f_SendDogNewYearInfo(_SendInfo);
    }
    public void f_DestoryView()
    {
        gameObject.SetActive(false);
    }



    private void _UpdateMain()
    {
        _Dog = f_GetObject("Dog").transform;
        _House = f_GetObject("House").transform.GetComponentsInChildren<Transform>();
        _DefaultPoint = f_GetObject("defaultPoint").transform;
        _Score = f_GetObject("Score").GetComponent<UILabel>();
        BigHouse1 = f_GetObject("SmallHouse5");
        BigHouse2 = f_GetObject("SmallHouse10");

        _DogAni = _Dog.GetComponent<Animator>();

        f_RegClickEvent("StepBtn", _StepBtn);
        //TsuCode
          _Score.text = string.Format(CommonTools.f_GetTransLanguage(1486), Data_Pool.m_NewYearActivityPool.m_DogNewYearResidueIntegral,
           Data_Pool.m_NewYearActivityPool.m_DogNewYearIntegral);
        //
        _SetFirstPoint(Data_Pool.m_NewYearActivityPool.m_DogNewYearStepNum);
    }

    private void _UpdatePoint()
    {
        MessageBox.ASSERT("TsuLog DogNewYearPage _UpdatePoint + " + Data_Pool.m_NewYearActivityPool.m_DogNewYearResidueIntegral +":"+ Data_Pool.m_NewYearActivityPool.m_DogNewYearIntegral);
        //_SetDogPoint(Data_Pool.m_NewYearActivityPool.m_DogNewYearStepNum);
        //TsuCode
        MessageBox.ASSERT("TsuLog Step? " + Data_Pool.m_NewYearActivityPool.m_DogNewYearStepNum);
        if (Data_Pool.m_NewYearActivityPool.m_DogNewYearStepNum == 0)
        {
            _SetDogPoint(Data_Pool.m_NewYearActivityPool.m_DogNewYearStepNum++);
        }
        else
        {
            _SetDogPoint(Data_Pool.m_NewYearActivityPool.m_DogNewYearStepNum++,false);
        }
        //
        //_SetDogPoint(_TestStep++);
       
        _Score.text = string.Format(CommonTools.f_GetTransLanguage(1486), Data_Pool.m_NewYearActivityPool.m_DogNewYearResidueIntegral,
           Data_Pool.m_NewYearActivityPool.m_DogNewYearIntegral);
    }
    private void _SetDogPoint(int iStep, bool First = false)
    {
        _DogAni.SetInteger("HouseNum", iStep % 10);
    }

    private void _SetFirstPoint(int iStep)
    {
        _DogAni.SetInteger("HouseNum", iStep % 10);
    }
    #region  按钮事件

    private void _StepBtn(GameObject go, object obj1, object obj2)
    {
        if (Data_Pool.m_NewYearActivityPool.m_DogNewYearResidueIntegral < 60)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1487));
            return;
        }

        //_UpdatePoint();
        Data_Pool.m_AwardPool.m_GetLoginAward.Clear();
        UITool.f_OpenOrCloseWaitTip(true);
        _SendStep.m_ccCallbackFail = _SendStepFail;
        _SendStep.m_ccCallbackSuc = _SendStepSuc;
        Data_Pool.m_NewYearActivityPool.f_SendDogNewYearStep(_SendStep);
    }


    private void _BoxBtn(GameObject go, object obj1, object obj2)
    {
        int SetpNum = Data_Pool.m_NewYearActivityPool.m_DogNewYearStepNum / 10;

        NewYearStepDT tNewYearStepDT = glo_Main.GetInstance().m_SC_Pool.m_NewYearStepSC.f_GetSC(SetpNum * 10 + (int)obj1) as NewYearStepDT;

        if (tNewYearStepDT == null)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1488));
            return;
        }



        //BoxGetSubPageParam tBoxGetSubPageParam = new BoxGetSubPageParam(tNewYearStepDT.iAwardId,1,);



    }

    #endregion


    #region 回调
    private void _SendStepSuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        _UpdatePoint();
        if (Data_Pool.m_AwardPool.m_GetLoginAward.Count > 0)
            ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN, new object[] { Data_Pool.m_AwardPool.m_GetLoginAward });
    }

    private void _SendStepFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);


        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1489) + obj.ToString());
    }

    private void _InfoSuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        gameObject.SetActive(true);
        _UpdateMain();
    }

    private void _InfoFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1490) + obj.ToString());
    }
    #endregion
}
