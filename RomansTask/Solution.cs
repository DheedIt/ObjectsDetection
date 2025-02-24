using OpenCvSharp;
using System.Drawing;

namespace RomansTask
{
    public class Solution
    {
        static private void CreateContours(Mat imageClone,Mat templateClone)
        {
            Mat cannImage = imageClone;
            Mat cannyTemplate = templateClone;
            Cv2.Blur(cannyTemplate, cannyTemplate, new OpenCvSharp.Size(2, 2));
            Cv2.Blur(cannImage, cannImage, new OpenCvSharp.Size(2, 2));
            Cv2.Canny(cannyTemplate, cannyTemplate, 200, 255);
            Cv2.Canny(cannImage, cannImage, 200, 255);
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
            Mat metchResult = image.EmptyClone();
            CreateContours(image,template);
         
            while (countOfFoundTepmlates < limitOfCycles)
            {
                Cv2.MatchTemplate(image, template, metchResult, TemplateMatchModes.SqDiff);
                Cv2.MinMaxLoc(metchResult, out _, out _, out minLocOfTemplate, out maxLocOfTemplate, new Mat());
                matchLocOfTemplate = minLocOfTemplate;
                foundObject = new Mat(image, new OpenCvSharp.Rect(matchLocOfTemplate, template.Size()));
                Cv2.ImShow("found",foundObject);
                Cv2.ImShow("template",template);
                Cv2.ImShow("image",image);
                // Cv2.WaitKey();
                if (foundObject.CountNonZero() <= template.CountNonZero() * 0.80)
                {
                    if (timeOut == 10)
                        return countOfFoundTepmlates;
                    timeOut += 1;
                    Cv2.Rectangle(image, matchLocOfTemplate, new OpenCvSharp.Point(matchLocOfTemplate.X + templateCols, matchLocOfTemplate.Y + templateRows), new Scalar(0, 0, 0), -1, LineTypes.Link8, 0);
                }
                else
                {
                //Console.WriteLine(Cv2.Norm(template,foundObject,NormTypes.L3));
                    countOfFoundTepmlates += 1;
                    Cv2.Rectangle(image, matchLocOfTemplate, new OpenCvSharp.Point(matchLocOfTemplate.X + templateCols, matchLocOfTemplate.Y + templateRows), new Scalar(0, 0, 0), -1, LineTypes.Link8, 0);
                    timeOut = 0;
                }
            }
            return countOfFoundTepmlates;
        }
    }
}