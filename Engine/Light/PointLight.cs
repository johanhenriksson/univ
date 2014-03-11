using System;
using System.Runtime.InteropServices;
using OpenTK;

namespace univ
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct PointLight
    {
        private BaseLight light;            /* 4 floats */
        private Attenuation attenuation;    /* 4 floats */
        private Vector3 position;           /* 3 floats */
        private float _pad1;
        /* 12 floats */
    }
    
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Attenuation
    {
        float constant;
        float linear;
        float square;
        private float _pad1;
        /* 4 floats */
    }
}

