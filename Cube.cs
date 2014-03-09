using System;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace univ
{
	public class Cube : DrawableComponent
	{
		VertexArray geometry;
		
		public Cube(Shader shader)
			: base(shader)
		{
			this.geometry = new VertexArray(PrimitiveType.Triangles);
			Vector3Buffer vertexBuffer = this.geometry.CreateBuffer<Vector3Buffer>("vertex", BufferTarget.ArrayBuffer);
			vertexBuffer.BufferData(ref cubeVerticies);
			
			Vector3Buffer colorBuffer = this.geometry.CreateBuffer<Vector3Buffer>("color", BufferTarget.ArrayBuffer);
			colorBuffer.BufferData(ref cubeColors);
			
			UShortBuffer indexBuffer = this.geometry.CreateBuffer<UShortBuffer>("index", BufferTarget.ElementArrayBuffer);
			indexBuffer.BufferData(ref cubeIndices);
			
			this.geometry.AddPointer("color",  new VertexAttribute(this.shader, "vColor",    VertexAttribPointerType.Float, 3, 0, false));	
			this.geometry.AddPointer("vertex", new VertexAttribute(this.shader, "vPosition", VertexAttribPointerType.Float, 3, 0, false));
		}
		
		public override void Draw(DrawEventArgs e)
		{
			Matrix4 mvp = this.modelMatrix * e.ModelMatrix * e.Camera.ViewProjection;
			shader.SetUniformMatrix("mvp", ref mvp);
			this.geometry.DrawElements(36, DrawElementsType.UnsignedShort);
			
			base.Draw(e);
		}
		
		public override void Update(float dt)
		{
			
		}
		
		private static Vector3[] cubeVerticies = new Vector3[] {
		    // front
		    new Vector3(-1.0f, -1.0f,  1.0f), // Bottom Left
		    new Vector3(1.0f, -1.0f,  1.0f),  // Bottom Right
		    new Vector3(1.0f,  1.0f,  1.0f),  // Top Left
		    new Vector3(-1.0f,  1.0f,  1.0f), // Top Right
		    // back
		    new Vector3(-1.0f, -1.0f, -1.0f), // Bottom Left
		    new Vector3(1.0f, -1.0f, -1.0f),  // Bottom Right
		    new Vector3(1.0f,  1.0f, -1.0f),  // Top Left
		    new Vector3(-1.0f,  1.0f, -1.0f), // Top Right
		};
		
		private static Vector3[] cubeColors = new Vector3[] {
		    // front colors
		    new Vector3(0.3f, 0.1f, 0.1f),
		    new Vector3(0.3f, 0.1f, 0.1f),
		    new Vector3(0.9f, 0.1f, 0.1f),
		    new Vector3(0.9f, 0.1f, 0.1f),
		    // back colors
		    new Vector3(0.3f, 0.1f, 0.1f),
		    new Vector3(0.3f, 0.1f, 0.1f),
		    new Vector3(0.9f, 0.1f, 0.1f),
		    new Vector3(0.9f, 0.1f, 0.1f),
		};
		
		private static ushort[] cubeIndices = new ushort[] {
		    // front
		    0, 1, 2, // z+
		    2, 3, 0,
		    // top
		    3, 2, 6, // y+
		    6, 7, 3,
		    // back
		    7, 6, 5, // z-
		    5, 4, 7,
		    // bottom // y-
		    4, 5, 1,
		    1, 0, 4,
		    // left // x-
		    4, 0, 3,
		    3, 7, 4,
		    // right x+
		    1, 5, 6,
		    6, 2, 1,
		};
	}
}

