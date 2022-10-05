using System;

using ImageZap;

namespace Barcode
{  // code definitoions
   // 1 = single
   // 2= double 
   // 3= tripple 
   // 4= quadrupal  

    // 5 = special narrow black
    // 6= special narrow white
    public  class BarcodeUPC:BarcodeAbstract
    {

        // left 6 start with White
        // right 6 start with black
        // fixed 12 digit numric only


        public static string MiddleGuard = "1111";
        public static string StartAndEnd = "111";
        public static string[] BarPattern = { "3211", "2221", "2122", "1411", "1132", "1231", "1114", "1312", "1213", "3112" };
        public const int UPCA = 1;
        public const int UPCE = 2;


        public  override byte[] MakeBarcode(string upc, SizeF size,int BarcodeType,int TextLoc)
        {

           
            string patternLeft = StartAndEnd;
            string PatternRight = "";
            int x = 0;
            int ic = 0;
            foreach (char c in upc)
            {
                x++;
                int.TryParse(c.ToString(), out ic);
                if (x <= 6)
                {
                    patternLeft += BarPattern[ic];

                }
                else
                {
                    PatternRight += BarPattern[ic];

                }



            }

            PatternRight += StartAndEnd;

            string pattern;
            pattern = patternLeft +MiddleGuard+PatternRight;



            Boolean IsBlackBar = false; //Lined up with left guard pattern

            // find relitave length of whole barcode to determine barcode widths
            int RelitiveSize = 0;
            int ot = 0;

            int QuietZone = (int)((size.Width * .2) / 2);
            int PrintArea = (int)(size.Width - (QuietZone * 2));
            float Cloc = QuietZone; // end of quiet zone
            float SingleBarWidth =  (float)PrintArea / (float)RelitiveSize;
            float DoubleBarWidth = (SingleBarWidth * 2);
            float TrippleBarWidth = (SingleBarWidth * 3);
            float QuadrupleBarWidth = (SingleBarWidth * 4);

            int TopMargin = (int)((size.Height * .2) / 2);
            int BarHeight = (int)(size.Height - (TopMargin * 2));
            
            BarImage bmpBarcode = new((int)size.Width, (int)size.Height);
            // key to proper usage is that the colors will alternate
            // and that the center guard made up of 4 bars will altrinate
            foreach (int t in pattern)
            {

                IsBlackBar = !IsBlackBar; // modify if black bar, if white bar just skip to next location
                switch (t)
                {
                    case 1: // Narrow black 1
                       if (IsBlackBar)
                        bmpBarcode.MakeBar( Cloc, TopMargin, SingleBarWidth, BarHeight);
                        Cloc += SingleBarWidth;
                        break;
                    case 2: // mid Black 2
                        if (IsBlackBar)
                            bmpBarcode.MakeBar( Cloc, TopMargin, DoubleBarWidth, BarHeight);
                        Cloc += DoubleBarWidth;
                        break;
                    case 3: // wide black 3
                        if (IsBlackBar)
                            bmpBarcode.MakeBar( Cloc, TopMargin, TrippleBarWidth, BarHeight);
                        Cloc += TrippleBarWidth;
                        break;
                    case 4: // Wide White
                        if (IsBlackBar)
                            bmpBarcode.MakeBar(Cloc, TopMargin, QuadrupleBarWidth, BarHeight);
                        Cloc += QuadrupleBarWidth;
                        break;


                }

               


            }
            return bmpBarcode.ImageArray;
        } 
    } 
    
    
}    
        
    
