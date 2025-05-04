using UnityEngine;
using System.Collections.Generic;


public class TipAniTemplateSC
{
    private const string c_FileName = "AniTemplate.bin";
    private List<TipAniTemplateDT> _dataList;
    private Dictionary<int, int> _templateIdxInfo;

    public TipAniTemplateSC()
    {
        Init();
    }

    private void Init()
    {
        _dataList = CommonTools.f_DeserializeTemplate<TipAniTemplateDT>(Application.streamingAssetsPath, c_FileName);
        _templateIdxInfo = new Dictionary<int, int>();
        //初始化templateIdxInfo
        for (int i = 0; i < _dataList.Count; i++)
        {
            if (_templateIdxInfo.ContainsKey(_dataList[i].Id))
            {
                if (_templateIdxInfo[_dataList[i].Id] < _dataList[i].Idx)
                {
                    _templateIdxInfo[_dataList[i].Id] = _dataList[i].Idx;
                }
            }
            else
            {
                _templateIdxInfo.Add(_dataList[i].Id, _dataList[i].Idx);
            }
        }
    }

    /// <summary>
    /// 获取某个动画模板对应Idx的数据
    /// </summary>
    /// <param name="templateId"></param>
    /// <param name="idx"></param>
    /// <returns></returns>
    public List<TipAniTemplateDT> f_GetAniDatas(int templateId, int idx)
    {
        return _dataList.FindAll(delegate(TipAniTemplateDT item) { return item.Id == templateId && item.Idx == idx; });
    }

    /// <summary>
    /// 获取某个动画模板的IdxMax
    /// </summary>
    /// <param name="templateId"></param>
    /// <returns></returns>
    public int f_GetIdxMaxCount(int templateId)
    {
        if (_templateIdxInfo.ContainsKey(templateId))
        {
            return _templateIdxInfo[templateId];
        }
        else
        {
            Debug.LogError(string.Format("AniTemplateSC Not Exist this Template! Id:{0}",templateId));
            return -1;
        }
    }
}
