using UnityEngine;
using System.Collections;
using ccU3DEngine;
using System.Collections.Generic;
public class AddProTripPageParam
{
    public int addProId;
    public int addProValue;
}
public class AddProTripPage : UIFramwork
{
    public TweenPosition tp;
    public TweenAlpha ta;
    public List<object> param;
    GameObject ItemParent;
    GameObject Item;
    protected override void UI_OPEN(object e)
    {
        base.UI_OPEN(e);
        param = (List<object>)e;
        ItemParent = f_GetObject("ItemParent");
        ItemParent.transform.localPosition = Vector3.zero;
        Destroy(ItemParent.transform.GetComponent<TweenPosition>());
        CancelInvoke("tSetActive");
        Item = f_GetObject("Item");
        UpdateFateTrip();
    }

    void UpdateFateTrip()
    {
        for (int i = ItemParent.transform.childCount - 1; i >= 0; i--)
            Destroy(ItemParent.transform.GetChild(i).gameObject);
        //属性
        for (int i = 0; i < param.Count; i++)
        {
            if (param[i] is Vector2)//位置信息
            {
                Vector2 pos = (Vector2)param[i];
                ItemParent.transform.localPosition = new Vector3(pos.x, pos.y, 0);
            }
            else if (param[i] is AddProTripPageParam)
            {
                AddProTripPageParam item = param[i] as AddProTripPageParam;
                string strAddPro = CommonTools.f_GetAddProperty((EM_RoleProperty)item.addProId, item.addProValue);
                if (item.addProValue != 0)
                {
                    EM_Important tImportant = EM_Important.Green;
                    string Pro = UITool.f_GetImporentForName((int)tImportant, UITool.f_GetProName((EM_RoleProperty)item.addProId) + "  " + (item.addProValue > 0 ? "+" : "") + strAddPro);

                    GameObject tFateTrip = NGUITools.AddChild(ItemParent, Item);
                    tFateTrip.SetActive(true);
                    tFateTrip.transform.localPosition = Vector3.zero;
                    tFateTrip.GetComponent<UILabel>().text = Pro;
                    //int length = tFateTrip.GetComponent<UILabel>().width;
                    TweenAlpha itemTA = tFateTrip.AddComponent<TweenAlpha>();
                    itemTA.from = ta.from;
                    itemTA.to = ta.to;
                    itemTA.duration = ta.duration;
                    //UISprite[] taSprite= tFateTrip.GetComponentsInChildren<UISprite>();
                    //for (int a = 0; a < taSprite.Length; a++)
                    //{
                    //    taSprite[a].width = length + 53;
                    //}
                }
            }
            else if(param[i] is string)
            {
                GameObject tFateTrip = NGUITools.AddChild(ItemParent, Item);
                tFateTrip.SetActive(true);
                tFateTrip.transform.localPosition = Vector3.zero;
                tFateTrip.GetComponent<UILabel>().text = param[i] as string;
                //int length = tFateTrip.GetComponent<UILabel>().width;
                TweenAlpha itemTA = tFateTrip.AddComponent<TweenAlpha>();
                itemTA.from = ta.from;
                itemTA.to = ta.to;
                itemTA.duration = ta.duration;
                //UISprite[] taSprite = tFateTrip.GetComponentsInChildren<UISprite>();
                //for (int a = 0; a < taSprite.Length; a++)
                //{
                //    taSprite[a].width = length + 53;
                //}
            }
        }
        ItemParent.GetComponent<UIGrid>().enabled = true;
        ItemParent.GetComponent<UIGrid>().Reposition();
        TweenPosition itemTP = ItemParent.AddComponent<TweenPosition>();
        itemTP.from = new Vector3(ItemParent.transform.localPosition.x, ItemParent.transform.localPosition.y + tp.from.y, tp.from.z);
        itemTP.to = new Vector3(ItemParent.transform.localPosition.x, ItemParent.transform.localPosition.y + tp.to.y, tp.to.z);
        itemTP.duration = tp.duration;
        Invoke("tSetActive", tp.duration);
    }

    void tSetActive()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
}
