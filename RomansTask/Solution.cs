using OpenCvSharp;
using OpenCvSharp.Internal.Vectors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomansTask
{
    public class Solution
    {
        public int ObjectsCount(string img, string item, int limit)
        {
            if (img == null || item == null) throw new Exception("Empty params!");
            Mat image = new Mat(img.ToString(), ImreadModes.Grayscale);
            HierarchyIndex[] hz;
            OpenCvSharp.Point[][] vec;
            Cv2.Blur(image, image, new OpenCvSharp.Size(4, 4));
            //Cv2.Canny(image, image, 0.09, 255);
            //Cv2.FindContours(image, out vec, out hz, RetrievalModes.Tree, ContourApproximationModes.ApproxNone);
            //for (int i = 0; i < vec.Length; ++i)
            //{
            //    Cv2.DrawContours(image, vec, i, new Scalar(255, 255, 255));
            //}
            Mat result = new Mat(img, ImreadModes.Color);
            Mat template = new Mat(item, ImreadModes.Grayscale);
            if (image.Cols <= template.Cols || image.Rows <= template.Rows)
                throw new Exception("The size of the template must not exceed or match the size of the original template.");
            List<OpenCvSharp.Point> points = new List<OpenCvSharp.Point>();
            int resCols = image.Cols - template.Cols + 1;
            int resRows = image.Cols - template.Cols + 1;
            int count = 0;
            double chance = 0;
            Mat MTemplate = new Mat();
            OpenCvSharp.Point pt = new OpenCvSharp.Point();
            OpenCvSharp.Point matchLoc = new OpenCvSharp.Point();
            Mat res = new Mat();
            double minVal = 0; double maxVal = 0; OpenCvSharp.Point minLoc = new OpenCvSharp.Point(); OpenCvSharp.Point maxLoc = new OpenCvSharp.Point();
            TemplateMatchModes method = TemplateMatchModes.SqDiffNormed;
            Mat templateContours = template.Clone();
            Cv2.Blur(templateContours, templateContours, new OpenCvSharp.Size(4, 4));
            for(int j = 0; j < limit; j++)
            {
                Cv2.MatchTemplate(image, templateContours, res, method);
                Cv2.Normalize(res, res, 1, 0, NormTypes.MinMax, -1, new Mat());
                Cv2.MinMaxLoc(res, out minVal, out maxVal, out minLoc, out maxLoc, new Mat());
                matchLoc.X = 0;
                matchLoc.Y = 0;
                if (method == TemplateMatchModes.SqDiff || method == TemplateMatchModes.SqDiffNormed)
                {
                    matchLoc = minLoc;
                }
                else
                { matchLoc = maxLoc; }
                Mat foundObject = new Mat(image.Clone(), new OpenCvSharp.Rect(matchLoc, template.Size()));
                if (count == 0)
                    chance = Cv2.Norm(templateContours, foundObject);
                else if (pt.X == matchLoc.X && pt.Y == matchLoc.Y)
                    break;
                pt = matchLoc;

                for (int i = 0; i < points.Count - 1; i += 2)
                {
                    if ((points[i].X < matchLoc.X && matchLoc.X < points[i + 1].X) &&
                    (points[i].Y < matchLoc.Y && matchLoc.Y < points[i + 1].Y))
                        return count;                    
                }
                points.Add(pt);
                points.Add(new OpenCvSharp.Point(matchLoc.X + template.Cols, matchLoc.Y + template.Rows));
                Console.WriteLine((Cv2.Norm(templateContours, foundObject) - chance) + $" - { count }");
                Cv2.Rectangle(image, matchLoc, new OpenCvSharp.Point(matchLoc.X + template.Cols, matchLoc.Y + template.Rows), new Scalar(0, 0, 0), -1, LineTypes.Link8, 0);
                Cv2.Rectangle(result, matchLoc, new OpenCvSharp.Point(matchLoc.X + template.Cols, matchLoc.Y + template.Rows), new Scalar(100, 100, 250), 1, LineTypes.Link8, 0);
                Cv2.Rectangle(res, matchLoc, new OpenCvSharp.Point(matchLoc.X + template.Cols, matchLoc.Y + template.Rows), new Scalar(0, 0, 0), -1, LineTypes.Link8, 0);
                count += 1;
            }
            return count;
        }
    }
}
