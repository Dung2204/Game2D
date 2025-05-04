using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
using System;
/// <summary>
/// 登录送礼(开服豪礼)
/// </summary>
public class ActLoginGiftNewServCtl : UIFramwork
{
    private SocketCallbackDT QueryCallback = new SocketCallbackDT();//查询回调
    private SocketCallbackDT RequestGetCallback = new SocketCallbackDT();//领取回调
    ActLoginGiftDT currentSelectLoginGiftDT;//当前选中的dt
    private string strTexBgRoot = "UI/TextureRemove/Activity/Tex_NewServBg";
    public void f_DestoryView()
    {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 初始化数据,视图
    /// </summary>
    public void f_ShowView()
    {
        gameObject.SetActive(true);
        RequestGetCallback.m_ccCallbackSuc = OnGetSucCallback;
        RequestGetCallback.m_ccCallbackFail = OnGetFailCallback;

        QueryCallback.m_ccCallbackSuc = OnQuerySucCallback;
        QueryCallback.m_ccCallbackFail = OnQueryFailCallback;
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_ActivityCommonData.f_QueryLoginGiftNewServ(QueryCallback);
        UpdateContent();

        int curOpenServDay = Data_Pool.m_ActivityCommonData.LoginGiftOpenSevDayNewServ;
        DateTime dataNow = ccMath.time_t2DateTime(GameSocket.GetInstance().f_GetServerTime());
        DateTime dataTime = new DateTime(dataNow.Year, dataNow.Month, dataNow.Day).AddDays(8 - curOpenServDay);
        f_GetObject("LabelActTime").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(1340), dataTime.Year, dataTime.Month, dataTime.Day, dataTime.Hour);
        f_LoadTexture();
    }
    public static int[] int2Arr(int n) //TsuCode
    {
        if (n == 0) return new int[1] { 0 };

        var digits = new List<int>();

        for (; n != 0; n /= 10)
            digits.Add(n % 10);

        var arr = digits.ToArray();
        Array.Reverse(arr);
        return arr;
    }

    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {
        //加载背景图
        UITexture TexBg = f_GetObject("TexBg").GetComponent<UITexture>();
        if (TexBg.mainTexture == null)
        {
            Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBgRoot);
            TexBg.mainTexture = tTexture2D;
        }
    }
    /// <summary>
    /// 更新内容
    /// </summary>
    private void UpdateContent()
    {
        List<NBaseSCDT> listLoginGiftDT = glo_Main.GetInstance().m_SC_Pool.m_ActLoginGiftSC.f_GetAll();
        int loginGiftDay = Data_Pool.m_ActivityCommonData.LoginGiftOpenSevDayNewServ;

        List<ActLoginGiftDT> listData = new List<ActLoginGiftDT>();
        for (int i = 0; i < listLoginGiftDT.Count; i++)
        {
            ActLoginGiftDT actLoginGiftDT = listLoginGiftDT[i] as ActLoginGiftDT;
            if (actLoginGiftDT.itype == 2)
            {
                listData.Add(actLoginGiftDT);
            }
        }
        int loginGiftIsGet = Data_Pool.m_ActivityCommonData.NewServGiftGetFlag;

        int[] arr;
        arr = int2Arr(loginGiftIsGet);

        for (int i = 0; i < 7; i++)
        {
            ActLoginGiftDT actLoginGiftDT = listData[i];
            GameObject item = f_GetObject("Item" + i);

            ResourceCommonDT dt = new ResourceCommonDT();
            dt.f_UpdateInfo((byte)actLoginGiftDT.iType1, actLoginGiftDT.iID1, actLoginGiftDT.iCount1);
            if (item.transform.Find("effect") != null)
                GameObject.DestroyImmediate(item.transform.Find("effect").gameObject);
            EM_AwardGetState getState = EM_AwardGetState.AlreadyGet;

            if (actLoginGiftDT.iday > loginGiftDay)//显示领取灰显
            {
                getState = EM_AwardGetState.Lock;
            }
            else
            {
                //if (BitTool.BitTest(loginGiftIsGet, (ushort)actLoginGiftDT.iday))
                if (arr[actLoginGiftDT.iday-1]!=1)
                {
                    getState = EM_AwardGetState.AlreadyGet;
                }
                else
                {
                    //if (actLoginGiftDT.iday == loginGiftDay)
                    if (actLoginGiftDT.iday <= loginGiftDay)
                    {
                        getState = EM_AwardGetState.CanGet;
                        UITool.f_CreateEquipEffect(item.transform, "effect", (EM_Important)dt.mImportant, new Vector3(0, -5, 0), new Vector3(160, 160, 160));
                    }
                    else
                    {
                        getState = EM_AwardGetState.Lock;
                    }
                }                
            }
           
            f_RegClickEvent(item.gameObject, OnGetClick, actLoginGiftDT, getState);
            item.GetComponent<LoginGiftNewServItem>().SetData(actLoginGiftDT.iday, getState, 2, 353, 1);//使用红色宝物礼包代用显示
        }
    }
    /// <summary>
    /// 点击领取按钮事件
    /// </summary>
    private void OnGetClick(GameObject go, object obj1, object obj2)
    {
        ActLoginGiftDT actLoginGiftDT = (ActLoginGiftDT)obj1;
        EM_AwardGetState getState = (EM_AwardGetState)obj2;
        if (getState == EM_AwardGetState.CanGet)
        {
            UITool.f_OpenOrCloseWaitTip(true);
            Data_Pool.m_ActivityCommonData.f_GetLoginGiftNewServ(RequestGetCallback, actLoginGiftDT.iday);
            currentSelectLoginGiftDT = glo_Main.GetInstance().m_SC_Pool.m_ActLoginGiftSC.f_GetSC(actLoginGiftDT.iId) as ActLoginGiftDT;
        }
        else
        {
            ItemShowPageParam param = new ItemShowPageParam();
            List<ItemShowGoodItem> m_listItem = new List<ItemShowGoodItem>();
            m_listItem.Add(new ItemShowGoodItem((EM_ResourceType)actLoginGiftDT.iType1, actLoginGiftDT.iID1, actLoginGiftDT.iCount1));
            m_listItem.Add(new ItemShowGoodItem((EM_ResourceType)actLoginGiftDT.iType2, actLoginGiftDT.iID2, actLoginGiftDT.iCount2));
            m_listItem.Add(new ItemShowGoodItem((EM_ResourceType)actLoginGiftDT.iType3, actLoginGiftDT.iID3, actLoginGiftDT.iCount3));
            m_listItem.Add(new ItemShowGoodItem((EM_ResourceType)actLoginGiftDT.iType4, actLoginGiftDT.iID4, actLoginGiftDT.iCount4));
            param.m_listItem = m_listItem;
            param.m_title = CommonTools.f_GetTransLanguage(1341);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.ItemsShowPage, UIMessageDef.UI_OPEN, param);
        }
    }

    #region 领取回调
    /// <summary>
    /// 领取成功回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnGetSucCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        //更新UI显示
        UpdateContent();
        List<AwardPoolDT> awardList = new List<AwardPoolDT>();
        AwardPoolDT item1 = new AwardPoolDT();
        item1.f_UpdateByInfo((byte)currentSelectLoginGiftDT.iType1, currentSelectLoginGiftDT.iID1, currentSelectLoginGiftDT.iCount1);
        awardList.Add(item1);
        AwardPoolDT item2 = new AwardPoolDT();
        item2.f_UpdateByInfo((byte)currentSelectLoginGiftDT.iType2, currentSelectLoginGiftDT.iID2, currentSelectLoginGiftDT.iCount2);
        awardList.Add(item2);
        AwardPoolDT item3 = new AwardPoolDT();
        item3.f_UpdateByInfo((byte)currentSelectLoginGiftDT.iType3, currentSelectLoginGiftDT.iID3, currentSelectLoginGiftDT.iCount3);
        awardList.Add(item3);
        AwardPoolDT item4 = new AwardPoolDT();
        item4.f_UpdateByInfo((byte)currentSelectLoginGiftDT.iType4, currentSelectLoginGiftDT.iID4, currentSelectLoginGiftDT.iCount4);
        awardList.Add(item4);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
            new object[] { awardList });
    }
    private void OnGetFailCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1342) + CommonTools.f_GetTransLanguage((int)obj));
    }
    /// <summary>
    /// 查询成功回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnQuerySucCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        //更新UI显示
        UpdateContent();
    }
    private void OnQueryFailCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1343) + CommonTools.f_GetTransLanguage((int)obj));
    }
    #endregion
}

