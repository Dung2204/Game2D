using UnityEngine;
using System.Collections;
using ccU3DEngine;
/// <summary>
/// 加载资源页面UI
/// </summary>
public class ResLoadPage : UIFramwork
{
    //是否需要下载资源
    bool isNeedDownLoadRes = false;
    private static bool isLoadResources = false;//是否已经加载完配置表
    bool isChangePage;//当前已切换页面
    bool isShowNotice = false;
	//My Code
	GameParamDT AssetOpen;
	UITexture TexBg;
	GameObject TexBgKD;
	Texture2D tTexture2D;
	//
    //public TrailerMovie trailer;
    /// <summary>
    /// 设置资源已经加载完成
    /// </summary>
    public static void OnSetConfigSeted(object Obj)
    {
        isLoadResources = true;
    }
    /// <summary>
    /// 打开界面
    /// </summary>
    protected override void UI_OPEN(object e)
    {
        _NeedCloseSound = false;
        base.UI_OPEN(e);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ShowLogoPage, UIMessageDef.UI_CLOSE);
        isNeedDownLoadRes = (bool)e;
        isChangePage = false;
		//My Code
		AssetOpen = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(93) as GameParamDT);
		TexBg = GameObject.Find("Tex_Bg").GetComponent<UITexture>();
		tTexture2D = null;
		if(AssetOpen != null)
		{
			if(AssetOpen.iParam1 == 1)
			{
				if(TexBg != null)
					tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("Tex_LogoLoginBg");
			}
			TexBg.mainTexture = tTexture2D;
			TexBgKD = f_GetObject("TexBgKD");
			if(TexBgKD != null)
			{
				MessageBox.ASSERT("AssetOpen.iParam2: " + AssetOpen.iParam2);
				if(AssetOpen.iParam2 == 0)
				{
					TexBgKD.SetActive(false);
				}
				else
				{
					TexBgKD.SetActive(false);
				}
			}
			if (TexBg.mainTexture == null)
			{
				tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("UI/TextureRemove/ShowLogoBG");
				TexBg.mainTexture = tTexture2D;
			}
			if(AssetOpen.iParam3 == 1)
			{
				f_GetObject("Slider").transform.localScale = new Vector3 (1f, 1f, 1f);
				f_GetObject("TxtLoading").transform.localScale = new Vector3 (1f, 1f, 1f);
			}
		}
		// if (TexBg.mainTexture == null)
		// {
			// tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("UI/TextureRemove/MainMenu/Tex_TempLoading");
			// TexBg.mainTexture = tTexture2D;
		// }
		//
        if (isNeedDownLoadRes)
        {
            string strVer = PlayerPrefs.GetString("Ver");
//            MessageBox.DEBUG("Du lieu luu tru",strVer);
            /*if (strVer == "201908092343VKgNTNkX222ABfyUsdftfJT")
            {
                trailer.gameObject.SetActive(true);
            }*/
            glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_RESOURCELOADPROGRESS, On_ResProgressEvent, this);
        }
        else
        {
            f_GetObject("TxtLoading").GetComponent<UILabel>().text = "Đang tải...";
            f_GetObject("Slider").GetComponent<UISlider>().value = 0;
        }
        if (!isShowNotice)
        {
            //检测有没有公告
            isShowNotice = true;
            StartCoroutine(StartCheckNotice());
        }
    }
    protected override void On_Destory()
    {
        base.On_Destory();
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_RESOURCELOADPROGRESS, On_ResProgressEvent, this);
    }
    /// <summary>
    /// 检测公告消息
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartCheckNotice()
    {
        //string ppSQL = GloData.glo_strNotice + "?channel=" + glo_Main.GetInstance().m_SDKCmponent.f_GetSdkChannelType() + "&version=" + Application.version;
        WWW www = new WWW(ResourceTools.f_CreateNoticeUrl());
        yield return www;
        if (www.error != null)
        {
            yield return null;
        }
        if (www.isDone && www.error == null)
        {
            if (www.text != null && www.text != "")
            {
                ccUIManage.GetInstance().f_SendMsg(UINameConst.NoticePanel, UIMessageDef.UI_OPEN, www.text);
            }
        }
    }
    /// <summary>
    /// 更新检测
    /// </summary>
    protected override void f_Update()
    {
        base.f_Update();
        if (isChangePage)
            return;
        if (!isNeedDownLoadRes)
        {
            float value = f_GetObject("Slider").GetComponent<UISlider>().value;
            if (!isLoadResources)
            {
                value = Mathf.Lerp(value, 0.8f, 0.05f);
            }
            else
            {
                value = Mathf.Lerp(value, 1.2f, 0.05f);
            }
            f_GetObject("Slider").GetComponent<UISlider>().value = value;

            if (f_GetObject("Slider").GetComponent<UISlider>().value >= 1 && isLoadResources)
                ChangePage();
        }
        else
        {
            if (isLoadResources)
                ChangePage();
        }
    }
    /// <summary>
    /// 切换页面
    /// </summary>
    private void ChangePage()
    {
        isChangePage = true;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.ResLoadPage, UIMessageDef.UI_CLOSE);
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LoginPage, UIMessageDef.UI_OPEN);
    }
    /// <summary>
    /// 显示资源下载进度
    /// </summary>
    /// <param name="progress">进度：（iNum：iMaxNum）iNum:资源剩余数量 iMaxNum:资源总数量</param>
    private void On_ResProgressEvent(object progress)
    {
        string[] Strprogress = ((string)progress).Split(';');
        int _iNum = int.Parse(Strprogress[0]);
        int _iMaxNum = int.Parse(Strprogress[1]);
        //f_GetObject("TxtLoading").GetComponent<UILabel>().text = "正在更新资源...（" + (_iMaxNum - _iNum) + "/" + _iMaxNum + "）";
        //f_GetObject("TxtLoading").GetComponent<UILabel>().text = "正在为您解压资源包（过程不消耗流量 " + (_iMaxNum - _iNum) + "/" + _iMaxNum + "）"; 
	f_GetObject("TxtLoading").GetComponent<UILabel>().text = "Đang tải(" + (_iMaxNum - _iNum) + "/" + _iMaxNum + "）"; 
        f_GetObject("Slider").GetComponent<UISlider>().value = (_iMaxNum - _iNum) * 1.0f / _iMaxNum;
    }
}
