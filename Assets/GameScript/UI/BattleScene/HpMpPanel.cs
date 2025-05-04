using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ccU3DEngine;

public class HpMpPanel : MonoBehaviour
{
    public UISlider m_HPPanel;
    public GameObject[] m_aMPObj;
    public GameObject[] m_aHPObj;

    public UILabel m_MPText;
    public UIPanel m_UIPanel;

    public TextControl m_HpText;
    public TextControl m_CritrcalNum;
    public TextControl m_Critrcal;
    public TextControl m_CureNum;
    public TextControl m_Cure;
    public TextControl m_FiringNum;
    public TextControl m_Firing;
    public TextControl m_PosionNum;
    public TextControl m_Posion;

    public TextControl m_Anger;
    public TextControl m_AngerNum;

    public UILabel m_EvoLevelText;

    public TweenScale m_HPAlph;
    private bool m_IsLastTurn;
    public UISprite m_IconFightType;
    private List<TextControl> normalHpTextControlCacheList;
    private List<TextControl> criticalHpTextControlCacheList;
    private List<TextControl> cureHpTextControlCacheList;
    private string hpStr = "HP1";

    private int __iMp;
    private int _iMp
    {
        set
        {
            __iMp = value;
            UpdateMP();
        }
    }

    public CardCashSkill _CardCashSkill = null;
    public Transform _ListIconBuff;
    public BuffIcon BuffIconItem;

    private void Awake()
    {
        m_IsLastTurn = false;
        normalHpTextControlCacheList = new List<TextControl>();
        normalHpTextControlCacheList.Add(m_HpText);
        criticalHpTextControlCacheList = new List<TextControl>();
        criticalHpTextControlCacheList.Add(m_CritrcalNum);
        cureHpTextControlCacheList = new List<TextControl>();
        cureHpTextControlCacheList.Add(m_CureNum);
        glo_Main.GetInstance().m_UIMessagePool.f_AddListener(UIMessageDef.UI_SHOWLASTTURN, On_ShowLastTurn);

        _CardCashSkill = glo_Main.GetInstance().m_ResourceManager.f_CreateCardCashSkill();
       
    }
  
    //显示最后一轮战斗（跳过战斗）
    private void On_ShowLastTurn(object obj) {
        m_IsLastTurn = true;
    }

    public void f_Reset()
    {
        m_HPPanel.value = 0;
        m_MPText.text = "";
        m_IsLastTurn = false;
    }

