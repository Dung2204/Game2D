using System.Collections;
using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// 阵容点击缘分详细信息界面
/// </summary>
public class FateIntroDetailPage : UIFramwork
{
    /// <summary>
    /// 初始化消息
    /// </summary>
    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BlackBG", OnBlackBGClick);
    }
    /// <summary>
    /// 打开界面
    /// </summary>
    /// <param name="e"></param>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        ShowFateView((EM_FormationPos)e);
       
    }
    /// <summary>
    /// 点击黑色背景
    /// </summary>
    private void OnBlackBGClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.FateIntroDetailPage, UIMessageDef.UI_CLOSE);
    }
    /// <summary>
    /// 显示缘分详细信息
    /// </summary>
    /// <param name="emFormationPos"></param>
    private void ShowFateView(EM_FormationPos emFormationPos)
    {
        TeamPoolDT dt = Data_Pool.m_TeamPool.f_GetForId((long)emFormationPos) as TeamPoolDT;
        int titleHeight = 50;
        int _FountSpacing = 26;
        int totalHeight = 0;
        int fateItemHeight = 0;
        GameObject FateGrid = f_GetObject("FateGrid");

        //删除子物体
        for (int i = FateGrid.transform.childCount - 1; i >= 0; i--)
        {
            GameObject.Destroy(FateGrid.transform.GetChild(i).gameObject);
        }
        Data_Pool.m_TeamPool.f_UpdateCardFate(dt);
        CardFatePoolDT fateDt = dt.m_CardPoolDT.m_CardFatePoolDT;
        //GameObject item = GameObject.Instantiate(f_GetObject("Intro_Fate")) as GameObject;
        //item.transform.SetParent(FateGrid.transform);
        //item.SetActive(true);
        //item.transform.localPosition = new Vector3(0, totalHeight * -1f, 0);
        //item.transform.localScale = Vector3.one;
        //item.transform.localEulerAngles = Vector3.zero;
        //if (dt.m_eFormationPos == EM_FormationPos.eFormationPos_Main)
        //    item.GetComponent<UILabel>().text = Data_Pool.m_UserData.m_szRoleName;
        //else
        //    item.GetComponent<UILabel>().text = dt.m_CardPoolDT.m_CardDT.szName;
        //totalHeight += titleHeight;
        for (int j = 0; j < fateDt.m_aFateList.Count; j++)
        {

            GameObject FateLabelItem = NGUITools.AddChild(FateGrid, f_GetObject("Introduce_Fate"));
            //FateLabelItem.transform.SetParent(FateGrid.transform);
            FateLabelItem.SetActive(true);
            //FateLabelItem.transform.localPosition = new Vector3(0, fateItemHeight * -1f, 0);
            //FateLabelItem.transform.localEulerAngles = Vector3.zero;
            //FateLabelItem.transform.localScale = Vector3.one;
            //if (fateDt.m_aFateIsOk[j])
            //{


            //}
            //else
            //{
            //    FateLabelItem.GetComponent<UILabel>().text = string.Format("[9a9a9a][{0}][-]", fateDt.m_aFateList[j].szName);
            //    FateLabelItem.transform.GetChild(0).GetComponent<UILabel>().text = "[9a9a9a]" + fateDt.m_aFateList[j].szReadme.Substring(fateDt.m_aFateList[j].szName.Length + 1);
            //}
            //fateItemHeight += FateLabelItem.transform.GetChild(0).GetComponent<UILabel>().height + _FountSpacing;
            //totalHeight += FateLabelItem.transform.GetChild(0).GetComponent<UILabel>().height + _FountSpacing;
            //UISprite Bg = FateLabelItem.transform.Find("bg").GetComponent<UISprite>();
            //Bg.spriteName = fateDt.m_aFateIsOk[j] ? "Border_Common_4" : "Border_Common_3";


            Transform fateName = FateLabelItem.transform.GetChild(0);
            fateName.GetComponent<UILabel>().text = string.Format("[F8E62D]{0}[-]", fateDt.m_aFateList[j].szName);
            Transform fateInfo = FateLabelItem.transform.GetChild(2);
            fateInfo.GetComponent<UILabel>().text = fateDt.m_aFateList[j].szReadme.Substring(fateDt.m_aFateList[j].szName.Length + 1);



            int[] aGoodsId = ccMath.f_String2ArrayInt(dt.m_CardPoolDT.m_CardFatePoolDT.m_aFateList[j].szGoodsId, ";");
            for (int i = 0; i < aGoodsId.Length; i++)
            {
                Transform iconItem = FateLabelItem.transform.GetChild(3 + i);
                if (aGoodsId[i] != 0)
                {
                    iconItem.gameObject.SetActive(true);
                    string sporderName = "";
                    UI2DSprite icon = iconItem.GetChild(0).GetComponent<UI2DSprite>();
                    UISprite CaseSprite = iconItem.GetChild(1).GetComponent<UISprite>();
                    bool isGray = false;
                    //UITool.f_SetIconSprite(iconItem.GetChild(0).GetComponent<UI2DSprite>(), (EM_ResourceType)dt.m_CardPoolDT.m_CardFatePoolDT.m_aFateList[j].iGoodsType, aGoodsId[i]);
                    //
                    int goodsType = fateDt.m_aFateList[j].iGoodsType;
                    if ((EM_ResourceType)goodsType == EM_ResourceType.Card)
                    {
                        CardDT cardDt = glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(aGoodsId[i]) as CardDT;
                        isGray = Data_Pool.m_TeamPool.f_CheckHaveCardForTemp(aGoodsId[i]);
                        sporderName = UITool.f_GetCardImporent(aGoodsId[i]);
                        CaseSprite.spriteName = sporderName;
                        icon.sprite2D = UITool.f_GetIconSpriteByCardId(aGoodsId[i]);
                    }
                    else if ((EM_ResourceType)goodsType == EM_ResourceType.Equip)
                    {
                        
                        EquipDT equipDt = glo_Main.GetInstance().m_SC_Pool.m_EquipSC.f_GetSC(aGoodsId[i]) as EquipDT;
                        isGray = Data_Pool.m_TeamPool.f_CheckInEquipByKeyId(dt.m_CardPoolDT.m_iTempleteId,aGoodsId[i]);
                        sporderName = UITool.f_GetImporentCase(equipDt.iColour);
                        Sprite sprite = UITool.f_GetIconSprite(equipDt.iIcon);
                        icon.sprite2D = sprite;
                        CaseSprite.spriteName = sporderName;

                    }
                    else if ((EM_ResourceType)goodsType == EM_ResourceType.Treasure)
                    {
                        isGray = Data_Pool.m_TeamPool.f_CheckInTreamByKeyId(dt.m_CardPoolDT.m_iTempleteId, aGoodsId[i]);
                        TreasureDT treasureDt = glo_Main.GetInstance().m_SC_Pool.m_TreasureSC.f_GetSC(aGoodsId[i]) as TreasureDT;
                        sporderName = UITool.f_GetImporentCase(treasureDt.iImportant);
                        CaseSprite.spriteName = sporderName;
                        Sprite sprite = UITool.f_GetIconSprite(treasureDt.iIcon);
                        icon.sprite2D = sprite;
                    }
                    UITool.f_SetSpriteGray(CaseSprite, !isGray);
                    UITool.f_Set2DSpriteGray(icon, !isGray);

                }
                else
                {
                    iconItem.gameObject.SetActive(false);
                }
            }
        }
        FateGrid.GetComponentInParent<UIScrollView>().ResetPosition();
        f_GetObject("GridBar").GetComponent<UIScrollBar>().value = 0f;
        FateGrid.GetComponent<UIGrid>().repositionNow = true;

        //FateGrid.GetComponent<UIGrid>().Reposition();
    }
}
