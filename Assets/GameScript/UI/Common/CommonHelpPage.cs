using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ccU3DEngine;

public class CommonHelpPage : UIFramwork
{
    private enum HelpType
    {
        Goods = 1,
        Label = 2,
    }

    private enum HelpSpecial
    {
        None,
        /// <summary>
        /// 卡牌碎片
        /// </summary>
        CardFram,
    }
    private class HelpData
    {
        public string mTitle { private set; get; }
        public string mLabel { private set; get; }
        public List<ResourceCommonDT> mGoods { private set; get; }
        public HelpType mType { private set; get; }

        public void f_UpdateLabel(string Title, string label, int Special)
        {
            mTitle = Title;
            if (label != null && label != string.Empty)
                mLabel = label;
            else
MessageBox.DEBUG("Support Error");
            mType = HelpType.Label;
            mSpecial = Special;
        }

        public void f_UpdateGoods(string Title, List<ResourceCommonDT> Goods, int Special)
        {
            mTitle = Title;
            if (Goods != null)
                mGoods = Goods;
            else
MessageBox.DEBUG("Item support error");
            mType = HelpType.Goods;
            mSpecial = Special;
        }

        public int mHeight
        {
            get
            {
                switch (mType)
                {
                    case HelpType.Goods:
                        return 304;
                    case HelpType.Label:
                        return mLabel.Length / 34 * 28 + 60;
                    default:
                        break;
                }

                return 0;
            }
        }

        public int mSpecial { private set; get; }
    }



