#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System.IO;
using VRT.Core;

namespace VRT.NativeLibraries {
    public class CopyNativeDLLs : IPostprocessBuildWithReport
    {
        public int callbackOrder { get { return 0; } }
        public void OnPostprocessBuild(BuildReport report)
        {
            Debug.LogWarning("xxxjack CopyNativeDLLs.OnPostprocessBuild not implemented yet");
            var assets = AssetDatabase.FindAssets("t:NativeLibraryDirectory");
            if (assets.Length == 0)
            {
                Debug.LogWarning("No NativeLibraryDirectory found");
                return;
            }
            foreach(var guid in assets)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
#if xxxjack_bad
                var nativeLibraries = AssetDatabase.LoadAssetAtPath<NativeLibraryDirectory>(path);
                if (nativeLibraries == null)
                {
                    Debug.LogWarning("NativeLibraryDirectory not found");
                    return;
                }
#endif
                string nativeLibrariesPath = Path.GetDirectoryName(path);
                string platformLibrariesPath;
                if (report.summary.platform == BuildTarget.StandaloneWindows64)
                {
                    platformLibrariesPath = Path.Combine(nativeLibrariesPath, "win-x64");
                }
                else if (report.summary.platform == BuildTarget.StandaloneOSX)
                {
                    platformLibrariesPath = Path.Combine(nativeLibrariesPath, "mac-x64");
                }
                else if (report.summary.platform == BuildTarget.StandaloneLinux64)
                {
                    platformLibrariesPath = Path.Combine(nativeLibrariesPath, "linux-x64");
                }
                else if (report.summary.platform == BuildTarget.Android)
                {
                    platformLibrariesPath = Path.Combine(nativeLibrariesPath, "android-arm");
                }
                else
                {
                    Debug.LogError("Unknown runtime platform");
                    return;
                }
                platformLibrariesPath = Path.GetFullPath(platformLibrariesPath);
                if (!Directory.Exists(platformLibrariesPath))
                {
                    Debug.LogError($"Directory {platformLibrariesPath} does not exist");
                    return;
                }
                Debug.Log($"CopyNativeDLLs.OnPostprocessBuild: platform path = {platformLibrariesPath}");
                if (report.summary.platform == BuildTarget.StandaloneWindows64)
                {
                    var buildOutputPath = Path.GetDirectoryName(report.summary.outputPath);
                    var dataDirs = Directory.GetDirectories(buildOutputPath, "*_Data");
                    if (dataDirs.Length != 1)
                    {
                        Debug.LogError($"Expected 1 *_Data directory but found {dataDirs.Length}");
                        return;
                    }
                    var dllOutputPath = Path.Combine(buildOutputPath, dataDirs[0], "Plugins", "x86_64");
                    CopyFiles(platformLibrariesPath, dllOutputPath);
                }
                else if (report.summary.platform == BuildTarget.StandaloneOSX)
                {
                    CopyFiles(platformLibrariesPath, report.summary.outputPath + "/Contents/");
                }
                else if (report.summary.platform == BuildTarget.StandaloneLinux64)
                {
                    CopyFiles(platformLibrariesPath, Path.GetDirectoryName(report.summary.outputPath) + "/");
                }
                else if (report.summary.platform == BuildTarget.Android)
                {
                    Debug.LogWarning("Including native DLLs not supported for Android builds");
                }
            }

        }

        void CopyFiles(string srcDir, string dstDir)
        {
            Debug.Log($"CopyNativeDLLs.CopyFiles src {srcDir} dst {dstDir}");
            if (!Directory.Exists(dstDir))
            {
                Directory.CreateDirectory(dstDir);
            }
            foreach (var file in Directory.GetFiles(srcDir))
            {
                if (file.EndsWith(".meta"))
                {
                    continue;
                }
                string fileName = Path.GetFileName(file);
                string destFile = Path.Combine(dstDir, fileName);
                File.Copy(file, destFile, true);
                Debug.Log($"CopyNativeDLLs.CopyFiles copied {file} to {destFile}");
            }
        }
    }
}
#endif