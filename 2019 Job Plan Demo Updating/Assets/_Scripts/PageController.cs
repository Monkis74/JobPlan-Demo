using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System.Diagnostics;
using System.ComponentModel;
using System;

//Controller for all form pages.
// controls various form states eg: using a loaded file, new file, or emergency.

public class PageController : MonoBehaviour
{
    GameObject welcomePanel;
    GameObject finishPanel;
   // GameObject customLayoutPanel;
   // GameObject tlpickerPanel;
   // public GameObject pathPanel;
    public GameObject CSEPButton; // prefab button object to add to each page if a csep is requested to be used.
    SignOffID[] supervisorSignOff; // array of supervisor sign off objects.
   
    public static GameObject selectedPage; // the current page  that is active.
    int pageIndex; // the current page number in the pageList
    public static bool usingTouchScreen = false; // check whether user is using a touchscreen to type or not.
    public static Vector3 orgPagePos; // the starting page pos of the selectedPage, used for moving page up or down on InputfieldId script.
    public bool isEmergency = false; // true if emergency for is being used.
    public bool isCSEP = false; // true is using confined space entry permit page.
    public bool isCSEP2 = false; // true if a second CSEP is being used.
    List<GameObject> pageList = new List<GameObject>(); // list of all the capturable pages to write to pdf.
    public RawImage layoutImage; // the image of the traffic layout placeholder.
    public Texture2D layoutPlaceholder; // the texture of the layoutImage.
    public GameObject createCSEPButton;
    public GameObject viewCSEPButton;
    public GameObject createCSEP2Button;
    public GameObject viewCSEP2Button;    
    public Text myText;
    public GameObject finishSignOffButton;
    public GameObject trafficLayoutController;
    string username;
    
    // Use this for initialization
    void Start()
    {           
        username = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        UnityEngine.Debug.Log("Username is " + username);
        supervisorSignOff = Resources.FindObjectsOfTypeAll<SignOffID>();
        welcomePanel = GameObject.FindGameObjectWithTag("WelcomePanel");
        finishPanel = GameObject.FindGameObjectWithTag("FinishPanel");     
        finishPanel.SetActive(false);        

        // test if one drive is running on machine //
        //DISABLED FOR DEMO VERSION//
        //Process[] runningProcessByName = Process.GetProcessesByName("OneDrive");
        //if (runningProcessByName.Length == 0)
        //{
        //    string myAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        //    string filePath = "/Microsoft/OneDrive/OneDrive.exe";
        //    Application.OpenURL(myAppData + filePath);
        //    UnityEngine.Debug.Log(myAppData + filePath);
        //}
        //if (runningProcessByName.Length > 0)
        //{
        //    UnityEngine.Debug.Log("OneDrive already running");
        //}


        OkToCapture[] myPages = Resources.FindObjectsOfTypeAll<OkToCapture>();
        foreach (OkToCapture thisPage in myPages) {
            pageList.Add(thisPage.gameObject);
        }
        pageList = pageList.OrderBy(x => x.GetComponent<OkToCapture>().pageNum).ToList();
      
        foreach (GameObject thisPage in pageList)
        {   
            thisPage.SetActive(false);
        }
        //foreach (SignOffID thisSignOff in supervisorSignOff) {
        //    thisSignOff.gameObject.SetActive(false);
        //}
        System.GC.Collect();
        ResetForm();
        trafficLayoutController.SetActive(false);
    }

    // called by the Create Emergency Plan button on the weolcome panel. 
    public void CreateEmergencyPlan()
    {
        ContinuousSaveController.continuousSaveActive = true;
        isEmergency = true; 
        pageList[0].SetActive(true); // [0] is the OKtoCapture id of the emergency page
        welcomePanel.SetActive(false);
        if (finishPanel.activeSelf == true)
        {
            finishPanel.SetActive(false);
        }
        orgPagePos = pageList[0].transform.position; 
        pageIndex = 0;
        if (!SaveFile.loadedFile) // check wether a job plan was loaded and reset form if not.
        {
          //  Debug.Log("Create Emergency Not From Loaded File");
            ResetForm();
        }
    }
     // establish a new csep for the job plan.
    public void CreateCSEP() {
        if (isCSEP)
        {
            isCSEP2 = true;
            pageList[7].SetActive(true);
            pageList[1].SetActive(false);
            createCSEP2Button.SetActive(false);
            viewCSEP2Button.SetActive(true);
        }
        if (!isCSEP || !isCSEP2)
        {
            isCSEP = true;
            pageList[6].SetActive(true);
            pageList[1].SetActive(false);
            createCSEPButton.SetActive(false);
            viewCSEPButton.SetActive(true);
        }
       
    }
     // go back and view the csep after it has already been created.
    public void ViewCSEP(int page) {
        if (page == 1)
        {   
            pageList[6].SetActive(true);
            pageList[1].SetActive(false);
        }
        if (page == 2)
        {
            pageList[7].SetActive(true);
            pageList[1].SetActive(false);
        }
    }

