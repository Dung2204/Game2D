using ccU3DEngine;
using UnityEngine;
using System.Collections;

public class BuyGoodOnekeyPage : UIFramwork
{
    private UI2DSprite m_Icon;
    private UISprite m_Frame;
    private UILabel m_GoodNum;
    private UILabel m_GoodName;
    private UILabel m_HaveNum;
    
    private UILabel m_BuyNum;
    private UILabel m_TotalPrice;
    private UILabel m_LeftTimes;
    private UIInput _Input;

    private ShopResourcePoolDT data;
    private int buyNum;
    private bool isLimit;
    private int leftTimes;
    private int[] priceArr;
    private int curPrice;
    private int totalPrice;
    private int haveSycee;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        m_Icon = f_GetObject("Icon").GetComponent<UI2DSprite>();
        m_Frame = f_GetObject("Frame").GetComponent<UISprite>();
        m_GoodName = f_GetObject("GoodName").GetComponent<UILabel>();
        m_GoodNum = f_GetObject("GoodNum").GetComponent<UILabel>();
        m_HaveNum = f_GetObject("HaveNum").GetComponent<UILabel>();
        
        m_BuyNum = f_GetObject("BuyNum").GetComponent<UILabel>();
        m_TotalPrice = f_GetObject("TotalPrice").GetComponent<UILabel>();
        m_LeftTimes = f_GetObject("LeftTimes").GetComponent<UILabel>();
        f_RegClickEvent("BtnClose", f_BtnClose);
        f_RegClickEvent("BtnSure", f_BtnSure);
        f_RegClickEvent("BtnAddOne", f_BtnAddOne);
        f_RegClickEvent("BtnAddTen", f_BtnAddTen);
        f_RegClickEvent("BtnReduceOne", f_BtnReduceOne);
        f_RegClickEvent("BtnReduceTen", f_BtnReduceTen);
        f_RegClickEvent("BtnBlack", f_BtnClose);
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        data = (ShopResourcePoolDT)e;
        priceArr = ccMath.f_String2ArrayInt(data.m_ShopResourceDT.szNewNum, ";");
        int vipLv = UITool.f_GetNowVipLv();
        int[] limitArr = ccMath.f_String2ArrayInt(data.m_ShopResourceDT.szVipLimitTimes,";");
        int limit = vipLv >= limitArr.Length ? limitArr[limitArr.Length - 1] : limitArr[vipLv];
        isLimit = limit != 0;
        leftTimes = limit - data.m_iBuyTimes;
m_LeftTimes.text = isLimit ? string.Format("Today's purchases are: {0}", leftTimes) : string.Empty;
        totalPrice = 0;
        haveSycee = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Sycee);
        ResourceCommonDT tResourceDt = new ResourceCommonDT();
        tResourceDt.f_UpdateInfo((byte)data.m_ShopResourceDT.iType, data.m_ShopResourceDT.iTempId, data.m_ShopResourceDT.iNum);
        string goodName = tResourceDt.mName;
        m_Frame.spriteName = UITool.f_GetImporentColorName(tResourceDt.mImportant, ref goodName);
        m_GoodNum.text = tResourceDt.mResourceNum.ToString();
        m_GoodName.text = goodName;
        UITool.f_SetIconSprite(m_Icon,(EM_ResourceType)tResourceDt.mResourceType,tResourceDt.mResourceId);
        m_HaveNum.text = UITool.f_GetMoney(UITool.f_GetResourceHaveNum(tResourceDt.mResourceType, tResourceDt.mResourceId)); 
		//MessageBox.ASSERT("" + UITool.f_GetResourceHaveNum(tResourceDt.mResourceType, tResourceDt.mResourceId));
        buyNum = 0;
        totalPrice = 0;
        m_TotalPrice.text = totalPrice.ToString();
        m_BuyNum.text = buyNum.ToString();
        f_AddOne(true);
        _Input = f_GetObject("InputBuyCountBg").GetComponent<UIInput>();
        UIEventListener.Get(f_GetObject("InputBuyCountBg")).onSelect= _SelectInput;
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }

    private void f_BtnClose(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.BuyGoodOnekeyPage, UIMessageDef.UI_CLOSE);
    }

    private bool f_AddOne(bool needTipPay = false,int AddNum=1)
    {
        if (isLimit && buyNum + AddNum > leftTimes)
        {
UITool.Ui_Trip("Số lượt mua hôm nay: " + leftTimes);
            buyNum = leftTimes;
            return false;
        }
        curPrice = f_GetCurPrice(buyNum + AddNum + data.m_iBuyTimes);
        if (curPrice + totalPrice <= haveSycee)
        {
            totalPrice += curPrice;
            buyNum++;
            m_TotalPrice.text = totalPrice.ToString();
            m_BuyNum.text = buyNum.ToString();
            return true;
        }
        else
        {
            if (needTipPay)
            {
string tipContent = string.Format("Không đủ KNB");
                ccUIManage.GetInstance().f_SendMsg(UINameConst.PayTipPage, UIMessageDef.UI_OPEN,tipContent);
                ccUIManage.GetInstance().f_SendMsg(UINameConst.BuyGoodOnekeyPage, UIMessageDef.UI_CLOSE);
            }
            else
            {
UITool.Ui_Trip("Không đủ tiền, đã mua:" + buyNum);
            }
            return false;
        }
    }

    private int f_GetCurPrice(int buyCount)
    {
        int price = 0;
        price = buyCount >= priceArr.Length ? priceArr[priceArr.Length - 1] : priceArr[buyCount];
        return price;
    }

    private void f_BtnSure(GameObject go, object value1, object value2)
    {
        if (buyNum == 0)
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.BuyGoodOnekeyPage, UIMessageDef.UI_CLOSE);
            return;
        }
        UITool.f_OpenOrCloseWaitTip(true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_Buy;
        socketCallbackDt.m_ccCallbackFail = f_Callback_Buy;
        Data_Pool.m_ShopResourcePool.f_Buy((int)data.iId, buyNum,socketCallbackDt);
    }
    private void f_BtnAddOne(GameObject go, object value1, object value2)
    {
        f_AddOne();
    }
    private void f_BtnAddTen(GameObject go, object value1, object value2)
    {
        for (int i = 0; i < 10; i++)
        {
            if (!f_AddOne())
            {
                break;
            }
        }
    }
    private void f_BtnReduceOne(GameObject go, object value1, object value2)
    {
        if (buyNum - 1 > 0)
        {
            curPrice = f_GetCurPrice(buyNum + data.m_iBuyTimes);
            totalPrice -= curPrice;
            buyNum--;
            m_TotalPrice.text = totalPrice.ToString();
            m_BuyNum.text = buyNum.ToString();
        }
    }

    private void f_BtnReduceTen(GameObject go, object value1, object value2)
    {
        for (int i = 0; i < 10; i++)
        {
            f_BtnReduceOne(null, null, null);
        }
    }

    private void f_Callback_Buy(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        if ((int)result != (int)eMsgOperateResult.OR_Succeed)
        {
            // UITool.UI_ShowFailContent("Mua thất bại，code:" + result); 
UITool.UI_ShowFailContent("Mua thất bại"); 
        }
        ccUIManage.GetInstance().f_SendMsg(UINameConst.BuyGoodOnekeyPage, UIMessageDef.UI_CLOSE);
    }

    private void _SelectInput(GameObject go,bool isSelect) {
        if (isSelect) { return; }
        buyNum = ccMath.atoi(_Input.value);
        f_AddOne(false,0);
        _Input.value = buyNum.ToString();
    }
}
