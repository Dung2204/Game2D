using ccU3DEngine;
using UnityEngine;
using System.Collections;

public class BoxCommonItem : MonoBehaviour
{
    public static BoxCommonItem f_Create(GameObject parent, GameObject item)
    {
        GameObject go = NGUITools.AddChild(parent, item);
        NGUITools.MarkParentAsChanged(go);
        BoxCommonItem result = go.GetComponent<BoxCommonItem>();
        if (result == null)
MessageBox.ASSERT("f_Create in Item must contain BoxCommonItem");
        else
            result.f_Init();
        return result;
    }

    public GameObject mItem;
    public UISprite mIcon;
    public Transform mEffectParent;
    public GameObject mStarIcon;
    public UILabel mNum;
    public UILabel mExtendInfo;

    private ccCallback _clickHandle;
    private object _clickParam;

    private GameObject _effectLock;
    private GameObject _effectCanGet;

    public void f_Init()
    {
        ccUIEventListener.Get(mItem).onClickV2 = f_BoxClickHandle;
    }

    public void f_UpdateClickHandle(ccCallback clickHandle, object value)
    {
        _clickParam = value;
        _clickHandle = clickHandle;
    }

    private void f_BoxClickHandle(GameObject go, object obj1, object obj2)
    {
        if (_clickHandle != null)
            _clickHandle(_clickParam);
    }

    public void f_UpdateInfo(EM_BoxType boxType, EM_BoxGetState boxState, string extendInfo,bool needEffect = false)
    {
        if (boxState == EM_BoxGetState.Invalid)
        {
            mItem.SetActive(false);
            return;
        }
        mItem.transform.Find("Reddot").gameObject.SetActive(boxState == EM_BoxGetState.CanGet);
        mItem.SetActive(true);
        if (boxState == EM_BoxGetState.AlreadyGet)
        {
            mIcon.spriteName = "ptfb_get_c";
        }
        else
        {
            mIcon.spriteName = boxState == EM_BoxGetState.Lock ? "ptfb_get_cc" : "ptfb_get_cc";
        }
        if (boxType == EM_BoxType.Chapter)
        {
            mNum.text = extendInfo;
            mExtendInfo.text = string.Empty;
        }
        else
        {
            mNum.text = string.Empty;
            mExtendInfo.text = extendInfo;
        }
        mIcon.MakePixelPerfect();
        mStarIcon.SetActive(boxType == EM_BoxType.Chapter);
        if (!needEffect)
        {
            if (_effectCanGet != null)
            {
                GameObject.Destroy(_effectCanGet);
                _effectCanGet = null;
            }
            if (_effectLock != null)
            {
                GameObject.Destroy(_effectLock);
                _effectLock = null;
            }
            return;
        }
       // ProcessEffectByState(boxState);
    }

    private void ProcessEffectByState(EM_BoxGetState state)
    {
        if (state == EM_BoxGetState.Lock)
        {
            if (_effectCanGet != null)
            {
                GameObject.Destroy(_effectCanGet);
                _effectCanGet = null;
            }
            if (_effectLock == null)
            {
                _effectLock = UITool.f_CreateEffect(UIEffectName.UIEffectAddress2, UIEffectName.BoxLockEffect, mEffectParent);
            }
        }
        else if (state == EM_BoxGetState.CanGet)
        {
            if (_effectCanGet == null)
            {
                _effectCanGet = UITool.f_CreateEffect(UIEffectName.UIEffectAddress2, UIEffectName.BoxCanGetEffect, mEffectParent);
            }
            if (_effectLock != null)
            {
                GameObject.Destroy(_effectLock);
                _effectLock = null;
            }
        }
        else
        {
            if (_effectLock != null)
            {
                GameObject.Destroy(_effectLock);
                _effectLock = null;
            }
            if (_effectCanGet != null)
            {
                GameObject.Destroy(_effectCanGet);
                _effectCanGet = null;
            }
        }
    }
}
