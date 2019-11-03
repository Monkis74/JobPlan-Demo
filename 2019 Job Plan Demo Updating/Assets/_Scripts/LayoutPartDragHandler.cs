using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LayoutPartDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler {

    public static GameObject itemBeingdDragged; // item on the screen selected for dragging.
    Vector3 startPosition; // start pos of itembeingdragged.
    Transform startParent; // original parent of the itembeingdragged
    Transform lastParent; // the last parent if moving on the layout board.
    public static GameObject itemToManipulate; // the selected item to be manipulated by the control panel, will have a glow(halo)
    DrawingFrame[] drawingFrame; // the drawing frame 
    PencilController pencilController; // the script to cotrol the pencil behaviour.
    RectTransform myRect; // the recttransform  of this game object. 
    public float width; // width of my rect
    public float height; // heigth of my rect
    Behaviour Halo; // the halo for each item, active when selected.

    void Awake() {
        
        
        pencilController = FindObjectOfType<PencilController>();
    }

    void Start() {
        drawingFrame = Resources.FindObjectsOfTypeAll<DrawingFrame>();  // there should only be one drawing frame, it is contained in an array to find it when disabled.
        myRect = GetComponent<RectTransform>();
        Halo = (Behaviour)GetComponent("Halo");
        Halo.enabled = false;

        // Debug.Log("drawing frame Length " + drawingFrame.Length);

    }

    // start the drag behaviour of the itembeing dragged
    // disable the drawing frame if it is active so the pencil will not work.
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBegindrag");
        int drawingFrameIndex = drawingFrame.Length;
       // Debug.Log("drawing frame Length " + drawingFrame.Length);
        if (drawingFrame[0].gameObject.activeSelf == true)
        {
            drawingFrame[0].gameObject.SetActive(false);
        }
        itemBeingdDragged = gameObject;
        startPosition = transform.position;
        startParent = transform.parent;
        lastParent = transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        
       
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 30);
        transform.position = Camera.main.ScreenToWorldPoint(mousePos);
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if (itemBeingdDragged.transform.parent.parent.name == "LayoutSlots")
        {
            PartManipulator.itemToMove = gameObject;
        }
        itemBeingdDragged = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (transform.parent == startParent)
        {
            transform.position = startPosition;
        }
        if (transform.parent != startParent && startParent.name == "PartSlot") {
            lastParent = transform.parent;
            GridLayoutGroup mySlotSize = GetComponentInParent<GridLayoutGroup>();
            mySlotSize.cellSize = new Vector2(width, height);
            GameObject thisImage = GetComponent<Image>().gameObject;
            GameObject replacementPart = Instantiate(thisImage) as GameObject;
            replacementPart.transform.SetParent(startParent);
            replacementPart.transform.position = startPosition;
            replacementPart.transform.localScale = new Vector3(1, 1, 1);
            
            replacementPart.name = gameObject.name; 
        }
        if (transform.parent != lastParent) { // deal with moving from slot to slot on the layout board
            GridLayoutGroup mySlotSize = GetComponentInParent<GridLayoutGroup>();
            mySlotSize.cellSize = new Vector2(width, height);
            lastParent = transform.parent;
        }
        
    }

    // set the item to be moved by the control panel buttons.
    public void OnPointerDown(PointerEventData eventData)
    {
        if (gameObject.transform.parent.parent.name == "LayoutSlots")
        {
            PartManipulator.itemToMove = gameObject;
        }

        
        
    }

    private void Update()
    {
        if (PartManipulator.itemToMove == gameObject)
        {
            Halo.enabled = true;
        }
        if (PartManipulator.itemToMove != gameObject)
        {
            Halo.enabled = false;
        }
    }




}