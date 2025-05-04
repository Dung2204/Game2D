using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;
using Spine;

public class MainLineTreasureBookPage : UIFramwork
{
    private GameObject mStarItem;
    private GameObject mStarBtn;
    private GameObject mEliteBtn;
    private GameObject mStarPanel;
    private GameObject mStarViewGrid;

    private GameObject mElitePanel;
    private GameObject mEliteLeftBtn;
    private GameObject mEliteRightBtn;
    private GameObject mEliteWeiBtn;
    private GameObject mEliteShuBtn;
    private GameObject mEliteWuBtn;
    private GameObject mEliteQunBtn;

    private GameObject mEliteChipItemWei;
    private GameObject mEliteChipItemShu;
    private GameObject mEliteChipItemWu;
    private GameObject mEliteChipItemQun;
    private UIGrid mEliteChipItemGridWei;
    private UIGrid mEliteChipItemGridShu;
    private UIGrid mEliteChipItemGridWu;
    private UIGrid mEliteChipItemGridQun;
    private GameObject mEliteSweepBtn;
    private UIGrid mEliteDungeonItemGrid;

    private UILabel mLabNostarLevel;
    private GameObject mNoMaxStarLevelTip;

    private int mMaxInitChapterNum = 0;
    private int mCurInitChapterNum = 0;
    private bool mInitComplete = false;

    protected override void f_CustomAwake()
    {
        InitGUI();
    }

    /// <summary>
    /// 初始化消息
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BtnClose", OnBlackBGClick);
        f_RegClickEvent("BtnBlack", OnBlackBGClick);
        f_RegClickEvent("CloseMaskObjDrag2", ObjDrag2Close);
        f_RegClickEvent(mStarBtn, f_OnClickToggle);
        f_RegClickEvent(mEliteBtn, f_OnClickToggle);
        f_RegClickEvent(mEliteWeiBtn, f_OnClickEliteBtnGroud, EM_CardCamp.eCardWei);
        f_RegClickEvent(mEliteShuBtn, f_OnClickEliteBtnGroud, EM_CardCamp.eCardShu);
        f_RegClickEvent(mEliteWuBtn, f_OnClickEliteBtnGroud, EM_CardCamp.eCardWu);
        f_RegClickEvent(mEliteQunBtn, f_OnClickEliteBtnGroud, EM_CardCamp.eCardGroupHero);
        f_RegClickEvent(mEliteSweepBtn, f_OnClickEliteSweepBtn);
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mStarItem = f_GetObject("starItem");
        mStarPanel = f_GetObject("starPanel");
        mStarViewGrid = f_GetObject("Grid");
        mStarBtn = f_GetObject("BtnStar");
        mEliteBtn = f_GetObject("BtnElite");

        mElitePanel = f_GetObject("elitePanel");
        mEliteLeftBtn = f_GetObject("leftBtn");
        mEliteRightBtn = f_GetObject("rightBtn");
        mEliteWeiBtn = f_GetObject("BtnWei");
        mEliteShuBtn = f_GetObject("BtnShu");
        mEliteWuBtn = f_GetObject("BtnWu");
        mEliteQunBtn = f_GetObject("BtnQun");

