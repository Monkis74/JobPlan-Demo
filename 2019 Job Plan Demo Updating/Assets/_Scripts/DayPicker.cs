using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Globalization;

// used to select a day from DatePicker panel.

public class DayPicker : MonoBehaviour
{
    public RectTransform panel; // hold the scrolling panel.
    public RectTransform dayList; // the parent object that holds all the days
    public GameObject dayText; // the text component of each day object.
    public RectTransform center; // the center point to campre to and snap to
    string[] days; // array pf all the days names
    GameObject[] dayGO; // array of date gameobjects in the scrollable list to select from
    float[] distance; // hold the days objects distances to the center.
    public float[] distanceReposition;
    bool dragging = false; // will be true when dragging the panel;
    int imageDistance; // holods the distance between each days text object;
    int mintextnum; // hold the number of the closest text object to the center.
    public static string myDay;
    bool wasStarted = false;


    // started after the month picker which is started after the year picker to set 
    // the proper amount of days in the chosen month.
    void Start()
    {
        int myYear;
         myYear = Int32.Parse(YearPicker.myYear);
        int myMonth = MonthPicker.myMonth;
        int daysInMonth = System.DateTime.DaysInMonth(myYear, myMonth);

        days = new string[daysInMonth];
        for (int d = 0; d < days.Length; d++) {
            days[d] = (d + 1).ToString();
        }

        dayGO = new GameObject[days.Length]; // initialize the array to the amount of days in  the chosen month.
        
        Vector3 spawnPos = transform.position;
        int i = 0;
        foreach (string day in days) // create an object for each day in the day list transform.
        {
            GameObject thisDay = Instantiate(dayText, spawnPos, Quaternion.identity) as GameObject;
            spawnPos = new Vector3(thisDay.transform.position.x, thisDay.transform.position.y - 2.5f, thisDay.transform.position.z);
            RectTransform myAnchor = thisDay.GetComponent<RectTransform>();
            myAnchor.anchorMin = new Vector2(0.5f, 0.5f);
            myAnchor.anchorMax = new Vector2(0.5f, 0.5f);
            myAnchor.pivot = new Vector2(0.5f, 0.5f);
            thisDay.transform.SetParent(dayList.transform);
            thisDay.transform.localScale = new Vector3(1, 1, 1);
            thisDay.name = day;
            Text myText = thisDay.GetComponent<Text>();
            myText.text = day;
            dayGO[i] = thisDay.gameObject;
            i++;
        }
        distance = new float[dayGO.Length];
        distanceReposition = new float[dayGO.Length];
        // the distance between each date object for snapping and scrolling use.
        imageDistance = (int)Mathf.Abs(dayGO[1].GetComponent<RectTransform>().anchoredPosition.y - dayGO[0].GetComponent<RectTransform>().anchoredPosition.y);
      

        SetCurrentday();
        wasStarted = true;


    }
    public void OnEnable() // reset day values to new year and month selected;
    {   
        if (!wasStarted) { return; }
        int childs = dayList.childCount;
        for (int c = childs; c > 0; c--)
        {
              GameObject.Destroy(dayList.GetChild(c-1).gameObject);
         }

        int myYear;
        Int32.TryParse(YearPicker.myYear, out myYear);
        int myMonth = MonthPicker.myMonth;
        int daysInMonth = System.DateTime.DaysInMonth(myYear, myMonth );

       
        //Debug.Log(daysInMonth);
        days = new string[daysInMonth];
        for (int d = 0; d < days.Length; d++)
        {
            days[d] = (d + 1).ToString();
        }

        dayGO = new GameObject[days.Length];
        Vector3 spawnPos = center.transform.position;
       // Vector3 spawnPos = new Vector3(0, 0, dayList.transform.position.z);
        int i = 0;
        foreach (string day in days) // create an object for each day in the day list transform.
        {
            GameObject thisDay = Instantiate(dayText, spawnPos, Quaternion.identity) as GameObject;
            spawnPos = new Vector3(thisDay.transform.position.x, thisDay.transform.position.y - 2.5f, thisDay.transform.position.z);
            RectTransform myAnchor = thisDay.GetComponent<RectTransform>();
            myAnchor.anchorMin = new Vector2(0.5f, 0.5f);
            myAnchor.anchorMax = new Vector2(0.5f, 0.5f);
            myAnchor.pivot = new Vector2(0.5f, 0.5f);
            thisDay.transform.SetParent(dayList.transform);
            thisDay.transform.localScale = new Vector3(1, 1, 1);
            thisDay.name = day;
            Text myText = thisDay.GetComponent<Text>();
            myText.text = day;
            dayGO[i] = thisDay.gameObject;
            i++;
        }
        distance = new float[dayGO.Length];
        distanceReposition = new float[dayGO.Length];

        // Get distance between day GO's in y axis
       
        imageDistance = (int)Mathf.Abs(dayGO[1].GetComponent<RectTransform>().anchoredPosition.y - dayGO[0].GetComponent<RectTransform>().anchoredPosition.y);
        

        SetCurrentday();
    }

    // sets the scroll for the day picker to the current day
    public void SetCurrentday()
    { 
        string day = System.DateTime.Now.Day.ToString();

        for (int i = 0; i < days.Length; i++)
        {
            if (days[i] == day)
            {
                mintextnum = i;
                float newY = mintextnum * imageDistance;
                Vector2 newPos = new Vector2(dayList.anchoredPosition.x, newY);
                dayList.anchoredPosition = newPos;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < dayGO.Length; i++)
        {
            distanceReposition[i] = center.GetComponent<RectTransform>().position.y - dayGO[i].GetComponent<RectTransform>().position.y;
            //distance[i] = Mathf.Abs(center.transform.position.y - dayGO[i].transform.position.y);
            distance[i] = Mathf.Abs(distanceReposition[i]);

          


        }

        float minDistance = Mathf.Min(distance); // get the minimum distance in the array

        for (int a = 0; a < dayGO.Length; a++)
        {
            if (minDistance == distance[a])
            {
                mintextnum = a;                     // sets the number for the game object that is closest to the center.
            }
        }

        if (!dragging)
        {
            //LerpTodayGO(mintextnum * imageDistance, 10f);
            LerpTodayGO(-dayGO[mintextnum].GetComponent<RectTransform>().anchoredPosition.y, 10f);
        }
        myDay = dayGO[mintextnum].GetComponent<Text>().text;
        

    }

    void LerpTodayGO(float position, float speed)
    {
        
        float newY = Mathf.Lerp(dayList.anchoredPosition.y, position, Time.deltaTime * speed);
        Vector2 newPos = new Vector2(dayList.anchoredPosition.x, newY);
        
        dayList.anchoredPosition = newPos;
       // Debug.Log("selected Day " + mintextnum);
    }

    public void StartDrag()
    {
        dragging = true;
    }

    public void EndDrag()
    {
        dragging = false;
    }


}