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
        
        BaseLight ambient;
        DirectionalLight sunlight;
        PointLight point;
        Model model;
        
        bool wireframe = false;
     
        public Window() 
            : base(800, 600, new GraphicsMode(new ColorFormat(8,8,8,8), 24, 24, 8), "univ", GameWindowFlags.Default, DisplayDevice.Default, 4, 1, GraphicsContextFlags.ForwardCompatible)
        {
            KeyPress += delegate(object sender, KeyPressEventArgs e) {
                switch(e.KeyChar) {
                case 't':
                    wireframe = !wireframe;
                    if (wireframe) {
                        GL.Disable(EnableCap.CullFace);
                        GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                    } else {
                        //GL.Enable(EnableCap.CullFace);
                        GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                    }
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
            
            shader = ShaderLibrary.Get("basic");
            line_shader = ShaderLibrary.Get("line");
            
            ObjLoader loader = new ObjLoader("teapot.obj");
            model = loader.Assemble();
            model.Rescale(0.02f);
         
            octree = new Octree(shader);
            octree.Rescale(10);
            
            axis = new Axis(line_shader);
            
            this.camera = new Camera(Width, Height);
            GL.Viewport(0, 0, Width, Height);
            
            /* lighting */
            sunlight = new DirectionalLight(new BaseLight(new Vector3(1.0f, 0.5f, 0.2f), 1.0f),
                                            new Vector3(0, -1, -2));
            ambient = new BaseLight(new Vector3(1.0f), 0.5f);
            point = new PointLight(new BaseLight(new Vector3(0.0f, 1.0f, 0.0f), 0.35f),
                                   new Attenuation(0.0f, 0.00f, 0.002f),
                                   new Vector3(7, 10, 0));
            
            shader.Use();
            /* fuck this shit for now */
            //shader.SetBlock<DirectionalLight>("sunlight", ref sunlight);
            //shader.SetBlock<BaseLight>("ambient", ref ambient);
            
            shader.SetBaseLight("ambient", ambient);
            shader.SetDirectionalLight("sunlight", sunlight);
            shader.SetPointLight("pointLights[0]", point);
            
            GL.Enable(EnableCap.DepthClamp);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.ClearColor(Color.LightGray);
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
            
            Vector3 eye = camera.Position;
            shader.SetVector3("eye", ref eye);
         
            DrawEventArgs args = new DrawEventArgs(null, this.camera, Matrix4.Identity);
            
            //octree.Draw(args); 
            axis.Draw(args);
            
            shader.Use();
            model.Draw(args);
         
            SwapBuffers();
        }
    }
}