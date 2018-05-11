using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

using System;
using BestHTTP;
using BestHTTP.SocketIO;
using BestHTTP.SocketIO.Events;

using StateMachine;

public class SocketIOTest : MonoBehaviour
{
    public string m_ConnectUrl;

    public GameObject m_OffStateScreen;
    public GameObject m_LobbyStateScreen;
    public GameObject m_RoomStateScreen;
    public GameObject m_GameStateScreen;
    public GameObject m_ResultStateScreen;

    public enum ScenarioStates
    {
        OffState,
        LoginState,
        LobbyState,
        GarageState,
        RoomState,
        GameState,
        ResultState,
    }

    private StateMachine<ScenarioStates> m_fsm;

    string m_szUserId;
    string m_szNickName;
    int m_iScore;

    void Awake()
    {
        m_fsm = StateMachine<ScenarioStates>.Initialize(this);
    }

    // Use this for initialization
    void Start () {
        SocketIOManager.GetManager().Create(m_ConnectUrl + "/socket.io/");
        SocketIOManager.GetManager().On(SocketIOEventTypes.Connect, OnConnect);

        SocketIOManager.GetManager().On("login", OnLogin);
        SocketIOManager.GetManager().On("lobby", OnLobby);
        SocketIOManager.GetManager().On("garage", OnGarage);
        SocketIOManager.GetManager().On("room", OnRoom);
        SocketIOManager.GetManager().On("gamestart", OnGameStart);
        SocketIOManager.GetManager().On("gameend", OnGameEnd);

        m_fsm.ChangeState(ScenarioStates.OffState);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Off
    /// </summary>
    void OffState_Enter()
    {
        m_OffStateScreen.SetActive(true);
        m_LobbyStateScreen.SetActive(false);
        m_RoomStateScreen.SetActive(false);
        m_GameStateScreen.SetActive(false);
        m_ResultStateScreen.SetActive(false);
    }

    public void OffState_ButtonLogin(string szID, string szNickname)
    {
        m_szUserId = szID;
        m_szNickName = szNickname;

        SocketIOManager.GetManager().Connect();
    }

    void OnConnect(Socket socket, Packet packet, params object[] args)
    {
        Debug.Log("OnConnect");

        Dictionary<string, object> loginData = new Dictionary<string, object>();
        loginData.Add("userid", m_szUserId);
        loginData.Add("name", m_szNickName);
        SocketIOManager.GetManager().Emit("login", loginData);
    }

    void OnLogin(Socket socket, Packet packet, params object[] args)
    {
        Dictionary<string, object> data = args[0] as Dictionary<string, object>;

        Debug.Log("OnLogin : ");
        Debug.Log("score : " + data["score"].ToString());
        Debug.Log("result : " +  data["result"].ToString());

        m_iScore = (int)(double)data["score"];
        int iResult = (int)(double)data["result"];

        if (iResult == 1)
        {
            // send lobby
            SocketIOManager.GetManager().Emit("lobby");
        } else
        {
            Debug.Log("error");
        }
    }

    void OnUserCreate(Socket socket, Packet packet, params object[] args)
    {
        Dictionary<string, object> data = args[0] as Dictionary<string, object>;

        Debug.Log("OnUserCreate : ");
    }

    void OnUserNickSet(Socket socket, Packet packet, params object[] args)
    {
        Dictionary<string, object> data = args[0] as Dictionary<string, object>;

        Debug.Log("OnUserNickSet : ");
    }

    void OnRoomMatching(Socket socket, Packet packet, params object[] args)
    {
        Dictionary<string, object> data = args[0] as Dictionary<string, object>;

        Debug.Log("OnRoomMatching : ");
    }

    void OnRoomJoin(Socket socket, Packet packet, params object[] args)
    {
        Dictionary<string, object> data = args[0] as Dictionary<string, object>;

        Debug.Log("OnRoomJoin : ");
    }

    void OnGameReady(Socket socket, Packet packet, params object[] args)
    {
        Dictionary<string, object> data = args[0] as Dictionary<string, object>;

        Debug.Log("OnGameReady : ");
    }

    /// <summary>
    /// Lobby
    /// </summary>
    /// 

    public void LobbyState_ButtonLobby()
    {
        SocketIOManager.GetManager().Emit("lobby");
    }

    public void LobbyState_ButtonGarage()
    {
        SocketIOManager.GetManager().Emit("garage");
    }

    public void LobbyState_ButtonRoom()
    {
        SocketIOManager.GetManager().Emit("room");
    }

    void OnLobby(Socket socket, Packet packet, params object[] args)
    {
        Dictionary<string, object> data = args[0] as Dictionary<string, object>;

        Debug.Log("OnLobby : " + data["result"].ToString());

        int iResult = (int)(double)data["result"];

        if (iResult == 1)
        {
            // send lobby
            m_fsm.ChangeState(ScenarioStates.LobbyState);
        }
        else
        {
            Debug.Log("error");
        }

        m_fsm.ChangeState(ScenarioStates.LobbyState);
    }

    void LobbyState_Enter()
    {
        m_OffStateScreen.SetActive(false);
        m_LobbyStateScreen.SetActive(true);
        m_RoomStateScreen.SetActive(false);
        m_GameStateScreen.SetActive(false);
        m_ResultStateScreen.SetActive(false);

        ExecuteEvents.Execute<IEventLobbyHandler>(m_LobbyStateScreen, null, 
            (reciever, eventData) => reciever.OnLobby(m_iScore));
    }

    /// <summary>
    /// Lobby
    /// </summary>
    /// 

    void OnGarage(Socket socket, Packet packet, params object[] args)
    {
        Dictionary<string, object> data = args[0] as Dictionary<string, object>;

        Debug.Log("OnGarage : " + data["result"].ToString());

        int iResult = (int)(double)data["result"];

        if (iResult == 1)
        {
            // send lobby
            m_fsm.ChangeState(ScenarioStates.GarageState);
        }
        else
        {
            Debug.Log("error");
        }

        m_fsm.ChangeState(ScenarioStates.GarageState);
    }

    void GarageState_Enter()
    {
        m_OffStateScreen.SetActive(false);
        m_LobbyStateScreen.SetActive(true);
        m_RoomStateScreen.SetActive(false);
        m_GameStateScreen.SetActive(false);
        m_ResultStateScreen.SetActive(false);

        ExecuteEvents.Execute<IEventLobbyHandler>(m_LobbyStateScreen, null,
            (reciever, eventData) => reciever.OnGarage());
    }

    /// <summary>
    /// Lobby
    /// </summary>
    /// 

    public void RoomState_ButtonGameStart()
    {
        SocketIOManager.GetManager().Emit("gamestart");
    }

    public void RoomState_ButtonBack()
    {
        SocketIOManager.GetManager().Emit("lobby");
    }


    void OnRoom(Socket socket, Packet packet, params object[] args)
    {
        Dictionary<string, object> data = args[0] as Dictionary<string, object>;

        Debug.Log("OnRoom : " + data["result"].ToString());

        int iResult = (int)(double)data["result"];

        if (iResult == 1)
        {
            // send lobby
            m_fsm.ChangeState(ScenarioStates.RoomState);
        }
        else
        {
            Debug.Log("error");
        }

        m_fsm.ChangeState(ScenarioStates.RoomState);
    }

    void RoomState_Enter()
    {
        m_OffStateScreen.SetActive(false);
        m_LobbyStateScreen.SetActive(false);
        m_RoomStateScreen.SetActive(true);
        m_GameStateScreen.SetActive(false);
        m_ResultStateScreen.SetActive(false);
    }

    /// <summary>
    /// Lobby
    /// </summary>
    /// 

    public void GameState_ButtonGameEnd()
    {
        SocketIOManager.GetManager().Emit("gameend");
    }

    public void GameState_ButtonBack()
    {
        SocketIOManager.GetManager().Emit("lobby");
    }

    void OnGameStart(Socket socket, Packet packet, params object[] args)
    {
        Dictionary<string, object> data = args[0] as Dictionary<string, object>;

        Debug.Log("OnGameStart : " + data["result"].ToString());

        int iResult = (int)(double)data["result"];

        if (iResult == 1)
        {
            // send lobby
            m_fsm.ChangeState(ScenarioStates.GameState);
        }
        else
        {
            Debug.Log("error");
        }

        m_fsm.ChangeState(ScenarioStates.GameState);
    }

    void GameState_Enter()
    {
        m_OffStateScreen.SetActive(false);
        m_LobbyStateScreen.SetActive(false);
        m_RoomStateScreen.SetActive(false);
        m_GameStateScreen.SetActive(true);
        m_ResultStateScreen.SetActive(false);
    }

    /// <summary>
    /// Lobby
    /// </summary>
    /// 

    public void ResultState_Button()
    {
        SocketIOManager.GetManager().Emit("lobby");
    }

    void OnGameEnd(Socket socket, Packet packet, params object[] args)
    {
        Dictionary<string, object> data = args[0] as Dictionary<string, object>;

        Debug.Log("OnGameEnd : " + data["score"].ToString() + data["result"].ToString());

        m_iScore = (int)(double)data["score"];
        int iResult = (int)(double)data["result"];

        if (iResult == 1)
        {
            // send lobby
            m_fsm.ChangeState(ScenarioStates.ResultState);
        }
        else
        {
            Debug.Log("error");
        }
    }

    void ResultState_Enter()
    {
        m_OffStateScreen.SetActive(false);
        m_LobbyStateScreen.SetActive(false);
        m_RoomStateScreen.SetActive(false);
        m_GameStateScreen.SetActive(false);
        m_ResultStateScreen.SetActive(true);

        ExecuteEvents.Execute<IEventResultHandler>(m_ResultStateScreen, null, 
            (reciever, eventData) => reciever.OnResult(m_iScore));
    }

}
