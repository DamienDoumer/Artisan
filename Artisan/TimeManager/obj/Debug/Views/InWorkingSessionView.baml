   M S B A M L   `   `   `  ����F  BTimeManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:TimeManager.ViewsTimeManager.Views  5      ?$clr-namespace:TimeManager.ViewModelsTimeManager.ViewModels  5      > :Dao, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null8'clr-namespace:Dao.Entities;assembly=DaoDao.Entities 5      ,    &TimeManager.Views.InWorkingSessionView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1localclr-namespace:TimeManager.Views     5      @localViewModels$clr-namespace:TimeManager.ViewModels     5      8data'clr-namespace:Dao.Entities;assembly=Dao     5      U QMaterialDesignThemes.Wpf, Version=2.2.1.750, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5	      $��Segoe Print)�5
        � DataContext  5      6   0TimeManager.ViewModels.InWorkingSessionViewModel  5   
    � 	Resources 5         DataTemplate1 �� DataType  Dao.Entities.Task  TaskTemplate  MaterialDesignDivider �� Orientation d� 	IsChecked �� Mode �� Triggers �� Value �� Binding	 �� 
TargetName  MaterialDesignSelection
 �� Value �� Property��%E  &	        &	 s     �� 5   
   "  6-   .R�5      �� $��Segoe Print)�6/   $��15(�6H   ��6   �� *Name+6H   5      �� 5   
   "  6,   .R�5      �� $��BackGroundBorder��6   #���  6-   ._�5      �� $ 
Horizontal=�6   .+�5      �� $��Center=�6    6:   �� *Accomplished+$ TwoWay=��� 5      $��8 0 0 0q�6!   $
��Bold"�62   ��6D   �� *Name+5      5       5      �� 5      $
 True��6?    6   �� *Accomplished+.a�5      �� $	 BackGroundBorder��6   #
 �  6Q    � ��5       5"      5%      .��5)      �  � RowDefinitions 5*   
   �� 5+      �� 5,      $��39 �6   .+�5/   
   �� $��0��6   $ Vertical=�6"   
������� J� Opacity$	 0.8Q�6Q   .+�50      �� -   5        $��WorkingSessoinTitle��50      $��	0 10 0 10q�63   $��Center=�51      $��25(�61   ��50   F   �� *MainWorkingSession.Name+51   1   �� 52      -   5        $��TimeLeftTextBlock��52      $��
10 10 0 10q�61   $��25(�6E   $
��Bold"�53      $��1��6*   $��Center=�54      $��Center=�55   E   ��6   �� *
TimeString+$ TwoWay=�6E   � 57      $��2��6    � ColumnDefinitions 58      �� 59      $��* �6'   �� 5:      $��* �6'   .+�5<      �� $��0��6   .+�5=      �� $ 
Horizontal=�6!   $��
10 10 0 10q�6:   .+�5>      �� $��Start Time:��6    $
��Bold"�66   $��0 0 5 0q�6H   $��15(�5?       61   �� 5@      $
��Bold"�6R   $��15(�5A   $   $��Center=�62   ��5@   $   �� *MainWorkingSession.StartTime+5A   2   5B      �� 5C      $ 
Horizontal=�6!   $��
10 10 0 10q�6:   .+�5D      �� $��	End Time:��6    $
��Bold"�64   $��15(�6F   $��0 0 5 0q�5E       63   �� 5F      $
��Bold"�5G   $   $��15(�66   $��Center=�5H   $   ��5F   $   �� *MainWorkingSession.EndTime+5H   $   5I      5J      �� 5K      $��1��6   .+�5L      �� -   5         ��Name$ Tasks��5L      $��1��6-   $	��700��6=   $��	10 0 0 10q�5N   C   #��[ 5O      ��5M      �� *MainWorkingSession.Tasks+{�5N      �� *EnableTaskTick+5O      5P      5Q      �� 5R      $��1��6   .+�5S      �� $��Description:��6   $��15(�60   $��5q�6>   $
��Bold"�6I   �� 5T      $
��Auto=�6   $	��130��6B   $��5q�6R   $��Disabled=�5U   '   .��5V      �� $��Segoe Print)�5W   !   $��15(�6:   $
��Wrap=�5X   !   $	��800��65   ��5V       �� * MainWorkingSession.Description+$ OneWay=�5X   5   5Y      5Z      5[      �� 5\   
   $��1��6   $ 
Horizontal=�6"   $��Center=�6;   .+�5]      �� -   5         ��Name$ 
SaveButton��5]      $
��Done��6I   +  %MaterialDesignThemes.Wpf.ShadowAssist   ShadowDepth$ Depth5=�5^      $��30��6F   $��80��6R   $��	10 0 10 0q�6]   ��5]   )   �� *SaveCommand+5^   ]   �� 5_      -   5        $ CancelButton��5_      $��Cancle��6M   $ Depth5=�5`      $��30��6F   $��80��6R   $��	10 0 10 0q�6]   ��5_   +   �� *CancleCommand+5`   ]   5a      5b      5c      