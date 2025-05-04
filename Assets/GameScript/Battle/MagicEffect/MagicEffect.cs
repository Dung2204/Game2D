using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;
using Spine.Unity;

public class MagicEffect : MonoBehaviour
{
    public UI2DSprite CardHeadMain;   //主头像
    public UI2DSprite CardHeadSup1;    //副头像
    public UI2DSprite CardHeadSup2;
    public UISprite CardHeadMainCase;
    public UISprite CardHeadSup1Case;
    public UISprite CardHeadSup2Case;

    public UIGrid CardHeadGrid;    //头像Grid

    public UILabel MagicName;   //技能名字

    public GameObject CardPoint;   //卡牌位置点

    public MeshRenderer EffectBgColor;

    public Material BlueMaterial;
    public Material GreenMaterial;
    public Material PurpleMaterial;
    public Material OragenMaterial;
    public Material RedMaterial;
    public Material GoldMaterial;

    private Animator EffectAnimator;

    private MagicDT _MagicDT;
    private ccCallback _ChangeStart;
    //private  _Animator;
    private void Start()
    {
        EffectAnimator = GetComponent<Animator>();
    }
    public void f_SetCardHead(MagicDT tmpMagicDT)
    {
        _MagicDT = tmpMagicDT;
        CardHeadMain.sprite2D = UITool.f_GetIconSpriteByCardId(tmpMagicDT.iGroupHero1);
        CardHeadMainCase.spriteName = UITool.f_GetCardImporent(tmpMagicDT.iGroupHero1);

        CardHeadSup1.sprite2D = UITool.f_GetIconSpriteByCardId(tmpMagicDT.iGroupHero2);
        CardHeadSup1Case.spriteName = UITool.f_GetCardImporent(tmpMagicDT.iGroupHero2);
        if (tmpMagicDT.iGroupHero3 != 0)
        {
            CardHeadSup2.sprite2D = UITool.f_GetIconSpriteByCardId(tmpMagicDT.iGroupHero3);
            CardHeadSup2Case.spriteName = UITool.f_GetCardImporent(tmpMagicDT.iGroupHero3);
        }

        CardHeadSup2.gameObject.SetActive(tmpMagicDT.iGroupHero3 != 0);

        CardHeadGrid.Reposition();
    }


    public void f_SetMagicName(string SkillName)
    {
        MagicName.text = SkillName;
    }
    GameObject CardModel;
    private Transform SpineParent;
    private Vector3 SpineScale;
    private string SpineAnimationName;
    /// <summary>
    /// 设置模型
    /// 需要改变父级 大小  和动作
    /// </summary>
    /// <param name="CardModel">模型</param>
    public void f_SetCardPoint(GameObject CardModel)
    {
        this.CardModel = CardModel;
        Transform SpineTran = CardModel.transform;
        SpineParent = SpineTran.parent;
        SpineScale = SpineTran.localScale;
        SpineAnimationName = SpineTran.GetComponent<SkeletonAnimation>().AnimationName;


        SetModel(CardModel, CardPoint.transform,new Vector3(0, -150, 0), Vector3.one * 180, "");

        //_Animator.
    }
    private void SetModel(GameObject Model, Transform Parent,Vector3 position, Vector3 Scale, string AnimationName)
    {
        Transform ModelTransform = Model.transform;
        ModelTransform.parent = Parent;
        ModelTransform.localScale = Scale;
        ModelTransform.localPosition = position;
        ModelTransform.localEulerAngles = Vector3.zero;
        ModelTransform.GetComponent<SkeletonAnimation>().AnimationName = AnimationName;
    }


    public void f_SetChangeAttack(ccCallback Change)
    {
        _ChangeStart = Change;
    }
    /// <summary>
    /// 设置背景流光颜色
    /// </summary>
    /// <param name="Important"></param>
    public void f_SetEffectBgColor(int Important)
    {
        switch ((EM_Important)Important)
        {
            case EM_Important.Green:
                EffectBgColor.material = GreenMaterial;
                break;
            case EM_Important.Blue:
                EffectBgColor.material = BlueMaterial;
                break;
            case EM_Important.Purple:
                EffectBgColor.material = PurpleMaterial;
                break;
            case EM_Important.Oragen:
                EffectBgColor.material = OragenMaterial;
                break;
            case EM_Important.Red:
                EffectBgColor.material = RedMaterial;
                break;
            case EM_Important.Gold:
            case EM_Important.None:
                EffectBgColor.material = GoldMaterial;
                break;
        }
    }
    public void f_SetRotation(int Rotation) {
        transform.localEulerAngles = new Vector3(0, Rotation, 0);
        EffectBgColor.transform.localEulerAngles = new Vector3(0,180- Rotation,0);
        EffectBgColor.transform.localPosition = new Vector3(0,0, Rotation == 180 ? -30 : 0);
        MagicName.transform.localEulerAngles= new Vector3(0, Rotation, 0);
    }
    public void f_DeleGameobjectEvent()
    {
        SetModel(CardModel, SpineParent, Vector3.zero, SpineScale, SpineAnimationName);
        glo_Main.GetInstance().m_UIMessagePool.f_Broadcast(UIMessageDef.UI_BATTLE_ACTIVE, true);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.FitSkillPage, UIMessageDef.UI_CLOSE);
        if (_ChangeStart != null)
        {
            _ChangeStart(null);
        }
        //Destroy(gameObject);
    }
}
