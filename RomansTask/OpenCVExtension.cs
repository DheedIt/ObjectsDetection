using OpenCvSharp;

namespace RomansTask;

public static class OpenCVExtension
{
    /// <summary>
    /// Создание объектов Mat image и Mat template
    /// </summary>
    /// <param name="imagePath">Путь к изображению</param>
    /// <param name="templatePath">Путь к шаблону</param>
    /// <param name="image">Матрица изображения</param>
    /// <param name="template">Матрица шаблона</param>
    /// <returns>true - в случае успешной работы</returns>
    /// <exception cref="ArgumentException">Вызывается при не верном пути к файлу</exception>
    /// <exception cref="Exception">Вызывается если шаблон больше изображения</exception>
    private static bool TryGetImageTemplate(string imagePath, string templatePath, out Mat image, out Mat template)
    {
		image = Cv2.ImRead(imagePath);
		template = Cv2.ImRead(templatePath);

        if (template.Empty() || image.Empty())
            throw new ArgumentException("Invalid arguments.");
        if (image.Cols <= template.Cols || image.Rows <= template.Rows)
            throw new Exception("The size of the template must not exceed or match the size of the original template.");
        return true;
	}

	/// <summary>
	/// Поиск шаблонов в изображении
	/// </summary>
	/// <param name="imagePath">Путь к изображению</param>
	/// <param name="templatePath">Путь к шаблону</param>
	/// <returns>Возвращает количество найденных шаблонов в изображении</returns>
	/// <exception cref="ArgumentException">Вызывается при не верном пути к файлу</exception>
	/// <exception cref="Exception">Вызывается если шаблон больше изображения</exception>
	/// <exception cref="InvalidOperationException"></exception>
	public static int FindTemplatesCount(string imagePath, string templatePath)
    {
        if (!TryGetImageTemplate(imagePath, templatePath, out Mat image, out Mat template))
            throw new InvalidOperationException();
		// -------------------------------------------------------------
		// Возможно создать новые матрицы (2 аргумент вызываемых методов)

		//Cv2.GaussianBlur(image, image, new Size(5, 5), 0);
		//Cv2.GaussianBlur(template, template, new Size(5, 5), 0);
		////Cv2.Canny(image, image, 200, 255);
		//Cv2.Canny(template, template, 200, 255);


		int resultCols = image.Cols - template.Cols + 1;
		int resultRows = image.Rows - template.Rows + 1;

		Mat result = image.EmptyClone();
		Cv2.MatchTemplate(image, template, result, TemplateMatchModes.CCoeffNormed);
		//Cv2.MinMaxLoc(result, out double minVal, out double maxVal, out Point minLoc, out Point maxLoc);

		//Point matchTopLeft = maxLoc;
		//Rect matchRect = new Rect(matchTopLeft.X, matchTopLeft.Y, ColoredTemplate.Cols, ColoredTemplate.Rows);

		//Cv2.Rectangle(ColoredImage, matchRect, Scalar.Red, 2);

		//Cv2.ImWrite("match_single_result.jpg", ColoredImage);

		//Mat CopiedGrayImage = GrayImage.Clone();


		List<OpenCvSharp.Rect> foundTemplates = [];

        double threshold = 0.5;
		bool reakFlag = false;
		int i = 0;
		while (true)
        {
			Cv2.MinMaxLoc(result, out double minVal, out double maxVal, out Point minLoc, out Point maxLoc);
			if (maxVal < threshold)
				break;

            OpenCvSharp.Point TopLeftCorner = maxLoc;
            OpenCvSharp.Rect foundObject = new(TopLeftCorner.X, TopLeftCorner.Y, template.Cols, template.Rows);
			foreach (var Rectangle in foundTemplates)
			{
				if (Rectangle.IntersectsWith(foundObject))
				{
					reakFlag = true;
					break;
				}
			}
			if (reakFlag)
			{
				Cv2.Rectangle(image, foundObject, Scalar.Black, -1);
				result.SubMat(foundObject).SetTo(Scalar.Black);
				reakFlag = false;
				continue;
			}
			foundTemplates.Add(foundObject);
			Cv2.Rectangle(image, foundObject, Scalar.Black, -1);
			result.SubMat(foundObject).SetTo(Scalar.Black);
			//Cv2.PutText(image, $"{maxVal:F2} + {i}", new Point(foundObject.X, foundObject.Y), HersheyFonts.HersheySimplex, 0.5, Scalar.Red, 1);
			//Cv2.ImWrite($"Photos\\Gg{i}.png", image);
			++i;
		}
        return i;
    }

 
}