using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public static string SELECTED_COLOR = "FFFE00";
    
    public static string OTHER_COLOR = "B0B0B0";
    public Text btn1; 

    public Text btn2; 

    public Text btn3; 

    public Text btn4;

    public Text btn5; 

    public void SetButtonColor(int index)
    {
        if(index == 1)
        {
            btn1.color = Color.yellow;
            btn2.color = Color.white;
            btn3.color = Color.white;
            btn4.color = Color.white;
            btn5.color = Color.white;

        }

        if (index == 2)
        {
            btn1.color = Color.white;
            btn2.color = Color.yellow;
            btn3.color = Color.white;
            btn4.color = Color.white;
            btn5.color = Color.white;

        }

        if (index == 3)
        {
            btn1.color = Color.white;
            btn2.color = Color.white;
            btn3.color = Color.yellow;
            btn4.color = Color.white;
            btn5.color = Color.white;

        }

        if (index == 4)
        {
            btn1.color = Color.white;
            btn2.color = Color.white;
            btn3.color = Color.white;
            btn4.color = Color.yellow;
            btn5.color = Color.white;

        }

        if (index == 5)
        {
            btn1.color = Color.white;
            btn2.color = Color.white;
            btn3.color = Color.white;
            btn4.color = Color.white;
            btn5.color = Color.yellow;
        }
    }

}
