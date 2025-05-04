using UnityEngine;
using ccU3DEngine;
using System.Collections;

public class CrossTournamentTopItem : MonoBehaviour
{
    public static CrossTournamentTopItem f_Create(GameObject parent, GameObject item)
    {
        CrossTournamentTopItem result = item.GetComponent<CrossTournamentTopItem>();
        if (result == null)
            MessageBox.ASSERT("CrossTournamentTopItem.f_Create in Item must contain CrossTournamentTopItem");
        else
            result.f_Init(parent);
        return result;
    }

    [HideInInspector]
    public GameObject mParent;
    public GameObject mItem;
    public UILabel LbName;
    public UILabel LbPower;
    public Texture2D TexBg;
    private GameObject mRole;
    public GameObject mModelParent;
    public void f_Init(GameObject parent)
    {
        mParent = parent;
        mItem.SetActive(false);
    }
    public void f_UpdateByInfo(CrossUserTournamentPoolDT info,int top, GameObject parent)
    {
        mItem.SetActive(true);
        if (info == null)
        {
            LbName.text = "Đang chờ";
            LbPower.text = "";
            return;
        }
            
       
        mParent = parent;
        LbName.text = info.m_szName;
        LbPower.text = "Lực chiến: " + info.m_iBattlePower ;
        //Texture2D tTexture2D = glo_Main.GetInstance().m_ResourceManager.f_CreateResourceTexture2D(strTexBgRoot);
        //TexBg.mainTexture = tTexture2D;
        //TexBg.sprite2D = UITool.f_GetIconSpriteBySexId(top);
        loadModel(info.m_CardId);
    }

    void loadModel(int m_CardId)
    {
        UITool.f_CreateRoleByCardId(m_CardId, ref mRole, mModelParent.transform, 3);
    }
  
}
