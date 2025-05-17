using System;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class DJBooth : Interactable
    {
        public static event Action OnStartedPlayingDJBoothGame;

        public override void Interact()
        {
            base.Interact();

            OnStartedPlayingDJBoothGame?.Invoke();
        }
    }
}
