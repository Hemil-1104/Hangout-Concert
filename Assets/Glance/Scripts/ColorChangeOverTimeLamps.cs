using UnityEngine;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class ColorChangeOverTimeLamps : MonoBehaviour
    {
        private MeshFilter meshFilter;
        private Mesh mesh;

        private void Awake()
        {
            meshFilter = GetComponent<MeshFilter>();
            mesh = meshFilter.mesh;

            GetUVs();
        }

        private void GetUVs()
        {
            Vector3[] uvs = mesh.vertices;

            Debug.Log($"Count: {uvs.Length} {uvs.Length / 18}");
            //foreach (Vector2 v in uvs)
            //{
            //    Debug.Log(v);
            //}
        }
    }
}
