using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Globalization;

// behaviour of the minute picker scroll contained on the time picker panel.

public class MinutePicker : MonoBehaviour
{
    public RectTransform panel; // hold the scrolling panel.
    public RectTransform minuteList; // the parent object that holds all the minutes
    public GameObject minuteText; // the text component of each day object.
    public RectTransform center; // the center point to campre to and snap to
    double[] minutes; // array pf all the minutes names
    GameObject[] minuteGO;
    float[] distance; // hold the minutes objects distances to the center.
    public float[] distanceReposition;
    bool dragging = false; // will be true when dragging the panel;
    int textDistance; // holods the distance between each minutes text object;
    int mintextnum; // hold the number of the closest text object to the center.
    public static string myMinute;
   


    // Use this for initialization
    void Start()
    {

        int minutesInDay = 60;

        //Debug.Log(minutesInMonth);
        minutes = new double[minutesInDay];
        for (int d = 0; d < minutes.Length; d++)
        {
            minutes[d] = d;
           
        }

        minuteGO = new GameObject[minutes.Length];
        //Debug.Log(minuteGO.Length);
        Vector3 spawnPos = transform.position;
        int i = 0;
        foreach (double minute in minutes) // create an object for each day in the day list transform.
        {
            GameObject thisminute = Instantiate(minuteText, spawnPos, Quaternion.identity) as GameObject;
            spawnPos = new Vector3(thisminute.transform.position.x, thisminute.transform.position.y - 2.5f, thisminute.transform.position.z);
            RectTransform myAnchor = thisminute.GetComponent<RectTransform>();
            myAnchor.anchorMin = new Vector2(0.5f, 0.5f);
            myAnchor.anchorMax = new Vector2(0.5f, 0.5f);
            myAnchor.pivot = new Vector2(0.5f, 0.5f);
            thisminute.transform.SetParent(minuteList.transform);
            thisminute.transform.localScale = new Vector3(1, 1, 1);
            thisminute.name = minute.ToString();
            Text myText = thisminute.GetComponent<Text>();
            if (minute < 10)
            {
                myText.text = "0" + minute.ToString();
            }
            else { myText.text = minute.ToString(); }
            minuteGO[i] = thisminute.gameObject;
            i++;
        }


        distance = new float[minuteGO.Length];
        distanceReposition = new float[minuteGO.Length];

        // Get distance between day GO's in y axis
        int GOlast = minuteGO.Length;

       // int GOsecondLast = minuteGO.Length - 1;
        // Debug.Log(GOlast + " " + GOsecondLast);
        textDistance = (int)Mathf.Abs(minuteGO[1].GetComponent<RectTransform>().anchoredPosition.y - minuteGO[0].GetComponent<RectTransform>().anchoredPosition.y);


        SetCurrentminute();
       

    }

    // sets the scroll to the current minute now.
    public void SetCurrentminute()
    { 
        double minute = System.DateTime.Now.Minute;

        for (int i = 0; i < minutes.Length; i++)
        {
            if (minutes[i] == minute )
            {
                mintextnum = i ;
                float newY = mintextnum * textDistance;
                Vector2 newPos = new Vector2(minuteList.anchoredPosition.x, newY);
                minuteList.anchoredPosition = newPos;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < minuteGO.Length; i++)
        {
            distanceReposition[i] = center.GetComponent<RectTransform>().position.y - minuteGO[i].GetComponent<RectTransform>().position.y;
            //distance[i] = Mathf.Abs(center.transform.position.y - minuteGO[i].transform.position.y);
            distance[i] = Mathf.Abs(distanceReposition[i]);

           

        }

        float minDistance = Mathf.Min(distance); // get the minimum distance in the array

        for (int a = 0; a < minuteGO.Length; a++)
        {
            if (minDistance == distance[a])
            {
                mintextnum = a;                     // sets the number for the game object that is closest to the center.
            }
        }

        if (!dragging)
        {
            //LerpTominuteGO(mintextnum * textDistance, 10f);
            LerpTominuteGO(-minuteGO[mintextnum].GetComponent<RectTransform>().anchoredPosition.y, 10f);
        }
        myMinute = minuteGO[mintextnum].GetComponent<Text>().text;


    }

    void LerpTominuteGO(float position, float speed)
    {

        float newY = Mathf.Lerp(minuteList.anchoredPosition.y, position, Time.deltaTime * speed);
        Vector2 newPos = new Vector2(minuteList.anchoredPosition.x, newY);

        minuteList.anchoredPosition = newPos;
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