    //清楚飘字表现（前置buff飘血，然后模型”移动“的时候消失再显示会激活断掉的飘字，造成连飘两次的错觉）
    public void ClearFlyTxt() {
        for (int i = 0; i < normalHpTextControlCacheList.Count; i++)
        {
            normalHpTextControlCacheList[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < criticalHpTextControlCacheList.Count; i++)
        {
            criticalHpTextControlCacheList[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < cureHpTextControlCacheList.Count; i++)
        {
            cureHpTextControlCacheList[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 从缓存里获取一个飘字control
    /// </summary>
    /// <param name="textControlList">飘字控件缓存数组</param>
    /// <returns></returns>
    public TextControl f_GetTextControlByCache(List<TextControl> textControlList)
    {
        //先从缓存中获取
        for (int i = 0; i < textControlList.Count; i++)
        {
            if (!textControlList[i].IsPlay)
            {
                textControlList[i].IsPlay = true;
                return textControlList[i];
            }
        }

        //不够则新增一个
        GameObject hpObj = NGUITools.AddChild(gameObject, textControlList[0].gameObject);
        TextControl hpTextControl = hpObj.GetComponent<TextControl>();
        hpTextControl.IsPlay = true;
        textControlList.Add(hpTextControl);
        return hpTextControl;
    }
    public void f_InitImageHp(EM_Factions m_EM_Factions)
    {
        if(m_EM_Factions == EM_Factions.ePlayer_A)
        {
            hpStr = "HP1";
        }
        else
        {
            hpStr = "HP2";
        }
        for (int i = 0; i < m_aHPObj.Length; i++)
        {
            m_aHPObj[i].transform.Find("Exbg").transform.Find("ExFg").GetComponent<UISprite>().spriteName = hpStr;
        }
    }
    public void f_InitIconFightType(int fightType)
    {
        m_IconFightType.spriteName = "IconType_" + fightType;
    }
    public void f_InitHp(float fHpPerCent)
    {
        if (!gameObject.activeInHierarchy)
        {
            //剧情系统隐藏的怪物表现上不扣血，避免一出来就残血
            return;
        }
        m_HPPanel.value = fHpPerCent;

        _CardCashSkill.f_InitHp(fHpPerCent);
    }
    private long lastTime = 0;
        public void f_LostHp(int iHp, float fHpPerCent, int iCriticalHit, bool bUnShowText = false,int harmNum = 0)
        {
            if (!gameObject.activeInHierarchy) {
                //剧情系统隐藏的怪物表现上不扣血，避免一出来就残血
                return;
            }
            if (iCriticalHit != 3)
            {
                m_HPPanel.value = fHpPerCent;
                _CardCashSkill.f_InitHp(fHpPerCent);
                f_SetHPAlph(fHpPerCent);
            }
            if (!bUnShowText)
            {
                if (iCriticalHit == (int)EM_HpChangeType.eHpCritical)
                {//暴击
                    TextControl hpText = f_GetTextControlByCache(criticalHpTextControlCacheList);
                    if (null == hpText)
                        return;
                    hpText.HarmNum = harmNum;
                    hpText.f_Show(iHp.ToString());
                    glo_Main.GetInstance().m_AdudioManager.f_PlayAudioEffect(UITool.GetEnumName(typeof(AudioEffectType), AudioEffectType.Crit));
                    m_Critrcal.f_Show("B");

                }
                else if (iCriticalHit == (int)EM_HpChangeType.eHpDodge)
                {//闪避
                    m_HpText.f_Show("Miss");
                    glo_Main.GetInstance().m_AdudioManager.f_PlayAudioEffect(UITool.GetEnumName(typeof(AudioEffectType), AudioEffectType.miss));
            }
                else if (iCriticalHit == (int)EM_HpChangeType.eHpFire)
                {//灼烧
                    m_FiringNum.f_Show(iHp.ToString());
                    m_Firing.f_Show("Z");
                }
                else if (iCriticalHit == (int)EM_HpChangeType.eHpPoison)
                {//中毒
                    m_PosionNum.f_Show(iHp.ToString());
                    m_Posion.f_Show("Z");
                }
                else if (iCriticalHit == (int)EM_HpChangeType.eHpPride)
                {//Kiêu hãnh new // cần xử lý đa ngôn ngữ
                    m_HpText.f_Show("Kiêu Hãnh");
                }
                else if (iCriticalHit == (int)EM_HpChangeType.eHpPersist)
                {//Kiên trì new
                    string str = "";
                    if (iHp != 0)
                        str = iHp.ToString();
                    m_HpText.f_Show("Kiên Trì " + str);
                }
                else if (iCriticalHit == (int)EM_HpChangeType.eHpIndomitable)
                {//Bất khuất new
                    string str = "";
                    if (iHp != 0)
                        str = iHp.ToString();
                    m_HpText.f_Show("Bất Khuất "+ str);
                }
                else
                {
                    if (iHp == 0)
                        return;

                    TextControl hpText = f_GetTextControlByCache(normalHpTextControlCacheList);
                    if (null == hpText)
                        return;
                    hpText.HarmNum = harmNum;
                    hpText.f_Show(iHp.ToString());
                    //float delay = lastTime - GameSocket.GetInstance().f_GetServerTime();
                    //if (delay <= 0)
                    //{
                    //    lastTime = GameSocket.GetInstance().f_GetServerTime() + 1;
                    //    hpText.HarmNum = harmNum;
                    //    hpText.f_Show(iHp.ToString());
                    //}
                    //else
                    //{
                    //    ccTimeEvent.GetInstance().f_RegEvent(delay/3, false, null, (a) => {
                    //        hpText.HarmNum = harmNum;
                    //        hpText.f_Show(iHp.ToString());
                    //    });
                    //    lastTime += 1;
                    //}


                }
            }
        }

    public void f_Dizzy() {
        m_FiringNum.f_Show("X");
    }

    public void f_AddHp(int iHp, float fHpPerCent, bool bUnShowText = false,int harmNum = 0)
    {
        if (iHp == 0)
        {
            return;
        }

        MessageBox.DEBUG("f_AddHp " + iHp);
        f_SetHPAlph(fHpPerCent);
        if (!bUnShowText)
        {
            //_AddBettle(m_AddHpText.f_Show, iHp.ToString());
            
            TextControl hpText = f_GetTextControlByCache(cureHpTextControlCacheList);
            if (null == hpText)
                return;
            hpText.HarmNum = harmNum;
            hpText.f_Show("+" + iHp);
            m_Cure.f_Show("Z");
        }
    }

    public void f_Mp(int iDefaultMp, int iChangMp, EM_BattleMpType tEM_BattleMpType)
    {
        if (tEM_BattleMpType == EM_BattleMpType.Add)
        {
            m_Anger.f_Show("");
            m_AngerNum.f_Show("+" + iChangMp);
            m_Anger.f_Show("");
MessageBox.DEBUG("Rage" + iChangMp);
        }
        else if (tEM_BattleMpType == EM_BattleMpType.Lost)
        {
            m_Anger.f_Show("");
            m_AngerNum.f_Show("-" + iChangMp);
            m_Anger.f_Show("");
MessageBox.DEBUG("Reduce fury" + iChangMp);
        }
        _iMp = iDefaultMp;
        m_MPText.gameObject.SetActive(iDefaultMp > 6);
        m_MPText.text = "X" + iDefaultMp;

        _CardCashSkill.f_Mp(iDefaultMp, iChangMp, tEM_BattleMpType);
    }

    public int f_GetMp()
    {
        return __iMp;
    }

    public void f_SetDepthForAttack(int iDepth)
    {
        m_UIPanel.sortingOrder = (int)EM_BattleDepth.AttackHP + iDepth;
    }

    public void f_SetDepthForBeAttack(int iDepth)
    {
        m_UIPanel.sortingOrder = (int)EM_BattleDepth.BeAttackHP + iDepth;
    }

    public void f_SetDepthForOther(int iDepth)
    {
        m_UIPanel.sortingOrder = (int)EM_BattleDepth.OtherHP + iDepth;
    }
    /// <summary>
    /// 设置进阶等级
    /// </summary>
    /// <param name="iEvoLevel"></param>
    public void f_SetEvoLevel(int iEvoLevel)
    {
        m_EvoLevelText.text = iEvoLevel != 0 ? iEvoLevel.ToString() : "";
    }
    /// <summary>
    /// 设置消失的血量
    /// </summary>
    /// <param name="fLostHp"></param>
    public void f_SetHPAlph(float fLostHp)
    {       
        if (fLostHp > 1f)
            fLostHp = 1f;
        else if (fLostHp < 0f)
            fLostHp = 0f;
        UpdateHP(fLostHp);
        if (m_IsLastTurn) {
            m_HPPanel.value = fLostHp;
            m_HPAlph.transform.localScale = new Vector3(fLostHp, 1, 1);
            return;
        }

        m_HPAlph.from = m_HPAlph.transform.localScale;
        m_HPAlph.to = new Vector3(fLostHp, 1, 1);
        m_HPAlph.ResetToBeginning();
        m_HPAlph.PlayForward();


        if (m_HPAlph.from.y - m_HPAlph.from.y < 0)
            EventDelegate.Add(m_HPAlph.onFinished, _SetHpValue);
        else
            m_HPAlph.onFinished.Clear();
    }

    private void _SetHpValue()
    {
        m_HPPanel.value = m_HPAlph.to.y;
    }
    private void UpdateMP()
    {
        int iA = __iMp;
        if (iA > 6)
        {
            iA = 6;
        }
        for (int i = 0; i < iA; i++)
        {
            m_aMPObj[i].SetActive(true);
        }
        for (int i = iA; i < m_aMPObj.Length; i++)
        {
            m_aMPObj[i].SetActive(false);
        }
    }

    private void UpdateHP(float fLostHp)
    {
        float fhp = fLostHp * 100;
        int iA = (int)Mathf.Ceil(fhp / 10f);
        int iB = (int)(fhp % 10 * 10);
        if (iA > 10)
        {
            iA = 10;
        }
        for (int i = 0; i < iA; i++)
        {
            m_aHPObj[i].SetActive(true);
            m_aHPObj[i].GetComponent<UISlider>().value = 1f;//transform.localScale = new Vector3(1, 1, 1);
        }
        for (int i = iA; i < m_aHPObj.Length; i++)
        {
            m_aHPObj[i].SetActive(false);
            m_aHPObj[i].GetComponent<UISlider>().value = 0f;
        }
        //if(iB > 0)
        //{
        //    m_aHPObj[iA].SetActive(true);
        //    m_aHPObj[iA].transform.localScale = new Vector3(iB/100f, 1, 1);
        //    m_aHPObj[iA].GetComponent<UISlider>().value = iB / 100f;
        //}
        
    }

    private List<GameObject> listBuffIcon = new List<GameObject>();
    public void f_UpdateListBuffIcon(List<stBeAttackInfor> list_stBeAttackInfors)
    {
        _CardCashSkill.f_UpdateListBuffIcon(list_stBeAttackInfors);
        f_ResetListBuffIcon();
        for (int i = 0; i < list_stBeAttackInfors.Count; i++)
        {
            GameObject go = f_GetBuffIconGO();
            if (go == null)
            {
                go = Instantiate(BuffIconItem.gameObject);
                go.SetActive(true);
                go.GetComponent<BuffIcon>().SetUI(list_stBeAttackInfors[i].m_iBuf1, list_stBeAttackInfors[i].m_iAnger);
                listBuffIcon.Add(go);
                go.transform.parent = _ListIconBuff;
                go.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                go.SetActive(true);
                go.GetComponent<BuffIcon>().SetUI(list_stBeAttackInfors[i].m_iBuf1, list_stBeAttackInfors[i].m_iAnger);
            }
        }
        _ListIconBuff.gameObject.GetComponent<UIGrid>().enabled = true;
    }

    private void f_ResetListBuffIcon()
    {
        for (int i = 0; i < listBuffIcon.Count; i++)
        {
            listBuffIcon[i].SetActive(false);
        }
    }

    private GameObject f_GetBuffIconGO()
    {
        GameObject go = null;
        go = listBuffIcon.Find(GO => GO.activeSelf == false);
        return go;
    }
}


public class TextDelayParm
{
    public ccCallBack_TextDelay TextDelay;
    public string szText;
    public GameObject go;
    public TextDelayParm(ccCallBack_TextDelay aaaaa, string szText, GameObject go)
    {
        TextDelay = aaaaa;
        this.szText = szText;
        this.go = go;
    }

}
