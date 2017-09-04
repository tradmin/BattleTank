using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CObjectsManager : MonoBehaviour {
    public static CObjectsManager _instance = null;

    public static CObjectsManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.Log("CObjectsManager install null");

            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    //--------------------------------------------------------------------

    private GameObject[] m_goUrbanRoad = new GameObject[4];
    private GameObject[] m_goUrbanTile = new GameObject[19];

    private GameObject[] m_goTanks = new GameObject[2];

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InitObjectsManager()
    {
        string strFilePath = "";
        for (int i = 0; i < m_goUrbanRoad.Length; i++)
        {
            strFilePath = "MapTiles/" + i.ToString("0000");
            m_goUrbanRoad[i] = (GameObject)Resources.Load(strFilePath);
        }

        for(int i = 0; i < m_goUrbanTile.Length; i++)
        {
            int nIndex = 100 + i;
            strFilePath = "MapTiles/" + nIndex.ToString("0000");
            m_goUrbanTile[i] = (GameObject)Resources.Load(strFilePath);
        }

        for(int i = 0; i < m_goTanks.Length; i++)
        {
            strFilePath = "Tanks/" + i.ToString("0000");
            m_goTanks[i] = (GameObject)Resources.Load(strFilePath);
        }
    }

    /// <summary>
    /// 타일값을 얻습니다.
    /// </summary>
    /// <param name="nType">맵 스타일</param>
    /// <param name="nSubType">거리 타입</param>
    /// <param name="nIndex">타일 인덱스</param>
    /// <returns></returns>
    public GameObject GetTile(int nType, int nSubType, int nIndex)
    {
        GameObject goTile = null;

        switch(nType)
        {
            case 0:
                if (nSubType == 0)
                    goTile = GetUrbanRoad(nIndex);
                else if (nSubType == 1)
                    goTile = GetUrbanTitle(nIndex);
                break;
        }

        return goTile;
    }

    public GameObject GetUrbanRoad(int nIndex)
    {
        if (nIndex > m_goUrbanRoad.Length)
            return m_goUrbanRoad[0];

        return m_goUrbanRoad[nIndex];
    }

    public GameObject GetUrbanTitle(int nIndex)
    {
        if (nIndex > m_goUrbanTile.Length)
            return m_goUrbanTile[0];

        return m_goUrbanTile[nIndex];
    }

    public GameObject GetTank(int nIndex)
    {
        if (nIndex > m_goTanks.Length)
            return m_goTanks[0];

        return m_goTanks[nIndex];
    }

}
