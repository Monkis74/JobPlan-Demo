using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Globalization;

// the behaviour of the month scroll in the datePicker panel.

public class MonthPicker : MonoBehaviour
{
    public RectTransform panel; // hold the scrolling panel.
    public RectTransform monthList; // the parent object that holds all the months
    public GameObject monthText; // the text component of each month object.
    public RectTransform center; // the center point to campre to and snap to
    string[] months; // array pf all the months names
    GameObject[] monthGO;
    float[] distance; // hold the months objects distances to the center.
    bool dragging = false; // will be true when dragging the panel;
    int imageDistance; // holods the distance between each months text object;
    int mintextnum; // hold the number of the closest text object to the center.
    public static int myMonth;
    public static string myMonthString;
    public GameObject dayPicker;
   public static bool dayPickerStarted = false;

    // Use this for initialization
    void Start()
    {
        months = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames;
        
        monthGO = new GameObject[months.Length];
        Vector3 spawnPos = transform.position;
        int i = 0;
        foreach (string month in months) // create an object for each month in the month list transform.
        {
            GameObject thisMonth = Instantiate(monthText, spawnPos, Quaternion.identity) as GameObject;
            spawnPos = new Vector3(thisMonth.transform.position.x, thisMonth.transform.position.y - 2.5f, thisMonth.transform.position.z);
            RectTransform myAnchor = thisMonth.GetComponent<RectTransform>();
            myAnchor.anchorMin = new Vector2(0.5f, 0.5f);
            myAnchor.anchorMax = new Vector2(0.5f, 0.5f);
            myAnchor.pivot = new Vector2(0.5f, 0.5f);
            thisMonth.transform.SetParent(monthList.transform);
            thisMonth.transform.localScale = new Vector3(1, 1, 1);
            thisMonth.name = month;
            Text myText = thisMonth.GetComponent<Text>();
            myText.text = month;
            monthGO[i] = thisMonth.gameObject;
            i++;
        }


        distance = new float[monthGO.Length];

        // Get distance between month GO's in y axis
        imageDistance = (int)Mathf.Abs(monthGO[1].GetComponent<RectTransform>().anchoredPosition.y - monthGO[0].GetComponent<RectTransform>().anchoredPosition.y);
        //print(imageDistance);

        SetCurrentMonth();


    }

    

    // sets the scroll for the months to the current month
    public void SetCurrentMonth() { 
        string monthName = System.DateTime.Now.ToString("MMM", CultureInfo.InvariantCulture);
        
        for (int i = 0; i < months.Length; i++) {
            if (months[i] == monthName){
                mintextnum = i;
                float newY = mintextnum * imageDistance;
                Vector2 newPos = new Vector2(monthList.anchoredPosition.x, newY);
                monthList.anchoredPosition = newPos;
                dayPickerStarted = false;
                dragging = false;
            }
        }
    }

   

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < monthGO.Length; i++)
        {
            distance[i] = Mathf.Abs(center.transform.position.y - monthGO[i].transform.position.y);
        }

        float minDistance = Mathf.Min(distance); // get the minimum distance in the array

        for (int a = 0; a < monthGO.Length; a++)
        {
            if (minDistance == distance[a])
            {
                mintextnum = a;                     // sets the number for the game object that is closest to the center.
            }
        }

        if (!dragging)
        {
            //LerpToMonthGO(mintextnum * imageDistance, 10f);
            LerpToMonthGO(-monthGO[mintextnum].GetComponent<RectTransform>().anchoredPosition.y, 10f);
            if (!dayPickerStarted)
            {
                //StartDayPicker();
                //dayPickerStarted = true;
                dayPicker.SetActive(true);
                dayPickerStarted = true;
            }

        }
        myMonth = mintextnum +1;
        myMonthString = monthGO[mintextnum].GetComponent<Text>().text;
       // Debug.Log(myMonth);

    }

    void LerpToMonthGO(float position, float speed)
    {
        float newY = Mathf.Lerp(monthList.anchoredPosition.y, position, Time.deltaTime * speed);
        Vector2 newPos = new Vector2(monthList.anchoredPosition.x, newY);
        monthList.anchoredPosition = newPos;
    }

    public void StartDrag()
    {
        dragging = true;
        dayPicker.SetActive(false);
        dayPickerStarted = false;
    }

    public void EndDrag() {
        dragging = false;
        dayPickerStarted = true;
        dayPicker.SetActive(true);
     }

 
}


