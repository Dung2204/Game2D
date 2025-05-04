using ccU3DEngine;
using UnityEngine;
using System.Collections;

/// <summary>
/// 公用的资源Icon展示
/// </summary>
public class ResourceCommonItem : MonoBehaviour
{
    public static ResourceCommonItem f_Create(GameObject parent, GameObject item)
    {
        GameObject go = NGUITools.AddChild(parent, item);
        NGUITools.MarkParentAsChanged(go);
        ResourceCommonItem result = go.GetComponent<ResourceCommonItem>();
        if (result == null)
MessageBox.ASSERT("f_Create in Item must contain ResourceCommonItem");
        else
            result.f_Init();
        return result;
    }

    public ResourceCommonDT mData
    {
        get;
        private set;
    }

    public EM_CommonItemClickType mCurClickType
    {
        get;
        private set;
    }

    public EM_CommonItemShowType mCurShowType
    {
        get;
        private set;
    }


    //初始化
    private void f_Init()
    {
        ccUIEventListener.Get(gameObject).onClickV2 = ClickBtnHandle;
    }

    public UI2DSprite mIcon;
    public UILabel mName;
    public UILabel mNum;
    public UISprite mBorder;

    /// <summary>
    /// 跳转界面需要hold的界面
    /// </summary>
    public ccUIBase mNeedHoldUI
    {
        get;
        private set;
    }

    /// <summary>
    /// 更新Item
    /// </summary>
    /// <param name="type"></param>
    /// <param name="id"></param>
    /// <param name="num"></param>
    /// <param name="showType"></param>
    /// <param name="clickType"></param>
    /// <param name="needHoldUI">点击图标需要跳转界面时且这个参数不为空，就会调用函数hold住此界面</param>
    public void f_UpdateByInfo(int type, int id, int num, EM_CommonItemShowType showType = EM_CommonItemShowType.All, EM_CommonItemClickType clickType = EM_CommonItemClickType.AllTip, ccUIBase needHoldUI = null)
    {
        if (mData == null)
            mData = new ResourceCommonDT();
        mData.f_UpdateInfo((byte)type, id, num);
        gameObject.SetActive(true);
        mCurClickType = clickType;
        mCurShowType = showType;
        mNeedHoldUI = needHoldUI;
        UITool.f_SetIconSprite(mIcon, (EM_ResourceType)type, id);
        string name = mData.mName;
        mBorder.spriteName = UITool.f_GetImporentColorName(mData.mImportant, ref name);
        if (mCurShowType == EM_CommonItemShowType.All)
        {
            mName.text = name;
            int addExp = 0;
            //经验值得增加军团技能添加的经验值显示
            if (mData.mResourceType == (byte)EM_ResourceType.Money && mData.mResourceId == (int)EM_UserAttr.eUserAttr_Exp)
            {
                addExp = GameFormula.f_Exp2AddExp(mData.mResourceNum);
            }
            string strAddExp = addExp > 0 ? "[FFF700FF]（+" + addExp + "）" : "";
            mNum.text = UITool.f_CountToChineseStr(mData.mResourceNum) + strAddExp;
        }
        else
        {
            mName.text = string.Empty;
            mNum.text = string.Empty;
        }


        if (ccUIEventListener.Get(gameObject).onClickV2 == null)
            f_Init();
    }
    /// <summary>
    /// 更新Item
    /// </summary>
    /// <param name="data"></param>
    /// <param name="showType"></param>
    /// <param name="clickType"></param>
    /// <param name="needHoldUI">点击图标需要跳转界面时且这个参数不为空，就会调用函数hold住此参数界面</param>
    public void f_UpdateByInfo(ResourceCommonDT data, EM_CommonItemShowType showType = EM_CommonItemShowType.All, EM_CommonItemClickType clickType = EM_CommonItemClickType.AllTip, ccUIBase needHoldUI = null)
    {
        gameObject.SetActive(data != null);
        mData = data;
        mCurClickType = clickType;
        mCurShowType = showType;
        mNeedHoldUI = needHoldUI;
        UITool.f_SetIconSprite(mIcon, (EM_ResourceType)data.mResourceType, data.mResourceId);
        string name = mData.mName;
        mBorder.spriteName = UITool.f_GetImporentColorName(mData.mImportant, ref name);
        if (mCurShowType == EM_CommonItemShowType.All)
        {
            mName.text = name;
            int addExp = 0;
            //经验值得增加军团技能添加的经验值显示
            if (data.mResourceType == (byte)EM_ResourceType.Money && data.mResourceId == (int)EM_UserAttr.eUserAttr_Exp)
            {
                addExp = GameFormula.f_Exp2AddExp(data.mResourceNum);
            }
            string strAddExp = addExp > 0 ? "[FFF700FF]（+" + addExp + "）" : "";
            mNum.text = UITool.f_CountToChineseStr(mData.mResourceNum) + strAddExp;
        }
        else
        {
            mName.text = string.Empty;
            mNum.text = string.Empty;
        }

        UITool.f_Set2DSpriteGray(mIcon, data.isGray);
        UITool.f_SetSpriteGray(mBorder, data.isGray);
        if (ccUIEventListener.Get(gameObject).onClickV2 == null)
            f_Init();
    }

