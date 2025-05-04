using UnityEngine;
using System.Collections;
using ccU3DEngine;

public class TariningItem : UIFramwork
{
    public int Idx;

    public ResourceCommonItem ResourceItem;
    private ResourceCommonDT tResourceCommonDT;

    private TweenRotation _TweenRotation;
    private TweenPosition _TweenPosition;

    private ccCallback _SendTarining;
    private ccCallback _GetAward;

    private TariningItemParam tTariningItemParam = new TariningItemParam();


    private TweenPosition _EffectLabelPosition;
    private TweenColor _EffectLabelColor;
    private void OnEnable()
    {
        tResourceCommonDT = new ResourceCommonDT();

        _TweenRotation = GetComponent<TweenRotation>();
        _TweenPosition = f_GetObject("TariningLine").GetComponent<TweenPosition>();
        _TweenPosition.gameObject.SetActive(false);
        f_GetObject("line_01").GetComponent<TrailRenderer>().sortingLayerName = GameParamConst.UILayerName;// = 3;
        f_GetObject("line_01_1").GetComponent<TrailRenderer>().sortingLayerName = GameParamConst.UILayerName;
        transform.localRotation = Quaternion.Euler(Vector3.zero);

        f_RegClickEvent("ItemBg", Btn_ClickItem);
        f_RegClickEvent("Btn_GetAward", Btn_GetAward);

        _EffectLabelPosition = f_GetObject("EffectLabel").GetComponent<TweenPosition>();
        _EffectLabelColor = f_GetObject("EffectLabel").GetComponent<TweenColor>();
        
    }

    /// <summary>
    /// 类似初始化
    /// </summary>                                
    public void UpdateItem(int type, int id, int num, int idx, int time, ccCallback _SendTarining, ccCallback _GetAward)
    {
        if(time != 0)
        {
            tResourceCommonDT.f_UpdateInfo((byte)type, id, num);
            ResourceItem.f_UpdateByInfo(tResourceCommonDT);
        }
        else
        {
            tResourceCommonDT = null;
        }

        f_GetObject("ResourceItem").SetActive(time != 0);
		f_GetObject("ItemBgUp").SetActive(time != 0);
        Idx = idx;
        this._SendTarining = _SendTarining;
        this._GetAward = _GetAward;

        tTariningItemParam.idx = Idx;
        tTariningItemParam._ReturnSockect = PlayTween;
        tTariningItemParam._ReturnGetAward = GetAwardEffect;

        f_GetObject("Point").transform.position = f_GetObject("Btn_Tactical").transform.position;
    }

    public void OneKeyTweenPlay(object obj)
    {
        PlayTween(obj);
    }

    public void OneKeyGetAward(object obj)
    {
        GetAwardEffect(obj);
    }

    public bool OneKeyShowItem()
    {
        return tResourceCommonDT != null;
    }

    private void UpdateItem(ResourceCommonDT tResourceCommonDT, bool IsShowAward = true)
    {
        f_GetObject("ResourceItem").SetActive(IsShowAward);
		f_GetObject("ItemBgUp").SetActive(IsShowAward);
        this.tResourceCommonDT = tResourceCommonDT;
        if(tResourceCommonDT != null)
        {
            ResourceItem.f_UpdateByInfo(tResourceCommonDT);
        }
    }

    private void Btn_ClickItem(GameObject go, object obj1, object obj2)
    {

        //SocketCallbackDT ttt = new SocketCallbackDT();
        //ttt.m_ccCallbackSuc = tttttSuc;
        //ttt.m_ccCallbackFail = tttttfail;
        //Data_Pool.m_TariningAndTacticalPool.f_TrainingTransSend((byte)Idx, ttt);


        //_OnItemClickStar();

        //if(TariningPage.IsTween)
        //{
        //    return;
        //}
        _SendTarining(tTariningItemParam);
    }

    private void Btn_GetAward(GameObject go, object obj1, object obj2)
    {
        //SocketCallbackDT ttt = new SocketCallbackDT();
        //ttt.m_ccCallbackSuc = tSuc;
        //ttt.m_ccCallbackFail = tFail;
        //Data_Pool.m_TariningAndTacticalPool.f_TrainingGetAward((byte)Idx, ttt);

        //PlayEffect();
        if(TariningPage.IsTween)
        {
            return;
        }
        _GetAward(tTariningItemParam);
    }

    #region 测试代码
    private void tttttSuc(object obj)
    {
        MessageBox.DEBUG(CommonTools.f_GetTransLanguage(1624) + Idx);
    }

