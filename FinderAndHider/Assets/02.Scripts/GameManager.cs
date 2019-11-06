using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance_GM; //게임 매니저 스크립트

    //public GameObject[] hinders; //Hider 프리팹
    public Transform spawnPoint_Hider; //Hider 스폰 포인트 
    public Transform spawnPoint_Finder; //Finder 스폰 포인트

    public bool isGameOver = false;

    

    void Awake()
    {
        instance_GM = this;
    }

    void Start()
    {

        Debug.Log("GameManager Start !!!");
        //Seeker인 경우 !!! 조건이 False이고 컨트롤 대상이 없을 경우 프리팹 생성
        if ((bool)PhotonNetwork.room.CustomProperties["isFinderSelect_key"] == false &&
                MoveCtrl.LocalPlayerInstance == null)
        {
            Debug.Log("isFinderSelect_key : false ########################");
            if(PhotonNetwork.isMasterClient){
                SpawnHinder();

            } else{
                SpawnFinder();
            }
            
        }
        //Finder인 경우 !!!
        if ((bool)PhotonNetwork.room.CustomProperties["isFinderSelect_key"] == true &&
                MoveCtrl.LocalPlayerInstance == null)
        {
            Debug.Log("isFinderSelect_key : true#########################");
            if(PhotonNetwork.isMasterClient){
                SpawnFinder();

            } else{
                SpawnHinder();
            }
            
        }

    }

    //Finder 생성 함수
    private void SpawnFinder()
    {
        String finder = "Finder";
        if(!isGameOver){
            GameObject finderPrefab = PhotonNetwork.Instantiate(finder, spawnPoint_Finder.position, spawnPoint_Finder.rotation, 0);
        }
    }

    //Seeker 생성 함수
    public void SpawnHinder()
    {
        if (!isGameOver)
        {

            var myName = ""; //숨는 사람 이름
            String[] slist = { "Item_ATM", "Item_Computer", "Item_Couch", "Item_Plant", "Item_Statue" };
            int hinderidx = Random.Range(0, slist.Length - 1);

            GameObject hinderPrefab = PhotonNetwork.Instantiate(slist[hinderidx], spawnPoint_Hider.position, spawnPoint_Hider.rotation, 0);

            myName = hinderPrefab.name;
            switch (myName)
            {
                case "Item_Couch(Clone)":
                    myName = "의자";
                    break;
                case "Item_ATM(Clone)":
                    myName = "ATM";
                    break;
                case "Item_Computer(Clone)":
                    myName = "컴퓨터";
                    break;
                case "Item_Plant(Clone)":
                    myName = "화분";
                    break;
                case "Item_Statue(Clone)":
                    myName = "조각상";
                    break;
                default:
                    myName = "지정된 이름이 아닙니다.";
                    break;
            }
            UIManager.instance_UI.ShowInfo(myName);
            
        }
    }

    public IEnumerator FinderWinGame(){
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("FinderWinGame");
    }

}
