   M S B A M L   `   `   `  ����B  >Artisan, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:Artisan.ViewsArtisan.Views  5   	   T PMahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls 5   	   H DHamburgerMenu, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullD2clr-namespace:HamburgerMenu;assembly=HamburgerMenuHamburgerMenu 5   	   7 clr-namespace:Artisan.ViewModelsArtisan.ViewModels  5   	   F BTimeManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullT9clr-namespace:TimeManager.ViewModels;assembly=TimeManagerTimeManager.ViewModels 5   	   J4clr-namespace:TimeManager.Views;assembly=TimeManagerTimeManager.Views 5   	   7 clr-namespace:Artisan.ViewModelsArtisan.ViewModels  5   	   D @Messanger, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullN5clr-namespace:Messenger.ViewModels;assembly=MessangerMessenger.ViewModels 5   	   D0clr-namespace:Messenger.Views;assembly=MessangerMessenger.Views 5   	       Artisan.Views.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:Artisan.Views     5   	   PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     5   	   C	iconPacks-http://metro.mahapps.com/winfx/xaml/iconpacks     5   	   LHamburgerMenu2clr-namespace:HamburgerMenu;assembly=HamburgerMenu     5   	   W	 SMaterialDesignThemes.MahApps, Version=0.0.7.0, Culture=neutral, PublicKeyToken=nullU
 QMaterialDesignThemes.Wpf, Version=2.2.1.750, Culture=neutral, PublicKeyToken=nullHmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes 	 
 5   	   -main clr-namespace:Artisan.ViewModels 	 
 5   	   VtimeManagerViewModel9clr-namespace:TimeManager.ViewModels;assembly=TimeManager 	 
 5   	   LtimeManagerView4clr-namespace:TimeManager.Views;assembly=TimeManager 	 
 5   	   8localViewModels clr-namespace:Artisan.ViewModels 	 
 5   	   PmessengerViewModel5clr-namespace:Messenger.ViewModels;assembly=Messanger 	 
 5   	   FmessengerView0clr-namespace:Messenger.Views;assembly=Messanger 	 
 5   	   -   5        (  "MahApps.Metro.Controls.MetroWindow   Name$  Artisan��5
   	    � Title$ Artisan��5	   	   $��511.76��6   $��791.829��6)    � WindowStartupLocation$ CenterScreen=�69   $��511.76��5
      $��636.329��6-   $��1q�6@      	GlowBrush
 ��  �
����  �   ShowIconOnTitleBar .  � Icon$" UIResources\DoumeraIcon2.ico��5   	      WindowTransitionsEnabled .    MaterialDesignBody#���   5   	   $��Regular"�5   	   $��Verdana)�5   	   $��13(�5   	   &   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=�5   	      TextRenderingMode$
 Auto=�5   	     MaterialDesignPaper#���  5    	     MaterialDesignFont#���  5!   	   	 � DataContext	 5$      ,  &Artisan.ViewModels.MainWindowViewModel  5%   
   
 � 	Resources
 5(       �� DataType4  .TimeManager.ViewModels.ViewStatisticsViewModel&   TimeManager.Views.StatisticsView-  'Messenger.ViewModels.MessengerViewModel#  Messenger.Views.MessengerView*   $Artisan.ViewModels.MainMenuViewModel 	   Artisan.Views.MainMenuView1
  +TimeManager.ViewModels.CreateEventViewModel'  !TimeManager.Views.CreateEventView1  +TimeManager.ViewModels.ManageEventViewModel'  !TimeManager.Views.ManageEventView>  8TimeManager.ViewModels.CreateEditWorkingSessionViewModel0  *TimeManager.Views.CreateWorkingSessionView;  5TimeManager.ViewModels.ManageWorkingSessionsViewModel1  +TimeManager.Views.ManageWorkingSessionsViewB  <TimeManager.ViewModels.CreateEditWorkingSessionNextViewModel8  2TimeManager.Views.CreateEditWorkingSessionNextView6  0TimeManager.ViewModels.InWorkingSessionViewModel,  &TimeManager.Views.InWorkingSessionView  img �� ImageSource��%�  (��       5.      *, +)(�� 2     51      *, +)(�� d     54      *, +)(�� �     57      *,
 +)(�� �     5:      *, +)(�� �     5=      *, +)(�� ,    5@      *, +)(�� ^    5C      *, +)(�� �    5F      *, +)&	 �    �� 5.   
   "  6   .R�5/        50      �� 51   
   "  6   .R�52        53      �� 54   
   "  6   .R�55      	  56      �� 57   
   " 
 6   .R�58        59      �� 5:   
   "  6   .R�5;        5<      �� 5=   
   "  6   .R�5>        5?      �� 5@   
   "  6   .R�5A        5B      �� 5C   
   "  6   .R�5D        5E      �� 5F   
   "  6   .R�5G        5H      �� 5J   
   $' !UIResources\SkyBlueBackGround.jpg��6!   5L      5N      .��5Q      ]� #��[ 6   .+�5R   
   !  HamburgerMenu.HamburgerMenu  
����d�   MenuIconColor
 ����� J� Opacity$	 0.8Q�5S   &   $��0��5T   &   $
��Left=�66      SelectionIndicatorColor
 �����   MenuItemForeground
 �����$
��Left=�5V   &      Content 5X      %  HamburgerMenu.HamburgerMenuItem  $��50��62      Text$
 Home��6>      Icon$ UIResources\HomeIcon.png��6J      SelectionCommand 5Y   2   �� *MainMenuSwitchCommand+  5Z      $��50��62   $ Wifi Messaging��6>   $ UIResources\Message.png��6T    5[   2   �� *MessageSwitchCommand+  5]      $��50��62   $ Manage your Appointments��6>   $ UIResources\Event.png��5_   2    5^   2   �� *ManageEventSwitchCommand+5_   2     5`      $��50��62   $" Manage Your Working Sessions��6>   $$ UIResources\WorkingSession.png��5b   2    5a   2   �� *#!ManageWorkingSessionSwitchCommand+5b   2     5c      $��50��62   $ 
Statistics��6>   $! UIResources\Performance.png��6P    5d   2   �� *StatisticsSwitchCommand+  5e      $��50��62   $ Settings��6>   $ UIResources\Settings.png��6N   5h      �� 5j   
   $
��Auto=�6   $��Right=�6<   $
��Auto=�5k      .��5m      �� 
���  ��$��Stretch=�6/   $��Stretch=�5n   !   $��2q�5o   !   $��1��5p   !   $��1��61   ��5o   5   �� *CurrentViewModel+5p   1   5r      5t      5v      