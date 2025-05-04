
using ccU3DEngine;
using UnityEngine;
using System.Text;
using System.Collections.Generic;

public class EmojiExtension : UIFramwork
{
    private List<GameObject> mItems = new List<GameObject>();

    public UIAtlas atlas;
    private BetterList<string> mListOfSprites;
    private GameObject mLabelTemplate;
    public Transform mWorldParent;
    public Transform mSystemParent;
    public Transform mGroupParent;
    public Transform mPrivateParent;

    public GameObject mMyselfChat;

    private int mWidgetDepth;
    private int mFontSize;
    private float mLineWidth;
    private float mLineHeight;
    private float mSpacingX;
    private float mSpacingY;

    private float mCurrentHeight = 0.0f;

    public bool mIsFirst;
    private bool mIsInten = false;
    public void Awake()
    {
        UILabel uiLabel = transform.Find("Chat_Label").GetComponent<UILabel>();

        mWidgetDepth = uiLabel.depth + 1;
        mFontSize = uiLabel.fontSize;
        mLineWidth = uiLabel.width;
        mLineHeight = uiLabel.fontSize + uiLabel.effectiveSpacingY;
        mSpacingX = uiLabel.effectiveSpacingX;
        mSpacingY = uiLabel.effectiveSpacingY;
    }

