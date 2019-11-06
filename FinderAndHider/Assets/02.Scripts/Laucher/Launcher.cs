using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Launcher : Photon.PunBehaviour
{
    public static Launcher instance_Launcher;
    bool isConnecting; //연결되었을때 
    public bool isFinderSelect = true; // Finder를 선택했으면 True / Seeker를 선택 했으면 False


    string _gameVersion = "1";

    public PhotonLogLevel Loglevel = PhotonLogLevel.Informational;

    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
    public byte MaxPlayersPerRoom = 4;

    [SerializeField]
    private GameObject info_Panel;

    [SerializeField]
    private GameObject connect_Img;

    bool isSelectedObject;
    void Awake()
    {
        instance_Launcher = this;

        PhotonNetwork.logLevel = Loglevel;

        PhotonNetwork.autoJoinLobby = false;

        PhotonNetwork.automaticallySyncScene = true;
    }


    void Start()
    {

        info_Panel.SetActive(true);
        connect_Img.SetActive(false);
    }

    public void Connect(bool _isFinderSelect)
    {
        info_Panel.SetActive(false);
        connect_Img.SetActive(true);

        isConnecting = true;
        isFinderSelect = _isFinderSelect;

        if (PhotonNetwork.connected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings(_gameVersion);
        }
    }

    //마스터 서버 접속 
    public override void OnConnectedToMaster()
    {
        Debug.Log("DemoAnimator/Launcher: OnConnectedToMaster() was called by PUN");
        if (isConnecting)
        {
            PhotonNetwork.JoinRandomRoom();
        }

    }

    public override void OnDisconnectedFromPhoton()
    {
        info_Panel.SetActive(true);
        connect_Img.SetActive(false);

        Debug.LogWarning("DemoAnimator/Launcher: OnDisconnectedFromPhoton() was called by PUN");
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        Debug.Log("DemoAnimator/Launcher:OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");

        //조인 룸에 실패 하면 유저는 새로 룸을 만든다. 
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = MaxPlayersPerRoom }, null);
    }

    public override void OnJoinedRoom()
    {

        Hashtable hash = new Hashtable();
        hash.Add("isFinderSelect_key", isFinderSelect);
        PhotonNetwork.room.SetCustomProperties(hash);

        if (PhotonNetwork.room.PlayerCount == 1)
        {

            Debug.Log("We load the 'Waiting Room' ");

            PhotonNetwork.LoadLevel("Waiting Room");
        }
    }
}
