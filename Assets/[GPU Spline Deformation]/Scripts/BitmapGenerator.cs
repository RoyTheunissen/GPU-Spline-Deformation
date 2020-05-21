using System;
using UnityEngine;

namespace RoyTheunissen.GPUSplineDeformation
{
    /// <summary>
    /// 
    /// </summary>
    [ExecuteInEditMode]
    public sealed class BitmapGenerator : MonoBehaviour
    {
        private static readonly int DisplacementTextureProperty = Shader.PropertyToID("_DisplacementAlongSplineTex");
        private static readonly int TextMatrixProperty = Shader.PropertyToID("_TestMatrix");

        [SerializeField] private Material material;
        [SerializeField] private Transform testTransform;
        
        [Space]
        [SerializeField] private int width = 32;
        [SerializeField] private int height = 32;
        [SerializeField] private TextureFormat textureFormat = TextureFormat.RGBA32;
        [SerializeField] private bool linear = true;
        [SerializeField] private TextureWrapMode wrapMode = TextureWrapMode.Clamp;

        [SerializeField] private Gradient gradient = new Gradient
        {
            colorKeys = new[] {new GradientColorKey(Color.black, 0), new GradientColorKey(Color.white, 1)},
            alphaKeys = new []{new GradientAlphaKey(0, 0), new GradientAlphaKey(1, 1), }
        };
        
        [ContextMenu("Generate")]
        private void Generate()
        {
            if (material == null)
                return;
            
            Texture2D texture2D = new Texture2D(width, height, textureFormat, false, linear)
            {
                wrapMode = wrapMode,
            };

            int count = width * height;
            Color[] colors = new Color[count];
            Matrix4x4 from = Matrix4x4.identity;
            Matrix4x4 to = testTransform.localToWorldMatrix;
            for (int x = 0; x < width; x++)
            {
                float xNormalized = (float)x / (width - 1);

                Vector3 positionInterpolated = Vector3.Lerp(
                    from.MultiplyPoint(Vector3.zero), to.MultiplyPoint(Vector3.zero), xNormalized);
                Quaternion rotationInterpolated = Quaternion.Slerp(from.rotation, to.rotation, xNormalized);
                Vector3 scaleInterpolated = Vector3.Lerp(from.lossyScale, to.lossyScale, xNormalized);
                Matrix4x4 matrix = Matrix4x4.TRS(positionInterpolated, rotationInterpolated, scaleInterpolated);

                for (int y = 0; y < height; y++)
                {
                    int index = y * width + x;
                    
                    float yNormalized = (float)y / (height - 1);
                    float yValue = yNormalized * 3;
                    
                    int rowFrom = Mathf.FloorToInt(yValue);
                    int rowTo = Mathf.Min(rowFrom + 1, 3);
                    float rowFraction = Mathf.Round(yValue - rowFrom);

                    colors[index] = Vector4.Lerp(matrix.GetRow(rowFrom), matrix.GetRow(rowTo), rowFraction);
                }
            }

            texture2D.SetPixels(colors);
            
            texture2D.Apply();

            material.SetTexture(DisplacementTextureProperty, texture2D);
        }

        private void Update()
        {
            // Generate if there is no texture yet.
            //if (material.GetTexture(DisplacementTextureProperty) == null)
                Generate();
            
            if (testTransform != null)
                material.SetMatrix(TextMatrixProperty, testTransform.localToWorldMatrix);
        }

        private void OnValidate()
        {
            width = Mathf.Max(width, 2);
            height = Mathf.Max(height, 4);

            Generate();
        }

        private void Reset()
        {
            if (material == null)
            {
                Renderer renderer = GetComponentInChildren<Renderer>();
                if (renderer != null)
                    material = renderer.sharedMaterial;
            }
        }
    }
}
