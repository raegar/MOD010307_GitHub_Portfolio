using UnityEngine;

public abstract class MicrogameBehaviour
{
    //Microgame Session
    protected bool IsRunning { get; private set; }
    protected MicrogameSession Session {get; private set; }

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
        if(IsRunning) Session.Finish(true);
    }

    protected void Lose()
    {
        if(IsRunning) Session.Finish(false);
    }



}
