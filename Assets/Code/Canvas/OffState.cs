using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OffState : MonoBehaviour {

    public SocketIOTest m_SocketIOTest;
    public Button m_SumitButton;
    public Text m_UserId;
    public Text m_NickName;

	// Use this for initialization
	void Start ()
    {
        m_SumitButton.onClick.AddListener(() => {
            m_SocketIOTest.OffState_ButtonLogin(m_UserId.text, m_NickName.text); });
    }
}
