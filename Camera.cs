using System;
using OpenTK;

namespace univ
{
	public class Camera
	{
		public Matrix4 ViewProjection { get { return this.viewProjection; } }
		
		protected int screenWidth;
		protected int screenHeight;
		protected float aspectRatio;
		protected Vector3 position;
		protected Matrix4 view;
		protected Matrix4 projection;
		protected Matrix4 viewProjection;
		
		public Camera(int screenWidth, int screenHeight)
		{
			this.screenHeight = screenHeight;
			this.screenWidth = screenWidth;
			this.aspectRatio = (float)screenWidth / screenHeight;
			this.position = new Vector3(3.0f, 3.0f, 5.0f);
			this.projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 2, this.aspectRatio, 0.1f, 100.0f);
		}
		
		public void Update(float dt)
		{
			this.view = Matrix4.LookAt(this.position, Vector3.Zero, Vector3.UnitY);
			this.viewProjection = view * projection;
		}
	}
}

