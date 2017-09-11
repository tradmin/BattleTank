using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CNetwork : MonoBehaviour {
    public static CNetwork _instance = null;

    public static CNetwork Instance
    {
        get
        {
            if (_instance == null)
                Debug.Log("CNetwork install null");

            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    public PhotonView PV = null;

    private static ExitGames.Client.Photon.Hashtable m_htPlayer;

	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 초반에 세팅
    /// </summary>
    public void InitNetwork()
    {
        PV = PhotonView.Get(this);

        m_htPlayer = new ExitGames.Client.Photon.Hashtable();
    }

    public void ConnectServer(string strUserID, string strNickname)
    {
        if (PV == null)
            return;

        if ( PhotonNetwork.connected )
            return;

        PhotonNetwork.playerName = strNickname;

        if (PhotonNetwork.AuthValues == null)
        {
            PhotonNetwork.AuthValues = new AuthenticationValues();
        }

        PhotonNetwork.AuthValues.UserId = strUserID;

        m_htPlayer.Clear();

        // TODO : 유저 커스텀 값
        // m_htPlayer.Add("", "")

        PhotonNetwork.ConnectUsingSettings("1.0");
    }

    /// <summary>
    /// 방 생성
    /// </summary>
    /// <param name="strRoomName"></param>
    /// <param name="nType"></param>
    /// <param name="nMaxPlayer"></param>
    /// <param name="strPassword"></param>
    public void CreateRoom(string strRoomName, int nType, int nMaxPlayer, string strPassword = "")
    {
        RoomOptions option = new RoomOptions();
        option.IsOpen = true;
        option.IsVisible = true;
        option.CleanupCacheOnLeave = true;
        option.MaxPlayers = (byte)nMaxPlayer;
        option.PublishUserId = true;

        option.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable()
        {
            { "RoomType", nType },
            { "PW", strPassword }
        };

        option.CustomRoomPropertiesForLobby = new string[] { "RoomType", "PW" };

        PhotonNetwork.CreateRoom(strRoomName, option, null);
    }

    public void QuickJoin(int nType)
    {
        ExitGames.Client.Photon.Hashtable htRoomType = new ExitGames.Client.Photon.Hashtable() { { "RoomType", nType } };
        PhotonNetwork.JoinRandomRoom(htRoomType, 0);
    }

    public void JoinRoom(string strRoomName, string strPW = "")
    {
        ExitGames.Client.Photon.Hashtable ht;
        foreach (RoomInfo info in PhotonNetwork.GetRoomList())
        {
            if (info.Name.Equals(strRoomName))
            {
                ht = info.CustomProperties;
                object objRoomPW;
                ht.TryGetValue("PW", out objRoomPW);

                if (objRoomPW.ToString().Equals(strPW))
                {
                    PhotonNetwork.JoinRoom(strRoomName);
                    return;
                }
                else
                {
                    // TODO : Display the message 'Password is incorrect'
                }
            }
        }

        // TODO : Display the message 'There is no matching room' 일치하는 방이 존재하지 않습니다.
    }

    // Photon call functions ------------------------
    void OnConnectedToPhoton()
    {
        Debug.Log("OnConnectToPhoton");

        PhotonNetwork.player.SetCustomProperties(m_htPlayer);
    }

    void OnLeftRoom()
    {
    }

    void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    {
        //	UpdateGenderCnt ();
    }

    void OnPhotonCreateRoomFailed(object[] codeAndMsg)
    {
    }

    void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
    }

    void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom Name : " + PhotonNetwork.room.Name);
    }

    void OnJoinedLobby()
    {
    }

    void OnLeftLobby()
    {
    }

    void OnDisconnectedFromPhoton()
    {
    }

    void OnConnectionFail(DisconnectCause cause)
    {
    }

    void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
    }

    void OnReceivedRoomListUpdate()
    {
    /*
        foreach (RoomInfo info in PhotonNetwork.GetRoomList())
        {
            string strRoomIndex = GetRoomIndex(info);
            string strRoomName = GetRoomName(info);
            string strRoomType = GetRoomType(info);
            string strRoomPW = GetRoomPW(info);
            int nMaleCnt = GetRoomMaleCount(info);
            int nFemaleCnt = GetRoomFemaleCount(info);

            Debug.Log("Index : " + strRoomIndex + ", Name : " + strRoomName + ", Type : " + strRoomType + ", PW : " + strRoomPW + ", Male : " + nMaleCnt + ", Female : " + nFemaleCnt);
        }
    */
    }

    void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom Name : " + PhotonNetwork.room.Name);
        CGameManager.Instance.CreateTank();
        //   Debug.Log("OnJoinedRoom Room Index : " + GetMyRoomIndex() + ", Name : " + GetMyRoomName());
        // Dummy Client
        // GameObject.Find ("InGame").GetComponent<CUIInGameManager> ().UpdateRoomName (PhotonNetwork.room.Name);
    }

    void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        //UpdateGenderCnt ();

        // TODO : 방에 새로 들어 온 유저 정보 업데이트
        //also player properties are not cleared when disconnecting and connecting
        //automatically, so we have to set all existing properties to null
        //these default values will get overriden by correct data soon
        newPlayer.Clear();
    }

    void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        //UpdateGenderCnt ();
    }

    void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
    }

    void OnConnectedToMaster()
    {
    }
    /*
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
    */
    void OnPhotonInstantiate(PhotonMessageInfo info)
    {
    }

    void OnPhotonMaxCccuReached()
    {
        // TODO : Display message
    }

    void OnPhotonCustomRoomPropertiesChanged(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
    //    Debug.Log("OnPhotonCustomRoomPropertiesChanged State : " + GetMyRoomState() + ", Music : " + GetMyRoomMusic() + ", StartTime : " + GetMyRoomStartTime());
    }

    void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
    {
    //    PhotonPlayer player = playerAndUpdatedProps[0] as PhotonPlayer;

    //    Debug.Log("OnPhotonPlayerPropertiesChanged Friendly : " + GetUserFriendly(player));
    }
}
