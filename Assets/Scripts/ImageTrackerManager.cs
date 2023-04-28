using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTrackerManager : MonoBehaviour
{
    public const string NAME1 = "Marker1";
    public const string NAME2 = "Marker2";
    public const int NUMBER_OF_MARKERS = 2;

    [SerializeField]
    private Vector3[] markerPos;
    [SerializeField]
    private GameObject ModelHandler;
    private Vector3 Orientation;
    private Quaternion initialRot, currentRotation;
   
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
        Orientation = Vector3.zero;
        initialRot = transform.rotation;
    }

    private void Update()
    {
        /*Debug.Log(arTrackedImageManager.trackables.count);
        Debug.Log(markerPos[0]);
        Debug.Log(markerPos[1]);
        Debug.Log("orientation: "+Orientation);*/
    }

    public void LateUpdate()
    {
        if(arTrackedImageManager.trackables.count == NUMBER_OF_MARKERS)
        {
            Orientation = markerPos[1] - markerPos[0];
            //ModelHandler.transform.eulerAngles = Orientation;
            currentRotation = Quaternion.LookRotation(Orientation.normalized);
            //currentRotation *= initialRot;
            ModelHandler.transform.rotation = currentRotation;
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

    private void UpdatePos(ARTrackedImage trackedImage)
    {
        if (trackedImage.trackingState != TrackingState.Tracking) { return; }
        //Debug.Log(trackedImage.referenceImage.name.ToString());
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