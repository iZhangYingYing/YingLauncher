   M S B A M L   `   `   `  ����?  ;Ying, Version=6.0.4.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Ying.YingControlsYing.YingControls  5   	   H DMahApps.Metro, Version=1.6.5.1, Culture=neutral, PublicKeyToken=nullV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls 5   	   &     Ying.YingWindows.YingMusicWindow   5   
   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4Controlsclr-namespace:Ying.YingControls     5   	   Pcontrols;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     5   	     � AllowsTransparency  .  � WindowStyle$
 None=�5	   #    � 
ResizeMode$ NoResize=�66    � Topmost .   � ShowInTaskbar .   � Title$ YingMusicWindow��5   	   $��180.64��6!   $��280.64��60    � 	Resources 5      '  !System.Windows.Shell.WindowChrome   WindowChromKey   ResizeBorderThickness��%G   &	          5   
    5      u�5      0q�6   5      5      .��5      � -   5        $��YContent��5      ��5   
   �� 5        yimages.yybackground �� ImageSource# [ 6   	 j� Stretch$
	 Fill=�6M   .+�5   
   � $��Bottom=�6   $��0, 0, 0, 110.604q�6*   $��46.604��6D   .+�5      �� -   5        $��yname��5      $
��Ying��6%   
 �� TextAlignment$
 Center=�61   $��20.604(�5      
�������6&   �� 5       $
 Center=�6   $��12.604(�5!      
�������$��Bottom=�6=   �� 5"      $��16.604(�6   .�6(   -6-    �� 5#      -   5         �Name$ yartist��5#      .�6%   Zhang Ying Ying66    �� 5$      $��16.604(�6   .�6(   -6.   5%      5&      %   Ying.YingControls.YingLyricView  5)   
   -   5          Name$ ylyric��5*      $��1��5+      $��0, 0, 0, 38.64q�5,      $��57.64��5-      $��Center=�5.      $��Bottom=�5/      � 50   
   $��38.64��6   $��Bottom=�6   .+�51      �� -   5        $��yplaytoggle��52        GlowIconButtonStyle#��[ 53      
������ $��0q�55       � DataContext$
 Ying��56      $��32��57      $��32��6   $
��Left=�58      $��Center=�59      $��6.4, 0, 0, 0q�5:      $��0q�5;      ��5<      �� 5=        yimages.ymusic.yplay# [ 6!   $
	 Fill=�6U   5?      �� 5A      -   5        $��	yfavorite��5B      #��[ 5C      
������ $��0q�5E      $
 Ying��5F      $��32��5G      $��32��6   $��Right=�5H      $��Center=�5I      $��6.4, 0, 40.64, 0q�5J      $��0q�6+   ��5K      �� 5L        yimages.ymusic.ylike# [ 6!   $
	 Fill=�6U   5N      �� 5P      -   5        $��ymore��5Q      #��[ 5R      
������ $��0q�5T      $
 Ying��5U      $��32��5V      $��32��6   $��Right=�5W      $��Center=�5X      $��6.4, 0, 6.4, 0q�5Y      $��0q�6)   ��5Z      �� 5[        yimages.ymusic.ymore# [ 6!   $
	 Fill=�6U   5]      5^      5_      5`      