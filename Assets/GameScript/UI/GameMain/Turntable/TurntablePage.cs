using ccU3DEngine;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TurntablePage : UIFramwork
{
    private string strTextBg = "UI/TextureRemove/Turntable/Turntable_Bg";
    private string strTextBg1 = "UI/TextureRemove/Turntable/Turntable_Bg1";
    private GameObject _btnClose;//关闭界面
    private GameObject _btnRechange;//充值按钮
    private GameObject _btnBuyOne;//抽一次
    private GameObject _btnBuyTen;//抽十次
    private UILabel _buyOneText;
    private UILabel _buyTenText;
    private UILabel _curAwardSyceeText;//当然奖池元宝
    private UILabel _awardClearEndTime;//奖池清空倒计时
    private UILabel _boxWeekCount;//本周宝箱抽取次数
    private GameObject[] _awardItem;//奖池物品
    private GameObject[] _turntableLBg;//转盘底图亮背景
    private GameObject _recordParent;//记录父节点
    private GameObject _recordItem;//记录子节点
    private GameObject[] _BoxParant;//宝箱父节点
    private GameObject _BoxCommonItem;//公用宝箱预设
                                      // private BoxCommonItem[] _taskBoxItems;//公用宝箱Item
    private GameParamDT _turntableParam;
    private SocketCallbackDT QueryBoxInfoCallback = new SocketCallbackDT();//宝箱信息回调
    private int Time_TurntableTime = 0;
    private GameObject _btnDiBg;//抽奖底板
    private const int ShowModeId = 11071;//模型id
    //private GameObject role;
    //初始化ui
    private void InitUI()
    {
        _btnClose = f_GetObject("BtnClose");
        _btnRechange = f_GetObject("RechangeBtn");
        _btnBuyOne = f_GetObject("BtnBuyOne");
        _btnBuyTen = f_GetObject("BtnBuyTen");
        _btnDiBg = f_GetObject("BtnBlackBg");
        _curAwardSyceeText = f_GetObject("CurSyceeText").GetComponent<UILabel>();
        _awardClearEndTime = f_GetObject("AwardEndTime").GetComponent<UILabel>();
        _buyOneText = _btnBuyOne.transform.Find("IconProp/LabelCount").GetComponent<UILabel>();
        _buyTenText = _btnBuyTen.transform.Find("IconProp/LabelCount").GetComponent<UILabel>();
        _boxWeekCount = f_GetObject("AwardWeekName").GetComponent<UILabel>();
        _recordParent = f_GetObject("RecordParent");
        _recordItem = f_GetObject("RecordItem");
        _awardItem = new GameObject[10];
        _turntableLBg = new GameObject[10];
        for (int i = 0; i < 10; i++)
        {
            _awardItem[i] = f_GetObject("Item" + (i + 1));
            _turntableLBg[i] = _awardItem[i].transform.Find("SpriteL").gameObject;
            _turntableLBg[i].SetActive(false);
        }
        //宝箱初始化
        _BoxParant = new GameObject[6];
        for (int i = 0; i < _BoxParant.Length; i++)
        {
            _BoxParant[i] = f_GetObject("BoxParent" + (i + 1).ToString());
        }
        _BoxCommonItem = f_GetObject("BoxCommonItem");
        // _taskBoxItems = new BoxCommonItem[_BoxParant.Length];
        //for (int i = 0; i < _taskBoxItems.Length; i++)
        //{
        //    _taskBoxItems[i] = BoxCommonItem.f_Create(_BoxParant[i], _BoxCommonItem);
        //    _taskBoxItems[i].f_UpdateClickHandle(f_BoxClickHandle, i);
        // }
        Data_Pool.m_TurntablePool.UpdateUI = f_BasicInfo;
        _btnDiBg.SetActive(false);
        f_SetTurntableInfo();
    }

    //加载背景
    private void f_LoadTextTure()
    {
        UITexture textBg = f_GetObject("SpriteBg").GetComponent<UITexture>();
        //UITexture textBg1 = f_GetObject("SpriteBg1").GetComponent<UITexture>();
        if (textBg.mainTexture == null)
        {
            Texture2D texTure2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTextBg);
            textBg.mainTexture = texTure2D;
        }
        //if (textBg1.mainTexture == null)
        //{
        //    Texture2D texTure2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTextBg1);
        //    textBg1.mainTexture = texTure2D;
        //}
    }

    //转盘转
    private void f_Turntable()
    {
        Time_TurntableTime = ccTimeEvent.GetInstance().f_RegEvent(0.05f, true, null, _TurntableTime);
    }

    //初始转盘物品
    private void f_SetTurntableInfo()
    {
      
        List<BasePoolDT<long>> listDT = Data_Pool.m_TurntablePool.f_GetAll();
        for (int i = 0; i < 10; i++)
        {
            TurntablePoolDT dt = (TurntablePoolDT)listDT[i];
            ResourceCommonDT commonDT = new ResourceCommonDT();
            commonDT.f_UpdateInfo((byte)dt.m_TurntableData.iAwardType, dt.m_TurntableData.iAwardId, dt.m_TurntableData.iAwardNum);
            string sailResName = commonDT.mName;
            Transform item = _awardItem[i].transform.Find("AwardItem");
            if (dt.m_TurntableData.iAwardType == 1)
            {
                f_UnRegClickEvent(item.gameObject);
item.transform.Find("LabelCount").GetComponent<UILabel>().text = "Return" + commonDT.mResourceNum.ToString() + "%";
            }
            else
            {
                item.transform.Find("LabelCount").GetComponent<UILabel>().text = commonDT.mResourceNum.ToString();
                f_RegClickEvent(item.gameObject, f_OpenGoodsDetai, commonDT);
            }
            item.transform.Find("IconBorder").GetComponent<UISprite>().spriteName = UITool.f_GetImporentColorName(commonDT.mImportant, ref sailResName);
            item.transform.GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSprite(commonDT.mIcon);
        }
    }

    //设置基本信息
    private void f_BasicInfo(object obj = null)
    {
        //当前奖池元宝
        _curAwardSyceeText.text = Data_Pool.m_TurntablePool.m_CurSyceeRemain.ToString();
    }

    //记录
    private void f_SetRecordInfo()
    {
        MessageBox.ASSERT("TsuLog List Record: " + Data_Pool.m_TurntablePool.m_RecordList.Count);
        GridUtil.f_SetGridView<TurntableLotteryInfo>(_recordParent, _recordItem, Data_Pool.m_TurntablePool.m_RecordList, OnTurntableRecordItemUpdate);
        _recordParent.transform.GetComponent<UIGrid>().Reposition();
        _recordParent.transform.GetComponentInParent<UIScrollView>().ResetPosition();
     
    }


    //Item更新
    private void OnTurntableRecordItemUpdate(GameObject item, TurntableLotteryInfo data)
    {

        ServerInforDT serverInfo = (ServerInforDT)glo_Main.GetInstance().m_SC_Pool.m_ServerInforSC.f_GetSC(data.iServerId);
        string serverName = serverInfo != null ? serverInfo.szName : string.Empty;
        ResourceCommonDT commonDT = new ResourceCommonDT();
        commonDT.f_UpdateInfo((byte)data.uType, data.uId, data.uNum);
        DateTime time = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Unspecified).AddSeconds(data.iTime + 7 * 60 * 60);
        string sztime = string.Format("{0}/{1} {2}:{3}", time.Month, time.Day, time.Hour, time.Minute);
