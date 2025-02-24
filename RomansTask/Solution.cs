using OpenCvSharp;
using System.Drawing;

namespace RomansTask
{
    public class Solution
    {
        static private void CreateContours(Mat imageClone,Mat templateClone) // обработка фотографий
        {
            Cv2.Blur(templateClone, templateClone, new OpenCvSharp.Size(2, 2)); // Блюр
            Cv2.Blur(imageClone, imageClone, new OpenCvSharp.Size(2, 2));
            Cv2.Canny(templateClone, templateClone, 200, 255); // Поиск краев на фото
            Cv2.Canny(imageClone, imageClone, 200, 255);
        }
        static public int ObjectsCount(string img, string item, int limitOfCycles)
        {
            if (img == null || item == null) throw new Exception("Empty params!");
            Mat image = Cv2.ImRead(img, ImreadModes.Grayscale); // считывание фотографии и перевод в черно белый цвет
            image.ConvertTo(image, MatType.CV_8UC1); // перевод фотографии в 8 битный формат
            Mat template = Cv2.ImRead(item, ImreadModes.Grayscale);
            template.ConvertTo(template, MatType.CV_8UC1);
            int templateRows = template.Rows, templateCols = template.Cols;
            if (image.Cols <= templateCols || image.Rows <= templateRows)
                throw new Exception("The size of the template must not exceed or match the size of the original template.");
            int countOfFoundTepmlates = 0;
            int timeOut = 0;

            OpenCvSharp.Point minLocOfTemplate;
            Mat foundObject;
            Mat metchResult = image.EmptyClone(); // пустой шаблон чтоб записать туда результат поиска шаблона (картинка имет спецефичный вид по этому нужна отдельная переменная)
            CreateContours(image,template);
         
            while (countOfFoundTepmlates < limitOfCycles)
            {
                Cv2.MatchTemplate(image, template, metchResult, TemplateMatchModes.SqDiff); // поиск шаблонов
                Cv2.MinMaxLoc(metchResult, out _, out _, out minLocOfTemplate, out _, new Mat()); // поиск минимальной координаты, где находится нужный шаблон
                foundObject = new Mat(image, new OpenCvSharp.Rect(minLocOfTemplate, template.Size())); // найденный шаблон
                // Cv2.ImShow("found",foundObject);
                // Cv2.ImShow("template",template);
                // Cv2.ImShow("image",image);
                // Cv2.WaitKey();
                if (foundObject.CountNonZero() <= template.CountNonZero() * 0.80) // отсвеевание неподходящих шаблонов
                {
                    if (timeOut == 10)
                        return countOfFoundTepmlates;
                    timeOut += 1;
                    Cv2.Rectangle(image, minLocOfTemplate, 
                        new OpenCvSharp.Point(minLocOfTemplate.X + templateCols, minLocOfTemplate.Y + templateRows),
                         new Scalar(0, 0, 0), -1, LineTypes.Link8, 0); // Закрашивание найденного шаблона
                }
                else
                {
                //Console.WriteLine(Cv2.Norm(template,foundObject,NormTypes.L3));
                    countOfFoundTepmlates += 1;
                    Cv2.Rectangle(image, minLocOfTemplate, 
                        new OpenCvSharp.Point(minLocOfTemplate.X + templateCols, minLocOfTemplate.Y + templateRows), 
                            new Scalar(0, 0, 0), -1, LineTypes.Link8, 0);
                    timeOut = 0;
                }
            }
            return countOfFoundTepmlates;
        }
    }
}