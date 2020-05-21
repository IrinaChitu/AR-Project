//-----------------------------------------------------------------------
// <copyright file="PawnManipulator.cs" company="Google">
//
// Copyright 2019 Google LLC. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace GoogleARCore.Examples.ObjectManipulation
{
    using GoogleARCore;
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine.Networking;
    using System;
    using System.IO;
    using GoogleARCore.Examples.ComputerVision;
    using UnityEngine.UI;

    /// <summary>
    /// Controls the placement of objects via a tap gesture.
    /// </summary>
    public class PawnManipulator : Manipulator
    {
        public bool spawn = true;
        /// <summary>
        /// The first-person camera being used to render the passthrough camera image (i.e. AR
        /// background).
        /// </summary>
        public Camera FirstPersonCamera;

        /// <summary>
        /// A prefab to place when a raycast from a user touch hits a plane.
        /// </summary>
        public GameObject PawnPrefab;

        /// <summary>
        /// Manipulator prefab to attach placed objects to.
        /// </summary>
        public GameObject ManipulatorPrefab;

        /// <summary>
        /// Returns true if the manipulation can be started for the given gesture.
        /// </summary>
        /// <param name="gesture">The current gesture.</param>
        /// <returns>True if the manipulation can be started.</returns>
        protected override bool CanStartManipulationForGesture(TapGesture gesture)
        {
            if (gesture.TargetObject == null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Function called when the manipulation is ended.
        /// </summary>
        /// <param name="gesture">The current gesture.</param>
        protected override void OnEndManipulation(TapGesture gesture)
        {
            if (gesture.WasCancelled)
            {
                return;
            }

            // If gesture is targeting an existing object we are done.
            if (gesture.TargetObject != null)
            {
                return;
            }

            // Raycast against the location the player touched to search for planes.
            TrackableHit hit;
            TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon;

            if (Frame.Raycast(
                gesture.StartPosition.x, gesture.StartPosition.y, raycastFilter, out hit))
            {
                // Use hit pose and camera pose to check if hittest is from the
                // back of the plane, if it is, no need to create the anchor.
                if ((hit.Trackable is DetectedPlane) &&
                    Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
                        hit.Pose.rotation * Vector3.up) < 0)
                {
                    Debug.Log("Hit at back of the current DetectedPlane");
                }
                else
                {
                    if (spawn == true)
                    {
                        var gameObject = Instantiate(PawnPrefab, hit.Pose.position, hit.Pose.rotation);
                        spawn = false;

                        // Instantiate manipulator.
                        var manipulator =
                            Instantiate(ManipulatorPrefab, hit.Pose.position, hit.Pose.rotation);

                        // Make game object a child of the manipulator.
                        gameObject.transform.parent = manipulator.transform;

                        // Create an anchor to allow ARCore to track the hitpoint as understanding of
                        // the physical world evolves.
                        var anchor = hit.Trackable.CreateAnchor(hit.Pose);

                        // Make manipulator a child of the anchor.
                        manipulator.transform.parent = anchor.transform;

                        // Select the placed object.
                        manipulator.GetComponent<Manipulator>().Select();


                        //MeshFilter yourMesh;

                        Debug.Log("Setez culoarea");

                        //yourMesh = PawnPrefab.GetComponent<MeshFilter>();
                        //yourMesh.sharedMesh = Resources.Load<Mesh>("MeshBody1");

                        //Debug.Log(yourMesh);

                        //yourMesh.GetComponent<Renderer>().material.color = new Color(0, 1, 0, 1);

                        var cubeRenderer = gameObject.GetComponent<Renderer>();

                        //Debug.LogWarning(GetComponent<Renderer>().material);
                        //Debug.LogWarning(Resources.FindObjectsOfTypeAll(typeof(Material)));
                        Debug.LogWarning(cubeRenderer);

                        //Call SetColor using the shader property name "_Color" and setting the color to red
                        cubeRenderer.material.SetColor("_Color", Color.red);

                        ChangeObjectTypeToCube();
                    }
                }
            }
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
            UnityWebRequest unityWebRequest = new UnityWebRequest("http://colorapi-env.eba-6nu7diz5.eu-west-3.elasticbeanstalk.com/?fbclid=IwAR33tXPifJ4ZIM59W7_m-faAK3909T2GgV6i3TstlnBLxcfSUrkQX3aBDHU", "POST");
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

            SharedData.Colours = new List<Color>() { Color.red, Color.blue, Color.green };

        }
    }
}
