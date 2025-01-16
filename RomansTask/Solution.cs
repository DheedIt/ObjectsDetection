using OpenCvSharp;
using System.Drawing;

namespace RomansTask
{
    public class Solution
    {
        static private Mat CreateContours(ref Mat imageClone, ref Mat templateClone)
        {
            Mat resultImage = new Mat(imageClone.Size(), imageClone.Type());

            Mat hiearachy = new();
            Mat hiearachy1 = new();
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
         
            while (countOfFoundTepmlates < limitOfCycles)
            {
                Cv2.MatchTemplate(cloneOfMainImage, cloneOfTemplateImage, resultImage, TemplateMatchModes.SqDiff);
                Cv2.MinMaxLoc(resultImage, out _, out _, out minLocOfTemplate, out maxLocOfTemplate, new Mat());
                matchLocOfTemplate = minLocOfTemplate;
                
                foundObject = new Mat(cloneOfMainImage, new OpenCvSharp.Rect(matchLocOfTemplate, cloneOfTemplateImage.Size()));
                if (foundObject.CountNonZero() <= cloneOfTemplateImage.CountNonZero() * 0.80)
                {
                    if (timeOut == 10)
                        return countOfFoundTepmlates;
                    timeOut += 1;
                    Cv2.Rectangle(cloneOfMainImage, matchLocOfTemplate, new OpenCvSharp.Point(matchLocOfTemplate.X + templateCols, matchLocOfTemplate.Y + templateRows), new Scalar(0, 0, 0), -1, LineTypes.Link8, 0);
                }
                else
                {
                //Console.WriteLine(Cv2.Norm(cloneOfTemplateImage,foundObject,NormTypes.L3));
                    countOfFoundTepmlates += 1;
                    Cv2.Rectangle(cloneOfMainImage, matchLocOfTemplate, new OpenCvSharp.Point(matchLocOfTemplate.X + templateCols, matchLocOfTemplate.Y + templateRows), new Scalar(0, 0, 0), -1, LineTypes.Link8, 0);
                    timeOut = 0;
                }
            }
            return countOfFoundTepmlates;
        }
    }
}
/*
        public int ObjectsCount(string img, string item)
        {
            if (img == null || item == null) throw new Exception("Empty params!");
            int countOfFoundTepmlates = 0;
            double minValOfLocation = 0, maxValOfLocation = 0;
            Mat image = new(img.ToString(), ImreadModes.Grayscale);
            Mat template = new(item, ImreadModes.Grayscale);
            int templateRows = template.Rows, templateCols = template.Cols;
            if (image.Cols <= templateCols || image.Rows <= templateRows)
                throw new Exception("The size of the template must not exceed or match the size of the original template.");
            Mat MTemplate = new(), resultImage = new();
            Mat templateContours = template.Clone();
            OpenCvSharp.Point minLocOfTemplate = new(), maxLocOfTemplate = new(), pointOfActiualPoint = new(), matchLocOfTemplate = new();
            TemplateMatchModes methodOfImageComparing = TemplateMatchModes.SqDiffNormed;
            List<OpenCvSharp.Point> listOfPreviousPointsOfLocation = new();
                Cv2.MatchTemplate(image, templateContours, resultImage, methodOfImageComparing);
                Cv2.Normalize(resultImage, resultImage, 1, 0, NormTypes.MinMax, -1, new Mat());
           while (true) {
            
                matchLocOfTemplate.X = 0;
                matchLocOfTemplate.Y = 0;
                Cv2.MinMaxLoc(resultImage, out minValOfLocation, out maxValOfLocation, out minLocOfTemplate, out maxLocOfTemplate, new Mat());
                if (methodOfImageComparing == TemplateMatchModes.SqDiff || methodOfImageComparing == TemplateMatchModes.SqDiffNormed)
                    matchLocOfTemplate = minLocOfTemplate;
                else
                    matchLocOfTemplate = maxLocOfTemplate;
                if (pointOfActiualPoint.X == matchLocOfTemplate.X && pointOfActiualPoint.Y == matchLocOfTemplate.Y)
                    break;
                pointOfActiualPoint = matchLocOfTemplate;
                for (int i = 0; i < listOfPreviousPointsOfLocation.Count - 1; i += 2)
                    if (listOfPreviousPointsOfLocation[i].X < matchLocOfTemplate.X && matchLocOfTemplate.X < listOfPreviousPointsOfLocation[i + 1].X &&
                    listOfPreviousPointsOfLocation[i].Y < matchLocOfTemplate.Y && matchLocOfTemplate.Y < listOfPreviousPointsOfLocation[i + 1].Y)
                        return countOfFoundTepmlates;
                listOfPreviousPointsOfLocation.Add(pointOfActiualPoint);
                listOfPreviousPointsOfLocation.Add(new OpenCvSharp.Point(matchLocOfTemplate.X + templateCols, matchLocOfTemplate.Y + templateRows));
                countOfFoundTepmlates += 1;
                Cv2.Rectangle(image, matchLocOfTemplate, new OpenCvSharp.Point(matchLocOfTemplate.X + templateCols, matchLocOfTemplate.Y + templateRows), new Scalar(0, 0, 0), -1, LineTypes.Link8, 0);                
            }
            return countOfFoundTepmlates;
        }
        public int ObjectsCount(string img, string item, int limitOfCycles, bool showDifferenceBetweenTemplates)
        {
            if (img == null || item == null) throw new Exception("Empty params!");
            int countOfFoundTepmlates = 0;
            double minValOfLocation = 0, maxValOfLocation = 0, chance = 0;
            Mat image = new(img.ToString(), ImreadModes.Grayscale);
            Mat template = new(item, ImreadModes.Grayscale);
            int templateRows = template.Rows, templateCols = template.Cols;
            if (image.Cols <= templateCols || image.Rows <= templateRows)
                throw new Exception("The size of the template must not exceed or match the size of the original template.");
            Mat MTemplate = new(), resultImage = new();
            OpenCvSharp.Point minLocOfTemplate = new(), maxLocOfTemplate = new(), pointOfActiualPoint = new(), matchLocOfTemplate = new();
            TemplateMatchModes methodOfImageComparing = TemplateMatchModes.SqDiff;
            List<OpenCvSharp.Point> listOfPreviousPointsOfLocation = new();
            Mat[] contoursOfMainImage;
            Mat[] contoursOfTemplateImage;
            Mat hierachy = new();
            Mat hierachy1 = new();
            Mat ni = new(image.Size(), image.Type());
            Mat nt = new((template.Size()), template.Type());
            Mat cn = template.Clone();
            Cv2.Canny(cn, nt, 0.00000006, 0.5);
            //Cv2.Canny(image, ni, 100, 255);
            //Cv2.Blur(ntc, ntc, new OpenCvSharp.Size(3, 3));
            //Cv2.Blur(nic, nic, new OpenCvSharp.Size(3, 3));
            Cv2.FindContours(image, out contoursOfMainImage, hierachy, RetrievalModes.List, ContourApproximationModes.ApproxSimple);
            Cv2.FindContours(cn, out contoursOfTemplateImage, hierachy1, RetrievalModes.List, ContourApproximationModes.ApproxSimple);
            for (int i = 0; i < contoursOfMainImage.Length; i++)
            {
                Cv2.DrawContours(ni, contoursOfMainImage, i, new Scalar(255, 255, 255));
            }
            for (int i = 0; i < contoursOfTemplateImage.Length; i++)
            {
                Cv2.DrawContours(nt, contoursOfTemplateImage, i, new Scalar(255, 255, 255));
            }


            for (int j = 0; j < limitOfCycles; j++)
            {
                Cv2.MatchTemplate(ni, nt, resultImage, methodOfImageComparing);
                //Cv2.Normalize(resultImage, resultImage, 1, 0, NormTypes.MinMax, -1, new Mat());
                Cv2.MinMaxLoc(resultImage, out minValOfLocation, out maxValOfLocation, out minLocOfTemplate, out maxLocOfTemplate, new Mat());
                if (methodOfImageComparing == TemplateMatchModes.SqDiff || methodOfImageComparing == TemplateMatchModes.SqDiffNormed)
                    matchLocOfTemplate = minLocOfTemplate;
                else
                    matchLocOfTemplate = maxLocOfTemplate;
                if (pointOfActiualPoint.X == matchLocOfTemplate.X && pointOfActiualPoint.Y == matchLocOfTemplate.Y)
                    break;
                pointOfActiualPoint = matchLocOfTemplate;
                for (int i = 0; i < listOfPreviousPointsOfLocation.Count - 1; i += 2)
                    if (listOfPreviousPointsOfLocation[i].X < matchLocOfTemplate.X && matchLocOfTemplate.X < listOfPreviousPointsOfLocation[i + 1].X &&
                    listOfPreviousPointsOfLocation[i].Y < matchLocOfTemplate.Y && matchLocOfTemplate.Y < listOfPreviousPointsOfLocation[i + 1].Y)
                        return countOfFoundTepmlates;
                listOfPreviousPointsOfLocation.Add(pointOfActiualPoint);
                listOfPreviousPointsOfLocation.Add(new OpenCvSharp.Point(matchLocOfTemplate.X + nt.Cols, matchLocOfTemplate.Y + nt.Rows));
                countOfFoundTepmlates += 1;
                Cv2.Rectangle(ni, matchLocOfTemplate, new OpenCvSharp.Point(matchLocOfTemplate.X + nt.Cols, matchLocOfTemplate.Y + nt.Rows), new Scalar(0, 0, 0), -1, LineTypes.Link8, 0);
                Cv2.Rectangle(image, matchLocOfTemplate, new OpenCvSharp.Point(matchLocOfTemplate.X + nt.Cols, matchLocOfTemplate.Y + nt.Rows), new Scalar(0, 0, 0), -1, LineTypes.Link8, 0);
                Cv2.ImShow("ni", ni);
                //Cv2.ImShow("ni", ni); 
                Cv2.ImShow("img", image);
                Cv2.ImShow("t", nt);

                Mat foundObject = new Mat(ni.Clone(), new OpenCvSharp.Rect(matchLocOfTemplate, template.Size()));
                if(countOfFoundTepmlates == 0) chance = Cv2.Norm(nt, foundObject);

                Console.WriteLine((Cv2.Norm(nt, foundObject) - chance) + $" - {countOfFoundTepmlates}");
                matchLocOfTemplate.X = 0;
                matchLocOfTemplate.Y = 0;

            }
            return countOfFoundTepmlates;
        }
        public int ObjectsCount(string img, string item, string v)
        {
            if (img == null || item == null) throw new Exception("Empty params!");
            int countOfFoundTepmlates = 0;
            double minValOfLocation = 0, maxValOfLocation = 0;
            Mat image = new(img.ToString(), ImreadModes.Grayscale);
            Mat template = new(item, ImreadModes.Grayscale);
            int templateRows = template.Rows, templateCols = template.Cols;
            if (image.Cols <= templateCols || image.Rows <= templateRows)
                throw new Exception("The size of the template must not exceed or match the size of the original template.");
            Mat MTemplate = new(), resultImage = new();
            Mat templateContours = template.Clone();
            OpenCvSharp.Point minLocOfTemplate = new(), maxLocOfTemplate = new(), pointOfActiualPoint = new(), matchLocOfTemplate = new();
            TemplateMatchModes methodOfImageComparing = TemplateMatchModes.SqDiffNormed;
            List<OpenCvSharp.Point> listOfPreviousPointsOfLocation = new();
            Mat[] contoursOfMainImage;
            Mat hierachy = new();
            
            Cv2.FindContours(image, out contoursOfMainImage, hierachy, RetrievalModes.List,ContourApproximationModes.ApproxSimple);
            Mat newImage = new(image.Size(), image.Type());
            for(int i = 0; i < contoursOfMainImage.Length; i++)
            {
                Cv2.DrawContours(newImage, contoursOfMainImage, i, new Scalar(100, 100, 255));
            }
            Cv2.ImShow("ct", newImage);
            Cv2.WaitKey();
            return countOfFoundTepmlates;
        }
    }
*/

