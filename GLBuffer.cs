using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace univ
{
	public abstract class GLBuffer
	{
		public int ID { get { return this.id; } }
		public BufferTarget Target { get { return this.target; } set { this.target = value; } }
		
		protected int id;
		protected Vector3[] vertexData;
		protected BufferTarget target;
		
		public GLBuffer(BufferTarget target)
		{
			this.id = GL.GenBuffer();
			this.target = target;
		}
		
		~GLBuffer()
		{
			GL.DeleteBuffer(this.id);
		}
		
		public void Bind()
		{
			GL.BindBuffer(this.target, this.id);
		}
		
		public void Unbind()
		{
			GL.BindBuffer(this.target, 0);
		}
	}
}