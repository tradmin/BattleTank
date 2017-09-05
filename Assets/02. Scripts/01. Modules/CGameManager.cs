using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGameManager : MonoBehaviour {
    public static CGameManager _instance = null;

    public static CGameManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.Log("CGameManager install null");

            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    public CUIInGameManager m_cUIInGameManager;

    // Use this for initialization
    void Start () {
        CObjectsManager.Instance.InitObjectsManager();

        CMapsManager.Instance.InitMapsManager();

        CNetwork.Instance.InitNetwork();


        for (int i = 0; i < CMapsManager.Instance.GetTeamUrbanColumn(); i++)
        {
            for (int j = 0; j < CMapsManager.Instance.GetTeamUrbanWidth(); j++)
            {
                TileInfo tiData = CMapsManager.Instance.GetTeamUrbanTile(j, i);
                //Instantiate(CObjectsManager.Instance.GetTile(tiData.nType, tiData.nSubIndex, tiData.nIndex), new Vector3(j * 10, 0, i * 10), Quaternion.identity);
                Instantiate(CObjectsManager.Instance.GetTile(tiData.nType, tiData.nSubIndex, tiData.nIndex), new Vector3(j * 10, tiData.nHeight, -(i * 10)), Quaternion.Euler(0, tiData.nAngle * 90, 0));
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CreateTank()
    {
        //Instantiate (CObjectsManager.Instance.GetTank(1), new Vector3(0, 0, 0), Quaternion.identity);
        PhotonNetwork.Instantiate("Tanks/0001", new Vector3(0, 0, 0), Quaternion.identity, 0);
    }
}
