using System;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace univ
{
	public class UShortBuffer : GLBuffer
	{
		public UShortBuffer() : this(BufferTarget.ArrayBuffer) { }
		
		public UShortBuffer(BufferTarget target)
			: base(target)
		{
		}
		
		public void BufferData(ref ushort[] data)
		{
			Bind();
			GL.BufferData<ushort>(
				this.target, 
				(IntPtr)(data.Length * sizeof(ushort)),
				data, 
				BufferUsageHint.StaticDraw
			);
		}
	}
}

