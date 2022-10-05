using System;
using ImageZap; 
    namespace Barcode
{
    public  class Barcode128:BarcodeAbstract
    {

        public const int CodeTypeA = 0;
        public const int CodeTypeB = 1;
        public const int CodeTypeC = 2;
        const int TypeAValue = 103;
        const int TypeBValue = 104;
        const int TypeCValue = 105;
        public override byte[] MakeBarcode(string EncodingNumber, SizeF size, int CodeType,int TextLoc)

        {
            int CheckSum = 0;
            string pattern = "";
            switch (CodeType)
            {
                case CodeTypeA:
                    pattern = StartCodeAPatern;
                    CheckSum =TypeAValue;
                    break;
                case CodeTypeB:
                    pattern = StartCodeBPatern;
                    CheckSum=TypeBValue;
                    break;
                case CodeTypeC:
                    pattern = StartCodeCPatern;
                    CheckSum=(int)TypeCValue;
                    break;
                default:
                    throw new ArgumentException("Invalid Code Type"); 

             }

            int Position = 0;
            

// build patern
foreach(char c in EncodingNumber)
                {
                Position++;

                if (CodeType == CodeTypeA)
                {
                    pattern += PaternA(c);
                    CheckSum += Avalue(c) * Position;

                }
                else if (CodeType == CodeTypeB)
                {
                    pattern += PaternB(c);
                    CheckSum += Bvalue(c) * Position;

                }
                else if (CodeType == CodeTypeC)
                {
                    pattern += PaternC(c);
                    CheckSum += Cvalue(c);
                }
                else
                    throw new ArgumentException("Invalid Code Type");
            
            
            
            }
            // create check digit and stop codes here

            Math.DivRem(CheckSum , 103, out int csum);
            pattern += PatternRaw(csum);
            pattern += StopPatternOverlap;










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




            BarImage bmpBarcode = new ((int)size.Width, (int)size.Height);
             

                float Cloc = QuietZone;
               Boolean IsWhite = true;
                foreach (char t in pattern)
                {

                IsWhite = !IsWhite;
                    switch (t)
                    {
                        case '1':
                           // g.FillRectangle(cBrush, Cloc, TopMargin, SingleBarWidth, BarHeight);
                        if (!IsWhite)
                        bmpBarcode.MakeBar(Cloc, TopMargin, SingleBarWidth, BarHeight);
                        
                        Cloc += SingleBarWidth;
                            break;
                        case '2': 
                           if (!IsWhite) bmpBarcode.MakeBar( Cloc, TopMargin, DoubleBarWidth, BarHeight);
                            Cloc += DoubleBarWidth;
                            break;
                        case '3': 
                           if(!IsWhite) bmpBarcode.MakeBar( Cloc, TopMargin, TrippleBarWidth, BarHeight);
                            Cloc += TrippleBarWidth;
                            break;
                        case '4': 
                            if(!IsWhite) bmpBarcode.MakeBar( Cloc, TopMargin, QuadrupleBarWidth, BarHeight);
                            Cloc += QuadrupleBarWidth;
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
                        Yloc = TopMargin + BarHeight ;
                        Height = size.Height - Yloc -5 ;


                    }


                  //  Font cFont = new Font("Aerial", BarcodeTools.GetFontPointSize(Height));

                    //float XLoc = 0;
                  //  XLoc = ((float)size.Width / (float)2) - (g.MeasureString(EncodingNumber, cFont).Width / (float)2);
                  //  g.DrawString(EncodingNumber, cFont, bBlack, XLoc, Yloc);






                }


            
            return bmpBarcode.ImageArray ;






















           


         }
       static int Avalue(char Char)
        {
            if ((int)Char > 31 && (int)Char < 127)
            {
                return (int)Char - 32;

            }
            throw new ArgumentException("Pattern A invalid Char");

        }
        static int Bvalue(char Char)
        {
            if ((int)Char > 31 && (int)Char < 127)
            {
                return (int)Char - 32;

            }
            throw new ArgumentException("Pattern A invalid Char");

        }
        static int Cvalue(char Char)
        {
            if ((int)Char > 31 && (int)Char < 127)
            {
                return (int)Char - 32;

            }
            throw new ArgumentException("Pattern A invalid Char");

        }
    
        static string PaternA(char Char)
        { 
        if ((int)Char > 31 && (int)Char < 127 )
            {
                return GetAsciiPattern(Char);

            }        
              throw new ArgumentException("Pattern A invalid Char");        
        
        }

        static string PaternB(char Char)
        {
            if ((int)Char > 31 && (int)Char < 127)
            {
                return GetAsciiPattern(Char);

            }
            throw new ArgumentException("Pattern A invalid Char");

        }


        static string PaternC(char Char)
        {
            if ((int)Char > 31 && (int)Char < 127)
            {
                return GetAsciiPattern(Char);

            }
            throw new ArgumentException("Pattern A invalid Char");

        }









        static string GetAsciiPattern(char Char)

         {
        if ((int)Char > 126) throw new ArgumentException("Cannot Get Char Higer than Char 126");
        if ((int)Char < 32) throw new ArgumentException("Cannot Return Char lower than 32 (space)");
        return PatternRaw((int)Char - 32);
         }


        static string PatternRaw(int number)
    // tesc code 128 symbology 
        {

        string[] pattern ={"212222","222122","222221","121223","121322","131222","122213","122312",
                "132212","221213","221312","231212","112232","122132","122231","113222","123122","123221",
                "223211","221132","221231","213212","223112","312131","311222","321122","321221","312212",
                "322112","322211","212123","212321","232121","111323","131123","131321","112313","132113",
                "132311","211313","231113","231311","112133","112331","132131","113123","113321","133121",
                "313121","211331","231131","213113","213311","213131","311123","311321","331121","312113",
                "312311","332111","314111","221411","431111","111224","111422","121124","121421","141122",
                "141221","112214","112412","122114","122411","142112","142211","241211","221114","413111",
                "241112","134111","111242","121142","121241","114212","124112","124211","411212","421112",
                "421211","212141","214121","412121","111143","111341","131141","114113","114311","411113",
                "411311","113141","114131","311141","411131","211412","211214","211232","233111","211133"
            };


        return pattern[number];


        }

       

        private static string StartCodeA
    { get
        {

            return PatternRaw(103);

        }
    }
    private static string StartCodeAPatern { get { return PatternRaw(102); } }
    private static string StartCodeBPatern { get { return PatternRaw(104); } }

    private static string StartCodeCPatern { get { return PatternRaw(105); } }
    private static string StopCodePattern { get { return PatternRaw(106); } }
    private static string reverseStopCodePattern { get { return PatternRaw(107); } }
    private static string StopPatternOverlap { get { return "2331112 "; } }
}
}
   

