using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using UnityEngine.Networking;
using System.Threading;

public class SelectLayoutController : MonoBehaviour {
    public RawImage image; // used for loading textures only.
    public RawImage myImage; // the rawimage on this page to set a layout to.
    public GameObject tlpickerPanel; // the parent gameobject to control the enabled disabled of the panel.
    TLPicker tlpicker; //thegameobject that has the tlpicker script attached.
    CustomLayoutCapture customLayoutPanel;
    public GameObject customPanel;
    public Transform myParent;
    public int numOfTlLayouts;
    string folderPath;
    public static string[] layouts; // array pf all the layouts in the folder.
    public static List<string> layoutStrings = new List<string>();
    public static bool _layoutsLoaded = false;
    public static bool _TLArraysFlushed = true;
    public Texture2D[] textureImages;
    string sharepointPath;
    public static SelectLayoutController layoutControllerInstance;
    private bool _startCalled = false;
    private bool _currentlyLoading = false; // used to check if images are already being loaded and cancel recall of ethod if pages are changed quick between page 2 and another back and forth.

    private void Awake()
    {
        layoutControllerInstance = this;
        UnityThread.initUnityThread();
    }

    // Use this for initialization
    void Start () {
        if (_startCalled) // Tetsing bug where this is started again from somewhere while layoutimage array is loading.
        {
            return;
        }
        sharepointPath = SaveFile.sharepointPath;
        folderPath = sharepointPath + "/Job Plans/FormData/MTOLayouts";
        UnityEngine.Debug.Log("SelectLayoutController Started");
        _TLArraysFlushed = true;
        _layoutsLoaded = false;
        _startCalled = true;
    }

    public void StartLayoutChoice()
    {      // start the coroutine listed by a button press.
        StartCoroutine(ChooseMTOlayout());
    }

    public IEnumerator ChooseMTOlayout()
    {  // start the mto layout panel picker and wait until a layout is picked.
                                           
        UnityEngine.Debug.Log("ChooseMTOLAyout called");
        GameObject NewTLPickerPanel = Instantiate(tlpickerPanel) as GameObject;
        NewTLPickerPanel.transform.SetParent(myParent, false);
        tlpicker = GameObject.FindObjectOfType<TLPicker>(); // there is only one, using this format to find it if it's disabled.
        tlpicker.SetPanelToPopulate(myImage);
        yield return new WaitUntil(() => tlpicker.layoutChosen == true);
        tlpicker.layoutChosen = false;
        UnityEngine.Debug.Log(EventSystem.current.currentSelectedGameObject.transform.parent.gameObject);
        Destroy(EventSystem.current.currentSelectedGameObject.transform.parent.gameObject);
        Resources.UnloadUnusedAssets();
    }

    public void StartCustomLayout()
    {       
        StartCoroutine(ChooseCustomLayout());
    }

    public IEnumerator ChooseCustomLayout()
    {
        GameObject NewCustomPanel = Instantiate(customPanel);
        NewCustomPanel.name = "CustomLayoutPanel";
        NewCustomPanel.transform.SetParent(myParent, false);
        customLayoutPanel = GameObject.FindObjectOfType<CustomLayoutCapture>();
        customLayoutPanel.SetPanelToPopulate(myImage);
        yield return new WaitUntil(() => customLayoutPanel.layoutChosen == true);
        customLayoutPanel.layoutChosen = false;
        Destroy(NewCustomPanel.gameObject);
    }

    public void BeginLoadImages(string callingFunction)
    {
        //UnityEngine.Debug.Log("Load images Called by " + callingFunction);
        LoadImages(); 
      
    }

   public  async Task LoadImages()
    {
        if (_currentlyLoading)
        {
          return;
        }
        _currentlyLoading = true;
        if (!_TLArraysFlushed)
        {
         StartCoroutine(FlushArrays());          
        }        
        SelectLayoutController._layoutsLoaded = false;        
        sharepointPath = SaveFile.sharepointPath;
        folderPath = sharepointPath + "/Job Plans/FormData/MTOLayouts";
        layouts = Directory.GetFiles(folderPath, "*.jpg");
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        for (int i = 0; i < layouts.Length; i++)
        {
            layoutStrings.Add(layouts[i]);
        }
        
        layoutStrings.Sort();
        numOfTlLayouts = layoutStrings.Count;       
        textureImages = new Texture2D[numOfTlLayouts];
        
        for (int i = 0; i < numOfTlLayouts; i++)    // Load the images in the selected folder into the array for populating the scroll selection.
        {
           await Task.Run(() =>ReadImageAsync(layoutStrings[i], i));           
        }

        stopWatch.Stop();
        UnityEngine.Debug.Log("Elapsed Time of Load = " + stopWatch.Elapsed);
        UnityEngine.Debug.Log("Load Images Finished");
        SelectLayoutController._layoutsLoaded = true;
        SelectLayoutController._TLArraysFlushed = false;
        _currentlyLoading = false;
    }

    public async Task ReadImageAsync(string path, int i)
    {
        string filePath = path;
        UnityEngine.Debug.Log("Read ASync Called");
        //Use ThreadPool to avoid freezing
        ThreadPool.QueueUserWorkItem(delegate
        {
            bool success = false;
            byte[] imageBytes;
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);

            try
            {
                int length = (int)fileStream.Length;
                imageBytes = new byte[length];
                int count;
                int sum = 0;
                // read until Read method returns 0
                while ((count = fileStream.Read(imageBytes, sum, length - sum)) > 0)
                    sum += count;
                success = true;
            }
            finally
            {
                fileStream.Close();
            }

            //Create Texture2D from the imageBytes in the main Thread if file was read successfully
            if (success)
            {
                UnityThread.executeInUpdate(async () =>
                {
                    Texture2D tex = new Texture2D(400, 700);
                    tex.LoadImage(imageBytes);
                    textureImages[i] = tex;                    
                });                
            }
        });
    }

    public void BeginFlushArrays()
        {
        StartCoroutine(FlushArrays());        
        }

    public IEnumerator FlushArrays()
    {
        UnityEngine.Debug.Log("Flush arrays called");
        if (SelectLayoutController._TLArraysFlushed)
        {
            UnityEngine.Debug.Log("Arrays already flushed");
            SelectLayoutController._TLArraysFlushed = true;
            yield break;
        }
        if (!SelectLayoutController._layoutsLoaded)
        {
            UnityEngine.Debug.Log("Array Flush waiting for layouts to finish loading");
            yield return new WaitUntil(() => SelectLayoutController._layoutsLoaded == true);
        }        
        UnityEngine.Debug.Log("FlushArrays called");
        for (int i = 0; i < numOfTlLayouts; i++)
        {
            Texture2D.Destroy(textureImages[i]);
            textureImages[i] = new Texture2D(0, 0);
            textureImages[i] = null;
        }
        textureImages = null;     
        SelectLayoutController.layoutStrings = new List<string>();
        SelectLayoutController.layouts = null;
        SelectLayoutController._layoutsLoaded = false;
        SelectLayoutController._TLArraysFlushed = true;
        Resources.UnloadUnusedAssets();
        if (PageController.selectedPage.name != "Page2")
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
