using UnityEngine.ProBuilder;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.ProBuilder;

namespace Attrition.ProBuilderAdvancedToolsOverlay
{
    [Overlay(typeof(SceneView), "ProBuilder Advanced", defaultDockPosition = DockPosition.Top, defaultDockZone = DockZone.LeftToolbar)]
    public class ProBuilderSceneViewOverlayUIToolkit : Overlay, ITransientOverlay
    {
        private const string EditorPath = "Assets/Editor/ProBuilderAdvancedToolsOverlay";
        private const string VisualTreeAssetFilename = "ProBuilderAdvancedToolsOverlay.uxml";
        private const string StyleSheetAssetFileName = "ProBuilderAdvancedToolsOverlay.uss";

        private SelectMode previousSelectionMode = SelectMode.None;

        public bool visible => this.IsProBuilderObjectSelected();

        private VisualElement root;
        private Foldout vertexModeContainer;
        private Foldout edgeModeContainer;
        private Foldout faceModeContainer;
        private Foldout selectionToolsContainer;
        private Foldout objectTransformationToolsContainer;
        private Foldout objectConversionToolsContainer;
        private Foldout subdivisionToolsContainer;
        private Foldout toggleToolsContainer;

        public override VisualElement CreatePanelContent()
        {
            var visualTreeAssetPath = $"{EditorPath}/{VisualTreeAssetFilename}";
            var styleSheetAssetPath = $"{EditorPath}/{StyleSheetAssetFileName}";

            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(visualTreeAssetPath);
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(styleSheetAssetPath);

            this.root = visualTree.CloneTree();
            this.root.styleSheets.Add(styleSheet);

            EditorApplication.update += this.OnEditorUpdated;

            this.vertexModeContainer = this.root.Q<Foldout>("vertex-mode-tools");
            this.edgeModeContainer = this.root.Q<Foldout>("edge-mode-tools");
            this.faceModeContainer = this.root.Q<Foldout>("face-mode-tools");
            this.selectionToolsContainer = this.root.Q<Foldout>("selection-tools");
            this.objectTransformationToolsContainer = this.root.Q<Foldout>("object-transformation-tools");
            this.objectConversionToolsContainer = this.root.Q<Foldout>("object-conversion-tools");
            this.subdivisionToolsContainer = this.root.Q<Foldout>("subdivision-tools");
            this.toggleToolsContainer = this.root.Q<Foldout>("toggle-tools");

            this.BindButtonCallbacks();
            this.UpdateToolDisplay();

            return this.root;
        }
        
        private bool IsProBuilderObjectSelected()
        {
            return Selection.activeGameObject?.GetComponent<ProBuilderMesh>() != null;
        }

        private void UpdateToolDisplay()
        {
            var currentMode = ProBuilderEditor.selectMode;

            // Vertex, Edge, and Face tools depend on selection mode
            this.SetVisibility(this.vertexModeContainer, currentMode == SelectMode.Vertex);
            this.SetVisibility(this.edgeModeContainer, currentMode == SelectMode.Edge);
            this.SetVisibility(this.faceModeContainer, currentMode == SelectMode.Face);

            // General groups are always visible
            this.SetVisibility(this.selectionToolsContainer, true);
            this.SetVisibility(this.objectTransformationToolsContainer, true);
            this.SetVisibility(this.objectConversionToolsContainer, true);
            this.SetVisibility(this.subdivisionToolsContainer, true);
            this.SetVisibility(this.toggleToolsContainer, true);
        }
        
        private void SetVisibility(VisualElement element, bool isVisible)
        {
            if (element == null) return;

            if (isVisible)
            {
                element.RemoveFromClassList("hidden");
            }
            else
            {
                element.AddToClassList("hidden");
            }
        }

