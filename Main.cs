using System;
using System.Drawing;

namespace univ
{
	public class Program
	{
		public static void Main(string[] args)
		{
            /*
            Noise n = new Noise(2314);
            float[,] noise = new float[256, 256];
            float max = 0.0f;
            for (int o = 0; o < 100; o += 10)
            {
                float oct = (float)Math.Pow(2.0, o);
                
                float[,] level = n.Noise2D(0,0, 256, 256, 1 / oct);
                for(int y = 0; y < 256; y++) {
                    for(int x = 0; x < 256; x++) {
                        noise[x,y] += level[x,y] * oct;
                        if (noise[x,y] > max)
                            max = noise[x,y];
                    }
                }
            }
            
            for(int y = 0; y < 256; y++)
                for(int x = 0; x < 256; x++)
                    noise[x,y] /= 10;
            
            int sx = noise.GetLength(0);
            int sy = noise.GetLength(1);
            Bitmap img = new Bitmap(sx, sy);
            for(int x = 0; x < sx; x++)
                for(int y = 0; y < sy; y++)
                    img.SetPixel(x,y, Color.FromArgb((int)(255.0f * noise[x,y]),
                                                     (int)(255.0f * noise[x,y]),
                                                     (int)(255.0f * noise[x,y])));
            img.Save("noise.png");
            
            */
            Window wnd = new Window();
            wnd.Run(30,60);
		}
	}
}

