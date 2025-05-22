using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class AddressablesManager : MonoBehaviour
    {
        [SerializeField] private AssetReference assetReference;

        private void Awake()
        {
            assetReference.LoadAssetAsync<GameObject>().Completed += AddressablesManager_Completed;
        }

        private void AddressablesManager_Completed(AsyncOperationHandle<GameObject> asyncOperationHandle)
        {
            if(asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
            {
                Instantiate(asyncOperationHandle.Result);
            }
            else
            {
                Debug.LogError("Failed to Load");
            }
        }
    }
}
