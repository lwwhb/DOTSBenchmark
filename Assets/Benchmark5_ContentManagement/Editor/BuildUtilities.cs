using System;
using System.Collections.Generic;
using System.IO;
using Common.Scripts;
using Unity.Scenes.Editor;
using UnityEngine;
using UnityEditor;
using Unity.Entities.Build;
using Unity.Entities.Content;
using Unity.Entities.Serialization;

namespace Benchmark5_ContentManagement.Editor
{
    static class BuildUtilities
    {
        private static string PathCombine(string path1, string path2)
        {
            if (!Path.IsPathRooted(path2)) return Path.Combine(path1, path2);
 
            path2 = path2.TrimStart(Path.DirectorySeparatorChar);
            path2 = path2.TrimStart(Path.AltDirectorySeparatorChar);
 
            return Path.Combine(path1, path2);
        }

        //不构建Player，只构建Content
        [MenuItem("DOTSBenchmark/Build/Content Update")]
        static void CreateContentUpdate()
        {
            var buildFolder = EditorUtility.OpenFolderPanel("Select Build To Publish", Path.GetDirectoryName(Application.dataPath), "Builds");
            if (!string.IsNullOrEmpty(buildFolder))
            {
                LogUtility.ContentDeliveryLog( $"BuildFolder: {buildFolder}");
                var buildTarget = EditorUserBuildSettings.activeBuildTarget;
                var tmpBuildFolder = PathCombine(Path.GetDirectoryName(Application.dataPath), $"/Library/ContentUpdateBuildDir/{PlayerSettings.productName}");
                
                LogUtility.ContentDeliveryLog( $"TempBuildFolder: {tmpBuildFolder}");
                
                var instance = DotsGlobalSettings.Instance;
                var playerGuid = instance.GetPlayerType() == DotsGlobalSettings.PlayerType.Client ? instance.GetClientGUID() : instance.GetServerGUID();
                if (!playerGuid.IsValid)
                    throw new Exception("Invalid Player GUID");

                var subSceneGuids = new HashSet<Unity.Entities.Hash128>();
                for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
                {
                    var ssGuids = EditorEntityScenes.GetSubScenes(EditorBuildSettings.scenes[i].guid);
                    foreach (var ss in ssGuids)
                        subSceneGuids.Add(ss);
                }
                RemoteContentCatalogBuildUtility.BuildContent(subSceneGuids, playerGuid, buildTarget, tmpBuildFolder);

                var publishFolder = Path.Combine(Path.GetDirectoryName(Application.dataPath) ?? string.Empty, "Builds", $"{buildFolder}-RemoteContent");
                LogUtility.ContentDeliveryLog( $"PublishFolder: {publishFolder}");
                if (RemoteContentCatalogBuildUtility.PublishContent(tmpBuildFolder, publishFolder,
                        f => new string[] { "all" }))
                {
                    LogUtility.ContentDeliveryLog( "ContentUpdate succeeded.");
                }
                else
                {
                    LogUtility.ContentDeliveryLogError( "ContentUpdate failed.");
                }
            }
        }
        
        [MenuItem("DOTSBenchmark/Content Management/Log UntypedWeakReferenceId of Selected")]
        private static void LogWeakReferenceIDs()
        {
            UnityEngine.Object[] selectedObjects = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets);
            for (int i = 0; i < selectedObjects.Length; i++)
            {
                LogUtility.ContentDeliveryLog($"{selectedObjects[i].name}: {UntypedWeakReferenceId.CreateFromObjectInstance(selectedObjects[i])}");
            }
        }
    }
}