using UnityEngine;
using System.Collections;

namespace IMF
{

    public class MapScene : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            MainController.instance.Init();
            MainController.instance.GameStart();
        }

    }
}