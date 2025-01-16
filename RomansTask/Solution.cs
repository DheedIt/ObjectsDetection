using OpenCvSharp;
using System.Drawing;

namespace RomansTask
{
    public class Solution
    {
        static private Mat CreateContours(ref Mat imageClone, ref Mat templateClone)
        {
            Mat resultImage = new Mat(imageClone.Size(), imageClone.Type());
            Mat cannImage = imageClone.Clone();
            Mat cannyTemplate = templateClone.Clone();
            Cv2.Blur(cannyTemplate, cannyTemplate, new OpenCvSharp.Size(2, 2));
            Cv2.Blur(cannImage, cannImage, new OpenCvSharp.Size(2, 2));
            Cv2.Canny(cannyTemplate, cannyTemplate, 200, 255);
            Cv2.Canny(cannImage, cannImage, 200, 255);
            templateClone = cannyTemplate;
            imageClone = cannImage;
            return resultImage;
        }
        static public int ObjectsCount(string img, string item, int limitOfCycles)
        {
            if (img == null || item == null) throw new Exception("Empty params!");
            Mat image = Cv2.ImRead(img, ImreadModes.Grayscale);
            image.ConvertTo(image, MatType.CV_8UC1);
            Mat template = Cv2.ImRead(item, ImreadModes.Grayscale);
            template.ConvertTo(template, MatType.CV_8UC1);
            int templateRows = template.Rows, templateCols = template.Cols;
            if (image.Cols <= templateCols || image.Rows <= templateRows)
                throw new Exception("The size of the template must not exceed or match the size of the original template.");
            int countOfFoundTepmlates = 0;
            int timeOut = 0;
            OpenCvSharp.Point minLocOfTemplate, maxLocOfTemplate, matchLocOfTemplate;
            Mat foundObject;
            Mat cloneOfTemplateImage = template;
            Mat cloneOfMainImage = image;
            Mat resultImage = CreateContours(ref cloneOfMainImage, ref cloneOfTemplateImage);
            while (countOfFoundTepmlates <limitOfCycles) {
                Cv2.MatchTemplate(cloneOfMainImage, cloneOfTemplateImage, resultImage, TemplateMatchModes.SqDiff);
                Cv2.MinMaxLoc(resultImage, out _, out _, out minLocOfTemplate, out maxLocOfTemplate, new Mat());
                matchLocOfTemplate = minLocOfTemplate;
                foundObject = new Mat(cloneOfMainImage, new OpenCvSharp.Rect(matchLocOfTemplate, cloneOfTemplateImage.Size()));
                if (foundObject.CountNonZero() <= cloneOfTemplateImage.CountNonZero() * 0.80) {
                    if (timeOut == 10)
                        return countOfFoundTepmlates;
                    timeOut += 1;
                    Cv2.Rectangle(cloneOfMainImage, matchLocOfTemplate, new OpenCvSharp.Point(matchLocOfTemplate.X + templateCols, matchLocOfTemplate.Y + templateRows), new Scalar(0, 0, 0), -1, LineTypes.Link8, 0);
                }
                else {
                    countOfFoundTepmlates += 1;
                    Cv2.Rectangle(cloneOfMainImage, matchLocOfTemplate, new OpenCvSharp.Point(matchLocOfTemplate.X + templateCols, matchLocOfTemplate.Y + templateRows), new Scalar(0, 0, 0), -1, LineTypes.Link8, 0);
                    timeOut = 0;
                }
            }
            return countOfFoundTepmlates;
        }
        static public int ObjectsCount(string img, string item)
        {
            if (img == null || item == null) throw new Exception("Empty params!");
            Mat image = Cv2.ImRead(img, ImreadModes.Grayscale);
            image.ConvertTo(image, MatType.CV_8UC1);
            Mat template = Cv2.ImRead(item, ImreadModes.Grayscale);
            template.ConvertTo(template, MatType.CV_8UC1);
            int templateRows = template.Rows, templateCols = template.Cols;
            if (image.Cols <= templateCols || image.Rows <= templateRows)
                throw new Exception("The size of the template must not exceed or match the size of the original template.");
            int countOfFoundTepmlates = 0;
            int timeOut = 0;
            OpenCvSharp.Point minLocOfTemplate, maxLocOfTemplate, matchLocOfTemplate;
            Mat foundObject;
            Mat cloneOfTemplateImage = template;
            Mat cloneOfMainImage = image;
            Mat resultImage = CreateContours(ref cloneOfMainImage, ref cloneOfTemplateImage);
            while (true) {
                Cv2.MatchTemplate(cloneOfMainImage, cloneOfTemplateImage, resultImage, TemplateMatchModes.SqDiff);
                Cv2.MinMaxLoc(resultImage, out _, out _, out minLocOfTemplate, out maxLocOfTemplate, new Mat());
                matchLocOfTemplate = minLocOfTemplate;

                foundObject = new Mat(cloneOfMainImage, new OpenCvSharp.Rect(matchLocOfTemplate, cloneOfTemplateImage.Size()));
                if (foundObject.CountNonZero() <= cloneOfTemplateImage.CountNonZero() * 0.80) {
                    if (timeOut == 10)
                        return countOfFoundTepmlates;
                    timeOut += 1;
                    Cv2.Rectangle(cloneOfMainImage, matchLocOfTemplate, new OpenCvSharp.Point(matchLocOfTemplate.X + templateCols, matchLocOfTemplate.Y + templateRows), new Scalar(0, 0, 0), -1, LineTypes.Link8, 0);
                }
                else
                {
                    countOfFoundTepmlates += 1;
                    Cv2.Rectangle(cloneOfMainImage, matchLocOfTemplate, new OpenCvSharp.Point(matchLocOfTemplate.X + templateCols, matchLocOfTemplate.Y + templateRows), new Scalar(0, 0, 0), -1, LineTypes.Link8, 0);
                    timeOut = 0;
                }
            }
        }
        static public int ObjectsCount(string img, string item, int limitOfCycles, bool showDifferenceBetweenTemplates)
        {
            if (img == null || item == null) throw new Exception("Empty params!");
            Mat image = Cv2.ImRead(img, ImreadModes.Grayscale);
            image.ConvertTo(image, MatType.CV_8UC1);
            Mat template = Cv2.ImRead(item, ImreadModes.Grayscale);
            template.ConvertTo(template, MatType.CV_8UC1);
            int templateRows = template.Rows, templateCols = template.Cols;
            if (image.Cols <= templateCols || image.Rows <= templateRows)
                throw new Exception("The size of the template must not exceed or match the size of the original template.");
            int countOfFoundTepmlates = 0;
            int timeOut = 0;
            OpenCvSharp.Point minLocOfTemplate, matchLocOfTemplate;
            Mat foundObject;
            Mat cloneOfTemplateImage = template;
            Mat cloneOfMainImage = image;
            Mat resultImage = CreateContours(ref cloneOfMainImage, ref cloneOfTemplateImage);
            while (countOfFoundTepmlates < limitOfCycles) {            
                Cv2.MatchTemplate(cloneOfMainImage, cloneOfTemplateImage, resultImage, TemplateMatchModes.SqDiff);
                Cv2.MinMaxLoc(resultImage, out _, out _, out minLocOfTemplate, out _, new Mat());
                matchLocOfTemplate = minLocOfTemplate;
                foundObject = new Mat(cloneOfMainImage, new OpenCvSharp.Rect(matchLocOfTemplate, cloneOfTemplateImage.Size()));
                if (foundObject.CountNonZero() <= cloneOfTemplateImage.CountNonZero() * 0.80) {
                    if (timeOut == 10)
                        return countOfFoundTepmlates;
                    timeOut += 1;
                    Cv2.Rectangle(cloneOfMainImage, matchLocOfTemplate, new OpenCvSharp.Point(matchLocOfTemplate.X + templateCols, matchLocOfTemplate.Y + templateRows), new Scalar(0, 0, 0), -1, LineTypes.Link8, 0);
                }
                else {                 
                    Console.WriteLine(Cv2.Norm(cloneOfTemplateImage, foundObject,NormTypes.L2));
                    countOfFoundTepmlates += 1;
                    Cv2.Rectangle(cloneOfMainImage, matchLocOfTemplate, new OpenCvSharp.Point(matchLocOfTemplate.X + templateCols, matchLocOfTemplate.Y + templateRows), new Scalar(0, 0, 0), -1, LineTypes.Link8, 0);
                    timeOut = 0;
                }
            }
            return countOfFoundTepmlates;
        }
    }
}

