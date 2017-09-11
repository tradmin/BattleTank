using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUIInGameManager : MonoBehaviour {

    public CUIJoystick[] m_ctlJoystick = new CUIJoystick[2];

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClickConnectToServer()
    {
        string strID = "ID_Really_" + Random.Range(0, 9999).ToString("0000");
        string strNickname = "Nick_Really_" + Random.Range(0, 9999).ToString("0000");
        CNetwork.Instance.ConnectServer(strID, strNickname);
    }

    public void OnClickCreateRoom()
    {
        string strRoomName = "Name_Room_" + Random.Range(0, 9999).ToString("0000");
        CNetwork.Instance.CreateRoom(strRoomName, 0, 8, "");
    }

    public void OnClickJoinRoom()
    {
        CNetwork.Instance.QuickJoin(0);
    }
}
