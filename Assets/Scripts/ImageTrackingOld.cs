using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.SceneManagement;
/*
[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTrackingOld : MonoBehaviour
{
    [Header("First string must be the name of the picture in the library, second the prefab for it.")]
    [SerializeField]
    StringGameobjectDictionary objectsToPlace = null;//<<< dictionnary to map pictures to their gameobject

    //using open source code from github: https://github.com/azixMcAze/Unity-SerializableDictionary
    public IDictionary<string, GameObject> StringGameobjectDictionary
    {
        get { return objectsToPlace; }
        set { objectsToPlace.CopyFrom(value); }
    }

    private Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>(); //<<< dictionnary to stock the prefabs once instantiated
    private ARTrackedImageManager trackedImageManager; //<<< manager to detect 2D images

    private void Awake()
    {
        //initialize the manager
        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();

        //instantiate the prefabs on load but disables them to prepare the scene
        foreach (KeyValuePair<string, GameObject> prefab in objectsToPlace)
        {
            GameObject newPrefab = Instantiate(prefab.Value, Vector3.zero, Quaternion.identity);
            newPrefab.name = prefab.Key;
            spawnedPrefabs.Add(prefab.Key.ToString(), newPrefab);
            newPrefab.SetActive(false);
        }

        //clear the prefab dictionnary to free memory
        objectsToPlace.Clear();
    }

    //add the ImageChanged function to the trackedImagesChanges event of the manager to add our functionalities
    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += ImageChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= ImageChanged;
    }

    //updates the display of prefabs depending on the images detected by calling the UpdateImage function
    private void ImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdateImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdateImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            //spawnedPrefabs[trackedImage.name].SetActive(false);
            if (spawnedPrefabs.ContainsKey(name)) Destroy(spawnedPrefabs[trackedImage.name]);
        }
    }

    //updates the positions and rotations of prefabs depending on the given detected image
    private void UpdateImage(ARTrackedImage trackedImage)
    {
        //link between the tracked image in parameters and the prefabs is made thanks to their name
        string name = trackedImage.referenceImage.name.ToString();

        //security test
        if (!spawnedPrefabs.ContainsKey(name)) return;
        GameObject prefab = spawnedPrefabs[name];

        if (trackedImage.trackingState != TrackingState.Tracking)
        {
            prefab.SetActive(false);
            return;
        }


        Vector3 position = trackedImage.transform.position;
        Quaternion rotation = trackedImage.transform.rotation;
        prefab.transform.position = position;
        prefab.transform.rotation = rotation;
        prefab.SetActive(true);
    }
}
*/