using UnityEngine;
using System.Collections;
/// <summary>
/// 夺宝界面选择机器人item
/// </summary>
public class RobotItem : MonoBehaviour
{
    public UILabel LabelRobotName;//机器人名称
    public UILabel LabelRobotLevel;//机器人等级
    public UILabel LabelProbability;//概率
    public GameObject BtnGrab;//按钮抢夺
    public GameObject BtnFiveGrab;//抢夺5次
    public GameObject ObjCard1;//主卡
    public GameObject ObjCard2;
    public GameObject ObjCard3;
    public GameObject ObjCard4;
    public GameObject ObjCard5;
    public GameObject ObjCard6;
    public UIGrid RoleRoot;
    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="robotName">机器人名称</param>
    /// <param name="robotLevel">机器人等级</param>
    /// <param name="probability">概率性</param>
    /// <param name="card1ID">主卡</param>
    /// <param name="card2ID">辅卡1</param>
    /// <param name="card3ID">辅卡2</param>
    /// <param name="card4ID">辅卡3</param>
    /// <param name="card5ID">辅卡4</param>
    /// <param name="card6ID">辅卡5</param>
    public void SetData(string robotName, int robotLevel, string probability, int card1ID, int card2ID, int card3ID, int card4ID, int card5ID, int card6ID)
    {
        LabelRobotName.text = robotName;
LabelRobotLevel.text = "Level" + robotLevel;
        LabelProbability.text = probability;
        SetCardData(ObjCard1, card1ID);
        SetCardData(ObjCard2, card2ID);
        SetCardData(ObjCard3, card3ID);
        SetCardData(ObjCard4, card4ID);
        SetCardData(ObjCard5, card5ID);
        SetCardData(ObjCard6, card6ID);
        RoleRoot.GetComponent<UIGrid>().enabled = true;
        RoleRoot.GetComponent<UIGrid>().Reposition();
        transform.parent.GetComponent<UIGrid>().Reposition();
    }
    /// <summary>
    /// 设置卡牌数据（icon和边框）
    /// </summary>
    /// <param name="cardObj">卡牌物体</param>
    /// <param name="cardId">卡牌模板id</param>
    private void SetCardData(GameObject cardObj, int cardId)
    {
        CardDT cardDT = glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(cardId) as CardDT;
        if (cardDT != null)
        {
            UITool.f_SetIconSprite(cardObj.transform.Find("SprHeadIcon").GetComponent<UI2DSprite>(), EM_ResourceType.Card, cardId);
            string name = cardDT.szName;
            cardObj.transform.Find("SprBorder").GetComponent<UISprite>().spriteName = UITool.f_GetImporentColorName(cardDT.iImportant, ref name);
        }
        cardObj.SetActive(cardDT != null);
        //    transform.Find("SprHeadIcon").gameObject.SetActive(cardDT != null);
        //cardObj.transform.Find("SprBorder").gameObject.SetActive(cardDT != null);
    }
}
