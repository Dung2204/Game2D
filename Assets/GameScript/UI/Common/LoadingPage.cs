using UnityEngine;
using System.Collections;
using ccU3DEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// 加载场景页面UI
/// </summary>
public class LoadingPage : UIFramwork
{
    public static bool isStartGame = false;//是否收到服务器进入游戏通知(登陆游戏时才用)
    public UILabel m_labelLoading;//加载显示标签
    public UISlider m_sliderProgress;//加载进度
    private float loadSceneProgress = 0f;//场景加载进度+
    AsyncOperation asyncOperation = null;//异步操作
    private float buffersValue = 0.05f;//进度条缓冲值
    private EM_Scene loadScene;
	//My Code
	GameParamDT AssetOpen;
	GameObject TexBgKD;
	//
    /// <summary>
    /// 进入游戏通知
    /// </summary>
    public static void OnEnterGameHandle(object obj)
    {
        isStartGame = true;
        Data_Pool.m_UserData.InitDataDefault();
    }

    private UITexture Tex_Bg1 = null;
    protected override void f_CustomAwake()
    {
        base.f_CustomAwake();
        Tex_Bg1 = f_GetObject("Tex_Bg1").GetComponent<UITexture>();
    }

    /// <summary>
    /// 打开界面
    /// </summary>
    /// <param name="e">bool 是否开始加载场景</param>
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        loadScene = (EM_Scene)e;
		StaticValue.m_preScene = StaticValue.m_curScene;
        StaticValue.m_curScene = loadScene;
		//My Code
		AssetOpen = (glo_Main.GetInstance().m_SC_Pool.m_GameParamSC.f_GetSC(93) as GameParamDT);
		//
		MessageBox.ASSERT("Scene: " + StaticValue.m_preScene + StaticValue.m_curScene);
        //随机背景图
        int randomBgIndex = Random.Range(2, 8);
        // if (StaticValue.m_preScene == EM_Scene.Login)
		// {
            // randomBgIndex = Random.Range(0, 2);
		// }
        f_GetObject("Tex_Bg1").SetActive(true);//(randomBgIndex == 0);
        f_GetObject("Tex_Bg2").SetActive(false);//(randomBgIndex == 1);
        f_GetObject("Tex_Bg3").SetActive(false); //(randomBgIndex == 2);
		//My Code
		TexBgKD = GameObject.Find("TexBgKD");
		if(TexBgKD != null && AssetOpen != null)
		{
			if(AssetOpen.iParam2 == 0)
			{
				TexBgKD.SetActive(false);
			}
		}
		//
		Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("UI/TextureRemove/ShowLogoBG");
		// My Code
		if(AssetOpen.iParam1 == 1)
		{
			tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(texArr[randomBgIndex]);
		}
		if(tTexture2D == null)
			tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("UI/TextureRemove/MainMenu/Tex_TempLoading");
		
        //Tex_Bg1.mainTexture = tTexture2D;

