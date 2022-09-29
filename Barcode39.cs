﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
//using System.Drawing;

//using System.Drawing.Drawing2D;
//using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

using SixLabors.ImageSharp.PixelFormats;

namespace Barcode
{
   public static class Barcode39
    {


      static  int[] GetCharCode39(Char cChar)
        {
             List<int> iChar;
            // 0 = skip
            // 1= wide black
            //2 = narrow black
            // 3 = narrow  white
            // 4 = wide white white
            // base code

            Boolean isLowerCase = false;
            int[] one = {3, 1,3, 2,3, 2,3, 2,3, 1,3 };
            int[] two = {3, 2,3, 1,3, 2,3, 2,3, 1,3 };
            int[] three = { 3,1,3, 1,3, 2,3, 2,3, 2,3 };
            int[] four = { 3,2,3, 2,3, 1,3, 2,3, 1,3 };
            int[] five = { 3,1,3, 2,3, 1,3, 2,3, 2,3 };
            int[] six = { 3,2,3, 1,3, 1,3, 2,3, 2,3 };
            int[] seven = { 3,2,3, 2,3, 2,3, 1,3, 1,3 };
            int[] eight = { 3,1,3, 2,3, 2,3, 1,3, 2,3 };
            int[] nine = { 3,2,3, 1,3, 2,3, 1,3, 2,3 };
            int[] ten = { 3,2,3, 2,3, 1,3, 1,3, 2,3 };
            int[] timesZero = {0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0 };
            int[] timesTen = { 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0 };
            int[] timesTwenty = { 0, 0, 0, 0, 0, 0, 0,0 , 4, 0, 0 }; 
            int[] timesThirty = { 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] dollarSign = { 3, 2, 4, 2, 4, 2, 4, 2,3, 2,3 };
            int[] slashSign = { 3, 2, 4, 2, 4, 2, 3, 2, 4,2,3 };
            int[] plusSign = { 3, 2, 4, 2, 3, 2, 4, 2, 4, 2, 3 };
            int[] percentSign = { 3, 2, 3, 2, 4, 2, 4, 2, 4, 2,  3 };
            int[] noChange = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };            
            
            
            int chartest;
            if ((int)cChar > 64 && (int)cChar < 91) // upper case leter
                chartest = (int)cChar - 55;
            else if ((int)cChar > 96 && (int)cChar < 123) //lower case letter
            {
                chartest = (int)cChar - 32; // convert to upper
                isLowerCase = true;
            }
            else if ((int)cChar == 45) chartest = 36; // - (minus) convert
            else if ((int)cChar == 46) chartest = 37; // . (dot) convert
            else if ((int)cChar == 95) chartest = 38; // _ (underscore) convert
            else if ((int)cChar == 42) chartest = 100; // (*) represents end char special encoding
            else if ((int)cChar == 36) chartest = 101; // $ special encoded char
            else if ((int)cChar == 47) chartest = 102; // forward slash
            else if ((int)cChar == 43) chartest = 103; // + sign special char
            else if ((int)cChar == 37) chartest = 104; // % special Char
            else if ((int)cChar > 48 && (int)cChar < 58) chartest = (int)cChar - 49; // numbers not including zero
            else if ((int)cChar == 48) chartest = 9; // zero fix


            else throw new ArgumentException(string.Format("The following Charater cannot be used in Code39 ascii({0}) or Char Value ({1})", (int)cChar, cChar));
            int[] BasePattern=one;
            int[] Multiplier=noChange;

