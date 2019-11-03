using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Globalization;

public class HourPicker : MonoBehaviour
{
    public RectTransform panel; // hold the scrolling panel.
    public RectTransform hourList; // the parent object that holds all the hours
    public GameObject hourText; // the text component of each day object.
    public RectTransform center; // the center point to campre to and snap to
    double[] hours; // array pf all the hours names
    GameObject[] hourGO;
    float[] distance; // hold the hours objects distances to the center.
    public float[] distanceReposition;
    bool dragging = false; // will be true when dragging the panel;
    int imageDistance; // holods the distance between each hours text object;
    int mintextnum; // hold the number of the closest text object to the center.
    public static string myHour;
   


    // Use this for initialization
    void Start()
    {
       int hoursInDay = 25; // allows for hours 0-24
        hours = new double[hoursInDay];
        for (int d = 0; d < hours.Length; d++)
        {
            hours[d] = d;
           
        }

        hourGO = new GameObject[hours.Length];
        //Debug.Log(hourGO.Length);
        Vector3 spawnPos = transform.position;
        int i = 0;
        foreach (double hour in hours) // create an object for each day in the day list transform.
        {
            GameObject thisHour = Instantiate(hourText, spawnPos, Quaternion.identity) as GameObject;
            spawnPos = new Vector3(thisHour.transform.position.x, thisHour.transform.position.y - 2.5f, thisHour.transform.position.z);
            RectTransform myAnchor = thisHour.GetComponent<RectTransform>();
            myAnchor.anchorMin = new Vector2(0.5f, 0.5f);
            myAnchor.anchorMax = new Vector2(0.5f, 0.5f);
            myAnchor.pivot = new Vector2(0.5f, 0.5f);
            thisHour.transform.SetParent(hourList.transform);
            thisHour.transform.localScale = new Vector3(1, 1, 1);
            thisHour.name = hour.ToString();
            Text myText = thisHour.GetComponent<Text>();
            if (hour < 10) {
                myText.text = "0" + hour.ToString();
            }
            else{ myText.text =  hour.ToString(); }
            hourGO[i] = thisHour.gameObject;
            i++;
        }


        distance = new float[hourGO.Length];
        distanceReposition = new float[hourGO.Length];
        // Get distance between day GO's in y axis
        imageDistance = (int)Mathf.Abs(hourGO[1].GetComponent<RectTransform>().anchoredPosition.y - hourGO[0].GetComponent<RectTransform>().anchoredPosition.y);
        SetCurrentHour();
    }
   

    public void SetCurrentHour()
    { // stets thye scroll for the hours to the current day
        double hour = System.DateTime.Now.Hour;

        for (int i = 0; i < hours.Length; i++)
        {
            if (hours[i] == hour)
            {
                mintextnum = i;
                float newY = mintextnum * imageDistance;
                Vector2 newPos = new Vector2(hourList.anchoredPosition.x, newY);
                hourList.anchoredPosition = newPos;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < hourGO.Length; i++)
        {
            distanceReposition[i] = center.GetComponent<RectTransform>().position.y - hourGO[i].GetComponent<RectTransform>().position.y;
            //distance[i] = Mathf.Abs(center.transform.position.y - hourGO[i].transform.position.y);
            distance[i] = Mathf.Abs(distanceReposition[i]);
        }

        float minDistance = Mathf.Min(distance); // get the minimum distance in the array

        for (int a = 0; a < hourGO.Length; a++)
        {
            if (minDistance == distance[a])
            {
                mintextnum = a;                     // sets the number for the game object that is closest to the center.
            }
        }

        if (!dragging)
        {
            //LerpTohourGO(mintextnum * imageDistance, 10f);
            LerpTohourGO(-hourGO[mintextnum].GetComponent<RectTransform>().anchoredPosition.y, 10f);
        }
        myHour = hourGO[mintextnum].GetComponent<Text>().text;


    }

    void LerpTohourGO(float position, float speed)
    {

        float newY = Mathf.Lerp(hourList.anchoredPosition.y, position, Time.deltaTime * speed);
        Vector2 newPos = new Vector2(hourList.anchoredPosition.x, newY);

        hourList.anchoredPosition = newPos;
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
