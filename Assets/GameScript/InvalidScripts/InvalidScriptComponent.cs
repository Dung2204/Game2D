public class InvalidScriptComponent
{
#if InvalidScripts01
    private InvalidScriptEntry01 m_Entry01;
#endif

#if InvalidScripts02
    private InvalidScriptEntry02 m_Entry02;
#endif

#if InvalidScripts03
    private InvalidScriptEntry03 m_Entry03;
#endif

#if InvalidScripts04
    private InvalidScriptEntry04 m_Entry04;
#endif

    public InvalidScriptComponent()
    {
#if InvalidScripts01
        m_Entry01 = new InvalidScriptEntry01();
#endif

#if InvalidScripts02
        m_Entry02 = new InvalidScriptEntry02();
#endif

#if InvalidScripts03
        m_Entry03 = new InvalidScriptEntry03();
#endif

#if InvalidScripts04
        m_Entry04 = new InvalidScriptEntry04();
#endif

    }

    public void Init()
    {
#if InvalidScripts01
        m_Entry01.Init();
#endif

#if InvalidScripts02
        m_Entry02.Init();
#endif

#if InvalidScripts03
        m_Entry03.Init();
#endif

#if InvalidScripts04
        m_Entry04.Init();
#endif
    }

    public void Update()
    {
#if InvalidScripts01
        m_Entry01.Update();
#endif

#if InvalidScripts02
        m_Entry02.Update();
#endif

#if InvalidScripts03
        m_Entry03.Update();
#endif

#if InvalidScripts04
        m_Entry04.Update();
#endif
    }
}
