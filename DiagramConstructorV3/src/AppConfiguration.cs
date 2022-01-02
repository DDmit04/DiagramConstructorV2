using System;

namespace DiagramConstructorV3
{
    internal static class AppConfiguration
    {
        public static readonly string DefaultFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        public static readonly string LogFolder = AppDomain.CurrentDomain.BaseDirectory + "logs\\";
        public static readonly string LogFileExt = ".txt";

        public static readonly string ShapesMastersFilePath = AppDomain.CurrentDomain.BaseDirectory + @"\Help\Shapes.vss";
        public static readonly string DiagramFileExtension = ".vsdx";

    }
}
