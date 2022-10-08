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


      
        public const int UPCA = 1;
        public const int UPCE = 2;
        public const int UPCAtoE = 3;
        private int TotalModules;

   private string CheckDigit(string upc)
        {

            int Even = 0;
            int Odd = 0;
            int pl;
            int modulo;

            for (int xx = 0; xx < upc.Length; xx++) // only odd position digits
            {
                if (int.TryParse(upc[xx].ToString(), out pl))

                    if (xx % 2 == 0)
                    {
                        Odd += pl;

                    }
                    else
                    {
                        Even += pl;
                    }
            }
            modulo = (((Odd * 3) + Even) % 10); // add parity for both type a and e
            if (modulo != 0)
            {
                modulo = 10 - modulo;


            }

           return modulo.ToString();





        }
      
        private string upcAPattern(string upc)
        {
             string MiddleGuard = "11115";
             string StartAndEnd = "111";
             string[] BarPattern = { "3211", "2221", "2122", "1411", "1132", "1231", "1114", "1312", "1213", "3112" };
            // 5 is transpose, will not do anything no bar just swap white for black
            // get parity digit

            if (upc.Length == 12) // includes check digit we will just remove. check digit will be generated
                upc = upc.Substring(0, 11);


            TotalModules = 95;

            if (upc.Length != 11) throw new Exception("UPC Must be 11 Digits long");

          
            upc = upc + CheckDigit(upc);




            string patternLeft = StartAndEnd;
            string PatternRight = "";
            int x = 0;
            int ic = 0;
            foreach (char c in upc)
            {
                x++;
                if (!int.TryParse(c.ToString(), out ic)) throw new Exception("UPC Must be numaric at least one digit is not numaric!");
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
            return patternLeft + MiddleGuard + PatternRight;


        }
        private string UPCEFromA(string upc)

        {
          
            if (upc.Length ==12) // includes check digit we will just remove. check digit will be generated
              upc = upc.Substring(0,11);

            TotalModules = 51;
            
            if (upc.Length != 11) throw new Exception("UPC Must be 11 Digits long");
            string[] BarEParOdd = { "3211", "2221", "2122", "1411", "1132", "1231", "1114", "1312", "1213", "3112" };
            string[] BarEParEven = { "1123", "1222", "2212", "1141", "2311", "1321", "4111", "2131", "3121", "2113" };
            string[] NumberSys = { "EEEOOO", "EEOEOO", "EEOOEO", "EEOOE", "EOEEOO", "EOOEEO", "EOOOEE", "EOEOEO", "EOEOOE", "EOOEOE" };
            string EStartGuard = "111";
            string EEndGuard = "111111";
            Boolean startrecord = false;
            int ci;
            string ManfCodeCond = "";
            string ProdCode = "";
            string FinalCode;
            string CodeSystem;
            // condense manf code
            CodeSystem =upc[0].ToString();
            for (int x = 5; x >= 0; x--)
            {
                if (!int.TryParse(upc[x].ToString(), out ci)) throw new Exception("UPC contains non numaric values");
                
                    if (startrecord)
                    ManfCodeCond = upc[x].ToString() + ManfCodeCond;
                if (ci != 0 && !startrecord)
                    {
                        startrecord = true;

                        ManfCodeCond = upc[x].ToString() + ManfCodeCond;


                    }
                


            }
           
            // remove code system from manf code

            ManfCodeCond = ManfCodeCond.Substring(1);

            // condense product code
            startrecord = false;
            for (int x = 6; x < 11; x++)

                if (int.TryParse(upc[x].ToString(), out ci))
                {
                    if (startrecord)
                        ProdCode += upc[x].ToString();
                    if (ci != 0 && !startrecord)
                    {
                        startrecord = true;

                        ProdCode += upc[x].ToString();


                    }
                }
            if (ProdCode.Length > 3) throw new Exception("Product Code too long max is 999");
            
            
            // generate last upcE digit
            int LastUPCE=0;
            if (ManfCodeCond.Length == 2) LastUPCE = 0;
            else if (ManfCodeCond[2] == '1' && ManfCodeCond.Length == 3) LastUPCE = 1;
            else if (ManfCodeCond[2] == '2' && ManfCodeCond.Length == 3) LastUPCE = 2;
            else if (ManfCodeCond.Length == 3 && ProdCode.Length < 3) LastUPCE = 3;
            else if (ManfCodeCond.Length == 4 && ProdCode.Length == 1) LastUPCE = 4;
            else if (ManfCodeCond.Length == 5 && ProdCode == "5") LastUPCE = 5;
            else if (ManfCodeCond.Length == 5 && ProdCode == "6") LastUPCE = 6;
            else if (ManfCodeCond.Length == 5 && ProdCode == "7") LastUPCE = 7;
            else if (ManfCodeCond.Length == 5 && ProdCode == "8") LastUPCE = 8;
            else if (ManfCodeCond.Length == 5 && ProdCode == "9") LastUPCE = 9;
            else throw new Exception("UPCA code cannot be condenced to type E");


                if (LastUPCE <3)  // ensure the product code is three chars long padd with zeros 
            {
                ManfCodeCond = ManfCodeCond.Substring(0, 2);
                if (ProdCode.Length == 0) ProdCode = "000";
                if(ProdCode.Length==1) ProdCode = "00" +ProdCode;
                if (ProdCode.Length == 2) ProdCode = "0" + ProdCode;
            }
            if (LastUPCE == 3)
            {

                if (ProdCode.Length == 0) ProdCode = "00";    
                if(ProdCode.Length == 1) ProdCode = "0" + ProdCode;



            }
            string CondCode;
            if (LastUPCE == 4  && ProdCode.Length == 0) ProdCode = "0";

              if (LastUPCE >4) // we have a valid product code
            {
                CondCode = ManfCodeCond + ProdCode; // last digit of product code same as LastUPCE

            }
              else
            {
                CondCode = ManfCodeCond + ProdCode + LastUPCE;

            }

            string CheckD = CheckDigit(upc);
            if (!int.TryParse(CheckD, out int chd)) throw new Exception("Invalid Check Digit");

            // TO FIND PARITY BIT WE NEED TO USE MODULS 10 AS IN CODE TYPE A
            // to determine parity pattern
            string barCode;
            if (CondCode.Length != 6) throw new Exception("Product Code could not be Reduced to 6 numbers (FinalCode)");
            barCode = EStartGuard;
            string BasePattern;
            // locate code system for this barcode
            BasePattern = NumberSys[chd];
              if (CodeSystem=="1") // base pattern needs inverted if code system is one
            {
                string bp1="";
                foreach(char l in BasePattern)
                {
                    if (l == 'O') bp1 += "E";
                    if (l == 'E') bp1 += "O";

                }
                BasePattern = bp1;
              }


            int cc,xx;
            xx = 0;
                foreach ( char C in CondCode)
            {
                if (!int.TryParse(C.ToString(), out cc))
                    throw new Exception("Barcode contains a Non a Numaric Value");
                if (BasePattern[xx] == 'E')
                    barCode += BarEParEven[cc];
                else
                    barCode += BarEParOdd[cc];
                xx++;
            }


           barCode += EEndGuard;
         
            
            return barCode;

        }
        private string upcEPattern(string upc)
        {

            throw new NotImplementedException("this algrithem is not complete, use UPCAtoE");
            return "";

        }
        public  override byte[] MakeBarcode(string upc, SizeF size,int BarcodeType,int TextLoc)
        {
            string pattern;
           switch (BarcodeType)
            {
                case BarcodeUPC.UPCA:
                    pattern = upcAPattern(upc);
                    break;

                case BarcodeUPC.UPCE:
                    pattern = upcEPattern(upc);
                    break;
                case BarcodeUPC.UPCAtoE:
                    pattern = UPCEFromA(upc);
                    break;

                default:
                        throw new Exception("BarcodeType Is invalid");
                   
            }







            
            



            Boolean IsBlackBar = false; //Lined up with left guard pattern

            // find relitave length of whole barcode to determine barcode widths
           
            //int ot = 0;

            int QuietZone = (int)((size.Width * .2) / 2);
            int PrintArea = (int)(size.Width - (QuietZone * 2));
            float Cloc = QuietZone; // end of quiet zone
            float SingleBarWidth =  (float)PrintArea / (float)TotalModules;
            float DoubleBarWidth = (SingleBarWidth * 2);
            float TrippleBarWidth = (SingleBarWidth * 3);
            float QuadrupleBarWidth = (SingleBarWidth * 4);

            int TopMargin = (int)((size.Height * .2) / 2);
            int BarHeight = (int)(size.Height - (TopMargin * 2));
            
            BarImage bmpBarcode = new((int)size.Width, (int)size.Height);
            // key to proper usage is that the colors will alternate
            // and that the center guard made up of 4 bars will altrinate
            foreach (char t in pattern)
            {

                IsBlackBar = !IsBlackBar; // modify if black bar, if white bar just skip to next location
                switch (t)
                {
                    case '1': // Narrow black 1
                       if (IsBlackBar)
                        bmpBarcode.MakeBar( Cloc, TopMargin, SingleBarWidth, BarHeight);
                        Cloc += SingleBarWidth;
                        break;
                    case '2': // mid Black 2
                        if (IsBlackBar)
                            bmpBarcode.MakeBar( Cloc, TopMargin, DoubleBarWidth, BarHeight);
                        Cloc += DoubleBarWidth;
                        break;
                    case '3': // wide black 3
                        if (IsBlackBar)
                            bmpBarcode.MakeBar( Cloc, TopMargin, TrippleBarWidth, BarHeight);
                        Cloc += TrippleBarWidth;
                        break;
                    case '4': // Wide White
                        if (IsBlackBar)
                            bmpBarcode.MakeBar(Cloc, TopMargin, QuadrupleBarWidth, BarHeight);
                        Cloc += QuadrupleBarWidth;
                        break;

                    // case 5 is transpose no need to do anyting just as a notation white will be swaped with black
                }

               


            }
            return bmpBarcode.ImageArray;
        } 
    } 
    
    
}    
        
    