    public void CloseCSEP() {
        pageList[1].SetActive(true);
        pageList[6].SetActive(false);
        pageList[7].SetActive(false);
    }


    public void LoadPlanWithCSEP() {
        if (isCSEP)
        {
            createCSEPButton.SetActive(false);
            viewCSEPButton.SetActive(true);
        }
        if (isCSEP2) {
            createCSEP2Button.SetActive(false);
            viewCSEP2Button.SetActive(true);
        }
    }

   
    // called from the welcome panel, create new job plan button
    public void CreateNewJobPLan()
    {
        ContinuousSaveController.continuousSaveActive = true;
        isEmergency = false;
        pageList[1].SetActive(true); // set to the first page of the main form.
        welcomePanel.SetActive(false);
        if (finishPanel.activeSelf == true)
        {
            finishPanel.SetActive(false);
        }
        orgPagePos = pageList[1].transform.position;
        pageIndex = 1;
        selectedPage = pageList[pageIndex].gameObject;
        if (!SaveFile.loadedFile) // check to see if a job plan was loaded, and reset form if false;
        {
           // Debug.Log("Reset Form was run from create new job plan");
            ResetForm();
        }
        System.GC.Collect(); 
    }
 
    // disable each page in the pagelist to hide them so the active object shows.
    public void ResetForPlanLoad() { // disable all pages so only page 1 will show after CreateNewJobPlan();
        foreach (GameObject thisPage in pageList) 
        {

            thisPage.SetActive(false);
        }

    }

    // move back to the welcome panel from various pages in the form.
    public void MainMenu() {
        ContinuousSaveController.continuousSaveActive = false;
        SaveFile.loadedForSignoff = false;
        SaveFile.loadedFile = false;
        foreach (GameObject thisPage in pageList)
        {
            thisPage.SetActive(false);
        }
        welcomePanel.SetActive(true);
        finishPanel.SetActive(false);
    }

    // move forward one page in the form.
    public void NextPage()
    {
        OnScreenKeyboard OSK = GameObject.FindObjectOfType<OnScreenKeyboard>();
        OSK.SetActive(false);
        for (int i = 0; i < pageList.Count; i++)
        {
            if (pageList[i].activeSelf == true)
            {
                pageIndex = i;
            }
        }
        //Debug.Log(pageList[pageIndex].name);
        pageList[pageIndex].SetActive(false);
        pageIndex++;
        //Debug.Log(pageList[pageIndex].name);
        pageList[pageIndex].SetActive(true);
        selectedPage = pageList[pageIndex].gameObject;
        orgPagePos = pageList[pageIndex].transform.position;
        System.GC.Collect();
        if (pageList[pageIndex].name == "Page2")
        {
            UnityEngine.Debug.Log("Page 2 selected");
            trafficLayoutController.SetActive(true);
            SelectLayoutController.layoutControllerInstance.BeginLoadImages("NextPage");
        }
        else
        {
            if (SelectLayoutController.layoutControllerInstance.isActiveAndEnabled)
            {
                SelectLayoutController.layoutControllerInstance.BeginFlushArrays();
            }
        }
    }



    // move to the previous page in the form.
    public void BackPage()
    {
        OnScreenKeyboard OSK = GameObject.FindObjectOfType<OnScreenKeyboard>();
        OSK.SetActive(false);
        for (int i = 0; i < pageList.Count; i++)
        {
            if (pageList[i].activeSelf == true)
            {
                pageIndex = i;
            }
        }
        //Debug.Log(pageList[pageIndex].name);
        pageList[pageIndex].SetActive(false);
        pageIndex--;
       // Debug.Log(pageList[pageIndex].name);
        pageList[pageIndex].SetActive(true);
        selectedPage = pageList[pageIndex].gameObject;
        orgPagePos = pageList[pageIndex].transform.position;
        System.GC.Collect();
        if (pageList[pageIndex].name == "Page2")
        {            
            trafficLayoutController.SetActive(true);
            SelectLayoutController.layoutControllerInstance.BeginLoadImages("BackPage");
        }
        else
        {
            if (SelectLayoutController.layoutControllerInstance.isActiveAndEnabled)
            {
                SelectLayoutController.layoutControllerInstance.BeginFlushArrays();
            }
        }

    }

    private void Update()
    {
        selectedPage = pageList[pageIndex];

        // see if user is using a touchscreen for input.
        if (Input.touchCount >= 1)
        { // should be 1, set at 0 for testing

            usingTouchScreen = true;
            //Debug.Log(" Is using TouchScreen");
        }
        // see if user is using a physical keyboard for input.
        if (Input.anyKeyDown) { 
            usingTouchScreen = false;
        }
    }

