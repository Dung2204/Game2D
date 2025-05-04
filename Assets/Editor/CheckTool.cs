
using UnityEditor;
using UnityEngine;
using System.Text;

public class CheckTool
{
    [MenuItem("GameTool/CheckUIAtlasRef(UIAltas)")]
    
    static void f_CheckUIAtlasRef_UIAltas()
    {
        f_CheckUIAtlasRef("UIAltas");
    }
    [MenuItem("GameTool/CheckUIAtlasRef(UIAltas2)")]
    static void f_CheckUIAtlasRef_UIAltas2()
    {
        f_CheckUIAtlasRef("UIAtlas2");
    }


    //检查图集引用
    static void f_CheckUIAtlasRef(string checkName)
    {
        string[] lookFor = new string[] { "Assets/Resources/UIPrefab", "Assets/Resources/LegionRes/UIPrefab" };
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(string.Format("CheckName:{0}", checkName));
        sb.AppendLine("UIPrefab Find Start:");
        sb.AppendLine("Find TotalCount:[findCount]");
        sb.AppendLine("Ref PanelCount:[refPanelCount]");
        sb.AppendLine("Ref TotalCount:[refCount]");
        sb.AppendLine("*******************************");
        int findCount = 0;
        int refPanelCount = 0;
        int refCount = 0;
        StringBuilder tWidgetSb = new StringBuilder();
        string[] guids = AssetDatabase.FindAssets("t:Prefab",lookFor);
        for (int i = 0; i < guids.Length; i++)
        {
            string tAssetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            Transform tGo = AssetDatabase.LoadAssetAtPath<Transform>(tAssetPath);
            UIWidget[] tWidgets = tGo.GetComponentsInChildren<UIWidget>();
            int subRefCount = 0;
            tWidgetSb.Remove(0, tWidgetSb.Length);
            for (int j = 0; j < tWidgets.Length; j++)
            {
                if (tWidgets[j].material != null && tWidgets[j].material.name.Equals(checkName))
                {
                    tWidgetSb.Append(tWidgets[j].name);
                    tWidgetSb.Append("  ");
                    subRefCount++;
                    refCount++;
                }
            }
            if (subRefCount > 0)
            {
                sb.Append("\nPanel:<color=#ff0000>");
                sb.Append(tGo.name);
                sb.Append("</color>");
                sb.Append(" RefWidgetsCount:<color=#00ff00>");
                sb.Append(subRefCount.ToString());
                sb.Append("</color>");
                sb.Append("   WidgetsName:"); 
                sb.Append(tWidgetSb.ToString());
                refPanelCount++;
            }
            findCount++;
        }
        sb.AppendLine("\n*******************************");
        sb.Replace("[findCount]", findCount.ToString());
        sb.Replace("[refPanelCount]", refPanelCount.ToString());
        sb.Replace("[refCount]", refCount.ToString());
        Debug.Log(sb.ToString());
    }
}
