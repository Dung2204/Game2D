using System;
using ccU3DEngine;
using UnityEngine;
using System.Collections;

public class DialogItem : MonoBehaviour
{
    public GameObject _root;
    public TweenPosition _dialogRoleTween;
    public TweenAlpha _dialogBGTween;
    public TweenAlpha _dialogLabelTween;
    public UIPlayTween _totalAlpha;

    public Transform _roleParent;
    private GameObject _role;
    public UILabel _dialogContent;
    public UILabel _dialogName;

    private DungeonDialogDT mData;
    private ccCallback mFiishCallBack;

    private bool isPlayForwarding = false;
    private int playFinishCount = 0;
    private const int PlayFinishConditions = 2;

    private const string DefaultColorCode = "393939";

    private float time = 3.0f;

    void Awake()
    {
        EventDelegate.Set(_dialogRoleTween.onFinished,f_RoleTweenFinish);
        EventDelegate.Set(_totalAlpha.onFinished, f_TotalAlphaFinish);
    }

    public void f_ShowDialog(DungeonDialogDT dt,ccCallback finishCallBack)
    {
        _root.SetActive(true); 
        if (mData == null || (mData != null && mData.iModeId != dt.iModeId))
        {
            playFinishCount = 0;
            mData = dt;
            f_UpdateRole();
            _dialogRoleTween.ResetToBeginning();
            _dialogBGTween.ResetToBeginning();
            _dialogLabelTween.ResetToBeginning();
            f_WaitForOneFrame(0,_dialogRoleTween.Play,true);
        }
        else
        {
            mData = dt;
            playFinishCount = PlayFinishConditions;
            isPlayForwarding = false;
            f_WaitForOneFrame(0, _totalAlpha.Play, false);
        }
        mFiishCallBack = finishCallBack; 
        if (mData.iModeId == GameParamConst.PlayerRoleId)
        {
            _dialogName.text = Data_Pool.m_UserData.m_szRoleName;
        }
        else
        {
            _dialogName.text = mData.szRoleName;
			if(_dialogName.text == "")
			{
				_dialogName.text = mData._szRoleName;
			}
        }

        time = 3.0f;
        if (!string.IsNullOrEmpty(mData.szMusic))
        {
            string name = mData.szMusic;
            if (mData.szMusic.IndexOf("role", StringComparison.OrdinalIgnoreCase) != -1)
            {
                int tSex = Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_MainCard) % 2;
                name = name.Replace("role", "role" + tSex);
            }
            time = glo_Main.GetInstance().m_AdudioManager.f_PlayAudioDialog(name, 10);
            time = Math.Max(time, 3);
        }
    }
    
    public void f_ShowOver()
    {
        mData = null;
        _root.SetActive(false); 
    }

    private void f_RoleTweenFinish()
    {
        //添加默认颜色码
        _dialogContent.text = string.Format("[{0}]{1}[-]", DefaultColorCode, mData.szDialog);
        isPlayForwarding = true;
        f_WaitForOneFrame(0, _totalAlpha.Play, true);
    }

    private void f_TotalAlphaFinish()
    {
        if (isPlayForwarding)
        {
            f_AllFinish();
        }
        else
        {
            f_RoleTweenFinish();
        }
    }

    private void f_AllFinish()
    {
        if (mFiishCallBack != null)
            mFiishCallBack(time);
        
    }

    /// <summary>
    /// 更新角色形象
    /// </summary>
    private void f_UpdateRole()
    {
        if (mData.iModeId == GameParamConst.PlayerRoleId)
        {
            CardPoolDT tCardPoolDT = Data_Pool.m_CardPool.f_GetPlayerRolePoolDT();
            CardDT tCardDT = tCardPoolDT.m_CardDT;
            if (tCardDT.iStatelId1 != 0)
			{
				MessageBox.ASSERT("Id: " + tCardDT.iStatelId1);
				//Fix Code
				if (tCardPoolDT.m_FanshionableDressPoolDT != null)
				{
					int FashId = tCardPoolDT.m_FanshionableDressPoolDT.m_FashionableDressDT.iModel;
					UITool.f_CreateFashRoleByModeId(FashId, ref _role, _roleParent, 501, true);
				}
				else
				{
					UITool.f_CreateRoleByModeId(tCardDT.iStatelId1, ref _role, _roleParent, 501);
				}
			}
            else if (tCardDT.iStatelId2 != 0)
			{
				MessageBox.ASSERT("Id: " + tCardDT.iStatelId2);
				if (tCardPoolDT.m_FanshionableDressPoolDT != null)
				{
					int FashId = tCardPoolDT.m_FanshionableDressPoolDT.m_FashionableDressDT.iModel;
					UITool.f_CreateFashRoleByModeId(FashId, ref _role, _roleParent, 501, true);
				}
				else
				{
					UITool.f_CreateRoleByModeId(tCardDT.iStatelId2, ref _role, _roleParent, 501);
				}
			}
        }
        else
        {
            UITool.f_CreateRoleByModeId(mData.iModeId, ref _role, _roleParent, 501);
        }
    }
    
    /// <summary>
    /// 延迟播放 默认在下一帧播放，如果需要延迟播放，加延迟时间
    /// </summary>
    /// <param name="delayTime">延迟时间</param>
    /// <param name="callback"></param>
    private void f_WaitForOneFrame(float delayTime, BoolCallback callback,bool isForward)
    {
        StartCoroutine(WaitOneFrameCallback(delayTime,callback,isForward));
    }

    IEnumerator WaitOneFrameCallback(float delayTime, BoolCallback callback,bool isForward)
    {
        yield return 0;
        if (delayTime > 0)
            yield return new WaitForSeconds(delayTime);
        if (callback != null)
        {
            callback(isForward);
        }
    }

    public delegate void BoolCallback(bool isForward);
}

