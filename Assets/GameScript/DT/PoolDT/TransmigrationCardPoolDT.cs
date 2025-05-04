using ccU3DEngine;
using System.Collections.Generic;

public class TransmigrationCardPoolDT : BasePoolDT<long>
{
    //卡牌转换表格数据
    public TransmigrationCardDT m_TransmigrationCardData;

    public Dictionary<EM_CardCamp, List<BasePoolDT<long>>> m_SelectCardDataDict = new Dictionary<EM_CardCamp, List<BasePoolDT<long>>>();

    public List<BasePoolDT<long>> f_GetTransmigrationCampArr(EM_CardCamp campType)
    {
        if (!m_SelectCardDataDict.ContainsKey(campType))
        {
            List<BasePoolDT<long>> tInitList = new List<BasePoolDT<long>>();
            int[] cardListData = null;
            string[] _cardModelIdString = ccMath.f_String2ArrayString(m_TransmigrationCardData.szCardTempId, "#");
            if (campType == EM_CardCamp.eCardWei)
            {
                cardListData = ccMath.f_String2ArrayInt(_cardModelIdString[0], ";");
            }
            else if (campType == EM_CardCamp.eCardShu)
            {
                cardListData = ccMath.f_String2ArrayInt(_cardModelIdString[1], ";");
            }
            else if (campType == EM_CardCamp.eCardWu)
            {
                cardListData = ccMath.f_String2ArrayInt(_cardModelIdString[2], ";");
            }
            else
            {
                cardListData = ccMath.f_String2ArrayInt(_cardModelIdString[3], ";");
            }
            for (int i = 0; i < cardListData.Length; i++)
            {
                SelectCardData _data = new SelectCardData();
                _data.CardId = cardListData[i];
                _data.cardDt = (CardDT)glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(_data.CardId);
                tInitList.Add(_data);
            }
            m_SelectCardDataDict.Add(campType, tInitList);
        }
        return m_SelectCardDataDict[campType];
    }


}

public class SelectCardData : BasePoolDT<long>
{
    public int CardId;
    public CardDT cardDt;
}
