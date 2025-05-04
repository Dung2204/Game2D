using ccU3DEngine;
using System;
using System.IO;
using System.Net;
using System.Text;
using UnityEditor;
using UnityEngine;
using Excel;

public class CsvTools
{
    private const string EditorSelectXlsxPath = "obUd";
    private const string EditorSelectCsvPath = "obUd";
    private const string EditorSelectOneCsvPath = "obUc";

    public static string FTPHost = "ftp://192.168.105.104/Pack/";
    public static string FTPUserName = "administrator";
    public static string FTPPassword = "hm123456";
    public static string FilePath;

    //[MenuItem("Tools/UpLoadToFtp")]
    public static void UploadFile()
    {
        FilePath = Application.dataPath + "/ccData/" + "ccData_A.bytes";
        Debug.Log("Path: " + FilePath);


        WebClient client = new System.Net.WebClient();
        Uri uri = new Uri(FTPHost + "ccData.byte");//new FileInfo(FilePath).Name);

        client.UploadProgressChanged += new UploadProgressChangedEventHandler(OnFileUploadProgressChanged);
        client.UploadFileCompleted += new UploadFileCompletedEventHandler(OnFileUploadCompleted);
        client.Credentials = new System.Net.NetworkCredential(FTPUserName, FTPPassword);
        client.UploadFileAsync(uri, "STOR", FilePath);
    }

//    [MenuItem("Tools/test")]
//    static void test()
//    {
//        string FilePath = Application.dataPath + "/ccData/" + "ccData_A1.bytes";
//        byte[] bData = ccFile.f_ReadFileForByte(Application.dataPath + "/ccData/", "ccData_A.bytes");
//        byte[] bUncompressData = ZipTools.ccUnCompress(bData);
//        string strData = System.Text.Encoding.UTF8.GetString(bUncompressData);
//        ccFile.f_SaveFileForString(Application.dataPath + "/ccData/", "aaa.txt", strData);
//    }
//
//    [MenuItem("Tools/test1")]
//    static void test1()
//    {
//        string FilePath = Application.dataPath + "/ccData/" + "aaa.txt";
//        string bData = File.ReadAllText(FilePath);
//        byte[] b = Encoding.UTF8.GetBytes(bData);
//        byte[] bb = ZipTools.ccCompress(b);
//        ccFile.f_SaveFileForByte(Application.dataPath + "//ccData//", "new.bytes", bb);
//    }

    static void OnFileUploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
    {
        Debug.Log("Uploading Progreess: " + e.ProgressPercentage);
    }

    static void OnFileUploadCompleted(object sender, UploadFileCompletedEventArgs e)
    {
        Debug.Log("File Uploaded");
    }


    static void CopyFile(string srcPath, string tarPath)
    {
        FileInfo fileInfo = new FileInfo(tarPath);
        if (fileInfo.Exists)
            FileUtil.ReplaceFile(srcPath, tarPath);
        else
            FileUtil.CopyFileOrDirectory(srcPath, tarPath);
    }

    [MenuItem("Tools/PackXlsxToScAndDt/PackOneXlsxToByte")]
    public static void f_PackOneXlsxToByte()
    {
        StringBuilder sbPackCsv = new StringBuilder();
        //设置生成的C#类目录
        string curDir = Application.dataPath + "/DTAndSCTemp/";
        //获取保存的表的路径
        //string path = EditorUserSettings.GetConfigValue(EditorSelectOneCsvPath);
        string path = "";
        path = EditorUtility.OpenFilePanel("Select Your xlsx", path, "xlsx");
        if (path != "")
        {
            //EditorUserSettings.SetConfigValue(EditorSelectOneCsvPath, path);
            if (!Directory.Exists(curDir))
            {
                Directory.CreateDirectory(curDir);
            }
            FileInfo file = new FileInfo(path);
            FileInfo[] files = new FileInfo[1];
            files[0] = file;
            string name = "";
            try
            {
                FileStream stream = file.OpenRead();
                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                if (!string.IsNullOrEmpty(name))
                {
                    sbPackCsv.Append(name + "1#QW");
                }
                int row = 0;
                string sFirstSheetName = excelReader.Name;
                do
                {
                    // sheet name
                    if (excelReader.Name.Equals(sFirstSheetName))// 固定规则，只读第一个表
                    {
                        while (excelReader.Read())
                        {
                            row++;
                            if (row < 3)
                            {
                                continue;
                            }

                            bool isFirst = true;
                            for (int i = 0; i < excelReader.FieldCount; i++)
                            {
                                string value = excelReader.IsDBNull(i) ? "" : excelReader.GetString(i);
                                if (i == 0 && string.IsNullOrEmpty(value))
                                {
                                    break;
                                }
                                if (isFirst)
                                {
                                    isFirst = false;
                                    sbPackCsv.Append(value);
                                }
                                else
                                {
                                    sbPackCsv.Append(",");
                                    sbPackCsv.Append(value);
                                }
                            }
                            sbPackCsv.Append("1#QW");
                        }
                    }
                    else
                    {
                        //Debug.Log(string.Format("{0}, {1}, {2}", sFirstSheetName, excelReader.Name, "break"));
                        break;
                    }

                } while (excelReader.NextResult());
                excelReader.Close();
            }
            catch (Exception e)
            {
                Debug.Log(name + "文件读写错误\n" + e.ToString());
                throw;
            }
        }
        ccFile.f_SaveFileForString(Application.streamingAssetsPath, "info.txt", sbPackCsv.ToString());
    }

