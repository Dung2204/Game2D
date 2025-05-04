using UnityEngine;
using System.Collections;

public class DailyPveItem : MonoBehaviour
{
    public UILabel tTitle;
    public UILabel tOpenStr;
    public UILabel LabelTimes;//次数
    public UI2DSprite tImage;
    public UI2DSprite tRoleImage;
    public GameObject tLockIcon;
    public GameObject mZeroTimesTip;

    private DungeonPoolDT mData;

    public void f_UpdateByInfo(string title,bool isLock, string iconID,int roleIcon,string openStr,int times)
    {
        tLockIcon.SetActive(isLock);
        if (isLock)
        {
            tTitle.text = string.Format("[C4BDAAFF]{0}", title);
            tOpenStr.text = string.Format("[D6D6CCFF]{0}", openStr);
            LabelTimes.text = string.Format("[D6D6CCFF]{0}/1", times);
        }
        else
        {
            tTitle.text = string.Format("[FFD571FF]{0}", title);
            tOpenStr.text = "";
            LabelTimes.text = string.Format("[F4E0B2FF]{0}/1", times);
        }
        tImage.sprite2D = UITool.f_GetDungeonSprite(int.Parse(iconID));
        UITool.f_Set2DSpriteGray(tImage, isLock);
        tRoleImage.sprite2D = UITool.f_GetDungeonSprite(roleIcon);
        tRoleImage.MakePixelPerfect();
        tRoleImage.transform.localScale = new Vector3(1.12f, 1.12f, 1.0f);
        UITool.f_Set2DSpriteGray(tRoleImage, isLock);
        LabelTimes.gameObject.SetActive(!isLock);
        mZeroTimesTip.SetActive(!isLock && times == 1);


    }
}
