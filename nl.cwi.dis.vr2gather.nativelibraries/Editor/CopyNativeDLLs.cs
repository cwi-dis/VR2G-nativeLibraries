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
                var srcDir = VRTNativeLoader.getEditorPlatformLibrariesPath(path);
                var dstDir = getBuildPlatformLibrariesPath(report);

                if ( string.IsNullOrEmpty(srcDir) || string.IsNullOrEmpty(dstDir) ||
                    !Directory.Exists(srcDir) || !Directory.Exists(dstDir)
                    )
                {
                    Debug.LogWarning("CopyNativeDLLs.OnPostprocessBuild: No native libraries copied");
                    return;
                }
                CopyFiles(srcDir, dstDir);
            }
        }
        public static string getBuildPlatformLibrariesPath(BuildReport report)
        {
            if (
                report.summary.platform == BuildTarget.StandaloneWindows64
                ||
                report.summary.platform == BuildTarget.StandaloneLinux64
                )
            {
                var buildOutputPath = Path.GetDirectoryName(report.summary.outputPath);
                var dataDirs = Directory.GetDirectories(buildOutputPath, "*_Data");
                if (dataDirs.Length != 1)
                {
                    Debug.LogError($"Expected 1 *_Data directory but found {dataDirs.Length}");
                    return null;
                }
                var dllOutputPath = Path.Combine(buildOutputPath, dataDirs[0], "Plugins", "x86_64");
                return dllOutputPath;
            }
            else if (report.summary.platform == BuildTarget.StandaloneOSX)
            {
                var dllOutputPath = Path.Combine(report.summary.outputPath, "Contents", "Libraries");
                return dllOutputPath;
            }
            else if (report.summary.platform == BuildTarget.Android)
            {
                Debug.LogWarning("Including native DLLs not supported for Android builds");
                return null;
            }
            return null;
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