using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class MainScript : MonoBehaviour, ITrackableEventHandler
{

    private TrackableBehaviour mTrackableBehaviour;
    public GameObject Play;
    public GameObject Information;
    public GameObject ModalInfo;
    public GameObject AreaSuggest;
    
    string subject = "Publicis Groupe App";
    string body = "App de realidade virtual";

    void Awake()
    {
        Play.SetActive(false);
        ModalInfo.SetActive(false);
        Screen.orientation = ScreenOrientation.Portrait;
    }
    // Start is called before the first frame update
    void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {

        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
            Play.SetActive(true);
            Information.SetActive(false);
            AreaSuggest.SetActive(false);
            OnTrackingFound();
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NO_POSE)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
            Play.SetActive(false);
            Information.SetActive(true);
            AreaSuggest.SetActive(true);
            OnTrackingLost();
        }
        else
        {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            Play.SetActive(false);
            Information.SetActive(true);
            OnTrackingLost();
        }
    }

    protected virtual void OnTrackingFound()
    {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Enable rendering:
        foreach (var component in rendererComponents)
            component.enabled = true;

        // Enable colliders:
        foreach (var component in colliderComponents)
            component.enabled = true;

        // Enable canvas':
        foreach (var component in canvasComponents)
            component.enabled = true;
    }


    protected virtual void OnTrackingLost()
    {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Disable rendering:
        foreach (var component in rendererComponents)
            component.enabled = false;

        // Disable colliders:
        foreach (var component in colliderComponents)
            component.enabled = false;

        // Disable canvas':
        foreach (var component in canvasComponents)
            component.enabled = false;
    }

    public void RefreshScene() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }


    public void ShowModal() {
        Information.SetActive(false);
        ModalInfo.SetActive(true);
    }

    public void HideModal() {
        ModalInfo.SetActive(false);
        Information.SetActive(true);
    }

    public void PlayAnimation() {

    }

    public void Share() {
        //execute the below lines if being run on a Android device
        #if UNITY_ANDROID
            //Refernece of AndroidJavaClass class for intent
            AndroidJavaClass intentClass = new AndroidJavaClass ("android.content.Intent");
            //Refernece of AndroidJavaObject class for intent
            AndroidJavaObject intentObject = new AndroidJavaObject ("android.content.Intent");
            //call setAction method of the Intent object created
            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            //set the type of sharing that is happening
            intentObject.Call<AndroidJavaObject>("setType", "text/plain");
            //add data to be passed to the other activity i.e., the data to be sent
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), subject);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), body);
            //get the current activity
            AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
            //start the activity by sending the intent data
            currentActivity.Call ("startActivity", intentObject);
        #endif
    }
     
}
