using System;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using univ.Engine.Geometry;

namespace univ
{
    public class Axis : Component
    {
        Matrix4 mvp;
        Shader shader;
        VertexArray geometry;
        
        public Axis(Shader shader)
            : base()
        {
            this.shader = shader;
            this.geometry = new VertexArray(PrimitiveType.Lines);
            
            geometry.CreateBuffer("vertex").BufferData<Byte3>(ref vertexData);
            geometry.CreateBuffer("colors").BufferData<byte>(ref colorData);
            
            geometry.AddPointer("vertex", new VertexAttribute(shader, "vPosition", VertexAttribPointerType.UnsignedByte, 3, 0, false));
            geometry.AddPointer("colors", new VertexAttribute(shader, "vColor", VertexAttribPointerType.UnsignedByte, 4, 0, true));
        }
        
        public override void Draw(DrawEventArgs e)
        {
            /* Calculate Model/View/Projection matrix */
            Matrix4.Mult(ref this.modelMatrix, ref e.ModelMatrix, out mvp);
            Matrix4.Mult(ref mvp, ref e.Camera.ViewProjection, out mvp);
            
            shader.Use();
            shader.SetMatrix("mvp", ref mvp);
            
            GL.LineWidth(2.0f);
            geometry.Draw(6);
            GL.LineWidth(1.0f);
            
            base.Draw(e);
        }
        
        private static Byte3[] vertexData = new Byte3[] {
            // X
            new Byte3(0,0,0),
            new Byte3(100,0,0),
            // Y
            new Byte3(0,0,0),
            new Byte3(0,100,0),
            // Z
            new Byte3(0,0,0),
            new Byte3(0,0,100)
        };
        
        private static byte[] colorData = new byte[] {
            // X
            255,0,0,160,
            255,0,0,160,
            // Y
            0,255,0,160,
            0,255,0,160,
            // Z
            0,0,255,160,
            0,0,255,160,
        };
    }
}

