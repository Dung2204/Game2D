using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 在线豪礼页面
/// </summary>
public class OnlineAwardCtl : UIFramwork
{
    private SocketCallbackDT QueryCallback = new SocketCallbackDT();//查询回调
    private SocketCallbackDT RequestGetCallback = new SocketCallbackDT();//领取回调 (添加购买回调)
    private List<BasePoolDT<long>> listOnlineAwardPoolDT = new List<BasePoolDT<long>>();
    OnlineAwardPoolDT currentSelectPoolDT = null;
    private string strTexBgRoot = "UI/TextureRemove/Activity/Tex_OlineAwardBg";
    private string strTexProgressRoot = "UI/TextureRemove/Activity/Tex_OnlineProgress";
    private string strTexCircleRoot = "UI/TextureRemove/Activity/Tex_OnlineCircle";
    private GameObject spineMagic = null;
    public void f_DestoryView()
    {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 初始化数据,视图
    /// </summary>
    public void f_ShowView()
    {
        spineMagic = f_GetObject("spineMagic");
        gameObject.SetActive(true);
        RequestGetCallback.m_ccCallbackSuc = OnGetSucCallback;
        RequestGetCallback.m_ccCallbackFail = OnGetFailCallback;

        QueryCallback.m_ccCallbackSuc = OnQuerySucCallback;
        QueryCallback.m_ccCallbackFail = OnQueryFailCallback;
        UITool.f_OpenOrCloseWaitTip(true);
        Data_Pool.m_OnlineAwardPool.f_QueryInfo(QueryCallback);
        f_RegClickEvent(f_GetObject("BtnGet"), OnGetAwardClick);
        f_GetObject("BtnGet").SetActive(false);
        f_LoadTexture();
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

            //UITexture TexProgress = f_GetObject("TexProgress").GetComponent<UITexture>();
            //Texture2D tTexProgress = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexProgressRoot);
            //TexProgress.mainTexture = tTexProgress;

            //UITexture TexCircle1 = f_GetObject("Tex_OnlineCircle1").GetComponent<UITexture>();
            //UITexture TexCircle2 = f_GetObject("Tex_OnlineCircle2").GetComponent<UITexture>();
            //Texture2D tTexCircle = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexCircleRoot);
            //TexCircle1.mainTexture = tTexCircle;
            //TexCircle2.mainTexture = tTexCircle;
        }

        GameObject effect = null;
        GameObject effect2 = null;
        if (spineMagic.transform.childCount == 0)
        {
            UITool.f_CreateMagicById((int)EM_MagicId.eOnlineAwardBg, ref effect, spineMagic.transform, 1, "animation",null, true, 100);
            UITool.f_CreateMagicById((int)EM_MagicId.eOnlineAwardGet, ref effect2, f_GetObject("BtnGet").transform, 3, "animation3", null, true, 160);

        }
    }
    /// <summary>
    /// 更新内容
    /// </summary>
    private void UpdateContent()
    {
        f_GetObject("Content").SetActive(true);
        listOnlineAwardPoolDT = CommonTools.f_CopyPoolDTArrayToNewList(Data_Pool.m_OnlineAwardPool.f_GetAll());
        GameObject ItemRoot = f_GetObject("ItemRoot");
        ItemRoot.SetActive(true);
        int timeSec = Data_Pool.m_OnlineAwardPool.m_timeSecondToday;
        int count = 0;
        float subProgressValue = 1 * 1.0f / 8;
        float progress = 0;
        int maxIndex = 0;
        bool isFindCanGetIndex = false;
        int FindCanGetIndex = 0;
        for (int i = 0; i < 8; i++)
        {
            Transform item = ItemRoot.transform.Find("Item" + i);
            OnlineAwardPoolDT onlineAwardPoolDT = listOnlineAwardPoolDT[i] as OnlineAwardPoolDT;
            OnlineAwardDT onlineAwardDT = onlineAwardPoolDT.m_OnlineAwardDT;
            string[] listAwardText = onlineAwardDT.szAward.Split(';');
            item.Find("SprItemSelect").gameObject.SetActive(false);
            item.Find("SprHasGet").gameObject.SetActive(false);
            item.GetComponent<OnlineAwardItem>().SetData((EM_ResourceType)(int.Parse(listAwardText[0])), int.Parse(listAwardText[1]), int.Parse(listAwardText[2]), onlineAwardPoolDT.mTime);
            //f_RegClickEvent(item.gameObject, OnItemIconClick, onlineAwardPoolDT, true);
            item.Find("reddot").gameObject.SetActive(false);
            ResourceCommonDT dt = new ResourceCommonDT();
            dt.f_UpdateInfo((byte)(int.Parse(listAwardText[0])), int.Parse(listAwardText[1]), int.Parse(listAwardText[2]));
            //if (item.transform.Find("effect") != null)
            //    GameObject.DestroyImmediate(item.transform.Find("effect").gameObject);
            if ((timeSec / 60) >= onlineAwardPoolDT.mTime)
            {
                progress += subProgressValue;
                maxIndex = i;
                if (onlineAwardPoolDT.m_isGet)
                {
                    count++;
                    item.Find("SprHasGet").gameObject.SetActive(true);
                }
                else
                {
                    if (!isFindCanGetIndex)
                    {
                        isFindCanGetIndex = true;
                        FindCanGetIndex = i;
                    }
                    item.Find("reddot").gameObject.SetActive(true);
                    //UITool.f_CreateEquipEffect(item.transform, "effect", (EM_Important)dt.mImportant, new Vector3(0, 0, 0), new Vector3(160, 160, 160));
                }
            }
        }
        //if (maxIndex < 7)
        //{
        //    OnlineAwardPoolDT onlineAwardPoolDT = listOnlineAwardPoolDT[maxIndex] as OnlineAwardPoolDT;
        //    OnlineAwardPoolDT onlineAwardPoolDTNext = listOnlineAwardPoolDT[maxIndex + 1] as OnlineAwardPoolDT;
        //    float value1 = timeSec * 1.0f / 60 - onlineAwardPoolDT.m_OnlineAwardDT.iTime;
        //    float value2 = onlineAwardPoolDTNext.m_OnlineAwardDT.iTime - onlineAwardPoolDT.m_OnlineAwardDT.iTime;
        //    if (value1 < 0)//第一个也没超过
        //    {
        //        value1 = timeSec * 1.0f / 60;
        //        value2 = onlineAwardPoolDT.mTime;
        //    }
        //    progress += subProgressValue * (value1 * 1.0f / value2);
        //}
        //else
        //    progress = 1;
        f_GetObject("BtnGet").SetActive(true);
        //UITool.f_CreateMagicById();
        count = count > 7 ? 7 : count;
        if (isFindCanGetIndex)
            count = FindCanGetIndex;
        OnItemIconClick(ItemRoot.transform.Find("Item" + count).gameObject, listOnlineAwardPoolDT[count], false);
        f_GetObject("LabelOnlineTimeHint").GetComponent<UILabel>().text = "[6A321B]" + string.Format(CommonTools.f_GetTransLanguage(1363), "[FFF6DA]"+timeSec / 60);
        f_GetObject("TexProgress").GetComponent<UITexture>().fillAmount = progress;
    }
    private void OnItemIconClick(GameObject item, object obj1, object obj2)
    {
        //1.设置当前点击的poolDT
        OnlineAwardPoolDT onlineAwardPoolDT = obj1 as OnlineAwardPoolDT;
        currentSelectPoolDT = onlineAwardPoolDT;
        //2.设置选中
        GameObject ItemRoot = f_GetObject("ItemRoot");
        for (int i = 0; i < 8; i++)
        {
            Transform itemI = ItemRoot.transform.Find("Item" + i);
            itemI.Find("SprItemSelect").gameObject.SetActive(false);
        }
        item.transform.Find("SprItemSelect").gameObject.SetActive(true);
        //3.设置是否可领取显示
        int timeSec = Data_Pool.m_OnlineAwardPool.m_timeSecondToday;
        GameObject btnGet = f_GetObject("BtnGet");
        UITool.f_SetSpriteGray(btnGet.transform.GetComponent<UISprite>(), true);
        if (btnGet.transform.childCount > 0)
        {
            btnGet.transform.GetChild(0).gameObject.SetActive((timeSec / 60) >= onlineAwardPoolDT.mTime && !onlineAwardPoolDT.m_isGet);
        }

        if ((timeSec / 60) >= onlineAwardPoolDT.mTime)
        {
            if (!onlineAwardPoolDT.m_isGet)
            {
                //UITool.f_SetSpriteGray(f_GetObject("BtnGet").GetComponent<UISprite>(), false);
                //if ((bool)obj2)//按钮点击
                    //OnGetAwardClick(null, null, null);
            }
        }
    }
    /// <summary>
    /// 点击领取奖励事件
    /// </summary>
    private void OnGetAwardClick(GameObject go,object obj1,object obj2)
    {
        if (currentSelectPoolDT == null)
            return;
        int timeSec = Data_Pool.m_OnlineAwardPool.m_timeSecondToday;
        if ((timeSec / 60) >= currentSelectPoolDT.mTime)
        {
            if (!currentSelectPoolDT.m_isGet)
            {
                UITool.f_OpenOrCloseWaitTip(true);
                Data_Pool.m_OnlineAwardPool.f_GetAward(currentSelectPoolDT.m_OnlineAwardDT.iId, RequestGetCallback);
            }
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
        //先显示奖励，再更新UI
        string[] listAwardText = currentSelectPoolDT.m_OnlineAwardDT.szAward.Split(';');
        List<AwardPoolDT> awardList = new List<AwardPoolDT>();
        AwardPoolDT item1 = new AwardPoolDT();
        item1.f_UpdateByInfo((byte)(int.Parse(listAwardText[0])), int.Parse(listAwardText[1]), int.Parse(listAwardText[2]));
        awardList.Add(item1);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.GainAwardShowPage, UIMessageDef.UI_OPEN,
            new object[] { awardList });
        //更新UI显示
        UpdateContent();
    }
    private void OnGetFailCallback(object obj)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        eMsgOperateResult teMsgOperateResult = (eMsgOperateResult)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1364) + CommonTools.f_GetTransLanguage((int)obj));
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
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, CommonTools.f_GetTransLanguage(1365) + CommonTools.f_GetTransLanguage((int)obj));
    }
    #endregion
}

