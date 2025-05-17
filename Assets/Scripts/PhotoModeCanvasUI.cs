using System;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.UI;
using Debug = MyGames.Debug;

namespace nostra.booboogames.hangoutconcert
{
    public class PhotoModeCanvasUI : MonoBehaviour
    {
        [SerializeField] private Canvas photoModeCameraCanvas;
        [SerializeField] private GameObject container;
        [SerializeField] private Camera photoModeCamera;

        [SerializeField] private Image photoImage;
        [SerializeField] private Button capturePhotoButton;
        [SerializeField] private Button closeButton;

        public static event Action OnPhotoCaptured;

        private Camera mainCamera;
        private int width;
        private int height;

        private void Awake()
        {
            mainCamera = Camera.main;

            width = (int)photoModeCameraCanvas.GetComponent<RectTransform>().rect.width;
            height = (int)photoModeCameraCanvas.GetComponent<RectTransform>().rect.height;
        }

        private void Start()
        {
            capturePhotoButton.onClick.AddListener(() =>
            {
                Capture();
                Show();
            });

            closeButton.onClick.AddListener(() =>
            {
                Hide();
            });
        }

        private void Show()
        {
            container.SetActive(true);
        }

        private void Hide()
        {
            container.SetActive(false);
        }

        private void Capture()
        {
            photoModeCamera.transform.SetPositionAndRotation(mainCamera.transform.position, mainCamera.transform.rotation);

            Texture2D texture2D = ConvertToTexture2D();
            Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero);
            photoImage.sprite = sprite;

            OnPhotoCaptured?.Invoke();
        }

        private Texture2D ConvertToTexture2D()
        {
            RenderTexture renderTexture = new RenderTexture(width, height, 24);
            photoModeCamera.targetTexture = renderTexture;
            photoModeCamera.Render();
            RenderTexture currentRenderTexture = RenderTexture.active;

            RenderTexture.active = renderTexture;
            Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
            RenderTexture.active = renderTexture;

            tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            tex.Apply();
            photoModeCamera.targetTexture = null;
            RenderTexture.active = currentRenderTexture;
            Destroy(renderTexture);
            return tex;
        }
    }
}