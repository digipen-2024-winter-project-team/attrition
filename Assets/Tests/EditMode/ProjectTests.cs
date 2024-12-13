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
        
        // [Test]
        // public void GivenProjectAssets_WhenCheckedForDuplicateNames_ThenNoAssetsHaveSameTypeAndNameInDifferentLocations()
        // {
        //     /* ARRANGE */
        //     var excludedExtensions = new HashSet<string>
        //     {
        //         ".asmref",
        //     };
        //
        //     var allAssets = AssetDatabase.GetAllAssetPaths()
        //         .Where(path => path.StartsWith("Assets/")) // Exclude Packages and other folders
        //         .Where(path => !string.IsNullOrEmpty(System.IO.Path.GetExtension(path))) // Ignore folders
        //         .Where(path => !excludedExtensions.Contains(System.IO.Path.GetExtension(path).ToLower())) // Exclude specific extensions
        //         .Select(path => new
        //         {
        //             Name = System.IO.Path.GetFileNameWithoutExtension(path),
        //             Extension = System.IO.Path.GetExtension(path),
        //             Path = path
        //         })
        //         .GroupBy(asset => new { asset.Name, asset.Extension }) // Group by name and type
        //         .ToList();
        //
        //     /* ACT */
        //     var duplicates = allAssets
        //         .Where(group => group.Count() > 1) // Find groups with duplicates
        //         .ToDictionary(group => group.Key, group => group.ToList());
        //
        //     var hasDuplicates = duplicates.Any();
        //
        //     /* ASSERT */
        //     if (hasDuplicates)
        //     {
        //         var stringBuilder = new StringBuilder();
        //
        //         stringBuilder.AppendLine($"{duplicates.Count} sets of duplicate assets were found:");
        //         stringBuilder.AppendLine();
        //
        //         foreach (var duplicateGroup in duplicates)
        //         {
        //             stringBuilder.AppendLine(
        //                 $"Duplicate Name: {duplicateGroup.Key.Name}, Type: {duplicateGroup.Key.Extension}");
        //             foreach (var asset in duplicateGroup.Value)
        //             {
        //                 stringBuilder.AppendLine($"- {asset.Path}");
        //             }
        //
        //             stringBuilder.AppendLine();
        //         }
        //
        //         Debug.LogError(stringBuilder.ToString());
        //     }
        //
        //     Assert.False(hasDuplicates, $"{duplicates.Count} sets of duplicate assets were found.");
        // }
        
        /*[Test]
        public void GivenProjectFiles_WhenCheckedForNamingConvention_ThenAllFilesFollowPascalCaseWithExclusions()
        {
            /* ARRANGE #1#
            
            const string namingPattern = @"^([A-Z][a-z0-9]*)+([A-Z]{2,}[a-z0-9]*)*$";
            var regex = new System.Text.RegularExpressions.Regex(namingPattern);
            
            var excludedExtensions = new HashSet<string>
            {
                ".asmdef",
                ".asmref",
            };
            
            var excludedPaths = new List<string>
            {
                "Assets/ThirdParty",
                "Assets/Plugins",
                "Assets/Runtime/Settings/Build Profiles",
            };
            
            var allFiles = AssetDatabase.GetAllAssetPaths()
                .Where(path => path.StartsWith("Assets/")) // Exclude non-Assets paths
                .Where(path => !string.IsNullOrEmpty(System.IO.Path.GetExtension(path))) // Ignore folders
                .Where(path => !excludedExtensions.Contains(System.IO.Path.GetExtension(path).ToLower())) // Exclude extensions
                .Where(path => !excludedPaths.Any(excludedPath => path.StartsWith(excludedPath))) // Exclude specific paths
                .ToList();

            /* ACT #1#
            
            var nonCompliantFiles = allFiles
                .Where(filePath =>
                {
                    var fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);
                    return !regex.IsMatch(fileName);
                })
                .ToList();

            var hasNonCompliantFiles = nonCompliantFiles.Any();

            /* ASSERT #1#
            
            if (hasNonCompliantFiles)
            {
                var stringBuilder = new StringBuilder();
                
                stringBuilder.AppendLine($"{nonCompliantFiles.Count} files do not follow the PascalCase naming convention:");
                stringBuilder.AppendLine();

                foreach (var file in nonCompliantFiles)
                {
                    stringBuilder.AppendLine(file);
                }

                Debug.LogError(stringBuilder.ToString());
            }

            Assert.False(hasNonCompliantFiles, $"{nonCompliantFiles.Count} files do not follow the PascalCase naming convention.");
        }*/
    }
}