        private void BindButtonCallbacks()
        {
            // Vertex Mode Tools
            this.root.Q<Button>("vertex-grow-selection-button").clicked += this.OnGrowSelectionButtonClicked;
            this.root.Q<Button>("vertex-shrink-selection-button").clicked += this.OnShrinkSelectionButtonClicked;
            this.root.Q<Button>("vertex-select-loop-button").clicked += this.OnSelectLoopButtonClicked;
            this.root.Q<Button>("vertex-select-ring-button").clicked += this.OnSelectRingButtonClicked;
            this.root.Q<Button>("vertex-collapse-vertices-button").clicked += this.OnCollapseVerticesButtonClicked;
            this.root.Q<Button>("vertex-split-vertices-button").clicked += this.OnSplitVerticesButtonClicked;
            this.root.Q<Button>("vertex-weld-vertices-button").clicked += this.OnWeldVerticesButtonClicked;
            this.root.Q<Button>("vertex-smart-connect-button").clicked += this.OnSmartConnectButtonClicked;

            // Edge Mode Tools
            this.root.Q<Button>("edge-grow-selection-button").clicked += this.OnGrowSelectionButtonClicked;
            this.root.Q<Button>("edge-shrink-selection-button").clicked += this.OnShrinkSelectionButtonClicked;
            this.root.Q<Button>("edge-select-loop-button").clicked += this.OnSelectLoopButtonClicked;
            this.root.Q<Button>("edge-select-ring-button").clicked += this.OnSelectRingButtonClicked;
            this.root.Q<Button>("edge-select-hole-button").clicked += this.OnSelectHoleButtonClicked;
            this.root.Q<Button>("edge-bevel-edges-button").clicked += this.OnBevelEdgesButtonClicked;
            this.root.Q<Button>("edge-bridge-edges-button").clicked += this.OnBridgeEdgesButtonClicked;
            this.root.Q<Button>("edge-insert-edge-loop-button").clicked += this.OnInsertEdgeLoopButtonClicked;
            this.root.Q<Button>("edge-smart-subdivide-button").clicked += this.OnSmartSubdivideButtonClicked;
            this.root.Q<Button>("edge-offset-elements-button").clicked += this.OnOffsetElementsButtonClicked;

            // Face Mode Tools
            this.root.Q<Button>("face-grow-selection-button").clicked += this.OnGrowSelectionButtonClicked;
            this.root.Q<Button>("face-shrink-selection-button").clicked += this.OnShrinkSelectionButtonClicked;
            this.root.Q<Button>("face-select-loop-button").clicked += this.OnSelectLoopButtonClicked;
            this.root.Q<Button>("face-select-ring-button").clicked += this.OnSelectRingButtonClicked;
            this.root.Q<Button>("face-select-hole-button").clicked += this.OnSelectHoleButtonClicked;
            this.root.Q<Button>("face-select-material-button").clicked += this.OnSelectMaterialButtonClicked;
            this.root.Q<Button>("face-select-smoothing-group-button").clicked += this.OnSelectSmoothingGroupButtonClicked;
            this.root.Q<Button>("face-select-vertex-color-button").clicked += this.OnSelectVertexColorButtonClicked;
            this.root.Q<Button>("face-center-pivot-button").clicked += this.OnCenterPivotButtonClicked;
            this.root.Q<Button>("face-delete-faces-button").clicked += this.OnDeleteFacesButtonClicked;
            this.root.Q<Button>("face-flip-face-normals-button").clicked += this.OnFlipFaceNormalsButtonClicked;
            this.root.Q<Button>("face-freeze-transform-button").clicked += this.OnFreezeTransformButtonClicked;
            this.root.Q<Button>("face-extrude-button").clicked += this.OnExtrudeButtonClicked;
            this.root.Q<Button>("face-merge-faces-button").clicked += this.OnMergeFacesButtonClicked;
            this.root.Q<Button>("face-detach-faces-button").clicked += this.OnDetachFacesButtonClicked;
            this.root.Q<Button>("face-duplicate-faces-button").clicked += this.OnDuplicateFacesButtonClicked;
            this.root.Q<Button>("face-triangulate-faces-button").clicked += this.OnTriangulateFacesButtonClicked;

            // General Tools
            this.root.Q<Button>("general-probuilderize-button").clicked += this.OnProBuilderizeButtonClicked;
            this.root.Q<Button>("general-set-collider-button").clicked += this.OnSetColliderButtonClicked;
            this.root.Q<Button>("general-set-trigger-button").clicked += this.OnSetTriggerButtonClicked;
            this.root.Q<Button>("general-conform-object-normals-button").clicked += this.OnConformObjectNormalsButtonClicked;
            this.root.Q<Button>("general-flip-object-normals-button").clicked += this.OnFlipObjectNormalsButtonClicked;
            this.root.Q<Button>("general-subdivide-object-button").clicked += this.OnSubdivideObjectButtonClicked;
            this.root.Q<Button>("general-triangulate-object-button").clicked += this.OnTriangulateObjectButtonClicked;
            this.root.Q<Button>("general-toggle-drag-rect-mode-button").clicked += this.OnToggleDragRectModeButtonClicked;
            this.root.Q<Button>("general-toggle-handle-orientation-button").clicked += this.OnToggleHandleOrientationButtonClicked;
            this.root.Q<Button>("general-toggle-select-back-faces-button").clicked += this.OnToggleSelectBackFacesButtonClicked;
            this.root.Q<Button>("general-toggle-x-ray-button").clicked += this.OnToggleXRayButtonClicked;
            this.root.Q<Button>("general-mirror-objects-button").clicked += this.OnMirrorObjectsButtonClicked;
        }
        
