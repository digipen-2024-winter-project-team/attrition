using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Attrition
{
    [TestFixture]
    public class ProjectTests
    {
        [Test]
        public void GivenProjectRoot_WhenCheckedForLooseFiles_ThenNoLooseFilesAreFound()
        {
            /* ARRANGE */
            
            var allowedPaths = new HashSet<string>
            {
                "Assets/Editor",
                "Assets/Plugins",
                "Assets/Resources",
                "Assets/Runtime",
                "Assets/Tests",
                "Assets/TextMeshPro",
                "Assets/WebGLTemplates",
            };

            var allowedRootDirectories = new[] { "Assets" };

            // Collect all asset paths from specific root directories
            var allPaths = allowedRootDirectories
                .SelectMany(dir => AssetDatabase.FindAssets("", new[] { dir })
                    .Select(AssetDatabase.GUIDToAssetPath))
                .Distinct()
                .ToList();

            /* ACT */
            
            // Filter out files that do not start with any of the allowed paths
            var looseFiles = allPaths
                .Where(path => !allowedPaths.Any(path.StartsWith))
                .ToList();

            var hasAnyLooseFiles = looseFiles.Any();

            /* ASSERT */
            
            if (hasAnyLooseFiles)
            {
                var stringBuilder = new StringBuilder();

                // Log the number of loose files found
                stringBuilder.AppendLine($"{looseFiles.Count} loose files were found:");
                stringBuilder.AppendLine();

                // Log the paths of the loose files found
                foreach (var looseFile in looseFiles)
                {
                    stringBuilder.AppendLine(looseFile);
                }
                
                stringBuilder.AppendLine();

                // Log the allowed paths
                stringBuilder.AppendLine("All assets must be a child (at any depth) of one of the following paths:");
                foreach (var allowedPath in allowedPaths)
                {
                    stringBuilder.AppendLine(allowedPath);
                }

                Debug.LogError(stringBuilder.ToString());
            }

            Assert.False(hasAnyLooseFiles, $"{looseFiles.Count} loose files were found.");
        }
        
        [Test]
        public void GivenProjectScenes_WhenCheckedForLocation_ThenAllScenesAreInScenesDirectory()
        {
            /* ARRANGE */
            const string scenesDirectory = "Assets/Runtime/Scenes";
            var allScenes = AssetDatabase.FindAssets("t:Scene")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(scenePath => scenePath.StartsWith("Assets/"))
                .ToList();

            /* ACT */
            var misplacedScenes = allScenes
                .Where(scenePath => !scenePath.StartsWith(scenesDirectory))
                .ToList();

            var hasMisplacedScenes = misplacedScenes.Any();

            /* ASSERT */
            if (hasMisplacedScenes)
            {
                var stringBuilder = new StringBuilder();

                stringBuilder.AppendLine(
                    $"{misplacedScenes.Count} scenes are outside the '{scenesDirectory}' directory:");
                stringBuilder.AppendLine();
                
                foreach (var misplacedScene in misplacedScenes)
                {
                    stringBuilder.AppendLine(misplacedScene);
                }
                
                Debug.LogError(stringBuilder.ToString());
            }

            Assert.False(hasMisplacedScenes, $"{misplacedScenes.Count} scenes are outside the '{scenesDirectory}' directory.");
        }
    }
}
