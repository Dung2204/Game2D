using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 推荐阵容界面
/// </summary>
public class RecommendTeamPage : UIFramwork
{
    private GameObject BtnWei;//按钮魏国
    private GameObject BtnShu;//按钮蜀国
    private GameObject BtnWu;//按钮吴国
    private GameObject BtnGroupHero;//按钮群雄
    
    //private UILabel LabelTitle;//标题

    private List<RecommendTeamDT> listTeamDT = new List<RecommendTeamDT>();
    int indexListReamDT;//当前显示的序号
    private bool isFirstEnter = true;
    /// <summary>
    /// 界面开启
    /// </summary>
    /// <param name="e"></param>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        if (isFirstEnter)
        {
            isFirstEnter = false;
            //默认点击魏国
            OnTapItemClick(null, EM_CardCamp.eCardWei, null);
        }
        else
        {
            ShowRoleInfo(listTeamDT[indexListReamDT]);
        }
    }
    /// <summary>
    /// 展示阵营的卡牌列表
    /// </summary>
    /// <param name="cardCamp">阵营类型</param>
    /// <returns></returns>
    private void ShowCampContent(EM_CardCamp cardCamp)
    {
        listTeamDT.Clear();
        List<NBaseSCDT> listDT = glo_Main.GetInstance().m_SC_Pool.m_RecommendTeamSC.f_GetAll();
        for (int i = 0; i < listDT.Count; i++)
        {
            RecommendTeamDT teamDT = listDT[i] as RecommendTeamDT;
            if (teamDT.iCardCamp == (int)cardCamp)
            {
                listTeamDT.Add(teamDT);
            }
        }
        indexListReamDT = 0;
        ShowRoleInfo(listTeamDT[indexListReamDT]);
    }
    /// <summary>
    /// 显示阵容信息
    /// </summary>
    /// <param name="teamDT">dt</param>
    private void ShowRoleInfo(RecommendTeamDT teamDT)
    {
        //f_GetObject("LabelTitle").GetComponent<UILabel>().text = teamDT.szName;
        f_GetObject("LabelDesc").GetComponent<UILabel>().text = teamDT.szDesc;
        CreateRole(f_GetObject("Pos1"), f_GetObject("NameRoot1"), teamDT.iPos1, 28);
        CreateRole(f_GetObject("Pos2"), f_GetObject("NameRoot2"), teamDT.iPos2, 26);
        CreateRole(f_GetObject("Pos3"), f_GetObject("NameRoot3"), teamDT.iPos3, 24);
        CreateRole(f_GetObject("Pos4"), f_GetObject("NameRoot4"), teamDT.iPos4, 22);
        CreateRole(f_GetObject("Pos5"), f_GetObject("NameRoot5"), teamDT.iPos5, 20);
        CreateRole(f_GetObject("Pos6"), f_GetObject("NameRoot6"), teamDT.iPos6, 18);
        f_GetObject("BtnLeftDir").SetActive(indexListReamDT > 0);
        f_GetObject("BtnRightDir").SetActive(indexListReamDT < (listTeamDT.Count - 1));

        f_RegClickEvent("Pos1", OnCardIconClick, teamDT.iPos1);
        f_RegClickEvent("Pos2", OnCardIconClick, teamDT.iPos2);
        f_RegClickEvent("Pos3", OnCardIconClick, teamDT.iPos3);
        f_RegClickEvent("Pos4", OnCardIconClick, teamDT.iPos4);
        f_RegClickEvent("Pos5", OnCardIconClick, teamDT.iPos5);
        f_RegClickEvent("Pos6", OnCardIconClick, teamDT.iPos6);
    }
    /// <summary>
    /// 创建角色
    /// </summary>
    /// <param name="ObjParent"></param>
    /// <param name="NameRoot"></param>
    /// <param name="cardTempId"></param>
    private void CreateRole(GameObject ObjParent,GameObject NameRoot,int cardTempId, int layer = 18)
    {
        if (ObjParent.transform.Find("Model") != null)
        {
            UITool.f_DestoryStatelObject(ObjParent.transform.Find("Model").gameObject);
        }
        for (int i = ObjParent.transform.childCount - 1; i >= 0; i--)
        {
            //GameObject.DestroyImmediate(ObjParent.transform.GetChild(i));
        }
        if (cardTempId <= 0)
        {
            NameRoot.SetActive(false);
            return;
        }
        NameRoot.SetActive(true);
        string cardName = "";
        int important = 0;
        if (cardTempId == 1000)
        {
            CardPoolDT cardPoolDT = Data_Pool.m_TeamPool.f_GetFormationPosCardPoolDT(EM_FormationPos.eFormationPos_Main);
            cardName = Data_Pool.m_UserData.m_szRoleName;
            important = cardPoolDT.m_CardDT.iImportant;
            UITool.f_GetStatelObject(cardPoolDT, ObjParent.transform, new Vector3(0, 180, 0), new Vector3(0, 30, 0), layer, "Model", 110, false);
        }
        else
        {
            CardDT cardDT = glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(cardTempId) as CardDT;
            cardName = cardDT.szName;
            important = cardDT.iImportant;
            UITool.f_GetStatelObject(cardTempId, ObjParent.transform, new Vector3(0, 180, 0), new Vector3(0, 30, 0), layer, "Model", 110, false);
        }
        char[] strArray = cardName.ToCharArray();
        string RoleName = "";
        for (int j = 0; j < strArray.Length; j++)
        {
            RoleName += strArray[j];
            if (j != strArray.Length - 1)
                RoleName += "\n";
        }
        UITool.f_GetImporentColorName(important, ref RoleName);
        NameRoot.transform.Find("LabelName").GetComponent<UILabel>().text = RoleName;
    }
    /// <summary>
    /// 卡牌被点击跳转至信息界面
    /// </summary>
    private void OnCardIconClick(GameObject go, object obj1, object obj2)
    {
        int cardTempId = (int)obj1;
        if (cardTempId > 0)
        {
            if (cardTempId == 1000)
            {
                //ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "点击了主角");
            }
            else
            {
                CardPoolDT cardPoolDT = new CardPoolDT();
                cardPoolDT.m_CardDT = glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(cardTempId) as CardDT;
                CardBox tmp = new CardBox();
                tmp.m_Card = cardPoolDT;
                tmp.m_bType = CardBox.BoxType.Intro;
                tmp.m_oType = CardBox.OpenType.handbook;

                ccUIHoldPool.GetInstance().f_Hold(ccUIManage.GetInstance().f_GetUIHandler(UINameConst.MainMenu));
                ccUIHoldPool.GetInstance().f_Hold(this);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.CardProperty, UIMessageDef.UI_OPEN, tmp);
            }
        }
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
        f_RegClickEvent(f_GetObject("BtnLeftDir"), OnBtnLeftDirClick);
        f_RegClickEvent(f_GetObject("BtnRightDir"), OnBtnRightDirClick);
    }
    /// <summary>
    /// 初始化UI组件
    /// </summary>
    private void InitUI()
    {
        BtnWei = f_GetObject("BtnWei");
        BtnShu = f_GetObject("BtnShu");
        BtnWu = f_GetObject("BtnWu");
        BtnGroupHero = f_GetObject("BtnGroupHero");
        //LabelTitle = f_GetObject("LabelTitle").GetComponent<UILabel>();
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
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RecommendTeamPage, UIMessageDef.UI_CLOSE);
    }
    /// <summary>
    /// 点击关闭按钮关闭界面
    /// </summary>
    private void OnBtnCloseClick(GameObject go, object obj1, object obj2)
    {
        glo_Main.GetInstance().m_AdudioManager.f_PlayAudioButtle(AudioButtle.ButtonNormal);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.RecommendTeamPage, UIMessageDef.UI_CLOSE);
    }
    /// <summary>
    /// 点击左箭头键
    /// </summary>
    private void OnBtnLeftDirClick(GameObject go, object obj1, object obj2)
    {
        indexListReamDT--;
        ShowRoleInfo(listTeamDT[indexListReamDT]);
    }
    /// <summary>
    /// 点击右箭头键
    /// </summary>
    private void OnBtnRightDirClick(GameObject go, object obj1, object obj2)
    {
        indexListReamDT++;
        ShowRoleInfo(listTeamDT[indexListReamDT]);
    }
    #endregion
}
