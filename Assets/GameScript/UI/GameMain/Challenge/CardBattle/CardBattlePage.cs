using ccU3DEngine;
using UnityEngine;
using System.Collections.Generic;

public class CardBattlePage : UIFramwork
{
public const string HELP_CONTENT = "1、Heavenly Martial Arts Festival is divided into 2 phases 【Preparation】 and 【Opening】，players need to line up before participating\n" +
                                        "2、Người chơi trong giai đoạn 【Chuẩn bị】 có 3 lần làm mới tướng。Kéo tướng để đưa vào đội hình，sau đó xác nhận để tham gia\n" +
                                        "3、Trong giai đoạn 【Chuẩn bị】 sẽ kiểm tra duyên giữa các tướng，qua đó tăng sức mạnh đội hình\n" +
                                        "4、Chỉ có thể thay đổi đội hình trong giai đoạn 【Chuẩn bị】\n" +
                                        "5、Trong giai đoạn 【Khai mạc】，người chơi có 10 lượt khiêu chiến，phần thưởng sẽ dựa trên số lần thắng\n" +
                                        "6、Thời gian hoạt động： Thứ 4、Thứ 7 hằng tuần. 【Chuẩn bị】9：00-17：55，【Khai mạc】18：00-24:00\n" +
                                        "7、Phát thưởng： khi dùng hết 10 lượt khiêu chiến hoặc đạt 24 điểm\n";

    private CardBattleApplyPage mApplyPage;
    private CardBattleChallengePage mChallengePage;