        mEliteChipItemWei = f_GetObject("ChipItemWei");
        mEliteChipItemShu = f_GetObject("ChipItemShu");
        mEliteChipItemWu  = f_GetObject("ChipItemWu");
        mEliteChipItemQun = f_GetObject("ChipItemQun");
        mEliteChipItemGridWei = f_GetObject("EliteChipGridWei").GetComponent<UIGrid>();
        mEliteChipItemGridShu = f_GetObject("EliteChipGridShu").GetComponent<UIGrid>();
        mEliteChipItemGridWu = f_GetObject("EliteChipGridWu").GetComponent<UIGrid>();
        mEliteChipItemGridQun = f_GetObject("EliteChipGridQun").GetComponent<UIGrid>();
        mEliteSweepBtn = f_GetObject("sweepBtn");
        mEliteDungeonItemGrid = f_GetObject("EliteItemGrid").GetComponent<UIGrid>();
        mLabNostarLevel = f_GetObject("NoStarLevel").GetComponent<UILabel>();
        mNoMaxStarLevelTip = f_GetObject("NoMaxStarLevel");
    }

    /// <summary>
    /// 打开界面
    /// </summary>
    /// <param name="e"></param>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_DUNGEON_CHAPTER_UPDATE, f_CallBackByInit, this);

        if (mInitComplete)
        {
            f_InitStarView();
        }
        else
        {
            f_GetAllData();
        }
        f_OnClickToggle(mStarBtn, null, null);
    }

    protected override void UI_HOLD(object e)
    {
        base.UI_HOLD(e);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_DUNGEON_CHAPTER_UPDATE, f_CallBackByInit, this);
    }

    protected override void UI_UNHOLD(object e)
    {
        base.UI_UNHOLD(e);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_DUNGEON_CHAPTER_UPDATE, f_CallBackByInit, this);
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_DUNGEON_CHAPTER_UPDATE, f_CallBackByInit, this);
    }

   /// <summary>
   /// 点击一键扫荡按钮
   /// </summary>
   /// <param name="go"></param>
   /// <param name="obj1"></param>
   /// <param name="obj2"></param>
    private void f_OnClickEliteSweepBtn(GameObject go, object obj1, object obj2)
    {
        if (sweepDungeonTollgatePoolDts.Count == 0)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(2166));

            //ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "没有可扫荡的关卡");
            return;
        }
        
        int mMaxSweepNum = 0;
        for (int i = 0; i < sweepDungeonTollgatePoolDts.Count; i++)
        {
            mMaxSweepNum = sweepDungeonTollgatePoolDts[i].mTollgateTemplate.iCountLimit - sweepDungeonTollgatePoolDts[i].mTimes + mMaxSweepNum;
        }
        if (mMaxSweepNum == 0)
        {
            UITool.Ui_Trip(CommonTools.f_GetTransLanguage(2175));
            //ccUIManage.GetInstance().f_SendMsg(UINameConst.LabelTipPage, UIMessageDef.UI_OPEN, "关卡挑战次数不足，请重置关卡");
            return;
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.MainLineOneKeySweepPage, UIMessageDef.UI_OPEN, mSelectDt);
    }
    /// <summary>
    /// 点击阵营按钮
    /// </summary>
    /// <param name="go"></param>
    /// <param name="obj1"></param>
    /// <param name="obj2"></param>
   private void f_OnClickEliteBtnGroud(GameObject go, object obj1, object obj2)
    {
        //EM_CardCamp camp = (EM_CardCamp) obj1;
        //SetTapState(mEliteWeiBtn, camp == EM_CardCamp.eCardWei);
        //SetTapState(mEliteShuBtn, camp == EM_CardCamp.eCardShu);
        //SetTapState(mEliteWuBtn, camp == EM_CardCamp.eCardWu);
        //SetTapState(mEliteQunBtn, camp == EM_CardCamp.eCardGroupHero);

        f_ShowEliteChipWei((int)EM_CardCamp.eCardWei);
        f_ShowEliteChipShu((int)EM_CardCamp.eCardShu);
        f_ShowEliteChipWu((int)EM_CardCamp.eCardWu);
        f_ShowEliteChipQun((int)EM_CardCamp.eCardGroupHero);
    }
    /// <summary>
    /// 点击页脚标签
    /// </summary>
    /// <param name="go"></param>
    /// <param name="obj1"></param>
    /// <param name="obj2"></param>
    private void f_OnClickToggle(GameObject go, object obj1, object obj2)
    {
        if (go.name == "BtnStar")
        {
            mStarPanel.SetActive(true);
            mElitePanel.SetActive(false);
            SetTapState(mStarBtn, true);
            SetTapState(mEliteBtn, false);
            if (mInitComplete) f_InitStarView();
            else f_GetAllData();
        }
        else if (go.name == "BtnElite")
        {
            mStarPanel.SetActive(false);
            mElitePanel.SetActive(true);
            SetTapState(mStarBtn, false);
            SetTapState(mEliteBtn, true);
            f_OnClickEliteBtnGroud(mEliteWeiBtn, EM_CardCamp.eCardWei, null);
        }
    }
    /// <summary>
    /// 卡牌碎片阵营缓存
    /// </summary>
    Dictionary<int, List<CardFragmentDT>> dicCamp = new Dictionary<int, List<CardFragmentDT>>();
    /// <summary>
    /// 根据阵营获取碎片数据
    /// </summary>
    /// <param name="camp"></param>
    /// <returns></returns>
    private List<CardFragmentDT> f_GetScData(int camp)
    {
        if (dicCamp.Count == 0)
        {
            List<NBaseSCDT> list = glo_Main.GetInstance().m_SC_Pool.m_CardFragmentSC.f_GetAll();
            for (int i = 0; i < list.Count; i++)
            {
                CardFragmentDT dt = list[i] as CardFragmentDT;
                if (!dt.szStage.Equals(""))
                {
                    CardDT cardDt = glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(dt.iNewCardId) as CardDT;
                    if (dicCamp.ContainsKey(cardDt.iCardCamp))
                    {
                        dicCamp[cardDt.iCardCamp].Add(dt);
                    }
                    else
                    {
                        dicCamp[cardDt.iCardCamp] = new List<CardFragmentDT>();
                        dicCamp[cardDt.iCardCamp].Add(dt);
                    }
                }
            }          
        }

        return dicCamp.ContainsKey(camp) ? dicCamp[camp] : new List<CardFragmentDT>();
    }

    /// <summary>
    /// 根据卡牌碎片获取扫荡关卡数据
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    private List<DungeonTollgatePoolDT> f_GetSweepPoolDts(CardFragmentDT dt)
    {
        string[] ids = dt.szStage.Split(new string[] { ";" }, StringSplitOptions.None);
        List<DungeonTollgatePoolDT> poolDt = new List<DungeonTollgatePoolDT>();//Data_Pool.m_DungeonPool.f_GetAllNoMaxStar((int)EM_Fight_Enum.eFight_DungeonMain);

        for (int i = 0; i < ids.Length - 1; i++)
        {
            int id = int.Parse(ids[i]);
            DungeonTollgateDT dungeonTollgateDt = glo_Main.GetInstance().m_SC_Pool.m_DungeonTollgateSC.f_GetSC(id) as DungeonTollgateDT;
            if (null == dungeonTollgateDt)
            {
MessageBox.ASSERT("f_GetSweepPoolDts，no chapter data，id：" + id);
                continue;
            }
            DungeonPoolDT dungeonPoolDt = Data_Pool.m_DungeonPool.f_GetForId(dungeonTollgateDt.iDungeonChapter) as DungeonPoolDT;
            if (null == dungeonPoolDt)
            {
MessageBox.ASSERT("f_GetSweepPoolDts，no server chapter data，id：" + dungeonTollgateDt.iDungeonChapter);
                continue;
            }
            DungeonTollgatePoolDT dungeonTollgatePoolDt = dungeonPoolDt.f_GetTollgateData(id);
            if (null == dungeonTollgatePoolDt)
            {
MessageBox.ASSERT("f_GetSweepPoolDts，no screen data，id：" + id);
                continue;
            }
            if (dungeonTollgatePoolDt.mStarNum >= 3)
            {
                poolDt.Add(dungeonTollgatePoolDt);
            }
        }

        return poolDt;
    }

    private GameObject lastItem = null;
    private bool isFirstClickElite = false;
    /// <summary>
    /// 更新碎片item
    /// </summary>
    /// <param name="item"></param>
    /// <param name="dt"></param>
    private void f_UpdateChipItem(GameObject item, CardFragmentDT dt)
    {
        UI2DSprite icon = item.transform.Find("icon").GetComponent<UI2DSprite>();
        icon.sprite2D = UITool.f_GetIconSpriteByCardId(dt.iNewCardId);
        //icon.MakePixelPerfect();
        string name = dt.szName;
        item.transform.Find("border").GetComponent<UISprite>().spriteName = UITool.f_GetImporentColorName(dt.iImportant, ref name);
        item.transform.Find("name").GetComponent<UILabel>().text = name;
        int num = Data_Pool.m_CardFragmentPool.f_GetHaveNumByTemplate(dt.iId);
        item.transform.Find("num").GetComponent<UILabel>().text = num.ToString();
        //item.transform.localScale = new Vector3(0.8f,0.8f, 0.8f);
        List<DungeonTollgatePoolDT> poolDt = f_GetSweepPoolDts(dt);
        UITool.f_Set2DSpriteGray(icon, poolDt.Count == 0);
        f_RegClickEvent(item, f_OnClickChipItem, dt, poolDt);
        //默认点击第一个
        //if (isFirstClickElite)
        //{
        //    f_OnClickChipItem(item, dt, poolDt);
        //    isFirstClickElite = false;
        //}

        item.name = dt.iNewCardId.ToString();
    }

    List<DungeonTollgatePoolDT> sweepDungeonTollgatePoolDts = new List<DungeonTollgatePoolDT>();
    CardFragmentDT mSelectDt = new CardFragmentDT();
    /// <summary>
    /// 点击碎片item
    /// </summary>
    /// <param name="go"></param>
    /// <param name="obj1"></param>
    /// <param name="obj2"></param>
    private void f_OnClickChipItem(GameObject go, object obj1, object obj2)
    {
        f_GetObject("ObjDrag2").SetActive(true);
        CardFragmentDT dt = obj1 as CardFragmentDT;
        List<DungeonTollgatePoolDT> poolDt = (List<DungeonTollgatePoolDT>)obj2;
        mSelectDt = dt;
        sweepDungeonTollgatePoolDts = poolDt;
        Debug.Log(go.name);
        if (lastItem)
        {
            lastItem.transform.Find("select").gameObject.SetActive(false);
        }
        go.transform.Find("select").gameObject.SetActive(true);

        lastItem = go;
        GridUtil.f_SetGridView(true, mEliteDungeonItemGrid.gameObject, mStarItem, poolDt, f_SetItem);
        mNoMaxStarLevelTip.SetActive(poolDt.Count == 0);
    }

    private void ObjDrag2Close(GameObject go, object obj1, object obj2)
    {
        f_GetObject("ObjDrag2").SetActive(false);
    }
    /// <summary>
    /// 根据阵营显示碎片
    /// </summary>
    /// <param name="camp"></param>
    private void f_ShowEliteChipWei(int camp)
    {
        isFirstClickElite = true;
        List<CardFragmentDT> dt = f_GetScData(camp);
        GridUtil.f_SetGridView(true, mEliteChipItemGridWei.gameObject, mEliteChipItemWei, dt, f_UpdateChipItem);
        mEliteChipItemGridWei.transform.parent.GetComponent<UIScrollView>().ResetPosition();
    }

    private void f_ShowEliteChipShu(int camp)
    {
        isFirstClickElite = true;
        List<CardFragmentDT> dt = f_GetScData(camp);
        GridUtil.f_SetGridView(true, mEliteChipItemGridShu.gameObject, mEliteChipItemShu, dt, f_UpdateChipItem);
        mEliteChipItemGridShu.transform.parent.GetComponent<UIScrollView>().ResetPosition();
    }
    private void f_ShowEliteChipWu(int camp)
    {
        isFirstClickElite = true;
        List<CardFragmentDT> dt = f_GetScData(camp);
        GridUtil.f_SetGridView(true, mEliteChipItemGridWu.gameObject, mEliteChipItemWu, dt, f_UpdateChipItem);
        mEliteChipItemGridWu.transform.parent.GetComponent<UIScrollView>().ResetPosition();
    }
    private void f_ShowEliteChipQun(int camp)
    {
        isFirstClickElite = true;
        List<CardFragmentDT> dt = f_GetScData(camp);
        GridUtil.f_SetGridView(true, mEliteChipItemGridQun.gameObject, mEliteChipItemQun, dt, f_UpdateChipItem);
        mEliteChipItemGridQun.transform.parent.GetComponent<UIScrollView>().ResetPosition();
    }

    /// <summary>
    /// 请求更新关卡数据
    /// </summary>
    private void f_GetAllData()
    {
        List<BasePoolDT<long>> tList = Data_Pool.m_DungeonPool.f_GetAllForData1((int)EM_Fight_Enum.eFight_DungeonMain);
        //mMaxInitChapterNum = tList.Count;
        for (int i = 0; i < tList.Count; i++)
        {
            DungeonPoolDT dt = tList[i] as DungeonPoolDT;
            if (dt.mTollgatePassNum == 0)
            {
                break;
            }
            
            mMaxInitChapterNum++;
            if (dt.mInitByServer)
            {
                Data_Pool.m_DungeonPool.f_ExecuteAfterInitDungeon(dt, f_CallBackByInit);
            }
            else
            {
                Data_Pool.m_DungeonPool.f_ExecuteAfterInitDungeon(dt, null);
            }

        }
    }
    /// <summary>
    /// 所有副本更新完成回调
    /// </summary>
    /// <param name="value"></param>
    private void f_CallBackByInit(object value)
    {
        mCurInitChapterNum++;
        if (mCurInitChapterNum >= mMaxInitChapterNum)
        {
            if (!mInitComplete)
            {
                f_InitStarView();
            }
            mInitComplete = true;
        }
        
    }
    /// <summary>
    /// 设置关卡信息item
    /// </summary>
    /// <param name="item"></param>
    /// <param name="dt"></param>
    private void f_SetItem(GameObject item, DungeonTollgatePoolDT dt)
    {
item.transform.Find("chapter").GetComponent<UILabel>().text = string.Format("Chương {0}", dt.mChapterId);
        for (int j = 1; j < 4; j++)
        {
            UITool.f_SetSpriteGray(item.transform.Find("star" + j).GetComponent<UISprite>(), dt.mStarNum < j);
            //item.transform.Find("star" + j).GetComponent<UISprite>().spriteName = dt.mStarNum >= j ? "Icon_DungeonStar2" : "Icon_DungeonStarGrey";
        }

        string id = dt.mTollgateTemplate.iModeId.ToString().Remove(dt.mTollgateTemplate.iModeId.ToString().Length-1, 1);
        CardDT card = glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(Int32.Parse(id)) as CardDT;

        item.transform.Find("name").GetComponent<UILabel>().text = UITool.f_GetImporentForName(card.iImportant, dt.mTollgateTemplate.szTollgateName);
        item.transform.Find("border").GetComponent<UISprite>().spriteName = UITool.f_GetImporentCase(card.iImportant);
        item.transform.Find("icon").GetComponent<UI2DSprite>().sprite2D =
            UITool.f_GetIconSprite(dt.mTollgateTemplate.iModeId);
        //item.transform.Find("icon").GetComponent<UI2DSprite>().MakePixelPerfect();
        item.SetActive(true);

        f_RegClickEvent(item, f_ClickItem, dt);
    }
    /// <summary>
    /// 点击关卡item
    /// </summary>
    /// <param name="go"></param>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    private void f_ClickItem(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.DungeonChallengePage, UIMessageDef.UI_OPEN, (DungeonTollgatePoolDT)value1);
    }

    /// <summary>
    /// 实例化未满星关view
    /// </summary>
    private void f_InitStarView()
    {
        List<DungeonTollgatePoolDT> poolDt = Data_Pool.m_DungeonPool.f_GetAllNoMaxStar((int)EM_Fight_Enum.eFight_DungeonMain);
        mLabNostarLevel.gameObject.SetActive(poolDt.Count == 0);
        GridUtil.f_SetGridView(true, mStarViewGrid, mStarItem, poolDt, f_SetItem);
    }
    /// <summary>
    /// 设置toggle按钮状态
    /// </summary>
    /// <param name="tapItem"></param>
    /// <param name="isPress"></param>
    private void SetTapState(GameObject tapItem, bool isPress)
    {
        tapItem.transform.Find("Normal").gameObject.SetActive(!isPress);
        tapItem.transform.Find("Press").gameObject.SetActive(isPress);
    }

    /// <summary>
    /// 点击黑色背景
    /// </summary>
    private void OnBlackBGClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.MainLineTreasureBookPage, UIMessageDef.UI_CLOSE);
        //ccUIManage.GetInstance().f_SendMsg(UINameConst.MainMenu, UIMessageDef.UI_OPEN);
    }
}