        private void OnEditorUpdated()
        {
            var currentMode = ProBuilderEditor.selectMode;

            if (this.previousSelectionMode != currentMode)
            {
                this.previousSelectionMode = currentMode;
                this.UpdateToolDisplay();
            }
        }

        // Button Callback Implementations
        private void OnGrowSelectionButtonClicked()
        {
            EditorApplication.ExecuteMenuItem("Tools/ProBuilder/Selection/Grow Selection");
        }

        private void OnShrinkSelectionButtonClicked()
        {
            EditorApplication.ExecuteMenuItem("Tools/ProBuilder/Selection/Shrink Selection");
        }

        private void OnSelectHoleButtonClicked()
        {
            EditorApplication.ExecuteMenuItem("Tools/ProBuilder/Selection/Select Hole");
        }

        private void OnSelectLoopButtonClicked()
        {
            EditorApplication.ExecuteMenuItem("Tools/ProBuilder/Selection/Select Loop");
        }

        private void OnSelectRingButtonClicked()
        {
            EditorApplication.ExecuteMenuItem("Tools/ProBuilder/Selection/Select Ring");
        }

        private void OnSelectMaterialButtonClicked()
        {
            EditorApplication.ExecuteMenuItem("Tools/ProBuilder/Selection/Select Material");
        }

        private void OnSelectSmoothingGroupButtonClicked()
        {
            EditorApplication.ExecuteMenuItem("Tools/ProBuilder/Selection/Select Smoothing Group");
        }

        private void OnSelectVertexColorButtonClicked()
        {
            EditorApplication.ExecuteMenuItem("Tools/ProBuilder/Selection/Select Vertex Color");
        }

        private void OnCenterPivotButtonClicked()
        {
            EditorApplication.ExecuteMenuItem("Tools/ProBuilder/Object/Center Pivot");
        }

        private void OnConformObjectNormalsButtonClicked()
        {
            EditorApplication.ExecuteMenuItem("Tools/ProBuilder/Object/Conform Object Normals");
        }

        private void OnFlipObjectNormalsButtonClicked()
        {
            EditorApplication.ExecuteMenuItem("Tools/ProBuilder/Object/Flip Object Normals");
        }

        private void OnFreezeTransformButtonClicked()
        {
            EditorApplication.ExecuteMenuItem("Tools/ProBuilder/Object/Freeze Transform");
        }

        private void OnMirrorObjectsButtonClicked()
        {
            EditorApplication.ExecuteMenuItem("Tools/ProBuilder/Object/Mirror Objects");
        }

        private void OnProBuilderizeButtonClicked()
        {
            EditorApplication.ExecuteMenuItem("Tools/ProBuilder/Object/ProBuilderize");
        }

        private void OnSetColliderButtonClicked()
        {
            EditorApplication.ExecuteMenuItem("Tools/ProBuilder/Object/Set Collider");
        }

