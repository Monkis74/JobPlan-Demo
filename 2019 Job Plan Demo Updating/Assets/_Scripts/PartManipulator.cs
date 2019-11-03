using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// used to move or rotate a  selected part on the layout panel.

public class PartManipulator : MonoBehaviour, IPointerClickHandler {
    public static GameObject itemToMove;
    GridLayoutGroup myLayout;



    public void OnPointerClick(PointerEventData eventData)
    {
      
    }

    public void MoveLeft()
    {
        if (itemToMove != null)
        {
            myLayout = itemToMove.GetComponentInParent<GridLayoutGroup>();
            RectOffset tempPadding = new RectOffset(myLayout.padding.left, myLayout.padding.right, myLayout.padding.top, myLayout.padding.bottom);
            tempPadding.right += 10;
            tempPadding.left -= 10;
            myLayout.padding = tempPadding;
        }
    }
    public void MoveRight()
    {
        if (itemToMove != null)
        {
            myLayout = itemToMove.GetComponentInParent<GridLayoutGroup>();
            RectOffset tempPadding = new RectOffset(myLayout.padding.left, myLayout.padding.right, myLayout.padding.top, myLayout.padding.bottom);
            tempPadding.right -= 10;
            tempPadding.left += 10;
            myLayout.padding = tempPadding;
        }
    }
    public void MoveUp()
    {
        if (itemToMove != null)
        {
            myLayout = itemToMove.GetComponentInParent<GridLayoutGroup>();
            RectOffset tempPadding = new RectOffset(myLayout.padding.left, myLayout.padding.right, myLayout.padding.top, myLayout.padding.bottom);
            tempPadding.top -= 10;
            tempPadding.bottom += 10;
            myLayout.padding = tempPadding;
        }
    }
    public void MoveDown()
    {
        if (itemToMove != null)
        {
            myLayout = itemToMove.GetComponentInParent<GridLayoutGroup>();
            RectOffset tempPadding = new RectOffset(myLayout.padding.left, myLayout.padding.right, myLayout.padding.top, myLayout.padding.bottom);
            tempPadding.top += 10;
            tempPadding.bottom -= 10;
            myLayout.padding = tempPadding;
        }
    }

    public void RotLeft()
    {
        if (itemToMove != null)
        {
            Transform myTrans = itemToMove.GetComponent<Transform>();
            myTrans.eulerAngles = new Vector3(myTrans.eulerAngles.x, myTrans.eulerAngles.y, myTrans.eulerAngles.z + 15f);
        }
    }

    public void RotRight()
    {
        if (itemToMove != null)
        {
            Transform myTrans = itemToMove.GetComponent<Transform>();
            myTrans.eulerAngles = new Vector3(myTrans.eulerAngles.x, myTrans.eulerAngles.y, myTrans.eulerAngles.z - 15f);
        }
    }

}
