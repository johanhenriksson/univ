using System;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace univ.Engine.Geometry
{
	/// <summary>
	/// Vertex attribute.
	/// </summary>
	public class VertexAttribute
	{
		public string Name { get { return this.name; } }
		public int Size { get { return this.size; } }
		public int Stride { get { return this.stride; } }
		public int Location { get { return this.location; } }
		public VertexAttribPointerType Type { get { return this.type; } }
		
		protected string name;
		protected int location;
		protected int size;
		protected int stride;
		protected bool normalized;
		protected Shader shader;
		protected VertexAttribPointerType type;
		
		public VertexAttribute(Shader shader, string name, VertexAttribPointerType type, int size, int stride, bool normalized)
		{
			this.name = name;
			this.type = type;
			this.size = size;
			this.stride = stride;
			this.normalized = normalized;
			this.shader = shader;
			this.location = shader.GetAttributeLocation(name);
		}
		
		public void Point()
		{
			GL.VertexAttribPointer(location, size, type, normalized, stride, 0);
		}
		
		public void Enable()
		{
			GL.EnableVertexAttribArray(this.location);
		}
		
		public void Disable()
		{
			GL.DisableVertexAttribArray(this.location);
		}
	}
}

