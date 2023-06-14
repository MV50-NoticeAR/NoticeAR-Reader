using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashingMaterialScript : MonoBehaviour
{
    public float fadeSpeed = 0.5f;
    private bool increase = false;

    public float moveSpeed = 15f;
    public float moveHeight = 15f;
    private bool moove;

    [SerializeField]
    private MeshRenderer _renderer;
    [SerializeField]
    private Transform _transform;

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
                _transform = transform.GetChild(0);

                _transform.localPosition = new Vector3(0f, moveHeight, 0f);
                baseMat = _renderer.material;
            }

            _playing = value;

            if (value)
            {
                moove = true;
                _renderer.material = transparentMat;
                _renderer.material.color = baseColor;
            }

            else
            {
                baseColor.a = 1f;
                moove = false;
                _transform.localPosition = new Vector3(0f, 0f, 0f);
                _renderer.material = baseMat;
                _renderer.material.color = baseColor;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!Playing) return;

        if (increase) baseColor.a += Time.deltaTime * fadeSpeed;
        else baseColor.a -= Time.deltaTime * fadeSpeed;

        _renderer.material.color = baseColor;

        if (baseColor.a <= 0) increase = true;
        if (baseColor.a >= 1) increase = false;

        if (moove) _transform.localPosition = new Vector3(0f, _transform.localPosition.y - Time.deltaTime * moveSpeed, 0f);
        else _transform.localPosition = new Vector3(0f, moveHeight, 0f);

        if (_transform.localPosition.y < 0) _transform.localPosition = new Vector3(0f, moveHeight, 0f);
    }

    public void Play() => Playing = true;

    public void Pause() => Playing = false;
}
