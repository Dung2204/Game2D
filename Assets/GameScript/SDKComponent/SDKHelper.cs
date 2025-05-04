using UnityEngine;
using System.Collections;

public class SDKHelper
{
#if VER0
    //勇闯马管包
    //空表示不需要支付热云功能
    public static string REYUN_KEY = "";

    public static int APP_ID = 1007;
    public static int CHANNEL_ID = 10071;
    public static string APP_KEY = "jAWtBVCNMpNnzDOJ";

    public static string bundleIdentifier = "com.lanmao.ycsg";
public static string m_strIosPccaccyInfor = "3,6,com.lanmao.ycsg.p1,60KNB#4.45,com.lanmao.ycsg.p2,450KNB#5,68,com.lanmao.ycsg.p3,680KNB#6,118, com.lanmao.ycsg.p4,1180KNB#7,198,com.lanmao.ycsg.p5,1980KNB#8,348,com.lanmao.ycsg.p6,3480KNB#9,648,com.lanmao.ycsg.p7,6480KNB#10,1998, com.lanmao.ycsg.p8,19980KNB#1.25,com.lanmao.ycsg.p9,monthly card#2.68,com.lanmao.ycsg.p11,VIP card";
#elif VER1
    //勇闯马甲包1_三国志神将之路
    public static string REYUN_KEY = "9f78cf1a9c6177bf488df6daefbb83d7";

    public static int APP_ID = 1029;
    public static int CHANNEL_ID = 10291;
    public static string APP_KEY = "2TAKFMGaGt7hUKf5";

    public static string bundleIdentifier = "com.lmkj.sgzsjzl";
public static string m_strIosPccaccyInfor = "3,6KNB,com.lmkj.sgzsjzl.6,60KNB#4,45KNB,com.lmkj.sgzsjzl.45,450KNB#5,68KNB,com.lmkj.sgzsjzl.68,680KNB#6,118KNB, .lmkj.sgzsjzl.118,1180KNB#7,198KNB,com.lmkj.sgzsjzl.198,1980KNB#8,348KNB,com.lmkj.sgzsjzl.348,3480KNB#9,648KNB,com.lm#10kzl.NB8s 1998KNB,com.lmkj.sgzsjzl.1998,19980KNB#1.25KNB,com.lmkj.sgzsjzl.25,monthly card#2.68KNB,com.lmkj.sgzsjzl.68,VIP card";
#elif VER2
    //勇闯马甲包2_三国之召唤猛将
    public static string REYUN_KEY = "dbdda3c86ade64bb64f47b50f0aa5cc9";

    public static int APP_ID = 1030;
    public static int CHANNEL_ID = 10301;
    public static string APP_KEY = "QcXKYvdUPxQP1LOD";

    public static string bundleIdentifier = "com.lmyc.sgzzhmj";
public static string m_strIosPccaccyInfor = "3,6KNB,com.lmyc.sgzzhmj.6,60KNB#4.45KNB,com.lmyc.sgzzhmj.45,450KNB#5,68KNB,com.lmyc.sgzzhmj.68,680KNB#6,118KNB,com .lmyc.sgzzhmj.118,1180KNB#7,198KNB,com.lmyc.sgzzhmj.198,1980KNB#8,348KNB,com.lmyc.sgzzhmj.348,3480KNB#9,648KNB,com.lmyc.s64#10zzhmj.NB 1998KNB,com.lmyc.sgzzhmj.1998,19980KNB#1.25KNB,com.lmyc.sgzzhmj.25,monthly card#2.68KNB,com.lmyc.sgzzhmj.68,VIP card";
#elif VER3
    //勇闯马甲包3-三国q传手游

    public static string REYUN_KEY = "";

    public static int APP_ID = 1028;
    public static int CHANNEL_ID = 10281;
    public static string APP_KEY = "x2AVJzYjw6KD5C9C";

    public static string bundleIdentifier = "com.toogames.sgqzsy";
    public static string m_strIosPccaccyInfor = "3,6KNB,com.toogames.sgqzsy.6#4,45KNB,com.toogames.sgqzsy.45#5,68KNB,com.toogames.sgqzsy.68#6,118KNB,com.toogames.sgqzsy.118#7,198KNB,com.toogames.sgqzsy.198#8,348KNB,com.toogames.sgqzsy.348#9,648KNB,com.toogames.sgqzsy.648#10,1998KNB,com.toogames.sgqzsy.1998#1,25KNB,com.toogames.sgqzsy.25#2,68KNB,com.toogames.sgqzsy.zzk68";

#elif VER4
    //勇闯马甲包4-口袋真三国
    public static string REYUN_KEY = "";

    public static int APP_ID = 1027;
    public static int CHANNEL_ID = 10271;
    public static string APP_KEY = "V5hIXKhITVAv2QzC";
    
    public static string bundleIdentifier = "com.legame.kdzsg";
public static string m_strIosPccaccyInfor = "3,6KNB,com.kexinzhang.kdzsg.6,60KNB#4.45KNB,com.kexinzhang.kdzsg.45,450KNB#5,68KNB,com.kexinzhang.kdzsg.68,680KNB#6,118KNB,com .kexinzhang.kdzsg.118,1180KNB#7,198KNB,com.kexinzhang.kdzsg.198,1980KNB#8,348KNB,com.kexinzhang.kdzsg.348,3480KNB#9,648KNB,com.kexinzhang.k8dzsg.NB 1998KNB,com.kexinzhang.kdzsg.1998,19880KNB#1.25KNB,com.kexinzhang.kdzsg.25,25KNBmonth card#2.68KNB,com.kexinzhang.kdzsg.zzk68,VIP card";
    
#elif Y_SDK
    public static string REYUN_KEY = "09611384f451f34b64ae31262d6e2d1d";

    public static int APP_ID = 1031;
    public static int CHANNEL_ID = 10311;
    public static string APP_KEY = "";

    public static string bundleIdentifier = "";
    public static string m_strIosPccaccyInfor = "";
#elif LCAT_SDK
    public static string REYUN_KEY = "8faa49bc774de48cffadb64e58a793d4";
    public static int APP_ID
    {
        get
        {
            return LcatSDKHelper.APP_ID;
        }
    }
    public static int CHANNEL_ID
    {
        get
        {
            return LcatSDKHelper.CHANNEL_ID;
        }
    } 
    public static string APP_KEY = "";

    public static string bundleIdentifier = "";
    public static string m_strIosPccaccyInfor = "";
#else
    public static string REYUN_KEY = "3ea944a8245fc66f0d69b68ff38565f1";

    public static int APP_ID = 0;
    public static int CHANNEL_ID = 0;
    public static string APP_KEY = "";

    public static string bundleIdentifier = "";
    public static string m_strIosPccaccyInfor = "";

#endif



}
