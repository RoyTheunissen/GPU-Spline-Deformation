using UnityEngine;

namespace RoyTheunissen.GPUSplineDeformation
{
    /// <summary>
    /// Provides deformation for the identity matrix (no deformation).
    /// </summary>
    public sealed class IdentityProvider : MonoBehaviour, IDeformationProvider
    {
        public bool IsLooping => false;
        
        public Vector3 GetPositionAt(float fraction)
        {
            return Vector3.forward * fraction;
        }

        public Quaternion GetRotationAt(float fraction)
        {
            return Quaternion.identity;
        }

        public Vector3 GetScaleAt(float fraction)
        {
            return Vector3.one;
        }
    }
}
