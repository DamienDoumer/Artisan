   M S B A M L   `   `   `  ����B  >Artisan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:Artisan.Views.NotificationsArtisan.Views.Notifications  5   	   4    .Artisan.Views.Notifications.GrowlNotifications   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ;local)clr-namespace:Artisan.Views.Notifications     5   	   $	��530��5      $	��300��6'     � ShowActivated  .   � AllowsTransparency .  � WindowStyle$
 None=�5   	    � ShowInTaskbar .  
������  � Topmost .  � UseLayoutRounding . $��Segoe Print)�5	   	    � 	Resources 5         CollapseStoryboard W� From	 W� To	 �� TargetProperty
 g� Duration  MessageTemplate �� DataType �Name
 � Tag �� Path J� Effect3  -System.Windows.Media.Effects.DropShadowEffect   ShadowDepth   Opacity   
BlurRadius � RowDefinitions � ColumnDefinitions&   System.Windows.Media.TextOptions   TextRenderingMode   TextFormattingMode ��Name �� Triggers :� RoutedEvent :� 
SourceName ��Name �� 
TargetName g� 	BeginTime P� Property P� Value  N� EnterActions! �� Offset" �� BeginStoryboardName# N� ExitActions$ P� 
SourceName% �� 
TargetName& �� Property' �� Value��%�	  &	        &	 g     7   �� 5   
   .��5      W� $	 100X�6   $ 0X�6)   $	 Height�60   $
 0:0:1D�6S   5      �� 5   
   $ Model:Notification��6/   .R�5      � $ NotificationWindow��6   -   5        
������  5   /   �� $ Id�.+�5      �� $��border��6   ���#2a3345$��0q�6<   $��10q�6Q    5        5      $ 0Q�6+   $	 0.8Q�6;   $ 10Q�6I   ._�5      � $	��100��6   $	��280��6(   $��6q�64    5       �� 5!      $
��Auto �6,   6<   �� 5"      $��* �6,   69    5$      �� 5%      $
��Auto �6/   6>   �� 5&      $��* �6/   6;   .+�5(      �� $��2��6    $��4q�6R   $��80��6]   ��61   �� $ ImageUrl�6j   �� 5)      $��1��6$   $ 	ClearType=�6Q   $ Display=�6{   
�������$��Arial)�5*   $   $��14(�67   $
��Bold"�6E   $��Center=�6W   $��2,4,4,2q�6s   $
��Wrap=�6�   $��CharacterEllipsis=�6�   ��5)   4   �� $ Title�5*   �   �� 5+      $ CloseButton��6!   $��1��66   $��16��6F   $��16��6Q   
�������$��X��5,   3   
�������$��Right=�5-   !   $��0,0,12,0q�6=   �� 5.      $��1��6$   $��1��61   $ 	ClearType=�6`   $ Display=�6�   
�������$��Arial)�5/   $   $��Center=�67   $��2,2,4,4q�6S   $
��Wrap=�6d   $��CharacterEllipsis=�6x   ��5.   A   �� $ Message�5/   x   50      51      52       53      :� 54      $ Window.Loaded��6   $ NotificationWindow��6;   .[�55      �� $ FadeInStoryBoard��6&   .��56      �� .��57      W� $ NotificationWindow��6.   $
 0.01X�6Y   $ 1X�6e   $	 Opacity�6l   $
 0:0:2D�6�   W� 58      $ NotificationWindow��6.   $ 1X�6Y   $ 0X�6b   $	 Opacity�6i   $
 0:0:2D�6�   $ 0:0:6X�6�   59      5:      5;      P� 5<       �@J�IsMouseOver .   5=      �� 5>      $! 0:0:3h�6)   $" FadeInStoryBoard��68   ?� 5?      $" FadeInStoryBoard��6*   # 5A      �� 5B      $! 0:0:3h�6)   $" FadeInStoryBoard��68   �� 5C      $" FadeInStoryBoard��6+   6T   5E      :� 5F      $ Button.Click��6   $ CloseButton��6:   .[�5G      �� .��5H      �� .��5I      W� $ NotificationWindow��6.   $ 1X�6Y   $ 0X�6b   $	 (Grid.Opacity)�6i   $
 0:0:0D�6�   5J      5K      5L      P� 5M      $$ NotificationWindow��6    �@J�Opacity$ 0Q�6M   . �5N      �� $% NotificationWindow��6   & � y�$' Hidden=�6S   6d     5O      �� 5P      8��  6*   5R      5T      5U      .��5V      �� -   5        ( ��Name$( NotificationsControl��5V      #��[ 6�   ) � FocusVisualStyle) 61   Z� ��6�   �� *.+6�   5X      