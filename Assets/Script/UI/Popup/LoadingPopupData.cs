using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UIContent
{
    public class LoadingPopupData : UIData
    {
        public int Number = 10;
        public int Max;
        public int Min;

        public override void OpenInitData()
        {
            Max = 100;
            Min = 0;
        }

    }
}