    void Start()
    {
        if (mIsInten)
        {
            return;
        }
        if (!mIsFirst)
            gameObject.SetActive(false);
        mLabelTemplate = Instantiate(gameObject) as GameObject;
        Destroy(mLabelTemplate.GetComponent<EmojiExtension>());
        mLabelTemplate.name = "LabelTemplate";
        mLabelTemplate.transform.Find("Chat_Name").GetComponent<UILabel>().text =
            "  ";
        mLabelTemplate.transform.parent = transform.parent;
        mLabelTemplate.SetActive(false);
        mIsInten = true;
    }
    /// <summary>
    /// 添加文本信息
    /// </summary>
    public void Add(string text, EM_ChatChan Chat, ChatPoolDT chat = null)
    {
        if (!mIsInten)
        {
            Start();
        }
        ProcessedText(text, Chat, chat);
    }
    /// <summary>
    /// 发送文字
    /// </summary>
    private void ProcessedText(string text, EM_ChatChan Chat, ChatPoolDT chat)
    {
        transform.Find("Chat_Label").GetComponent<UILabel>().UpdateNGUIText();

        GameObject go = NGUITools.AddChild(gameObject);//复制一个文本框
        go.name = "Item";//取名为Item
        go.transform.localPosition = new Vector3(0, -mCurrentHeight, 0);//文本框位置为
        mItems.Add(go);//MITEMS为gameobject集合 list
        GameObject tmpmLabelTemplate;
        if (chat == null)
        {
            tmpmLabelTemplate = NGUITools.AddChild(go, mMyselfChat);
        }
        else
            tmpmLabelTemplate = NGUITools.AddChild(go, mLabelTemplate);
        StringBuilder sb = new StringBuilder();//创建一个动态字符串
        //sb.AppendLine(header);//添加输入的至下一行
        //sb.Append("   ");
        int numOfLine = -1;//另起一行就+1；
        bool isUseColor = false;
        float regionWidth = mLineWidth;//宽度初始化
        for (int offset = 0; offset < text.Length; offset++)//遍历text字符串
        {
            char ch = text[offset];//创建一个遍历字符
            if (ch == '#')//如果是表情关键字  进入
            {
                if (ParseEmoji(ref text, ref tmpmLabelTemplate, ref regionWidth, ref numOfLine, ref offset, ref sb))
                    continue;
            }

            //float w = NGUIText.GetGlyphWidth(ch, 0) + mSpacingX;
            float w = NGUIText.GetGlyphWidth(ch, 0, mFontSize) + mSpacingX;
            //if (regionWidth - w < 0)
            //{
            //    sb.Append('\n');
            //    ++numOfLine;
            //    regionWidth = mLineWidth;
            //}
            if (ch == '@')
                continue;
            sb.Append(ch);

            regionWidth -= w;
        }

        if (isUseColor)
        {
            sb.Append("[-]");
        }

        UILabel uiLabel = AddEmojiText(go, sb.ToString(), chat, tmpmLabelTemplate);
        mCurrentHeight += uiLabel.height;
        switch (Chat)
        {
            case EM_ChatChan.eChan_World:
                go.transform.parent = (mWorldParent);
                mWorldParent.GetComponent<UIGrid>().enabled = true;
                break;
            case EM_ChatChan.eChan_Legion:
                go.transform.parent = (mGroupParent);
                mGroupParent.GetComponent<UIGrid>().enabled = true;
                break;
            case EM_ChatChan.eChan_Team:
                break;
            case EM_ChatChan.eChan_Private:
                go.transform.parent = (mPrivateParent);
                mPrivateParent.GetComponent<UIGrid>().enabled = true;
                break;
            case EM_ChatChan.eChan_System:
                go.transform.parent = (mSystemParent);
                mSystemParent.GetComponent<UIGrid>().enabled = true;
                break;
        }

    }
    /// <summary>
    /// 能否添加表情，可以就添加表情  不能就返回false
    /// </summary>
    private bool ParseEmoji(ref string text, ref GameObject go, ref float regionWidth,
        ref int numOfLine, ref int offset, ref StringBuilder sb)
    {
        int start = offset;//复制一个开始位数
        int end = GetLastNumberIndex(text, start);//返回表情对应ID
        if (end >= 0)
        {
            string spriteName = text.Substring(start + 1, end - start);//截取复制表情对应id
            if (IsEmojiImage(spriteName))
            {
                int num = CalcuNumOfPlaceholder();//计算占位符
                float width = NGUIText.GetGlyphWidth(' ', 0, mFontSize) + mSpacingX;
                float totalWidth = mFontSize;
                if (regionWidth - totalWidth < 0)
                {
                    sb.Append('\n');
                    ++numOfLine;
                    regionWidth = mLineWidth;
                }
                Vector3 pos;
                if (!mIsFirst)
                    pos = CalculateEmojiPos(mLineWidth - regionWidth, numOfLine);
                else
                    pos = CalculateEmojiPos(mLineWidth + regionWidth, numOfLine);
                AddEmojiImage(go, pos, spriteName);
                //sb.Append(' ', num);
                regionWidth -= totalWidth;

                // offset += end - start + 1 - 1 ===> offset += end - start
                offset += end - start;
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 查找数字的下标  并返回
    /// </summary>
    private int GetLastNumberIndex(string str, int start)
    {
        int end = -1;

        for (int i = start + 1; i < str.Length; i++)
        {
            if (char.IsDigit(str[i]))
                end = i;
            else
                break;
        }
        return end;
    }
    /// <summary>
    /// 计算数字占位符
    /// </summary>
    /// <returns></returns>
    private int CalcuNumOfPlaceholder()
    {
        UISpriteData data = atlas.GetSprite(mListOfSprites[0]);
        float width = NGUIText.GetGlyphWidth(' ', 0, mFontSize) + mSpacingX;
        int num = Mathf.CeilToInt(data.width / width);
        return num;
    }
    /// <summary>
    /// 计算表情位置
    /// </summary>
    private Vector3 CalculateEmojiPos(float x, float y)
    {
        float yPos = (y + 1) * mLineHeight;
        return new Vector3(x, -yPos, 0);
    }
    /// <summary>
    /// 如果有这个对应的表情  则返回true
    /// </summary>
    private bool IsEmojiImage(string spriteName)
    {
        if (mListOfSprites == null)
            mListOfSprites = atlas.GetListOfSprites();
        if (mListOfSprites.Contains(spriteName))
            return true;
        return false;
    }
    /// <summary>
    /// 添加表情以及文本
    /// </summary>
    private UILabel AddEmojiText(GameObject parent, string text, ChatPoolDT chat, GameObject go)
    {
        UILabel uiLabel = go.transform.Find("Chat_Label").GetComponent<UILabel>();
        //Data_Pool.m_GeneralPlayerPool.f_ReadInfor(chat.m_Id, EM_ReadPlayerStep.Base, (object obj) => {
        //    print(((BasePlayerPoolDT)obj).iId); });
		if(go.name == "MyselfChat(Clone)")
		{
			go.transform.Find("Chat_Vip").GetComponent<UILabel>().text = "VIP" + UITool.f_GetNowVipLv();
		}
        if (chat != null)
        {
            if (chat.m_Id == 99)
            {
                go.transform.Find("Chat_Icon").gameObject.SetActive(false);
                go.transform.Find("Chat_Case").gameObject.SetActive(false);
                go.transform.Find("Chat_Name").gameObject.SetActive(false);
                go.transform.Find("Chat_Label").gameObject.SetActive(false);
                go.transform.Find("Chat_TextBg").gameObject.SetActive(false);
                go.transform.Find("Chat_Vip").gameObject.SetActive(false);

                go.transform.Find("SystemNotice").gameObject.SetActive(true);
                go.transform.Find("SystemNotice/Chat_Label").GetComponent<UILabel>().text = text;
            }
            else { 
            BasePlayerPoolDT tBasePlayerPoolDT = ((BasePlayerPoolDT)Data_Pool.m_GeneralPlayerPool.f_GetForId(chat.m_Id));
            string tNname = tBasePlayerPoolDT.m_szName;
            go.transform.Find("Chat_Icon").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSpriteBySexId(tBasePlayerPoolDT.m_iSex);
            go.transform.Find("Chat_Case").GetComponent<UISprite>().spriteName = UITool.f_GetImporentColorName(tBasePlayerPoolDT.m_iFrameId, ref tNname);
            go.transform.Find("Chat_Name").GetComponent<UILabel>().text = tNname;
            go.transform.Find("Chat_Vip").GetComponent<UILabel>().text = "VIP"+ tBasePlayerPoolDT.m_iVip;
            //go.transform.Find("Chat_Vip").GetComponent<UISprite>().spriteName = string.Format("Vip_ ({0})", tBasePlayerPoolDT.m_iVip); ;
            f_RegClickEvent(go.transform.Find("Chat_Case").gameObject, IconBtn, chat.m_Id);
            }
        }
        else
        {
            string tNname = Data_Pool.m_UserData.m_szRoleName;
            // go.transform.Find("Chat_Icon").GetComponent<UI2DSprite>().sprite2D =
                // UITool.f_GetIconSpriteByCardId(Data_Pool.m_CardPool.mRolePoolDt.m_CardDT.iId);
			go.transform.Find("Chat_Icon").GetComponent<UI2DSprite>().sprite2D =
                UITool.f_GetMyIcon(Data_Pool.m_CardPool.mRolePoolDt.m_CardDT.iId); 
            go.transform.Find("Chat_Case").GetComponent<UISprite>().spriteName = UITool.f_GetImporentColorName(Data_Pool.m_CardPool.mRolePoolDt.m_CardDT.iImportant, ref tNname);
            go.transform.Find("Chat_Name").GetComponent<UILabel>().text = tNname;
            //go.transform.Find("Chat_Vip").GetComponent<UISprite>().spriteName = string.Format("Vip_ ({0})", UITool.f_GetVipLv(Data_Pool.m_UserData.f_GetProperty((int)EM_UserAttr.eUserAttr_Vip)));

        }
        //uiLabel.depth = mWidgetDepth;//层级
        uiLabel.overflowMethod = UILabel.Overflow.ClampContent;//锚点
        uiLabel.text = text;
        go.SetActive(true);

        return uiLabel;
    }
    /// <summary>
    /// 添加表情图片
    /// </summary>
    private UISprite AddEmojiImage(GameObject parent, Vector3 pos, string spriteName)
    {
        UISprite uiSprite = NGUITools.AddChild<UISprite>(parent.transform.Find("Chat_Label").gameObject);//gameObject);.transform.Find("LabelTemplate(Clone)").gameObject
		GameObject label = parent.transform.Find("Chat_Label").gameObject;
		MessageBox.ASSERT("" + label);
        uiSprite.name = "Sprite";
        uiSprite.depth = mWidgetDepth;//层级
        uiSprite.pivot = UIWidget.Pivot.Left;//锚点
        uiSprite.atlas = atlas;//图集
        uiSprite.spriteName = spriteName;//精灵名字
		if(parent.name == "MyselfChat(Clone)")
		{
			label.transform.localPosition = new Vector3(900, -25, 0);
			uiSprite.transform.localPosition = new Vector3(-100,8f,0);//图片位置
		}
		else
		{
			label.transform.localPosition = new Vector3(540, -28, 0);
			uiSprite.transform.localPosition = new Vector3(-100,8f,0);
		}
		uiSprite.transform.localScale = new Vector3(2.8f,2.8f,2.8f);
        //uiSprite.MakePixelPerfect();
        uiSprite.width = mFontSize;//文字大小
        uiSprite.height = mFontSize;
        return uiSprite;
    }
    void IconBtn(GameObject go, object obj1, object obj2)
    {
		MessageBox.ASSERT("GO");
        Data_Pool.m_GeneralPlayerPool.f_ReadInfor((long)obj1, EM_ReadPlayerStep.Extend1, PayerInfo);
    }

    void PayerInfo(object obj)
    {
        BasePlayerPoolDT tmp = (BasePlayerPoolDT)obj;
        ccUIManage.GetInstance().f_SendMsg(UINameConst.LookPlayerInfoPage, UIMessageDef.UI_OPEN, tmp);
    }
}
