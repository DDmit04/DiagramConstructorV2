using System;
using System.Windows;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using DiagramConsructorV2.fileTypes;
using DiagramConsructorV2.src;
using DiagramConstructor;
using System.Diagnostics;
using DiagramConsructorV2.src.Config;

namespace DiagramConsructorV2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private List<Language> allLangs = new List<Language>(){ new PytonLanguage(), new CppLanguage() };
        private Language choosenLang = null;
        public MainWindow()
        {
            choosenLang = allLangs[0];
            InitializeComponent();
            closeAfterBuildCheckBox.IsEnabled = false;
            searchSaveFilderTextBox.Text = Configuration.defaultFilePath;
            chooseLangComboBox.ItemsSource = allLangs.Select(lang => lang.displayName);
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
                System.Windows.MessageBox.Show($"Путь до диаграммы - {finalDiagramFilePath}", "Диаграмма сохранена!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception exception)
            {
                string logFilepath = ExceptionLogger.logException(exception);
                if (logFilepath != "")
                {
                    showErrorMessage("Не удалось создать диаграмму", $"Ошибка построения диаграммы! Файл с логами ошибки - {logFilepath}");
                }
                else
                {
                    showErrorMessage("Не удалось создать диаграмму", "Ошибка построения диаграммы! В процессе записи логов возникла ошибка!");
                }
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

        private void showErrorMessage(string title, string body)
        {
            System.Windows.MessageBox.Show(body, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
