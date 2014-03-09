using System;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace univ
{
	public class Vector3Buffer : GLBuffer
	{
		public Vector3Buffer ()
			: base(BufferTarget.ArrayBuffer)
		{
		}
		
		public void BufferData(ref Vector3[] data)
		{
			Bind();
			GL.BufferData<Vector3>(
				this.target, 
				(IntPtr)(data.Length * Vector3.SizeInBytes),
				data, 
				BufferUsageHint.StaticDraw
			);	
		}
	}
}

