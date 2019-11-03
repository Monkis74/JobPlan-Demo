using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Diagnostics;
using UnityEngine.EventSystems;

using Microsoft.Win32;

// provide a unique id number for each input field sor saving and loading each fields state.
// controls the onscreen keyboard if using touchscreen.
// will move up the focused input field if its hidden by the onscreen keyboard.

//[ExecuteInEditMode]
public class InputFieldID : MonoBehaviour, IUpdateSelectedHandler {

    public int objID;
    InputField myInputField;
    OnScreenKeyboard[] OSK; // controller for the onscreen keyboard to activate or de-activate it.
    public static bool keyboardActive; // checked on CkeckKeyboard script
    PageController pageController;
    public static Rect kbRect; // get from checkkeyboard
    Rect myRect; // rect of this input field
    Vector3 myrectPos;
    Transform myPageTrans; // transform of parent page (used for moving page above keyboard)
    public static bool screenMoved = false; // check if screen is moved up
    public int numofInputs; // total of input fields contaiined in the project.
    InputFieldID[] amount; // array of all the input fields.
    public bool isTaken; // test if Id number is already used or not, see in inspector.
    public static InputField selectedField;




    void Start()
    {
        amount = FindObjectsOfType<InputFieldID>();
        numofInputs = amount.Length;
        myInputField = GetComponent<InputField>();
        OSK = Resources.FindObjectsOfTypeAll<OnScreenKeyboard>(); pageController = GameObject.FindObjectOfType<PageController>();
        myRect = GetComponent<RectTransform>().rect;
        
        myrectPos = Camera.main.WorldToScreenPoint(transform.position);

        // myPageTrans = gameObject.transform.parent.parent.GetComponent<Transform>();
        if (GetComponentInParent<OkToCapture>())
        {
            myPageTrans = GetComponentInParent<OkToCapture>().gameObject.transform;
        }
    }

    public void OnUpdateSelected(BaseEventData data)
    {
        //Debug.Log("Dropdown updated");
        if (SaveFile.loadedForSignoff)
        {
            Text[] childText = GetComponentsInChildren<Text>();
            foreach (Text thisText in childText)
            {
                thisText.color = Color.red;
            }
            //GetComponentInChildren<Text>().color = Color.red;
            //UnityEngine.Debug.Log("Using red font");
            return;
        }
        else
        {
            Text[] childText = GetComponentsInChildren<Text>();
            foreach (Text thisText in childText)
            {
                if (thisText.name == "Placeholder") {
                    thisText.color = new Color(.2f,.2f,.2f,.5f);
                }
                if (thisText.name == "Text") {
                    thisText.color = Color.black;
                }
                
            }
            //GetComponentInChildren<Text>().color = Color.red;
            //UnityEngine.Debug.Log("Using black font");
        }
        if (GetComponent<InputField>().isFocused)
        {
            selectedField = myInputField;
            OSK[0].SetFocus(selectedField);
        }
    }


    // if the focused input field is hidden by the onscreen keyboard, move the page up accordingly to have the input in view.
    void MoveScreenUp() {
        if (myPageTrans != null)
        {
            Vector3 parentPos = new Vector3(myPageTrans.transform.position.x, myPageTrans.transform.position.y - 10);
            Vector3 parentscreenPos = Camera.main.WorldToScreenPoint(parentPos);
            //UnityEngine.Debug.Log("parentscreenPos " + parentscreenPos);
            float moveY = ((Screen.height - 250f) - myrectPos.y);
            parentscreenPos = new Vector3(parentscreenPos.x, parentscreenPos.y + moveY, parentscreenPos.z);
           // UnityEngine.Debug.Log("parentscreenPos Adjusted " + parentscreenPos);
            Vector3 movedParentPos = Camera.main.ScreenToWorldPoint(parentscreenPos);
            myPageTrans.transform.position = new Vector3(movedParentPos.x, movedParentPos.y, movedParentPos.z);
            screenMoved = true;
        }
    }

    // move the page back down when onscreen keybpard is gone.
   public void MoveScreenDown() {
        if (myPageTrans != null)
        {
            myPageTrans.position = PageController.orgPagePos;
            screenMoved = false;
        }
    }
    
    // check wether or not to show the keyboard.
     void Update()
    {
        

        if (PageController.usingTouchScreen && myInputField.isFocused && !keyboardActive)
        {

            keyboardActive = true;
            OSK[0].SetActiveFocus(myInputField);
            UnityEngine.Debug.Log("Input Field should be calling OSK");
            kbRect = new Rect(OSK[0].GetComponent<RectTransform>().rect);
            //UnityEngine.Debug.Log("kbRect" + kbRect);              

            if ((myrectPos.y <= 250) && !screenMoved)
            {

                MoveScreenUp();
            }

        }

        //if (Input.GetKeyDown(KeyCode.Return) && keyboardActive)
        //{
        //    myInputField.isFocused.Equals(false);
        //    //vk.HideTouchKeyboard(); // pressing enter hides the keybord already, no need to duplicate.
        //    MoveScreenDown();
        //}

        if (!keyboardActive && screenMoved)
        {
            MoveScreenDown();
        }
        //bool testfieldsfocused = false;
        //foreach (InputFieldID thisField in amount)
        //{
        //    if (thisField.GetComponent<InputField>().isFocused)
        //    {
        //        testfieldsfocused = true;
        //    }
        //}
        //if (!testfieldsfocused && vk.vkShown == true)
        //{
        //    myInputField.isFocused.Equals(false);
        //    vk.HideTouchKeyboard();
        //    if (screenMoved)
        //    {
        //        MoveScreenDown();
        //    }
        //}
    }


    // set the text of an ID'ed input field from a loaded file.
    public void SetText(string myText) {
        this.gameObject.GetComponent<InputField>().text = myText;
       // UnityEngine.Debug.Log("My Text for input field = " + myText);
    }


   
}
