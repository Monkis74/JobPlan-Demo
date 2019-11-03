using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

// control the traffic layout image on page2

public class ChosenLayout : MonoBehaviour {
    public string bytesString;  
    byte[] textureBytes;
    RawImage myImage;  // the base object for the layout image
    public Texture2D placeholder; // the texure to apply to myImage if it is null
    int width; // the rect width of myImage
    int height; // the height of myImage

	// Use this for initialization
	void Start () {
        myImage = GetComponent<RawImage>();
        width = (int)myImage.rectTransform.rect.width;        
        height = (int)myImage.rectTransform.rect.height;
    }

    // Set the texture of the image placeholder on page2
    public void SetImage(Texture2D myTex)
    {
        myImage.texture = myTex;
        textureBytes = myTex.EncodeToPNG();
       // Debug.Log(textureBytes.Length);
        //bytesString = Convert.ToBase64String(textureBytes);
        bytesString = GetTextureString(textureBytes);
        //Debug.Log(bytesString);
       // myButton.SetActive(false);
    }

    // convert a string from a loaded file to a usable image for the layout placeholder
    public void ConvertStringToTexture(string imageString)
    {
        byte[] imageBytes = GetTextureBytes(imageString);
        if (imageBytes.Length == 0)
        {
            myImage.texture = placeholder;
        }

        if (imageBytes.Length > 0)
        {
            Texture2D thisTex = new Texture2D(width, height);
            thisTex.LoadImage(imageBytes);
            thisTex.Apply();
           // byte[] bytes = thisTex.EncodeToPNG();
            SetImage(thisTex);
        }
    }


    // get a string to save to a binary file.
    private string GetTextureString(byte[] bytes)
    {
        string something = Convert.ToBase64String(bytes);
        return something;
    }



    // get the bites from a saved string in a saved file
    private byte[] GetTextureBytes(string str)
    {
        byte[] toBytes = Convert.FromBase64String(str);
        return toBytes;

    }
}
