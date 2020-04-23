using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalSignature
{
    class Stribog256 : Stribog
    {
        public override byte[] GetHash(string message)
        {
            N = new byte[64];
            Sigma = new byte[64];
            for (int i = 0; i < 64; i++)
            {
                iv[i] = 0x01;
            }
            byte[] hash = GetHashX(Encoding.ASCII.GetBytes(message));
            byte[] h256 = new byte[32];
            Array.Copy(hash, 0, h256, 0, 32);
            return h256;
        }

    }
}
