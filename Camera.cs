using System;
using OpenTK;
using OpenTK.Input;

namespace univ
{
	public class Camera
	{
		public Matrix4 ViewProjection;
		
		protected int screenWidth;
		protected int screenHeight;
		protected float aspectRatio;
		protected Vector3 position;
		protected Matrix4 view;
		protected Matrix4 projection;
		
		protected float speed = 4.0f;
		
		public Camera(int screenWidth, int screenHeight)
		{
			this.screenHeight = screenHeight;
			this.screenWidth = screenWidth;
			this.aspectRatio = (float)screenWidth / screenHeight;
			this.position = new Vector3(3.0f, 3.0f, 5.0f);
			this.projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 2, this.aspectRatio, 0.1f, 100.0f);
			this.ViewProjection = Matrix4.Identity;
		}
		
		public void Update(float dt)
		{
			var keystate = Keyboard.GetState();
			if (keystate.IsKeyDown(Key.W)) {
				position.Z -= speed * dt;
			}
			if (keystate.IsKeyDown(Key.S)) {
				position.Z += speed * dt;
			}
			if (keystate.IsKeyDown(Key.A)) {
				position.X -= speed * dt;
			}
			if (keystate.IsKeyDown(Key.D)) {
				position.X += speed * dt;
			}
			if (keystate.IsKeyDown(Key.Space)) {
				position.Y += speed * dt;
			}
			if (keystate.IsKeyDown(Key.LShift)) {
				position.Y -= speed * dt;
			}
			
			this.view = Matrix4.LookAt(this.position, position - Vector3.UnitZ, Vector3.UnitY);
			this.ViewProjection = view * projection;
		}
	}
}

