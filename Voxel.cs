using System;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using univ.Engine.Geometry;

namespace univ
{
    public class Voxel : Component
    {
        Matrix4 mvp;
        Shader shader;
        VertexArray geometry;
        
        VertexBuffer vertex;
        VertexBuffer colors;
        VertexBuffer normal;
        
        public Voxel(Shader shader)
            : base()
        {
            this.shader = shader;
            geometry = new VertexArray(PrimitiveType.Triangles);
            vertex = geometry.CreateBuffer("vertex", BufferTarget.ArrayBuffer);
            colors = geometry.CreateBuffer("colors", BufferTarget.ArrayBuffer);
            normal = geometry.CreateBuffer("normal", BufferTarget.ArrayBuffer);
            
            Byte3[] colorData =  ColorData(new Byte3(255, 0, 0), new Byte3(0, 255, 255),
                                           new Byte3(0, 255, 0), new Byte3(255, 0, 255),
                                           new Byte3(0, 0, 255), new Byte3(255, 255, 0));
            vertex.BufferData<Byte3>(ref vertexData);
            colors.BufferData<Byte3>(ref colorData);
            normal.BufferData<SByte3>(ref normalData);
            
            geometry.AddPointer("vertex", new VertexAttribute(this.shader, "vPosition", VertexAttribPointerType.UnsignedByte, 3, 0, false));
            geometry.AddPointer("colors", new VertexAttribute(this.shader, "vColor", VertexAttribPointerType.UnsignedByte, 3, 0, true));   
            geometry.AddPointer("normal", new VertexAttribute(this.shader, "vNormal", VertexAttribPointerType.Byte, 3, 0, true));
        }
        
        public override void Draw(DrawEventArgs e)
        {
            shader.Use();
            
            /* Calculate Model/View/Projection matrix */
            Matrix4.Mult(ref this.modelMatrix, ref e.ModelMatrix, out mvp);
            Matrix3 m = new Matrix3(mvp); // Copy the top-right 3x3 model matrix for later...
            Matrix4.Mult(ref mvp, ref e.Camera.ViewProjection, out mvp);
            shader.SetMatrix("mvp", ref mvp);
            
            /* Normal matrix */
            m.Invert();
            m.Transpose();
            shader.SetMatrix3("G", ref m);
            
            geometry.Draw(vertexData.Length);
            
            base.Draw(e);
        }
        
        #region Vertex Data
        private static Byte3[] vertexData = new Byte3[] {
            // x+ (east)
            new Byte3(1,0,0),
            new Byte3(1,0,1),
            new Byte3(1,1,0),
    
            new Byte3(1,1,0),
            new Byte3(1,0,1),
            new Byte3(1,1,1),
    
            // x- (west)
            new Byte3(0,1,0),
            new Byte3(0,0,1),
            new Byte3(0,0,0),
    
            new Byte3(0,1,1),
            new Byte3(0,0,1),
            new Byte3(0,1,0),
    
            // y+ (up)
            new Byte3(0,1,0),
            new Byte3(1,1,0),
            new Byte3(0,1,1),
    
            new Byte3(0,1,1),
            new Byte3(1,1,0),
            new Byte3(1,1,1),
    
            // y- (down)
            new Byte3(0,0,1),
            new Byte3(1,0,0),
            new Byte3(0,0,0),
    
            new Byte3(1,0,1),
            new Byte3(1,0,0),
            new Byte3(0,0,1),
   
            // z+ (south)
            new Byte3(0,0,1),
            new Byte3(0,1,1),
            new Byte3(1,1,1),
    
            new Byte3(1,1,1),
            new Byte3(1,0,1),
            new Byte3(0,0,1),
            
            // z- (north)
            new Byte3(1,1,0),
            new Byte3(0,1,0),
            new Byte3(0,0,0),
    
            new Byte3(0,0,0),
            new Byte3(1,0,0),
            new Byte3(1,1,0)
        };
        #endregion
        
        #region Color Data
        /* GL_BYTE x3, Normalized */
        protected static Byte3[] ColorData(
            Byte3 xpos, Byte3 xneg, 
            Byte3 ypos, Byte3 yneg, 
            Byte3 zpos, Byte3 zneg
        ) {
            Byte3[] colors = new Byte3[36];
            for(int i = 0; i < 6; i++)
                colors[i] = xpos;
            for(int i = 6; i < 12; i++)
                colors[i] = xneg;
            for(int i = 12; i < 18; i++)
                colors[i] = ypos;
            for(int i = 18; i < 24; i++)
                colors[i] = yneg;
            for(int i = 24; i < 30; i++)
                colors[i] = zpos;
            for(int i = 30; i < 36; i++)
                colors[i] = zneg;
            return colors;
        }
        #endregion 
        
        #region Normal Data
        /* GL_SIGNED_BYTE x3, Normalized */
        private static SByte3[] normalData = new SByte3[] {
            // X+
            new SByte3(127, 0, 0),
            new SByte3(127, 0, 0),
            new SByte3(127, 0, 0),
            new SByte3(127, 0, 0),
            new SByte3(127, 0, 0),
            new SByte3(127, 0, 0),
            // X-
            new SByte3(-128, 0, 0),
            new SByte3(-128, 0, 0),
            new SByte3(-128, 0, 0),
            new SByte3(-128, 0, 0),
            new SByte3(-128, 0, 0),
            new SByte3(-128, 0, 0),
            // Y+
            new SByte3(0, 127, 0),
            new SByte3(0, 127, 0),
            new SByte3(0, 127, 0),
            new SByte3(0, 127, 0),
            new SByte3(0, 127, 0),
            new SByte3(0, 127, 0),
            // Y-
            new SByte3(0, -128, 0),
            new SByte3(0, -128, 0),
            new SByte3(0, -128, 0),
            new SByte3(0, -128, 0),
            new SByte3(0, -128, 0),
            new SByte3(0, -128, 0),
            // Z+
            new SByte3(0, 0, 127),
            new SByte3(0, 0, 127),
            new SByte3(0, 0, 127),
            new SByte3(0, 0, 127),
            new SByte3(0, 0, 127),
            new SByte3(0, 0, 127),
            // Z-
            new SByte3(0, 0, -128),
            new SByte3(0, 0, -128),
            new SByte3(0, 0, -128),
            new SByte3(0, 0, -128),
            new SByte3(0, 0, -128),
            new SByte3(0, 0, -128),
        };
        #endregion
    }
}

