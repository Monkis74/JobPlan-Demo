using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Linq;
using System.Diagnostics;
using System.ComponentModel;

public class SaveFile : MonoBehaviour {
   // string[] savedFiles; // the array of the saved files at a specified location.
    public static string saveName; // the name of the file to be saved
    public static string loadName; // the name of the file to be loaded.
    public string deleteName; // the name of the file to be deleted.
    public GameObject saveNameGO; // the text componment of the input field on the save panel
    public GameObject savePanelGO; // the save panel 
    public GameObject newSavePanel; // the confirm new save file panel
    public GameObject overwriteSavePanel; // the confirm overwrite selected file save panel
    public GameObject invalidCharPanel; // the notice that the filename chosen contains invalid characters for saving.
    public GameObject loadPanel; // the load file panel
    public GameObject deletePanel; // the delete file panel
    public GameObject deleteConfirmPanel; // the confirm deleting selected file panel
    public GameObject savedFileButton; // the button to populate the save file list with
    public GameObject loadFileButton; // the button to poulate the load file list with
    public GameObject errorPanel; // the panel to catch errors with saving and loading files.
    public GameObject pathPanel; // Panel to set location of shared drive (sharepoint or onedrive etc...)
    public GameObject checkSyncFilesPanel; // panel to manual check if late submitted files have already been signed off by comparing against log.
    public GameObject foremanReminderPanel; // the panel to remind to select a forman name during submission.
    public GameObject supervisorReminderPanel; // the panel to remind to select a spervisor during submission.
    public GameObject SavingErrorPanel; // panel that appears when a file did not get created during a save/submission.
    public CheckBoxID[] checkBoxes; // hold all the checkbox objects
    public InputFieldID[] inputfields; // hold all the input fields.
    public SignatureID[] sigimages; // hold all the signature images;
    public DropdownID[] dropdowns; // aray of all the dropdowns
    public GetDate[] date; // array of all the date objects
    public TimeID[] times; // arra of all the time objects
    public TimeStampID[] timeStamps; // array of all the time stamp objects
    public ChosenLayout[] myLayout; // array of the one layoutimage, used to be able to find wen disabled.
    public SignOffID[] signOffs; // array of the supervisor sign offs 
    public List<CSEPNumberID> CSEPList = new List<CSEPNumberID>(); // sorted list of csep numbers;
    public CSEPNumberID[] CSEPNUm; // there should be only one so index to use is 0
    LoadFileCenterSnap loadcentersnap; // the script to control the loading files panel behaviour.
    SaveFileCenterSnap savecentersnap; // the script to control the save files panel behaviour
    PageController pageController; // the pagecontroller
    public string dataPath; // the specified path to retreive data from for loading, saving or deleting.
    public string loadedFilePath; // the path of the loadedfile selected.
    string myPrep; // the name of the preparer of the form to be used for part of the submittal file name.
    string mySup; // the name of the supervisor of the crew for sign off.
    public static bool loadedFile = false; // true if a saved file was loaded to populate the form. 
    public static bool loadedForSignoff = false; // true if file has been loaded from a submitted plan save for signoff.
    public bool savingTemplate = false; // true if saving a template.
    public Text saveGreeting; // the text element of the greeting on the save file panel.
    public static string sharepointPath;
    const string SHAREPOINT_PATH = "sharepointpath";
    GameObject inputSharePointText;
    Text displaySharepointText;
    public bool fileisSaved = false;

    void Start()
    {
        sharepointPath = PlayerPrefs.GetString(SHAREPOINT_PATH);
        UnityEngine.Debug.Log("Sharepointpath is " + sharepointPath);
        //if (sharepointPath == null || sharepointPath == "") {
        //    SetSharepointPath();
        //}
        pageController = GameObject.Find("PageController").GetComponent<PageController>();
        newSavePanel.SetActive(false);
        overwriteSavePanel.SetActive(false);
        invalidCharPanel.SetActive(false);
        savePanelGO.SetActive(false);
        loadPanel.SetActive(false);
        deletePanel.SetActive(false);
        deleteConfirmPanel.SetActive(false);
        errorPanel.SetActive(false);
        SavingErrorPanel.SetActive(false);   
    }

