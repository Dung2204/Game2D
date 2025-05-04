using UnityEngine;
using System.Collections;
using Spine.Unity;
/// <summary>
/// 招募显示得到的卡牌item
/// </summary>
public class RecruitCardItem : UIFramwork {
    public void ShowData(CardDT cardDT)
    {
        string cardName = cardDT.szName;
        UITool.f_GetImporentColorName(cardDT.iImportant, ref cardName);
        f_GetObject("CardName").GetComponent<UILabel>().text = cardName;

        Transform ModelPoint = f_GetObject("ModelPoint").transform;
        if (ModelPoint.transform.Find("Model") != null)
        {
            UITool.f_DestoryStatelObject(ModelPoint.transform.Find("Model").gameObject);
        }
        GameObject model = UITool.f_GetStatelObject(cardDT) as GameObject;
        model.transform.parent = ModelPoint.transform;
        model.transform.localPosition = Vector3.zero;
        int scaleSize = 80;
        model.transform.localScale = Vector3.one * scaleSize;
        model.layer = 5;
        SkeletonAnimation SkeAni = model.GetComponent<SkeletonAnimation>();
        //SkeAni.loop = true;
        //SkeAni.AnimationName = "Stand";
        SkeAni.state.SetAnimation(0, "Stand", true);
        model.GetComponent<Renderer>().sortingOrder = 7;
        model.name = "Model";
    }
}
