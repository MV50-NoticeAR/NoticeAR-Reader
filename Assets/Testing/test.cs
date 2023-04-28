using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public Vector3 rotTest;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.forward = transform.forward + new Vector3(1, 0, 0);
        //transform.Rotate(0, 1*Time.deltaTime, 0);
        //Debug.Log(transform.forward);

        transform.eulerAngles = rotTest;
    }
}
