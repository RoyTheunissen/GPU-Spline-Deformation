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

        [SerializeField] private Material material;
        
        [Space]
        [SerializeField] private int width = 2;
        [SerializeField] private int height = 2;
        [SerializeField] private TextureFormat textureFormat = TextureFormat.RGBA32;
        [SerializeField] private bool linear = true;
        [SerializeField] private TextureWrapMode wrapMode = TextureWrapMode.Clamp;

        [SerializeField] private Gradient gradient = new Gradient
            { colorKeys = new[] {new GradientColorKey(Color.black, 0), new GradientColorKey(Color.white, 1)} };
        
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
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = y * width + x;
                    colors[index] = gradient.Evaluate((float)x / (width - 1));
                }
            }

            texture2D.SetPixels(colors);
            
            texture2D.Apply();

            material.SetTexture(DisplacementTextureProperty, texture2D);
        }

        private void Update()
        {
            // Generate if there is no texture yet.
            if (material.GetTexture(DisplacementTextureProperty) == null)
                Generate();
        }

        private void OnValidate()
        {
            width = Mathf.Max(width, 2);
            height = Mathf.Max(height, 1);
            
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
