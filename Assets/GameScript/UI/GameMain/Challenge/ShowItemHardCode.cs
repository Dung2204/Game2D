using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowItemHardCode : MonoBehaviour
{
    private GameObject mAwardItem;

    private UIGrid mAwardGrid;
    public string szAward;
    public GameObject TaskAwardItem;



    private void Start()
    {
        mAwardGrid = transform.Find("Grid").GetComponent<UIGrid>();
        UI_OPEN();
    }

    private  void UI_OPEN()
    {
        List<ResourceCommonDT> listCommonDT = CommonTools.f_GetListCommonDT(szAward);

        for (int i = 0; i < listCommonDT.Count; i++)
        {
            _CreateItem(listCommonDT[i].mIcon, listCommonDT[i].mImportant, listCommonDT[i].mResourceNum);
        }
        mAwardGrid.GetComponent<UIGrid>().Reposition();

    }

    private void _CreateItem(int IconID, int DropOut, int Num)
    {
        GameObject tgo = NGUITools.AddChild(mAwardGrid.gameObject, TaskAwardItem);
        tgo.transform.Find("IconBorder").GetComponent<UISprite>().spriteName = UITool.f_GetImporentCase(DropOut);
        tgo.transform.Find("Icon").GetComponent<UI2DSprite>().sprite2D = UITool.f_GetIconSprite(IconID);
        tgo.transform.Find("Num").GetComponent<UILabel>().text = "";
        tgo.SetActive(true);
    }

}
