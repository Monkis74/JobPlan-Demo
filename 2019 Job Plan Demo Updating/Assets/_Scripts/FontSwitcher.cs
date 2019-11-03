using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// switch the font of all the text object in the project
// used to clear up fuzzy text problem rather than change very font manually.
public class FontSwitcher : MonoBehaviour {
    public Text[] allText;

    void Start()
    {
        allText = Resources.FindObjectsOfTypeAll<Text>();
        Font newFont = (Font)Resources.Load("ARIAL");
        Font newBoldFont = (Font)Resources.Load("ARIALBD");
        Font unifont = (Font)Resources.Load("unifont");
        foreach (Text thisText in allText)
        {
            if (thisText.tag != "OSK Key")
            {

            }
            {
                thisText.font = newFont;
                if (thisText.fontStyle == FontStyle.Bold)
                {
                    thisText.font = newBoldFont;
                }
            }
            if (thisText.tag == "OSK Key")
            {
                thisText.font = unifont;
            }
            //thisText.fontSize -= 1;
        }
        allText = null;
    }

    

    // Update is called once per frame
    void Update () {
		
	}
}