    public void GetArrays()
    {
        checkBoxes = Resources.FindObjectsOfTypeAll<CheckBoxID>();
        inputfields = Resources.FindObjectsOfTypeAll<InputFieldID>();
        sigimages = Resources.FindObjectsOfTypeAll<SignatureID>();
        dropdowns = Resources.FindObjectsOfTypeAll<DropdownID>();
        times = Resources.FindObjectsOfTypeAll<TimeID>();
        timeStamps = Resources.FindObjectsOfTypeAll<TimeStampID>();
        date = Resources.FindObjectsOfTypeAll<GetDate>();
        myLayout = Resources.FindObjectsOfTypeAll<ChosenLayout>();
        signOffs = Resources.FindObjectsOfTypeAll<SignOffID>();
        CSEPNUm = Resources.FindObjectsOfTypeAll<CSEPNumberID>();
        foreach (CSEPNumberID thisnUM in CSEPNUm)
        {
            for (int i = 0; i < CSEPNUm.Length; i++)
            {
                if (CSEPNUm[i].ObjID == i)
                {
                    CSEPList.Add(CSEPNUm[i]);
                }
            }
        }
    }

    public void KillArrays()
    {
        checkBoxes = null;
        inputfields = null;
        sigimages = null;
        dropdowns = null;
        times = null;
        date = null;
        myLayout = null;
        signOffs = null;
        CSEPNUm = null;
        System.GC.Collect();
    }

    // Template files are in C:\\JobPlanTemplates
    // Workinking Files are in C:\\JobPlanWorkingFiles
    // Submitted plans are in C:\\SharePoint\Operations - Documents\Job Plans\SubmittedPlans

    // set the data path for all save / load / delete panel files to populate 
    public void OpenSharePointPath()
    {
        Transform myParent = GameObject.Find("Canvas").transform;
        GameObject PathPanel = Instantiate(pathPanel) as GameObject;
        PathPanel.transform.SetParent(myParent, false);
        PathPanel.transform.localScale = new Vector3(1, 1, 1); 
        PathPanel.name = "PathPanel";
        PathPanel.SetActive(true);
        displaySharepointText = GameObject.Find("Canvas/PathPanel/CurrentPath").GetComponent<Text>();
        displaySharepointText.text = "Current Sharepoint Path Is: " + PlayerPrefs.GetString(SHAREPOINT_PATH);
    }

    public void SetSharepointPath()
    {
        inputSharePointText = GameObject.Find("Canvas/PathPanel/SharePointInputField");
        sharepointPath = inputSharePointText.GetComponent<InputField>().text;
        displaySharepointText = GameObject.Find("Canvas/PathPanel/CurrentPath").GetComponent<Text>();
        displaySharepointText.text = "Current Sharepoint Path Is: " + PlayerPrefs.GetString(SHAREPOINT_PATH);
        PlayerPrefs.SetString(SHAREPOINT_PATH, sharepointPath);
        DestroyPanel();
     }

    public void OpenCheckSyncFilesPanel()
    {
        Transform myParent = GameObject.Find("Canvas").transform;
        GameObject CheckSyncFilesPanel = Instantiate(checkSyncFilesPanel) as GameObject;
        CheckSyncFilesPanel.transform.SetParent(myParent, false);
        CheckSyncFilesPanel.transform.localScale = new Vector3(1, 1, 1);
        CheckSyncFilesPanel.name = "CheckSyncFilesPanel";
        CheckSyncFilesPanel.SetActive(true);
    }

    public void SetDataPath(string myPath)
    { 
        dataPath = myPath;
        if (!Directory.Exists(dataPath))
        {
            //Debug.Log("Deirectory does not exist");
            Directory.CreateDirectory(dataPath);
        }       
    }
    public void SetSubmissionPath(string myPath)
    {
        ContinuousSaveController.continuousSaveActive = false;
        sharepointPath = PlayerPrefs.GetString(SHAREPOINT_PATH);
        dataPath = sharepointPath + myPath;
       
        if (!Directory.Exists(dataPath))
        {
           Directory.CreateDirectory(dataPath);
        }
    }


