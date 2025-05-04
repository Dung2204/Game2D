using UnityEngine;
using System.Collections.Generic;
using System.Text;

public class TaskItem : MonoBehaviour
{
    public UI2DSprite mIcon;
    public GameObject mGotoBtn;
    public GameObject mGetAwardBtn;
    public GameObject mArealyGetTip;

    public UILabel mTitleLable;
    public UILabel mProgressLabel;

    public UIGrid mAwardGrid;
    public GameObject mAwardItem;

    private ResourceCommonItemComponent _awardShowComponent;
    public ResourceCommonItemComponent mAwardShowComponent
    {
        get
        {
            if (_awardShowComponent == null)
            {
                _awardShowComponent = new ResourceCommonItemComponent(mAwardGrid, mAwardItem);
            }
            return _awardShowComponent;
        }
    }

    public UILabel mAwardLabel;
    public void f_UpdateByInfo(TaskDailyPoolDT dt)
    {
        List<AwardPoolDT> tAwardList = Data_Pool.m_AwardPool.f_GetAwardPoolDTByAwardId((int)EM_ResourceType.Money, (int)EM_UserAttr.eUserAttr_TaskScore, dt.mCurScore, dt.mCurAwardId);
        if (tAwardList.Count >= 2)
            mIcon.sprite2D = UITool.f_GetIconSprite(tAwardList[1].mTemplate.mIcon);
        mGotoBtn.SetActive(dt.mTaskState == EM_TaskState.Unfinished);
        mGetAwardBtn.SetActive(dt.mTaskState == EM_TaskState.Finish);
        mArealyGetTip.SetActive(dt.mTaskState == EM_TaskState.AlreadyAward);
        mTitleLable.text = dt.mTemplate.szDesc.Replace(GameParamConst.ReplaceFlag,dt.mTemplate.iConditionParam.ToString());
        mProgressLabel.text = string.Format("({0}/{1})",Mathf.Min(dt.mProgress,dt.mTemplate.iConditionParam),dt.mTemplate.iConditionParam); 
        if (mAwardLabel == null)
            return; 
        StringBuilder tAwardSb = new StringBuilder();
        for (int i = 0; i < tAwardList.Count; i++)
        {
            tAwardSb.AppendFormat("{0} x {1}        ", tAwardList[i].mTemplate.mName, tAwardList[i].mTemplate.mResourceNum);
        }
        mAwardLabel.text = tAwardSb.ToString();
    }

    public void f_UpdateByInfo(TaskAchvPoolDT dt)
    {
        mIcon.sprite2D = UITool.f_GetIconSprite(dt.mTemplate.iIcon);
        mGotoBtn.SetActive(dt.mTaskState == EM_TaskState.Unfinished);
        mGetAwardBtn.SetActive(dt.mTaskState == EM_TaskState.Finish);
        mArealyGetTip.SetActive(dt.mTaskState == EM_TaskState.AlreadyAward);
        mTitleLable.text = dt.mTemplate.szDesc.Replace(GameParamConst.ReplaceFlag, dt.mTemplate.iConditionParam.ToString());
        mProgressLabel.text = string.Format("({0}/{1})", Mathf.Min(dt.mProgress,dt.mTemplate.iConditionParam), dt.mTemplate.iConditionParam);
        mAwardShowComponent.f_Show(Data_Pool.m_AwardPool.f_GetAwardPoolDTByAwardId(dt.mTemplate.iAwardId), EM_CommonItemShowType.All, EM_CommonItemClickType.AllTip);
    }
}
