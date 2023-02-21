using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace VRT.NativeLibraries
{
    public class VRTNativeLibraries : MonoBehaviour
    {
        public NativeLibraryDirectory nativeLibraries;
        void Awake()
        {
            Init();
        }

        public void Init()
        {
            Debug.Log("VRT.NativeLibraries.Support: Init() called");
            var path = UnityEditor.AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(nativeLibraries));
            Debug.Log($"VRT.NativeLibraries.Support: path = {path}");
        }
    }
}
