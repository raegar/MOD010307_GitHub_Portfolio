using System.Collections;
using UnityEngine;

namespace MobileGameTemplate
{
    /// <summary>
    /// Owns the shared Menu -> Countdown -> Playing -> Result flow.
    /// </summary>
    public class MicrogameManager : MonoBehaviour
    {
        [SerializeField] private MicrogameUI ui;
        [SerializeField] private MicrogameBase activeMicrogame;
        [SerializeField, Min(1)] private int countdownSeconds = 3;
        [SerializeField, Min(1f)] private float gameDurationSeconds = 5f;

        private Coroutine gameFlow;

        public MicrogameState CurrentState { get; private set; } = MicrogameState.Menu;

        public void Configure(MicrogameUI microgameUI, MicrogameBase microgame)
        {
            ui = microgameUI;
            activeMicrogame = microgame;
        }

        private void Awake()
        {
            if (activeMicrogame != null)
            {
                activeMicrogame.Initialise(this);
            }

            ShowMenuState();
        }

        public void StartGame()
        {
            StopCurrentFlow();
            activeMicrogame.ResetGame();
            gameFlow = StartCoroutine(RunGameFlow());
        }

        public void Replay()
        {
            StartGame();
        }

        public void ReturnToMenu()
        {
            StopCurrentFlow();
            activeMicrogame.ResetGame();
            ShowMenuState();
        }

        public void ReportResult(MicrogameBase reportingGame, bool wasSuccessful)
        {
            if (CurrentState != MicrogameState.Playing || reportingGame != activeMicrogame)
            {
                return;
            }

            StopCurrentFlow();
            activeMicrogame.StopGame();
            CurrentState = wasSuccessful ? MicrogameState.Success : MicrogameState.Failure;
            ui.ShowResult(wasSuccessful);
        }

        private IEnumerator RunGameFlow()
        {
            CurrentState = MicrogameState.Countdown;
            ui.ShowCountdown(countdownSeconds);

            for (int number = countdownSeconds; number > 0; number--)
            {
                ui.SetCountdown(number.ToString());
                yield return new WaitForSecondsRealtime(1f);
            }

            ui.SetCountdown("GO!");
            yield return new WaitForSecondsRealtime(0.5f);

            CurrentState = MicrogameState.Playing;
            activeMicrogame.BeginGame();

            float remainingTime = gameDurationSeconds;
            ui.ShowPlaying(remainingTime);

            while (remainingTime > 0f && CurrentState == MicrogameState.Playing)
            {
                remainingTime -= Time.unscaledDeltaTime;
                ui.SetTimer(Mathf.Max(0f, remainingTime));
                yield return null;
            }

            if (CurrentState == MicrogameState.Playing)
            {
                ReportResult(activeMicrogame, false);
            }
        }

        private void ShowMenuState()
        {
            CurrentState = MicrogameState.Menu;
            ui.ShowMenu();
        }

        private void StopCurrentFlow()
        {
            if (gameFlow != null)
            {
                StopCoroutine(gameFlow);
                gameFlow = null;
            }
        }
    }
}
