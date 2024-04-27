using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace spindafysharp
{
    public class SpindaDrawer
    {
        Bitmap SpindaBase;

        Bitmap SpindaMask;

        Bitmap[] spots;

        Bitmap image;

        Graphics g;

        public SpindaDrawer(Graphics g, Bitmap image, Bitmap spindaBase, Bitmap spindaMask, Bitmap[] spindaSpots)
        {
            this.g = g;
            this.image = image;
            SpindaBase = spindaBase;
            SpindaMask = spindaMask;
            spots = spindaSpots;
        }

        public void DrawSpinda(Point pos, Spinda spinda)
        {
            Bitmap bmp = (Bitmap)SpindaBase.Clone();

            //spinda.SetPID(389223);

            DrawSpot(0);
            DrawSpot(1);
            DrawSpot(2);
            DrawSpot(3);

            g.DrawImage(bmp,new Point(pos.X- (bmp.Width/3), pos.Y - (bmp.Height / 3)));

            void DrawSpot(int index)
            {
                Bitmap spot = spots[index];

                uint xP = spinda.spots[index].Item1;
                uint yP = spinda.spots[index].Item2;

                int xO = Spinda.offsets[index].Item1;
                int yO = Spinda.offsets[index].Item2;

                for (int i = 0; i < spot.Width; i++)
                {
                    for (int j = 0; j < spot.Height; j++)
                    {
                        int x = (int)(i + xO + xP);
                        int y = (int)(j + yO + yP);

                        if(spot.GetPixel(i,j).A != 0)
                        {
                            Color c = SpindaMask.GetPixel(x, y);
                            if (c.A != 0)
                            {
                                bmp.SetPixel(x, y, c);
                            }
                        }
                    }
                }
            }

        }

        public Spinda GenerateSpinda(Point Pos, Bitmap sourceImage)
        {
            Spinda spinda = new Spinda();

            GetDarkestPixel(0, ref spinda);
            GetDarkestPixel(1, ref spinda);
            GetDarkestPixel(2, ref spinda);
            GetDarkestPixel(3, ref spinda);

            return spinda;

            void GetDarkestPixel(int index, ref Spinda spinda)
            {
                byte darkX = Spinda.invisiblePos[index].Item1;
                byte darkY = Spinda.invisiblePos[index].Item2;

                float darkColor = 1000;

                float maxValue = 60;

                int valueCount = 0;

                float X = 0;
                float Y = 0;


                for (byte i = 0; i < 15; i++)
                {
                    for (byte j = 0; j < 15; j++)
                    {
                        int x = i + Spinda.offsets[index].Item1 + Pos.X;
                        int y = j + Spinda.offsets[index].Item2 + Pos.Y;
                        Color c;
                        try
                        {
                            c = sourceImage.GetPixel(x, y);
                        }
                        catch (Exception)
                        {
                            break;
                        }

                        float B = 0.2126f * c.R + 0.7152f * c.G + 0.0722f * c.B;

                        if (B*1.5f > maxValue) { continue; }

                        X += i;
                        Y += j;
                        valueCount++;

                        //if (B < darkColor)
                        //{
                        //    darkX= i;
                        //    darkY= j;
                        //    darkColor = B;
                        //}
                    }
                }

                if(valueCount != 0)
                {
                    darkX = (byte)(X / valueCount);
                    darkY = (byte)(Y / valueCount);
                }

                spinda.spots[index] = (darkX, darkY);
            }
        }

        public struct Spinda
        {
            public (byte, byte)[] spots = new (byte, byte)[4];
            public static readonly (int, int)[] offsets = {
                (8, 6),
                (32, 7),
                (14, 24),
                (26, 25)
            };

            public static readonly (byte, byte)[] invisiblePos = {
                (0, 0),
                (15, 0),
                (0, 0),
                (15, 15)
            };

            public void SetPID(uint pid)
            {
                spots[0] = ((byte)(pid & 0x0000000f), (byte)((pid & 0x000000f0) >> 4));
                spots[1] = ((byte)((pid & 0x00000f00) >> 8), (byte)((pid & 0x0000f000) >> 12));
                spots[2] = ((byte)((pid & 0x000f0000) >> 16), (byte)((pid & 0x00f00000) >> 20));
                spots[3] = ((byte)((pid & 0x0f000000) >> 24), (byte)((pid & 0xf0000000) >> 28));
            }

            public Spinda()
            {
            }

            public Point GetXY(byte spot)
            {
                Point p = new Point();
                p.X = spot >> 4;
                p.Y = spot & 15;

                return p;
            }
        }

    }
}
