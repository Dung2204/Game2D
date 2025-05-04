using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
#endif

using System.IO;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;


#if UNITY_EDITOR
public class Editor_BuildPreprocess : IPreprocessBuild
{
    public int callbackOrder { get { return 0; } }

    public void OnPreprocessBuild(BuildTarget target, string path)
    {

        string dataDirectory = Application.streamingAssetsPath + "/" + GloData.glo_ProName +"/";
        string fileToCreate = Application.streamingAssetsPath + "/Data.tgz";

        Utility_SharpZipCommands.CreateTarGZ_FromDirectory(fileToCreate, dataDirectory);

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
#endif