    [MenuItem("Tools/CsvTools/PackXlsxToByte")]
    public static void f_PackXlsxToByte()
    {
        StringBuilder sbPackCsv = new StringBuilder();
        //string path = Application.streamingAssetsPath + "/csv/";
        string path = EditorUserSettings.GetConfigValue(EditorSelectXlsxPath);

        if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
        {   // 没有path就弹出面板设置
            f_SetExcelPath(EditorSelectXlsxPath);
            return;
        }

        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        FileInfo[] files = directoryInfo.GetFiles("*.xlsx", SearchOption.TopDirectoryOnly);
        DateTime startTime = DateTime.Now;
        Debug.Log("开始遍历Csv");
        int num = 0;
        for (int i = 0; i < files.Length; i++)
        {
            ////过滤掉临时文件
            //if (files[i].Name.EndsWith(".meta"))
            //{
            //    continue;
            //}

            //获取不带扩展名的文件名
            string name = Path.GetFileNameWithoutExtension(files[i].ToString());
            //Debug.Log("遍历文件: " + name + files[i].Extension);
            if (name.StartsWith("表格") || name.StartsWith("Tips") || files[i].Name.StartsWith("~$"))
                continue;
            f_PackString(files[i], name, sbPackCsv);
            num += 1;
        }
        Debug.Log("遍历" + num + "个文件完成，用时: " + (DateTime.Now - startTime));

        byte[] b = Encoding.UTF8.GetBytes(sbPackCsv.ToString());
        byte[] bb = ZipTools.ccCompress(b);

        ccFile.f_SaveFileForByte(Application.dataPath + "//ccData//", "ccData_A.bytes", bb);
        ccFile.f_SaveFileForByte(Application.dataPath + "//ccData//", "ccData_I.bytes", bb);
        ccFile.f_SaveFileForByte(Application.dataPath + "//ccData//", "ccData_W.bytes", bb);

        CopyFile(Application.dataPath + "/ccData/" + "ccData_A.bytes",
            Application.persistentDataPath + "/" + GloData.glo_ProName + "/ccData.xlscc");

        Debug.Log(string.Format("输出ccData文件：{0}, 并复制到{1}", Application.dataPath + "/ccData/" + "ccData_*.bytes", Application.persistentDataPath + "/" + GloData.glo_ProName + "/ccData.xlscc"));
    }

    [MenuItem("Tools/Set Xlsx Path")]
    static void f_SetExcelPath()
    {
        string path = EditorUserSettings.GetConfigValue(EditorSelectXlsxPath);
        path = EditorUtility.OpenFolderPanel("Select Your Xlsx Path", path, "");
        if (path != "")
        {
            EditorUserSettings.SetConfigValue(EditorSelectXlsxPath, path);
            Debug.Log("Set Your Xlsx Path: " + path);
        }
    }

    //[MenuItem("Tools/PackXlsxToScAndDt/Set Xlsx Path")]
    static void f_SetExcelPath(string key)
    {
        string path = EditorUserSettings.GetConfigValue(key);
        path = EditorUtility.OpenFolderPanel("Select Your Xlsx Path", path, "");
        if (path != "")
        {
            EditorUserSettings.SetConfigValue(key, path);
            Debug.Log("Set Your Xlsx Path: " + path);
        }
    }

