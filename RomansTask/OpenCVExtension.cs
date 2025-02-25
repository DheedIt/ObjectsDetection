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
        image = Cv2.ImRead(imagePath, ImreadModes.Grayscale);
        template = Cv2.ImRead(templatePath, ImreadModes.Grayscale);
        image.ConvertTo(image, MatType.CV_8UC1);
        template.ConvertTo(template, MatType.CV_8UC1);
        if (image.Empty() || template.Empty())
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

        // Возможно создать новые матрицы (2 аргумент вызываемых методов)
        Cv2.Blur(image, image, new Size(2, 2));
        Cv2.Canny(image, image, 200, 255);
        Cv2.Blur(template, template, new Size(2, 2));
        Cv2.Canny(template, template, 200, 255);
        // -------------------------------------------------------------

        int countOfFoundTepmlates = 0;
        int timeOut = 0;
        while (timeOut != 10) //TODO: Продумать логику
        {
            Mat metchResult = image.EmptyClone();
            Cv2.MatchTemplate(image, template, metchResult, TemplateMatchModes.SqDiff); // поиск шаблонов
            Cv2.MinMaxLoc(metchResult, out _, out _, out Point minLocOfTemplate, out _); // поиск минимальной координаты, где находится нужный шаблон
            Mat foundObject = new(image, new Rect(minLocOfTemplate, template.Size())); // найденный шаблон
            if (foundObject.CountNonZero() > template.CountNonZero() * 0.80) // отсвеевание неподходящих шаблонов
            {
                countOfFoundTepmlates += 1;
                timeOut = 0;
            }
            else timeOut += 1;
            Cv2.Rectangle(image, minLocOfTemplate,
                new Point(minLocOfTemplate.X + template.Cols, minLocOfTemplate.Y + template.Rows),
                    new Scalar(0, 0, 0), -1, LineTypes.Link8, 0);
        }
        return countOfFoundTepmlates;
    }
}