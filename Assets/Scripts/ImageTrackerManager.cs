using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTrackerManager : MonoBehaviour
{
    public const string NAME1 = "Marker1";
    public const string NAME2 = "Marker2";
    public const int NUMBER_OF_MARKERS = 2;

    [SerializeField]
    private Vector3[] markerPos; //<<< current marker positions
    [SerializeField]
    private GameObject ModelHandler; //<<< empty parent handling the complete model
    private Vector3 orientation; //<<< vectorial direction of the ModelHandler, computed with makerPos
   
    //create the “trackable” manager to detect 2D images
    private ARTrackedImageManager arTrackedImageManager;

    void Awake()
    {
        //initialized tracked image manager  
        arTrackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    private void Start()
    {
        markerPos = new[] { new Vector3(), new Vector3() };
        orientation = Vector3.zero;
    }


    public void LateUpdate()
    {
        //if 2 markers were detected, updates the ModelHandler orientation
        if(arTrackedImageManager.trackables.count == NUMBER_OF_MARKERS)
        {
            orientation = markerPos[1] - markerPos[0];
            ModelHandler.transform.LookAt(ModelHandler.transform.position + orientation);
        }
    }


    //when the tracked image manager is enabled add binding to the tracked 
    //image changed event handler by calling a method to iterate through 
    //image reference’s changes 
    private void OnEnable()
    {
        arTrackedImageManager.trackedImagesChanged += OnImageChanged;
    }

    //when the tracked image manager is disabled remove binding to the 
    //tracked image changed event handler by calling a method to iterate 
    //through image reference’s changes
    private void OnDisable()
    {
        arTrackedImageManager.trackedImagesChanged -= OnImageChanged;
    }
    
    public void OnImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        // for each tracked image that has been added
        foreach (var addedImage in args.added)
        {
            UpdatePos(addedImage);
        }

        // for each tracked image that has been updated
        foreach (var updated in args.updated)
        {
            UpdatePos(updated);
        }

        // for each tracked image that has been removed  
        foreach (var trackedImage in args.removed)
        {
            
        }
    }

    //called for each detected refenrence picture every frame
    private void UpdatePos(ARTrackedImage trackedImage)
    {
        //only considers properly tracked images
        if (trackedImage.trackingState != TrackingState.Tracking) { return; }

        //Image is the position reference: the position of the ModelHandler is updated
        if (trackedImage.referenceImage.name.ToString() == NAME1)
        {
            markerPos[0] = trackedImage.transform.position;
            ModelHandler.transform.position = trackedImage.transform.position;
        }
        if (trackedImage.referenceImage.name.ToString() == NAME2)
        {
            markerPos[1] = trackedImage.transform.position;
        }
    }
}