using System;
using UnityEngine;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class FadingObject : MonoBehaviour, IEquatable<FadingObject>
    {
        public Material originalMaterial;
        public Material transparentMaterial;
        public Renderer _renderer;
        public Vector3 position;
        [HideInInspector]
        public float initialAlpha;

        private void Awake()
        {
            position = transform.position;

            initialAlpha = 1f;

            if(_renderer == null)
            {
                _renderer = GetComponent<Renderer>();
            }
        }

        public bool Equals(FadingObject other)
        {
            return position.Equals(other.position);
        }

        public override int GetHashCode()
        {
            return position.GetHashCode();
        }
    }
}
