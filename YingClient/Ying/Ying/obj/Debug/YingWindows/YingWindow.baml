   M S B A M L   `   `   `  ����  Ying    Ying.YingWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   PpoAhttp://schemas.microsoft.com/winfx/2006/xaml/presentation/options     5   	   -   5        $��360.604��5   	   $��640.604��6     � AllowsTransparency  .  � WindowStyle$
 None=�5   	    �� BitmapScalingMode$ HighQuality=�5   	    J� SnapsToDevicePixels .  � WindowStartupLocation$ CenterScreen=�5	   	    � 
ResizeMode$ NoResize=�6.    � Triggers 5      :� 5   
    :� RoutedEvent$ Window.Loaded��6   .[�5      �� .��5      �� .��5      W�  
  Freeze4True  5   @    �� TargetProperty$ Opacity�5   &   	 g� Duration$	 0:0:0.5D�5      
 W� From$	
 0.0X�6,   	 W� To$	 1.0X�67    g� DecelerationRatio$	 0.2Q�6@   W� 5      4True  5   X   $	 Top�5   &   $	 0:0:0.3D�5      $	
 233X�6,   $	 210X�67    g� AccelerationRatio$	 0.4Q�6@   $	 0.6Q�6X   5      5      5      ��5      �� 5   
     yimages.ybackground �� ImageSource# [ 6   .��5      �� ��5    
   �� 5!        
ThemeColor �� Color# �  6   .+�5$   
    � -   5          �Name$ yframe��5$        � NavigationUIVisibility$ Hidden=�6    |�.  6@   �� 5&   
   -   5         ��Name$ _shutdownButton��5&   >     GlowIconButtonStyle#��[ 6   $��22��6W   $��22��6b    �� Right$ 7��6n   
 �� Top$ 7��6   ��5'      �� 5(      4True  6=   $ /images/close.png��6   6=   5*      �� 5,   
   -   5        $ _minimizeButton��5,   >   #��[ 6   $��22��6W   $��22��6b   $ 42��6n   $ 7��6�   ��5-      �� 5.      4True  6@   $ /images/minimize.png��6   6@   50      53      54      