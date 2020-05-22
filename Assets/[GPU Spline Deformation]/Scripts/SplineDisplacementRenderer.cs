using UnityEngine;

namespace RoyTheunissen.GPUSplineDeformation
{
    /// <summary>
    /// Renders spline displacement to a texture.
    /// </summary>
    [ExecuteInEditMode]
    public sealed class SplineDisplacementRenderer : MonoBehaviour
    {
        private static readonly int DisplacementTextureProperty = Shader.PropertyToID("_DisplacementAlongSplineTex");

        [SerializeField] private Material material;
        [SerializeField] private BezierSpline spline;
        
        [Space]
        [SerializeField] private int width = 32;
        [SerializeField] private int height = 32;
        [SerializeField] private TextureFormat textureFormat = TextureFormat.RGBA32;
        [SerializeField] private bool linear = true;

        [ContextMenu("Generate")]
        private void Generate()
        {
            if (material == null || spline == null)
                return;
            
            Texture2D texture2D = new Texture2D(width, height, textureFormat, false, linear)
            {
                wrapModeU = spline.Loop ? TextureWrapMode.Repeat : TextureWrapMode.Clamp,
                wrapModeV = TextureWrapMode.Clamp,
            };

            int count = width * height;
            Color[] colors = new Color[count];
            for (int x = 0; x < width; x++)
            {
                float xNormalized = (float)x / (width - 1);

                Vector3 positionInterpolated = spline.GetPoint(xNormalized);
                Vector3 directionInterpolated = spline.GetDirection(xNormalized);
                Quaternion rotationInterpolated = Quaternion.LookRotation(
                    directionInterpolated, Vector3.Cross(directionInterpolated, Vector3.right));
                Vector3 scaleInterpolated = Vector3.one;
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

            texture2D.SetPixels(colors);
            
            texture2D.Apply();

            material.SetTexture(DisplacementTextureProperty, texture2D);
        }

        private void Update()
        { 
            Generate();
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
