using System;
using UnityEngine;

namespace RoyTheunissen.GPUSplineDeformation
{
    /// <summary>
    /// Renders spline displacement to a texture.
    /// </summary>
    [ExecuteInEditMode]
    public sealed class DisplacementTextureRenderer : MonoBehaviour
    {
        public enum TextureMode
        {
            Dynamic = 0,
            Asset = 1,
        }
        
        private static readonly int DisplacementTextureProperty = Shader.PropertyToID("_DisplacementAlongSplineTex");

        [Tooltip("Where the displacement is read from, for instance: a spline. Any linear sequence of matrices will do.")]
        [SerializeField] private MonoBehaviour displacementProvider;
        
        [Tooltip("If specified, material whose texture is updated so you can see your result conveniently.")]
        [SerializeField] private Material material;
        
        [Space]
        [SerializeField] private int width = 32;
        [SerializeField] private int height = 8;
        
        [Space]
        [SerializeField, Tooltip("Whether to generate an asset or not.")] private TextureMode mode;
        public TextureMode Mode => mode;
        
        [SerializeField, HideInInspector] private Texture2D textureAsset;

        [NonSerialized] private Texture2D textureDynamic;

        [ContextMenu("Generate")]
        public Texture2D Render()
        {
            if (displacementProvider == null)
                return null;

            IDisplacementProvider spline = displacementProvider as IDisplacementProvider;
            if (spline == null)
                return null;
            
            if (textureDynamic != null)
                DestroyImmediate(textureDynamic);
            
            // TODO: This is leaking, would be more efficient to only create a new texture if needed.
            textureDynamic = new Texture2D(width, height, TextureFormat.RGBAFloat, false, true)
            {
                wrapModeU = spline.IsLooping ? TextureWrapMode.Repeat : TextureWrapMode.Clamp,
                wrapModeV = TextureWrapMode.Clamp,
            };

            int count = width * height;
            Color[] colors = new Color[count];
            for (int x = 0; x < width; x++)
            {
                float xNormalized = (float)x / (width - 1);

                Vector3 positionInterpolated = spline.GetPositionAt(xNormalized);
                Quaternion rotationInterpolated = spline.GetRotationAt(xNormalized);
                Vector3 scaleInterpolated = spline.GetScaleAt(xNormalized);
                Matrix4x4 matrix = transform.worldToLocalMatrix * Matrix4x4.TRS(
                                       positionInterpolated, rotationInterpolated, scaleInterpolated);

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

            textureDynamic.SetPixels(colors);
            textureDynamic.Apply();

            return textureDynamic;
        }

        private void Update()
        {
            if (mode == TextureMode.Asset)
            {
                if (material != null)
                    material.SetTexture(DisplacementTextureProperty, textureAsset);
            }
            else
            {
                Render();
                if (material != null)
                    material.SetTexture(DisplacementTextureProperty, textureDynamic);
            }
        }

        private void OnValidate()
        {
            width = Mathf.Max(width, 2);
            height = Mathf.Max(height, 4);
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
