using System;
using System.Drawing;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace univ
{
    public class Window : GameWindow
    {
        // Move to some kind of shader manager
        Shader shader;
     
        // Move to a scene graph
        Camera camera;
        Octree octree;
        
        bool wireframe = false;
     
        public Window() 
            : base(800, 600, new GraphicsMode(new ColorFormat(8,8,8,8), 24, 24, 8), "univ", GameWindowFlags.Default, DisplayDevice.Default, 4, 1, GraphicsContextFlags.ForwardCompatible)
        {
            KeyPress += delegate(object sender, KeyPressEventArgs e) {
                switch(e.KeyChar) {
                case 't':
                    wireframe = !wireframe;
                    GL.PolygonMode(MaterialFace.FrontAndBack, wireframe ? PolygonMode.Line : PolygonMode.Fill);
                    break;
                case 'g':
                    octree.Split(0,1,0);
                    break;
                }
            };
            
            Move += delegate(object sender, EventArgs e) {
                GL.Viewport(0, 0, Width, Height); 
            };
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Title = "univ engine";
            GL.ClearColor(Color.LightGray);
         
            shader = new Shader("Shaders/vertex.glsl", "Shaders/fragment.glsl");
            shader.Link();
         
            octree = new Octree(shader);
            octree.Rescale(5);
         
            this.camera = new Camera(Width, Height);
            GL.Viewport(0, 0, Width, Height);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.Disable(EnableCap.CullFace);
        }
     
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            float dt = (float)e.Time;
            camera.Update(dt);
         
            octree.Update(dt);
        }
     
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
         
            shader.Use();
            
            Vector4 lightvec = new Vector4(0, 1, 2, 0);
            //Vector4.Transform(ref lightvec, ref camera.View, out lightvec);
            Vector3 transformed = lightvec.Xyz.Normalized();
            
            shader.SetVector3("light", ref transformed);
         
            DrawEventArgs args = new DrawEventArgs(null, this.camera, Matrix4.Identity);
            octree.Draw(args);
         
            SwapBuffers();
        }
    }
}