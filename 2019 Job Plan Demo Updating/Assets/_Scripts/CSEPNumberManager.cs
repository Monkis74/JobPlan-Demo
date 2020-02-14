using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class CSEPNumberManager : MonoBehaviour {
    const string CSEP_PERMIT_NUMBER = "permit_number";
    const string CSEP_BASE_NUMBER = "base_number";
   // int currentCSEPNum;
    public Text numberText;
    public Text numberText2;
    public GameObject setCSEPNumPanel; // The panel for viewing current number or setting it if not issued.
    public Text currentNumText; // displayed in CSEP set base number panel as currently set number if available.
    public Button setNewCSEPButton; // sets a new Base CSEP base number when pressed
    

    private void Start()
    {
       // numberText = GameObject.FindGameObjectWithTag("CSEPNumber").GetComponent<Text>();
        if (GetCSEPBaseNumber() == "" || !PlayerPrefs.HasKey(CSEP_BASE_NUMBER)) {
            Debug.Log("NO CSEP BASE, Setting New");
            SetCSEPBaseNumber();
        }
    }
    public static void SetCSEPNumber() {
        PlayerPrefs.SetInt(CSEP_PERMIT_NUMBER, GetCSEPNumber() + 1);
    }
    public static int GetCSEPNumber() {
        return PlayerPrefs.GetInt(CSEP_PERMIT_NUMBER);
    }

    public void SetCSEPBaseNumber() {
        string NewNum;
        int j = Mathf.Abs(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0));
        int k = Mathf.Abs(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0));
        int l = Mathf.Abs(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0));
        string NewSum = ((j + k + l)).ToString();
        char[] NewSumArr = NewSum.ToCharArray();
        NewNum = NewSum.Substring(NewSumArr.Length - 4);
       // Debug.Log(NewNum);
        string[] issuedNums = File.ReadAllLines(SaveFile.sharepointPath + "/Job Plans/FormData/IssuedCSEPBaseNumbers.txt");
        foreach (string thisNUm in issuedNums) {
            if (thisNUm == NewNum) {
                Debug.Log("Duplicate CSEP NUmber WAS Created, issuing new one.");
                SetCSEPBaseNumber();
                return;
            }
        }
        //string[] NumtoWrite = new string[1];
        //NumtoWrite[0] = NewNum;
        //File.WriteAllLines(SaveFile.sharepointPath + "/Job Plans/FormData/IssuedCSEPBaseNumbers.txt", NumtoWrite);
        using (StreamWriter sw = new StreamWriter(SaveFile.sharepointPath + "/Job Plans/FormData/IssuedCSEPBaseNumbers.txt"))
        {
            foreach (string thisNUm in issuedNums) {
                sw.WriteLine(thisNUm);
            }
            sw.WriteLine(NewNum);
            
        }
            PlayerPrefs.SetString(CSEP_BASE_NUMBER, NewNum);
        currentNumText.text = "Your New Issued CSEP Base Number is " + NewNum;
        
        
    }

    public string GetCSEPBaseNumber() {
        
        return PlayerPrefs.GetString(CSEP_BASE_NUMBER);
    }

    public void IssueNewCSEPNumber() {
        if (GetCSEPBaseNumber() == "") {
            SetCSEPBaseNumber();
        }
        string baseNum = GetCSEPBaseNumber();
       // Debug.Log(baseNum);
        string adderNum = GetCSEPNumber().ToString();
      //  Debug.Log(baseNum + adderNum);
        PageController pageController = FindObjectOfType<PageController>();
        if (pageController.isCSEP && !pageController.isCSEP2)
        {
            numberText.text = baseNum + adderNum;
            pageController.viewCSEPButton.GetComponentInChildren<Text>().text = "View CSEP # " + (baseNum + adderNum);
        }
        if (pageController.isCSEP2) {
            numberText2.text = baseNum + adderNum;
            pageController.viewCSEP2Button.GetComponentInChildren<Text>().text = "View CSEP # " + (baseNum + adderNum);

        }
        SetCSEPNumber();

    }

    public void OpenSetCSEPNumPanel()
    {
        setCSEPNumPanel.SetActive(true);
        string currentNUm = GetCSEPBaseNumber();
        if (currentNUm == "")
        {
            currentNumText.text = "Your currently issued CSEP number is not set. PLease press the button to issue a new number.";
            setNewCSEPButton.gameObject.SetActive(true);
        }
        else {
            currentNumText.text = "Your currently issued CSEP number is " + currentNUm;
            setNewCSEPButton.gameObject.SetActive(true);
        }
        
    }

    public void CloseSetCSEPNumPanel()
    {
        setCSEPNumPanel.SetActive(false);
    }
   
}
