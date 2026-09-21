using TMPro;
using UnityEngine;

namespace MobileGameTemplate
{
    /// <summary>
    /// Keeps panel and text changes in one straightforward place.
    /// </summary>
    public class MicrogameUI : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject menuPanel;
        [SerializeField] private GameObject gamePanel;
        [SerializeField] private GameObject resultPanel;

        [Header("Text")]
        [SerializeField] private TMP_Text countdownText;
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private TMP_Text resultText;

        public void Configure(
            GameObject menu,
            GameObject game,
            GameObject result,
            TMP_Text countdown,
            TMP_Text timer,
            TMP_Text resultLabel)
        {
            menuPanel = menu;
            gamePanel = game;
            resultPanel = result;
            countdownText = countdown;
            timerText = timer;
            resultText = resultLabel;
        }

        public void ShowMenu()
        {
            menuPanel.SetActive(true);
            gamePanel.SetActive(false);
            resultPanel.SetActive(false);
        }

        public void ShowCountdown(int startingNumber)
        {
            menuPanel.SetActive(false);
            gamePanel.SetActive(true);
            resultPanel.SetActive(false);
            countdownText.gameObject.SetActive(true);
            countdownText.text = startingNumber.ToString();
            timerText.text = string.Empty;
        }

        public void SetCountdown(string value)
        {
            countdownText.text = value;
        }

        public void ShowPlaying(float startingTime)
        {
            countdownText.gameObject.SetActive(false);
            SetTimer(startingTime);
        }

        public void SetTimer(float remainingSeconds)
        {
            timerText.text = $"Time: {remainingSeconds:0.0}";
        }

        public void ShowResult(bool wasSuccessful)
        {
            menuPanel.SetActive(false);
            gamePanel.SetActive(false);
            resultPanel.SetActive(true);
            resultText.text = wasSuccessful ? "SUCCESS!" : "TIME'S UP!";
            resultText.color = wasSuccessful
                ? new Color(0.2f, 0.75f, 0.35f)
                : new Color(0.9f, 0.25f, 0.25f);
        }
    }
}
