   M S B A M L   `   `   `  ���� N  JDiagramConsructorV2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:DiagramConsructorV2DiagramConsructorV2  $    DiagramConsructorV2.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     3local!clr-namespace:DiagramConsructorV2       � Title$  Diagram constructor��$��617.833��$��582.833�� � Icon$% Help/diagramConstructorIcon.ico��.��� $��0,0,0,2q� � RowDefinitions �� $
��475* ��� $	��79* ��� $	��39* ��� $	��58* � � ColumnDefinitions �� $
��365* ��� $
��223* ��� $
��114* �.+��� -    ��Name$ closeAfterBuildCheckBox��$X��RЗакрыть документ диаграммы после построения��$��10,0,0,12.667q�$��14(�$��2��{�.   d� IsThreeState .  $��18��$��Bottom=� J� Opacity$	 0.5Q�$
��Left=�$	��342��$��2���� -    ��Name$ filepathTextBox��$��90��$��
10,88,21,0q� �� TextWrapping$
 Wrap=�$	��Top=�	 J� 	AllowDrop	 . 
 � Cursor$
 Arrow��$m��g
 Перетащите нужный файл или кликните для выбора вручную��$��18(�$	 0.5Q� �� TextAlignment$ Center=� �� AcceptsReturn . $��3���� -   $ codeContentTextBox��$��10,225,21,10.667q�$
 Wrap=� . " �� VerticalScrollBarVisibility$ Visible=� �� 
IsReadOnly . $��3��{�.   �� 
AcceptsTab . 	� -    	�Name$ useFileRadioBtn��$��Выбрать файл��$
��Left=�$��	10,62,0,0q�$	��Top=�$	��110�� J� RenderTransformOrigin$ 0.489,0.534&� d� 	IsChecked$
 TrueY�$��14(�$��18��	� -   $ inputCodeRadioBtn��$��Ввести код��$
��Left=�$��
10,199,0,0q�$	��Top=�$	��101��$ 
0.465,0.45&�$��14(�$��18���� -    ��Name$ createDiagramBtn��$%��Создать диаграму��$��10,9.333,11.667,0q�#��� ��$��14(�{�.  $��3��$��38��$	��Top=��� $ ��Выберите язык:��$
��Left=�$��	10,14,0,0q�$	��Top=�$	��128��$��14(�$��29���� -    ��Name$ chooseLangComboBox��$
��Left=�$��
126,14,0,0q�$	��Top=�$��66�� �� SelectedIndex$ 0��$��29��$��14(�
���������Z� �� -   $ searchSaveFilderTextBox��$��10,35.333,11.333,0q�$��1��$
 Wrap=�$��18(�$��2��{�.  $��30��$	��Top=��� -	   $ searchSaveFolderBtn��$��Изменить��$��1��$��176.667,36,0,0q�$��1��$��14(�$
��Left=�$��80��{�.  $��29��$	��Top=�$��2���� -
    ��Name$ searchSaveFilderLabel��$J��DДиректория для сохранения диаграммы:��$
��Left=�$��8,1.333,0,0q�$��1��$	��Top=�$��14(�$	 0.5Q�$��29��$	��273��