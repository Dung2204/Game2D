
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;



public class TQTTools
{
    //[MenuItem("GameObject/TQTuan/FindAllTMP", false, -1)]
    //static void FindAllTMP()
    //{
    //    Object[] temp = Selection.gameObjects[0].GetComponentsInChildren<TextMeshProUGUI>(true).ToArray<Object>();
    //    Selection.objects = temp;
    //}
    [MenuItem("GameObject/TQTuan/FindAllTMPInside",false,-1)]
    static void FindAndReplaceFont()
    {
        List<UILabel > rs = RecursiveFindChild(Selection.gameObjects[0]);
        Selection.objects = rs.ToArray();
    }

    public static List<UILabel> RecursiveFindChild(GameObject parent)
    {
        Dictionary<string, UILabel> rs = new Dictionary<string, UILabel>();
        GetInside(parent, rs);
        return rs.Select(o=>o.Value).ToList();
    }

    private static void GetInside(GameObject parent, Dictionary<string, UILabel> rs)
    {
        foreach (Transform transform in parent.transform)
        {
            UILabel[] tmp = parent.GetComponentsInChildren<UILabel>(true);
            foreach (var item in tmp)
            {
                if (!rs.ContainsKey(item.GetInstanceID().ToString())) { 
                    rs.Add(item.GetInstanceID().ToString(),item);
                }
            }
            if (transform.childCount > 0)
            {
                GetInside(transform.gameObject, rs);
            }
        }
    }

