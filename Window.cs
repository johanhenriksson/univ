using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace univ
{
	public class Window : GameWindow
	{
		Shader shader;
		Camera camera;
		Cube cube;
		
		public Window () : base(800, 600)
		{
		}
		
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			Title = "Test";
			GL.ClearColor(Color.CornflowerBlue);
			
			shader = new Shader("Shaders/vertex.glsl", "Shaders/fragment.glsl");
			shader.Link();
			
			cube = new Cube(shader);
			
			Cube child = new Cube(shader);
			child.Rotate(45, 0, 0);
			child.Move(0,2,0);
			child.Rescale(0.8f);
			cube.Attach(child);
			cube.Move(-1,0,-1);
			
			this.camera = new Camera(Width, Height);
			GL.Viewport(0, 0, Width, Height);
			GL.Enable(EnableCap.DepthTest);
			GL.DepthFunc(DepthFunction.Less);
			GL.Disable(EnableCap.CullFace);
		}
		
		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			base.OnUpdateFrame (e);
			float dt = (float)e.Time;
			camera.Update(dt);
			cube.Update(dt);
			
			cube.Rotate(0,30.0f * dt, 0);
		}
		
		protected override void OnRenderFrame(FrameEventArgs e)
		{
			base.OnRenderFrame (e);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			
			shader.Use();
			
			DrawEventArgs args = new DrawEventArgs(null, this.camera, Matrix4.Identity);
			cube.Draw(args);
			
			SwapBuffers();
		}
	}
}

