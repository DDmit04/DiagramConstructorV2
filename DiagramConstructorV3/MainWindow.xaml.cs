using System;
using System.Windows;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using DiagramConstructorV3.language;

namespace DiagramConstructorV3
{
    public partial class MainWindow : Window
    {

        private readonly List<Language> allLanguages = new List<Language>();
        private Language chosenLang = null;
        public MainWindow()
        {
            InitializeComponent();
            closeAfterBuildCheckBox.IsEnabled = false;
            searchSaveFilderTextBox.Text = AppConfiguration.DefaultFilePath;
            LoadLanguages();
            chosenLang = allLanguages[0];
            chooseLangComboBox.ItemsSource = allLanguages.Select(lang => lang.DisplayName);
        }

        private void LoadLanguages()
        {
            IEnumerable<Type> languageSubclassTypes = Assembly.GetAssembly(typeof(Language)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Language)));
            foreach (Type languageSubclassType in languageSubclassTypes)
            {
                try
                {
                    allLanguages.Add((Language)Activator.CreateInstance(languageSubclassType));
                }
                catch (Exception exception)
                {
                    string logFilepath = ExceptionLogger.LogWarning(exception, "no code", $"Language subclass with name {languageSubclassType.FullName} hase no default constructor!");
                    showWarningMessage("Неполадка при инициализации программы!", $"Возникла ошибка при загрузке конфигурации языка {languageSubclassType.Name}! Вы можете продолжать пользоваться программой, но данный язык будет недоступен.", logFilepath);
                }
            }
        }

        private void FilepathTextBox_MouseDown(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = chosenLang.GetExtensionsFileConditions();
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

                if (filenames.Length == 1 && chosenLang.CheckExtensions(Path.GetExtension(filenames[0])))
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

                if (chosenLang.CheckExtensions(Path.GetExtension(filenames[0])))
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
            chosenLang = allLanguages.SingleOrDefault(lang => lang.DisplayName == newSelectedLang);
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

        private void CreateDiagramBtn_Click(object sender, RoutedEventArgs e)
        {
            var starter = new DiagramCreator(chosenLang);
            try
            {
                var filePathToSave = searchSaveFilderTextBox.Text;
                var codeToBuildDiagram = codeContentTextBox.Text;
                var finalDiagramFilePath = starter.CreateDiagram(codeToBuildDiagram, filePathToSave,
                    (bool)closeAfterBuildCheckBox.IsChecked);
                ShowInfoMessage("Диаграмма сохранена!", $"Путь до диаграммы - {finalDiagramFilePath}");
            }
            catch (Exception exception)
            {
                var logFilepath = ExceptionLogger.LogException(exception, codeContentTextBox.Text);
                ShowErrorMessage("Не удалось создать диаграмму", $"Ошибка построения диаграммы! {exception.Message}", logFilepath);
            }
            // catch (Exception exception)
            // {
            //     var logFilepath = ExceptionLogger.LogException(exception, codeContentTextBox.Text);
            //     ShowErrorMessage("Не удалось создать диаграмму", $"Ошибка построения диаграммы!", logFilepath);
            // }
        }

        private void SearchSaveFolderBtn_Click(object sender, RoutedEventArgs e)
        {
            var folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                searchSaveFilderTextBox.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void ShowInfoMessage(string title, string body)
        {
            System.Windows.MessageBox.Show(body, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ShowErrorMessage(string title, string body, string logFilepath = "")
        {
            if (logFilepath != "")
            {
                body += $" \nФайл с логами ошибки - {logFilepath}";
            }
            System.Windows.MessageBox.Show(body, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void showWarningMessage(string title, string body, string logFilepath = "")
        {
            if (logFilepath != "")
            {
                body += $" \nФайл с логами ошибки - {logFilepath}";
            }
            System.Windows.MessageBox.Show(body, title, MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
