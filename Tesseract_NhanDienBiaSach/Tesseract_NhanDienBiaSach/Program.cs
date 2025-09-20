using System;
using System.IO;
using System.Text;
using Tesseract;
using System.Diagnostics;

namespace Tesseract_NhanDienBiaSach
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string imageFolder = @"D:\Tessaract_OCR\MauBiaSach";
            string tessDataPath = @"D:\Tesseract\tessdata";
            string outputFile = @"D:\Tessaract_OCR\All_OCR_Result.txt";

            if (!Directory.Exists(imageFolder))
            {
                Console.WriteLine("Không có ảnh trong thư mục");
                return;
            }

            try
            {
                Stopwatch totalWatch = Stopwatch.StartNew();
                int totalWords = 0;
                double totalConfidence = 0;
                int totalImages = 0;

                using (var engine = new TesseractEngine(tessDataPath, "vie+eng", EngineMode.Default))
                using (var writer = new StreamWriter(outputFile, false, Encoding.UTF8))
                {
                    foreach (var imgPath in Directory.GetFiles(imageFolder, "*.jpg"))
                    {
                        Console.WriteLine($"Đang xử lý: {Path.GetFileName(imgPath)}");

                        Stopwatch imgWatch = Stopwatch.StartNew();

                        using (var img = Pix.LoadFromFile(imgPath))
                        using (var page = engine.Process(img))
                        {
                            string text = page.GetText();
                            float confidence = page.GetMeanConfidence(); // 0 → 1
                            int wordCount = text.Split(new char[] { ' ', '\n', '\r', '\t' },
                                                       StringSplitOptions.RemoveEmptyEntries).Length;

                            imgWatch.Stop();
                            double seconds = imgWatch.Elapsed.TotalSeconds;
                            double speed = seconds > 0 ? wordCount / seconds : wordCount;

                            // Ghi kết quả OCR + thống kê cho từng ảnh
                            writer.WriteLine("===== " + Path.GetFileName(imgPath) + " =====");
                            writer.WriteLine(text);
                            writer.WriteLine();
                            writer.WriteLine($"📌 Số từ nhận diện: {wordCount}");
                            writer.WriteLine($"📌 Độ tin cậy trung bình: {(confidence * 100):F2}%");
                            writer.WriteLine($"📌 Thời gian xử lý: {imgWatch.ElapsedMilliseconds} ms");
                            writer.WriteLine($"📌 Tốc độ: {speed:F2} từ/giây");
                            writer.WriteLine("--------------------------------------------------");
                            writer.WriteLine();

                            // In ra console
                            Console.WriteLine($"⏱ {Path.GetFileName(imgPath)}: {imgWatch.ElapsedMilliseconds} ms, {wordCount} từ, {confidence * 100:F2}% chính xác");

                            // Cộng dồn
                            totalImages++;
                            totalWords += wordCount;
                            totalConfidence += confidence;
                        }
                    }

                    totalWatch.Stop();
                    double avgConfidence = totalImages > 0 ? (totalConfidence / totalImages) * 100 : 0;
                    double totalSeconds = totalWatch.Elapsed.TotalSeconds;
                    double overallSpeed = totalSeconds > 0 ? totalWords / totalSeconds : totalWords;

                    // Ghi tổng kết vào file
                    writer.WriteLine("===== TỔNG KẾT =====");
                    writer.WriteLine($"📌 Tổng số ảnh: {totalImages}");
                    writer.WriteLine($"📌 Tổng số từ: {totalWords}");
                    writer.WriteLine($"📌 Độ tin cậy trung bình: {avgConfidence:F2}%");
                    writer.WriteLine($"📌 Tổng thời gian xử lý: {totalWatch.ElapsedMilliseconds} ms");
                    writer.WriteLine($"📌 Tốc độ trung bình: {overallSpeed:F2} từ/giây");

                    // In tổng kết ra console
                    Console.WriteLine("===== TỔNG KẾT =====");
                    Console.WriteLine($"📌 Tổng số ảnh: {totalImages}");
                    Console.WriteLine($"📌 Tổng số từ: {totalWords}");
                    Console.WriteLine($"📌 Độ tin cậy trung bình: {avgConfidence:F2}%");
                    Console.WriteLine($"📌 Tổng thời gian xử lý: {totalWatch.ElapsedMilliseconds} ms");
                    Console.WriteLine($"📌 Tốc độ trung bình: {overallSpeed:F2} từ/giây");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Lỗi: {ex.Message}");
            }
        }
    }
}
