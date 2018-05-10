using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public interface IEventResultHandler : IEventSystemHandler
{
    void OnResult(int score);
}

public class ResultState : MonoBehaviour, IEventResultHandler
{
    public Text m_StateName;

    int m_Score = 0;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnResult(int score)
    {
        m_Score = score;
        m_StateName.text = "GameEnd" + " : " + m_Score.ToString();
    }
}
