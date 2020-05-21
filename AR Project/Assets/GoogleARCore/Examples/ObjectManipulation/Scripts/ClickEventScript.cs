﻿using System.Collections;
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
        //var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;
        //if (Physics.Raycast(ray, hit))
        //{
        //    if (hit.collider.tag == "clickableCube") {
        //        //hit.collider.gameObject now refers to the 
        //        //cube under the mouse cursor if present
        //    }
        //}

        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                Debug.Log(hit);

                var cubeRenderer = hit.collider.GetComponent<Renderer>();

                //Debug.LogWarning(GetComponent<Renderer>().material);
                //Debug.LogWarning(Resources.FindObjectsOfTypeAll(typeof(Material)));
                Debug.LogWarning(cubeRenderer);
                Debug.Log(SharedData.Colours.Count);
                Debug.Log(SharedData.Colours[0]);

                if (SharedData.Colours.Count > 0)
                {
                    cubeRenderer.material.SetColor("_Color", SharedData.Colours[iterator]);
                    iterator = (iterator + 1) % SharedData.Colours.Count;
                }
            }
        }
    }


}
