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
		Debug.Log(selectedObject);
	}

	public void OpenObjectManipulationScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}
