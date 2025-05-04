using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
/// <summary>
/// 商店抽牌数据
/// </summary>
public class ShopLotteryPoolDT : BasePoolDT<long>
{
    /// <summary>
    /// 累计购买次数
    /// </summary>
    public int totalTimes;
    /// <summary>
    /// 上次免费购买时间
    /// </summary>
    public int lastFreeTime;
    /// <summary>
    /// 今日半价单抽次数
    /// </summary>
    public int totalHalfTimes;
    /// <summary>
    /// số lần quay có thể biến động khi nhận quà
    /// </summary>
    public int tempTotalTimes;
    /// <summary>
    /// trạng thái nhận quà:  0 chưa nhận / 1-4 các mốc quà
    /// </summary>
    public int award;
    /// <summary>
    /// id quà đã chọn
    /// </summary>
    public int itemId;
    /// <summary>
    /// trạng thái khóa của item
    /// 0. chưa quay ra / 1. khóa đã quay ra item trên
    /// </summary>
    public int iLock;
    /// <summary>
    /// 商店抽牌表DT
    /// </summary>
    public ShopLotteryDT shopLotteryDT;
    public ArrayList m_List;
    public ArrayList GetList() {
        if(m_List!=null && shopLotteryDT != null)
        {
            return m_List;
        }
        if (m_List == null)
        {
            m_List = new ArrayList();
        }
        if (shopLotteryDT!=null)
        {
            string szItems = shopLotteryDT.szItems;
            string[] pStr = szItems.Split(new string[] { "#" }, System.StringSplitOptions.None);
            for(int i = 0; i < pStr.Length; i++)
            {
                string[] items = pStr[i].Split(new string[] { ";" }, System.StringSplitOptions.None);
                if(items.Length == 2)
                {
                    m_List.Add(new ShopLotteryItem(int.Parse(items[0]), int.Parse(items[1])));
                }

            }
        }
        return m_List;
    }
    public int GetValueById(int index)
    {
        ArrayList list = GetList();
        for (int i = 0; i < list.Count; i++)
        {
            ShopLotteryItem tData = (ShopLotteryItem)list[i];
            if (tData.key == index) return tData.value;
        }
        return 0;
    }
    public int GetId(int index)
    {
        ArrayList list = GetList();
        for (int i = 0; i < list.Count; i++)
        {
            ShopLotteryItem tData = (ShopLotteryItem)list[i];
            if (tData.key == index) return tData.key;
        }
        return 0;
    }

    public List<BasePoolDT<long>> f_GetList(EM_CardCamp type)
    {
        List<BasePoolDT<long>> result = new List<BasePoolDT<long>>();
        ArrayList list = GetList();
        for (int i = 0; i < list.Count; i++)
        {
            ShopLotteryItem tData = (ShopLotteryItem)list[i];
            if(type == EM_CardCamp.eCardMain || (int)type == tData.camp)
            {
                result.Add(tData);
            }
        }
        
        return result;
    }
}

public class ShopLotteryItem : BasePoolDT<long>
{
    public int key;
    public int value;
    public int camp;
    public ShopLotteryItem(int k, int v) {
        key = k;
        value = v;
        camp = (value % 1000) / 100;
    }
}