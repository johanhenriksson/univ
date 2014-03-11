using System;
using System.Runtime.InteropServices;
using OpenTK;

namespace univ
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct PointLight
    {
        public BaseLight Light { get { return this.light; } }
        public Attenuation Attenuation { get { return this.attenuation; } }
        public Vector3 Position { get { return this.position; } }
        
        private BaseLight light;            /* 4 floats */
        private Attenuation attenuation;    /* 4 floats */
        private Vector3 position;           /* 3 floats */
        private float _pad1;
        /* 12 floats */
        
        public PointLight(BaseLight light, Attenuation attenuation, Vector3 position)
        {
            this.light = light;
            this.attenuation = attenuation;
            this.position = position;
            this._pad1 = 0.0f;
        }
    }
    
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Attenuation
    {
        public float Constant { get { return this.constant; } }
        public float Linear { get { return this.linear; } }
        public float Square { get { return this.square; } }
        
        float constant;
        float linear;
        float square;
        private float _pad1;
        /* 4 floats */
        
        public Attenuation(float constant, float linear, float square)
        {
            this.constant = constant;
            this.linear = linear;
            this.square = square;
            this._pad1 = 0.0f;
        }
    }
}

