using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UILoadingProgress : UIPopupBase
{
    //private RectTransform _loadImg;
    private const string ImageName = "Image";

    void Awake()
    {
        IsModalBlack = true;

        //_loadImg = GetImage(ImageName).transform as RectTransform;
    }

    /*void FixedUpdate()
    {
        _loadImg.Rotate(new Vector3(0, 0, -10f));
    }*/


}
