using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace VRT.NativeLibraries
{
    public class VRTNativeLoader : MonoBehaviour
    {
        public NativeLibraryDirectory nativeLibraries;
        private string platformLibrariesPath;
        void Awake()
        {
            Init();
        }

        public void Init()
        {
            Debug.Log("VRTNativeLoader: Init() called");
            string nativeLibrariesPath;
#if UNITY_EDITOR
            string path = UnityEditor.AssetDatabase.GetAssetPath(nativeLibraries);
            nativeLibrariesPath = Path.GetDirectoryName(path);
#else
            nativeLibrariesPath = "DLLs";
#endif
            Debug.Log($"VRTNativeLoader: path = {nativeLibrariesPath}");

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            platformLibrariesPath = Path.Combine(nativeLibrariesPath, "win-x64");
#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
            platformLibrariesPath = Path.Combine(nativeLibrariesPath, "mac-x64");

#elif UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX
            platformLibrariesPath = Path.Combine(nativeLibrariesPath, "linux-x64");
#elif UNITY_ANDROID
            platformLibrariesPath = Path.Combine(nativeLibrariesPath, "android-arm");
#else
            Debug.LogFatal("VRTNativeLoader: Unknown runtime platform");
#endif
            Debug.Log($"VRTNativeLoader: path = {platformLibrariesPath}");

        }
    }
}
