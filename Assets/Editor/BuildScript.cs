using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Attrition
{
    /// <summary>
    /// Provides functionality for building the Unity project for WebGL.
    /// </summary>
    public static class BuildScript
    {
        private const string WebGLBuildProfileName = "WebGL";
        private const string DefaultBuildProfile = WebGLBuildProfileName;
        private const string BuildOutputPath = "build/WebGL";
        private const string MenuItemPath = "Build/Build for WebGL";
        private const string NoScenesErrorMessage = "No scenes are enabled in Build Settings. Aborting build.";

        /// <summary>
        /// Builds the Unity project for the WebGL platform.
        /// </summary>
        [MenuItem(MenuItemPath)]
        public static void BuildForWebGL()
        {
            var buildProfile = CommandLineArgs.GetArg("-buildProfile") ?? DefaultBuildProfile;
            
            ApplyBuildProfileSettings(buildProfile, false);
            
            var scenes = EditorBuildSettings.scenes
                .Where(scene => scene.enabled)
                .Select(scene => scene.path)
                .ToArray();

            if (scenes.Length == 0)
            {
                Debug.LogError(NoScenesErrorMessage);
                return;
            }

            BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = BuildOutputPath,
                target = BuildTarget.WebGL
            });

            Debug.Log($"Build completed with profile: {buildProfile}");
        }

        /// <summary>
        /// Applies settings specific to the provided build profile.
        /// </summary>
        /// <param name="profile">The name of the build profile.</param>
        /// <param name="isDevelopmentBuild">Indicates whether the build is a development build.</param>
        private static void ApplyBuildProfileSettings(string profile, bool isDevelopmentBuild = false)
        {
            if (profile == WebGLBuildProfileName)
            {
                EditorUserBuildSettings.development = isDevelopmentBuild;
                Debug.Log("Applying WebGL build settings...");
            }
            else
            {
                Debug.LogWarning($"Unknown profile: {profile}. Defaulting to WebGL settings.");
            }
        }
    }

    /// <summary>
    /// Provides utility methods for parsing command-line arguments.
    /// </summary>
    public static class CommandLineArgs
    {
        /// <summary>
        /// Retrieves the value of a specified command-line argument.
        /// </summary>
        /// <param name="name">The name of the command-line argument to retrieve.</param>
        /// <returns>The value of the argument if found; otherwise, null.</returns>
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
