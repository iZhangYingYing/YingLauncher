   M S B A M L   `   `   `  ����  Ying     Ying.Controls.ForgeInstall   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   $	��290��5   	   $	��630��6     � RowDefinitions  5      �� 5   
   $��30 �6   �� 5   
   �� 5	   
   $��40 �6   �� 5
   
   $��23 �6    � ColumnDefinitions 5      �� 5   
   �� 5   
   $��49 �6   .+�5      �� $#��安装Forge到选择的版本��6      InfoTextBlockStyle#��[  6%   $��0��6Q   $	��Top=�6^   �� 5      -   5        $	��375��5      $��Right=�6   $	��Top=�67   $	��1,0q�6O    �� DisplayMemberPath$ Id��5   =   ��6   �� *
Versions+ �� Mode$ OneWay=� �� SelectedIndex 5      �� *VersionIndex+$ TwoWay=�{�5      �� *HasAnyVersion+$ OneWay=�5      �� 5      -   5         ��Name$ 
_forgeList��5        GridListViewStyle#��[ 6#   $	��580��5      $	��235��6   $��1��6(   $��2��65   $��0��6F    �� View 5   
   �� 5      .P�5      �� $��Forge版本��6!    �� Width$	 260��62    �� DisplayMemberBinding 6>   �� *	Version+$ OneWay=��� 5      $��修改日期��6!   $	 310��6/    6;   �� *ModifiedTime+$ OneWay=�5       5"      �� 5$      -   5        	 ��Name$	 
_statusBox��5$      #��[  6$   $��3��6P   $��20��6]   $��14(�6i   �� 5&      -   5        
 ��Name$
 _downloadButton��5&        GlowIconButtonStyle#��[ 6&    � ToolTip$# 下载安装选中Forge版本��6S   $��32��5'      $��32��6   $��2��6$   $��1��61   ��5(   
   �� 5)       �� ImageSource$) #/Ying;component/images/download.png��6   5+      5,      