using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace univ
{
	public class GLBuffer
	{
		public int ID { get { return this.id; } }
		public BufferTarget Target { get { return this.target; } }
		
		protected int id;
		protected BufferTarget target;
        protected BufferUsageHint usage;   
		
        public GLBuffer()
            : this(BufferTarget.ArrayBuffer, BufferUsageHint.StaticDraw) { }
        
        public GLBuffer(BufferTarget target)
            : this(target, BufferUsageHint.StaticDraw) { }
        
		public GLBuffer(BufferTarget target, BufferUsageHint usage)
		{
			this.id = GL.GenBuffer();
			this.target = target;
            this.usage = usage;
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
		
		public unsafe virtual void BufferData<T>(ref T[] data) where T : struct
		{
			Bind();
			GL.BufferData<T>(
				this.target, 
                (IntPtr)(data.Length * sizeof(T)),
				data, 
                this.usage         
			);
		}

	}
}