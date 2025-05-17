using System;
using System.Collections;
using UnityEngine;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class XPEarningMethod : MonoBehaviour
    {
        public static event Action<int> OnEarnedXp;

        protected int xpReward;
        protected float xpEarningCooldown;
        protected bool canEarnXp = true;

        private WaitForSeconds waitForSeconds;

        protected virtual void Start()
        {
            waitForSeconds = new WaitForSeconds(xpEarningCooldown);
        }

        protected IEnumerator StartCooldown()
        {
            canEarnXp = false;

            yield return waitForSeconds;

            canEarnXp = true;
        }

        protected void EarnXp(int xpReward)
        {
            OnEarnedXp?.Invoke(xpReward);
        }
    }
}