    private void tttttfail(object obj)
    {
        MessageBox.DEBUG(CommonTools.f_GetTransLanguage(1625) + obj + Idx);
    }

    private void tSuc(object obj)
    {
        MessageBox.DEBUG(CommonTools.f_GetTransLanguage(1626) + obj + Idx);
    }

    private void tFail(object obj) { MessageBox.DEBUG(CommonTools.f_GetTransLanguage(1627) + obj + Idx); }
    #endregion


    private void PlayTween(object obj)
    {
        ResourceCommonDT tResourceCommonDT = (ResourceCommonDT)obj;
        this.tResourceCommonDT = tResourceCommonDT;
        f_GetObject("ItemBg").GetComponent<BoxCollider>().enabled = false;
        _OnItemClickStar();
    }
    private void GetAwardEffect(object obj)
    {
        TariningPage.IsTween = true;
        //PlayEffect();
        if(tResourceCommonDT.mResourceType == (int)EM_ResourceType.Good)
        {
            PlayEffect();
        }
        else if(tResourceCommonDT.mResourceType == (int)EM_ResourceType.Money)
        {
            EffectLabel();
        }
        UpdateItemClear();
    }
    #region 翻牌的动画

    private void _OnItemClickStar()
    {
        TariningPage.IsTween = true;
        _TweenRotation.from = Vector3.zero;
        _TweenRotation.to = new Vector3(0, 90, 0);
        _TweenRotation.onFinished.Clear();
        _TweenRotation.onFinished.Add(new EventDelegate(_OnItemClickmiddle));

        PlayTwwen();
    }

    private void _OnItemClickmiddle()
    {
        UpdateItem(tResourceCommonDT);
        _TweenRotation.from = new Vector3(0, 90, 0);
        _TweenRotation.to = Vector3.zero;
        _TweenRotation.onFinished.Clear();
        _TweenRotation.onFinished.Add(new EventDelegate(_OnItemClickEnd));
        PlayTwwen();
    }

    private void _OnItemClickEnd()
    {
        _TweenRotation.Play(false);
        TariningPage.IsTween = false;
    }
    private void PlayTwwen()
    {
        _TweenRotation.ResetToBeginning();
        _TweenRotation.Play(true);
    }

    #endregion

    #region 领取奖励的动画

    private void PlayEffect()
    {
        _TweenPosition.gameObject.SetActive(true);
        _TweenPosition.onFinished.Clear();
        _TweenPosition.onFinished.Add(new EventDelegate(PlayEffectEnd));
        _TweenPosition.onFinished.Add(new EventDelegate(SetBox));
        _TweenPosition.from = new Vector3(0, 0, -280);
        _TweenPosition.to = new Vector3(f_GetObject("Point").transform.localPosition.x, f_GetObject("Point").transform.localPosition.y, -280);

        _TweenPosition.ResetToBeginning();
        _TweenPosition.Play(true);
    }

    private void PlayEffectEnd()
    {
        _TweenPosition.gameObject.SetActive(false);
        
        f_GetObject(UIEffectName.lianbingchang_lianbing_01).SetActive(true);
        ParticleSystem[] tParticleSystem = f_GetObject(UIEffectName.lianbingchang_lianbing_01).GetComponentsInChildren<ParticleSystem>();
        for(int i = 0; i < tParticleSystem.Length; i++)
        {
            if(tParticleSystem[i] != null)
                tParticleSystem[i].Play();
        }
        //ccTimeEvent.GetInstance().f_RegEvent(1f,false,null, SetIsTween);
        //TariningPage.IsTween = false;
    }

    private void SetBox() {
        TariningPage.IsTween = false;
        f_GetObject("ItemBg").GetComponent<BoxCollider>().enabled = true;
    }

    private void SetIsTween(object obj) {
        TariningPage.IsTween = false;
    }

    private void UpdateItemClear()
    {
        UpdateItem(null, false);
    }

    private void EffectLabel()
    {
        
        _EffectLabelColor.onFinished.Clear();
        _EffectLabelColor.onFinished.Add(new EventDelegate(SetBox));
        //_EffectLabelColor.gameObject.SetActive(true);
        _EffectLabelColor.gameObject.GetComponent<UILabel>().text = "+" + tResourceCommonDT.mResourceNum.ToString();
        _EffectLabelColor.ResetToBeginning();
        _EffectLabelColor.Play(true);

        _EffectLabelPosition.ResetToBeginning();
        _EffectLabelPosition.Play(true);
    }


    #endregion
}

public class TariningItemParam
{

    public int idx;
    public ccCallback _ReturnSockect;
    public ccCallback _ReturnGetAward;
}