    // take in savename input if input manually.
    public void SetSaveName() 
    { 
        saveName = saveNameGO.GetComponent<Text>().text;
        if (saveName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
        {
            InvalidCharacters();
        }
    }

    public void InvalidCharacters()
    {
        invalidCharPanel.SetActive(true);        
        Text noticeText = GameObject.Find("Canvas/SavePanel/InvalidCharPanel/NoticeText").GetComponent<Text>();
        noticeText.text = "Your Filename contains one or more invalid characters \n EG " + @": * ? | < > / \ :" + " \" " ;
    }

    // set the name of the file to load
    public void SetLoadName(string name)  
    { 
        loadName = name.ToString();
        if (dataPath.Contains("Archive"))
        {
            Application.OpenURL(dataPath + "/" + loadName + ".pdf");
            return;
        }
        else
        {
            LoadFile();
        }
    }

    // set that a submitted plan is loaded for signoff, show the supervison sign off fields.
    public void SetLoadedForSignoff()
    { 
        loadedForSignoff = true;
    }

    // Set true if the file is being saved as a template.
    public void SetSavingTemplate()
    {
        savingTemplate = true;
    }

    // open the delete files panel for the dataPath specified  
    public void ShowDeletePanel() 
    {
        ContinuousSaveController.continuousSaveActive = false;
        deletePanel.SetActive(true);
        DeleteFileCenterSnap deletecentersnap = FindObjectOfType<DeleteFileCenterSnap>();
        deletecentersnap.StartRepopulate(dataPath);
        savePanelGO.SetActive(false);
        DestroySaveList();
       
    }

    // delete the selected file called from deleteconfirmationpanel OK button.
    public void DeleteFile()
    { 
        ContinuousSaveController.continuousSaveActive = false;       
        FileInfo fi = new System.IO.FileInfo(dataPath + "/" + deleteName + ".dat");
        UnityEngine.Debug.Log(fi);
        if (fi.Exists)
        {
            fi.Delete();
            fi.Refresh();
            while (fi.Exists)
            {
                System.Threading.Thread.Sleep(100);
                fi.Refresh();
            }
        }
        DeleteFileCenterSnap deletecentersnap = FindObjectOfType<DeleteFileCenterSnap>();
        deletecentersnap.StartRepopulate(dataPath);
        deleteConfirmPanel.SetActive(false);
    }

    // open the delete confirmation panel, called from the selected DeleteFileButton.
    public void ShowDeleteFileConfirmation(string file)
    {
        deleteConfirmPanel.SetActive(true);
        Text deleteText = GameObject.Find("Canvas/DeletePanel/DeleteConfirmPanel/DeleteFileLabel").GetComponent<Text>();
        deleteText.text = "Would you like to delete the saved file: " + file;
        deleteName = file;
    }

    // activate the saving dialogue panel, and populate the list of saved files from the data path specified.
    // calledfrom SaveTempate, and SaveCurrent buttons.
    public void ShowSavePanel()
    { 
        ContinuousSaveController.continuousSaveActive = false;
        savePanelGO.gameObject.SetActive(true);
        savecentersnap = FindObjectOfType<SaveFileCenterSnap>();
        savecentersnap.StartRepopulate(dataPath);
        if (savingTemplate) {
            saveGreeting.text = "Please enter a new template name or choose an existing one to overwrite.";
        }
        if (!savingTemplate) {
            saveGreeting.text = "Please enter a new save name or choose an existing one to overwrite.";
        }
       
    }

    // destroy all SavedFileButton objects to allow list to repopulate when called again.
    public void DestroySaveList() 
    { 
        SavedFileButton[] saveButtons = FindObjectsOfType<SavedFileButton>();
        foreach (SavedFileButton thisButton in saveButtons)
        {           
            Destroy(thisButton.gameObject);
        }
    }

    // activate the load dialogue panel
    public void ShowLoadPanel()
    { 
        loadPanel.SetActive(true);
        loadcentersnap = FindObjectOfType<LoadFileCenterSnap>();
        StartCoroutine(loadcentersnap.RepopulateList(dataPath));
    }

    // Show the panel to confirm that file has been saved. called from SavePanel-SaveNewFileButton gameobject.
    public void ShowNewSavePanel()
    {
        ContinuousSaveController.continuousSaveActive = false;
        saveName = saveNameGO.GetComponent<Text>().text;
        if (saveNameGO.GetComponent<Text>().text == "" )
        {
            return;
        }
        if (saveName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
        {
            InvalidCharacters();
            return;
        }
        else {
            newSavePanel.SetActive(true);
            Text newSaveLabel = GameObject.Find("Canvas/SavePanel/NewSavePanel/NewSaveLabel").GetComponent<Text>();
            newSaveLabel.text = "Confirm saving new file: " + saveName; 
        }
      }

    // hide a panel using the cancel button. if panel is a subpanel of the gameobject you wish to close, do not use this.
    public void HidePanel()
    {       
        EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.SetActive(false);
        loadedForSignoff = false;
        savingTemplate = false;
    }

    // Destroys the currently active panel, use for panels that are called from instatiation. do not use for panels that should be de-activated.
    public void DestroyPanel()
    {
        Destroy(EventSystem.current.currentSelectedGameObject.transform.parent.gameObject);
    }

    // Opens confirmation panel to overwrite and existing saved file.
    public void ShowOverwriteSavePanel(string overwriteText) {        
        saveName = overwriteText;
        overwriteSavePanel.SetActive(true);
        Text overwriteSaveLabel = GameObject.Find("Canvas/SavePanel/OverwriteSavePanel/OverwriteSaveLabel").GetComponent<Text>();
        overwriteSaveLabel.text = "Would you like to overwrite existing save: " + saveName;
    }

    // do not need anymore?
    public void SelectedExistingSave()
    {             
        saveName = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text;       
        saveNameGO.GetComponent<Text>().text = saveName;       
    }

    //public void GetSaves() { //populate save slots with save files.
    //    savedFiles = Directory.GetFiles(dataPath, "*.dat");
    //    // add stuff to populate save slots...
    //}

    // initiate the Co-routine below, should be called form the Submit Plan button.
    public void StartTimeChecks() {
        StartCoroutine(CheckTimes());
    }
    public IEnumerator CheckTimes() { //checks for foreman and supervisor names selected, and any time that is time on, has a time off with it.
        dropdowns = Resources.FindObjectsOfTypeAll<DropdownID>();
        foreach (DropdownID thisdropdown in dropdowns)
        {
            if (!pageController.isEmergency)
            {
                if (thisdropdown.objID == 15) // the foreman dropdown.
                {
                    if (thisdropdown.GetComponent<Dropdown>().value == 0)
                    {
                        Transform myParent = GameObject.Find("Canvas").transform;
                        GameObject reminderPanel = Instantiate(foremanReminderPanel) as GameObject;
                        reminderPanel.transform.SetParent(myParent, false);
                        reminderPanel.transform.localScale = new Vector3(1, 1, 1);
                        yield return new WaitUntil(() => ForemanReminder.foremanChosen == true);
                        ForemanReminder.foremanChosen = false;
                        if (thisdropdown.GetComponent<Dropdown>().value == 0)
                        {
                            yield break;
                        }
                    }

                }
                 if (thisdropdown.objID == 16) // the supervisor dropdown.
                {
                    if (thisdropdown.GetComponent<Dropdown>().value == 0)
                    {
                        Transform myParent = GameObject.Find("Canvas").transform;
                        GameObject reminderPanel = Instantiate(supervisorReminderPanel) as GameObject;
                        reminderPanel.transform.SetParent(myParent, false);
                        reminderPanel.transform.localScale = new Vector3(1, 1, 1);
                        yield return new WaitUntil(() => SupervisorReminder.supervisorChosen == true);
                        SupervisorReminder.supervisorChosen = false;
                        if (thisdropdown.GetComponent<Dropdown>().value == 0)
                        {
                            yield break;
                        }
                    }

                }

            }

        
            if (pageController.isEmergency)
            {
                if (thisdropdown.objID == 1) // the foreman dropdown.
                {
                    if (thisdropdown.GetComponent<Dropdown>().value == 0)
                    {
                        Transform myParent = GameObject.Find("Canvas").transform;
                        GameObject reminderPanel = Instantiate(foremanReminderPanel) as GameObject;
                        reminderPanel.transform.SetParent(myParent, false);
                        reminderPanel.transform.localScale = new Vector3(1, 1, 1);
                        yield return new WaitUntil(() => ForemanReminder.foremanChosen == true);
                        ForemanReminder.foremanChosen = false;
                        if (thisdropdown.GetComponent<Dropdown>().value == 0)
                        {
                            yield break;
                        }
                    }
                }
            }

        }

        times = Resources.FindObjectsOfTypeAll<TimeID>();
        foreach (TimeID thistime in times)
        {
            GetTime thisGetTime = thistime.gameObject.GetComponent<GetTime>();
            thisGetTime.isTimeFinished = false;
            thisGetTime.CheckTimeGroup();
            yield return new WaitUntil(() => thisGetTime.isTimeFinished == true);
        }
        UnityEngine.Debug.Log("times checked, plan ready to submit");
       StartCoroutine( SubmitPlan());
        
    }

    // Prepare name string of of submitted plan, and call the saving process.
    public IEnumerator SubmitPlan() {
        fileisSaved = false;
        GetArrays();
            foreach (DropdownID thisdropdown in dropdowns) {

            if (!pageController.isEmergency)
            {
                if (thisdropdown.objID == 15) // the foreman dropdown.
                {
                    // Capture the name of the plan preparer to use in file name construction
                    myPrep = thisdropdown.GetComponent<Dropdown>().gameObject.GetComponentInChildren<Text>().text;
                }
                if (thisdropdown.objID == 16)
                {
                    //Capture the name of the supervisor and use the initials for file name construction.
                    string str = thisdropdown.GetComponent<Dropdown>().gameObject.GetComponentInChildren<Text>().text;
                    mySup = new String(str.Split(' ').Select(x => x[0]).ToArray());
                }
            }
            if (pageController.isEmergency)
                mySup = "";
            {
                if (thisdropdown.objID == 1)
                {
                    // Capture the name of the plan preparer to use in file name construction
                    myPrep = thisdropdown.GetComponent<Dropdown>().gameObject.GetComponentInChildren<Text>().text;
                }
            }
        }
        string myDate = System.DateTime.Now.ToString("yyyy-MMM-dd_HH-mm");
        saveName = mySup +"_" + myDate + "_" + myPrep;
        if (pageController.isEmergency)
        {
            saveName = "Emergency_" + saveName;
        }
        UnityEngine.Debug.Log(dataPath + "/" + saveName);

        // Initiate all the data processing to add to save file, and wait until save is completed.
        Save();
        yield return new WaitUntil(() => fileisSaved == true);
        UnityEngine.Debug.Log("File should be saved");        
        pageController.FinishPage("The job plan " + saveName + " has been submitted" + "\n Please remember to turn on your computer at the end of the day to sync the files.");
    }

    // ensure the file was saved successfully.
    public void CheckFileSaved()
    {
        if (File.Exists(dataPath + "/" + saveName + ".dat"))
        {
            fileisSaved = true;
           // Debug.Log("Save File was found at :" + dataPath + "/" + saveName + ".dat");
        }
        if (!File.Exists(dataPath + "/" + saveName + ".dat")) {
            SavingErrorPanel.SetActive(true);
            UnityEngine.Debug.Log("Save file was not found");
        }
    
    }

    // prepare all the data to be serialized for saving, and save the file to the specified location with the saveName prepared by SubmitPlan();
    public void Save()
    { 
        GetArrays(); // called to initiate all the gameobject arrays to be saved.
        //Close the SavePanel if file is being saved from that location.
        if (savePanelGO.activeSelf == true)
        {
            GameObject.FindObjectOfType<SaveFileCenterSnap>().savePanelShowing = false;
        }
        BinaryFormatter bf = new BinaryFormatter();

        // dataPath is set from SetDatatPath(string) being called by the various saving / submitting / loading buttons.
        if (!Directory.Exists(dataPath))
        {
            Directory.CreateDirectory(dataPath);
        }
        FileStream file = File.Create(dataPath + "/" + saveName + ".dat");
        SaveData data = new SaveData();

        for (int i = 0; i < data.inputText.Length; i++)
        { // serialize all the input fields;
            if (!savingTemplate)
            {
                foreach (InputFieldID thisInputfield in inputfields)
                {
                    if (thisInputfield.objID == i + 1)
                    {
                        data.inputText[i] = thisInputfield.GetComponent<InputField>().text;
                    }
                }
            }
            if (savingTemplate)
            {
                foreach (InputFieldID thisInputfield in inputfields)
                {
                    if (thisInputfield.objID == i + 1 && thisInputfield.CompareTag("TemplateField")) // only save the templatable input fields
                    {
                        data.inputText[i] = thisInputfield.GetComponent<InputField>().text;
                    }
                }
            }

        }
        for (int i = 0; i < data.checkboxstate.Length; i++)
        { // serialize all the check boxes;
            foreach (CheckBoxID thisCheckBox in checkBoxes)
            {
                if (thisCheckBox.objID == i + 1)
                {
                    data.checkboxstate[i] = thisCheckBox.gameObject.GetComponent<Toggle>().isOn;
                    //  Debug.Log(thisCheckBox.gameObject.GetComponent<Toggle>().isOn);
                }
            }
        }
        if (!savingTemplate)
        {
            // Debug.Log("Saving Sigs not template");
            for (int i = 0; i < data.sigbytes.Length; i++)
            { // serialize all the textures of the signatures textures
                foreach (SignatureID thisSig in sigimages)
                {
                    if (thisSig.objID == i + 1 && thisSig.bytesString != null && thisSig.isSigned)
                    {
                        data.sigbytes[i] = thisSig.bytesString;
                    }
                }
            }
        }

        for (int i = 0; i < data.dropdownvalue.Length; i++) // serialize all the dropdown selected value as int
        {
            foreach (DropdownID thisDropdown in dropdowns)
            {
                if (!savingTemplate)
                {
                    if (thisDropdown.objID == i + 1)
                    {
                        //  data.dropdownvalue[i] = thisDropdown.GetComponent<Dropdown>().value;
                        Dropdown myDropdown = thisDropdown.GetComponent<Dropdown>();
                        data.dropdownString[i] = myDropdown.captionText.text.ToString();
                        //  Debug.Log(data.dropdownString[i]);

                    }
                }
                if (savingTemplate)
                {
                    if (thisDropdown.objID == i + 1 && thisDropdown.CompareTag("SWP"))
                    {
                        // data.dropdownvalue[i] = thisDropdown.GetComponent<Dropdown>().value;
                        Dropdown myDropdown = thisDropdown.GetComponent<Dropdown>();
                        data.dropdownString[i] = myDropdown.captionText.text.ToString();
                        //  Debug.Log(data.dropdownString[i]);
                    }
                }
            }
        }

        if (!savingTemplate)
        {
            for (int i = 0; i < data.timestring.Length; i++) // serialize all the time pickers strings.
            {
                foreach (TimeID thisTime in times)
                {
                    if (thisTime.objID == i + 1)
                    {
                        data.timestring[i] = thisTime.GetComponent<GetTime>().myText.text;
                    }
                }
            }

            for (int i = 0; i < data.timeStampString.Length; i++)
            {
                foreach (TimeStampID thisStamp in timeStamps)
                {
                   // UnityEngine.Debug.Log("ThisStamp ID # = " + thisStamp.objID + " i = " + i);
                    if (thisStamp.objID == i + 1)
                    {
                        data.timeStampString[i] = thisStamp.GetComponent<Text>().text;
                       // UnityEngine.Debug.Log("Save timestamp string #" + i + " = " + data.timeStampString[i]);
                    }
                }
            }

        }
        if (!savingTemplate)
        {
            for (int i = 0; i < date.Length; i++)
            {
                foreach (GetDate thisDate in date)
                {
                    if (thisDate.objID == i + 1)
                    {
                        data.datestring[i] = thisDate.GetComponent<Text>().text;
                    }
                }
            }
        }
        if (!savingTemplate)
        {
            data.trafficLayout = myLayout[0].bytesString;
            //data.datestring = date[0].GetComponent<Text>().text;
            data.isEmergency = GameObject.Find("PageController").GetComponent<PageController>().isEmergency;
            data.isCSEP = pageController.isCSEP;
            data.isCSEP2 = pageController.isCSEP2;
            if (data.isCSEP == true)
            {
               foreach (CSEPNumberID thisNum in CSEPNUm)
                    {
                        if (thisNum.ObjID == 0)
                        { // the value given to page 1
                            data.CSEPNumber = thisNum.GetComponent<Text>().text;
                           // Debug.Log("Saving CSEP 1 as " + data.CSEPNumber);

                        }
                    }
                
            }

            if (data.isCSEP2)
            {
                foreach (CSEPNumberID thisNum in CSEPNUm)
                {
                    if (thisNum.ObjID == 1)
                    {
                        data.CSEP2Number = thisNum.GetComponent<Text>().text;
                        //Debug.Log("Saving CSEP 2 as " + data.CSEP2Number);
                    }
                }
            }
            if (data.isCSEP != true)
            {
                data.CSEPNumber = null;
                data.CSEP2Number = null;
            }
        }
            bf.Serialize(file, data);
            file.Close();
            DestroySaveList();
            newSavePanel.SetActive(false);
            overwriteSavePanel.SetActive(false);
            savePanelGO.SetActive(false);
            loadedFile = false;
            savingTemplate = false;
        KillArrays();
        ContinuousSaveController.isSaving = false;
        CheckFileSaved();

        
    }

    public void LoadFile()
    {
        GetArrays();
            loadedFile = true;

            PageController pageController = GameObject.FindObjectOfType<PageController>();
            pageController.ResetForm();
            BinaryFormatter bf = new BinaryFormatter();
            loadedFilePath = dataPath + "/" + loadName + ".dat";
            FileStream file = File.Open(loadedFilePath, FileMode.Open);
            // loadedFilePath = dataPath + "/" + loadName + ".dat";
            // Debug.Log(file.Name);
            SaveData data = (SaveData)bf.Deserialize(file);
            // Debug.Log(data.inputText.Length + " " + data.checkboxstate.Length);
            file.Close();
        

        try
        {
            GameObject.Find("PageController").GetComponent<PageController>().isEmergency = data.isEmergency;

            for (int i = 0; i < inputfields.Length; i++)
            {
                //   UnityEngine.Debug.Log("here 2");
                foreach (InputFieldID thisInputfield in inputfields)
                {
                    if (thisInputfield.objID == i + 1 && data.inputText[i] != null)
                        {
                            //Debug.Log("Loaded text for input field  " + i + " = " + data.inputText[i]);
                            thisInputfield.SetText(data.inputText[i]);
                        }
                }
            }
            for (int i = 0; i < checkBoxes.Length; i++)
            {
                //  UnityEngine.Debug.Log("here 3");
                foreach (CheckBoxID thisCheckBox in checkBoxes)
                {
                    if (thisCheckBox.objID == i + 1 )
                    {
                        thisCheckBox.gameObject.GetComponent<Toggle>().isOn = data.checkboxstate[i];
                        // Debug.Log("checkBoxes" + data.checkboxstate[i]);
                    }
                }
            }
            for (int i = 0; i < data.sigbytes.Length; i++)
            {
                foreach (SignatureID thisSig in sigimages)
                {
                    if (thisSig.objID == i + 1 && data.sigbytes[i] != null)
                    {
                        thisSig.ConvertStringToTexture(data.sigbytes[i]);
                    }
                }
            }
            for (int i = 0; i < data.dropdownvalue.Length; i++) // serialize all the dropdown selected value as int
            {
                foreach (DropdownID thisDropdown in dropdowns)
                {
                    if (thisDropdown.objID == i + 1)
                    {
                        //  thisDropdown.GetComponent<Dropdown>().value = data.dropdownvalue[i];
                        Dropdown myDropdown = thisDropdown.GetComponent<Dropdown>();
                        //myDropdown.RefreshShownValue();
                        Dropdown.OptionData[] theseOptions = myDropdown.options.ToArray();
                        if (data.dropdownString[i] != null)
                        {
                            for (int j = 0; j < theseOptions.Length; j++) {
                                string s = theseOptions[j].text;
                                if (s == data.dropdownString[i]) {
                                    myDropdown.value = j;
                                    if (myDropdown.CompareTag("SWP"))
                                    {
                                        SWPController mySWP = myDropdown.GetComponent<SWPController>();
                                        if (j > 0)
                                        {
                                            mySWP.myButton.gameObject.SetActive(true);
                                        }
                                    }
                                }
                            }
                           // myDropdown.captionText.text = data.dropdownString[i].ToString();
                            
                          //  Debug.Log(data.dropdownString[i]);
                        }
                    }
                }
            }
            for (int i = 0; i < data.timestring.Length; i++) // serialize all the dropdown selected value as int
            {
                foreach (TimeID thisTime in times)
                {
                    if (thisTime.objID == i + 1)
                    {
                        if (data.timestring[i] == null && thisTime.CompareTag("MainTime"))
                        {
                          //  Debug.Log("Main Time null called" + thisTime.name);
                            thisTime.GetComponent<GetTime>().SetTime();
                            thisTime.GetComponent<GetTime>().myText.text = System.DateTime.Now.ToString("HH:mm");

                        }
                        if (data.timestring[i] == null && !thisTime.CompareTag("MainTime"))
                        {
                            thisTime.GetComponent<GetTime>().myText.text = "00:00";

                        }
                        if (data.timestring[i] != null)
                        {
                            thisTime.GetComponent<GetTime>().myText.text = data.timestring[i];
                        }
                    }
                }
            }
            for (int i = 0; i < date.Length; i++)
            {
                foreach (GetDate thisDate in date)
                {

                    if (thisDate.objID == i + 1 && data.datestring[i] != null)
                    {

                        thisDate.GetComponent<Text>().text = data.datestring[i];

                    }
                }
            }
            if (data.timeStampString != null)
            {
                for (int i = 0; i < data.timeStampString.Length; i++)
                {
                    foreach (TimeStampID thisStamp in timeStamps)
                    {
                        if (thisStamp.objID == i + 1 && data.timeStampString[i] != null)
                        {
                            UnityEngine.Debug.Log("Timestamp #" + thisStamp.objID);
                            thisStamp.GetComponent<Text>().text = data.timeStampString[i];
                           
                        }
                    }
                }
            }


            if (data.trafficLayout != null)
            {
                myLayout[0].ConvertStringToTexture(data.trafficLayout); // there is only one layout element
            }
            //date[0].GetComponent<Text>().text = data.datestring;
            
        }
        catch (Exception e){
            UnityEngine.Debug.Log("Caught an error type " + e);
            errorPanel.SetActive(true);
            return;
        }
        if (data.isEmergency)
        {
            pageController.CreateEmergencyPlan();
        }
        if (!data.isEmergency)
        {
           
            pageController.ResetForPlanLoad();
            pageController.CreateNewJobPLan();
            if (data.isCSEP == true)
            {
                pageController.isCSEP = data.isCSEP;
                foreach (CSEPNumberID thisNum in CSEPNUm)
                    {
                        if (thisNum.ObjID == 0)
                        {
                            thisNum.GetComponent<Text>().text = data.CSEPNumber;
                        }
                    }
                pageController.viewCSEPButton.GetComponentInChildren<Text>().text = "View CSEP # " + data.CSEPNumber;
                //Debug.Log("View button 1 should be " + data.CSEPNumber);

            }


            if (data.isCSEP2) {
                    pageController.isCSEP2 = data.isCSEP2;
                    foreach (CSEPNumberID thisNum in CSEPNUm)
                    {
                        if (thisNum.ObjID == 1)
                        {
                            thisNum.GetComponent<Text>().text = data.CSEP2Number;
                        }
                    }
                
                 pageController.viewCSEP2Button.GetComponentInChildren<Text>().text = "View CSEP # " + data.CSEP2Number;
                //Debug.Log("View button 2 should be " + data.CSEP2Number);


            }
            pageController.LoadPlanWithCSEP();
            }
           
            
           
        
        loadPanel.SetActive(false);
        KillArrays();
        ContinuousSaveController.continuousSaveActive = true;
    }

    // all the parameters to save outlined below
    [Serializable]
    public class SaveData
    {
        public string[] inputText;
        public bool[] checkboxstate;
        public string[] sigbytes;
        public int[] dropdownvalue; // replaced by dropdownString to work around changes to list between a save file and a loaded file.
        public string[] dropdownString;
        public string[] timestring;
        public string[] datestring;
        public string[] timeStampString;
        public string trafficLayout;
        public string emergencyDateTime;
        public bool isEmergency;
        public bool isCSEP;
        public string CSEPNumber;
        public bool isCSEP2;
        public string CSEP2Number;

        public SaveData() 
        {
            // Initialize all the data arrays for saving data to them;

            inputText = new string[Resources.FindObjectsOfTypeAll<InputFieldID>().Length];
            checkboxstate = new bool[Resources.FindObjectsOfTypeAll<CheckBoxID>().Length];
            sigbytes = new string[Resources.FindObjectsOfTypeAll<SignatureID>().Length];
            dropdownvalue = new int[Resources.FindObjectsOfTypeAll<DropdownID>().Length];
            timestring = new string[Resources.FindObjectsOfTypeAll<TimeID>().Length];
            datestring = new string[Resources.FindObjectsOfTypeAll<GetDate>().Length];
            dropdownString = new string[Resources.FindObjectsOfTypeAll<DropdownID>().Length];
            timeStampString = new string[Resources.FindObjectsOfTypeAll<TimeStampID>().Length];
            
        }

           


    }
   

}
//checkBoxes = Resources.FindObjectsOfTypeAll<CheckBoxID>();
//        inputfields = Resources.FindObjectsOfTypeAll<InputFieldID>();
//        sigimages = Resources.FindObjectsOfTypeAll<SignatureID>();
//        dropdowns = Resources.FindObjectsOfTypeAll<DropdownID>();
//        times = Resources.FindObjectsOfTypeAll<TimeID>();
//        date = Resources.FindObjectsOfTypeAll<GetDate>();
//        myLayout = Resources.FindObjectsOfTypeAll<ChosenLayout>();
//        signOffs = Resources.FindObjectsOfTypeAll<SignOffID>();
//        CSEPNUm = Resources.FindObjectsOfTypeAll<CSEPNumberID>();

