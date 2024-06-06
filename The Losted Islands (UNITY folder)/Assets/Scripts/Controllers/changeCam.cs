using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeCam : MonoBehaviour
{

    [SerializeField] Camera cam;
    [SerializeField] Camera cam2;

    private bool isView1 = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.F1)) changeView();

    }

    private void changeView()
    {
        if (isView1) 
        {
            cam.enabled = false;
            cam2.enabled = true;
            isView1 = false;
        }
        else
        {
            cam.enabled = true;
            cam2.enabled = false;
            isView1 = true;
        }
    }

}
