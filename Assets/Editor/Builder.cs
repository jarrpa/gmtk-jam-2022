using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;

public class Builder
{
    public static void BuildProject(BuildTargetGroup buildTargetGroup, BuildTarget buildTarget, string outputPath)
    {
        var options = new BuildPlayerOptions
        {
            scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes),
            target = buildTarget,
            targetGroup = buildTargetGroup,
            locationPathName = outputPath,
        };

        EditorUserBuildSettings.SwitchActiveBuildTarget(buildTargetGroup, buildTarget);
        BuildPipeline.BuildPlayer(options);
    }

    public static void PublishBuild(string buildDir, string pushTarget)
    {
        var processInfo = new ProcessStartInfo("butler.exe");
        processInfo.Arguments = string.Join(" ", "push", @buildDir, @pushTarget);
        processInfo.CreateNoWindow = false;
        Process.Start(processInfo);
    }
}
