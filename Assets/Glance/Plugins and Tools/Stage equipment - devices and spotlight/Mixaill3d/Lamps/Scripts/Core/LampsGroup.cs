using System.Collections.Generic;
using UnityEngine;

namespace Mixaill3d.Lamps.Scripts.Core
{
    public class LampsGroup : MonoBehaviour
    {
        [SerializeField] private LampBasicBehaviour _behaviour;
        [SerializeField] private float _timeOffset;
        [SerializeField] private float _speed = 1f;
        [SerializeField] private GameObject[] _lamps;

        private Renderer[] _additionalRenderers;
        private Renderer[] _renderers;
        private LampInfo[] _lampInfos;
        private LampsController _lampsController;

        public List<GameObject> FindLamps(Transform t)
        {
            List<GameObject> result = new List<GameObject>();
            FindLamps(t, result);
            return result;
        }

        private void BuildLampsInfos()
        {
            _lampInfos = new LampInfo[_lamps.Length];
            for (int i = 0; i < _lamps.Length; i++)
            {
                _lampInfos[i] = new LampInfo(_lamps[i]);
            }

            _renderers = new Renderer[_lamps.Length];
            for(int i = 0; i < _lamps.Length; i++)
            {
                _renderers[i] = _lampInfos[i].Renderer;
            }

            _additionalRenderers = new Renderer[_lamps.Length];
            for (int i = 0; i < _lamps.Length; i++)
            {
                _additionalRenderers[i] = _lampInfos[i].LightCone;
            }
        }

        private void FindLamps(Transform t, List<GameObject> result)
        {
            foreach (Transform child in t)
            {
                if (child.name.Contains("Lamp_"))
                    result.Add(child.gameObject);
                FindLamps(child, result);
            }
        }

        public void Awake()
        {
            InitializeLamps();
            _lampsController = LampsController.Instance;
            _lampsController.RegisterGroup(this);
        }

        public void OnDestroy()
        {
            if (_lampsController != null)
            {
                _lampsController.UnregisterGroup(this);
            }
        }

        private void InitializeLamps()
        {
            if (_lamps == null || _lamps.Length == 0)
            {
                _lamps = FindLamps(transform).ToArray();
            }
            BuildLampsInfos();
        }

        public void UpdateLightning()
        {
            _behaviour.ProcessBehaviour(_renderers, _additionalRenderers, _lampInfos, _timeOffset, _speed);
        }
    }

    public struct LampInfo
    {
        public Light Light { get; set; }
        public Renderer Renderer { get; set; }
        public Renderer LightCone { get; set; }
        public LampInfo(GameObject gameObject)
        {
            Renderer = gameObject.GetComponent<Renderer>();
            if(Renderer == null)
            { 
                Renderer = gameObject.GetComponentInChildren<Renderer>();
            }
            Light = gameObject.GetComponentInChildren<Light>();
            LightCone = null;
            var otherRenderers = gameObject.GetComponentsInChildren<Renderer>();
            foreach (var renderer in otherRenderers)
            {
                if (renderer != Renderer)
                {
                    LightCone = renderer;
                    break;
                }
            }
        }
    }
}