using MyGames;
using UnityEngine;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class BarrierWall : MonoBehaviour
    {
        private const string HEX_COLOR = "_Hex_Color";

        [SerializeField] private float minimumDistanceBeforeVisibility;

        private Transform playerTransform;
        private MeshRenderer meshRenderer;
        private Material material;

        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            material = meshRenderer.material;
        }

        private void Start()
        {
            playerTransform = DependencyManager.PlayerTransform;
        }

        private void Update()
        {
            HandleMaterialTransparency();
        }

        private void HandleMaterialTransparency()
        {
            float distance = Utils.GetDistance(transform.position, playerTransform.position, excludeX: true, excludeY: true);

            Color color = material.GetColor(HEX_COLOR);
            color.a = Mathf.Clamp(1f - (distance / minimumDistanceBeforeVisibility), 0f, 1f);

            material.SetColor(HEX_COLOR, color);
        }
    }
}
