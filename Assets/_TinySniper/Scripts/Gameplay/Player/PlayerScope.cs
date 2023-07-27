using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;



/// <summary>
/// Handle scope logic
/// </summary>
public class PlayerScope : MonoBehaviour
{
    public static event Action<float, float, float> OnScoping;

    public Camera playerCam;
    [SerializeField] private GameObject scopeUI;
    [SerializeField] private GameObject weaponCamera;
    [SerializeField] private Transform weaponObj;


    public float maxZoom = 5f;
    public float minZoom = 60f;


    private void Start()
    {
        OnUnscoped();
    }

    public void OnUnscoped()
    {
        scopeUI.SetActive(false);
        weaponCamera.SetActive(true);

        playerCam.fieldOfView = minZoom;
        OnScoping?.Invoke(maxZoom, minZoom, playerCam.fieldOfView);
    }

    public void OnScoped(float FOV)
    {
        scopeUI.SetActive(true);
        weaponCamera.SetActive(false);
        playerCam.fieldOfView = FOV;
        OnScoping?.Invoke(maxZoom, minZoom, playerCam.fieldOfView);
    }

}
