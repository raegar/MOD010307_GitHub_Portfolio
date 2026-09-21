using UnityEngine;
using UnityEngine.UI;

namespace MobileGameTemplate
{
    /// <summary>
    /// A deliberately trivial test game: press WIN before the timer expires.
    /// Replace this with a student microgame after the framework is understood.
    /// </summary>
    public class PlaceholderMicrogame : MicrogameBase
    {
        [SerializeField] private Button winButton;

        public void Configure(Button button)
        {
            winButton = button;
        }

        public void Win()
        {
            CompleteGame(true);
        }
    }
}
