﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace OnefallGames
{
    public class EditorTools : EditorWindow
    {


        [MenuItem("Tools/Reset PlayerPrefs")]
        public static void ResetPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("*************** PlayerPrefs Was Deleted ***************");
        }


        [MenuItem("Tools/Capture Screenshot")]
        public static void CaptureScreenshot()
        {
            ScreenCapture.CaptureScreenshot("C:/Users/TIENNQ/Desktop/icon.png");
        }

    }
}
