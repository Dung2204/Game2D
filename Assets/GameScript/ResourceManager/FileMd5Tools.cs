using UnityEngine;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class FileMd5Tools
{


    public static string f_EncryptAndCaculateFileMD5(string path, string strPwd)
    {
        byte[] _bPwd = System.Text.Encoding.Default.GetBytes(strPwd); 

        //Caculate file's MD5 code
        if (File.Exists(path))
        {
            FileStream file = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
            byte[] bBuf = new byte[100];
            file.Read(bBuf, 0, 100);
            int iKeyIndex = 0;
            for (int i = 0; i < bBuf.Length; i++)
            {
                if (iKeyIndex == _bPwd.Length)
                {
                    iKeyIndex = 0;
                }
                bBuf[i] = (byte)(bBuf[i] ^ _bPwd[iKeyIndex]);
                iKeyIndex++;
            }
            file.Seek(0, SeekOrigin.Begin);
            file.Write(bBuf, 0, 100);
            file.Flush();
            file.Close();

            file = new FileStream(path, FileMode.Open, FileAccess.Read);
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(file);
            file.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        else
        {
            return string.Empty;
        }
    }


    public static void f_DecodeAndCaculateFileMD5(string strPwd, ref byte[] bBuf)
    {
        return;

        int iKeyIndex = 0;
        byte[] _bPwd = System.Text.Encoding.Default.GetBytes(strPwd);
        int iIndex = 100;
        if (bBuf.Length < 100)
        {
            iIndex = bBuf.Length;
        }
        for (int i = 0; i < iIndex; i++)
        {
            if (iKeyIndex == _bPwd.Length)
            {
                iKeyIndex = 0;
            }
            bBuf[i] = (byte)(bBuf[i] ^ _bPwd[iKeyIndex]);
            iKeyIndex++;
        }
    }


}