   M S B A M L   `   `   `  ����  Ying$    Ying.Controls.YingGameDownload   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   $	��290��5   	   $	��630��6     � RowDefinitions  5      �� 5   
   �� 5   
   $��40 �6   �� 5	   
   $��40 �6   �� 5
   
   $��23 �6    � ColumnDefinitions 5      �� 5   
   �� 5   
   $��49 �6   .+�5      �� -   5         ��Name$ _downloadVersionList��5         GridListViewStyle#��[  6-   $	��580��5      $	��266��6   $��0��6(   $��4��65   $��0��6F   $	��Top=�6V    �� View 5   
   �� 5      .P�5      �� $��版本��6!    �� Width$	 150��6-    �� DisplayMemberBinding 69   �� *ID+�� 5      $��发布日期��6!   $	 300��6/    6;   �� *ReleaseTime+�� 5      $��类型��6!   $	 120��6-    69   �� *Type+5      5      �� 5      -   5         ��Name$ 
_statusBox��5        InfoTextBlockStyle#��[ 6$   $��3��6P   $��20��6]   $��14(�6i   �� 5      -   5         ��Name$ _refreshButton��5        GlowIconButtonStyle#��[ 6%    � ToolTip$ 刷新列表��6R   $��32��5       $��32��6   $��1��6$   $��1��61   ��5!   
   �� 5"      	 �� ImageSource$(	 "/Ying;component/images/refresh.png��6   5$      �� 5&      -   5        $ _downloadButton��5&      #��[ 6&   $ 下载选中版本��6S   $��32��5'      $��32��6   $��2��6$   $��1��61   ��5(   
   �� 5)      $)	 #/Ying;component/images/download.png��6   5+      5,      