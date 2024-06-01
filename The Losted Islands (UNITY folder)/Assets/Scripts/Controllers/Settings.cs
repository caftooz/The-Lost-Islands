using DarkHorizon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public Slider sensitivitySlider;
    public Slider smoothingSlider;
    public Slider fovSlider;
    public Toggle postProcToggle;

    [SerializeField] Player pl;
    [SerializeField] RotatePl rPl;
    [SerializeField] Camera cam;

    private void Start()
    {
        sensitivitySlider.value = rPl.sensitivity;
        smoothingSlider.value = rPl.smoothing;
        fovSlider.value = cam.fieldOfView;
    }
    private void Update()
    {
        rPl.sensitivity = sensitivitySlider.value;
        rPl.smoothing = smoothingSlider.value;
        cam.fieldOfView = fovSlider.value;
        cam.GetComponent<Volume>().enabled = postProcToggle.isOn;
    }
}
