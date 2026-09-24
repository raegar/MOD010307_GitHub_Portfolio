using MicrogameCourse.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace MicrogameCourse.Microgames
{
    /// <summary>Worked example for students to type and explain themselves.</summary>
    public sealed class TargetTapGame : MicrogameBehaviour
    {
        [SerializeField] private RectTransform playArea;
        [SerializeField] private RectTransform target;
        [SerializeField] private Text progressText;
        [SerializeField, Min(1)] private int tapsToWin = 5;

        private int tapsRemaining;

        public override void Begin(MicrogameSession session)
        {
            base.Begin(session);
            tapsRemaining = tapsToWin;
            UpdateProgress();
            MoveTarget();
        }

        public void TapTarget()
        {
            if (!IsRunning) return;

            tapsRemaining--;
            UpdateProgress();

            if (tapsRemaining == 0)
                Win();
            else
                MoveTarget();
        }

        private void UpdateProgress()
        {
            progressText.text = $"Taps left: {tapsRemaining}";
        }

        private void MoveTarget()
        {
            float maxX = (playArea.rect.width - target.rect.width) * 0.5f;
            float maxY = (playArea.rect.height - target.rect.height) * 0.5f;
            float x = Random.Range(-maxX, maxX);
            float y = Random.Range(-maxY, maxY);
            target.anchoredPosition = new Vector2(x, y);
        }
    }
}
