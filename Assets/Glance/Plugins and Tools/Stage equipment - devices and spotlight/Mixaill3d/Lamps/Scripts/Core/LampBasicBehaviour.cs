using System;
using UnityEngine;

namespace Mixaill3d.Lamps.Scripts.Core
{
    public class LampBasicBehaviour : ScriptableObject
    {
        private static MaterialPropertyBlock materialPropertyBlock;
        private static string EMISSION_COLOR = "_EmissionColor";
        private static string COLOR = "_Color";

        public virtual void ProcessBehaviour(Renderer[] renderers, Renderer[] additionalRenderes, LampInfo[] lampInfos, float timeOffset, float speed)
        {
            for(int i = 0; i < lampInfos.Length; i++)
            {
                LampInfo lamp = lampInfos[i];
                ProcessSingleLampBehaviour(renderers[i], additionalRenderes[i], lamp, timeOffset, speed);
            }
        }

        protected void SetColor(Renderer _renderer, Renderer additionalRenderer, LampInfo lampInfo, Color color)
        {
            SetMaterialColor(color, _renderer, EMISSION_COLOR, true);
            SetMaterialColor(color, additionalRenderer, COLOR, false);
            var light = lampInfo.Light;
            if (light != null)
            {
                light.color = color;
            }
        }

        private static void SetMaterialColor(Color color, Renderer renderer, string colorName, bool changeAlpha)
        {
            if (renderer == null)
                return;

            materialPropertyBlock = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(materialPropertyBlock);

            var colorToApply = color;
            if (!changeAlpha)
            {
                var materialColor = materialPropertyBlock.GetColor(colorName);
                colorToApply.a = materialColor.a;
            }

            if (colorName == EMISSION_COLOR)
            {
                colorToApply *= Mathf.Pow(2.0F, 1.2f - 0.4169F);
            }

            materialPropertyBlock.SetColor(colorName, colorToApply);
            renderer.SetPropertyBlock(materialPropertyBlock);
        }

        protected virtual void ProcessSingleLampBehaviour(Renderer _renderer, Renderer additionalRenderer, LampInfo lampInfo, float timeOffset, float speed)
        {
            throw new Exception("It is basic behaviour. There is no realization.");
        }
        
        protected float GetCurrentTime(float timeOffset, float speed)
        {
            return Mathf.Repeat(Time.time * speed + timeOffset, 1f);
        }
    }
}
