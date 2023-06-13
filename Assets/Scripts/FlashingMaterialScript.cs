using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashingMaterialScript : MonoBehaviour
{
    public float fadeSpeed = 0.5f;
    private bool increasing;
    
    [SerializeField]
    private MeshRenderer _renderer;

    [SerializeField]
    private Color color;

    // Start is called before the first frame update
    void Start()
    {
        //getchild because of the hierarchy of the created gameobject in modeldisplay>display
       _renderer = this.transform.GetChild(0).GetComponent<MeshRenderer>();
        color = _renderer.material.color;
       increasing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (increasing)
        {
           color.a += Time.deltaTime * fadeSpeed;
        }
        else
        {
            color.a -= Time.deltaTime * fadeSpeed;
        }
        _renderer.material.color = color;

        if (color.a <= 0) increasing = true;
        if (color.a >= 1) increasing = false;
    }

    public void RemoveScript()
    {
        color.a = 1f;
        try {
            _renderer.material.color = color;
        } catch {}
        
        Destroy(this);
    }
}
