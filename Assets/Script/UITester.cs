using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIContent;

public class UITester : MonoBehaviour
{
    public Canvas canvas;

    private void Start()
    {
        UIManager.i.CreateHUD();
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Destroy(canvas.gameObject);
            UIManager.i.CreateHUDUI<TestHUD>();

            Destroy(gameObject);
        }
    }

   
}
