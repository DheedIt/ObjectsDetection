namespace RomansTask;

public static class Program
{
    static int Main()
    {
		try
		{
            string? imagePath = "C:\\Users\\zhula\\source\\repos\\RomansTask\\RomansTask\\Pictures\\img1.png";
            string? templatePath = "C:\\Users\\zhula\\source\\repos\\RomansTask\\RomansTask\\Pictures\\img0001.png";
            if (!File.Exists(imagePath) || !File.Exists(templatePath))
                throw new FileNotFoundException("Не найден указанный файл");
            int count = OpenCVExtension.FindTemplatesCount(imagePath, templatePath);
            Console.WriteLine($"Objects count: {count}");
            return 0;
        }
		catch (Exception e)
		{
            Console.WriteLine($"ERROR: {e.Message}");
            return -1;
		}
    }
}


