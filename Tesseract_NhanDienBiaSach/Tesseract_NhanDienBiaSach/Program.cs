using System;
using System.IO;
using System.Text;
using Tesseract;

namespace Tesseract_NhanDienBiaSach
{
    internal class Program
    {
        static void Main(string[] args)
        {  
            string imageFolder = @"D:\Nam4HK1\DoAnChuyenNganh\Tessaract_OCR\MauBiaSach";
           
            string tessDataPath = @"D:\Tesseract\tessdata";
           
            string outputFile = @"D:\Nam4HK1\DoAnChuyenNganh\Tessaract_OCR\All_OCR_Result.txt";

            
            if (!Directory.Exists(imageFolder))
            {
                Console.WriteLine("Không có ảnh trong thư mục");
                return;
            }

          
            try
            {
                
                using (var engine = new TesseractEngine(tessDataPath, "vie+eng", EngineMode.Default))
               

                using (var writer = new StreamWriter(outputFile, false, Encoding.UTF8))
                {   
                    foreach (var imgPath in Directory.GetFiles(imageFolder, "*.jpg"))
                    {
                        
                        Console.WriteLine($"Đang xử lý: {Path.GetFileName(imgPath)}");
                        
                        using (var img = Pix.LoadFromFile(imgPath))
                        
                        using (var page = engine.Process(img))
                        {
                           
                            string text = page.GetText();
                            
                            writer.WriteLine("===== " + Path.GetFileName(imgPath) + " =====");
                           
                            writer.WriteLine(text);
                            writer.WriteLine(); 
                        }
                    }
                }

                Console.WriteLine($"✅ Hoàn tất OCR! Kết quả lưu tại: {outputFile}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi: {ex.Message}");
            }
        }
    }
}