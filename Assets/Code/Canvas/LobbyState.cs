using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public interface IEventLobbyHandler : IEventSystemHandler
{
    void OnLobby(int score);
    void OnGarage();
}

public class LobbyState : MonoBehaviour, IEventLobbyHandler
{
    public SocketIOTest m_SocketIOTest;
    public Text m_StateName;

    int m_Score = 0;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnLobby(int score)
    {
        m_Score = score;
        m_StateName.text = "Lobby" + " : " + m_Score.ToString();
    }

    public void OnGarage()
    {
        m_StateName.text = "Garage" + " : " + m_Score.ToString();
    }
}
