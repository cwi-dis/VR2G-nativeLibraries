using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRT.NativeLibraries
{
    public class VRTNativeLibraries : MonoBehaviour
    {
        void Awake()
        {
            Init();
        }

        public static void Init()
        {
            Debug.Log("VRT.NativeLibraries.Support: Init() called");
        }
    }
}
