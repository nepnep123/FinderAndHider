using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaucherCtrl : MonoBehaviour
{
    private Transform camTr;

    private Ray ray;
    private RaycastHit hit;

    public float dist = 100.0f;

    private GameObject inputField;
    private GameObject connectBtn;

    GvrControllerInputDevice gvrControllerInput;

    void OnEnable()
    {
        GvrControllerInput.OnDevicesChanged += InitController;
    }
    void OnDisable()
    {
        GvrControllerInput.OnDevicesChanged -= InitController;
    }
    private void InitController()
    {
        gvrControllerInput = GvrControllerInput.GetDevice(GvrControllerHand.Dominant);
    }

    void Start()
    {
        camTr = Camera.main.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, hit.point, Color.red, dist);

        ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out hit, dist, 1 << LayerMask.NameToLayer("BUTTON_FINDER")))
        {
            //Debug.Log("FINDER START!!!!!!!!!!!!");
            IsFinderSelectBtn();
        }
        if (Physics.Raycast(ray, out hit, dist, 1 << LayerMask.NameToLayer("BUTTON_SEEKER")))
        {
            //Debug.Log("SEEKER START!!!!!!!!!!!!");
            IsSeekerSelectBtn();
        }

    }

    public void IsFinderSelectBtn()
    {
        if (gvrControllerInput == null) return;

        bool isAppButtonClick = gvrControllerInput.GetButton(GvrControllerButton.App);

        if (isAppButtonClick)
        {
            Debug.Log("Finder Button Click");
            //들어온사람이 Finder인 경우는 True를 보낸다!!
            Launcher.instance_Launcher.Connect(true);
        }
    }

    public void IsSeekerSelectBtn()
    {
        if (gvrControllerInput == null) return;

        bool isAppButtonClick = gvrControllerInput.GetButton(GvrControllerButton.App);

        if (isAppButtonClick)
        {
            Debug.Log("Seeker Button Click");
            //들어온사람이 Seeker인 경우는 false를 보낸다!!
            Launcher.instance_Launcher.Connect(false);
        }
    }

}
