using System;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using univ.Engine.Geometry;

namespace univ
{
    public class Model : Component
    {
        public Shader Shader { get { return shader; } }
        public int Triangles { get { return triangles; } }
        
        protected int triangles;
        protected Matrix4 mvp;
        protected VertexArray mesh;
        protected Shader shader;
        
        public Model(int triangles, VertexArray mesh)
            : base()
        {
            this.mesh = mesh;
            this.triangles = triangles;
            this.shader = ShaderLibrary.Get("basic");
        }
        
        public override void Draw(DrawEventArgs e)
        {
            shader.Use();
            
            /* Calculate Model/View/Projection matrix */
            Matrix4.Mult(ref this.modelMatrix, ref e.ModelMatrix, out mvp);
            Matrix3 m = new Matrix3(mvp);
            shader.SetMatrix3("model", ref m); // TODO: Mat4 instead?
            Matrix4.Mult(ref mvp, ref e.Camera.ViewProjection, out mvp);
            shader.SetMatrix("mvp", ref mvp);
            
            
            /* Normal matrix */
            m.Invert();
            m.Transpose();
            shader.SetMatrix3("G", ref m);
            
            mesh.DrawElements(this.triangles * 3, DrawElementsType.UnsignedShort);
            
            base.Draw(e);
        }
    }
}