    public void f_Disable()
    {
        gameObject.SetActive(false);
    }


    private void ClickBtnHandle(GameObject go, object boj1, object boj2)
    {
        if (mData == null)
        {
            MessageBox.ASSERT("ResourceCommonItem Data Is NULL!");
            return;
        }
        if (mCurClickType == EM_CommonItemClickType.AllTip)
        {
            ShowCommonTipUI(mData);
        }
        else if (mCurClickType == EM_CommonItemClickType.Normal || mCurClickType == EM_CommonItemClickType.Goods)
        {
MessageBox.DEBUG("Show the corresponding interface");
            if (mData.mResourceType == (int)EM_ResourceType.Money)
            {
                ShowCommonTipUI(mData);
            }
            else if (mData.mResourceType == (int)EM_ResourceType.Good)
            {
                ShowCommonTipUI(mData);
            }
            else if (mData.mResourceType == (int)EM_ResourceType.AwakenEquip)
            {
                //跳转时hold住需要的界面
                //if (mNeedHoldUI != null)
                //{
                //    ccUIHoldPool.GetInstance().f_Hold(mNeedHoldUI);
                //}
                //跳转
                //ccUIManage.GetInstance().f_SendMsg(UINameConst.CardBagPage, UIMessageDef.UI_OPEN);
MessageBox.DEBUG("Awakening props");
            }
            else if (mData.mResourceType == (int)EM_ResourceType.Card)
            {
MessageBox.DEBUG("Champion");
            }
            else if (mData.mResourceType == (int)EM_ResourceType.CardFragment)
            {
MessageBox.DEBUG("Champion Fragment");
            }
            else if (mData.mResourceType == (int)EM_ResourceType.Equip)
            {
MessageBox.DEBUG("Equip");
            }
            else if (mData.mResourceType == (int)EM_ResourceType.EquipFragment)
            {
MessageBox.DEBUG("Equipment Piece");
            }
            else if (mData.mResourceType == (int)EM_ResourceType.Treasure)
            {
MessageBox.DEBUG("Dharma Treasure");
            }
            else if (mData.mResourceType == (int)EM_ResourceType.TreasureFragment)
            {
MessageBox.DEBUG("Magic Fragment");
            }
            else
            {
MessageBox.ASSERT("ResourceComonItem Unprocessed type clicked, type:" + mData.mResourceType);
            }
        }
        else if (mCurClickType == EM_CommonItemClickType.EquipManage)
        {
            EquipBox tEquip = new EquipBox();
            tEquip.oType = EquipBox.OpenType.SelectAward;
            tEquip.tType = EquipBox.BoxTye.Intro;
            tEquip.tEquipPoolDT = new EquipPoolDT();
            tEquip.tEquipPoolDT.m_iTempleteId = mData.mResourceId;
            ccUIHoldPool.GetInstance().f_Hold(ccUIManage.GetInstance().f_GetUIHandler(UINameConst.GoodsBagPage));
            ccUIHoldPool.GetInstance().f_Hold(mNeedHoldUI);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.EquipManage, UIMessageDef.UI_OPEN, tEquip);
        }
        else if (mCurClickType == EM_CommonItemClickType.CardManage)
        {
            CardBox tCard = new CardBox();
            tCard.m_bType = CardBox.BoxType.Intro;
            tCard.m_oType = CardBox.OpenType.SelectAward;
            tCard.m_Card = new CardPoolDT();
            tCard.m_Card.m_iTempleteId = mData.mResourceId;
            ccUIHoldPool.GetInstance().f_Hold(ccUIManage.GetInstance().f_GetUIHandler(UINameConst.GoodsBagPage));
            ccUIHoldPool.GetInstance().f_Hold(mNeedHoldUI);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.CardProperty, UIMessageDef.UI_OPEN, tCard);
        }
        else if (mCurClickType == EM_CommonItemClickType.TreasureManage)
        {
            TreasureBox tTreasure = new TreasureBox();
            tTreasure.tTreasurePoolDT = new TreasurePoolDT();
            tTreasure.tTreasurePoolDT.m_iTempleteId = mData.mResourceId;
            tTreasure.IsSelectAward = 1;
            tTreasure.IsShowChange = 1;
            tTreasure.tType = TreasureBox.BoxType.GetWay;
            ccUIHoldPool.GetInstance().f_Hold(ccUIManage.GetInstance().f_GetUIHandler(UINameConst.GoodsBagPage));
            ccUIHoldPool.GetInstance().f_Hold(mNeedHoldUI);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.TreasureManage, UIMessageDef.UI_OPEN, tTreasure);
        }
    }

    private void ShowCommonTipUI(ResourceCommonDT data)
    {
MessageBox.DEBUG("Show Tip interface");
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResourceCommonItemDetailPage, UIMessageDef.UI_OPEN, data);
    }
}

