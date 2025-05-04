using UnityEngine;
using System.Collections;
using ccU3DEngine;
/// <summary>
/// 强制加载资源页面UI
/// </summary>
public class ForceResLoadPage : UIFramwork
{
    public static bool isOpen = false;
	//My Code
	GameParamDT AssetOpen;
	UITexture tTexture2D;
	GameObject TexBgKD;
	//
    protected override void UI_CLOSE(object e)
    {
        base.UI_CLOSE(e);
        isOpen = false;
    }
    /// <summary>
    /// 打开界面
    /// </summary>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        isOpen = true;
		//My Code
		AssetOpen = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(93) as GameParamDT);
		tTexture2D = GameObject.Find("Tex_Bg").GetComponent<UITexture>();
		if(AssetOpen != null)
		{
			if(AssetOpen.iParam1 == 1)
			{
				if(tTexture2D != null)
					tTexture2D.mainTexture = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("Tex_LogoLoginBg");
			}
			TexBgKD = GameObject.Find("TexBgKD");
			if(TexBgKD != null)
			{
				if(AssetOpen.iParam2 == 0)
				{
					TexBgKD.SetActive(false);
				}
			}
		}
		//
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_RESOURCELOADPROGRESS, On_ResProgressEvent, this);
    }
    protected override void On_Destory()
    {
        base.On_Destory();
        glo_Main.GetInstance().m_UIMessagePool.f_RemoveListener(UIMessageDef.UI_RESOURCELOADPROGRESS, On_ResProgressEvent, this);
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
        //f_GetObject("TxtLoading").GetComponent<UILabel>().text = "正在解压资源...（" + (_iMaxNum - _iNum) + "/" + _iMaxNum + "）";
f_GetObject("TxtLoading").GetComponent<UILabel>().text = "Loading（" + (_iMaxNum - _iNum) + "/" + _iMaxNum + "）";
        f_GetObject("Slider").GetComponent<UISlider>().value = (_iMaxNum - _iNum) * 1.0f / _iMaxNum;
    }
}
