   M S B A M L   `   `   `  ����U  QOmnia.Pie.Bdm.Bootstrapper, Version=4.4.3.3, Culture=neutral, PublicKeyToken=nullo<clr-namespace:Omnia.Pie.Bdm.Bootstrapper.Views.ChequeDeposit.Omnia.Pie.Bdm.Bootstrapper.Views.ChequeDeposit  5      ]3clr-namespace:Omnia.Pie.Bdm.Bootstrapper.Properties%Omnia.Pie.Bdm.Bootstrapper.Properties  5      V ROmnia.Pie.Supervisor.Shell, Version=2.8.10.3, Culture=neutral, PublicKeyToken=nullVclr-namespace:Omnia.Pie.Supervisor.Shell.Utilities;assembly=Omnia.Pie.Supervisor.Shell$Omnia.Pie.Supervisor.Shell.Utilities 5	      N    HOmnia.Pie.Bdm.Bootstrapper.Views.ChequeDeposit.SignatureVerificationView   5      Flocal<clr-namespace:Omnia.Pie.Bdm.Bootstrapper.Views.ChequeDeposit  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Ap3clr-namespace:Omnia.Pie.Bdm.Bootstrapper.Properties     5      l	converterVclr-namespace:Omnia.Pie.Supervisor.Shell.Utilities;assembly=Omnia.Pie.Supervisor.Shell     5	         BaseView.Style#��[  5        � 	Resources  5      A  ;Omnia.Pie.Supervisor.Shell.Utilities.ConfigurationConverter  	Converter��%   &	         5      6%   5      .��5      �  � RowDefinitions 5      �� 5      $
��auto �6   .+�5      � $��Center=�6	     5       �� 
TargetType  TextBlock.Info24.Style �� BasedOn �� Property �� Value  TextBlock.Detail.Style  TextBlock.Detail24.Style��%4  '��       7  &	 v     7  �� 5      " ��6   8   .�5      ��  � ��$
 12 6q�6    �� 5       � ��$ Right=�6-   5      �� 5      " ��6+   8   .�5      ��  � ��$
 12 6q�6    �� 5       � ��$
 Left=�6-   �� 5        � ��$ LeftToRight=�6'   5!      5"       � ColumnDefinitions 5$      �� 5%      $��* �6   �� 5&      $��* �6    5)      �� 5*      $
��auto �6   �� 5+      $
��auto �6   .+�5.      �� -   5         ��Name$ lstVwImages��5.   '   $	��900��6   $	��350��6   $��0��6<   $��2��6I   $��Center=�6�   $��5q�6�   ��.  
������ $��0q�6  $��Disabled=�63  ��6]   �� *DepositedCheques+ �� UpdateSourceTrigger$ PropertyChanged=�	 �� Mode$	 TwoWay=���5/      �� 50      .R�51      � $��10q�6    52   	   �� 53   
   $
��auto �6   �� 54   
   $��* �6   .+�57   	   �� $��1��6   $��Center=�6   $	��Top=�6:   ��58   
   �� 59      
 �� Path$
 MediaId�6   .��5<   
   ��  J� RenderTransformOrigin$ .5,.5&�6`   ��6   �� *
FrontImage+$ PropertyChanged=�$	 TwoWay=�z�6~   �� *ChequeImageTransform+5=   
   5>   	   5?      ��5A      �� 5B      .R�5C      ��  �� Orientation$ 
Horizontal=�6   6.   5D      5F      �� 5G      -   5        $ lstVwImagesBack��5G   '   $	��900��6   $	��270��6   $��1��6@   $��2��6M   $��Center=�6�   $��5q�6�   ��.  
������ $��0q�6#  $��Disabled=�67  ��6a   �� *DepositedCheques+$ PropertyChanged=�$	 TwoWay=���5H      �� 5I      .R�5J      � $��10q�6    5K   	   �� 5L   
   $
��auto �6   �� 5M   
   $��* �6   .+�5P   	   �� $��1��6   $��Center=�6   $	��Top=�6:   ��5Q   
   �� 5R      $
 MediaId�6   .��5U   
   �� $	��900��6   $ .5,.5&�6k   ��6   �� *	BackImage+$ PropertyChanged=�$	 TwoWay=�5Y      5Z   
   5[   	   5\      ��5^      �� 5_      .R�5`      �� $ 
Horizontal=�6   6.   5a      5c      5d      5e      5f      