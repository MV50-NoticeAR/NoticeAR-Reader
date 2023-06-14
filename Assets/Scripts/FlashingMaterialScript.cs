using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashingMaterialScript : MonoBehaviour
{
    public float fadeSpeed = 0.5f;
    private bool increasing = false;

    [SerializeField]
    private MeshRenderer _renderer;

    public Material transparentMat;
    public Material baseMat;

    [SerializeField]
    public Color baseColor;

    private bool _playing;
    private bool Playing
    {
        get => _playing;
        set
        {
            if (_renderer == null)
            {
                _renderer = transform.GetChild(0).GetComponent<MeshRenderer>();
                baseMat = _renderer.material;
            }

            _playing = value;

            if (value)
            {
                _renderer.material = transparentMat;
                _renderer.material.color = baseColor;
            }

            else
            {
                baseColor.a = 1f;
                _renderer.material = baseMat;
                _renderer.material.color = baseColor;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!Playing) return;

        if (increasing)
        {
            baseColor.a += Time.deltaTime * fadeSpeed;
        }
        else
        {
            baseColor.a -= Time.deltaTime * fadeSpeed;
        }

        _renderer.material.color = baseColor;

        if (baseColor.a <= 0) increasing = true;
        if (baseColor.a >= 1) increasing = false;
    }

    public void Play() => Playing = true;

    public void Pause() => Playing = false;
}
