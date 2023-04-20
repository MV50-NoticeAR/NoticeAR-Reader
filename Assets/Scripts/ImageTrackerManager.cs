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

    public Vector3[] markerPos;
   
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
            UpdatePos(addedImage.name, addedImage.transform.position);
        }

        // for each tracked image that has been updated
        foreach (var updated in args.updated)
        {
            UpdatePos(updated.name, updated.transform.position);
        }

        // for each tracked image that has been removed  
        foreach (var trackedImage in args.removed)
        {
            
        }
    }

    private void UpdatePos(string s, Vector3 pos)
    {
        if (s == NAME1)
        {
            markerPos[1] = pos;
        }
        if (s == NAME2)
        {
            markerPos[2] = pos;
        }
    }
}