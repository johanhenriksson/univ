using System;
using System.Runtime.InteropServices;
using OpenTK;

namespace univ
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct DirectionalLight
    {
        public BaseLight Light { get { return this.light; } }
        public Vector3  Direction { get { return this.direction; } }
        
        private BaseLight light;
        private Vector3 direction;
        private float _pad1;
        /* 8 floats */
        
        public DirectionalLight(BaseLight light, Vector3 direction)
        {
            this.light = light;
            this.direction = direction;
            this._pad1 = 0.0f;
        }
    }
}

