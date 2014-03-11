using System;
using System.Runtime.InteropServices;
using OpenTK;

namespace univ
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct BaseLight
    {
        public Vector3 Color { get { return this.color; } }
        public float Intensity { get { return this.intensity; } }
            
        private Vector3 color;
        private float intensity;
        /* 4 floats - no padding */
        
        public BaseLight(Vector3 color, float intensity)
        {
            this.color = color;
            this.intensity = intensity;
        }
    }
}

