using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace univ.Engine.Geometry
{
	public class VertexBuffer
	{
		public int ID { get { return this.id; } }
		public BufferTarget Target { get { return this.target; } }
		
		protected int id;
		protected BufferTarget target;
		
		public VertexBuffer(BufferTarget target)
		{
			this.id = GL.GenBuffer();
			this.target = target;
		}
		
		~VertexBuffer()
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
		
		public void BufferData<T>(ref T[] data) where T : struct
		{
			Bind();
            unsafe {      
			GL.BufferData<T>(
				this.target, 
                (IntPtr)(data.Length * sizeof(T)),
				data, 
				BufferUsageHint.StaticDraw
			);
            }
		}
	}
}