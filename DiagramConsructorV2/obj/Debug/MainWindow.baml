   M S B A M L   `   `   `  ����N  JDiagramConsructorV2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:DiagramConsructorV2DiagramConsructorV2  5   	   $    DiagramConsructorV2.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:DiagramConsructorV2     5   	     � Title$  Diagram constructor��5   	   $��617.833��6%   $��582.833��66    � Icon$% Help/diagramConstructorIcon.ico��6F   .��5	      � $��0,0,0,2q�6    � RowDefinitions 5
   
   �� 5      $
��475* �6   �� 5      $	��79* �6   �� 5      $	��39* �6   �� 5      $	��58* �6    � ColumnDefinitions 5   
   �� 5      $
��365* �6   �� 5      $
��223* �6   �� 5      $
��114* �6   .+�5   
   �� -   5         ��Name$ closeAfterBuildCheckBox��5      $X��RЗакрыть документ диаграммы после построения��64   $��10,0,0,12.667q�6j   $��14(�6�   $��2��6�   {�.   d� IsThreeState .  $��18��6�   $��Bottom=�6�    J� Opacity$	 0.5Q�6�   $
��Left=�6�   $	��342��6  $��2��6!  �� 5   
   -   5         ��Name$ filepathTextBox��5      $��90��6+   $��
10,88,21,0q�67    �� TextWrapping$
 Wrap=�6K   $	��Top=�6_   	 J� 	AllowDrop	 . 
 � Cursor$
 Arrow��6  $m��g
 Перетащите нужный файл или кликните для выбора вручную��6   $��18(�6e  $	 0.5Q�6s   �� TextAlignment$ Center=�6�   �� AcceptsReturn . $��3��6�  �� 5   
   -   5        $ codeContentTextBox��5      $��10,225,21,10.667q�6.   $
 Wrap=�6H    . " �� VerticalScrollBarVisibility$ Visible=�6q    �� 
IsReadOnly . $��3��6�   {�.   �� 
AcceptsTab . 6�   	� 5   
   -   5         	�Name$ useFileRadioBtn��5      $��Выбрать файл��6/   $
��Left=�6F   $��	10,62,0,0q�6a   $	��Top=�6t   $	��110��6�    J� RenderTransformOrigin$ 0.489,0.534&�6�    d� 	IsChecked$
 TrueY�6�   $��14(�6�   $��18��6�   	� 5   
   -   5        $ inputCodeRadioBtn��5      $��Ввести код��61   $
��Left=�6F   $��
10,199,0,0q�6a   $	��Top=�6u   $	��101��6�   $ 
0.465,0.45&�6�   $��14(�6�   $��18��6�   �� 5   
   -   5         ��Name$ createDiagramBtn��5      $%��Создать диаграму��6+   $��10,9.333,11.667,0q�6F   #��� ��6a   $��14(�6�   {�.  $��3��6�   $��38��6�   $	��Top=�6  �� 5   
   $ ��Выберите язык:��6   $
��Left=�6)   $��	10,14,0,0q�6D   $	��Top=�6W   $	��128��6o   $��14(�6{   $��29��6�   �� 5   
   -   5         ��Name$ chooseLangComboBox��5      $
��Left=�6/   $��
126,14,0,0q�6J   $	��Top=�6^   $��66��6v    �� SelectedIndex$ 0��6�   $��29��6�   $��14(�6�   
���������6�   Z� 6�   �� 5   
   -   5        $ searchSaveFilderTextBox��5      $��10,35.333,11.333,0q�63   $��1��6O   $
 Wrap=�6\   $��18(�6p   $��2��6~   {�.  $��30��6�   $	��Top=�6�   �� 5   
   -	   5        $ searchSaveFolderBtn��5      $��Изменить��6.   $��1��6A   $��176.667,36,0,0q�6Q   $��1��6i   $��14(�6v   $
��Left=�6�   $��80��6�   {�.  $��29��6�   $	��Top=�6�   $��2��6  �� 5   
   -
   5         ��Name$ searchSaveFilderLabel��5      $J��DДиректория для сохранения диаграммы:��6/   $
��Left=�6^   $��8,1.333,0,0q�6y   $��1��6�   $	��Top=�6�   $��14(�6�   $	 0.5Q�6�   $��29��6�   $	��273��6�   5       5!      