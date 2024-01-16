using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIContent;

public class UITester : MonoBehaviour
{

    private void Start()
    {
        UIManager.i.CreateHUD();
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            UIManager.i.CreateHUDUI<TestHUD>();
        }
    }

   
}
