using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// UserLevelList(玩家等級表)
/// </summary>
public class ResourceSC
{
    Dictionary<string, ResourceDT> _aServerResourceSC = new Dictionary<string, ResourceDT>();
    Dictionary<string, ResourceDT> _aResourceSC = new Dictionary<string, ResourceDT>();
    Dictionary<int, ResourceDT> _aResourceSC_Id = new Dictionary<int, ResourceDT>();

  
    public void f_LoadSCForData(string strBuild)
    {
        DispSaveData(strBuild);
    }

    public int f_GetCount()
    {
        return _aResourceSC.Count;
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    private void DispSaveData(string ppSQL)
    {
        int i;
        ResourceDT DataDT;
        string[] tData;
        string[] tBufData = ppSQL.Split(new string[] { "\r\n" }, System.StringSplitOptions.None);
        for (i = 2; i < tBufData.Length; i++)
        {
            try
            {
                if (tBufData[i] == "")
                {
MessageBox.DEBUG("String with empty record, " + i);
                    continue;
                }
                tData = tBufData[i].Split(new string[] { "," }, System.StringSplitOptions.None);
                int a = 0;
                DataDT = new ResourceDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (!CheckEro.f_CheckId(DataDT.iId))
                {
MessageBox.DEBUG("ResourceDT Error String, Id, " + i);
                    continue;
                }
                //iType iPriority	iIsInResource	strPath
                DataDT.iType = ccMath.atoi(tData[a++]);
                DataDT.iPriority = ccMath.atoi(tData[a++]);
                DataDT.iIsInResource = ccMath.atoi(tData[a++]);
                DataDT.strPath = tData[a++];
                DataDT.Md5 = tData[a++];
                DataDT.m_aDenpendency = ccMath.f_String2ArrayInt(tData[a++], ";");

                _aResourceSC.Add(DataDT.strPath, DataDT);
                _aResourceSC_Id.Add(DataDT.iId, DataDT);
            }
            catch
            {
MessageBox.DEBUG("ResourceDT String record error, " + i);
                continue;
            }
        }
    }

    public ResourceDT f_GetResourceDT(int iId)
    {
        ResourceDT tResourceDT = null;

        _aResourceSC_Id.TryGetValue(iId, out tResourceDT);
        return tResourceDT;
    }

    public ResourceDT f_GetResourceDT(string strFile)
    {
        ResourceDT tResourceDT;
        if (_aResourceSC.TryGetValue(strFile, out tResourceDT))
        {
            return tResourceDT;
        }
        return null;
    }


    public List<ResourceDT> f_GetBaseResource()
    {
        List<ResourceDT> aLoadResource = new List<ResourceDT>();
        foreach (KeyValuePair<string, ResourceDT> fpair in _aResourceSC)
        {
            if (fpair.Value.iPriority == 1)
            {
                aLoadResource.Add(fpair.Value);
            }
        }
        return aLoadResource;
    }

    public bool f_CheckDependency(int iId)
    {
        if (iId <= 0)
        {
MessageBox.ASSERT("f_CheckDependency iId error " + iId);
            return false;
        }

        ResourceDT tResourceDT = f_GetResourceDT(iId);
        if (tResourceDT == null)
        {
MessageBox.ASSERT("No resource found with id " + iId);
        }
        else
        {
            if (tResourceDT.m_Ab != null)
            {
                return true;
            }
        }
        return false;
    }

    public List<ResourceDT> f_GetOtherResource()
    {
        List<ResourceDT> aLoadResource = new List<ResourceDT>();
        foreach (KeyValuePair<string, ResourceDT> fpair in _aResourceSC)
        {
            if (fpair.Value.iPriority > 1)
            {
                aLoadResource.Add(fpair.Value);
            }
        }
        return aLoadResource;
    }

        
    public void f_LoadServerResourceData(string ppSQL)
    {
        int i;
        ResourceDT DataDT;
        string[] tData;
        string[] tBufData = ppSQL.Split(new string[] { "\r\n" }, System.StringSplitOptions.None);
        for (i = 2; i < tBufData.Length; i++)
        {
            try
            {
                if (tBufData[i] == "")
                {
MessageBox.DEBUG("ResourceDT2 String with empty record, " + i);
                    continue;
                }
                tData = tBufData[i].Split(new string[] { "," }, System.StringSplitOptions.None);
                int a = 0;
                DataDT = new ResourceDT();
                DataDT.iId = ccMath.atoi(tData[a++]);
                if (!CheckEro.f_CheckId(DataDT.iId))
                {
MessageBox.DEBUG("ResourceDT 2 string error, Id, " + i);
                    continue;
                }
                //iType iPriority	iIsInResource	strPath
                DataDT.iType = ccMath.atoi(tData[a++]);
                DataDT.iPriority = ccMath.atoi(tData[a++]);
                DataDT.iIsInResource = ccMath.atoi(tData[a++]);
                DataDT.strPath = tData[a++];
                DataDT.Md5 = tData[a++];
                DataDT.m_aDenpendency = ccMath.f_String2ArrayInt(tData[a++], ";");

                _aServerResourceSC.Add(DataDT.strPath, DataDT);
            }
            catch
            {
MessageBox.DEBUG("ResourceDT 2 String record error, " + i);
                continue;
            }
        }
    }

    public List<ResourceDT> f_GetNeedUpdateFileList()
    {
        bool bNeedUpdate = false;
        List<ResourceDT> aLoadResource = new List<ResourceDT>();
        foreach (KeyValuePair<string, ResourceDT> fpair in _aServerResourceSC)
        {
            if (GloData.glo_iPackType == 1)
            {
                bNeedUpdate = CheckResourceNeedUpdateFor1(fpair.Value);
            }
            else
            {
                bNeedUpdate = CheckResourceNeedUpdateFor0(fpair.Value);
            }

            if (!bNeedUpdate)
            {
                continue;       //aLoadResource.Add(fpair.Value);
            }
            else
            {
                aLoadResource.Add(fpair.Value);
            }
        }
        return aLoadResource;
    }

    //public List<ResourceDT> f_GetNeedUpdateFileList(int iPriority)
    //{
    //    ResourceDT tResourceDT;
    //    List<ResourceDT> aLoadResource = new List<ResourceDT>();
    //    foreach (KeyValuePair<string, ResourceDT> fpair in _aServerResourceSC)
    //    {
    //        if (fpair.Value.iPriority == iPriority)
    //        {
    //            tResourceDT = f_GetResourceDT(fpair.Key);            
    //            if (tResourceDT != null)
    //            {
    //                if (!tResourceDT.Md5.Equals(fpair.Value.Md5))
    //                {
    //                    aLoadResource.Add(tResourceDT);
    //                }
    //                else
    //                {
    //                    if (!CheckResourceIsDownLoadSuc(tResourceDT))
    //                    {
    //                        aLoadResource.Add(tResourceDT);
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                aLoadResource.Add(fpair.Value);
    //            }
    //        }
    //    }
    //    return aLoadResource;
    //}

    public void f_UpdateData2Local()
    {
        _aResourceSC.Clear();
        _aResourceSC = _aServerResourceSC;
    }

    public void f_Clear()
    {
        foreach(KeyValuePair<string, ResourceDT> tItem in _aResourceSC)
        {
            if (tItem.Value.iType == 5)
            {
                tItem.Value.m_MulSprite = null;
            }
        }

    }
        
    public void f_SaveDownLoadSucResource(ResourceDT tResourceDT)
    {
        PlayerPrefs.SetString(tResourceDT.strPath, tResourceDT.Md5);
    }

    private bool CheckResourceNeedUpdateFor0(ResourceDT tResourceDT)
    {
        string strMd5 = PlayerPrefs.GetString(tResourceDT.strPath);
        if (strMd5 == tResourceDT.Md5)
        {
            if (!File.Exists(Application.persistentDataPath + "/" + GloData.glo_ProName + "/" + tResourceDT.strPath))
            {
                return true;
            }
            return false;
        }       
        return true;
    }

    private bool CheckResourceNeedUpdateFor1(ResourceDT tResourceDT)
    {
        ResourceDT tLocalResourceDT;
        string strMd5 = PlayerPrefs.GetString(tResourceDT.strPath);
        if (string.IsNullOrEmpty(strMd5))
        {
            //本地没有更新记录
            tLocalResourceDT = f_GetResourceDT(tResourceDT.strPath);
            if (tLocalResourceDT == null)
            {
                return true;
            }
            if (tLocalResourceDT.Md5.Equals(tResourceDT.Md5))
            {
                return false;
            }
            return true;
        }
        if (strMd5 == tResourceDT.Md5)
        {
            if (!File.Exists(Application.persistentDataPath + "/" + GloData.glo_ProName + "/" + tResourceDT.strPath))
            {
                return true;
            }
            return false;
        }
        return true;
    }
    

}
