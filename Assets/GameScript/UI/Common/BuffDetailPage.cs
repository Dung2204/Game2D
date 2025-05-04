using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuffDetailPage : UIFramwork
{
    private UIWrapComponent _WrapComponent = null;
    private UIWrapComponent mWrapComponent
    {
        get
        {
            if (_WrapComponent == null)
            {
                _WrapComponent = new UIWrapComponent(120, 1, 600, 5, f_GetObject("ItemParent"), f_GetObject("Item"), GetList(), ItemUpdateByInfo, null);
            }
            return _WrapComponent;
        }
    }

    private List<int> m_ListBuffId = new List<int>();
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        m_ListBuffId = e as List<int>;
        if (_WrapComponent != null)
        {
            mWrapComponent.f_UpdateList(GetList());
            mWrapComponent.f_UpdateView();
            return;
        }
        mWrapComponent.f_ResetView();
    }

    protected override void f_Create()
    {
        base.f_Create();
    }

    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        f_RegClickEvent("BlackBG", OnBtnBlackClick);
    }

    private void OnBtnBlackClick(GameObject go, object obj1, object obj2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.BuffDetailPage, UIMessageDef.UI_CLOSE);
    }
    private List<NBaseSCDT> GetList()
    {
        List<NBaseSCDT> result = new List<NBaseSCDT>();
        Dictionary<int, int> m_DictList = new Dictionary<int, int>();

        for (int i = 0; i < m_ListBuffId.Count; i++)
        {
            BufDT _BufDT = (BufDT)glo_Main.GetInstance().m_SC_Pool.m_BufSC.f_GetSC(m_ListBuffId[i]);
            if (_BufDT == null) continue;
            string spriteName = "";
            switch (_BufDT.iType)
            {
                case 1:
                    spriteName = _BufDT.iType.ToString() + _BufDT.iPara.ToString();
                    if (_BufDT.iParaY > 0)//% buff số âm là debuff
                    {
                        spriteName += "1";
                    }
                    else
                    {
                        spriteName += "0";
                    }
                    // 110
                    //111
                    break;
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                    spriteName = _BufDT.iType.ToString();// 2-6
                    break;
                case 7:
                    spriteName = _BufDT.iType.ToString();//70-71
                    if (_BufDT.iParaY > 10000)//% buff lấy mốc 10000 là 100% 
                    {
                        spriteName += "1";
                    }
                    else
                    {
                        spriteName += "0";
                    }
                    break;
                case 8: // hồi phục mỗi lượt
                    spriteName = _BufDT.iType.ToString();
                    break;
                case 9: // tăng máu max
                    spriteName = _BufDT.iType.ToString();
                    break;
                default:
                    spriteName = _BufDT.iType.ToString();
                    break;
            }
            int iKey = int.Parse(spriteName);
            int value;
            if(m_DictList.TryGetValue(iKey, out value)){
                m_DictList[iKey] = 1;
            }
            else
            {
                m_DictList.Add(iKey, 1);
            }
        }
        if(m_DictList.Count> 0)
        {
            for (int i = 0; i < m_DictList.Count; i++)
            {
                int key = m_DictList.ElementAt(i).Key;
                BufDetailDT _BufDetailDT = (BufDetailDT)glo_Main.GetInstance().m_SC_Pool.m_BufDetailSC.f_GetSC(key);
                if(_BufDetailDT!= null)
                {
                    result.Add(_BufDetailDT);
                }
            }
        }
        MessageBox.DEBUG("BufDetailDT lenght=" + result.Count);
        return result;
    }
    private void ItemUpdateByInfo(Transform item, NBaseSCDT dt)
    {
        BufDetailDT data = (BufDetailDT)dt;
        UISprite Icon = item.Find("icon").GetComponent<UISprite>();
        Icon.spriteName = data.icon;
        UILabel Name = item.Find("name").GetComponent<UILabel>();
        Name.text = data.szName;
        UILabel Desc = item.Find("desc").GetComponent<UILabel>();
        Desc.text = data.szDesc;
    }
}
