using System;
using UnityEngine;
using UnityEngine.UI;

public sealed class MicrogameSession : MonoBehaviour
{
    private enum Phase { Ready, Playing, Result }

    [SerializeField] private MicrogameBehaviour game;
    [SerializeField] private GameObject readyPanel;
    [SerializeField] private GameObject playArea;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private Text timerText;
    [SerializeField] private Text resultText;
    [SerializeField, Min(1f)] private float durationSeconds = 10f; 

    private Phase currentPhase;
    private float remainingSeconds;

    private void Awake()
    {
        currentPhase = Phase.Ready;
        readyPanel.SetActive(true);
        playArea.SetActive(false);
        resultPanel.SetActive(false);
        timerText.text = string.Empty;
    }

    public void StartGame()
    {
        if (currentPhase != Phase.Ready || game == null) return;

        remainingSeconds = durationSeconds;
        readyPanel.SetActive(false);
        playArea.SetActive(true);
        resultPanel.SetActive(false);
        currentPhase = Phase.Playing;
        ShowTime();

    }

    private void Update()
    {
        if (currentPhase != Phase.Playing) return;

        remainingSeconds = Mathf.Max(0f, remainingSeconds - Time.deltaTime);
        ShowTime();
        if(remainingSeconds <=0f) Finish(false);
    }

    public void Finish(bool won)
    {
        if (currentPhase != Phase.Playing) return;

        currentPhase = Phase.Result;
        game.End();
        playArea.SetActive(false);
        resultPanel.SetActive(true);
        resultText.text = won ? "You win!" : "Time is up!";
    }

    private void ShowTime()
    {
        timerText.text = $"Time : {remainingSeconds:0.0}";
    }
}
