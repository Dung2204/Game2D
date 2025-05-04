using UnityEngine;
using System.Collections;
using ccU3DEngine;
/// <summary>
/// 屏幕适配脚本
/// </summary>
public class ScreenAdaptive : MonoBehaviour 
{

    public enum Type
    {
        Background = 0,//背景图
        Function = 1,//普通窗口
        DragScrollVertical = 2,//垂直滚动条
    }

    public Type m_type = Type.Function;

    private Vector3 awakeScale = Vector3.one;
    private bool runOne = false;
	// Use this for initialization
	void Awake()
    {
        awakeScale = transform.localScale;
        f_RunScript();
    }
    // 手动执行脚本
    public void f_RunScript()
    {
        transform.localScale = awakeScale;
        switch (m_type)
        {
            case Type.Background:
                {
                    Vector3 scale = transform.localScale;
                    scale.x = scale.x * ScreenControl.Instance.mScaleRatio;
                    scale.y = scale.y * ScreenControl.Instance.mScaleRatio;
                    transform.localScale = scale;
                }
                break;
            case Type.Function:
                {
                    Vector3 scale = transform.localScale;
                    scale.x = scale.x * ScreenControl.Instance.mFunctionRatio;
                    scale.y = scale.y * ScreenControl.Instance.mFunctionRatio;
                    scale.z = scale.y;
                    transform.localScale = scale;
                }
                break;
            case Type.DragScrollVertical:
                {
                    Vector3 scale = transform.localScale;
                    scale.x = scale.x * ScreenControl.Instance.mFunctionRatio;
                    scale.y = scale.y * ScreenControl.Instance.mFunctionRatio;
                    scale.z = scale.y;
                    transform.localScale = scale;
                    if (!runOne)
                    {
                        runOne = true;
                        UIDragScrollView uiDragScrollView = transform.GetComponentInChildren<UIDragScrollView>();
                        if (uiDragScrollView != null)
                        {
                            Vector3 UIDragScrollViewScale = uiDragScrollView.transform.GetComponent<BoxCollider>().size;
                            UIDragScrollViewScale.y /= ScreenControl.Instance.mFunctionRatio;
                            uiDragScrollView.transform.GetComponent<BoxCollider>().size = UIDragScrollViewScale;
                        }
                        UIScrollView uiScrollView = transform.GetComponentInChildren<UIScrollView>();
                        if (uiScrollView != null)
                        {
                            Vector2 UIScrollViewScale = uiScrollView.transform.GetComponent<UIPanel>().GetViewSize();
                            UIScrollViewScale.y /= ScreenControl.Instance.mFunctionRatio;
                            uiScrollView.transform.GetComponent<UIPanel>().SetRect(0, 0, UIScrollViewScale.x, UIScrollViewScale.y);
                        }
                    }
                }
                break;
        }

        //enabled = false;
    }
}