    [MenuItem("Tools/PackXlsxToScAndDt/Create All SC And DT Script")]
    public static void f_CreateAllScAndDtScript()
    {
        string path = EditorUserSettings.GetConfigValue(EditorSelectCsvPath);
        if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
        {   // 没有path就弹出面板设置
            f_SetExcelPath(EditorSelectCsvPath);
            return;
        }
        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        FileInfo[] files = directoryInfo.GetFiles("*.csv", SearchOption.AllDirectories);

        //设置生成的C#类目录
        string curDtDir = Application.dataPath + "/GameScript/DT/ScDT/";
        string curScDir = Application.dataPath + "/GameScript/SC/";
        //判断路径是否存在
        if (!Directory.Exists(curDtDir))
        {
            Directory.CreateDirectory(curDtDir);
        }
        if (!Directory.Exists(curScDir))
        {
            Directory.CreateDirectory(curScDir);
        }
        f_WriteScript(files, curDtDir, curScDir);
    }

    [MenuItem("Tools/PackXlsxToScAndDt/Create One SC And DT Script")]
    public static void f_CreateOneScAndDtScript()
    {
        //设置生成的C#类目录
        string curDir = Application.dataPath + "/GameScript/";
        //获取保存的表的路径
        string path = EditorUserSettings.GetConfigValue(EditorSelectOneCsvPath);
        path = EditorUtility.OpenFilePanel("Select Your Csv", path, "csv");
        if (path != "")
        {
            EditorUserSettings.SetConfigValue(EditorSelectOneCsvPath, path);
            if (!Directory.Exists(curDir))
            {
                Directory.CreateDirectory(curDir);
            }
            FileInfo file = new FileInfo(path);
            FileInfo[] files = new FileInfo[1];
            files[0] = file;
            f_WriteScript(files, curDir, curDir);
        }
    }

