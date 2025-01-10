using Attrition.Common.Events.SerializedEvents;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace Attrition.Runtime.Common.SerializedEvents.Editor
{
    /// <summary>
    /// Custom property drawer for <see cref="SerializedEvent"/>.
    /// </summary>
    [CustomPropertyDrawer(typeof(SerializedEvent))]
    public class SerializedEventPropertyDrawer : PropertyDrawer
    {
        /// <summary>
        /// Creates the GUI for the SerializedEvent property.
        /// </summary>
        /// <param name="property">The serialized property being drawn.</param>
        /// <returns>A configured <see cref="VisualElement"/>.</returns>
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            return SerializedEventEditorHelpers.CreateRoot(property);
        }
    }

    /// <summary>
    /// Abstract base class for property drawers of generic <see cref="SerializedEvent"/> types.
    /// </summary>
    /// <typeparam name="T">The type of the SerializedEvent.</typeparam>
    public abstract class SerializedEventPropertyDrawer<T> : PropertyDrawer
    {
        /// <summary>
        /// Creates the GUI for the SerializedEvent property of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="property">The serialized property being drawn.</param>
        /// <returns>A configured <see cref="VisualElement"/>.</returns>
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = SerializedEventEditorHelpers.CreateRoot(property);
            root = this.ConfigureVisualElement(root, property);
            return root;
        }

        /// <summary>
        /// Configures the root <see cref="VisualElement"/> for the property drawer.
        /// </summary>
        /// <param name="root">The root visual element.</param>
        /// <param name="property">The serialized property being drawn.</param>
        /// <returns>A configured <see cref="VisualElement"/>.</returns>
        protected virtual VisualElement ConfigureVisualElement(VisualElement root, SerializedProperty property)
        {
            return root;
        }
    }

    /// <summary>
    /// Helper class for creating and configuring property drawer elements for <see cref="SerializedEvent"/>.
    /// </summary>
    public static class SerializedEventEditorHelpers
    {
        /// <summary>
        /// Path to the folder where this editor is located.
        /// </summary>
        private const string EditorPath = "Assets/Runtime/Common/SerializedEvents/Editor";

        /// <summary>
        /// Filename of the UXML template asset.
        /// </summary>
        private const string VisualTreeAssetFilename = "SerializedEventPropertyDrawer.uxml";

        /// <summary>
        /// Filename of the USS stylesheet asset.
        /// </summary>
        private const string StyleSheetAssetFilename = "SerializedEventPropertyDrawer.uss";

        /// <summary>
        /// Creates the root <see cref="VisualElement"/> for a SerializedEvent property drawer.
        /// </summary>
        /// <param name="property">The serialized property being drawn.</param>
        /// <returns>A configured <see cref="VisualElement"/>.</returns>
        public static VisualElement CreateRoot(SerializedProperty property)
        {
            var visualTreeAssetFilePath = $"{EditorPath}/{VisualTreeAssetFilename}";
            var styleSheetAssetFilePath = $"{EditorPath}/{StyleSheetAssetFilename}";

            // Load UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(visualTreeAssetFilePath);
            var root = visualTree.CloneTree();

            // Load USS
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(styleSheetAssetFilePath);
            root.styleSheets.Add(styleSheet);
            
            // Configure PropertyField
            var propertyField = root.Q<PropertyField>();
            propertyField.label = property.displayName;

            return root;
        }

        /// <summary>
        /// Configures a <see cref="Label"/> element within the root <see cref="VisualElement"/>.
        /// </summary>
        /// <param name="root">The root visual element.</param>
        /// <param name="labelName">The name of the label element to configure.</param>
        /// <param name="text">The text to set for the label.</param>
        public static void ConfigureLabel(VisualElement root, string labelName, string text)
        {
            var label = root.Q<Label>(labelName);
            if (label != null)
            {
                label.text = text;
            }
        }
    }
    
    #region C# Primitive Types
    [CustomPropertyDrawer(typeof(SerializedEvent<string>))]
    public class SerializedEventStringPropertyDrawer : SerializedEventPropertyDrawer<string> { }

    [CustomPropertyDrawer(typeof(SerializedEvent<int>))]
    public class SerializedEventIntPropertyDrawer : SerializedEventPropertyDrawer<int> { }

    [CustomPropertyDrawer(typeof(SerializedEvent<float>))]
    public class SerializedEventFloatPropertyDrawer : SerializedEventPropertyDrawer<float> { }

    [CustomPropertyDrawer(typeof(SerializedEvent<bool>))]
    public class SerializedEventBoolPropertyDrawer : SerializedEventPropertyDrawer<bool> { }

    [CustomPropertyDrawer(typeof(SerializedEvent<double>))]
    public class SerializedEventDoublePropertyDrawer : SerializedEventPropertyDrawer<double> { }
    #endregion
    
    #region Unity Entity Component Types
    [CustomPropertyDrawer(typeof(SerializedEvent<GameObject>))]
    public class SerializedEventGameObjectPropertyDrawer : SerializedEventPropertyDrawer<GameObject> { }

    [CustomPropertyDrawer(typeof(SerializedEvent<Transform>))]
    public class SerializedEventTransformPropertyDrawer : SerializedEventPropertyDrawer<Transform> { }
    
        [CustomPropertyDrawer(typeof(SerializedEvent<Rigidbody>))]
    public class SerializedEventRigidbodyPropertyDrawer : SerializedEventPropertyDrawer<Rigidbody> { }

    [CustomPropertyDrawer(typeof(SerializedEvent<Rigidbody2D>))]
    public class SerializedEventRigidbody2DPropertyDrawer : SerializedEventPropertyDrawer<Rigidbody2D> { }

    [CustomPropertyDrawer(typeof(SerializedEvent<Collider>))]
    public class SerializedEventColliderPropertyDrawer : SerializedEventPropertyDrawer<Collider> { }

    [CustomPropertyDrawer(typeof(SerializedEvent<Collider2D>))]
    public class SerializedEventCollider2DPropertyDrawer : SerializedEventPropertyDrawer<Collider2D> { }

    [CustomPropertyDrawer(typeof(SerializedEvent<MeshRenderer>))]
    public class SerializedEventMeshRendererPropertyDrawer : SerializedEventPropertyDrawer<MeshRenderer> { }

    [CustomPropertyDrawer(typeof(SerializedEvent<SkinnedMeshRenderer>))]
    public class SerializedEventSkinnedMeshRendererPropertyDrawer : SerializedEventPropertyDrawer<SkinnedMeshRenderer> { }

    [CustomPropertyDrawer(typeof(SerializedEvent<SpriteRenderer>))]
    public class SerializedEventSpriteRendererPropertyDrawer : SerializedEventPropertyDrawer<SpriteRenderer> { }

    [CustomPropertyDrawer(typeof(SerializedEvent<Light>))]
    public class SerializedEventLightPropertyDrawer : SerializedEventPropertyDrawer<Light> { }

    [CustomPropertyDrawer(typeof(SerializedEvent<Camera>))]
    public class SerializedEventCameraPropertyDrawer : SerializedEventPropertyDrawer<Camera> { }

    [CustomPropertyDrawer(typeof(SerializedEvent<Animator>))]
    public class SerializedEventAnimatorPropertyDrawer : SerializedEventPropertyDrawer<Animator> { }

    [CustomPropertyDrawer(typeof(SerializedEvent<AudioSource>))]
    public class SerializedEventAudioSourcePropertyDrawer : SerializedEventPropertyDrawer<AudioSource> { }

    [CustomPropertyDrawer(typeof(SerializedEvent<Canvas>))]
    public class SerializedEventCanvasPropertyDrawer : SerializedEventPropertyDrawer<Canvas> { }

    [CustomPropertyDrawer(typeof(SerializedEvent<CanvasRenderer>))]
    public class SerializedEventCanvasRendererPropertyDrawer : SerializedEventPropertyDrawer<CanvasRenderer> { }

    [CustomPropertyDrawer(typeof(SerializedEvent<EventSystem>))]
    public class SerializedEventEventSystemPropertyDrawer : SerializedEventPropertyDrawer<EventSystem> { }
    #endregion
    
    #region Unity Data Types
    [CustomPropertyDrawer(typeof(SerializedEvent<Vector2>))]
    public class SerializedEventVector2PropertyDrawer : SerializedEventPropertyDrawer<Vector2> { }
    
    [CustomPropertyDrawer(typeof(SerializedEvent<Vector2Int>))]
    public class SerializedEventVector2IntPropertyDrawer : SerializedEventPropertyDrawer<Vector2Int> { }

    [CustomPropertyDrawer(typeof(SerializedEvent<Vector3>))]
    public class SerializedEventVector3PropertyDrawer : SerializedEventPropertyDrawer<Vector3> { }
    
    [CustomPropertyDrawer(typeof(SerializedEvent<Vector3Int>))]
    public class SerializedEventVector3IntPropertyDrawer : SerializedEventPropertyDrawer<Vector3Int> { }

    [CustomPropertyDrawer(typeof(SerializedEvent<Color>))]
    public class SerializedEventColorPropertyDrawer : SerializedEventPropertyDrawer<Color> { }

    [CustomPropertyDrawer(typeof(SerializedEvent<Quaternion>))]
    public class SerializedEventQuaternionPropertyDrawer : SerializedEventPropertyDrawer<Quaternion> { }

    [CustomPropertyDrawer(typeof(SerializedEvent<Rect>))]
    public class SerializedEventRectPropertyDrawer : SerializedEventPropertyDrawer<Rect> { }
    #endregion
    
    #region Asset Types
    [CustomPropertyDrawer(typeof(SerializedEvent<Material>))]
    public class SerializedEventMaterialPropertyDrawer : SerializedEventPropertyDrawer<Material> { }

    [CustomPropertyDrawer(typeof(SerializedEvent<Texture>))]
    public class SerializedEventTexturePropertyDrawer : SerializedEventPropertyDrawer<Texture> { }

    [CustomPropertyDrawer(typeof(SerializedEvent<AudioClip>))]
    public class SerializedEventAudioClipPropertyDrawer : SerializedEventPropertyDrawer<AudioClip> { }

    [CustomPropertyDrawer(typeof(SerializedEvent<AnimationClip>))]
    public class SerializedEventAnimationClipPropertyDrawer : SerializedEventPropertyDrawer<AnimationClip> { }

    [CustomPropertyDrawer(typeof(SerializedEvent<Shader>))]
    public class SerializedEventShaderPropertyDrawer : SerializedEventPropertyDrawer<Shader> { }

    [CustomPropertyDrawer(typeof(SerializedEvent<Sprite>))]
    public class SerializedEventSpritePropertyDrawer : SerializedEventPropertyDrawer<Sprite> { }

    [CustomPropertyDrawer(typeof(SerializedEvent<Mesh>))]
    public class SerializedEventMeshPropertyDrawer : SerializedEventPropertyDrawer<Mesh> { }

    [CustomPropertyDrawer(typeof(SerializedEvent<Font>))]
    public class SerializedEventFontPropertyDrawer : SerializedEventPropertyDrawer<Font> { }
    #endregion
}