    [MenuItem("Assets/TQTuan/SetTMPSpriteAssets")]
    static void SetTMPSpriteAssets()
    {
        Object[] selectedObjs = Selection.objects;
        foreach (var item in selectedObjs)
        {
            string filePath = $"{Application.dataPath.Replace("Assets", string.Empty)}/{AssetDatabase.GetAssetPath(item)}";
            Debug.LogError(filePath);
            string[] lines = File.ReadAllLines(filePath);
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("m_HorizontalBearingX:"))
                {
                    lines[i] = lines[i].Replace("-125", "0");
                }
                else if (lines[i].Contains("m_HorizontalBearingY:"))
                {
                    lines[i] = lines[i].Replace("125", "160");
                } 
            }
            File.WriteAllLines(filePath, lines);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.LogError("Done");
    }

    [MenuItem("Assets/TQTuan/AutoMaxSize")]
    static void AutoMaxSize()
    {
        Object[] selectedObjs = Selection.objects;
        int[] maxSizeArray = new int[10] { 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384 };
        foreach (var item in selectedObjs)
        {
            AssetImporter assetImporter = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(item));
            TextureImporter importer = (TextureImporter)assetImporter;
            int width = 0, height = 0;
            if (importer != null)
            {
                object[] args = new object[2] { 0, 0 };
                MethodInfo mi = typeof(TextureImporter).GetMethod("GetWidthAndHeight", BindingFlags.NonPublic | BindingFlags.Instance);
                mi.Invoke(importer, args);

                width = (int)args[0];
                height = (int)args[1];
            }

            int setMaxSize = 2048;// 2048 is default
            for (int i = 0; i < maxSizeArray.Length; i++)
            {
                if (maxSizeArray[i] >= Mathf.Max(width, height))
                {
                    setMaxSize = maxSizeArray[i];
                    break;
                }
            }
            importer.maxTextureSize = setMaxSize;
            importer.SaveAndReimport();
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/TQTuan/TextureNamePrefix 371_", false, -1)]
    static void AddPrefixName()
    {


        Object[] oAssets = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets);
        foreach (Object asset in oAssets)
        {
            if (asset.GetType() == typeof(DefaultAsset))// folder
            {
                string fpath = Application.dataPath.Replace("Assets", AssetDatabase.GetAssetPath(asset));
                string[] fileInDirs = Directory.GetFiles(fpath, "*.*", SearchOption.AllDirectories).Where(o => !o.EndsWith(".meta")).ToArray();
                foreach (string file in fileInDirs)
                {
                    string rFile = file.Replace("\\", "/");
                    string fileName = rFile.Substring(rFile.LastIndexOf("/") + 1, rFile.Length - (rFile.LastIndexOf("/") + 1));
                    RenameAsset(rFile.Replace(Application.dataPath, "Assets"), fileName);
                }
            }
            else
            { // asset
                RenameAsset(AssetDatabase.GetAssetPath(asset), asset.name);
            }
        }
        AssetDatabase.Refresh();

    }
    static void RenameAsset(string path, string fileName)
    {
        string prefix = "371_";
        if (fileName.Contains(prefix)) return;
        AssetDatabase.RenameAsset(path, prefix + fileName);
    }

    [MenuItem("GameObject/TQTuan/Get Child Path", false, -1)]
    static void GetChildPath()
    {
        Transform target = Selection.gameObjects[0].transform;
        List<string> rs = new List<string>();
        while (target.parent != null)
        {
            rs.Add(target.name);
            target = target.parent;
        }
        rs.Reverse();
        rs.RemoveAt(0);
        string targetPath = string.Join("/", rs);
        string code = "AddGOReference(\"" + targetPath + "\");";
        GUIUtility.systemCopyBuffer = code;
    }

    [MenuItem("GameObject/TQTuan/Syns Component _F9", false, -1)]
    static void CopyAndPastAllComponent()
    {
        Component[] form = Selection.gameObjects[0].GetComponents(typeof(Component));
        Component[] to = Selection.gameObjects[1].GetComponents(typeof(Component));
        for (int i = 0; i < form.Length; i++)
        {
            Debug.LogError(form[i].GetType());
            Component fromScript = form[i];
            Component toScript = null;
            for (int j = 0; j < to.Length; j++)
            {
                Component tempScript = to[j];
                if (tempScript.GetType() == fromScript.GetType())
                {
                    toScript = tempScript;
                    break;
                }
            }
            if(toScript == null)
            {
                Selection.gameObjects[1].AddComponent(fromScript.GetType());
            }
            EditorUtility.CopySerialized(fromScript, toScript);
        }
    }
    public static Component[] Form = null;
    [MenuItem("GameObject/TQTuan/Copy All Component _F3", false, -1)]
    static void CopyAllComponent()
    {
        Form = Selection.gameObjects[0].GetComponents(typeof(Component));
    }

    [MenuItem("GameObject/TQTuan/Past All Component _F4", false, -1)]
    static void PastAllComponent()
    {
        Component[] form = Form;
        if (form == null) return;
        Component[] to = Selection.gameObjects[0].GetComponents(typeof(Component));
        for (int i = 0; i < form.Length; i++)
        {
            Debug.LogError(form[i].GetType());
            Component fromScript = form[i];
            Component toScript = null;
            for (int j = 0; j < to.Length; j++)
            {
                Component tempScript = to[j];
                if (tempScript.GetType() == fromScript.GetType())
                {
                    toScript = tempScript;
                    break;
                }
            }
            if (toScript == null)
            {
                Selection.gameObjects[1].AddComponent(fromScript.GetType());
            }
            EditorUtility.CopySerialized(fromScript, toScript);
        }
    }

    [MenuItem("GameObject/TQTuan/ShowFilterQual _F5", false, -1)]
    static void ShowFilterQal()
    {
        List<UISprite> rs = FindChild(Selection.gameObjects[0], filterQual);
        Selection.objects = rs.ToArray();
    }

    [MenuItem("GameObject/TQTuan/ShowFilterMoney _F6", false, -1)]
    static void ShowFilterMoney()
    {
        List<UISprite> rs = FindChild(Selection.gameObjects[0], filterMoney);
        Selection.objects = rs.ToArray();
    }

    public static List<UISprite> FindChild(GameObject parent, string[] filter)
    {
        Dictionary<string, UISprite> rs = new Dictionary<string, UISprite>();
        GetInsideUISprite(parent, rs, filter);
        return rs.Select(o => o.Value).ToList();
    }
    private static void GetInsideUISprite(GameObject parent, Dictionary<string, UISprite> rs, string[] filter)
    {
        foreach (Transform transform in parent.transform)
        {
            UISprite[] tmp = parent.GetComponentsInChildren<UISprite>(true);
            foreach (var item in tmp)
            {
                if (!rs.ContainsKey(item.GetInstanceID().ToString()) && Checkfilter(item.spriteName.ToString(), filter))
                {
                    find2DSprite(item);
                    rs.Add(item.GetInstanceID().ToString(), item);
                    Debug.LogError(item.name, item);
                }
            }
            if (transform.childCount > 0)
            {
                GetInsideUISprite(transform.gameObject, rs, filter);
            }
        }
    }

    private static void find2DSprite(UISprite uISprite)
    {
        UIAtlas atlas = AssetDatabase.LoadAssetAtPath("Assets/Res375ver2/Quality/QualityAtlas.prefab", typeof(UIAtlas)) as UIAtlas;
        uISprite.atlas = atlas ;
        uISprite.transform.localScale = Vector3.one;
        uISprite.keepAspectRatio = UIWidget.AspectRatioSource.Free;
        uISprite.aspectRatio = 1;
        UI2DSprite[] sInChildren = uISprite.GetComponentsInChildren<UI2DSprite>(true);
        if(sInChildren.Length <= 0)
        {
            UI2DSprite[] parent = uISprite.parent.GetComponentsInChildren<UI2DSprite>(true);
            if(parent.Length <= 0)
            {
                Debug.LogError("ko tim thay UI2DSprite o parent");
            }
            else
            {
                SetUI2DSprite(parent[0], uISprite);
            }
        }
        else
        {
            SetUI2DSprite(sInChildren[0], uISprite);
        }
    }

    private static void SetUI2DSprite(UI2DSprite uI2DSprite, UISprite uISprite)
    {
        uI2DSprite.transform.localScale = Vector3.one;
        uI2DSprite.keepAspectRatio = UIWidget.AspectRatioSource.Free;
        uI2DSprite.aspectRatio = 1;

        uI2DSprite.depth = uISprite.depth + 1;
        uI2DSprite.width = (int)(uISprite.width * 0.88);
        uI2DSprite.height = (int)(uISprite.height * 0.88);
        Debug.LogError(uI2DSprite.name, uI2DSprite);
    }

    private static bool Checkfilter(string name, string[] filter)
    {
        for (int i = 0; i < filter.Length; i++)
        {
            if(filter[i] == name)
            {
                return true;
            }
        }

        return false;
    }

    private static string[] filterMoney = new string[] { "Icon_BattleFeat", "Icon_ChaosPoint", "Icon_CrossServerScore", "Icon_CrusadeToken", "Icon_Energy",
                                             "Icon_Exp", "Icon_Fame", "Icon_GeneralSoul", "Icon_GodSoul", "Icon_KP",
                                             "Icon_LegionContribution", "Icon_Money", "Icon_Prestige", "Icon_Sycee", "Icon_Vigor" ,"Icon_VIP", "Iocn_Exp"};
    private static string[] filterQual = new string[] { "Icon_White", "Icon_Green", "Icon_Blue",  "Icon_Purple", "Icon_Oragen", "Icon_Red", "Icon_Gold"};
}
#endif