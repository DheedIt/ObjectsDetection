using OpenCvSharp;
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
        public int ObjectsCount(string image, string item) {
            Mat map = new Mat(image, ImreadModes.Grayscale);
            Mat result = new Mat(image, ImreadModes.Color);
            Mat building = new Mat(item, ImreadModes.Grayscale);
            List< OpenCvSharp.Point > points = new List< OpenCvSharp.Point >();
            int resCols = map.Cols - building.Cols + 1;
            int resRows = map.Cols - building.Cols + 1;
            int count = 0;
            int route = 0;
            int stopSize = 0;
            OpenCvSharp.Point pt = new OpenCvSharp.Point();
            while (true)
            {
                Mat res = new Mat();
                res.Create(resRows, resCols, type: MatType.CV_32FC1);

                TemplateMatchModes method = TemplateMatchModes.SqDiff;

                Cv2.MatchTemplate(map, building, res, method);
                Cv2.Normalize(res, res, 0, 1, NormTypes.MinMax, -1, new Mat());
                double minVal = 0; double maxVal = 0; OpenCvSharp.Point minLoc = new OpenCvSharp.Point(); OpenCvSharp.Point maxLoc = new OpenCvSharp.Point();
                OpenCvSharp.Point matchLoc = new OpenCvSharp.Point();
                Cv2.MinMaxLoc(res, out minVal, out maxVal, out minLoc, out maxLoc, new Mat());

                if (method == TemplateMatchModes.SqDiff || method == TemplateMatchModes.SqDiffNormed)
                {
                    matchLoc = minLoc;
                }
                else
                { matchLoc = maxLoc; }


                if (route == 0)
                {
                    route += 1;
                    stopSize = matchLoc.X + building.Cols;
                }
                if (stopSize > matchLoc.X + building.Cols)
                    break;
                else if(pt.X == matchLoc.X && pt.Y == matchLoc.Y)
                    break;
                pt = matchLoc;
                if (points.Contains(pt))
                {
                    break;
                }
               
                points.Add(pt);

                count += 1;
                Cv2.Rectangle(map, matchLoc, new OpenCvSharp.Point(matchLoc.X + building.Cols, matchLoc.Y + building.Rows), new Scalar(255, 255, 255), 3, LineTypes.Link4, 0);
                Cv2.Rectangle(res, matchLoc, new OpenCvSharp.Point(matchLoc.X + building.Cols, matchLoc.Y + building.Rows),new Scalar(255,255,25), 3, LineTypes.Link4, 0);

                Cv2.ImShow("Map", map);
                Cv2.ImShow("Res", res);
                Cv2.WaitKey(0);
            }
            return count;
        }
    }
}