        private void OnSetTriggerButtonClicked()
        {
            EditorApplication.ExecuteMenuItem("Tools/ProBuilder/Object/Set Trigger");
        }

        private void OnSubdivideObjectButtonClicked()
        {
            EditorApplication.ExecuteMenuItem("Tools/ProBuilder/Object/Subdivide Object");
        }

        private void OnTriangulateObjectButtonClicked()
        {
            EditorApplication.ExecuteMenuItem("Tools/ProBuilder/Object/Triangulate Object");
        }

        private void OnToggleDragRectModeButtonClicked()
        {
            EditorApplication.ExecuteMenuItem("Tools/ProBuilder/Interaction/Toggle Drag Rect Mode");
        }

        private void OnToggleHandleOrientationButtonClicked()
        {
            EditorApplication.ExecuteMenuItem("Tools/ProBuilder/Interaction/Toggle Handle Orientation");
        }

        private void OnToggleSelectBackFacesButtonClicked()
        {
            EditorApplication.ExecuteMenuItem("Tools/ProBuilder/Interaction/Toggle Select Back Faces");
        }

        private void OnToggleXRayButtonClicked()
        {
            EditorApplication.ExecuteMenuItem("Tools/ProBuilder/Interaction/Toggle X Ray");
        }
        
        private void OnCollapseVerticesButtonClicked()
        {
            EditorApplication.ExecuteMenuItem("Tools/ProBuilder/Geometry/Collapse Vertices");
        }

        private void OnSplitVerticesButtonClicked()
        {
            EditorApplication.ExecuteMenuItem("Tools/ProBuilder/Geometry/Split Vertices");
        }

        private void OnWeldVerticesButtonClicked()
        {
            EditorApplication.ExecuteMenuItem("Tools/ProBuilder/Geometry/Weld Vertices");
        }
        
        private void OnSmartConnectButtonClicked()
        {
            EditorApplication.ExecuteMenuItem("Tools/ProBuilder/Geometry/Smart Connect");
        }

        private void OnBevelEdgesButtonClicked()
        {
            EditorApplication.ExecuteMenuItem("Tools/ProBuilder/Geometry/Bevel Edges");
        }

        private void OnBridgeEdgesButtonClicked()
        {
            EditorApplication.ExecuteMenuItem("Tools/ProBuilder/Geometry/Bridge Edges");
        }

        private void OnInsertEdgeLoopButtonClicked()
        {
            EditorApplication.ExecuteMenuItem("Tools/ProBuilder/Geometry/Insert Edge Loop");
        }

        private void OnSmartSubdivideButtonClicked()
        {
            EditorApplication.ExecuteMenuItem("Tools/ProBuilder/Geometry/Subdivide Object");
        }

        private void OnOffsetElementsButtonClicked()
        {
            EditorApplication.ExecuteMenuItem("Tools/ProBuilder/Geometry/Offset Elements");
        }
        
        private void OnDeleteFacesButtonClicked()
        {
            EditorApplication.ExecuteMenuItem("Tools/ProBuilder/Geometry/Delete Faces");
        }

        private void OnDetachFacesButtonClicked()
        {
            EditorApplication.ExecuteMenuItem("Tools/ProBuilder/Geometry/Detach Faces");
        }

        private void OnExtrudeButtonClicked()
        {
            EditorApplication.ExecuteMenuItem("Tools/ProBuilder/Geometry/Extrude");
        }

        private void OnDuplicateFacesButtonClicked()
        {
            EditorApplication.ExecuteMenuItem("Tools/ProBuilder/Geometry/Duplicate Faces");
        }

        private void OnMergeFacesButtonClicked()
        {
            EditorApplication.ExecuteMenuItem("Tools/ProBuilder/Geometry/Merge Faces");
        }

        private void OnFlipFaceNormalsButtonClicked()
        {
            EditorApplication.ExecuteMenuItem("Tools/ProBuilder/Geometry/Flip Face Normals");
        }

        private void OnTriangulateFacesButtonClicked()
        {
            EditorApplication.ExecuteMenuItem("Tools/ProBuilder/Geometry/Triangulate Object");
        }
    }
}
