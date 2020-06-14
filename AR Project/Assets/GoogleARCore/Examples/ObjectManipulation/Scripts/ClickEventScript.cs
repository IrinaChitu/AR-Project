using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ClickEventScript : MonoBehaviour
{

    private int iterator = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Am apasat pe obiect");

                var allComponents = hit.collider.GetComponentsInChildren<Component>(true);
                foreach (var component in allComponents)
                {
                    if (component.GetType() == typeof(MeshRenderer))
                    {
                        //renderer.material.SetColor("_Color", Color.red);
                        if (SharedData.Colours.Count > 0)
                        {
                            var renderer = component.GetComponent<Renderer>();
                            renderer.material.SetColor("_Color", SharedData.Colours[iterator]);
                        }
                    }
                }
                iterator = (iterator + 1) % SharedData.Colours.Count;
            }
        }
    }
}