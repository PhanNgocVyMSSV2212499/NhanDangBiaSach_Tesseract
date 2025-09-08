# 📚 Hành Trình Xây Dựng Chương Trình OCR Bìa Sách với Tesseract

## 🚀 Giới thiệu

xây dựng chương trình này để **tự động quét toàn bộ ảnh bìa sách** trong một thư mục, nhận dạng chữ (OCR) bằng **Tesseract**,
Chương trình hỗ trợ **tiếng Việt + tiếng Anh**,

---

## 🧠 Kiến thức áp dụng

- **OCR (Optical Character Recognition)** với thư viện `Tesseract` trong C#.
- **Xử lý file & thư mục** với `Directory.GetFiles`, `Path`, `StreamWriter`.
- **Unicode**: Ghi file UTF-8 để hiển thị tiếng Việt đúng.

---

## 🔧 Cài đặt

### 1. Cài Tesseract OCR

- Tải cho Windows tại: [UB Mannheim Tesseract Builds](https://github.com/UB-Mannheim/tesseract/wiki)
- Cài đặt và **tick**:
  - `Add to PATH`
  - `Vietnamese` (ngôn ngữ OCR)
- Kiểm tra cài đặt:

```bash
tesseract --version
```

### 2. Chuẩn bị dữ liệu ngôn ngữ

- Đảm bảo thư mục `tessdata` chứa:
  - `vie.traineddata`
  - `eng.traineddata`
- Nếu thiếu, tải từ: [Tesseract tessdata](https://github.com/tesseract-ocr/tessdata)

### 3. Tạo Project C# và cài thư viện

- Mở **Visual Studio**
- **Create new project** → Console App (.NET Framework)
- Cài NuGet Package:

```powershell
Install-Package Tesseract
```

---

## 💻 Cách sử dụng

### 1. Chuẩn bị ảnh

- Tất cả ảnh `.jpg` đặt trong thư mục, ví dụ:

```
D:\Nam4HK1\DoAnChuyenNganh\tesseract\MauBiaSach
```

### 2. Chỉnh code đường dẫn

Trong file `Program.cs`:

```csharp
string imageFolder = @"D:\Nam4HK1\DoAnChuyenNganh\tesseract\MauBiaSach";
string tessDataPath = @"D:\Tesseract\tessdata";
string outputFile = @"D:\Nam4HK1\DoAnChuyenNganh\tesseract\All_OCR_Result.txt";
```

### 3. Chạy chương trình

- Nhấn **F5** trong Visual Studio
- Xem kết quả tại `All_OCR_Result.txt`

---

## 📝 Code chính

```csharp
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
            string imageFolder = @"D:\Nam4HK1\DoAnChuyenNganh\tesseract\MauBiaSach";
            string tessDataPath = @"D:\Tesseract\tessdata";
            string outputFile = @"D:\Nam4HK1\DoAnChuyenNganh\tesseract\All_OCR_Result.txt";

            if (!Directory.Exists(imageFolder))
            {
                Console.WriteLine("❌ Không tìm thấy thư mục ảnh!");
                return;
            }

            try
            {
                using (var engine = new TesseractEngine(tessDataPath, "vie+eng", EngineMode.Default))
                using (var writer = new StreamWriter(outputFile, false, Encoding.UTF8))
                {
                    foreach (var imgPath in Directory.GetFiles(imageFolder, "*.jpg"))
                    {
                        Console.WriteLine($"🔍 Đang xử lý: {Path.GetFileName(imgPath)}");

                        using (var img = Pix.LoadFromFile(imgPath))
                        using (var page = engine.Process(img))
                        {
                            string text = page.GetText();
                            writer.WriteLine("===== " + Path.GetFileName(imgPath) + " =====");
                            writer.WriteLine(text.Trim());
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
```

---

## 🐞 Lỗi đã gặp & Cách khắc phục

| Lỗi                             | Nguyên nhân                                      | Giải pháp                                                  |
| ------------------------------- | ------------------------------------------------ | ---------------------------------------------------------- |
| `'tesseract' is not recognized` | Chưa thêm vào PATH hoặc sai đường dẫn `tessdata` | Thêm PATH hoặc dùng đường dẫn tuyệt đối                    |
| `Failed loading language 'vie'` | Thiếu `vie.traineddata`                          | Tải từ tessdata GitHub và bỏ vào `tessdata`                |
| Chữ bị nhận sai hoặc mất        | Font nghệ thuật, ảnh mờ, nền chữ phức tạp        | Thêm ngôn ngữ Tiếng Anh Và Việt giúp giảm thiểu độ sai sót |

---

## 🎯 Kết quả

- Tự động hóa việc OCR hàng loạt ảnh bìa sách,tuy nhiên 1 số bìa vẫn chưa nhận diện được đúng ,vẫn còn sai sót.
- Lưu vào file .txt
- Hỗ trợ song ngữ Việt + Anh.

---

## 💡 Kế hoạch nâng cấp

- Thêm **OpenCvSharp** để tiền xử lý ảnh trước khi OCR.
- Crop tự động vùng tiêu đề sách để tăng độ chính xác với chữ lớn.
- Hỗ trợ thêm PDF và các định dạng ảnh khác.

---

---