item.GetComponent<UILabel>().text = sztime + " [" + serverName + "]" + "[4a4988]【" + data.szRoleName + "】[-]" + "lucky to get: " + "[ d1783d]" + data.uNum + commonDT.mName + "[-]";
        //item.GetComponent<UILabel>().text = "may mắn nhận:";
    }
    private DateTime tDate;
    private int Time_ShowTime;
    // 时间
    private void _UpdateTimeData(object boj)
    {
        //TsuComment
        //long timeEnd = Data_Pool.m_TurntablePool.m_EndTime - GameSocket.GetInstance().f_GetServerTime();
        ////iTime = (timeEnd + _turntableParam.iParam3 * 60 * 60) - GameSocket.GetInstance().f_GetServerTime();
        //if (timeEnd <= 0)
        //{
        //    //活动结束
        //    _awardClearEndTime.text = "Sự kiện kết thúc";
        //    return;
        //}
        //tDate = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Unspecified).AddSeconds(timeEnd);
        //if (tDate.Day != 1)
        //    _awardClearEndTime.text = string.Format("{0} ngày {1}:{2}:", tDate.Day - 1, tDate.Hour, tDate.Minute) + tDate.Second + "";
        //else
        //    _awardClearEndTime.text = string.Format("{0}:{1}:", tDate.Hour, tDate.Minute) + tDate.Second + "";


        ////TsuCode
        int dateEnd = Data_Pool.m_TurntablePool.m_EndTime;
        String end = dateEnd.ToString();
        int eYear = int.Parse(end.Substring(0, 4));
        int eMonth = int.Parse(end.Substring(4, 2));
        int eDay = int.Parse(end.Substring(6, 2));
        DateTime endDate = new DateTime(eYear, eMonth, eDay, 0, 0, 0, 0); //TsuCode

        //DateTime now = System.DateTime.Now;
         int nowTimeServer = GameSocket.GetInstance().f_GetServerTime(); //ServerTime now(second)
        DateTime dateTimeServerNow = ccMath.time_t2DateTime(nowTimeServer);

        TimeSpan Time = endDate - dateTimeServerNow;
        int TongSoNgay = Time.Days;
        int TongSoGio = Time.Hours;
        int TongSoGiay = Time.Seconds;

		_awardClearEndTime.text = string.Format("{0} Date {1} Hour {2} Seconds", TongSoNgay, TongSoGio, TongSoGiay);
        ////////
    }


    //初始界面
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        InitUI();
        f_RegClickEvent(_btnClose, OnCloseClick);//关闭界面
        f_RegClickEvent(_btnRechange, OnRechangeClick, ShowVip.EM_PageIndex.Recharge);

        f_RegClickEvent(_btnDiBg, OnCloseTurntableClick);//手动关闭抽奖界面

        f_RegClickEvent("btnOpenAwardBox", Btn_OpenRebate);
        f_RegClickEvent("RebateBtnBlack", Btn_CloseRebate);
        f_RegClickEvent("OneKeyGet", f_OneKeyBox);
    }

  

    //打开界面
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);

        //查询 成功 / 失败
        QueryBoxInfoCallback.m_ccCallbackSuc = OnBoxInfoSucCallback;
        QueryBoxInfoCallback.m_ccCallbackFail = OnBoxInfoFailCallback;
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_TurntablePool.f_QueryTurntableBoxInfo(QueryBoxInfoCallback);
       
        f_LoadTextTure();
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_TurntablePage_UpdateRecord, f_EeventCallback_UpdateList, this);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_TurntablePage_UpdateBoxInfo, f_EeventCallback_UpdateBoxInfoList, this);
        _turntableParam = (GameParamDT)glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC((int)EM_GameParamType.TurntableLottery);
        _buyOneText.text = _turntableParam.iParam1.ToString();
        _buyTenText.text = _turntableParam.iParam2.ToString();
        //if (role != null)
        //    UITool.f_DestoryStatelObject(role);
        //role = UITool.f_GetStatelObject(1107, f_GetObject("ModelParent").transform, Vector3.zero, Vector3.zero, 18);
        Time_ShowTime = ccTimeEvent.GetInstance().f_RegEvent(1f, true, null, _UpdateTimeData);
       
        f_SetRecordInfo();
        f_UpdateTaskBox();
        f_BasicInfo();
        f_RegClickEvent(_btnBuyOne, f_LotteryDrawOneClick);//抽奖一次
        f_RegClickEvent(_btnBuyTen, f_LotteryDrawTenClick);//抽奖十次
        f_GetObject("RebatePage").SetActive(false);

    }
    
    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_TurntablePage_UpdateRecord, f_EeventCallback_UpdateList, this);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_TurntablePage_UpdateBoxInfo, f_EeventCallback_UpdateBoxInfoList, this);
        ccTimeEvent.GetInstance().f_UnRegEvent(Time_TurntableTime);
    }

    // 更新宝箱状态
    private void f_UpdateTaskBox()
    {
        //for (int i = 0; i < _taskBoxItems.Length; i++)
        //{
        //    _taskBoxItems[i].f_UpdateInfo(EM_BoxType.Task, Data_Pool.m_TurntablePool.f_GetBoxStateByIdx(i), Data_Pool.m_TurntablePool.f_GetBoxExtraInfo(i), true);
        //}
    }
    private int mCurBoxIdx = 0;
    private List<int> mCurBoxArr = new List<int>();
    //点击宝箱信息
    private void f_BoxClickHandle(object value)
    {
        //int id = (int)(value);
        //mCurBoxIdx = id;
        //TurntableBoxInfo tBoxInfo = Data_Pool.m_TurntablePool.f_GetBoxInfo(id);
        // BoxGetSubPageParam tParam = new BoxGetSubPageParam(tBoxInfo.mBoxId, tBoxInfo.mIdx, tBoxInfo.mBoxScore.ToString()+"次", EM_BoxType.Task, tBoxInfo.mBoxState, f_BoxGetHandle, this);
        //打开宝箱显示界面
        // ccUIManage.GetInstance().f_SendMsg(UINameConst.BoxGetSubPage, UIMessageDef.UI_OPEN, tParam);
    }

    private void f_OpenGoodsDetai(GameObject go, object value1, object value)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetailPage, UIMessageDef.UI_OPEN, (ResourceCommonDT)value1);
    }

    //抽奖一次
    private void f_LotteryDrawOneClick(GameObject go, object value1, object value)
    {
        int syceeCount = UITool.f_GetResourceHaveNum((int)EM_ResourceType.Money, (int)EM_MoneyType.eUserAttr_Sycee);
        if (syceeCount < _turntableParam.iParam1)
        {
UITool.Ui_Trip("Không đủ KNB!");
            return;
        }
        SocketCallbackDT callbackDT = new SocketCallbackDT();
        callbackDT.m_ccCallbackSuc = f_GetBuyOneSuc;
        callbackDT.m_ccCallbackFail = f_GetBuyOneFail;
        Data_Pool.m_TurntablePool.f_QueryLotteryDraw((byte)0, callbackDT);
        UITool.f_OpenOrCloseWaitTip(true);
    }

    //抽奖一次成功回调
    private void f_GetBuyOneSuc(object obj)
    {
        f_UpdateTaskBox();
        f_BasicInfo();
        f_SetRecordInfo();
        _btnDiBg.SetActive(true);
        m_Prize = Data_Pool.m_TurntablePool.m_TempIdList[0] - 1;
        f_Turntable();
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
        //          new object[] { Data_Pool.m_TurntablePool.m_TurntableAwardList });
        UITool.f_OpenOrCloseWaitTip(false);
    }
    //抽奖一次失败
    private void f_GetBuyOneFail(object obj)
    {
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Không thành công" + CommonTools.f_GetTransLanguage((int)obj));
        UITool.f_OpenOrCloseWaitTip(false);
    }

    //抽奖十次
    private void f_LotteryDrawTenClick(GameObject go, object value1, object value)
    {
        int syceeCount = UITool.f_GetResourceHaveNum((int)EM_ResourceType.Money, (int)EM_MoneyType.eUserAttr_Sycee);
        if (syceeCount < _turntableParam.iParam2)
        {
UITool.Ui_Trip("Không đủ KNB!");
            return;
        }
        SocketCallbackDT callbackDT = new SocketCallbackDT();
        callbackDT.m_ccCallbackSuc = f_GetBuyTenSuc;
        callbackDT.m_ccCallbackFail = f_GetBuyTenFail;
        Data_Pool.m_TurntablePool.f_QueryLotteryDraw((byte)1, callbackDT);
        UITool.f_OpenOrCloseWaitTip(true);

    }

    //抽奖十次成功回调
    private void f_GetBuyTenSuc(object obj)
    {
        f_UpdateTaskBox();
        f_BasicInfo();
        f_SetRecordInfo();
        _btnDiBg.SetActive(true);
        m_Prize = Data_Pool.m_TurntablePool.m_TempIdList[0] - 1;
        f_Turntable();
        //显示物品
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
        //   new object[] { Data_Pool.m_TurntablePool.m_TurntableAwardList });
        UITool.f_OpenOrCloseWaitTip(false);
    }
    //抽奖十次失败
    private void f_GetBuyTenFail(object obj)
    {
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Quay 10 lần không thành công" + CommonTools.f_GetTransLanguage((int)obj));
        UITool.f_OpenOrCloseWaitTip(false);
    }


    //关闭界面事件
    private void OnCloseClick(GameObject go, object value1, object value)
    {
        ccTimeEvent.GetInstance().f_UnRegEvent(Time_ShowTime);
        ccTimeEvent.GetInstance().f_UnRegEvent(Time_TurntableTime);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.TurntablePage, UIMessageDef.UI_CLOSE);
    }

    //立即充值事件
    private void OnRechangeClick(GameObject go, object obj1, object obj2)
    {
        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShowVip, UIMessageDef.UI_OPEN, obj1);
    }

    //请求宝箱服务器信息成功回调
    private void OnBoxInfoSucCallback(object obj)
    {
        f_UpdateTaskBox();
_boxWeekCount.text = "This week's cumulative reward： " + Data_Pool.m_TurntablePool.m_BoxCount.ToString();
        UITool.f_OpenOrCloseWaitTip(false);
    }

    //宝箱信息失败回调
    private void OnBoxInfoFailCallback(object obj)
    {
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "Truy vấn không thành công" + CommonTools.f_GetTransLanguage((int)obj));
        UITool.f_OpenOrCloseWaitTip(false);
    }

    //通天转盘记录链表刷新
    private void f_EeventCallback_UpdateList(object value)
    {
      
        f_BasicInfo();
        f_SetRecordInfo();
    }
    private void f_EeventCallback_UpdateBoxInfoList(object value)
    {
_boxWeekCount.text = "Phần thưởng tích lũy của tuần này: " + Data_Pool.m_TurntablePool.m_BoxCount.ToString();
        f_UpdateTaskBox();
    }
    private int m_Index = 0;//转圈起点位置
    private int m_Count = 10;//总共10个位置
    private float m_FirstTime = 0;//记录第一次时间
    private float m_Speed = 0f;//初始速度
    private int m_Cycle = 5;//基本转到圈数
    private int m_TurntableTimes = 0;//转动次数
    private int m_Prize = 0;//中奖位置
    int m_IndexTimes = 0;
    private void _TurntableTime(object obj)
    {
        int index = m_Index;
        int count = m_Count;
        if (Time.time - m_FirstTime >= m_Speed)
        {
            m_FirstTime = Time.time;
            _turntableLBg[index].SetActive(false);
            index++;
            if (index > count - 1)
            {
                index = 0;
            }
            m_Index = index;
            _turntableLBg[index].SetActive(true);
            m_TurntableTimes++;
            if (m_TurntableTimes > m_Cycle + 10 && m_Prize == m_Index)
            {
                m_IndexTimes = m_IndexTimes + 1;
                if (m_IndexTimes < Data_Pool.m_TurntablePool.m_TempIdList.Count - 1)
                {
                    m_Prize = Data_Pool.m_TurntablePool.m_TempIdList[m_IndexTimes] - 1;
                }
                m_TurntableTimes = 0;
                m_Speed = 0.01f;
                m_FirstTime = 0;
                int countLenght = 0;
                if (Data_Pool.m_TurntablePool.m_TempIdList.Count > 1)
                {
                    countLenght = 1;
                }
                else
                    countLenght = Data_Pool.m_TurntablePool.m_TempIdList.Count - 1;
                if (m_IndexTimes >= countLenght)
                {
                    Invoke("ShowTurntableAward", 0.2f);
                    m_IndexTimes = 0;
                    ccTimeEvent.GetInstance().f_UnRegEvent(Time_TurntableTime);
                }
                //else
                //    Invoke("ShowTurntableAward", 0.2f);
            }
            else
            {
                if (m_TurntableTimes < m_Cycle)
                {
                    m_Speed = m_Speed - 20.0f;
                }
            }
        }
    }
    private List<AwardPoolDT> m_AwardList = new List<AwardPoolDT>();
    //抽中奖励展示
    private void ShowTurntableAward()
    {
        //m_AwardList.Clear();
        //if(Data_Pool.m_TurntablePool.m_TurntableAwardList.Count == 1)
        //{
        //    m_IndexTimes = 0;
        //    m_AwardList.Add(Data_Pool.m_TurntablePool.m_TurntableAwardList[0]);
        //}
        //else
        //    m_AwardList.Add(Data_Pool.m_TurntablePool.m_TurntableAwardList[m_IndexTimes - 1]);
        //if (m_IndexTimes >= Data_Pool.m_TurntablePool.m_TempIdList.Count - 1)
        //{
        //    m_IndexTimes = 0;
        //    _btnDiBg.SetActive(false);
        //}
        //else
        //{
        //    if(m_IndexTimes == 0)
        //    {
        //        _btnDiBg.SetActive(false);
        //    }
        //    else
        //        _btnDiBg.SetActive(true);
        //}
        _btnDiBg.SetActive(false);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
          new object[] { Data_Pool.m_TurntablePool.m_TurntableAwardList });
    }

    //手动关闭抽奖循环
    private void OnCloseTurntableClick(GameObject go, object obj1, object obj2)
    {
        for (int i = 0; i < 10; i++)
        {
            if (m_Prize == i)
            {
                _turntableLBg[i].SetActive(true);
            }
            else
                _turntableLBg[i].SetActive(false);
        }
        m_Index = m_Prize;
        _btnDiBg.SetActive(false);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
          new object[] { Data_Pool.m_TurntablePool.m_TurntableAwardList });
        ccTimeEvent.GetInstance().f_UnRegEvent(Time_TurntableTime);
    }

    #region 返利

    private void Btn_OpenRebate(GameObject go, object obj1, object obj2)
    {
        f_GetObject("RebatePage").SetActive(true);
        OpenRebatePage();
    }

    private void Btn_CloseRebate(GameObject go, object obj1, object obj2)
    {
        f_GetObject("RebatePage").SetActive(false);
    }
    private void OpenRebatePage()
    {
        GridUtil.f_SetGridView<TurntableBoxInfo>(f_GetObject("RebateParent"), f_GetObject("Rebateitem"), new List<TurntableBoxInfo>(Data_Pool.m_TurntablePool.mBoxInfo), UpdateItem);
        f_GetObject("RebateParent").transform.GetComponent<UIGrid>().Reposition();
        f_GetObject("RebateParent").transform.GetComponentInParent<UIScrollView>().ResetPosition();

    }

    private void UpdateItem(GameObject item, TurntableBoxInfo dt)
    {
        Transform trna = item.transform;
        UI2DSprite Icon = trna.Find("Good/Icon").GetComponent<UI2DSprite>();
        UISprite Case = trna.Find("Good/Case").GetComponent<UISprite>();
        UILabel Num = trna.Find("Good/Num").GetComponent<UILabel>();

        UILabel Name = trna.Find("Name").GetComponent<UILabel>();
        UILabel Desc = trna.Find("Desc").GetComponent<UILabel>();
        UILabel Rate = trna.Find("Rate").GetComponent<UILabel>();

        GameObject isGet = trna.Find("isGet").gameObject;
        GameObject Get = trna.Find("Get").gameObject;

        List<AwardPoolDT> award = Data_Pool.m_AwardPool.f_GetAwardPoolDTByAwardId(dt.mBoxId);
        Icon.sprite2D = UITool.f_GetIconSprite(award[0].mTemplate.mIcon);
        string name = award[0].mTemplate.mName;
        Case.spriteName = UITool.f_GetImporentColorName(award[0].mTemplate.mImportant, ref name);
        Num.text = "X" + award[0].mTemplate.mResourceNum;

        Name.text = name;

Desc.text = string.Format("{0} possible draws", dt.mBoxScore);
        Rate.text = Data_Pool.m_TurntablePool.m_BoxCount + "/" + dt.mBoxScore;

        isGet.SetActive(dt.mBoxState == EM_BoxGetState.AlreadyGet);
        Get.SetActive(dt.mBoxState == EM_BoxGetState.CanGet);
        f_RegClickEvent(Get, f_BoxGetHandle, dt.mIdx);
    }
    //宝箱领取事件
    private void f_BoxGetHandle(GameObject go, object obj1, object value)
    {
        int id = (int)obj1;
        mCurBoxIdx = id;
        EM_BoxGetState tBoxState = Data_Pool.m_TurntablePool.f_GetBoxStateByIdx(id);
        if (tBoxState == EM_BoxGetState.CanGet)
        {
            SocketCallbackDT callbackDT = new SocketCallbackDT();
            callbackDT.m_ccCallbackSuc = f_GetBoxAwardSuc;
            callbackDT.m_ccCallbackFail = f_GetBoxAwardFail;
            Data_Pool.m_TurntablePool.f_QueryTurntableBox((byte)(id + 1), callbackDT);
            UITool.f_OpenOrCloseWaitTip(true);
        }
    }

    public void f_OneKeyBox(GameObject go, object obj1, object value)
    {
        bool isGet = false;
        mCurBoxArr.Clear();
        for (int i = 0; i < Data_Pool.m_TurntablePool.mBoxInfo.Length; i++)
        {
            if (Data_Pool.m_TurntablePool.mBoxInfo[i].mBoxState == EM_BoxGetState.CanGet)
            {
                mCurBoxArr.Add(Data_Pool.m_TurntablePool.mBoxInfo[i].mBoxId);
                isGet = true;
            }
        }
        if (!isGet)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(2193));
            return;
        }

        SocketCallbackDT callbackDT = new SocketCallbackDT();
        callbackDT.m_ccCallbackSuc = f_OneKeyBoxSuc;
        callbackDT.m_ccCallbackFail = f_GetBoxAwardFail;
        Data_Pool.m_TurntablePool.f_OneKeyGetBox(callbackDT);
        UITool.f_OpenOrCloseWaitTip(true);
    }

    private void f_OneKeyBoxSuc(object obj)
    {
        List<AwardPoolDT> aAward = new List<AwardPoolDT>();
        for (int i = 0; i < mCurBoxArr.Count; i++)
        {
            aAward.AddRange(Data_Pool.m_AwardPool.f_GetAwardPoolDTByAwardId(mCurBoxArr[i]));
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN, new object[] { aAward, this });
        OpenRebatePage();
        UITool.f_OpenOrCloseWaitTip(false);

    }
    //领取成功回调
    private void f_GetBoxAwardSuc(object obj)
    {
        TurntableBoxInfo tBoxInfo = Data_Pool.m_TurntablePool.f_GetBoxInfo(mCurBoxIdx);
        //关闭宝箱领取界面
        // ccUIManage.GetInstance().f_SendMsg(UINameConst.BoxGetSubPage, UIMessageDef.UI_CLOSE);
        //展示获得奖励
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN, new object[] { Data_Pool.m_AwardPool.f_GetAwardPoolDTByAwardId(tBoxInfo.mBoxId), this });
        OpenRebatePage();
        UITool.f_OpenOrCloseWaitTip(false);
    }

    //领取失败回调
    private void f_GetBoxAwardFail(object obj)
    {
UITool.UI_ShowFailContent("Receive Failure Reward，Code:" + obj);
        MessageBox.ASSERT("GetBoxAwardFail. Fail Code:" + obj);
        //关闭宝箱领取界面
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.BoxGetSubPage, UIMessageDef.UI_CLOSE);
        OpenRebatePage();
        UITool.f_OpenOrCloseWaitTip(false);
    }


    #endregion
}
