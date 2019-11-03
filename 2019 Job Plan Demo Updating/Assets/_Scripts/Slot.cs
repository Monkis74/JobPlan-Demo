using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler  {
    Color startColor;
    
    public GameObject item
    {
        get{
            if(transform.childCount > 0)
            {
                return transform.GetChild(0).gameObject;
            }
            return null;
        }
    }
    void Start()
    {
        startColor = GetComponent<Image>().color;
       
    }


    public void OnDrop(PointerEventData eventData)
    {
        if (!item)
        {
            if (transform.name == "TrashSlot")
            {
               // Debug.Log("destroying item");
                Destroy(LayoutPartDragHandler.itemBeingdDragged);
            }
            if (LayoutPartDragHandler.itemBeingdDragged != null)
            {
                LayoutPartDragHandler.itemBeingdDragged.transform.SetParent(transform);
               // Debug.Log(transform.name);
            }
           
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Image>().color = new Color(.5f,.5f,.5f,0.25f);
        //if (!item)
        //{
        //    LayoutPartDragHandler.itemBeingdDragged.transform.SetParent(transform);
        //}
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Image>().color = startColor;
        //if (!item)
        //{
        //    LayoutPartDragHandler.itemBeingdDragged.transform.SetParent(transform);
        //}
    }
}