    public static void f_WriteScript(FileInfo[] files, string curDtDir, string curScDir)
    {
        StringBuilder sbDt = new StringBuilder();
        StringBuilder sbSc = new StringBuilder();
        string name = string.Empty;
        string classDtName = string.Empty;
        string classScName = string.Empty;
        string DtPath = string.Empty;
        string SCPath = string.Empty;
        string fileDtPath = string.Empty;
        string fileScPath = string.Empty;
        for (int i = 0; i < files.Length; i++)
        {
            using (StreamReader sr = files[i].OpenText())
            {
                string line1 = sr.ReadLine();
                string line2 = sr.ReadLine();

                sbDt.Remove(0, sbDt.Length);
                name = Path.GetFileNameWithoutExtension(files[i].ToString());
                classDtName = name + "DT.cs";
                classScName = name + "SC.cs";
                DtPath = "DT/ScDT/";
                SCPath = "SC/";
                //生成的是c#类   所以需要加上.cs后缀  这里的Pathname为生成类的名字
                fileDtPath = curDtDir + DtPath + classDtName;
                fileScPath = curScDir + SCPath + classScName;
                #region DT文件文本
                sbDt.Append("using System;\n");
                sbDt.Append("using System.Collections;\n");
                sbDt.Append("using System.Collections.Generic;\n\n");
                sbDt.Append("using UnityEngine;\n\n");
                sbDt.AppendFormat("public class {0}DT : NBaseSCDT", name);
                sbDt.Append(" {\n\t");

                string[] desc = line2.Split(new string[] { "," }, StringSplitOptions.None);
                string[] parmas = line1.Split(new string[] { "," }, StringSplitOptions.None);
                for (int parmasIndex = 1; parmasIndex < parmas.Length; parmasIndex++)
                {
                    if (parmasIndex <= desc.Length - 1)
                    {
                        sbDt.AppendFormat("\n\t///<summary>\n\t///{0}\n\t///</summary>\n\t", desc[parmasIndex]);
                    }
                    if (parmas[parmasIndex].StartsWith("i"))
                    {
                        sbDt.AppendFormat("public int {0};\n\t", parmas[parmasIndex]);
                    }
                    else if (parmas[parmasIndex].StartsWith("s"))
                    {
                        sbDt.AppendFormat("public string {0};\n\t", parmas[parmasIndex]);
                    }
                }
                sbDt.Append("\n}\n");
                #endregion
                #region SC文件文本
                sbSc.Remove(0, sbSc.Length);
                sbSc.Append("using System;\n");
                sbSc.Append("using System.Collections;\n");
                sbSc.Append("using System.Collections.Generic;\n\n");
                sbSc.Append("using UnityEngine;\n\n");
                sbSc.AppendFormat("public class {0}SC : NBaseSC", name);
                sbSc.Append(" {\n\t");

                sbSc.AppendFormat("public {0}SC() ", name);//;Create('{1}DT', true)}\n\n\t", name, name);
                sbSc.Append("{\n\t");
                sbSc.AppendFormat("\tCreate(\"{0}DT\", true);", name);
                sbSc.Append("\n\t}\n\n\t");

                sbSc.Append("public override void f_LoadSCForData(string strData) {\n\t\tDispSaveData(strData);\n\t}\n\n\t");

                sbSc.Append("private void DispSaveData(string ppSQL) {\n\t");// DispSaveData(strData);\n\t}\n\n\t");
                sbSc.Append("\tstring[] ttt = ppSQL.Split(new string[] { \"1#QW\"}, System.StringSplitOptions.None);\n\t");
                sbSc.AppendFormat("\t{0}DT DataDT;\n\t", name);
                sbSc.Append("\tstring[] tData;");
                sbSc.Append("\tstring[] tFoddScData = ttt[1].Split(new string[] { \"|\" },System.StringSplitOptions.None);\n\t");
                sbSc.Append("\tfor (int i = 0; i < tFoddScData.Length; i++) {\n\t\t\ttry {");
                sbSc.Append("\n\t\t\t\tif (tFoddScData[i] == \"\") {\n");
                sbSc.Append("\t\t\t\t\tMessageBox.DEBUG(m_strRegDTName + \"脚本存在空记录, \" + i);");
                sbSc.Append("\n\t\t\t\t\tcontinue;\n\t\t\t\t}");
                sbSc.Append("\n\t\t\t\ttData = tFoddScData[i].Split(new string[] { \"@,\" }, System.StringSplitOptions.None);");
                sbSc.Append("\n\t\t\t\tint a = 0;");
                sbSc.AppendFormat("\n\t\t\t\tDataDT = new {0}DT();", name);
                sbSc.Append("\n\t\t\t\tDataDT.iId = ccMath.atoi(tData[a++]);");
                sbSc.Append("\n\t\t\t\tif (DataDT.iId <= 0) {");
                sbSc.Append("\n\t\t\t\t\tMessageBox.ASSERT(\"Id错误\");\n\t\t\t\t}");
                for (int parmasIndex = 1; parmasIndex < parmas.Length; parmasIndex++)
                {
                    if (parmas[parmasIndex].StartsWith("i"))
                    {
                        sbSc.AppendFormat("\n\t\t\t\tDataDT.{0} = ccMath.atoi(tData[a++]);", parmas[parmasIndex]);
                    }
                    else if (parmas[parmasIndex].StartsWith("s"))
                    {
                        sbSc.AppendFormat("\n\t\t\t\tDataDT.{0} = tData[a++];", parmas[parmasIndex]);
                    }
                }
                sbSc.Append("\n\t\t\t\tSaveItem(DataDT);");
                sbSc.Append("\n\t\t\t}\n\t\t\tcatch {\n\t\t\t\tMessageBox.DEBUG(m_strRegDTName + \"脚本记录存在错误, \" + i);");
                sbSc.Append("\n\t\t\t\tcontinue;\n\t\t\t}\n\t\t}");
                sbSc.Append("\n\t}\n\n\t");
                sbSc.Append("\n}\n");
                #endregion
                File.WriteAllText(fileDtPath, sbDt.ToString());
                File.WriteAllText(fileScPath, sbSc.ToString());
            }
        }
        Debug.Log(string.Format("生成SC文件路径：{0}, DT文件路径: {1}, 生成脚本成功。", fileDtPath, fileScPath));
        AssetDatabase.Refresh();
    }

