using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance_UI;

    private float limitTime; //Hide Over Time
    private float startTime; //처음 숨는 시간 

    public GameObject timerInfo; //타이머 정보 Panel
    public Text text_Timer; //타이머 표시 UI

    public GameObject hinderInfo; //Hinder 정보 Panel
    public Text hinderName; //Hinder 이름 UI

    public GameObject finderInfo; //Finder 정보 Image
    public Text finderInfo_txt; //Finder정보 Text

    private bool isStart = false; //게임시작 상태

    void Awake()
    {
        instance_UI = this;
    }

    // Start is called before the first frame update
    void Start()
    {

        startTime = 30.0f;
        limitTime = 180.0f;
        timerInfo.SetActive(false);
        finderInfo.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        startTime -= Time.deltaTime;
        int startTime_int = (int)Mathf.Round(startTime);

        finderInfo_txt.text = "제한 시간안에 숨겨진 사물을 찾으세요 "
                    + "\n\n [ 제한 시간은 2분 입니다 ] "
                    + "\n\n" + "<color=#ff0000>" + startTime_int + "</color>" + "초 뒤에 시작합니다 ";


        if (!isStart && startTime_int <= 0)
        {
            startTime = 0;
            finderInfo.SetActive(false);
            TimerStart();
        }
    }





    public void ShowInfo(string myName)
    {
        hinderInfo.SetActive(true);

        hinderName.text = "당신은 지금 '" + "<color=#ff0000>" + myName + "</color>" + "' 입니다."
                        + "\n\n 제한시간안에 숨으세요."
                        + "\n\n" + "<color=#ff0000>" + "'30초' 뒤에 시작합니다. " + "</color>";
    }

    public void TimerStart()
    {
        hinderInfo.SetActive(false);
        timerInfo.SetActive(true);

        limitTime -= Time.deltaTime;
        text_Timer.text = "남은 시간 : " + "<color=#ff0000>" + Mathf.Round(limitTime) + "</color>" + "  초";

        //남은 시간 끝날시 게임 종료 !! 로직 구현
        if (Mathf.Round(limitTime) == 0)
        {
            StartCoroutine(HiderWinGame());
        }
    }

    //시간 초과로 인한 Hider 승리 
    public IEnumerator HiderWinGame(){

        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("HiderWinGame");
    }
}
