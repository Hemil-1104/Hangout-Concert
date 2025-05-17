using Mixaill3d.Lamps.Scripts.Core;
using UnityEngine;

namespace Mixaill3d.Lamps.Scripts.Behaviours
{
    [CreateAssetMenu(fileName = "EveryColorChangeOverTime", menuName = "Mixaill3d/LampsBehaviour/EveryColorChangeOverTime")]
    public class EveryColorChangeOverTime : LampBasicBehaviour
    {
        [SerializeField] protected Gradient[] _gradients;

        protected override void ProcessSingleLampBehaviour(Renderer _renderer, Renderer additionalRenderer, LampInfo lampInfo, float timeOffset, float speed)
        {
            var currentTime = GetCurrentTime(timeOffset, speed);
            var transform = lampInfo.Renderer.transform;
            var siblingIndex = transform.GetSiblingIndex();
            var color = _gradients[siblingIndex].Evaluate(currentTime);

            SetColor(_renderer, additionalRenderer, lampInfo, color);
        }
        
    }
}