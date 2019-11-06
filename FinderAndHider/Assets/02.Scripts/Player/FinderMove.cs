using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleVR;
using UnityEngine.UI;

public class FinderMove : Photon.MonoBehaviour
{
    public static FinderMove instance_finderMove;
    public Text touchChance_txt; //터치 카운트 txt
    private int touchChanceCount = 10;

    public float speed = 6.0f; //플레이어 움직임

    private GameObject tagName; //Hider 태그 변수

    //Animator 파라미터 변수
    private int hashIsRun = Animator.StringToHash("IsRun");

    private float verticalVelocity;
    private float gravity = 20.0f; //점프 거리 
    private float jumpForce = 10.0f; //점프 힘 

    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    //필요한 컴포넌트
    private Transform tr;
    private Transform camTr;
    private CharacterController cc;
    private Animator anim;

    int facing = 0;

    GvrControllerInputDevice gvrControllerInput;


    void OnEnable()
    {
        GvrControllerInput.OnDevicesChanged += InitController;
    }

    void OnDisable()
    {
        GvrControllerInput.OnDevicesChanged -= InitController;
    }

    // Start is called before the first frame update

    void Start()
    {
        tr = GetComponent<Transform>();
        cc = GetComponent<CharacterController>();
        camTr = Camera.main.GetComponent<Transform>();
        //자식 오브젝트의 Animator를 가져온다
        anim = GetComponentInChildren<Animator>();

    }

    void Awake()
    {
        if (photonView.isMine)
        {
            FinderMove.LocalPlayerInstance = this.gameObject;
        }
        else
        {
            GetComponentInChildren<Camera>().enabled = false;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void FindHiderInfo(GameObject obj)
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.isMine == false && PhotonNetwork.connected == true)
        {
            return;
        }

        MoveTouchPad();
        CheckButton();
    }


    void InitController()
    {
        gvrControllerInput = GvrControllerInput.GetDevice(GvrControllerHand.Dominant);
    }

    void MoveTouchPad()
    {
        if (gvrControllerInput == null) return;

        bool isTouchPad = gvrControllerInput.GetButton(GvrControllerButton.TouchPadTouch);
        bool isTouchPadUp = gvrControllerInput.GetButtonUp(GvrControllerButton.TouchPadTouch);
        if (isTouchPad)
        {
            Vector2 pos = gvrControllerInput.TouchPos;
            Debug.LogFormat("Touch position x={0}, y={1},", pos.x, pos.y);

            //facing = (pos.y > 0.0f) ? 1 : -1;
            if (pos.y > 0.0f)
            {
                facing = 1;
            }
            else
            {
                facing = -1;
            }
            MoveLookAt(facing);
        }
        if (isTouchPadUp)
        {
            facing = 0;
            MoveLookAt(facing);
        }

    }

    void MoveLookAt(int facing)
    {
        Vector3 heading = camTr.forward;
        heading.y = 0.0f;

        if (facing != 0)
        {
            anim.SetBool(hashIsRun, true);

            cc.SimpleMove(heading * speed * facing);
        }
        else
        {
            anim.SetBool(hashIsRun, false);
            //cc.SimpleMove(heading * speed * facing);
        }





        //플레이어가 앞으로가면 앞으로가는 애니메이션 / 뒤로가면 뒤로가는 애니메이션

    }

    void CheckButton()
    {
        if (gvrControllerInput == null) return;

        bool touchPadClick = gvrControllerInput.GetButtonDown(GvrControllerButton.TouchPadButton);
        bool appButtonUp = gvrControllerInput.GetButtonUp(GvrControllerButton.App);

        //점프 버튼을 눌럿을 때 
        if (cc.isGrounded)
        {
            verticalVelocity = -gravity * Time.deltaTime;

            if (touchPadClick)
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

        //앱버튼 ( 사물 캐치 ) 버튼을 눌럿을때 
        if (appButtonUp)
        {
            touchChance_txt.text = "남은 기회" + "\n\n" + "<color=#ff0000>" + (touchChanceCount > 0 ? --touchChanceCount : 0) + "</color>";
            LaserCtrl.instance_LayerCtrl.RayShoot();

            //터치 카운트가 0이 되면 UIManager에 HiderWinGame
            if (touchChanceCount == 0)
            {
                StartCoroutine(UIManager.instance_UI.HiderWinGame());
            }
            //Laserctrl스크립트에 RayShoot함수 실행 
        }
    }
}
