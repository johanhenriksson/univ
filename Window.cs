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
        Shader line_shader;
     
        // Move to a scene graph
        Camera camera;
        Octree octree;
        Axis axis;
        
        Model model;
        
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
                case 'h':
                    octree.Delete(0,1,0);
                    break;
                case 'f':
                    Vector3 pos = camera.Unproject(Width / 2, Height / 2);   
                    Console.WriteLine("Unprojected " + pos);
                    break;
                }
            };
            
            Move += delegate(object sender, EventArgs e) {
                GL.Viewport(0, 0, Width, Height); 
            };
            
            Mouse.ButtonUp += delegate(object sender, MouseButtonEventArgs e) {
                if (e.Button == MouseButton.Left) {
                    Vector3 pos = camera.Unproject(e.X, e.Y);
                    Console.WriteLine("Clicked at {0},{1} -> {2}", e.X, e.Y, pos);
                }
            };
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Title = "univ engine";
            GL.ClearColor(Color.LightGray);
            
            shader = ShaderLibrary.Get("basic");
            line_shader = ShaderLibrary.Get("line");
            
            ObjLoader loader = new ObjLoader("teapot.obj");
            model = loader.Assemble();
            model.Rescale(0.05f);
         
            octree = new Octree(shader);
            octree.Rescale(10);
            
            axis = new Axis(line_shader);
            
            this.camera = new Camera(Width, Height);
            GL.Viewport(0, 0, Width, Height);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Front);
        }
     
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            float dt = (float)e.Time;
            camera.Update(dt);
         
            octree.Update(dt);
            axis.Update(dt);
            
            model.Rotate(0, 30 * dt, 0);
        }
     
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
         
            shader.Use();
            
            Vector3 lightvec = new Vector3(0, 1, 2);
            shader.SetVector3("light", ref lightvec);
         
            DrawEventArgs args = new DrawEventArgs(null, this.camera, Matrix4.Identity);
            
            //octree.Draw(args); 
            axis.Draw(args);
            
            shader.Use();
            model.Draw(args);
         
            SwapBuffers();
        }
    }
}