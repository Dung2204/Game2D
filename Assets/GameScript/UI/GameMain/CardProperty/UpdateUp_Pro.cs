using UnityEngine;
using System.Collections;

public class UpdateUp_Pro : MonoBehaviour
{
    private string Hp;
    private string Atk;
    private string PhyDef;
    private string MagDef;
    private UILabel HpLabel;
    private UILabel AtkLabel;
    private UILabel PhyDefLabel;
    private UILabel MagDefLabel;
    private UISlider UiSlider;
    private CardPoolDT card;
    private UILabel LvLabel;
    CarLvUpDT carlvup;
    int Exp;
    private void OnEnable()
    {
        HpLabel = transform.Find("Up_Pro/Up_Hp").GetComponent<UILabel>();
        AtkLabel = transform.Find("Up_Pro/Up_Atk").GetComponent<UILabel>();
        PhyDefLabel = transform.Find("Up_Pro/Up_PhyDef").GetComponent<UILabel>();
        MagDefLabel = transform.Find("Up_Pro/Up_MagDef").GetComponent<UILabel>();
        UiSlider = transform.Find("ShowLv/ExpStrand").GetComponent<UISlider>();
        LvLabel = transform.Find("ShowLv").GetComponent<UILabel>();
        card = S_CardManage.m_Card;
        Hp = HpLabel.text = "+"+card.m_CardDT.iAddHP ;
        Atk = AtkLabel.text = "+" + card.m_CardDT.iAddAtk ;
        PhyDef = PhyDefLabel.text = "+" + card.m_CardDT.iAddDef ;
        MagDef = MagDefLabel.text = "+" + card.m_CardDT.iAddMagDef ;

        Exp = 0;
        LvLabel.text = "LV." + card.m_iLv;
        carlvup = (CarLvUpDT)glo_Main.GetInstance().m_SC_Pool.m_CarLvUpSC.f_GetSC(card.m_iLv+1);

        UiSlider.value = (float)card.m_iExp /(float)NeedExp();
    }

    private void Start()
    {
        AddTween();
    }


    private void FixedUpdate()
    {
        if (Exp != S_CardManage.m_Card.m_iExp|| card != S_CardManage.m_Card)
        {
            HpLabel.text = "+" + card.m_CardDT.iAddHP;
            Hp = card.m_CardDT.iAddHP.ToString();
            AtkLabel.text = "+" + card.m_CardDT.iAddAtk;
            Atk = card.m_CardDT.iAddAtk.ToString();
            PhyDefLabel.text = "+" + card.m_CardDT.iAddDef;
            PhyDef = card.m_CardDT.iAddDef.ToString();
            MagDefLabel.text = "+" + card.m_CardDT.iAddMagDef;
            MagDef = card.m_CardDT.iAddMagDef.ToString();
            LvLabel.text = "LV." + card.m_iLv;
            card = S_CardManage.m_Card;
        }
        //if (UiSlider.value != (float)card.m_iExp / (float)NeedExp())
        //{
        //    UiSlider.value = (float)card.m_iExp / (float)NeedExp();
        //}
    }

    /// <summary>
    /// 点击更新
    /// </summary>
    void UpdateShow()
    {
        HpLabel.text = "+" + card.m_CardDT.iAddHP;
        AtkLabel.text = "+" + card.m_CardDT.iAddAtk;
        PhyDefLabel.text = "+" + card.m_CardDT.iAddDef;
        MagDefLabel.text = "+" + card.m_CardDT.iAddMagDef;
    }
    /// <summary>
    /// 添加动画
    /// </summary>
    private void AddTween()
    {
        TweenAlpha t =
         transform.Find("Up_Pro/Up_Hp").gameObject.AddComponent<TweenAlpha>();

        t.from = 0.4f;
        t.style = UITweener.Style.PingPong;

        t = transform.Find("Up_Pro/Up_Atk").gameObject.AddComponent<TweenAlpha>();


        t.from = 0.4f;
        t.style = UITweener.Style.PingPong;

        t = transform.Find("Up_Pro/Up_PhyDef").gameObject.AddComponent<TweenAlpha>();

        t.from = 0.4f;
        t.style = UITweener.Style.PingPong;

        t = transform.Find("Up_Pro/Up_MagDef").gameObject.AddComponent<TweenAlpha>();

        t.from = 0.4f;
        t.style = UITweener.Style.PingPong;
    }
    /// <summary>
    /// 计算所需经验
    /// </summary>
    private int NeedExp()
    {
        switch (card.m_CardDT.iImportant)
        {
            case 1:
                return carlvup.iWhiteCard;
            case 2:
                return carlvup.iGreenCard;
            case 3:
                return carlvup.iBlueCard;
            case 4:
                return carlvup.iPurpleCard;
            case 5:
                return carlvup.iOragenCard;
            case 6:
                return carlvup.iRedCard;
            //TsuCode -tuong kim
            case 7:
                return carlvup.iGoldCard;
                //
        }
        return 0;
    }


}