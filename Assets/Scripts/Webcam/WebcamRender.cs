using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebcamRender : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.RawImage _rawImage;
    [SerializeField] bool initOnStart;



    void Start()
    {
        if (initOnStart)
            Init();
    }

    public void Init()
    {
        WebCamDevice[] devices = WebCamTexture.devices;

        // for debugging purposes, prints available devices to the console
        for (int i = 0; i < devices.Length; i++)
        {
            print("Webcam available: " + devices[i].name);
        }

        WebCamTexture tex = new WebCamTexture(devices[0].name);
        this._rawImage.texture = tex;
        _rawImage.enabled = true;
        tex.Play();
    }

    public void Stop()
    {
        _rawImage.enabled = false;
    }
}