    private HelpDataDT mData;
    private List<HelpData> HelpDataList = new List<HelpData>();
    private List<Transform> mItemList = new List<Transform>();
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        if (e is int)
        {
            mData = glo_Main.GetInstance().m_SC_Pool.m_HelpDataSC.f_GetSC((int)e) as HelpDataDT;
        }
        UpdateData();
        CreateItem();
        f_GetObject("Scroll View").GetComponent<UIScrollView>().ResetPosition();
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }

    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("Bg", OnCloseUI);
    }

    private void OnCloseUI(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CommonHelpPage, UIMessageDef.UI_CLOSE);
    }
    /// <summary>
    /// 刷新数据
    /// </summary>
    private void UpdateData()
    {
        HelpDataList.Clear();
        string[] tHelpData = mData.szData.Split('@');
        string[] Special = mData.szSpecial.Split(';');

        for (int i = 0; i < tHelpData.Length; i++)
        {
            string[] Help = tHelpData[i].Split('#');
            HelpData tData = new HelpData();
            int type = 0;
            int iSpecial = 0;
            if (Special[0] == string.Empty)
                iSpecial = 0;
            else
            {
                iSpecial = int.Parse(Special[i]);
            }
            if (int.TryParse(Help[1], out type))
            {
                switch ((HelpType)type)
                {
                    case HelpType.Goods:
                        List<ResourceCommonDT> Goods = new List<ResourceCommonDT>();
                        string[] GoodsArr = Help[2].Split('B');
                        for (int j = 0; j < GoodsArr.Length; j++)
                        {
                            if (GoodsArr[j] == string.Empty) continue;
                            string[] GoodsItem = GoodsArr[j].Split(';');
                            ResourceCommonDT t = new ResourceCommonDT();
                            t.f_UpdateInfo(byte.Parse(GoodsItem[0]), int.Parse(GoodsItem[1]), int.Parse(GoodsItem[2]));
                            Goods.Add(t);
                        }
                        tData.f_UpdateGoods(Help[0], Goods, iSpecial);
                        break;
                    case HelpType.Label:
                        tData.f_UpdateLabel(Help[0], Help[2], iSpecial);
                        break;
                    default:
                        break;
                }
            }
            HelpDataList.Add(tData);
        }


    }

    private void Special(HelpData Data)
    {
        switch ((HelpSpecial)Data.mSpecial)
        {
            case HelpSpecial.CardFram:
                DisposeCardFram(Data);
                break;
            default:
                break;
        }
    }

    private void CreateItem()
    {
        float Pos = 0;
        for (int i = 0; i < mItemList.Count; i++)
            mItemList[i].gameObject.SetActive(false);
        for (int i = 0; i < HelpDataList.Count; i++)
        {
            if (i >= mItemList.Count)
            {
                GameObject go = NGUITools.AddChild(f_GetObject("ItemParent"), f_GetObject("Item"));
                mItemList.Add(go.transform);
            }
            mItemList[i].gameObject.SetActive(true);
            Special(HelpDataList[i]);
            UpdateItem(mItemList[i].transform, HelpDataList[i], ref Pos);
        }


    }
    /// <summary>
    /// 刷新单个Item
    /// </summary>
    /// <param name="tran"></param>
    /// <param name="Data"></param>
    /// <param name="y"></param>
    private void UpdateItem(Transform tran, HelpData Data, ref float y)
    {
        UILabel Title = tran.Find("Title").GetComponent<UILabel>();
        GameObject GoodsBg = tran.Find("GoodsBg").gameObject;
        GameObject Goods = tran.Find("Goods").gameObject;
        UILabel Label = tran.Find("Label").GetComponent<UILabel>();
        GameObject GoodsParent = tran.Find("Goods/GoodsParent").gameObject;
        GameObject ResourceItem = tran.Find("Goods/ResourceCommonItem").gameObject;


        Title.text = Data.mTitle;

        Label.gameObject.SetActive(Data.mType == HelpType.Label);
        GoodsBg.SetActive(Data.mType == HelpType.Goods && Data.mGoods.Count > 6);
        Goods.SetActive(Data.mType == HelpType.Goods);
        switch (Data.mType)
        {
            case HelpType.Goods:
                GridUtil.f_SetGridView<ResourceCommonDT>(GoodsParent, ResourceItem, Data.mGoods,
                    (GameObject go, ResourceCommonDT goods) =>
                    {
                        go.GetComponent<ResourceCommonItem>().f_UpdateByInfo(goods);
                    });
                //if (Data.mGoods.Count > 6)
                    GoodsParent.transform.parent.GetComponent<UIScrollView>().ResetPosition();
                break;
            case HelpType.Label:
                Label.text = Data.mLabel;// UITool.f_ReplaceName( Data.mLabel, ".", ".\n");
                break;
            default:
                break;
        }
        tran.localPosition = new Vector3(0, -y, 0);
        y += Data.mHeight;
    }
    /// <summary>
    /// 处理卡牌碎片
    /// </summary>
    private void DisposeCardFram(HelpData Data)
    {
        CardFragmentDT tCardFramDT;
        List<NBaseSCDT> ListDT = glo_Main.GetInstance().m_SC_Pool.m_CardFragmentSC.f_GetAll();
        Data.mGoods.Clear();
        for (int i = 0; i < ListDT.Count; i++)
        {
            ResourceCommonDT ResourceDt = new ResourceCommonDT();
            ResourceDt.isGray = true;
            tCardFramDT = ListDT[i] as CardFragmentDT;
            ResourceDt.f_UpdateInfo((byte)EM_ResourceType.CardFragment, tCardFramDT.iId, 0);
            string[] DungeonID = tCardFramDT.szStage.Split(';');
            for (int j = 0; j < DungeonID.Length - 1; j++)
            {
                DungeonTollgatePoolDT DungeonDT = Data_Pool.m_DungeonPool.f_GetDungeonTollgateForID(int.Parse(DungeonID[j]));
                if (DungeonDT.mStarNum > 0)
                {
                    ResourceDt.isGray = false;
                    break;
                }
            }
            Data.mGoods.Add(ResourceDt);
        }
        Data.mGoods.Sort((ResourceCommonDT a, ResourceCommonDT b) =>
        {
            if (a.isGray && !b.isGray) return -1;
            else if (!a.isGray && b.isGray) return 1;
            else
            {
                if (a.mResourceId < b.mResourceId) return -1;
                else if (a.mResourceId > b.mResourceId) return 1;
                else return 0;
            }
        });

    }

}
