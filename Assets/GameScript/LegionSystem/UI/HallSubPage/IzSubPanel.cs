using UnityEngine;
using System.Collections;

interface IzSubPanel
{
    void f_Init(UIFramwork parentUI);
    void f_Open();
    void f_Close();
    void f_RegEvent();
    void f_UnregEvent();
}
