using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Runtime.InteropServices;


namespace VRT.NativeLibraries
{
    public class VRTNativeLoader : MonoBehaviour
    {

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool AddDllDirectory(string lpPathName);

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
            platformLibrariesPath = Path.Combine(nativeLibrariesPath, "mac");

#elif UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX
            platformLibrariesPath = Path.Combine(nativeLibrariesPath, "linux-x64");
#elif UNITY_ANDROID
            platformLibrariesPath = Path.Combine(nativeLibrariesPath, "android-arm");
#else
            Debug.LogFatal("VRTNativeLoader: Unknown runtime platform");
#endif
            Debug.Log($"VRTNativeLoader: platform path = {platformLibrariesPath}");
            platformLibrariesPath = Path.GetFullPath(platformLibrariesPath);
            Debug.Log($"VRTNativeLoader: abs platform path = {platformLibrariesPath}");
            if (!Directory.Exists(platformLibrariesPath))
            {
                Debug.LogError($"VRTNativeLoader: Directory {platformLibrariesPath} does not exist");
                return;
            }
            addPathToDynamicLoaderPath(platformLibrariesPath);
        }
        void addPathToDynamicLoaderPath(string path)
        {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            const string dynPathName = "PATH";
#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
            const string dynPathName = "DYLD_LIBRARY_PATH";
#else
            const string dynPathName = "LD_LIBRARY_PATH";
#endif

            string dynamicLoaderPath = System.Environment.GetEnvironmentVariable(dynPathName);
            if (dynamicLoaderPath == null)
            {
                dynamicLoaderPath = path;
            }
            else
            {
                if (dynamicLoaderPath.Contains(path))
                {
                    return;
                }
                dynamicLoaderPath = path + Path.PathSeparator + dynamicLoaderPath;
            }
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            AddDllDirectory(path);
#endif
            System.Environment.SetEnvironmentVariable(dynPathName, dynamicLoaderPath);
            Debug.Log($"VRTNativeLoader: {dynPathName} = {dynamicLoaderPath}");
        }
    }
}
