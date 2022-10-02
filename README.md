# BarcodeGraphics
A light weight class that is used to Create the bitmap used in  2D barcodes. 
Very limited Graphic Capability, We just need to make Black Rectangles on white surfaces, and print letters at the bottom.
It creates a Byte array that can be converted to a DataStream that represents a bitmap. 
I wanted to make a platform independent class that can handle this simple task in lieu of using a graphic class like ImageSharp. 
On a Windows Machine 
   Useage;       
     BarImage bi = new BarImage(800,600);
    
    // barcode alg
    ..
    ..
    ..
    bi.MakeBar(StartX,StartY,Width,Height); 
    ..
    ..
    ..
    // end
    
    // on windows platform if you wish
    
       MemoryStream ms = new MemoryStream(bi);
       PictureBox1.Image = Image.FromStream(ms);
       
       
