using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class BuilderEditor : EditorWindow
{
    public TextField ProductName;
    public TextField BuildName;
    public TextField OutputDir;
    public TextField ItchAccount;
    public TextField ItchProject;
    public TextField ItchProjectChannel;

    private UnityEditor.UIElements.MaskField bldopts;

    [MenuItem("Window/Builder")]
    public static void Init()
    {
        BuilderEditor bld = GetWindow<BuilderEditor>();
        bld.titleContent = new GUIContent("BUILDER!!");
        EditorWindow.GetWindow(typeof(BuilderEditor)).Show();
    }

    public void CreateGUI()
    {
        var basicWindow = new ScrollView();
        rootVisualElement.Add(basicWindow);

        ProductName = new TextField("Product Name") { value = Application.productName };
        BuildName = new TextField("Build Name") { value = System.DateTime.Now.ToString("yyyy.MM.dd") };
        var pwd = Directory.GetCurrentDirectory();
        OutputDir = new TextField("Output Path") { value = string.Join("", pwd, "-builds") };

        basicWindow.Add(ProductName);
        basicWindow.Add(BuildName);
        basicWindow.Add(OutputDir);

        var buildWinButton = new Button(() => BuildIt("win")) { text = "Build Windows" };
        var buildWebGLButton = new Button(() => BuildIt("webgl")) { text = "Build WebGL" };
        basicWindow.Add(buildWinButton);
        basicWindow.Add(buildWebGLButton);

	/*
        basicWindow.Add(new UnityEngine.UIElements.Label());

        ItchAccount = new TextField("Itch.io Account") { };
        ItchProject = new TextField("Project Name") { value = ProductName.value };
        ItchProjectChannel = new TextField("Project Channel") { };

        basicWindow.Add(ItchAccount);
        basicWindow.Add(ItchProject);
        basicWindow.Add(ItchProjectChannel);

        var publishWinButton = new Button(() => PublishIt("win")) { text = "Publish Windows Build" };
        var publishWebGLButton = new Button(() => PublishIt("webgl")) { text = "Publish WebGL Build" };
        basicWindow.Add(publishWinButton);
        basicWindow.Add(publishWebGLButton);
	*/
    }

    public void BuildIt(string target)
    {
        var buildDir = string.Join("-", ProductName.value, target);
        var buildName = string.Join("-", buildDir, BuildName.value);
        var outputDirPath = Path.Combine(OutputDir.value, buildDir, buildName);

        var outputFile = "";
        BuildTarget buildTarget = 0;
        BuildTargetGroup buildTargetGroup = 0;
        switch (target)
        {
            case "win":
                outputFile = string.Join(".", ProductName.value, "exe");
                buildTarget = BuildTarget.StandaloneWindows64;
                buildTargetGroup = BuildTargetGroup.Standalone;
                break;
            case "webgl":
                outputFile = "index.html";
                buildTarget = BuildTarget.WebGL;
                buildTargetGroup = BuildTargetGroup.WebGL;
                break;
        }
        var outputPath = Path.Join(outputDirPath, outputFile);

        Builder.BuildProject(buildTargetGroup, buildTarget, outputPath);

        Debug.LogFormat("Done building Target: {0} \n Output Path: {1}", buildTarget, outputPath);
    }

    public void PublishIt(string target)
    {
        var buildDir = string.Join("-", ProductName.value, target);
        var buildName = string.Join("-", buildDir, BuildName.value);
        var outputDirPath = Path.Combine(OutputDir.value, buildDir, buildName);

        var project = string.Join("/", ItchAccount.value, ItchProject.value);
        var pushTargets = new List<string>();
        switch (target)
        {
            case "win":
                var winBuild = string.Join("-", ItchProjectChannel.value, "win");
                pushTargets.Add(string.Join(":", project, winBuild));
                pushTargets.Add(string.Join(":", project, string.Join("-", winBuild, BuildName.value)));
                break;
            case "webgl":
                pushTargets.Add(string.Join(":", project, ItchProjectChannel.value));
                pushTargets.Add(string.Join(":", project, ItchProjectChannel.value + "-webgl"));
                break;
        }

        foreach (string pushTarget in pushTargets)
        {
            Builder.PublishBuild(outputDirPath, pushTarget);
            Debug.LogFormat("Target Published: {0} \n Build: {1}", pushTarget, outputDirPath);
        }


    }
}
