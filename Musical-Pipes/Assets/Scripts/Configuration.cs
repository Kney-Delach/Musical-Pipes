using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameControllers;
using LevelManagement;

public class Configuration : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #if UNITY_ANDROID
            Screen.orientation = ScreenOrientation.LandscapeLeft;
            PlayerController.Instance.IsMobile = true;
        //PauseMenu.isMobile = true;
        #endif

        #if UNITY_EDITOR
                PlayerController.Instance.IsMobile = false;
        #endif
    }


}
