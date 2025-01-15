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
        public static string platformLibrariesPath;
        void Awake()
        {
            Init();
        }

        public void Init()
        {
            Debug.Log("VRTNativeLoader: Init() called");
#if UNITY_EDITOR
            string path = UnityEditor.AssetDatabase.GetAssetPath(nativeLibraries);
            platformLibrariesPath = getEditorPlatformLibrariesPath(path);
#else
            platformLibrariesPath = getRuntimePlatformLibrariesPath();
#endif
            if (!Directory.Exists(platformLibrariesPath))
            {
                Debug.LogError($"VRTNativeLoader: Directory {platformLibrariesPath} does not exist");
                return;
            }
            addPathToDynamicLoaderPath(platformLibrariesPath);
        }

#if UNITY_EDITOR
        public static string getEditorPlatformLibrariesPath(string path)
        {
            string fullPath = Path.GetDirectoryName(path);
#if UNITY_EDITOR_WIN
            platformLibrariesPath = Path.Combine(fullPath, "win-x64");
#elif UNITY_EDITOR_OSX
            platformLibrariesPath = Path.Combine(fullPath, "mac");

#elif UNITY_EDITOR_LINUX
            platformLibrariesPath = Path.Combine(fullPath, "linux-x64");
#elif UNITY_ANDROID
            platformLibrariesPath = Path.Combine(fullPath, "android-arm");
#else
            Debug.LogFatal("VRTNativeLoader: Unknown editor runtime platform");
#endif
            Debug.Log($"VRTNativeLoader: platform path = {platformLibrariesPath}");
            platformLibrariesPath = Path.GetFullPath(platformLibrariesPath);
            Debug.Log($"VRTNativeLoader: abs platform path = {platformLibrariesPath}"); 
            return platformLibrariesPath;

        }
 
#else

        public static string getRuntimePlatformLibrariesPath()
        {
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX
            string nativeLibrariesPath = Path.Combine(Application.dataPath, "Plugins", "x86_64");
#elif UNITY_STANDALONE_OSX
            string nativeLibrariesPath = Path.Combine(Application.dataPath, "Libraries");
#endif
            return nativeLibrariesPath;
        }
#endif
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
