using System;

namespace univ
{
    public struct Byte3
    {
        public byte X;
        public byte Y;
        public byte Z;
        
        public Byte3(byte x, byte y, byte z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
    }
    
    public struct SByte3
    {
        public static SByte3 PositiveX = new SByte3(127, 0, 0);
        public static SByte3 PositiveY = new SByte3(0, 127, 0);
        public static SByte3 PositiveZ = new SByte3(0, 0, 127);
        
        public static SByte3 NegativeX = new SByte3(-128, 0, 0);
        public static SByte3 NegativeY = new SByte3(0, -128, 0);
        public static SByte3 NegativeZ = new SByte3(0, 0, -128);
        
        public sbyte X;
        public sbyte Y;
        public sbyte Z;
        
        public SByte3(sbyte x, sbyte y, sbyte z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
    }
}

