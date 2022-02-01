using System;
using System.Windows;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using DiagramConstructorV3.app.exceptions;
using DiagramConstructorV3.language;
using ICSharpCode.AvalonEdit.Highlighting;

namespace DiagramConstructorV3
{
    public partial class MainWindow : Window
    {
        private readonly List<Language> _allLanguages = new List<Language>();
        private Language _chosenLang = null;

        public MainWindow()
        {
            InitializeComponent();
            closeAfterBuildCheckBox.IsEnabled = false;
            SetDefaultDiagramSaveFilepath();
            LoadLanguages();
            _chosenLang = _allLanguages[0];
            chooseLangComboBox.ItemsSource = _allLanguages.Select(lang => lang.DisplayName);
            SetSyntaxHighlighting(_chosenLang.DisplayName);
        }

        public void SetSyntaxHighlighting(string langName)
        {
            var highlightingObj = HighlightingManager.Instance.GetDefinition(langName);
            if (highlightingObj == null)
            {
                highlightingObj = HighlightingManager.Instance.GetDefinition("C++");
                ShowWarningMessage("Ощибка определения языка для подсветки!",
                    $"Не найдена схема досветки для языка с именем '{langName}'! " +
                    "По умолчанию использована подсветка С++");
            }
            codeContentTextBox.SyntaxHighlighting = highlightingObj;
        }

        public void SetDefaultDiagramSaveFilepath()
        {
            searchSaveFilderTextBox.Text = "";
            var canUseDefaultDir = CheckDiagramSaveFilepath(AppConfiguration.DefaultFilePath);
            if (canUseDefaultDir)
            {
                searchSaveFilderTextBox.Text = AppConfiguration.DefaultFilePath;
            }
        }

        public bool CheckDiagramSaveFilepath(string path)
        {
            var canWrite = CheckLocationWritable(path);
            var isDirExists =  Directory.Exists(path);
            if (canWrite && isDirExists)
            {
                return true;
            }
            if (!canWrite)
            {
                ShowWarningMessage("Ошибка пути сохранения диаграмы!",
                    "У приложения недостаточно прав, чтобы сохранять файлы в данную директорию!" +
                    " Пожалуйста, выбирете другое мето для сохранения диаграм!");
            }
            if (!isDirExists)
            {
                ShowWarningMessage("Ошибка пути сохранения диаграмы!",
                    "Приложению не удалось найти указанную диркторию!" +
                    " Пожалуйста, выбирете другое мето для сохранения диаграм!");
            }

            return false;
        }

        public bool CheckLocationWritable(string path)
        {
            var permissionSet = new PermissionSet(PermissionState.None);
            try
            {
                var writePermission = new FileIOPermission(FileIOPermissionAccess.Write, path);
                permissionSet.AddPermission(writePermission);
                return permissionSet.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet);
            }
            catch
            {
                return false;
            }
        }
        
