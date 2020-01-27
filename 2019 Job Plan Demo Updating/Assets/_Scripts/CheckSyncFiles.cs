using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;


public class CheckSyncFiles : MonoBehaviour
{

    public Button checkFilesButton;
    public GameObject resultsPanel;
    public GameObject confirmPanel;
    public Text confirmText;
    public Text resultsText;
    public Text resultsAmountText;
    public List<string> logList = new List<string>();
    public List<string> submittedList = new List<string>();
    public List<string> deletionList = new List<string>();
    string logPath;
    string submittedPath;
    



    // Start is called before the first frame update
    void Start()
    {
        confirmPanel.SetActive(false);
        logPath = SaveFile.sharepointPath + "/Job Plans/Log/";
        if (!Directory.Exists(logPath))
        {
            Directory.CreateDirectory(logPath);
        }
        if (!File.Exists(logPath + "SignedPlanLog.txt"))
        {
            var newFile = File.Create(logPath + "SignedPlanLog.txt");
            newFile.Close();
        }
        logPath = logPath + "SignedPlanLog.txt";
        submittedPath = SaveFile.sharepointPath + "/Job Plans/SubmittedPlans/";
        if (!Directory.Exists(submittedPath))
        {
            Directory.CreateDirectory(submittedPath);
        }
    }

    public void DeleteDuplicateFiles()
    {
        //for (int i = deletionList.Count; i > 0 ; i--)
        //{
        //    File.Delete(submittedPath + deletionList[i]);
        //    deletionList.Remove(deletionList[i]);
            
        //}
        foreach (string thisFile in deletionList)
        {
            File.Delete(submittedPath + thisFile);
        }
        deletionList.Clear();
        confirmPanel.SetActive(false);
        CheckForDuplicates();
    }

    public void OpenConfirmDelete()
    {
        if (deletionList.Count < 1)
        {
            return;
        }
        confirmPanel.SetActive(true);
        confirmText.text = " Are you sure you would like to remove the " + deletionList.Count + " files that are listed in the log as having been signed off already?";
    }

    public void CheckForDuplicates()
    {
        logList.Clear();
        submittedList.Clear();
        deletionList.Clear();
        resultsText.text = "";
        //Debug.Log("test running");
        string logLine;
        StreamReader logFile = new StreamReader(logPath);
        while ((logLine = logFile.ReadLine()) != null)
        {
           // Debug.Log(logLine);
            logList.Add(logLine);            
        }
        logFile.Close();

        //foreach (string thisLine in logList)
        //{
        //    Debug.Log(thisLine);
        //}
        string[] submittedFiles = Directory.GetFiles(submittedPath);
        foreach (string thisSub in submittedFiles)
        {
            string filename = Path.GetFileName(thisSub);
            submittedList.Add(filename);    
        }
        foreach (string item in submittedList)
        {
            for (int i = 0; i < logList.Count; i++)
            {
                if (item == logList[i])
                {
                    deletionList.Add(item);
                }
            }
        }
        resultsAmountText.text = deletionList.Count.ToString() + " Duplicate files found that have already been signed off";
        foreach (string result in deletionList)
        {
            resultsText.text += "\n" + result;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
