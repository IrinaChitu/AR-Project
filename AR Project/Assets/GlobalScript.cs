using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalScript : MonoBehaviour
{
	public static string selectedObject;

	public void SetObjectForManipulation(string objectName)
	{
		selectedObject = objectName;
		Debug.Log("Parameter object is " + objectName);
		Debug.Log("Selected object is " + selectedObject);
	}

	public void OpenObjectManipulationScene()
	{
		Debug.Log(GlobalScript.selectedObject);
		// // Material material = Resources.Load("MaterialTest", typeof(Material)) as Material;
		// Mesh mesh = ((GameObject)Resources.Load (GlobalScript.selectedObject)).GetComponent<MeshFilter>().mesh;
		//
		// //Instantiate a new game object, and add mesh components so it's visible
		// //We set the sharedMesh to the mesh we extracted from the prefab and the material
		// //of the MeshRenderer component
		// GameObject go = new GameObject(GlobalScript.selectedObject);
		// MeshFilter meshFilter = go.AddComponent<MeshFilter>();
		// meshFilter.sharedMesh = mesh;
		// MeshRenderer meshRenderer = go.AddComponent<MeshRenderer>();

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKey(KeyCode.Escape)) {
			Application.Quit();
		}
	}
}
