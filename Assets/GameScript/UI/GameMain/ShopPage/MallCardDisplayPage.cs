using ccU3DEngine;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MallCardDisplayPage : UIFramwork
{
    private UISprite spriteCamp;
    private UILabel txtName;
    private UILabel txtSkill;
    private UILabel txtCardDesc;
    private EquipSythesis mCardDt;
    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        InitGUI();
    }

    protected override void InitGUI()
    {
        base.InitGUI();
        f_RegClickEvent("Texture_BG", OnClose);
        spriteCamp = f_GetObject("Sprite_Camp").GetComponent<UISprite>();
        txtName = f_GetObject("Label_Name").GetComponent<UILabel>();
        txtSkill = f_GetObject("Label_Skill").GetComponent<UILabel>();
        txtCardDesc = f_GetObject("Label_CardDesc").GetComponent<UILabel>();
        initBg();
    }

    private string strTextureBg = "UI/TextureRemove/Shop/Tex_ShopBg";

    private void initBg() {
        UITexture t = f_GetObject("Texture_BG").GetComponent<UITexture>();

        if (t.mainTexture==null) {
            t.mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTextureBg);
        }
    }

    /// <summary>
    /// 打开界面
    /// </summary>
    /// <param name="e">EquipSythesis类似</param>
    protected override void UI_OPEN(object e)
    {
		glo_Main.GetInstance().m_AdudioManager.f_PlayAudioEffect(AudioEffectType.TenRaffle);
        if (null == e) {
MessageBox.ASSERT("No parameters passed in！");
            return;
        }
        base.UI_OPEN(e);
        mCardDt = (EquipSythesis)e;

        //设置卡牌信息
        CardDT cardDt = (CardDT)glo_Main.GetInstance().m_SC_Pool.m_CardSC.f_GetSC(mCardDt.id);
        if (null == cardDt) {
MessageBox.ASSERT("Hero does not exist，id： " + mCardDt.id);
            return;
        }
        // txtName.text = UITool.f_GetImporentForName(cardDt.iImportant, cardDt.szName);
		//My Code
			txtName.text = UITool.f_GetImporentForName(cardDt.iImportant, cardDt.szName).Replace(" ","\n");
		//
        txtCardDesc.text = cardDt.szCardDesc;
        spriteCamp.spriteName = "IconCamp_" + cardDt.iCardCamp;// UITool.f_GetCardCampForUISpriteName(cardDt.iCardCamp);
        spriteCamp.MakePixelPerfect();


        //设置技能信息
        MagicDT[] tmpMagic = UITool.f_GetCardMagic(cardDt);
        if (tmpMagic.Length <= 0) {
MessageBox.ASSERT("Skill does not exist， champion id："+cardDt.iId);
            return;
        }
        txtSkill.text = string.Format("[efedcc][f1bf49]{0}[-]{1}[-]", tmpMagic[1].szName, tmpMagic[1].szReadme);

        //定位
        f_GetObject("Label_CardFightType").GetComponent<UILabel>().text = UITool.f_GetCardFightType((EM_CardFightType)cardDt.iCardFightType);


        //加载模型(删除旧模型（如果有），加载新模型)
        GameObject ModelPoint = f_GetObject("ModelParent");
        if (null == ModelPoint)
            return;
        if (ModelPoint.transform.Find("Model") != null)
            UITool.f_DestoryStatelObject(ModelPoint.transform.Find("Model").gameObject);
        GameObject card = UITool.f_GetStatelObject(cardDt.iId, ModelPoint.transform, Vector3.zero, Vector3.zero);
        card.transform.localScale = Vector3.one * 100;
        card.GetComponent<Renderer>().sortingLayerName = "Default";
        card.GetComponent<Renderer>().sortingOrder = 51;
    }

    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
    }

    private void OnClose(GameObject go, object value1, object value2)
    {
      
        GameObject ModelPoint = f_GetObject("ModelParent");
        if (null == ModelPoint)
            return;
        if (ModelPoint.transform.Find("Model") != null)
            UITool.f_DestoryStatelObject(ModelPoint.transform.Find("Model").gameObject);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.MallCardDisplayPage, UIMessageDef.UI_CLOSE);
        ccUIHoldPool.GetInstance().f_UnHold();
        if (null != mCardDt.m_OnCloseCallback)
        {
            mCardDt.m_OnCloseCallback(mCardDt.mCallbackParam);
        }
    }
}
