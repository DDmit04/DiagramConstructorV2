using System;
using System.Windows;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using DiagramConsructorV2.src;
using DiagramConsructorV2.src.lang;
using DiagramConstructorV2.src.config;
using System.Reflection;
using DiagramConsructorV2.src.excaptions;

namespace DiagramConsructorV2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly List<Language> allLangs = new List<Language>();
        private Language choosenLang = null;
        public MainWindow()
        {
            InitializeComponent();
            closeAfterBuildCheckBox.IsEnabled = false;
            searchSaveFilderTextBox.Text = Configuration.defaultFilePath;
            LoadLanguages();
            choosenLang = allLangs[0];
            chooseLangComboBox.ItemsSource = allLangs.Select(lang => lang.displayName);
        }

        private void LoadLanguages()
        {
            IEnumerable<Type> languageSubclassTypes = Assembly.GetAssembly(typeof(Language)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Language)));
            foreach (Type languageSubclassType in languageSubclassTypes)
            {
                try
                {
                    allLangs.Add((Language)Activator.CreateInstance(languageSubclassType));
                }
                catch (Exception exception)
                {
                    string logFilepath = ExceptionLogger.logWarning(exception, "no code", $"Language subclass with name {languageSubclassType.FullName} hase no default constructor!");
                    showWarningMessage("Неполадка при инициализации программы!", $"Возникла ошибка при загрузке конфигурации языка {languageSubclassType.Name}! Вы можете продолжать пользоваться программой, но данный язык будет недоступен.", logFilepath);
                }
            }
        }

        private void filepathTextBox_MouseDown(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = choosenLang.getExtantionsFileConditions();
            openFileDialog.FilterIndex = 0;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string selectedFileName = openFileDialog.FileName;
                filepathTextBox.Text = "\n" + selectedFileName;
                codeContentTextBox.Text = File.ReadAllText(selectedFileName);
            }
        }

        private void useFileRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            codeContentTextBox.IsReadOnly = true;
            filepathTextBox.IsEnabled = true;
        }

        private void inputCodeRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            codeContentTextBox.IsReadOnly = false;
            codeContentTextBox.IsEnabled = true;
            filepathTextBox.IsEnabled = false;
        }

        private void filepathTextBox_PreviewDragOver(object sender, System.Windows.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop, true))
            {
                string[] filenames = e.Data.GetData(System.Windows.DataFormats.FileDrop, true) as string[];

                if (filenames.Length == 1 && choosenLang.checkExtantions(Path.GetExtension(filenames[0])))
                {
                    e.Effects = System.Windows.DragDropEffects.Copy;
                }
                else
                {
                    e.Effects = System.Windows.DragDropEffects.None;
                }
            }
            e.Handled = true;
        }

        private void filepathTextBox_PreviewDrop(object sender, System.Windows.DragEventArgs e)
        {
            bool dropEnabled = true;
            if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop, true))
            {
                string[] filenames = e.Data.GetData(System.Windows.DataFormats.FileDrop, true) as string[];

                if (choosenLang.checkExtantions(Path.GetExtension(filenames[0])))
                {
                    filepathTextBox.Text = "\n" + filenames[0];
                    codeContentTextBox.Text = File.ReadAllText(filenames[0]);
                }
                else
                {
                    dropEnabled = false;
                }
            }
            else
            {
                dropEnabled = false;
            }

            if (!dropEnabled)
            {
                e.Effects = System.Windows.DragDropEffects.None;
                e.Handled = true;
            }
        }

        private void chooseLangComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string newSelectedLang = e.AddedItems[0].ToString();
            choosenLang = allLangs.SingleOrDefault(lang => lang.displayName == newSelectedLang);
        }

        private void codeContentTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (codeContentTextBox.Text.Trim() == "")
            {
                closeAfterBuildCheckBox.IsEnabled = false;
                createDiagramBtn.IsEnabled = false;
                searchSaveFilderTextBox.IsEnabled = false;
                searchSaveFolderBtn.IsEnabled = false;
                searchSaveFilderLabel.Opacity = 100;
                closeAfterBuildCheckBox.Opacity = 100;
            }
            else
            {
                closeAfterBuildCheckBox.IsEnabled = true;
                createDiagramBtn.IsEnabled = true;
                searchSaveFilderTextBox.IsEnabled = true;
                searchSaveFolderBtn.IsEnabled = true;
                searchSaveFilderLabel.Opacity = 50;
                closeAfterBuildCheckBox.Opacity = 50;
            }
        }

        private void createDiagramBtn_Click(object sender, RoutedEventArgs e)
        {
            DiagramCreator starter = new DiagramCreator(choosenLang);
            try
            {
                string filePathToSave = searchSaveFilderTextBox.Text;
                string codeToBuildDiagram = codeContentTextBox.Text;
                string finalDiagramFilePath = starter.createDiagram(codeToBuildDiagram, filePathToSave, (bool)closeAfterBuildCheckBox.IsChecked);
                showInfoMessage($"Путь до диаграммы - {finalDiagramFilePath}", "Диаграмма сохранена!");
            }
            catch (Exception exception)
            {
                string logFilepath = ExceptionLogger.logException(exception, codeContentTextBox.Text);
                showErrorMessage("Не удалось создать диаграмму", $"Ошибка построения диаграммы!", logFilepath);
            }
        }

        private void searchSaveFolderBtn_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                searchSaveFilderTextBox.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void showInfoMessage(string title, string body)
        {
            System.Windows.MessageBox.Show(body, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void showErrorMessage(string title, string body, string logFilepath = "")
        {
            if (logFilepath != "")
            {
                body += $" Файл с логами ошибки - {logFilepath}";
            }
            System.Windows.MessageBox.Show(body, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void showWarningMessage(string title, string body, string logFilepath = "")
        {
            if (logFilepath != "")
            {
                body += $" Файл с логами ошибки - {logFilepath}";
            }
            System.Windows.MessageBox.Show(body, title, MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
