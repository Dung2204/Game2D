//#define TempReturn
#define LoadByAB
using UnityEngine;
using System.Collections.Generic;


public class IconDataPool
{
    private const string IconResourcePath = "UIAltas/IconSprite/{0}";

    //key:packName  key：IconId  Item:Sprite
    private Dictionary<string, Dictionary<int, Sprite>> _spritePackDic;

    private PngAtlasDT[] _atlasData;

    public IconDataPool()
    {
        f_Init();
    }

    private void f_Init()
    {
        _spritePackDic = new Dictionary<string, Dictionary<int, Sprite>>();
        List<NBaseSCDT> tmp = glo_Main.GetInstance().m_SC_Pool.m_PngAtlasSC.f_GetAll();
        if (tmp == null || tmp.Count <= 0)
            _atlasData = new PngAtlasDT[0];
        else
        {
            _atlasData = new PngAtlasDT[tmp.Count];
            for (int i = 0; i < tmp.Count; i++)
            {
                _atlasData[i] = (PngAtlasDT)tmp[i];
            }
        }
    }

    public Sprite f_GetSprieById(int iconId)
    {
#if TempReturn
        return Resources.Load<Sprite>(string.Format(IconResourcePath, "Temp_Icon"));
#endif
        for (int i = 0; i < _atlasData.Length; i++)
        {
            if (iconId >= _atlasData[i].iStartId && iconId <= _atlasData[i].iEndId)
            {
#if LoadByAB
                return GetSpriteByABLoad(iconId, _atlasData[i]);
#else
                return GetSpriteByResourcesLoad(iconId, _atlasData[i]);
#endif
            }
        }
        //MessageBox.ASSERT(string.Format("图标缺失图集数据，IconId：{0}", iconId));
        //return null;
        return GetSpriteByABLoad(999999, (PngAtlasDT)glo_Main.GetInstance().m_SC_Pool.m_PngAtlasSC.f_GetSC(2));
    }

    /// <summary>
    /// 根据ResourcesLoad加载对应sprite
    /// </summary>
    /// <param name="iconId">图标ID</param>
    /// <param name="data">对应图包数据</param>
    /// <returns></returns>
    private Sprite GetSpriteByResourcesLoad(int iconId,PngAtlasDT data)
    {
        if (!_spritePackDic.ContainsKey(data.szFileName))
        {
            Dictionary<int, Sprite> tmpDic = new Dictionary<int, Sprite>();
            Sprite[] tmp = Resources.LoadAll<Sprite>(string.Format(IconResourcePath, data.szFileName));
            if (tmp != null)
            {
                for (int i = 0; i < tmp.Length; i++)
                {
                    tmpDic.Add(int.Parse(tmp[i].name), tmp[i]);
                }
            }
            else
            {
MessageBox.ASSERT(string.Format("No resource found, Id: {0} FileName:{1}",data.iId,data.szFileName));
            }
            _spritePackDic.Add(data.szFileName, tmpDic);
        }
        if (_spritePackDic[data.szFileName].ContainsKey(iconId))
        {
            return _spritePackDic[data.szFileName][iconId];
        }
        else
        {
MessageBox.ASSERT(string.Format("No resources found Icon, IconId:{0} FileName:{1}", iconId, data.szFileName));
        }
        return null;
    }

    /// <summary>
    /// 通过AB加载资源
    /// </summary>
    /// <param name="iconId"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    private Sprite GetSpriteByABLoad(int iconId, PngAtlasDT data)
    {
        return glo_Main.GetInstance().m_ResourceManager.f_CreateIcon(data, iconId);
    }
}
