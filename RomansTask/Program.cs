namespace RomansTask;

public static class Program
{
    static int Main()
    {
		try
		{
            string? imagePath = Console.ReadLine();
            string? templatePath = Console.ReadLine();
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


