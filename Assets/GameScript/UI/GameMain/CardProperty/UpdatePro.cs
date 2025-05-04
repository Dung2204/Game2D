using UnityEngine;
using System.Collections;

public class UpdatePro : MonoBehaviour
{
    int CardLv;
    UILabel HpLabel;
    UILabel AtkLabel;
    UILabel PhyDefLabel;
    UILabel MagDefLabel;
    CardPoolDT card;
    
    void OnEnable()
    {
        HpLabel = transform.Find("ShowHp/Bg/Pro_Hp").GetComponent<UILabel>();
        //Hp = HpLabel.text;
        AtkLabel= transform.Find("ShowAtk/Bg/Pro_Atk").GetComponent<UILabel>();
        //Atk = AtkLabel.text;
        PhyDefLabel = transform.Find("ShowPhyDef/Bg/Pro_PhyDef").GetComponent<UILabel>();
        //PhyDef = PhyDefLabel.text;
        MagDefLabel = transform.Find("ShowMagDef/Bg/Pro_MagDef").GetComponent<UILabel>();
        //MagDef = MagDefLabel.text;
        card = S_CardManage.m_Card;
        CardLv = 0;
    }

    //protected override void UpdateGUI()
    //{
    //    base.UpdateGUI();
    //    UpdateShow();
    //}
    /// <summary>
    /// 实时更新
    /// </summary>
    void FixedUpdate()
    {
        //if (Data_Pool.m_CardPool.f_GetCard(S_CardManage.CardId) != card)
        //{
        //    card = Data_Pool.m_CardPool.f_GetCard(S_CardManage.CardId);
        //}
        //if (Hp != card .m_CardDT.iInitHP.ToString() || Atk != card.m_CardDT.iInitAtk.ToString()
        //    || PhyDef != card.m_CardDT.iInitPhyDef.ToString() || MagDef != card.m_CardDT.iInitMagDef.ToString())
        if(CardLv!=S_CardManage.m_Card.m_iLv|| card!= S_CardManage.m_Card)
        {
            CardLv = S_CardManage.m_Card.m_iLv;
            card = S_CardManage.m_Card;
            updatePro();
            //HpLabel.text = (card.m_CardDT.iInitHP*CardLv).ToString();
            //Hp = HpLabel.text;
            //AtkLabel.text = (card.m_CardDT.iInitAtk*CardLv).ToString();
            //Atk = AtkLabel.text;
            //PhyDefLabel.text = (card.m_CardDT.iInitPhyDef*CardLv).ToString();
            //PhyDef = PhyDefLabel.text;
            //MagDefLabel.text = (card.m_CardDT.iInitMagDef*CardLv).ToString();
            //MagDef = MagDefLabel.text;
        }
    }
    /// <summary>
    /// 点击更新
    /// </summary>
    public void UpdateShow()
    {
        HpLabel.text = card.m_CardDT.iInitHP.ToString();
        AtkLabel.text = card.m_CardDT.iInitAtk.ToString();
        PhyDefLabel.text = card.m_CardDT.iInitPhyDef.ToString();
        MagDefLabel.text = card.m_CardDT.iInitMagDef.ToString();
    }

    private void updatePro()
    {
        RoleProperty Pro = Data_Pool.m_TeamPool.f_GetCardAllProperty(S_CardManage.m_Card.iId);
        HpLabel.text = (Pro.f_GetHp()).ToString();
        //Hp = HpLabel.text;
        AtkLabel.text = (Pro.f_GetProperty((int)EM_RoleProperty.Atk)).ToString();
        //Atk = AtkLabel.text;
        PhyDefLabel.text = (Pro.f_GetProperty((int)EM_RoleProperty.Def)).ToString();
        //PhyDef = PhyDefLabel.text;
        MagDefLabel.text = (Pro.f_GetProperty((int)EM_RoleProperty.MDef)).ToString();
    }
}