        if (StaticValue.m_isPlayMusic)
        {
            //glo_Main.GetInstance().m_AdudioManager.f_PlayAudioMusic(AudioMusicType.Loading, true);
            //glo_Main.GetInstance().m_AdudioManager.f_PauseAudioMusic();
        }
        if (StaticValue.m_isPlaySound)
        {
            glo_Main.GetInstance().m_AdudioManager.f_PauseAudioButtle();
        }
        startLoadingScene();
        f_GetObject("TextTips").GetComponent<UILabel>().text = getTips();
        //f_LoadTexture();
        Data_Pool.m_ChatPool.lRecord.Clear();
        
    }
    // private string strTexBgRoot1 = "UI/TextureRemove/Login/Tex_LoadingBg";
    // private string strTexBgRoot3 = "UI/TextureRemove/Login/Tex_LoadingBg2";
    // private string strTexBgRoot4 = "UI/TextureRemove/Login/Tex_LoadingBg3";
    // private string strTexBgRoot2 = "UI/TextureRemove/Login/Tex_ResProgressBg";
	private string strTexBgRoot1 = "Tex_LoadingBg";
    private string strTexBgRoot3 = "Tex_LoadingBg2";
    private string strTexBgRoot4 = "Tex_LoadingBg3";
    private string strTexBgRoot2 = "Tex_ResProgressBg";

    private string[] texArr = new[]
    {
		// "UI/TextureRemove/Login/Tex_Base1",
		// "UI/TextureRemove/Login/Tex_LoginBg",
        // "UI/TextureRemove/Login/Tex_LoadingBg",
        // "UI/TextureRemove/Login/Tex_ResProgressBg",
        // "UI/TextureRemove/Login/Tex_LoadingBg2",
        // "UI/TextureRemove/Login/Tex_LoadingBg3",
		// "UI/TextureRemove/Login/Tex_LoadingBg4",
		// "UI/TextureRemove/Login/Tex_LoadingBg5",
		// "UI/TextureRemove/Login/Tex_LoadingBg6",
		// "UI/TextureRemove/Login/Tex_LoadingBg7",
		"Tex_Base1",
		"Tex_LoginBg",
        "Tex_LoadingBg",
        "Tex_LoadingBg2",
        "Tex_LoadingBg3",
		"Tex_LoadingBg4",
		"Tex_LoadingBg5",
		"Tex_LoadingBg6",
    };
    /// <summary>
    /// 加载texture
    /// </summary>
    private void f_LoadTexture()
    {
        //加载背景图
        UITexture Tex_Bg1 = f_GetObject("Tex_Bg1").GetComponent<UITexture>();
        if (Tex_Bg1.mainTexture == null)
        {
            Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBgRoot1);
			if(tTexture2D == null)
			{
				tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D("UI/TextureRemove/MainMenu/Tex_TempLoading");
			}
            Tex_Bg1.mainTexture = tTexture2D;
        }
        UITexture Tex_Bg2 = f_GetObject("Tex_Bg2").GetComponent<UITexture>();
        if (Tex_Bg2.mainTexture == null)
        {
            Texture2D tTex_Bg2 = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBgRoot2);
            Tex_Bg2.mainTexture = tTex_Bg2;
        }
        UITexture Tex_Bg3 = f_GetObject("Tex_Bg3").GetComponent<UITexture>();
        if (Tex_Bg3.mainTexture == null)
        {
            Texture2D tTex_Bg3 = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBgRoot3);
            Tex_Bg3.mainTexture = tTex_Bg3;
        }
    }
    private void startLoadingScene()
    {
m_labelLoading.text = "Đang tải...0%";
        m_sliderProgress.value = 0f;
        string SceneName = GetSceneName(loadScene);
        glo_Main.GetInstance().m_ResourceManager.Clear3DModleDT();
        StartCoroutine(LoadScene(SceneName));
    }
    /// <summary>
    /// 通过全局场景枚举获取场景名字
    /// </summary>
    /// <param name="em_scene">场景枚举类别</param>
    /// <returns></returns>
    private string GetSceneName(EM_Scene em_scene)
    {
        string loadscenename = "LoginScene";
        switch (em_scene)
        {
            case EM_Scene.Login:
                loadscenename = "LoginScene";
                break;
            case EM_Scene.GameMain:
                loadscenename = "GameMain";
                break;
            case EM_Scene.BattleMain:
                loadscenename = "BattleScene";
                break;
        }
        return loadscenename;
    }

    /// <summary>
    /// 异步加载场景
    /// </summary>
    /// <param name="sceneName">需要加载的场景名字</param>
    /// <returns></returns>
    private IEnumerator LoadScene(string sceneName)
    {
        yield return null;
        glo_Main.GetInstance().asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation = glo_Main.GetInstance().asyncOperation;
        asyncOperation.allowSceneActivation = false;
    }
    /// <summary>
    /// 更新场景加载进度
    /// </summary>
    protected override void f_Update()
    {
        if (asyncOperation != null)
        {
            this.loadSceneProgress = asyncOperation.progress;
            if (!isStartGame)
                buffersValue = 0.01f;
            else
                buffersValue = 0.05f;

            if (loadSceneProgress >= 0.88f)
            {
                loadSceneProgress = 1;
            }
            m_sliderProgress.value = Mathf.Lerp(m_sliderProgress.value, loadSceneProgress, buffersValue);
            m_labelLoading.text = string.Format("Đang tải...{0}%", (int)(m_sliderProgress.value * 100));
            if (m_sliderProgress.value >= 0.97f)
            {
                m_sliderProgress.value = m_sliderProgress.value = Mathf.Lerp(m_sliderProgress.value, 1, buffersValue);
                if (isStartGame)
                {
                    if (loadScene == EM_Scene.GameMain && ForceResLoadPage.isOpen)//需要加载资源
                    {

                    }
                    else
                    {
                        m_sliderProgress.value = 1f;
                        //允许进入场景
                        asyncOperation.allowSceneActivation = true;
                        glo_Main.GetInstance().f_ChangeScene();
                        m_labelLoading.text = "Đang tải...100%";
						glo_Main.GetInstance().m_AdudioManager.f_PauseAudioMusic();
                    }
                }
            }
        }
    }

    #region 获取tips提示
    private string getTips()
    {
        int randomNum = Random.Range(1, 25);
        string msg = "";
        switch (randomNum)
        {
            case 1:
                msg = CommonTools.f_GetTransLanguage(2061); break;
            case 2:
                msg = CommonTools.f_GetTransLanguage(2062); break;
            case 3:
                msg = CommonTools.f_GetTransLanguage(2063); break;
            case 4:
                msg = CommonTools.f_GetTransLanguage(2064); break;
            case 5:
                msg = CommonTools.f_GetTransLanguage(2065); break;
            case 6:
                msg = CommonTools.f_GetTransLanguage(2066); break;
            case 7:
                msg = CommonTools.f_GetTransLanguage(2067); break;
            case 8:
                msg = CommonTools.f_GetTransLanguage(2068); break;
            case 9:
                msg = CommonTools.f_GetTransLanguage(2069); break;
            case 10:
                msg = CommonTools.f_GetTransLanguage(2070); break;
            case 11:
                msg = CommonTools.f_GetTransLanguage(2071); break;
            case 12:
                msg = CommonTools.f_GetTransLanguage(2072); break;
            case 13:
                msg = CommonTools.f_GetTransLanguage(2073); break;
            case 14:
                msg = CommonTools.f_GetTransLanguage(2074); break;
            case 15:
                msg = CommonTools.f_GetTransLanguage(2075); break;
            case 16:
                msg = CommonTools.f_GetTransLanguage(2076); break;
            case 17:
                msg = CommonTools.f_GetTransLanguage(2077); break;
            case 18:
                msg = CommonTools.f_GetTransLanguage(2078); break;
            case 19:
                msg = CommonTools.f_GetTransLanguage(2079); break;
            case 20:
                msg = CommonTools.f_GetTransLanguage(2080); break;
            case 21:
                msg = CommonTools.f_GetTransLanguage(2081); break;
            case 22:
                msg = CommonTools.f_GetTransLanguage(2082); break;
            case 23:
                msg = CommonTools.f_GetTransLanguage(2083); break;
            case 24:
                msg = CommonTools.f_GetTransLanguage(2084); break;
        }
        return msg;
    }
    #endregion
}
