﻿namespace xxHash.Lib
{
    public static class xxHash32
    {
        private const uint p1 = 2654435761U;
        private const uint p2 = 2246822519U;
        private const uint p3 = 3266489917U;
        private const uint p4 =  668265263U;
        private const uint p5 =  374761393U;

        public static unsafe uint ComputeHash(byte[] data, int len, uint seed = 0)
        {
            fixed (byte* pData = &data[0])
            {
                uint* ptr = (uint*) pData;
                byte* end = pData + len;
                uint h32;

                if (len >= 16)
                {
                    byte* limit = end - 16;

                    uint v1 = seed + p1 + p2;
                    uint v2 = seed + p2;
                    uint v3 = seed + 0;
                    uint v4 = seed - p1;

                    do
                    {
                        v1 += ptr[0] * p2;
                        v1 = (v1 << 13) | (v1 >> (32 - 13)); // rotl 13
                        v1 *= p1;

                        v2 += ptr[1] * p2;
                        v2 = (v2 << 13) | (v2 >> (32 - 13)); // rotl 13
                        v2 *= p1;

                        v3 += ptr[2] * p2;
                        v3 = (v3 << 13) | (v3 >> (32 - 13)); // rotl 13
                        v3 *= p1;

                        v4 += ptr[3] * p2;
                        v4 = (v4 << 13) | (v4 >> (32 - 13)); // rotl 13
                        v4 *= p1;

                        ptr += 4;

                    } while (ptr <= limit);

                    h32 = ((v1 << 1) | (v1 >> (32 - 1))) +   // rotl 1
                          ((v2 << 7) | (v2 >> (32 - 7))) +   // rotl 7
                          ((v3 << 12) | (v3 >> (32 - 12))) + // rotl 12
                          ((v4 << 18) | (v4 >> (32 - 18)));  // rotl 18
                }
                else
                {
                    h32 = seed + p5;
                }
                h32 += (uint) len;

                // finalize
                while (ptr <= end - 4)
                {
                    h32 += ptr[0] * p3;
                    h32 = ((h32 << 17) | (h32 >> (32 - 17))) * p4; // (rotl 17) * p4
                    ptr += 1;
                }

                byte* lst = (byte*) ptr;
                while (lst < end)
                {
                    h32 += (*lst) * p5;
                    h32 = ((h32 << 11) | (h32 >> (32 - 11))) * p1; // (rotl 11) * p1
                    lst += 1;
                }

                // avalanche
                h32 ^= h32 >> 15;
                h32 *= p2;
                h32 ^= h32 >> 13;
                h32 *= p3;
                h32 ^= h32 >> 16;

                return h32;
            }
        }
    }
}
