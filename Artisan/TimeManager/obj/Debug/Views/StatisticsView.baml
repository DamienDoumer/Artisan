   M S B A M L   `   `   `  ����F  BTimeManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:TimeManager.ViewsTimeManager.Views  5      ?$clr-namespace:TimeManager.ViewModelsTimeManager.ViewModels  5      > :Dao, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null8'clr-namespace:Dao.Entities;assembly=DaoDao.Entities 5      D @WpfCharts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null8*clr-namespace:WpfCharts;assembly=WpfCharts	WpfCharts 5	      ;"clr-namespace:TimeManager.ServicesTimeManager.Services  5
      &     TimeManager.Views.StatisticsView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1localclr-namespace:TimeManager.Views     5      :	viewModel$clr-namespace:TimeManager.ViewModels     5      8data'clr-namespace:Dao.Entities;assembly=Dao     5      @	WpfCharts*clr-namespace:WpfCharts;assembly=WpfCharts     5	      BworkingSessionStats"clr-namespace:TimeManager.Services     5
      $��Segoe Print)�5        � DataContext  5      4   .TimeManager.ViewModels.ViewStatisticsViewModel  5   
    � 	Resources 5         AccomplishedTasksTemplate �� DataType  Dao.Entities.Task � ToolTip   NotAccomplishedTasksTemplate  TaskTemplate �� 
TargetType �� Property �� Value �� Triggers �� Binding	 �� Value  WrkStatsTemplate2   ,TimeManager.Services.WorkingSessionStatistic
 �� Orientation��%�  &	        &	 q     &	 �     7   7  &	 �    �� 5   
   "  69   .R�5      �� 
��� � �$ Accomplished��6A   ��6   �� *Name+6A   5      �� 5   
   "  6<   .R�5      �� 
���  ��$ Not Accomplished��6?   ��6   �� *Name+6?   5      �� 5   
   "  6,   .R�5      �� ��6   �� ��5      �� 5      " ��6   .�5      ��  � ��8   6<    5      �� 5        6*   �� *Accomplished+		 false6K   .a�5!   "   ��  � ��8  6D   5"      5$      5&      5'      �� 5)   
   "  60   .R�5*      �� .+�5+      �� $��Segoe Print)�64   $��15(�6M   ��6   �� *Title+6M   �� 5,      $��Segoe Print)�67   $��15(�6P   ��6   �� *	StartTime+6P   �� 5-      $
 
Horizontal=�6   .+�5.      �� ��6    �� *
Percentage+�� 5/      $��% Accomplished��6    50      51      52      53      .��55      �  � ColumnDefinitions 56   
   �� 57      $��* �6   �� 58      $��7 �6   �� 59      $��* �6   �� 5:      $��7 �6   �� 5;      $��* �6   .+�5>   
   �� $��1��6   $��Stretch=�6'   $��2��5?      
�������$	��1,0q�6:   �� 5@   
   $��3��6   $��Stretch=�6'   $��2��5A      
�������$	��1,0q�6:   �� 5C   
   -   5        $��WorkingSessionsStats��5D      
������� J� Opacity$	 0.7Q�5C   )   #��[ 5F      $��0��5G      $��5q�6"    �� SelectedItem 5D   .   �� *CurrentWorkingSessionStat+��5E      �� *WorkingSessionsStats+5G   "   �� 5H   
   
�������$	 0.7Q�6)   $��2��5I      $��5q�6"   #��[ 6-   ��5J      �� *Tasks+  WpfCharts.SpiderChart  5L   
      Title$ 
Statistics��6    $��4��5M          Minimum$ 0Q�5P          Maximum$ 1Q�5Q          Ticks$ 5��5R          
ShowLegend .    LegendLocation$ BottomRight=�5T          LegendBackgroundColor$ 
Aquamarine��5U          Lines 5N       �� *Lines+   Axis 5O       �� *Axes+5U       5W      5X      