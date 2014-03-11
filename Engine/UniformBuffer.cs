using System;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace univ
{
    public class UniformBuffer : GLBuffer
    {
        private static int nextUniformIndex = 0;
        
        protected Shader shader;
        protected int uniformIndex;
        
        public UniformBuffer(Shader shader)
            : base(BufferTarget.UniformBuffer, BufferUsageHint.DynamicDraw)
        {
            this.shader = shader;
            this.uniformIndex = nextUniformIndex++;
            
        }
                               
        public unsafe override void BufferData<T>(ref T[] data)
        {
            base.BufferData(ref data);
            GL.BindBufferRange(BufferRangeTarget.UniformBuffer,
                               uniformIndex, this.id, (IntPtr)0,
                               (IntPtr)(data.Length * sizeof(T)));
        }
    }
}

