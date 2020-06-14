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
