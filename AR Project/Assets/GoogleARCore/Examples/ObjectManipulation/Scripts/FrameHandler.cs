namespace GoogleARCore.Examples.ObjectManipulation
{
    using System.Collections;
    using System.Collections.Generic;
    using GoogleARCore;
    using UnityEngine;
    using UnityEngine.Networking;
    using System;
    using System.IO;
    using GoogleARCore.Examples.ComputerVision;
    using UnityEngine.UI;

    public class FrameHandler : MonoBehaviour
    {

        private Texture2D _texture;

        //private void Awake()
        //{
        //    _texture = new Texture2D(Screen.width, Screen.height, _format, false);
        //}

        //private void Update()
        //{
        //    using (CameraImageBytes imageBytes = Frame.CameraImage.AcquireCameraImageBytes())
        //    {
        //        if (!imageBytes.IsAvailable) return;

        //        int size = imageBytes.Width * imageBytes.Height;
        //        byte[] yBuff = new byte[size];
        //        System.Runtime.InteropServices.Marshal.Copy(imageBytes.Y, yBuff, 0, size);
        //        _texture.Apply();
        //    }
        //}

        public void TakeScreenShot()
        {
            RenderTexture renderTexture = RenderTexture.GetTemporary(Screen.width, Screen.height, 16);
            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            renderResult.ReadPixels(rect, 0, 0);

            byte[] byteArray = renderResult.EncodeToPNG();
            File.WriteAllBytes(Path.Combine(Application.persistentDataPath, "test-frame.jpg"), byteArray);
            Debug.LogWarning("Am salvat ss");
            RenderTexture.ReleaseTemporary(renderTexture);
        }

        public void SaveImage()
        {

            var image = Frame.CameraImage.AcquireCameraImageBytes();
            Debug.LogWarning("amm luat bytes");
            _texture = new Texture2D(image.Width, image.Height, TextureFormat.RGBA32, false, false);
            byte[] m_EdgeDetectionResultImage = new byte[image.Width * image.Height * 4];

            Debug.LogWarning("create textura");
            System.Runtime.InteropServices.Marshal.Copy(image.Y, m_EdgeDetectionResultImage, 0, image.Width * image.Height * 4);

            _texture.LoadRawTextureData(m_EdgeDetectionResultImage);
            _texture.Apply();
            Debug.LogWarning("Incarca bitii");

            //var encodedJpg = _texture.EncodeToJPG();

            //_texture = new Texture2D(Screen.width, Screen.height, _format, false);
            //using (CameraImageBytes imageBytes = Frame.CameraImage.AcquireCameraImageBytes())
            //{

            //    int size = imageBytes.Width * imageBytes.Height;
            //    byte[] yBuff = new byte[size];
            //    System.Runtime.InteropServices.Marshal.Copy(imageBytes.Y, yBuff, 0, size);
            //    _texture.Apply();
            //}

            Debug.LogWarning("Am ajuns in save");
            var encodedJpg = _texture.EncodeToJPG();
            Debug.LogWarning("Encodat textura");
            File.WriteAllBytes(Path.Combine(Application.persistentDataPath, "test.jpg"), encodedJpg);
            Debug.LogWarning("salvat fisierul in " + Path.Combine(Application.persistentDataPath, "test.jpg"));
        }

        public void ChangeObjectTypeToCube()
        {
            Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            screenShot.Apply();

            Debug.LogWarning("Am ajuns aici");
            var encodedJpg = screenShot.EncodeToJPG();
            Debug.LogWarning("Encodat textura");
            File.WriteAllBytes(Path.Combine(Application.persistentDataPath, "test.jpg"), encodedJpg);
            Debug.LogWarning("salvat fisierul in " + Path.Combine(Application.persistentDataPath, "test.jpg"));

            StartCoroutine(getRequest(imageBytes: encodedJpg));
        }

        public IEnumerator getRequest(byte[] imageBytes)
        {
            UnityWebRequest unityWebRequest = new UnityWebRequest("http://flask-env.eba-6nu7diz5.eu-west-3.elasticbeanstalk.com/upload", "POST");
            unityWebRequest.uploadHandler = new UploadHandlerRaw(imageBytes);
            unityWebRequest.SetRequestHeader("content-Type", "image/jpeg");

            DownloadHandlerBuffer downloadHandlerBuffer = new DownloadHandlerBuffer();
            unityWebRequest.downloadHandler = downloadHandlerBuffer;

            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
            {
                Debug.Log(unityWebRequest.error);
            }
            else
            {
                Debug.Log("Form upload complete! Status Code: " + unityWebRequest.responseCode);

                string response = unityWebRequest.downloadHandler.text;
                Debug.LogWarning("Am primit: " + response);
            }

        }

    }
}

