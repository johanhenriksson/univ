using System;

namespace univ
{
    public struct UInt3
    {
        public uint X;
        public uint Y;
        public uint Z;
        
        public UInt3(uint x, uint y, uint z) {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
    }
    
    public struct UShort3
    {
        public ushort X;
        public ushort Y;
        public ushort Z;
        
        public UShort3(ushort x, ushort y, ushort z) {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
    }
}

