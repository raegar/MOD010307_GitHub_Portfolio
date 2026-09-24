using UnityEngine;

namespace MicrogameCourse.Framework
{
    /// <summary>The small contract that every game script uses.</summary>
    public abstract class MicrogameBehaviour : MonoBehaviour
    {
        protected MicrogameSession Session { get; private set; }
        protected bool IsRunning { get; private set; }

        public virtual void Begin(MicrogameSession session)
        {
            Session = session;
            IsRunning = true;
        }

        public virtual void End()
        {
            IsRunning = false;
        }

        protected void Win()
        {
            if (IsRunning) Session.Finish(true);
        }

        protected void Lose()
        {
            if (IsRunning) Session.Finish(false);
        }
    }
}
