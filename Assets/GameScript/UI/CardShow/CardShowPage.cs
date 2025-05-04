using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 卡牌图鉴信息界面
/// </summary>
public class CardShowPage : UIFramwork
{
    private GameObject BtnWei;//按钮魏国
    private GameObject BtnShu;//按钮蜀国
    private GameObject BtnWu;//按钮吴国
    private GameObject BtnGroupHero;//按钮群雄

    private GameObject CardItemParent;//卡牌父物体
    private GameObject TitleExample;//标题预设
    private GameObject CardItemExample;//卡牌item预设物体（包含6个）

    private int curLine = 0;//当前位置
    private int cardHeight = 440;//卡牌的高度
    private int titleHeight = 178;//标题高度
    private int offsetHeight = 165;//卡牌和标题间隔
    private ccUIBase baseUI = null;

    private CardShowParam Param;
    /// <summary>
    /// item更新
    /// </summary>
    private void OnItemUpdate(Transform item, CardDT dt)
    {
        CardDT cardDT = dt as CardDT;
        bool isHas = CheckHasCard(cardDT.iId);

        ResourceCommonDT resourceCommonDT = new ResourceCommonDT();
        resourceCommonDT.f_UpdateInfo((byte)EM_ResourceType.Card, cardDT.iId, 1);
        string Name = UITool.f_ReplaceName(resourceCommonDT.mName, " ", "\n");
        string BorderName = UITool.f_GetImporentCase(resourceCommonDT.mImportant);
        string CampName = UITool.f_GetCampName(cardDT.iCardCamp);
        string BorderBGName = UITool.f_GetImporentBGCase(resourceCommonDT.mImportant);

        item.GetComponent<CardShowCardItemCtl>().SetData(UITool.f_GetCardIcon(cardDT.iStatelId1, "L2_"), BorderName, Name, isHas, CampName, BorderBGName, (EM_CardFightType)cardDT.iCardFightType);
        f_RegClickEvent(item.GetComponent<CardShowCardItemCtl>().m_btnIcon.gameObject, OnCardIconClick, dt);
    }
    /// <summary>
    /// 检测某个卡牌模板id是否已经收集
    /// </summary>
    /// <param name="cardID">模板id</param>
    /// <returns></returns>
    private bool CheckHasCard(int cardID)
    {
        List<BasePoolDT<long>> listAllCard = Data_Pool.m_CardPool.f_GetAll();
        for (int i = 0; i < listAllCard.Count; i++)
        {
            CardPoolDT data = listAllCard[i] as CardPoolDT;
            if (cardID == data.m_CardDT.iId)
                return true;
        }
        return false;
    }
    /// <summary>
    /// 卡牌icon被点击跳转至信息界面
    /// </summary>
    private void OnCardIconClick(GameObject go, object obj1, object obj2)
    {
        CardPoolDT cardPoolDT = new CardPoolDT();
        cardPoolDT.m_CardDT = obj1 as CardDT;
        CardBox tmp = new CardBox();
        tmp.m_Card = cardPoolDT;
        tmp.m_bType = CardBox.BoxType.Intro;
        tmp.m_oType = CardBox.OpenType.handbook;
        //通知HoldPool保存当前页
        if (baseUI != null)
            ccUIHoldPool.GetInstance().f_Hold(baseUI);
        else
            ccUIHoldPool.GetInstance().f_Hold(ccUIManage.GetInstance().f_GetUIHandler(UINameConst.MainMenu));

        ccUIHoldPool.GetInstance().f_Hold(this);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CardProperty, UIMessageDef.UI_OPEN, tmp);
    }
    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
    }
    /// <summary>
    /// 界面开启
    /// </summary>
    /// <param name="e"></param>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        Param = null;
        if (e != null && !(e is CardShowParam))
            baseUI = (ccUIBase)e;
        if (e is CardShowParam)
        {
            Param = (CardShowParam)e;
            baseUI = Param.m_ccUIBase;
        }



        //默认点击魏国
        OnTapItemClick(null, EM_CardCamp.eCardWei, null);
    }
    /// <summary>
    /// 展示阵营的卡牌列表
    /// </summary>
    /// <param name="cardCamp">阵营类型</param>
    /// <param name="listRedCard">红色卡牌列表</param>
    /// <param name="listOragenCard">橙色卡牌列表</param>
    /// <param name="listPurpleCard">紫色卡牌列表</param>
    /// <param name="listBlueCard">蓝色卡牌列表</param>
    /// <param name="listGreenCard">绿色卡牌列表</param>
    /// <returns></returns>
    private void ShowCampContent(EM_CardCamp cardCamp)
    {
        //1.普通卡牌品质分为红橙紫蓝绿
		List<CardDT> listGoldCard = new List<CardDT>();
        List<CardDT> listRedCard = new List<CardDT>();
        List<CardDT> listOragenCard = new List<CardDT>();
        List<CardDT> listPurpleCard = new List<CardDT>();
        List<CardDT> listBlueCard = new List<CardDT>();
        List<CardDT> listGreenCard = new List<CardDT>();
        //2.判断阵营，加入列表
        List<NBaseSCDT> allListCardData = glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetAll();
        //UISprite Title = f_GetObject("Title").GetComponent<UISprite>();
        if (Param != null)
        {
            List<AwardPoolDT> tList = null;
            //switch (Param.m_EM_RecruitType)
            //{
            //    case EM_RecruitType.NorAd:
            //        Title.spriteName = "sc_font_zjzm";
            //        tList = Data_Pool.m_AwardPool.f_GetAwardPoolDTByAwardId(1);
            //        break;
            //    case EM_RecruitType.GenAd:
            //        Title.spriteName = "sc_font_sjzm";
            //        tList = Data_Pool.m_AwardPool.f_GetAwardPoolDTByAwardId(3);
            //        break;
            //    default:
            //        Title.spriteName = "sc_font_wjtj";
            //        break;
            //}
            allListCardData = new List<NBaseSCDT>();

            if (tList!=null) {
                for (int i = 0; i < tList.Count; i++)
                {
                    allListCardData.Add(glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(tList[i].mTemplate.mResourceId));
                }
            }
        }
           
        for (int index = 0; index < allListCardData.Count; index++)
        {
            CardDT cardDt = allListCardData[index] as CardDT;
            if (cardDt.iCardCamp == (int)cardCamp)
            {
                switch ((EM_Important)cardDt.iImportant)
                {
                    case EM_Important.Green: listGreenCard.Add(cardDt); break;
                    case EM_Important.Blue: listBlueCard.Add(cardDt); break;
                    case EM_Important.Purple: listPurpleCard.Add(cardDt); break;
                    case EM_Important.Oragen: listOragenCard.Add(cardDt); break;
                    case EM_Important.Red: listRedCard.Add(cardDt); break;
					case EM_Important.Gold: listGoldCard.Add(cardDt); break;
                }
            }
        }
        //删除旧资源
        foreach (Transform child in CardItemParent.transform)
        {
            Destroy(child.gameObject);
        }
        curLine = 0;
        //显示颜色卡牌(金阶神将、红阶神将、橙阶名将、紫阶名将、蓝阶战将、绿阶战将)
        string title = CommonTools.f_GetTransLanguage(2263);
		if (listGoldCard.Count != 0)
        {
            UITool.f_GetImporentColorName((int)EM_Important.Gold, ref title);
            ShowListCardWithTitle(listGoldCard, title);
        }
        if (listRedCard.Count != 0)
        {
			title = CommonTools.f_GetTransLanguage(1137);
            UITool.f_GetImporentColorName((int)EM_Important.Red, ref title);
            ShowListCardWithTitle(listRedCard, title);
        }
        if (listOragenCard.Count != 0)
        {
            title = CommonTools.f_GetTransLanguage(1138);
            UITool.f_GetImporentColorName((int)EM_Important.Oragen, ref title);
            ShowListCardWithTitle(listOragenCard, title);
        }
        if (listPurpleCard.Count != 0)
        {
            title = CommonTools.f_GetTransLanguage(1139);
            UITool.f_GetImporentColorName((int)EM_Important.Purple, ref title);
            ShowListCardWithTitle(listPurpleCard, title);
        }
        if (listBlueCard.Count != 0)
        {
            title = CommonTools.f_GetTransLanguage(1140);
            UITool.f_GetImporentColorName((int)EM_Important.Blue, ref title);
            ShowListCardWithTitle(listBlueCard, title);
        }
        if (listGreenCard.Count != 0)
        {
            title = CommonTools.f_GetTransLanguage(1141);
            UITool.f_GetImporentColorName((int)EM_Important.Green, ref title);
            ShowListCardWithTitle(listGreenCard, title);
        }
        //重设坐标
        CardItemParent.transform.parent.GetComponent<UIScrollView>().ResetPosition();
    }
    /// <summary>
    /// 显示待标题的某个颜色的列表卡牌
    /// </summary>
    /// <param name="listCard">某个颜色卡牌列表</param>
    /// <param name="title">标题</param>
    private void ShowListCardWithTitle(List<CardDT> listCard, string title)
    {
        //GameObject titleRed = GameObject.Instantiate(TitleExample) as GameObject;
        //SetObjectPos(titleRed, curLine);
        //titleRed.transform.GetComponentInChildren<UILabel>().text = title;
        //curLine -= titleHeight;
        int length = listCard.Count / 5;
        for (int i = 0; i < length + 1; i++)
        {
            GameObject cardItem = GameObject.Instantiate(CardItemExample) as GameObject;
            SetObjectPos(cardItem, curLine);
            for (int j = 0; j < 5; j++)
            {
                bool isVlid = (i * 5 + j) < listCard.Count;
                Transform Item = cardItem.transform.Find("Item" + j);
                Item.gameObject.SetActive(isVlid);
                if (isVlid)
                {
                    OnItemUpdate(Item, listCard[i * 5 + j]);
                }
            }
            if (i == length)
                curLine -= cardHeight;
            else
                curLine -= cardHeight;
        }
    }
    /// <summary>
    /// 设置物体坐标
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="posY"></param>
    private void SetObjectPos(GameObject obj, float posY)
    {
        obj.transform.SetParent(CardItemParent.transform);
        obj.gameObject.SetActive(true);
        obj.transform.localPosition = new Vector3(0, posY, 0);
        obj.transform.localEulerAngles = Vector3.zero;
        obj.transform.localScale = Vector3.one;
    }
    /// <summary>
    /// 注册UI事件
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        InitUI();
        f_RegClickEvent("BtnBlack", OnBtnBlackClick);
        f_RegClickEvent("BtnClose", OnBtnCloseClick);
        f_RegClickEvent(BtnWei, OnTapItemClick, EM_CardCamp.eCardWei);
        f_RegClickEvent(BtnShu, OnTapItemClick, EM_CardCamp.eCardShu);
        f_RegClickEvent(BtnWu, OnTapItemClick, EM_CardCamp.eCardWu);
        f_RegClickEvent(BtnGroupHero, OnTapItemClick, EM_CardCamp.eCardGroupHero);
    }
    /// <summary>
    /// 初始化UI组件
    /// </summary>
    private void InitUI()
    {
        BtnWei = f_GetObject("BtnWei");
        BtnShu = f_GetObject("BtnShu");
        BtnWu = f_GetObject("BtnWu");
        BtnGroupHero = f_GetObject("BtnGroupHero"); ;

        CardItemParent = f_GetObject("CardItemParent");
        CardItemExample = f_GetObject("CardItemExample");
        TitleExample = f_GetObject("TitleExample");
    }
    /// <summary>
    /// 点击了分页按钮
    /// </summary>
    private void OnTapItemClick(GameObject go, object obj1, object obj2)
    {
        EM_CardCamp cardCamp = (EM_CardCamp)obj1;//卡牌阵营
        SetTapState(BtnWei, cardCamp == EM_CardCamp.eCardWei);
        SetTapState(BtnShu, cardCamp == EM_CardCamp.eCardShu);
        SetTapState(BtnWu, cardCamp == EM_CardCamp.eCardWu);
        SetTapState(BtnGroupHero, cardCamp == EM_CardCamp.eCardGroupHero);
        ShowCampContent(cardCamp);
    }
    /// <summary>
    /// 设置分页状态
    /// </summary>
    private void SetTapState(GameObject tapItem, bool isPress)
    {
        tapItem.transform.Find("Normal").gameObject.SetActive(!isPress);
        tapItem.transform.Find("Press").gameObject.SetActive(isPress);
    }
    #region 按钮事件
    /// <summary>
    /// 点击窗口外黑色背景关闭界面
    /// </summary>
    private void OnBtnBlackClick(GameObject go, object obj1, object obj2)
    {
        baseUI = null;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CardShowPage, UIMessageDef.UI_CLOSE);
    }
    /// <summary>
    /// 点击关闭按钮关闭界面
    /// </summary>
    private void OnBtnCloseClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        baseUI = null;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CardShowPage, UIMessageDef.UI_CLOSE);
    }
    #endregion

    public void f_DestoryView()
    {
        gameObject.SetActive(false);
    }

    public void f_ShowView(ccUIBase actPage)
    {
        gameObject.SetActive(true);
        baseUI = actPage;
        OnTapItemClick(null, EM_CardCamp.eCardWei, null);
    }

    private void OnBtnGotoClick(GameObject go, object obj1, object obj2)
    {
        string uiName = (string)obj1;
        int param = (int)obj2;
        if (uiName != null && uiName != "")
        {
            UITool.f_GotoPage(baseUI, uiName, param);
        }
    }
}

class CardShowParam
{

    public CardShowParam(ccUIBase UI, EM_RecruitType Recruittype)
    {
        m_ccUIBase = UI;
        m_EM_RecruitType = Recruittype;
    }

    public ccUIBase m_ccUIBase
    {
        get;
        private set;
    }

    public EM_RecruitType m_EM_RecruitType
    {
        get;
        private set;
    }

}
