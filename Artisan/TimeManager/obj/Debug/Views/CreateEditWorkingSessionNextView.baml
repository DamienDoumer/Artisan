   M S B A M L   `   `   `  ����F  BTimeManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:TimeManager.ViewsTimeManager.Views  5      ?$clr-namespace:TimeManager.ViewModelsTimeManager.ViewModels  5      > :Dao, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null8'clr-namespace:Dao.Entities;assembly=DaoDao.Entities 5	      8    2TimeManager.Views.CreateEditWorkingSessionNextView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1localclr-namespace:TimeManager.Views     5      U QMaterialDesignThemes.Wpf, Version=2.2.1.750, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      :localViewModels$clr-namespace:TimeManager.ViewModels  5      2data'clr-namespace:Dao.Entities;assembly=Dao  5	      $��Segoe Print)�5
        � DataContext  5      B   <TimeManager.ViewModels.CreateEditWorkingSessionNextViewModel  5   
    J� InputBindings 5      �� 5   
   
 �� Key$ Return��6    �� Command 5      �� *
AddCommand+ � 	Resources 5       �� DataType  Dao.Entities.Task   ItemDisplay1��%�   &	        �� 5   
   "  6   .R�5      �� .+�5      �� $
��Wrap=�62   ��6   �� *Name+62   5      5      5      .��5!      �  � RowDefinitions 5"   
   �� 5#      �� 5$      $��39 �6   .+�5'   
   � 
������� J� Opacity$	 0.8Q�6&    � ColumnDefinitions 5(      �� 5)      $
��139* �6#   �� 5*      $
��119* �6#    5,      �� 5-      .+�50      �� $��10q�6   $��30��6"   )  #MaterialDesignThemes.Wpf.HintAssist	   Hint$	 Enter Each Step Here.��51      ��50   .   �� *Name+
 �� Mode$
 TwoWay=�51      �� 53      -   5         ��Name$ listBox��53      $��2��54      $��10q�55      #��[  56      ��53   '   �� *
TaskList+ �� SelectedItem 54   "   �� *CurrentTask+56      57      �� 59   
    �� Orientation$ 
Horizontal=�6   $��1��6.   $��Center=�6;   .+�5;      �� -   5         ��Name$ button��5;      $��5 0 0 0q�5<      $��Center=�6"   +  %MaterialDesignThemes.Wpf.ShadowAssist   ShadowDepth$ Depth5=�6=   $��80��5=   0   ��5;   %   �� *AddEditButtonText+$
 TwoWay=���5=      �� *
AddCommand+60   �� 5>      -   5        $ button1��5>      $��Remove��6&   $ Depth5=�68   $��5 0 0 0q�5?      $��Center=�6#   $��85��5@   3   ��6   �� *RemoveCommand+63   �� 5A      -   5        $ button2��5A      $
��Save��6&   $ Depth5=�65   $��Center=�5B      $��85��6,   $��5 0 0 0q�67   ��5C      �� *SaveCommand+�� 5D      -   5        $ Previous��5D      $
��Back��6'   $
��Left=�66   $��Center=�5E      $��85��6,   $��5 0 5 0q�67   $ Depth5=�6H   ��5F      �� *PreviousScreenCommand+5G      5H      5I      