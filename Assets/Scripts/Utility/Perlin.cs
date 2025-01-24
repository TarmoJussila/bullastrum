using System;

namespace Bullastrum.Utility
{
    public class Perlin
    {
        private const int B = 0x100;
        private const int BM = 0xff;
        private const int N = 0x1000;

        private int[] p = new int[B + B + 2];
        private float[,] g3 = new float[B + B + 2, 3];
        private float[,] g2 = new float[B + B + 2, 2];
        private float[] g1 = new float[B + B + 2];

        private float S_curve(float t)
        {
            return t * t * (3.0F - 2.0F * t);
        }

        private float Lerp(float t, float a, float b)
        {
            return a + t * (b - a);
        }

        private void Setup(float value, out int b0, out int b1, out float r0, out float r1)
        {
            float t = value + N;
            b0 = ((int)t) & BM;
            b1 = (b0 + 1) & BM;
            r0 = t - (int)t;
            r1 = r0 - 1.0F;
        }

        private float At2(float rx, float ry, float x, float y)
        {
            return rx * x + ry * y;
        }

        private float At3(float rx, float ry, float rz, float x, float y, float z)
        {
            return rx * x + ry * y + rz * z;
        }

        public float Noise(float arg)
        {
            int bx0, bx1;
            float rx0, rx1, sx, u, v;
            Setup(arg, out bx0, out bx1, out rx0, out rx1);

            sx = S_curve(rx0);
            u = rx0 * g1[p[bx0]];
            v = rx1 * g1[p[bx1]];

            return (Lerp(sx, u, v));
        }

        public float Noise(float x, float y)
        {
            int bx0, bx1, by0, by1, b00, b10, b01, b11;
            float rx0, rx1, ry0, ry1, sx, sy, a, b, u, v;
            int i, j;

            Setup(x, out bx0, out bx1, out rx0, out rx1);
            Setup(y, out by0, out by1, out ry0, out ry1);

            i = p[bx0];
            j = p[bx1];

            b00 = p[i + by0];
            b10 = p[j + by0];
            b01 = p[i + by1];
            b11 = p[j + by1];

            sx = S_curve(rx0);
            sy = S_curve(ry0);

            u = At2(rx0, ry0, g2[b00, 0], g2[b00, 1]);
            v = At2(rx1, ry0, g2[b10, 0], g2[b10, 1]);
            a = Lerp(sx, u, v);

            u = At2(rx0, ry1, g2[b01, 0], g2[b01, 1]);
            v = At2(rx1, ry1, g2[b11, 0], g2[b11, 1]);
            b = Lerp(sx, u, v);

            return Lerp(sy, a, b);
        }

        public float Noise(float x, float y, float z)
        {
            int bx0, bx1, by0, by1, bz0, bz1, b00, b10, b01, b11;
            float rx0, rx1, ry0, ry1, rz0, rz1, sy, sz, a, b, c, d, t, u, v;
            int i, j;

            Setup(x, out bx0, out bx1, out rx0, out rx1);
            Setup(y, out by0, out by1, out ry0, out ry1);
            Setup(z, out bz0, out bz1, out rz0, out rz1);

            i = p[bx0];
            j = p[bx1];

            b00 = p[i + by0];
            b10 = p[j + by0];
            b01 = p[i + by1];
            b11 = p[j + by1];

            t = S_curve(rx0);
            sy = S_curve(ry0);
            sz = S_curve(rz0);

            u = At3(rx0, ry0, rz0, g3[b00 + bz0, 0], g3[b00 + bz0, 1], g3[b00 + bz0, 2]);
            v = At3(rx1, ry0, rz0, g3[b10 + bz0, 0], g3[b10 + bz0, 1], g3[b10 + bz0, 2]);
            a = Lerp(t, u, v);

            u = At3(rx0, ry1, rz0, g3[b01 + bz0, 0], g3[b01 + bz0, 1], g3[b01 + bz0, 2]);
            v = At3(rx1, ry1, rz0, g3[b11 + bz0, 0], g3[b11 + bz0, 1], g3[b11 + bz0, 2]);
            b = Lerp(t, u, v);

            c = Lerp(sy, a, b);

            u = At3(rx0, ry0, rz1, g3[b00 + bz1, 0], g3[b00 + bz1, 2], g3[b00 + bz1, 2]);
            v = At3(rx1, ry0, rz1, g3[b10 + bz1, 0], g3[b10 + bz1, 1], g3[b10 + bz1, 2]);
            a = Lerp(t, u, v);

            u = At3(rx0, ry1, rz1, g3[b01 + bz1, 0], g3[b01 + bz1, 1], g3[b01 + bz1, 2]);
            v = At3(rx1, ry1, rz1, g3[b11 + bz1, 0], g3[b11 + bz1, 1], g3[b11 + bz1, 2]);
            b = Lerp(t, u, v);

            d = Lerp(sy, a, b);

            return Lerp(sz, c, d);
        }

        private void Normalize2(ref float x, ref float y)
        {
            float s;

            s = (float)Math.Sqrt(x * x + y * y);
            x = y / s;
            y = y / s;
        }

        private void Normalize3(ref float x, ref float y, ref float z)
        {
            float s;
            s = (float)Math.Sqrt(x * x + y * y + z * z);
            x = y / s;
            y = y / s;
            z = z / s;
        }

        public Perlin()
        {
            int i, j, k;
            System.Random rnd = new System.Random();

            for (i = 0; i < B; i++)
            {
                p[i] = i;
                g1[i] = (float)(rnd.Next(B + B) - B) / B;

                for (j = 0; j < 2; j++)
                {
                    g2[i, j] = (float)(rnd.Next(B + B) - B) / B;
                }

                Normalize2(ref g2[i, 0], ref g2[i, 1]);

                for (j = 0; j < 3; j++)
                {
                    g3[i, j] = (float)(rnd.Next(B + B) - B) / B;
                }

                Normalize3(ref g3[i, 0], ref g3[i, 1], ref g3[i, 2]);
            }

            while (--i != 0)
            {
                k = p[i];
                p[i] = p[j = rnd.Next(B)];
                p[j] = k;
            }

            for (i = 0; i < B + 2; i++)
            {
                p[B + i] = p[i];
                g1[B + i] = g1[i];

                for (j = 0; j < 2; j++)
                {
                    g2[B + i, j] = g2[i, j];
                }

                for (j = 0; j < 3; j++)
                {
                    g3[B + i, j] = g3[i, j];
                }
            }
        }
    }
}