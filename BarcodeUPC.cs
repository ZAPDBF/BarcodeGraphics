using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcode
{  // code definitoions
   // 1 = single
   // 2= double 
   // 3= tripple 
   // 4= quadrupal  

    // 5 = special narrow black
// 6= special narrow white
    public static class BarcodeUPC
    {

        // left 6 start with White
        // right 6 start with black
        // fixed 12 digit numric only


        public static string MiddleGuard = "65656";
        public static string StartAndEnd = "565";
        public static string[] BarPattern = { "3211", "2221", "2122", "1411", "1132", "1231", "1114", "1312", "1213", "3112" };

        public static Image GenerateBarcode(string upc, SizeF size)
        {

            int CheckSum = 0;
            string patternLeft = StartAndEnd;
            string PatternRight="";
            int x = 0;
            int ic=0;
            foreach(char c in upc)
            {
                x++;
                int.TryParse(c.ToString(), out ic);
                if (x <=6)
                {
                    patternLeft += BarPattern[ic];

                }
                else
                {
                    PatternRight += BarPattern[ic];

                }



            }









            // find relitave length of whole barcode to determine barcode widths
            int RelitiveSize = 0;

            int ot = 0;
            foreach (char i in pattern)
            {
                int.TryParse(i.ToString(), out ot);
                RelitiveSize += ot;
            }
            int QuietZone = (int)((size.Width * .2) / 2);
            int PrintArea = (int)(size.Width - (QuietZone * 2));





            float SingleBarWidth = (float)PrintArea / (float)RelitiveSize;

            float DoubleBarWidth = (SingleBarWidth * 2);
            float TrippleBarWidth = (SingleBarWidth * 3);
            float QuadrupleBarWidth = (SingleBarWidth * 4);

            int TopMargin = (int)((size.Height * .2) / 2);
            int BarHeight = (int)(size.Height - (TopMargin * 2));



            // double Ratio = ((double)NarrowBarscount / (double)PrintArea);


          
            

            Bitmap bmpBarcode = new Bitmap((int)size.Width, (int)size.Height);
            using (Graphics g = Graphics.FromImage(bmpBarcode))
            {
                g.Clear(Color.White);

                float Cloc = QuietZone;
                Brush bBlack = new SolidBrush(Color.Black);
                Brush bWhite = new SolidBrush(Color.White);

                foreach (int t in pattern)
                {
                    switch (t)
                    {
                        case 1: // wide black
                            g.FillRectangle(bBlack, Cloc, TopMargin, DoubleBarWidth, BarHeight);
                            Cloc += DoubleBarWidth;
                            break;
                        case 2: // Narrow Black
                            g.FillRectangle(bBlack, Cloc, TopMargin, SingleBarWidth, BarHeight);
                            Cloc += SingleBarWidth;
                            break;
                        case 3: // Narrow White
                            g.FillRectangle(bWhite, Cloc, TopMargin, SingleBarWidth, BarHeight);
                            Cloc += SingleBarWidth;
                            break;
                        case 4: // Wide White
                            g.FillRectangle(bWhite, Cloc, TopMargin, DoubleBarWidth, BarHeight);
                            Cloc += DoubleBarWidth;
                            break;


                    }


                }

            }
            return (Image)bmpBarcode;
        }

    } }
