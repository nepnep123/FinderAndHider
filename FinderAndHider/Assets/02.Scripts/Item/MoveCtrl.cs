using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleVR;

public class MoveCtrl : Photon.MonoBehaviour
{
    private float speed = 3.0f; //움직임 속도

    private float verticalVelocity;
    private float gravity = 20.0f; //점프 거리 
    private float jumpForce = 10.0f; //점프 힘 

    public static bool isNotMove = false; //사물 컨트롤 on/off


    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    //필요한 컴포넌트(Private)
    private Transform tr;
    private CharacterController cc;
    private Transform camTr;

    //필요한 스크립트 컴포넌트 
    GvrControllerInputDevice gvrControllerInput;

    void OnEnable()
    {
        GvrControllerInput.OnDevicesChanged += InitController;
    }
    void OnDisable()
    {
        GvrControllerInput.OnDevicesChanged -= InitController;
    }



    void Start()
    {
        tr = GetComponent<Transform>();
        cc = GetComponent<CharacterController>();
        camTr = Camera.main.GetComponent<Transform>();
    }


    void Awake()
    {
        //자기 자신이면 자기만 컨트롤 할수 있게 한다. 
        if (photonView.isMine)
        {
            MoveCtrl.LocalPlayerInstance = this.gameObject;
        }
        //자기 자신꺼 아니면 전부 off
        else
        {
            GetComponentInChildren<Camera>().enabled = false;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        //연결은 됫지만 본인이 아닐시 종료 !! 
        if (photonView.isMine == false && PhotonNetwork.connected == true)
        {
            return;
        }

        //게임 오버 로직 구현 !!!



        bool isButtonClick = gvrControllerInput.GetButton(GvrControllerButton.App);
        //if (isNotMove) return;

        MoveTouchPad();



        //터치 패드를 클릭했을 때 (점프 기능)
        if (cc.isGrounded)
        {
            verticalVelocity = -gravity * Time.deltaTime;
            if (isButtonClick)
            {
                verticalVelocity = jumpForce;
                //Debug.Log("groundedddd");
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        Vector3 jumpVector = new Vector3(0, verticalVelocity, 0);
        cc.Move(jumpVector * Time.deltaTime);
    }


    private void InitController()
    {
        gvrControllerInput = GvrControllerInput.GetDevice(GvrControllerHand.Dominant);
    }


    private void MoveLookAt(int facing)
    {
        Vector3 heading = camTr.forward;

        heading.y = 0.0f;

        Debug.DrawRay(tr.position, heading.normalized * 1.0f, Color.red);

        cc.SimpleMove(heading * speed * facing);
    }

    private void MoveTouchPad()
    {
        if (gvrControllerInput == null) return;

        bool isTouchPad = gvrControllerInput.GetButton(GvrControllerButton.TouchPadTouch);

        //터치 패드가 터치되었을 때 
        if (isTouchPad)
        {
            Vector2 pos = gvrControllerInput.TouchPos;
            Debug.LogFormat("Touch position x = {0}, y = {1}", pos.x, pos.y);

            int facing = 0;

            facing = (pos.y > 0.0f) ? 1 : -1;

            MoveLookAt(facing);
        }
    }

}
