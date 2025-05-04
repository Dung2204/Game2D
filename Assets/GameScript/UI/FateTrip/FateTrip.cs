using UnityEngine;
using System.Collections;
using ccU3DEngine;

public class FateTrip : UIFramwork
{
    FateTripParam _TeamCard;
    GameObject FateTripParent;
    GameObject FateTripItem;
    float _mHgith;
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        _TeamCard = (FateTripParam)e;
        FateTripParent = f_GetObject("FateTripParent");
        FateTripItem = f_GetObject("FateTripItem");
        UpdateFateTrip();
        Invoke("tSetActive", 2f);
    }

    void UpdateFateTrip()
    {
        _mHgith = 0;
        for (int i = FateTripParent.transform.childCount -1; i >= 0 ; i--)
            Destroy(FateTripParent.transform.GetChild(i).gameObject);
        Data_Pool.m_TeamPool.f_UpdateCardFate(_TeamCard.m_TeamPoolDT);
        //缘分
        for (int i = 0; i < _TeamCard.m_TeamPoolDT.m_CardPoolDT.m_CardFatePoolDT.m_aFateList.Count; i++)
        {
            if (_TeamCard.m_TeamPoolDT.m_CardPoolDT.m_CardFatePoolDT.m_aFateIsOk[i])
            {
                GameObject tFateTrip = NGUITools.AddChild(FateTripParent, FateTripItem);
                tFateTrip.SetActive(true);
                UILabel tmpUILabel = tFateTrip.GetComponent<UILabel>();
                tFateTrip.transform.localPosition += new Vector3(0, _mHgith, 0);
                _mHgith -= (tmpUILabel.height + 30);
                tFateTrip.GetComponent<UILabel>().text = string.Format(tFateTrip.GetComponent<UILabel>().text,
               _TeamCard.m_TeamPoolDT.m_CardPoolDT.m_CardDT.iCardType == (int)EM_CardType.RoleCard ? Data_Pool.m_UserData.m_szRoleName :
               _TeamCard.m_TeamPoolDT.m_CardPoolDT.m_CardDT.szName, _TeamCard.m_TeamPoolDT.m_CardPoolDT.m_CardFatePoolDT.m_aFateList[i].szName);
            }
        }
        //大师
        for (int i = 0; i < _TeamCard.m_MasterLevel.Length; i++)
        {
            if ( _TeamCard.m_MasterLevel[i] <= 1)
            {
                continue;
            }
            GameObject tFateTrip = NGUITools.AddChild(FateTripParent, FateTripItem);
            tFateTrip.SetActive(true);
            string tMaster = "[fff700]";
            switch ((EM_Master)i + 1)
            {
                case EM_Master.EquipIntensify:
tMaster += "Strengthening Master[-]";
                    break;
                case EM_Master.EquipRefine:
tMaster += "Master Refining[-]";
                    break;
                case EM_Master.TreasureIntensify:
tMaster += "Great Master of France[-]";
                    break;
                case EM_Master.TreasureRefine:
tMaster += "Faculty of Master[-]";
                    break;
            }
tMaster += string.Format("[f6f3f0] reach level [-][3cff00]{0}[-]", _TeamCard.m_MasterLevel[i]);
            tFateTrip.transform.localPosition += new Vector3(0, _mHgith, 0);
            tFateTrip.GetComponent<UILabel>().text = tMaster;
            _mHgith -= (tFateTrip.GetComponent<UILabel>().height + 20);
        }


        //属性
        for (int i = 0; i < _TeamCard.m_Pro.Length; i++)
        {
            if (_TeamCard.m_Pro[i] == 0)
            {
                continue;
            }
            GameObject tFateTrip = NGUITools.AddChild(FateTripParent, FateTripItem);
            tFateTrip.SetActive(true);

            EM_Important tImportant = EM_Important.Red;
            string Pro = string.Empty;
            if (i + 1 == (int)EM_RoleProperty.Hp)
            {
                if (_TeamCard.m_Hp > 0)
                    tImportant = EM_Important.Green;
                Pro = UITool.f_GetImporentForName((int)tImportant, UITool.f_GetProName((EM_RoleProperty)i + 1) + "  {0}" + _TeamCard.m_Hp.ToString());
                Pro = Pro.Replace("{0}", _TeamCard.m_Hp > 0 ? "+" : "");
            }
            else
            {
                if (_TeamCard.m_Pro[i] > 0)
                    tImportant = EM_Important.Green;
                Pro = UITool.f_GetImporentForName((int)tImportant, UITool.f_GetProName((EM_RoleProperty)i + 1) + "  {0}" + _TeamCard.m_Pro[i].ToString());
                Pro = Pro.Replace("{0}", _TeamCard.m_Pro[i] > 0 ? "+" : "");
            }


            tFateTrip.transform.localPosition += new Vector3(0, _mHgith, 0);
            _mHgith -= (tFateTrip.GetComponent<UILabel>().height + 20);
            tFateTrip.GetComponent<UILabel>().text = Pro;
        }
    }

    void tSetActive()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
}


public class FateTripParam
{
    public TeamPoolDT m_TeamPoolDT;
    public int[] m_Pro;
    public long m_Hp;
    public int[] m_MasterLevel;
    public FateTripParam(TeamPoolDT tTeamPoolDT, int[] tPro, long hp, int[] m_MasterLevel)
    {
        m_TeamPoolDT = tTeamPoolDT;
        m_Pro = tPro;
        m_Hp = hp;
        this.m_MasterLevel = m_MasterLevel;
    }
}
