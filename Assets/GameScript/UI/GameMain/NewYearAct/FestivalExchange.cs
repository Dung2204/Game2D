using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccU3DEngine;
using UnityEngine;
class FestivalExchange : UIFramwork
{
    private SocketCallbackDT tGetFestivalExchange = new SocketCallbackDT();  //获取
    private UIWrapComponent tUIWrapComponent;
    private List<ccU3DEngine.BasePoolDT<long>> tData = new List<ccU3DEngine.BasePoolDT<long>>();
    private string strTexAdsRoot = "UI/TextureRemove/Activity/Tex_ActAd";
    UITexture tUITexture;
    private UIWrapComponent mUIWrapComponent
    {
        get
        {
            if(tUIWrapComponent == null)
            {
                tUIWrapComponent = new UIWrapComponent(250, 1, 1, 4, f_GetObject("ItemParent"), f_GetObject("Item"),
                   tData, UpdateItem, null);
            }
            return tUIWrapComponent;
        }
    }

    public void f_ShowView(UIFramwork actPage)
    {
        gameObject.SetActive(true);
        tGetFestivalExchange.m_ccCallbackFail = GetFail;
        tGetFestivalExchange.m_ccCallbackSuc = GetSuc;
        UpdatePool();
		//My Code
		FestivalExchangeDT tFestivalExchangeDT = glo_Main.GetInstance().m_SC_Pool.m_FestivalExchangeSC.f_GetAll()[glo_Main.GetInstance().m_SC_Pool.m_FestivalExchangeSC.f_GetAll().Count -1] as FestivalExchangeDT;
		MessageBox.ASSERT("Begin: " + tFestivalExchangeDT.iBeginTime.ToString() + " End: " + tFestivalExchangeDT.iEndTime.ToString());
		string BeginTime = tFestivalExchangeDT.iBeginTime + "";
		string EndTime = tFestivalExchangeDT.iEndTime + "";
f_GetObject("BeginTime").GetComponent<UILabel>().text = string.Format("Event open time: {0}/{1}/{2} 0:00", BeginTime.Substring(6, 2 ), BeginTime.Substring(4, 2), BeginTime.Substring(0, 4));
		f_GetObject("EndTime").GetComponent<UILabel>().text = string.Format(CommonTools.f_GetTransLanguage(1473), EndTime.Substring(6, 2), EndTime.Substring(4, 2), EndTime.Substring(0, 4));
		//
        mUIWrapComponent.f_UpdateView();


    }

    private void LoadTexture()
    {
        if(tUITexture == null)
        {
            tUITexture = f_GetObject("Texture").GetComponent<UITexture>();
        }
        if(tUITexture.mainTexture == null)
            tUITexture.mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexAdsRoot);
    }

    public void f_DestoryView()
    {
        gameObject.SetActive(false);
    }

    private void UpdateItem(Transform Item, BasePoolDT<long> data)
    {
        FestivalExchangePoolDT tFestivalExchangePoolDT = data as FestivalExchangePoolDT;

        Transform tTran = Item.Find("GoodsItemGrid");
        GameObject Btn = Item.Find("Btn").gameObject;
        GameObject noBtn = Item.Find("NoBtn").gameObject;
        UILabel Num = Item.Find("Num").GetComponent<UILabel>();
        ResourceCommonItem item1 = tTran.GetChild(0).GetComponent<ResourceCommonItem>();
        ResourceCommonItem item2 = tTran.GetChild(1).GetComponent<ResourceCommonItem>();
        ResourceCommonItem item3 = tTran.GetChild(2).GetComponent<ResourceCommonItem>();
        ResourceCommonItem item4 = Item.Find("GoodsItem").GetComponent<ResourceCommonItem>();

        Btn.SetActive(tFestivalExchangePoolDT.IsCanGet);
        noBtn.SetActive(!tFestivalExchangePoolDT.IsCanGet);

        //Btn.SetActive(true);

        string[] tStringArr = tFestivalExchangePoolDT.mFestivalExchangeDT.szResNeed.Split('#');
        item3.gameObject.SetActive(tStringArr.Length >= 3);
        string[] tItem = tStringArr[0].Split(';');
        item1.f_UpdateByInfo(int.Parse(tItem[0]), int.Parse(tItem[1]), int.Parse(tItem[2]));
        if(tStringArr.Length >= 2)
        {
            tItem = tStringArr[1].Split(';');
            item2.f_UpdateByInfo(int.Parse(tItem[0]), int.Parse(tItem[1]), int.Parse(tItem[2]));
        }
        if(tStringArr.Length >= 3)
        {
            tItem = tStringArr[2].Split(';');
            item3.f_UpdateByInfo(int.Parse(tItem[0]), int.Parse(tItem[1]), int.Parse(tItem[2]));
        }
        tItem = tFestivalExchangePoolDT.mFestivalExchangeDT.szResAward.Split(';');
        item4.f_UpdateByInfo(int.Parse(tItem[0]), int.Parse(tItem[1]), int.Parse(tItem[2]));
        Num.text = string.Format(CommonTools.f_GetTransLanguage(1496), tFestivalExchangePoolDT.mNum, tFestivalExchangePoolDT.mFestivalExchangeDT.iCount);
        f_RegClickEvent(Btn, Btn_Get, tFestivalExchangePoolDT);

    }
    private void Btn_Get(GameObject go, object obj1, object obj2)
    {
        FestivalExchangePoolDT Goods = (FestivalExchangePoolDT)obj1;
        string[] tStringArr = Goods.mFestivalExchangeDT.szResNeed.Split('#');
        string[] item;
        ResourceCommonDT tResourceCommonDT = new ResourceCommonDT();
        int type = 0;
        int num = 0;
        int id = 0;
        int NeedNum = 0;
        string titp = string.Empty;
        for(int i = 0; i < tStringArr.Length; i++)
        {
            item = tStringArr[i].Split(';');
            type = int.Parse(item[0]);
            id = int.Parse(item[1]);
            num = int.Parse(item[2]);
            tResourceCommonDT.f_UpdateInfo((byte)type, id, num);
            NeedNum = UITool.f_GetResourceHaveNum(type, id);
            if(tResourceCommonDT.mResourceNum > NeedNum)
            {
                titp += string.Format(CommonTools.f_GetTransLanguage(1497), tResourceCommonDT.mName, tResourceCommonDT.mResourceNum - NeedNum);
            }
            if(titp != string.Empty)
                if(i < tStringArr.Length - 1)
                    titp += "\n";
        }
        if(titp != string.Empty)
        {
            UITool.Ui_Trip(titp);
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_NewYearActivityPool.f_GetFestivaExchange(Goods.mId, tGetFestivalExchange);
    }
    private void GetSuc(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        mUIWrapComponent.f_UpdateView();
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1498));
    }

    private void GetFail(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        UITool.Ui_Trip(CommonTools.f_GetTransLanguage(1499) + obj.ToString());
    }

    private void UpdatePool()
    {
        for(int i = 0; i < Data_Pool.m_NewYearActivityPool.mFestivalExchangePoolDTData.Count; i++)
        {
            if(tData.Find((BasePoolDT<long> a) => { return ((FestivalExchangePoolDT)a).mId == Data_Pool.m_NewYearActivityPool.mFestivalExchangePoolDTData[i].mId; }) == null)
                tData.Add(Data_Pool.m_NewYearActivityPool.mFestivalExchangePoolDTData[i]);
        }
    }
}

