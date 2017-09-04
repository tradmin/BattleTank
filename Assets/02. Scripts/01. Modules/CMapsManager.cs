using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class CMapsManager : MonoBehaviour {
    public static CMapsManager _instance = null;

    public static CMapsManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.Log("CMapsManager install null");

            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    private int m_nTeamUrbanWidth = 0;
    private int m_nTeamUrbanColumn = 0;

    private TileInfo[] m_tiUrban;

    public void InitMapsManager()
    {
        string strKey = "";
        int nTileIndex = 0;

        strKey = "Scripts/team_urban_00";
        TextAsset _txtFile = (TextAsset)Resources.Load(strKey) as TextAsset;
        string fileFullPath = _txtFile.text;

        string[] strLineList = fileFullPath.Split('\n');
        m_nTeamUrbanColumn = strLineList.Length;

        string[] strContentsList = strLineList[0].Split('\t');
        m_nTeamUrbanWidth = strContentsList.Length;

        Debug.Log("MapLenght : " + (m_nTeamUrbanWidth * m_nTeamUrbanColumn) + ", Widht : " + m_nTeamUrbanWidth + ", Column : " + m_nTeamUrbanColumn);

        m_tiUrban = new TileInfo[m_nTeamUrbanWidth * m_nTeamUrbanColumn];

        for(int i = 0; i < m_nTeamUrbanColumn; i++)
        {
            string[] strTileList = strLineList[i].Split('\t');
            for (int j = 0; j < m_nTeamUrbanWidth; j++)
            {
                string strTileInfo = strTileList[j];
                string strType = strTileInfo.Substring(0, 1);
                string strSubType = strTileInfo.Substring(1, 1);
                string strIndex = strTileInfo.Substring(2, 2);
                string strAngle = strTileInfo.Substring(4, 1);
                string strHeight = strTileInfo.Substring(5, 1);

                m_tiUrban[nTileIndex].nType = Convert.ToInt32(strType);
                m_tiUrban[nTileIndex].nSubIndex = Convert.ToInt32(strSubType);
                m_tiUrban[nTileIndex].nIndex = Convert.ToInt32(strIndex);
                m_tiUrban[nTileIndex].nAngle = Convert.ToInt32(strAngle);
                m_tiUrban[nTileIndex].nHeight = Convert.ToInt32(strHeight);

                Debug.Log(i + ", " + j + " - Length : " + m_tiUrban.Length + ", TileList : " + strTileList[j] + ", TileIndex : " + nTileIndex + ", Index : " + m_tiUrban[nTileIndex].nIndex + ", Angle : " + m_tiUrban[nTileIndex].nAngle + ", Height : " + m_tiUrban[nTileIndex].nHeight);
       
                nTileIndex++;
            }
        }
    }

    public int GetTeamUrbanWidth()
    {
        return m_nTeamUrbanWidth;
    }

    public int GetTeamUrbanColumn()
    {
        return m_nTeamUrbanColumn;
    }

    public TileInfo GetTeamUrbanTile(int nTileIndex)
    {
        return m_tiUrban[nTileIndex];
    }

    public TileInfo GetTeamUrbanTile(int nX, int nY)
    {
        int nTileIndex = (nY * GetTeamUrbanWidth()) + nX;
        return GetTeamUrbanTile(nTileIndex);
    }
}
