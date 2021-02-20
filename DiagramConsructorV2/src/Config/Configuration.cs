﻿using DiagramConsructorV2.src.enumerated;
using System;

namespace DiagramConstructorV2.src.config
{
    class Configuration
    {
        /// <summary>
        /// Default filepath to save builded diagrams
        /// </summary>
        public static String defaultFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        /// <summary>
        /// Var to save custom user filepath to save builded diagrams
        /// </summary>
        public static String customFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        /// <summary>
        /// Filepath to work in program
        /// </summary>
        public static String finalFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        /// <summary>
        /// Use test code to run program
        /// </summary>
        public static TestRunType testRun = TestRunType.PROD;

        public static string shapesMastersFilePath = AppDomain.CurrentDomain.BaseDirectory + @"\Help\Shapes.vssx";
        public static string diagramFileExtention = ".vsdx";


    }
}