    public static void f_PackString(FileInfo file, string name, StringBuilder sbPackCsv)
    {
        //StreamReader sr = null;
        try
        {
            FileStream stream = file.OpenRead();
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            if (!string.IsNullOrEmpty(name))
            {
                sbPackCsv.Append(name + "1#QW");
            }
            int row = 0;
            string sFirstSheetName = excelReader.Name;
            do
            {
                // sheet name
                if (excelReader.Name.Equals(sFirstSheetName))// 固定规则，只读第一个表
                {
                    while (excelReader.Read())
                    {
                        row++;
                        if (row < 3)
                        {
                            continue;
                        }

                        bool isFirst = true;
                        bool isBreak = false;
                        for (int i = 0; i < excelReader.FieldCount; i++)
                        {
                            string value = excelReader.IsDBNull(i) ? "" : excelReader.GetString(i);
                            if (i == 0 && string.IsNullOrEmpty(value))
                            {
                                isBreak = true;
                                break;
                            }
                            if (isFirst)
                            {
                                isFirst = false;
                                sbPackCsv.Append(value);
                            }
                            else
                            {
                                sbPackCsv.Append("@,");
                                sbPackCsv.Append(value);
                            }
                        }

                        if (!isBreak)
                        {
                            sbPackCsv.Append("|");
                        }
                    }
                }
                else
                {
                    //Debug.Log(string.Format("{0}, {1}, {2}", sFirstSheetName, excelReader.Name, "break"));
                    break;
                }

            } while (excelReader.NextResult());
            sbPackCsv.Append("#1SD#");
            excelReader.Close();
        }
        catch (Exception e)
        {
            Debug.Log(name + "文件读写错误\n" + e.ToString());
            throw;
        }
    }

    [MenuItem("Tools/PackXlsxToScAndDt/Create One DT Server ")]
    public static void f_CreateOneDtServer()
    {
        //设置生成的C#类目录
        string curDir = Application.dataPath + "/GameScript/";
        //获取保存的表的路径
        string path = EditorUserSettings.GetConfigValue(EditorSelectOneCsvPath);
        path = EditorUtility.OpenFilePanel("Select Your Csv", path, "csv");
        if (path != "")
        {
            EditorUserSettings.SetConfigValue(EditorSelectOneCsvPath, path);
            if (!Directory.Exists(curDir))
            {
                Directory.CreateDirectory(curDir);
            }
            FileInfo file = new FileInfo(path);
            FileInfo[] files = new FileInfo[1];
            files[0] = file;
            f_WriteScriptServer(files, curDir, curDir);
        }
    }

    public static void f_WriteScriptServer(FileInfo[] files, string curDtDir, string curScDir)
    {
        StringBuilder sbDt = new StringBuilder();
        StringBuilder sbSc = new StringBuilder();
        string name = string.Empty;
        string classDtName = string.Empty;
        string DtPath = string.Empty;
        string fileDtPath = string.Empty;
        for (int i = 0; i < files.Length; i++)
        {
            using (StreamReader sr = files[i].OpenText())
            {
                string line1 = sr.ReadLine();
                string line2 = sr.ReadLine();

                sbDt.Remove(0, sbDt.Length);
                name = Path.GetFileNameWithoutExtension(files[i].ToString());
                classDtName = name + "DT.h";
                DtPath = "DT/ScDT/";
                fileDtPath = curDtDir + DtPath + classDtName;
                #region DT文件文本
                sbDt.Append("#pragma once\n");
                sbDt.AppendFormat("struct {0}DT", name);
                sbDt.Append(" {\n\t");

                string[] desc = line2.Split(new string[] { "," }, StringSplitOptions.None);
                string[] parmas = line1.Split(new string[] { "," }, StringSplitOptions.None);
                for (int parmasIndex = 0; parmasIndex < parmas.Length; parmasIndex++)
                {
                    if (parmasIndex <= desc.Length - 1)
                    {
                        sbDt.AppendFormat("\n\t///<summary>\n\t///{0}\n\t///</summary>\n\t", desc[parmasIndex]);
                    }
                    if (parmas[parmasIndex].StartsWith("i"))
                    {
                        sbDt.AppendFormat("int {0};\n\t", parmas[parmasIndex]);
                    }
                    else if (parmas[parmasIndex].StartsWith("s"))
                    {
                        sbDt.AppendFormat("string {0};\n\t", parmas[parmasIndex]);
                    }
                }
                sbDt.Append("\n}\n;");
                #endregion
                File.WriteAllText(fileDtPath, sbDt.ToString());
            }
        }
        Debug.Log(string.Format("Tạo đường dẫn tệp DT: {0}, Tạo tập lệnh thành công。", fileDtPath));
        AssetDatabase.Refresh();
    }
}
