using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Globalization;

public class YearPicker : MonoBehaviour
{
    public RectTransform panel; // hold the scrolling panel.
    public RectTransform yearList; // the parent object that holds all the years
    public GameObject yearText; // the text component of each year object.
    public RectTransform center; // the center point to campre to and snap to
    string[] years; // array pf all the years names
    GameObject[] yearGO;
    float[] distance; // hold the years objects distances to the center.
    bool dragging = false; // will be true when dragging the panel;
    int imageDistance; // holods the distance between each years text object;
    int mintextnum; // hold the number of the closest text object to the center.
    public static string myYear;
    public GameObject monthPicker;
    public GameObject dayPicker;

    // Use this for initialization
    void Start()
    {
        monthPicker.SetActive(false);
        dayPicker.SetActive(false);

        years = new string[3];
        
        int currentYear = System.DateTime.Now.Year;
        currentYear -= 1;
        for (int y = 0; y < years.Length; y++)
        {
            years[y] = currentYear.ToString();
            currentYear++;
           
        }
        
        yearGO = new GameObject[years.Length];
        Vector3 spawnPos = transform.position;
        int i = 0;
        foreach (string year in years) // create an object for each year in the year list transform.
        {
            GameObject thisyear = Instantiate(yearText, spawnPos, Quaternion.identity) as GameObject;
            spawnPos = new Vector3(thisyear.transform.position.x, thisyear.transform.position.y - 2.5f, thisyear.transform.position.z);
            RectTransform myAnchor = thisyear.GetComponent<RectTransform>();
            myAnchor.anchorMin = new Vector2(0.5f, 0.5f);
            myAnchor.anchorMax = new Vector2(0.5f, 0.5f);
            myAnchor.pivot = new Vector2(0.5f, 0.5f);
            thisyear.transform.SetParent(yearList.transform);
            thisyear.transform.localScale = new Vector3(1, 1, 1);
            thisyear.name = year;
            Text myText = thisyear.GetComponent<Text>();
            myText.text = year;
            yearGO[i] = thisyear.gameObject;
            i++;
        }


        distance = new float[yearGO.Length];

        // Get distance between year GO's in y axis
        imageDistance = (int)Mathf.Abs(yearGO[1].GetComponent<RectTransform>().anchoredPosition.y - yearGO[0].GetComponent<RectTransform>().anchoredPosition.y);
       // print(imageDistance);

        SetCurrentyear();


    }
    

    public void SetCurrentyear()
    { // stets thye scroll for the years to the current year
        string yearName = System.DateTime.Now.Year.ToString();
       // Debug.Log("Current Year " + yearName);
        for (int i = 0; i < years.Length; i++)
        {
            if (years[i] == yearName)
            {
                mintextnum = i;
               // Debug.Log("Year i" + years[i] + i);
                float newY = mintextnum * imageDistance;
                Vector2 newPos = new Vector2(yearList.anchoredPosition.x, newY);
                yearList.anchoredPosition = newPos;
                monthPicker.SetActive(true);
                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < yearGO.Length; i++)
        {
            distance[i] = Mathf.Abs(center.transform.position.y - yearGO[i].transform.position.y);
        }

        float minDistance = Mathf.Min(distance); // get the minimum distance in the array

        for (int a = 0; a < yearGO.Length; a++)
        {
            if (minDistance == distance[a])
            {
                mintextnum = a;                     // sets the number for the game object that is closest to the center.
            }
        }

        if (!dragging)
        {
            LerpToyearGO(mintextnum * imageDistance, 10f);
            monthPicker.SetActive(true);
        }

        myYear = yearGO[mintextnum].GetComponent<Text>().text;

       // Debug.Log(myYear);

    }

    void LerpToyearGO(int position, float speed)
    {
        float newY = Mathf.Lerp(yearList.anchoredPosition.y, position, Time.deltaTime * speed);
        Vector2 newPos = new Vector2(yearList.anchoredPosition.x, newY);
        yearList.anchoredPosition = newPos;
    }

    public void StartDrag()
    {
        dragging = true;
        MonthPicker.dayPickerStarted = false;
        monthPicker.SetActive(false);
        dayPicker.SetActive(false);
        
    }

    public void EndDrag()
    {
        dragging = false;
        monthPicker.SetActive(true);
       // dayPicker.SetActive(false);
    }


}