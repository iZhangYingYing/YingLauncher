   M S B A M L   `   `   `  ����  Ying    Ying.Pages.DownloadPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5         	PageStyle#��[  5        N� Title$  DownloadPage��6*   .��5      �  � RowDefinitions 5   
   �� 5	      �� 5
      $��30 �6   �� 5      $��40 �6   �� 5      $��25 �6   �� 5      $��25 �6   �� 5      .+�5   
   �� -   5         ��Name$ _backButton��5        GlowIconButtonStyle#��[ 6'    � ToolTip$ 取消下载并返回��6T   $��22��5      $��22��6   $	��Top=�6(   $
��Left=�6@   $��0��6[   $��7q�6h   ��5      �� 5       �� ImageSource$ /images/back.png��6   5      �� 5   
     InfoTextBlockStyle#��[ 6   $��	总进度��6@   $��2��6K   $
��10,0q�6X   �� 5   
   #��[ 6   $��Right=�6@   $��1��6\   $
��25,0q�6i   ��5      �� *DownloadTitle+ �� Mode$ OneWay=��� 5   
   #��[ 6   $��14(�6@   $��18��6N   $��3��6Z   $
��75,0q�6g   ��5      �� *DownloadCompletes+$ OneWay=��� 5    
   #��[ 6   $��14(�6@   $��18��6N   $��4��6Z   $
��75,0q�6g   ��5!      �� *DownloadFails+$ OneWay=��� 5#   
   #��[ 6   $��14(�6@   $��18��6N   $��3��6Z   $��Right=�6g   $
��25,0q�6�   ��5$      �� *DownloadSpeed+$ OneWay=�� 5&   
     ThemeColorBrush#���  6   ���	#60FFFFFF$��27��5'      $	��540��6"   $��2��6.   $
��Left=�6;   $��75,0,0,0q�6V    � Minimum$	 0.0Q�5(       � Maximum$	 1.0Q�6$    � Value 5)      �� *DownloadProgress+$ OneWay=�5+      5,      