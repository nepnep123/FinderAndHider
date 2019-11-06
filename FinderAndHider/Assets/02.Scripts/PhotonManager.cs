using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class PhotonManager : Photon.PunBehaviour
{
    public static PhotonManager Instance_PhotonMgr;

    void Start()
    {
        Instance_PhotonMgr = this;

        if (!PhotonNetwork.connected)
        {
            SceneManager.LoadScene("Launcher");

            return;
        }
    }
    void LoadArena()
    {
        if (!PhotonNetwork.isMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        }

        Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.room.PlayerCount);
        
        //플레이어가 2명일 때만 "MainPlay"씬으로 이동
        if(PhotonNetwork.room.PlayerCount == 2){
            PhotonNetwork.LoadLevel("MainPlay");
            
        }
        
    }

    // public override void OnJoinedRoom(){
    //     Debug.Log("PhotonManager OnJoinedRoom");
    // }

    public override void OnPhotonPlayerConnected(PhotonPlayer other)
    {
        
        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("OnPhotonPlayerConnected isMasterClient " + PhotonNetwork.isMasterClient); // called before OnPhotonPlayerDisconnected

            LoadArena();
        }
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer other)
    {
    
        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("OnPhotonPlayerConnected isMasterClient " + PhotonNetwork.isMasterClient); // called before OnPhotonPlayerDisconnected

            LoadArena();
        }
    }


}