    public void OpenBook7()
    {
        string sharepointPath = SaveFile.sharepointPath;
        string folderPath = sharepointPath + "/Job Plans/FormData/MTOLayouts";
        string pdfName = "OTM-Book-7-2014";
        Application.OpenURL(folderPath + "/" + pdfName + ".pdf");
    }
        // close the app.
        public void QuitApplication()
    {
         Application.Quit();
        //if (!Application.isEditor) {
        //    //  PlayerPrefs.DeleteAll();
        //   // Screen.SetResolution(1366, 768, true);
            
        //    System.Diagnostics.Process.GetCurrentProcess().Kill();
        //}
    }

    // reset the bools that control the form loading, and scren capture behaviours, and move back to the welcome panel.
    public void CancelPlanType() {
        ContinuousSaveController.continuousSaveActive = false;
        SaveFile.loadedForSignoff = false;
        welcomePanel.SetActive(true);
        pageList[pageIndex].SetActive(false);
       // Debug.Log("pageList" + pageList[pageIndex].name);
        if (isEmergency)
        {
            isEmergency = false;
        }
        System.GC.Collect();


    }

    // called from the submit plan button to indicate you are done. etc..
    public void FinishPage(string finishText) {
        finishSignOffButton.SetActive(false);
        ContinuousSaveController.continuousSaveActive = false;
        finishPanel.SetActive(true);
        //Debug.Log(myText.name);
        myText.text = finishText;
        if (SaveFile.loadedForSignoff) {
            finishSignOffButton.SetActive(true);
        }
        
    }

    public void AddPage() //use to add more content pages to plan ie: extra traffic plan or job steps
    { 

    }


    // reset all the fields of the form to default state.
    public void ResetForm() {

        InputFieldID[] inputfields = Resources.FindObjectsOfTypeAll<InputFieldID>();
        foreach (InputFieldID input in inputfields)
        {
            input.gameObject.GetComponent<InputField>().text = "";
           // Debug.Log("Inputfield name = " + input.name);
            Text[] childText = input.GetComponentsInChildren<Text>();
          //  Debug.Log("Child TEXT objects = " + childText.Length);
            foreach (Text thisText in childText) {
               // Debug.Log("Text Nmae = " + thisText.name);
                if (thisText.name == "Placeholder") {
                   // Debug.Log("Placeholder is Gray");
                    thisText.color = new Color(.2f,.2f,.2f,.5f);
                }
                if (thisText.name == "Text")
                {
                    thisText.color = Color.black;
                  //  Debug.Log("Inputfield Text is black");

                }
            }
            
        }
        CheckBoxID[] checkboxes = Resources.FindObjectsOfTypeAll<CheckBoxID>();
        foreach (CheckBoxID box in checkboxes)
        {
            box.gameObject.GetComponent<Toggle>().isOn = false;
            box.gameObject.GetComponentInChildren<Text>().color = Color.black;
        }
        SignatureID[] signatures = Resources.FindObjectsOfTypeAll<SignatureID>();
        foreach (SignatureID signature in signatures) {
            signature.GetComponent<RawImage>().texture = null;
            signature.transform.GetChild(0).gameObject.SetActive(true);
            signature.bytesString = null;
        }
        DropdownID[] dropdowns = Resources.FindObjectsOfTypeAll<DropdownID>();
        foreach (DropdownID thisDropdown in dropdowns) {
          // Debug.Log("Dropdowns reset");
            thisDropdown.GetComponent<Dropdown>().value = 0;
            thisDropdown.GetComponent<Dropdown>().RefreshShownValue();
            thisDropdown.GetComponentInChildren<Text>().color = Color.black;
        }
        TimeID[] times = Resources.FindObjectsOfTypeAll<TimeID>();
        foreach (TimeID thisTime in times) {
            thisTime.GetComponent<GetTime>().ResetMainTime();
        }
        GetDate[] date = Resources.FindObjectsOfTypeAll<GetDate>(); // reset main date and CSEP dates to today.
        foreach (GetDate thisdate in date) {
            thisdate.ResetDate();
        }
        TimeStampID[] timeStamps = Resources.FindObjectsOfTypeAll<TimeStampID>(); // reset the time stamps to blank
        foreach (TimeStampID thisStamp in timeStamps) {
            thisStamp.GetComponent<Text>().text = "";
        }
        //date[0].ResetDate();

        ChosenLayout[] myLayout = Resources.FindObjectsOfTypeAll<ChosenLayout>();
        myLayout[0].bytesString = null;
        layoutImage.texture = layoutPlaceholder;
        isCSEP = false;
        isCSEP2 = false;
        createCSEPButton.SetActive(true);
        viewCSEPButton.SetActive(false);
        createCSEP2Button.SetActive(true);
        viewCSEP2Button.SetActive(false);
        inputfields = null;
        checkboxes = null;
        signatures = null;
        dropdowns = null;
        times = null;
        date = null;
        System.GC.Collect();

    }

    
    

}
  



    

