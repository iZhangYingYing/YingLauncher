   M S B A M L   `   `   `  ����?  ;Ying, Version=6.0.4.0, Culture=neutral, PublicKeyToken=null5clr-namespace:Ying.YingControlsYing.YingControls  5          Ying.Pages.YingMainPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      PpoAhttp://schemas.microsoft.com/winfx/2006/xaml/presentation/options     5      8YingControlsclr-namespace:Ying.YingControls     5         	PageStyle#��[  5        N� Title$  MainPage��5       � 	Resources 5	        _GlowIconButtonStyle �� 
TargetType  GlowIconButtonStyle �� BasedOn �� Property �� Value��%�   &	       7  �� 5
   
   " ��6-   8   .�5      ��  � ��$ 32��6&   �� 5       � ��$ 32��6'   �� 5       � ��$
 Left=�64   �� 5       � ��$ 2��6)   5      5      .��5      �  � RowDefinitions 5   
   �� 5      $	��160 �6   �� 5      �� 5      $��45 �6   .+�5   
   � $��132.604��6   $��16.64, 16.64, 0, 0q�6   $
��Left=�6;   $	��Top=�6V   .+�5      �� -   5        $��YAvatar��5      $��34.64��6$   $��34.64��62   #��[ 5       � Cursor$
 Hand��5       J� Opacity$ 1Q�5      $��0��6   $
��Left=�5      ���	#82FFFFFF��5       �� 5!      	 �� 
TargetType"	 ��6&   .R�5"      �� -   5        #��z��5"   !   $��1q�6U   
 �� CornerRadius$
 32,32,32,32��6i   ��5$      �� 5%   "    �� ImageSource$# /yresources/yimages/yuser.jpg��6-   ._�5'      �� #��z��6/   $��Center=�6b   $��Center=�6   6�   5(      5)      5+      �� 5,      $��38.604, 3.604, 0, 0q�6   $
��Ying��65   
�������6A   5.      �� 50   
   $��2��6   $	��280��6    $��26��6,   $��Right=�68   $��100,0q�6T    �� DisplayMemberPath$ Id��51   A   ��6   �� *
Versions+ �� Mode$ OneWay=� �� SelectedIndex 52      �� *VersionIndex+$ TwoWay=�{�53      �� *HasAnyVersion+$ OneWay=��� 55   
   -   5        $��_launchButton��55      $��启动��6&     GlowNormalButtonStyle#��[ 63   $��2��56      $��80��6   $��26��6)   $��Right=�65   $
��10,0q�6Q    � ToolTip$ Let's Play!��6n   $��15(�6�   {�57      �� *HasAnyVersion+$ OneWay=��� 59   
   -   5        
 � Tag$ ResourcepackPage��59      #��[ 6(   $ 资源包管理器��6V   $��128,0q�6g   ��5:      �� 5;       
 Freeze4True 6D   $ /images/resourcepack.png��6   6D   5=      �� 5?   
   -   5        $ ModPage��5?      #��[ 6   $ MOD管理器��6M   $
��88,0q�6^   ��5@      �� 5A      4True 6;   $ /images/mod.png��6   6;   5C      �� 5E   
   -   5        $ SkinPage��5E      #��[ 6    $ 外观选项��6N   $
��48,0q�6]   ��5F      �� 5G      4True 6<   $ /images/skin.png��6   6<   5I      �� 5K   
   -   5        $ YingSettingsPage��5K      #��[ 6(   $ 游戏选项��6V   $	��8,0q�6e   ��5L      �� 5M      4True 6@   $ /images/settings.png��6   6@   5O      �� 5Q   
   -   5        $��	_titleBox��5Q      $
��Ying��6%   $��0��61   $��40(�6>   
�������$	��600��6_    �� TextAlignment$ Center=�6n   $	��620��6�   $��80��6�    J� Effect 5R      3  -System.Windows.Media.Effects.DropShadowEffect  5S      4True 6�      RenderingBias$ Quality=�6#      Opacity$	 0.6Q�6;      
BlurRadius$ 12Q�6I     
ThemeColor   Color# �  6Y      ShadowDepth$ 0Q�6~   6�   5U      5W      5X      