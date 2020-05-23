using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace RoyTheunissen.GPUSplineDeformation
{
    /// <summary>
    /// Responsible for the editor interface, mostly for managing the texture asset workflow.
    /// </summary>
    [CustomEditor(typeof(SplineDisplacementRenderer))]
    public class SplineDisplacementRendererEditor : Editor
    {
        private SerializedProperty textureAsset;

        private void OnEnable()
        {
            textureAsset = serializedObject.FindProperty("textureAsset");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            serializedObject.Update();

            SplineDisplacementRenderer splineDisplacementRenderer = target as SplineDisplacementRenderer;

            if (splineDisplacementRenderer.Mode == SplineDisplacementRenderer.TextureMode.Asset)
            {
                EditorGUILayout.PropertyField(textureAsset);
                if (textureAsset.objectReferenceValue == null)
                {
                    if (GUILayout.Button("Save As.."))
                    {
                        string projectPath = EditorUtility.SaveFilePanelInProject(
                            "Save spline displacement texture", "Spline Displacement Texture", "exr",
                            "Save the spline displacement to an asset so it doesn't have to be recomputed at run-time.");
                        RenderToFile(projectPath);
                    }
                }
                else
                {
                    if (GUILayout.Button("Update"))
                    {
                        string projectPath = AssetDatabase.GetAssetPath(textureAsset.objectReferenceValue);
                        RenderToFile(projectPath);
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void RenderToFile(string projectPath)
        {
            if (string.IsNullOrEmpty(projectPath))
                return;

            SplineDisplacementRenderer splineDisplacementRenderer = target as SplineDisplacementRenderer;
            
            string absolutePath = Path.Combine(Directory.GetCurrentDirectory(), projectPath);
            Texture2D texture2dOriginal = splineDisplacementRenderer.Render();
            byte[] bytes = texture2dOriginal.EncodeToEXR();
            File.WriteAllBytes(absolutePath, bytes);
            AssetDatabase.Refresh();

            TextureImporter textureImporter = (TextureImporter)AssetImporter.GetAtPath(projectPath);
            textureImporter.textureCompression = TextureImporterCompression.Uncompressed;
            textureImporter.sRGBTexture = false;
            textureImporter.mipmapEnabled = false;
            textureImporter.wrapModeU = texture2dOriginal.wrapModeU;
            textureImporter.wrapModeV = texture2dOriginal.wrapModeV;
            textureImporter.SaveAndReimport();
            AssetDatabase.SaveAssets();

            Texture2D texture2dNew = AssetDatabase.LoadAssetAtPath<Texture2D>(projectPath);
            textureAsset.objectReferenceValue = texture2dNew;
        }
    }
}
