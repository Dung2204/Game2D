using ccU3DEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class Logo18Panel : UIFramwork
//{
//    GameObject logo;
//    private bool isDragging = false;
//    private Vector3 offset;
//    private float imageWidth, imageHeight;
//    private Vector3 minBounds, maxBounds;
//    private Transform panelTransform;
//    protected override void UI_OPEN(object e)
//    {
//        base.UI_OPEN(e);
//    }
//    protected override void f_Create()
//    {
//        base.f_Create();
//    }
//    protected override void UI_CLOSE(object e)
//    {
//        base.UI_CLOSE(e);
//    }
//    protected override void f_InitMessage()
//    {
//        base.f_InitMessage();
//        logo = f_GetObject("logo");

//        imageWidth = 203;
//        imageHeight = 44;
//        panelTransform = logo.transform.parent; // Lấy panel chứa logo
//        CalculateBounds();
//        UIEventListener tEvent = UIEventListener.Get(logo);
//        tEvent.onPress = OnPress;
//        tEvent.onDrag = OnDrag;
//    }

//    void CalculateBounds()
//    {
//        UITexture texture = logo.GetComponent<UITexture>();
//        if (texture != null)
//        {
//            imageWidth = texture.width;
//            imageHeight = texture.height;
//        }
//        else
//        {
//            Debug.LogError("Không tìm thấy UITexture trên logo!");
//            return;
//        }
//        // Giới hạn theo Panel
//        Vector3 panelSize = panelTransform.GetComponent<UIPanel>().GetViewSize();
//        minBounds = new Vector3(-panelSize.x / 2, -panelSize.y / 2, 0);
//        maxBounds = new Vector3(panelSize.x / 2, panelSize.y / 2, 0);

//    }
//    public void OnPress(GameObject go, bool obj1)
//    {

//        if (obj1) // Khi nhấn xuống
//        {
//            Vector3 mouseWorldPos = UICamera.currentCamera.ScreenToWorldPoint(Input.mousePosition);
//            Vector3 mouseLocalPos = panelTransform.InverseTransformPoint(mouseWorldPos);
//            offset = go.transform.localPosition - mouseLocalPos;

//            isDragging = true;
//        }
//        else
//        {
//            isDragging = false;
//            OnSetPosition(go);
//        }
//    }
//    public void OnDrag(GameObject go, Vector2 delta)
//    {
//        if (!isDragging) return;

//        Vector3 mouseWorldPos = UICamera.currentCamera.ScreenToWorldPoint(Input.mousePosition);
//        Vector3 newLocalPos = panelTransform.InverseTransformPoint(mouseWorldPos) + offset;

//        // chạm biên X
//        if (newLocalPos.x <= minBounds.x + imageWidth)
//        {
//            newLocalPos.x = minBounds.x + imageWidth;
//        }
//        else if (newLocalPos.x >= maxBounds.x - imageWidth)
//        {
//            newLocalPos.x = maxBounds.x - imageWidth;
//        }
//        // chạm biên Y
//        if (newLocalPos.y <= minBounds.y + imageHeight)
//        {
//            newLocalPos.y = minBounds.y + imageHeight;
//        }
//        else if (newLocalPos.y >= maxBounds.y - imageHeight)
//        {
//            newLocalPos.y = maxBounds.y - imageHeight;
//        }

//        go.transform.localPosition = newLocalPos;

//    }

//    private void OnSetPosition(GameObject go)
//    {
//        Vector3 mouseWorldPos = UICamera.currentCamera.ScreenToWorldPoint(Input.mousePosition);
//        Vector3 newLocalPos = panelTransform.InverseTransformPoint(mouseWorldPos) + offset;

//        // chạm biên X
//        if (newLocalPos.x <= minBounds.x + imageWidth)
//        {
//            newLocalPos.x = minBounds.x + imageWidth;
//        }
//        else if (newLocalPos.x >= maxBounds.x - imageWidth)
//        {
//            newLocalPos.x = maxBounds.x - imageWidth;
//        }
//        // chạm biên Y
//        if (newLocalPos.y <= minBounds.y + imageHeight)
//        {
//            newLocalPos.y = minBounds.y + imageHeight;
//        }
//        else if (newLocalPos.y >= maxBounds.y - imageHeight)
//        {
//            newLocalPos.y = maxBounds.y - imageHeight;
//        }

//        go.transform.localPosition = newLocalPos;
//    }
//}


public class Logo18Panel : UIFramwork
{
    GameObject logo;
    RectTransform logoRect;
    RectTransform parentRect;
    bool isDragging = false;
    Vector2 offset;

    protected override void f_InitMessage()
    {
        base.f_InitMessage();
        logo = f_GetObject("logo");
        logoRect = logo.GetComponent<RectTransform>();
        parentRect = logo.transform.parent.GetComponent<RectTransform>();

        UIEventListener tEvent = UIEventListener.Get(logo);
        tEvent.onPressV2 = OnPress;
        tEvent.onDrag = OnDrag;
    }

    void OnPress(GameObject go, bool isPressed, object obj = null)
    {
        if (isPressed)
        {
            isDragging = true;

            // offset giữa điểm click và vị trí hiện tại
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, Input.mousePosition, UICamera.currentCamera, out Vector2 localMousePos);
            offset = (Vector2)logoRect.localPosition - localMousePos;
        }
        else
        {
            isDragging = false;
            ClampToBounds();
        }
    }

    void OnDrag(GameObject go, Vector2 delta)
    {
        if (!isDragging) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, Input.mousePosition, UICamera.currentCamera, out Vector2 localMousePos);
        Vector2 targetPos = localMousePos + offset;

        logoRect.localPosition = targetPos;

        ClampToBounds();
    }

    void ClampToBounds()
    {
        Vector3 pos = logoRect.localPosition;
        Vector2 halfSize = logoRect.rect.size ;
        Vector2 parentSize = parentRect.rect.size * 0.5f;

        // Clamp x
        pos.x = Mathf.Clamp(pos.x, -parentSize.x + halfSize.x, parentSize.x - halfSize.x);
        // Clamp y
        pos.y = Mathf.Clamp(pos.y, -parentSize.y + halfSize.y, parentSize.y - halfSize.y);

        logoRect.localPosition = pos;
    }
}