        public bool CheckLocationReadable(string path)
        {
            var permissionSet = new PermissionSet(PermissionState.None);
            try
            {
                var writePermission = new FileIOPermission(FileIOPermissionAccess.Read, path);
                permissionSet.AddPermission(writePermission);
                return permissionSet.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet);
            }
            catch
            {
                return false;
            }
        }

        private void LoadLanguages()
        {
            var languageSubclassTypes = Assembly.GetAssembly(typeof(Language)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Language)));
            foreach (var languageSubclassType in languageSubclassTypes)
            {
                try
                {
                    _allLanguages.Add((Language)Activator.CreateInstance(languageSubclassType));
                }
                catch (Exception exception)
                {
                    var logFilepath = ExceptionLogger.LogWarning(exception, "no code",
                        $"Language subclass with name {languageSubclassType.FullName} has no default constructor!");
                    ShowWarningMessage("Неполадка при инициализации программы!",
                        $"Возникла ошибка при загрузке конфигурации языка {languageSubclassType.Name}! Вы можете продолжать пользоваться программой, но данный язык будет недоступен.",
                        logFilepath);
                }
            }

            if (_allLanguages.Count == 0)
            {
                ShowErrorMessage("Ошибка загрузки приложения!", "Не удалось загрузить ни одну из конфигураций языков!");
                System.Windows.Application.Current.Shutdown();
            }
        }

        private void FilepathTextBox_MouseDown(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = _chosenLang.GetExtensionsFileConditions();
            openFileDialog.FilterIndex = 0;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var selectedFileName = openFileDialog.FileName;
                var canRead = CheckLocationReadable(selectedFileName);
                if (canRead)
                {
                    filepathTextBox.Text = "\n" + selectedFileName;
                    codeContentTextBox.Text = File.ReadAllText(selectedFileName);
                }
                else
                {
                    ShowWarningMessage("Ошибка чтения!", "У приложения недостаточно прав, чтобы прочитать данный файл!");
                }
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
                var filenames = e.Data.GetData(System.Windows.DataFormats.FileDrop, true) as string[];

                if (filenames != null && filenames.Length == 1 &&
                    _chosenLang.CheckExtensions(Path.GetExtension(filenames[0])))
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
            var dropEnabled = true;
            if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop, true))
            {
                var filenames = e.Data.GetData(System.Windows.DataFormats.FileDrop, true) as string[];

                if (filenames != null && _chosenLang.CheckExtensions(Path.GetExtension(filenames[0])))
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

        private void chooseLangComboBox_SelectionChanged(object sender,
            System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var newSelectedLang = e.AddedItems[0].ToString();
            _chosenLang = _allLanguages.SingleOrDefault(lang => lang.DisplayName == newSelectedLang);
            if (_chosenLang != null)
            {
                SetSyntaxHighlighting(_chosenLang.DisplayName);
            }
        }

        private void codeContentTextBox_TextChanged(object sender, EventArgs e)
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
            var starter = new DiagramCreator(_chosenLang);
            try
            {
                var filePathToSave = searchSaveFilderTextBox.Text;
                var codeToBuildDiagram = codeContentTextBox.Text;
                var creationProps = new DiagramCreateProps(codeToBuildDiagram, filePathToSave,
                    closeAfterBuildCheckBox.IsChecked != null && (bool)closeAfterBuildCheckBox.IsChecked);
                var finalDiagramFilePath = starter.CreateDiagram(creationProps);
                ShowInfoMessage("Диаграмма сохранена!", $"Путь до диаграммы - {finalDiagramFilePath}");
            }
            catch (LexException exception)
            {
                var logFilepath = ExceptionLogger.LogException(exception, codeContentTextBox.Text);
                ShowDiagramCreateErrorMessage(logFilepath, $"Не удалось распознать токен - '{exception.ErrorToken}'" +
                                                           $"\nНомер строки, в кторой возникла ошибка - {exception.ErrorLineNumber}");
            }
            catch (ParseException exception)
            {
                var logFilepath = ExceptionLogger.LogParseException(exception, codeContentTextBox.Text);
                ShowDiagramCreateErrorMessage(logFilepath, $"Ошибка парсинга для узла {exception.ParsedNodeType}!" +
                                                           $"\nНомер строки, в кторой возникла ошибка - {exception.ErrorLineNumber}");
            }
            catch (Exception exception)
            {
                var logFilepath = ExceptionLogger.LogException(exception, codeContentTextBox.Text);
                ShowDiagramCreateErrorMessage(logFilepath);
            }
        }

        private void SearchSaveFolderBtn_Click(object sender, RoutedEventArgs e)
        {
            var folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var canUseDir = CheckDiagramSaveFilepath(folderBrowserDialog.SelectedPath);
                if (canUseDir)
                {
                    searchSaveFilderTextBox.Text = folderBrowserDialog.SelectedPath;
                }
            }
        }

        private void ShowDiagramCreateErrorMessage(string logFilepath = "", string additionalInfo = "")
        {
            var body = "Не удалось создать диаграмму!";
            var title = "Ошибка построения диаграммы!";
            if (additionalInfo != "")
            {
                body += $" \nПодробности ошибки - {additionalInfo}";
            }

            ShowErrorMessage(title, body, logFilepath);
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

        private void ShowWarningMessage(string title, string body, string logFilepath = "")
        {
            if (logFilepath != "")
            {
                body += $" \nФайл с логами ошибки - {logFilepath}";
            }

            System.Windows.MessageBox.Show(body, title, MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}