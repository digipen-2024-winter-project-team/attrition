using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Attrition
{
    public static class BuildScript
    {
        private const string WebGLBuildProfileName = "WebGL";
        private const string DefaultBuildProfile = WebGLBuildProfileName;
        private const string BuildOutputPath = "build/WebGL";
        private const string MenuItemPath = "Build/Build for WebGL";
        private const string NoScenesErrorMessage = "No scenes are enabled in Build Settings. Aborting build.";

        [MenuItem(MenuItemPath)]
        public static void BuildForWebGL()
        {
            // Default to "WebGL" profile
            var buildProfile = CommandLineArgs.GetArg("-buildProfile") ?? DefaultBuildProfile;

            // Apply settings for the WebGL profile
            ApplyBuildProfileSettings(buildProfile, false);

            // Get the enabled scenes from Build Settings
            var scenes = EditorBuildSettings.scenes
                .Where(scene => scene.enabled)
                .Select(scene => scene.path)
                .ToArray();

            if (scenes.Length == 0)
            {
                Debug.LogError(NoScenesErrorMessage);
                return;
            }

            // Build project
            BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = BuildOutputPath,
                target = BuildTarget.WebGL
            });

            Debug.Log($"Build completed with profile: {buildProfile}");
        }

        private static void ApplyBuildProfileSettings(string profile, bool isDevelopmentBuild = false)
        {
            if (profile == WebGLBuildProfileName)
            {
                // Customize WebGL settings here if needed
                EditorUserBuildSettings.development = isDevelopmentBuild; // Set to true for development builds
                Debug.Log("Applying WebGL build settings...");
            }
            else
            {
                Debug.LogWarning($"Unknown profile: {profile}. Defaulting to WebGL settings.");
            }
        }
    }

    public static class CommandLineArgs
    {
        public static string GetArg(string name)
        {
            var args = System.Environment.GetCommandLineArgs();
            for (var i = 0; i < args.Length; i++)
            {
                if (args[i] == name && args.Length > i + 1)
                {
                    return args[i + 1];
                }
            }
            return null;
        }
    }
}
