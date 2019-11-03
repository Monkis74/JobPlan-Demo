using System;
using System.Diagnostics;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

// check for a keyboard or touchscreen input.

public class CheckKeyboard : MonoBehaviour
{
    class NativeMethods
    {
        // http://msdn.microsoft.com/en-us/library/ms633519(VS.85).aspx
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        // http://msdn.microsoft.com/en-us/library/a5ch4fda(VS.80).aspx
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
    }
    public static Rect GetKeyboardRect()
    { // find the rect of the onscreen keyboard.
        Rect kbRect = new Rect();
        IntPtr hWnd = GetKeyboardWindowHandle();
        NativeMethods.RECT rect = new NativeMethods.RECT();
        if (NativeMethods.GetWindowRect(hWnd, ref rect))
        {
            kbRect = new Rect(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
        }

        return kbRect;
    }

    private void Update() // check if onscreen keyboard is active, and pass the keyboard rect to the focused input field
    {
        InputFieldID.keyboardActive = IsKeyboardVisible();

        if (IsKeyboardVisible())
        {
            InputFieldID.kbRect = GetKeyboardRect();
        }
    }

    /// <summary>
    /// The window is disabled. See http://msdn.microsoft.com/en-gb/library/windows/desktop/ms632600(v=vs.85).aspx.
    /// </summary>

    /// </summary>
    public const UInt32 WS_DISABLED = 0x8000000;
    public const UInt32 WS_VISIBLE = 0X94000000;
    /// <summary>
    /// Specifies we wish to retrieve window styles.
    /// </summary>
    public const int GWL_STYLE = -16;

    [DllImport("user32.dll")]
    public static extern IntPtr FindWindow(String sClassName, String sAppName);

    [DllImport("user32.dll", SetLastError = true)]
    static extern UInt32 GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle); // trying to get keyboard size

    //Rect r = new Rect();


    static void Main(string[] args)
    {
        // Crappy loop to poll window state.
        while (true)
        {
            if (IsKeyboardVisible())
            {
                // UnityEngine. Debug.Log("keyboard is visible");
                InputFieldID.keyboardActive = true;

            }
            else
            {
                //  UnityEngine.Debug.Log("keyboard is NOT visible");
                InputFieldID.keyboardActive = false;
                if (InputFieldID.screenMoved)
                {
                    
                    InputFieldID myInputfield = GameObject.FindObjectOfType<InputFieldID>();
                    InputField[] fields = Resources.FindObjectsOfTypeAll<InputField>();
                    foreach (InputField F in fields) {
                        if (F.isFocused) {
                            F.isFocused.Equals(false);
                        }
                    }

                    myInputfield.MoveScreenDown();
                    
                    // UnityEngine.Debug.Log("Keyboard closed, Should have moved screen down");
                }
            }

            Thread.Sleep(1000);
        }
    }

    /// <summary>
    /// Gets the window handler for the virtual keyboard.
    /// </summary>
    /// <returns>The handle.</returns>
    public static IntPtr GetKeyboardWindowHandle()
    {
        return FindWindow("IPTip_Main_Window", null);
    }

    /// <summary>
    /// Checks to see if the virtual keyboard is visible.
    /// </summary>
    /// <returns>True if visible.</returns>
    public static bool IsKeyboardVisible()
    {
        IntPtr keyboardHandle = GetKeyboardWindowHandle();

        bool visible = false;

        if (keyboardHandle != IntPtr.Zero)
        {
            UInt32 style = GetWindowLong(keyboardHandle, GWL_STYLE);
            visible = (style == WS_VISIBLE);
        }

        return visible;
    }

}