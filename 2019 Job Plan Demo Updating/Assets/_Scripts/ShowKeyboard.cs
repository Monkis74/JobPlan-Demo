using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowKeyboard : MonoBehaviour
{

    VirtualKeyboard vk = new VirtualKeyboard();

    public void OpenKeyboard()
    {
        {
            vk.ShowTouchKeyboard();
        }
    }

    public void CloseKeyboard()
    {
        {
            vk.HideTouchKeyboard();
        }
    }
}
