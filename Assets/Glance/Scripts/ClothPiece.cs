using UnityEngine;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class ClothPiece : MonoBehaviour
    {
        private const string BASE_TEXTURE_INDEX = "_Base_Texture_Index";
        private const string NORMAL_MAP_INDEX = "_Normal_Map_Index";
        private const string METALLIC_MAP_INDEX = "_Metallic_Map_Index";

        [SerializeField] private Renderer clothRenderer;
        [SerializeField] private int baseTextureIndex;
        [SerializeField] private int normalMapIndex;
        [SerializeField] private int metallicMapIndex;

        private void Awake()
        {
            ChangeMaterialAppearance();
        }

        private void ChangeMaterialAppearance()
        {
            MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
            clothRenderer.GetPropertyBlock(materialPropertyBlock);

            materialPropertyBlock.SetInt(BASE_TEXTURE_INDEX, baseTextureIndex);
            materialPropertyBlock.SetInt(NORMAL_MAP_INDEX, normalMapIndex);
            materialPropertyBlock.SetInt(METALLIC_MAP_INDEX, metallicMapIndex);

            clothRenderer.SetPropertyBlock(materialPropertyBlock);  
        }
    }
}
