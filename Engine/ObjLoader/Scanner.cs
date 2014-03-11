using System;
using System.IO;
using System.Text;
using OpenTK;

namespace univ
{
    public class Scanner
    {
        StringBuilder buffer;
        string word;
        int index;
        string line;

        public Scanner()
        {
            this.index = 0;
            this.buffer = new StringBuilder();
        }

        public void Feed(string line)
        {
            this.line = line;
            this.index = 0;
            readNextWord();
        }

        protected void readNextWord()
        {
            char next;

            while(index < line.Length) {
                next = line[index++]; 
                if (char.IsWhiteSpace(next))
                    break;
                buffer.Append(next);
            }
            
            while(index < line.Length && char.IsWhiteSpace(line[index]))
                index++;

            if (buffer.Length == 0) {
                word = null;
                return;
            }

            word = buffer.ToString();
            buffer.Clear();
        }

        public bool HasNextWord()
        {
            return word != null;
        }

        public string NextWord()
        {
            try {
                if (word == null) throw new NullReferenceException();
                return word;
            } finally {
                readNextWord();
            }
        }

        public bool HasNextFloat()
        {
            if (word == null)
                return false;
            float dummy;
            return float.TryParse(word, out dummy);
        }

        public float NextFloat()
        {
            try {
                return float.Parse(word);
            } finally {
                readNextWord();
            }
        }
        
        public uint NextUInt()
        {
            try {
                return uint.Parse(word);
            } finally {
                readNextWord();
            }
        }
        
        public ushort NextUShort()
        {
            try {
                return ushort.Parse(word);
            } finally {
                readNextWord();
            }
        }

        public Vector3 NextVector3()
        {
            return new Vector3(NextFloat(), NextFloat(), NextFloat());
        }
        
        public UInt3 NextUInt3()
        {
            return new UInt3(NextUInt(), NextUInt(), NextUInt());
        }
        
        public UShort3 NextUShort3()
        {
            return new UShort3(NextUShort(), NextUShort(), NextUShort());
        }
    }
}

