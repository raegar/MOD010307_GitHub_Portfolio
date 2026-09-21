using UnityEngine;

namespace MobileGameTemplate
{
    /// <summary>
    /// Base class for games that plug into the shared framework.
    /// Student microgames can inherit from this and call CompleteGame(true/false).
    /// </summary>
    public abstract class MicrogameBase : MonoBehaviour
    {
        protected MicrogameManager Manager { get; private set; }

        public bool IsRunning { get; private set; }

        public virtual void Initialise(MicrogameManager manager)
        {
            Manager = manager;
        }

        public virtual void BeginGame()
        {
            IsRunning = true;
        }

        public virtual void StopGame()
        {
            IsRunning = false;
        }

        public virtual void ResetGame()
        {
            IsRunning = false;
        }

        protected void CompleteGame(bool wasSuccessful)
        {
            if (!IsRunning || Manager == null)
            {
                return;
            }

            IsRunning = false;
            Manager.ReportResult(this, wasSuccessful);
        }
    }
}
