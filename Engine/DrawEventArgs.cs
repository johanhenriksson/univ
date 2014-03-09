using System;
using OpenTK;

namespace univ
{
	public class DrawEventArgs
	{
		public Scene Scene { get { return this.scene; } }
		public Camera Camera { get { return this.camera; } } 
		public Matrix4 ModelMatrix { get { return this.modelMatrix; } }
		
		protected Scene scene;
		protected Camera camera;
		protected Matrix4 modelMatrix;
		
		public DrawEventArgs(Scene scene, Camera camera, Matrix4 model)
		{
			this.scene = scene;
			this.camera = camera;
			this.modelMatrix = model;
		}
	}
}

