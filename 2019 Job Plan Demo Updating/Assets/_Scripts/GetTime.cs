using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// provide a time for each time field entry.
public class GetTime : MonoBehaviour {

    public Text myText; // text component of the time entry.
    public GameObject[] timeGroup;
    public bool isTimeFinished = false;
    bool timecheckactive = false; // if the time check activated the page to allow time input;
    public GameObject parentPage;
    public GetTime checkTimeMasterTime;
    string reminderText;

    // Use this for initialization
    void Start()
    {
       
       // myText = GetComponent<Text>();
        if (gameObject.CompareTag("MainTime") && !SaveFile.loadedFile)
        {
            //Debug.Log("Get Time Start Set time Ran and loaded file = " + SaveFile.loadedFile );
            SetTime();
        }

    }

    public void SetTime()
    {

        myText.text = DateTimeController.time.ToString();

    }

    public void GetNewTime()
    {
        StartCoroutine(SetNewTime());
    }

    public IEnumerator SetNewTime()
    { // wait until date picker is accepted
        yield return new WaitUntil(() => DateTimeController.newTimeSet == true);
        if (DateTimeController.timeCancelled) {
            DateTimeController.timeCancelled = false;
            DateTimeController.newTimeSet = false;
            yield break;
        }
        SetTime();
        DateTimeController.newTimeSet = false;
        if (timecheckactive) {
            isTimeFinished = true;
            timecheckactive = false;
            checkTimeMasterTime.isTimeFinished = true;
            Debug.Log("New Check Time Was Set");
            parentPage.SetActive(false);
              
        }
       
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ResetMainTime() {
        if (gameObject.CompareTag("MainTime"))
        {
            //myText = GetComponent<Text>();
            myText.text = System.DateTime.Now.ToString("HH:mm");
        }
        else {
           // myText = GetComponent<Text>();
            myText.text = "00:00";
        }
       
    }

    public void CheckTimeGroup() {
        if (timeGroup.Length < 1) {
            isTimeFinished = true;
           // Debug.Log("timeGroup is null");
            return;
        }
        string time1;
        string time2;
        string name1 = null;
        if (timeGroup.Length == 4) {
           // Debug.Log("Timegroup is not null " + gameObject.GetComponent<TimeID>().objID);
            time1 = timeGroup[0].GetComponentInChildren<Text>().text;
            time2 = timeGroup[1].GetComponentInChildren<Text>().text;
            
            if (timeGroup[2].GetComponent<Dropdown>() != null)
            {
                name1 = timeGroup[2].GetComponent<Dropdown>().captionText.text.ToString();
            }
            if (timeGroup[2].GetComponent<InputField>() != null)
            {
                name1 = timeGroup[2].GetComponent<InputField>().text;
            }
            if (timeGroup[2].GetComponent<CSEPNumberID>() != null) {
                name1 = timeGroup[2].GetComponent<Text>().text;
               // Debug.Log(name1);
            }
            
            GameObject parentPage = timeGroup[3].gameObject;

            if (time1 == "00:00") {
                isTimeFinished = true;
                return;
            }
            if ((time1 != "00:00" && time2 != "00:00"))
            {
                isTimeFinished = true;
                return;
            }
            if (time1 != "00:00" && time2 == "00:00")
            {
                // run please add your finish time.
                timeGroup[3].SetActive(true);
                timeGroup[1].GetComponent<GetTime>().FinishTimeRequest(name1);
            }   
        }
        
    }
    public void  FinishTimeRequest(string name) {
        
        if (gameObject.CompareTag("HoldOffTime")) {
            reminderText = "Please enter a surrender time for the " + name + " Hold Off.";
        }
        if (gameObject.CompareTag("CSEntrantTime")) {
            reminderText = "Please enter a Time Out for confined space entrant " + name + ".";
        }
        if (gameObject.CompareTag("CSEPTime")) {
            reminderText = "Please enter a surrender time for CSEP # " + name + ".";
        }
        DateTimeController timeController = FindObjectOfType<DateTimeController>();
        timeController.OpenTimePicker();
        Text controllerText = GameObject.Find("Canvas/TimePicker(Clone)/Text").GetComponentInChildren<Text>();
        controllerText.text = reminderText;
        timecheckactive = true;
        GetNewTime();


    }

}