            if (chartest < 39)
            {
                if (chartest < 10)  Multiplier = timesZero;
            else if(chartest > 9 && chartest < 20) Multiplier = timesTen;
            else if(chartest > 19 && chartest < 30) Multiplier = timesTwenty;  
            else if(chartest > 29 && chartest <39) Multiplier = timesThirty;


                int SingDig;
                if (chartest > 9)

                {
                    int.TryParse(chartest.ToString().Substring(1, 1), out SingDig);
                
                
                
                }

                else
                    SingDig = chartest;

                switch (SingDig)
                {
                    case 1: // number one
                        BasePattern = two;

                        break;
                    case 2: // number two
                        BasePattern = three;

                        break;
                    case 3: // number 3
                        BasePattern = four;

                        break;
                    case 4: // number 4
                        BasePattern = five;

                        break;
                    case 5: // number 5
                        BasePattern = six;

                        break;
                    case 6: // number 6
                        BasePattern = seven;

                        break;
                    case 7: //number 7
                        BasePattern = eight;

                        break;
                    case 8: // number 8
                        BasePattern = nine;

                        break;
                    case 9: // number 9
                        BasePattern = ten;
                        break;
                    case 0: // number 0
                        BasePattern = one;
                        break;

                }                  
            }
            else // special char
            {
               switch (chartest)
                {
                    case 100: // (*) or end marker for code
                    BasePattern = ten;
                        Multiplier = timesThirty;
                        break;
                    case 101: // $ dollar sign
                        BasePattern = dollarSign;
                        Multiplier = noChange;
                        break;
                    case 102:
                        BasePattern = slashSign;
                        Multiplier = noChange;
                        break ;
                    case 103:
                        BasePattern = plusSign;
                        Multiplier = noChange;
                        break;
                    case 104:
                        BasePattern = percentSign;
                        Multiplier = noChange;
                      break;  
                }
            }
            List<int> oout = new();
            if (isLowerCase) // lower case add plus symbol
            {

                oout.AddRange(plusSign.ToList());


            }
            
            // build final symbol
          
            for (int x=0; x< 11; x++)
            {
if (Multiplier[x] > 0)
                {
                    oout.Add(Multiplier[x]);
                }
else
                {
                    oout.Add(BasePattern[x]);

                }



            }

           return oout.ToArray();

        }


        public static Image CreateBarcode(string Barcode, SizeF size,int TextLoc)
        {



            List<int> barcodeSymbol = GetCharCode39('*').ToList(); // add beginning astrict

            char[] bcode = Barcode.ToCharArray();
            foreach (char c in bcode)
            {
                barcodeSymbol.AddRange( GetCharCode39(c).ToList());
            }
            barcodeSymbol.AddRange(GetCharCode39('*').ToList()); // add trailing astrict


            // find relitave length of whole barcode to determine barcode widths
            int RelitiveSize = 0;
            int WideBarsCount = 0;
            int NarrowBarscount = 0;
foreach(int i in barcodeSymbol)
            {
                switch (i)
                {

                    case 1: // wide black bar
                        RelitiveSize += 2;
                        WideBarsCount ++;
                        break;       
                    case 2: // narrow black bar
                        RelitiveSize += 1;
                        NarrowBarscount ++;
                        break;
                    case 3: // narrow white bar
                        RelitiveSize += 1;
                        NarrowBarscount++;
                        break;
                    case 4: // Wide White Bar
                        RelitiveSize += 2;
                        WideBarsCount++;    
                        break;                
                }


            }
            
            int QuietZone = (int)((size.Width * .2)/2);
            int PrintArea = (int)(size.Width -( QuietZone*2));

           // double Ratio = ((double)NarrowBarscount / (double)PrintArea);

            
            float SingleBarWidth =(float)PrintArea/ (float)RelitiveSize ;
           
            float DoubleBarWidth = (SingleBarWidth * 2);

            int TopMargin = (int)((size.Height * .2) / 2);
            int BarHeight = (int)(size.Height - (TopMargin * 2));
            Bitmap bmpBarcode = new Bitmap((int)size.Width,(int)size.Height);
            using (Graphics g = Graphics.FromImage(bmpBarcode))
            { g.Clear(Color.White);

                float Cloc = QuietZone;
            IBrush  bBlack = new Brushes.Horizontal(Color.Black);
                Brush bWhite = new SolidBrush(Color.White);
                
                foreach (int t  in barcodeSymbol)
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



                if (TextLoc == TextLocation.Top || TextLoc == TextLocation.Bottom)
                {

                    float Yloc = 0;
                    float Height = 0;
                    if (TextLoc == TextLocation.Top)
                    {
                        Yloc = 1;
                        Height = TopMargin - 10;

                    }
                    else
                    {
                        Yloc = TopMargin + BarHeight;
                        Height = size.Height - Yloc - 5;


                    }


                    Font cFont = new Font("Aerial", BarcodeTools.GetFontPointSize(Height));

                    float XLoc = 0;
                    XLoc = ((float)size.Width / (float)2) - (g.MeasureString(Barcode, cFont).Width / (float)2);
                    g.DrawString(Barcode, cFont, bBlack, XLoc, Yloc);






                }




            }
            return (Image)bmpBarcode;







        }














        }   
    
    
    
    
    }

