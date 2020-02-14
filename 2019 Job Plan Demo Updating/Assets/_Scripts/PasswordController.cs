using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class PasswordController : MonoBehaviour
{

    public Button setUpSharepointButton;
    public Button checkSyncFilesButton;
    public Button loadSubmittedPlan;
    public Button loadArchivedPDF;
    public Button setSupPass;
    public Button setCSEPNum;
    public InputField adminPass;
    public InputField supPass;
    List<string> passCharacters = new List<string>();
    InputField focusedField;
    InputField previousFocused;
    string adminPassword = "Sm0k3r";
    string supervisorPassword = "@l3ctr@";

    public GameObject enterPassPanel;
    public InputField firstPass;
    public InputField secondPass;
    public Text reenterText;
    public Text matchText;
    public Button savePassword;




    // Set colors of supervisor and admin buttons to grayed out if no passwords are eneterd
    void Start()
    {

        loadSubmittedPlan.GetComponentInChildren<Text>().color = new Color(.196f, .196f, .196f, .15f);
        loadArchivedPDF.GetComponentInChildren<Text>().color = new Color(.196f, .196f, .196f, .15f);
        setUpSharepointButton.GetComponentInChildren<Text>().color = new Color(.196f, .196f, .196f, .15f);
        checkSyncFilesButton.GetComponentInChildren<Text>().color = new Color(.196f, .196f, .196f, .15f);
        setSupPass.GetComponentInChildren<Text>().color = new Color(.196f, .196f, .196f, .15f);
        setCSEPNum.GetComponentInChildren<Text>().color = new Color(.196f, .196f, .196f, .15f);

        if (File.Exists(SaveFile.sharepointPath + "/Job Plans/FormData/SupervisorPasswords.dat"))
        {
            return;
        }
        else
        {
            Debug.Log("new file created on start");
            var newFile = File.Create(SaveFile.sharepointPath + "/Job Plans/FormData/SupervisorPasswords.dat");
            newFile.Close();
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenSavePasswordPanel()
    {
        enterPassPanel.SetActive(true);
    }

    public void EnableSupButtons() // Check input against saved supervisor password list, and enable supervisor buttons if valid.
    {
        List<String> checkList = LoadPasswordStrings();
        for (int i = 0; i < checkList.Count; i++)
        {
            if (supPass.text == checkList[i])
            {
                loadSubmittedPlan.interactable = true;
                loadArchivedPDF.interactable = true;
                loadSubmittedPlan.GetComponentInChildren<Text>().color = new Color(.196f, .196f, .196f, 1);
                loadArchivedPDF.GetComponentInChildren<Text>().color = new Color(.196f, .196f, .196f, 1);
                supPass.text = "";
                return;
            }
        }
    }

    public void EnableAdminButtons() // check input against harcoded admin password, and enable admin buttons if valid.
    {
        if (adminPass.text == adminPassword)
        {
            setUpSharepointButton.interactable = true;
            checkSyncFilesButton.interactable = true;
            setSupPass.interactable = true;
            setCSEPNum.interactable = true;
            setUpSharepointButton.GetComponentInChildren<Text>().color = new Color(.196f, .196f, .196f, 1);
            checkSyncFilesButton.GetComponentInChildren<Text>().color = new Color(.196f, .196f, .196f, 1);
            setSupPass.GetComponentInChildren<Text>().color = new Color(.196f, .196f, .196f, 1);
            setCSEPNum.GetComponentInChildren<Text>().color = new Color(.196f, .196f, .196f, 1);
            adminPass.text = "";
        }
    }

    public void CheckFirstPass() // test first input passowrd to be saved against criteria below, if ok enable second input window, if not promt user.
    {
        if (firstPass.text.Length >= 6)
        {
            secondPass.gameObject.SetActive(true);
            reenterText.gameObject.SetActive(true);
            reenterText.text = "Please re-enter your password to confirm";
            EventSystem.current.SetSelectedGameObject(secondPass.gameObject, null);
            //secondPass.OnPointerClick(null);

        }
        else
        {
            reenterText.gameObject.SetActive(true);
            reenterText.text = "Please use a password with at least 6 charachters.";
        }
    }

    public void CheckSecondPass() // test second input of same password to be saved against first entry for match. if matching, enable save button, if not prompt user.
    {
        matchText.gameObject.SetActive(true);
        if (secondPass.text == firstPass.text)
        {

            matchText.text = "Passwords match. Please save to be able to use it";
            savePassword.gameObject.SetActive(true);
        }
        else
        {
            matchText.text = "Passwords do not match, please try again.";
        }
    }

    public void HideReenterText() 
    {
        reenterText.gameObject.SetActive(false);
    }

    public void HideSaveButton()
    {
        savePassword.gameObject.SetActive(false);
        matchText.gameObject.SetActive(false);
    }

    public void BackButton() // reset page, and close.
    {
        firstPass.text = "";
        secondPass.text = "";
        reenterText.gameObject.SetActive(false);
        secondPass.gameObject.SetActive(false);
        matchText.gameObject.SetActive(false);
        savePassword.gameObject.SetActive(false);
        GameObject.FindObjectOfType<SaveFile>().HidePanel();
    }

    public void SaveButton() // save password to .dat file as listed below for comparison use.
    {
        List<string> passList = LoadPasswordStrings();
        passList.Add(firstPass.text);
        for (int i = 0; i < passList.Count; i++)
        {
            Debug.Log("Saved string " + i + " is" + passList[i]);
        }
        BinaryFormatter bf = new BinaryFormatter();
        
        string saveFilePath = SaveFile.sharepointPath + "/Job Plans/FormData/SupervisorPasswords.dat";
        FileInfo fileInfo =  new FileInfo(saveFilePath);
        if (!IsFileLocked(fileInfo))
        {
            Debug.Log("File is not locked");
        }
        FileStream file = File.Open(saveFilePath, FileMode.Open);
        PasswordData passData = new PasswordData();
        passData.passwordStrings = new List<string>(passList);
        for (int i = 0; i < passData.passwordStrings.Count; i++)
        {
            Debug.Log("passData string are " + passData.passwordStrings[i]);
        }
        bf.Serialize(file, passData);
        file.Close();
        BackButton();

    }


    List<String> LoadPasswordStrings() // load passwords from saved file for comparison use.
    {
        List<string> passList = new List<string>();
        BinaryFormatter bf = new BinaryFormatter();
        string loadedFilePath = SaveFile.sharepointPath + "/Job Plans/FormData/SupervisorPasswords.dat";
        FileStream file = File.Open(loadedFilePath, FileMode.Open);
        PasswordData passData = new PasswordData();
        if (new FileInfo(loadedFilePath).Length > 0)
        {
            passData = (PasswordData)bf.Deserialize(file);
            file.Close();
            for (int i = 0; i < passData.passwordStrings.Count; i++)
            {
                Debug.Log("Saved string " + i + " is" + passData.passwordStrings[i]);
            }
        }
        else if (file.Length == 0)
        {
            file.Close();
            Debug.Log("Loaded File was Closed");
        }
        foreach (string thispass in passData.passwordStrings)
        {
            Debug.Log(thispass);
        }
        
        return passData.passwordStrings;
        
    }
    [Serializable]
    public class PasswordData // passData class for saving a password list.
    {
        public List<string> passwordStrings;

        public PasswordData()
        {
            passwordStrings = new List<string>();
        }
    }

    private bool IsFileLocked(FileInfo file) // check if tested file is in use.
    {
        FileStream stream = null;

        try
        {
            stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        }
        catch (IOException)
        {
            //the file is unavailable because it is:
            //still being written to
            //or being processed by another thread
            //or does not exist (has already been processed)
            return true;
        }
        finally
        {
            if (stream != null)
                stream.Close();
        }

        //file is not locked
        return false;
    }


}

