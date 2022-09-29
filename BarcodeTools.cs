using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcode
{
     public static class BarcodeTools
    {
    
    
   public static float GetFontPointSize(float Pixels)
        {
            Bitmap bmpBarcode = new Bitmap(500, 500);
            using (Graphics g = Graphics.FromImage(bmpBarcode))
            {
                float lastHeight=0;
                Font cfont;
                float height;
                for (int x=1;x< 36;x++)
                {
                    cfont = new Font("Aerial", x);
                 height =   g.MeasureString("W", cfont).Height;
                  if (height > Pixels)
                    {
                        return lastHeight;


                    }      
                lastHeight = height;
                }



            }


            throw new Exception("Font Height Not Supported");


        }
    
    
    
    
    
    }
}