    private CardBattlePool.EM_CardBattleState m_CBState = CardBattlePool.EM_CardBattleState.Invalid;
    private int m_TimeEventId = 0;

    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        mApplyPage = f_GetObject("ApplyPage").GetComponent<CardBattleApplyPage>();
        mChallengePage = f_GetObject("ChallengePage").GetComponent<CardBattleChallengePage>();
        mApplyPage.f_Init(this);
        mChallengePage.f_Init(this);
        f_RegClickEvent("BtnReturn", f_OnBtnReturnClick); 
    }

    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);

        //断线重连如果切去其他界面，这时候不处理，服务器消息返回再打开界面就会和其他界面重叠
        if (!UITool.f_IsCanOpenChallengePage(UINameConst.CardBattlePage))
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.CardBattlePage, UIMessageDef.UI_CLOSE);
            return;
        }

        m_CBState = Data_Pool.m_CardBattlePool.f_GetState();
        if (m_CBState == CardBattlePool.EM_CardBattleState.Invalid)
        {
UITool.Ui_Trip("Hoạt động chưa mở");
            ccUIManage.GetInstance().f_SendMsg(UINameConst.CardBattlePage, UIMessageDef.UI_CLOSE);
            ccUIHoldPool.GetInstance().f_UnHold();
            return;
        }
        f_LoadTexture();
        f_OpenOrCloseMoneyTopPage(true);
        UITool.f_OpenOrCloseWaitTip(true, true);
        SocketCallbackDT socketCallbackDt = new SocketCallbackDT();
        socketCallbackDt.m_ccCallbackSuc = f_Callback_CardBattleInit;
        socketCallbackDt.m_ccCallbackFail = f_Callback_CardBattleInit;
        Data_Pool.m_CardBattlePool.f_CardBattleInit(socketCallbackDt);
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        f_OpenOrCloseMoneyTopPage(false);
        if (m_TimeEventId != 0)
            ccTimeEvent.GetInstance().f_UnRegEvent(m_TimeEventId);
        mApplyPage.f_Close();
        mChallengePage.f_Close();
    }

    private void f_Callback_CardBattleInit(object result)
    {
        UITool.f_OpenOrCloseWaitTip(false);
        m_CBState = Data_Pool.m_CardBattlePool.f_GetState();
        //过来报名时间 如果防守阵容为空（攻击阵容在刷新阵容会和防守阵容一致） 就视为无效阵容
        if (m_CBState == CardBattlePool.EM_CardBattleState.BetweenApplyBattle || m_CBState == CardBattlePool.EM_CardBattleState.InBattle)
        {
            bool invalid = true;
            for (int i = 0; i < Data_Pool.m_CardBattlePool.DefClothList.Count; i++)
            {
                if (Data_Pool.m_CardBattlePool.DefClothList[i].CardTemplateId != 0)
                {
                    invalid = false;
                    break;
                }
            }
            if (invalid)
            {
UITool.Ui_Trip("Đội hình không hợp lệ");
                ccUIManage.GetInstance().f_SendMsg(UINameConst.CardBattlePage, UIMessageDef.UI_CLOSE);
                ccUIHoldPool.GetInstance().f_UnHold();
                return;
            }
        }
        f_UpdateByState(Data_Pool.m_CardBattlePool.f_GetState());
        m_TimeEventId = ccTimeEvent.GetInstance().f_RegEvent(1.0f, true, null, f_UpdateBySecond);
    }

    private void f_UpdateByState(CardBattlePool.EM_CardBattleState state)
    {
        m_CBState = state;
        if (m_CBState == CardBattlePool.EM_CardBattleState.InApply)
        {
            mApplyPage.f_Open();
            mChallengePage.f_Close();
        }
        else if (m_CBState == CardBattlePool.EM_CardBattleState.BetweenApplyBattle ||
            m_CBState == CardBattlePool.EM_CardBattleState.InBattle)
        {
            mApplyPage.f_Close();
            mChallengePage.f_Open();
        }
        else
        {
UITool.Ui_Trip("Hoạt động đã kết thúc");
            mApplyPage.f_Close();
            mChallengePage.f_Close();
            ccUIManage.GetInstance().f_SendMsg(UINameConst.CardBattlePage, UIMessageDef.UI_CLOSE);
            ccUIHoldPool.GetInstance().f_UnHold();
        }
    }

    private void f_OnBtnReturnClick(GameObject go, object value1, object value2)
    {
        ccUIManage.GetInstance().f_SendMsg(UINameConst.CardBattlePage, UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_UnHold();
    }

    private void f_UpdateBySecond(object value)
    {
        CardBattlePool.EM_CardBattleState state = Data_Pool.m_CardBattlePool.f_GetState();
        if (m_CBState == CardBattlePool.EM_CardBattleState.InApply &&
            (state == CardBattlePool.EM_CardBattleState.BetweenApplyBattle ||
            state == CardBattlePool.EM_CardBattleState.InBattle))
        {
UITool.Ui_Trip("Đã hết thời gian đăng ký");
            f_UpdateByState(state);
        }
        mApplyPage.f_UpdateBySecond();
        mChallengePage.f_UpdateBySecond();
    }

    private void f_OpenOrCloseMoneyTopPage(bool isOpen)
    {
        if (isOpen)
        {
            List<EM_MoneyType> listMoneyType = new List<EM_MoneyType>();
            listMoneyType.Add(EM_MoneyType.eUserAttr_Sycee);
            listMoneyType.Add(EM_MoneyType.eUserAttr_Money);
            listMoneyType.Add(EM_MoneyType.eUserAttr_Energy);
            ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_OPEN, listMoneyType);
        }
        else
        {
            ccUIManage.GetInstance().f_SendMsg(UINameConst.TopMoneyPage, UIMessageDef.UI_CLOSE);
        }
    }

    private string strTexBgRoot = "UI/TextureRemove/MainMenu/Tex_BattleFormBg";
    private string strApplyTexBgRoot = "UI/TextureRemove/MainMenu/Tex_ClothArrayBg";
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {
        //加载背景图
        //My Code
        float windowAspect = (float)Screen.width / (float)Screen.height;
        MessageBox.ASSERT("" + windowAspect);
        if (windowAspect < 1.6)
        {
            f_GetObject("LeftRoot").transform.localPosition = new Vector3(150f, 0f, 0f);
            f_GetObject("RightRoot").transform.localPosition = new Vector3(-520f, 0f, 0f);
            f_GetObject("RightRoot").transform.localScale = new Vector3(1f, 1f, 1f);
            f_GetObject("ChallengePage").transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
			f_GetObject("RoleBg2").transform.localPosition = new Vector3(0f, 26f, 0f);
        }
        //
        //    UITexture texBg = f_GetObject("TexBg").GetComponent<UITexture>();
        //    UITexture applyTexBg = f_GetObject("ApplyTexBg").GetComponent<UITexture>();
        //    if (texBg.mainTexture == null)
        //    {
        //        Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBgRoot);
        //        texBg.mainTexture = tTexture2D;

        //        Texture2D tTexMagic = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strApplyTexBgRoot);
        //        applyTexBg.mainTexture = tTexMagic;
        //    }
        //
    }
}
