   M S B A M L   `   `   `  ����V  ROmnia.Pie.Supervisor.Shell, Version=2.8.10.3, Culture=neutral, PublicKeyToken=nullVclr-namespace:Omnia.Pie.Supervisor.Shell.Utilities;assembly=Omnia.Pie.Supervisor.Shell$Omnia.Pie.Supervisor.Shell.Utilities  5       Omnia.Pie.Bdm.BootstrapperM   GOmnia.Pie.Bdm.Bootstrapper.Views.ChequeDeposit.VerifyChequeSecurityView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      l	converterVclr-namespace:Omnia.Pie.Supervisor.Shell.Utilities;assembly=Omnia.Pie.Supervisor.Shell     5         BaseView.Style#��[  5	        � 	Resources  5      A   ;Omnia.Pie.Supervisor.Shell.Utilities.ConfigurationConverter  	Converter��%   &	         5      6%   5      .��5      �  � RowDefinitions 5      �� 5      $
��auto �6   .+�5      � $��Center=�6	     5       �� 
TargetType  TextBlock.Info24.Style �� BasedOn �� Property �� Value  TextBlock.Detail.Style  TextBlock.Detail24.Style��%4  '��       7  &	 v     7  �� 5      " ��6   8   .�5      ��  � ��$
 12 6q�6    �� 5       � ��$ Right=�6-   5      �� 5      " ��6+   8   .�5      ��  � ��$
 12 6q�6    �� 5       � ��$
 Left=�6-   �� 5       � ��$ LeftToRight=�6'   5      5       � ColumnDefinitions 5!      �� 5"      $��* �6   �� 5#      $��* �6    5&      �� 5'      $
��auto �6   .+�5)      �� -   5         ��Name$ lstVwImages��5)   <   $��
0,-680,0,0q�6   $	��900��6"   $
��1300��6.   $��0��6Q   $��2��6^   $��Center=�6�   ��.  
������ $��0q�6)  $��Disabled=�6=  ��6r   �� *DepositedCheques+ �� UpdateSourceTrigger$ PropertyChanged=�	 �� Mode$	 TwoWay=���5*      �� 5+      .R�5,      � $��10q�6    5-   	   �� 5.   
   $��* �6   .+�51   	   �� $��0��6   $��Center=�6   $	��Top=�6:   $	��900��6R   $	��900��6^   ��52   
   �� 53      
 �� Path$
 MediaId�6   ��55   
   �� 57       j� Stretch$
 Fill=�6    �� ImageSource 6%   �� *ValidationImage+$ PropertyChanged=�$	 TwoWay=�5;      5B   
   5C   	   5D      ��5F      �� 5G      .R�5H      ��  �� Orientation$ 
Horizontal=�6   6.   5I      5K      5L      5M      5N      