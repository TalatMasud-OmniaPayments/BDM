   M S B A M L   `   `   `  ����U  QOmnia.Pie.Bdm.Bootstrapper, Version=4.4.3.3, Culture=neutral, PublicKeyToken=nullo<clr-namespace:Omnia.Pie.Bdm.Bootstrapper.Views.ChequeDeposit.Omnia.Pie.Bdm.Bootstrapper.Views.ChequeDeposit  5      ]3clr-namespace:Omnia.Pie.Bdm.Bootstrapper.Properties%Omnia.Pie.Bdm.Bootstrapper.Properties  5      V ROmnia.Pie.Supervisor.Shell, Version=2.8.10.3, Culture=neutral, PublicKeyToken=nullVclr-namespace:Omnia.Pie.Supervisor.Shell.Utilities;assembly=Omnia.Pie.Supervisor.Shell$Omnia.Pie.Supervisor.Shell.Utilities 5	      R    LOmnia.Pie.Bdm.Bootstrapper.Views.ChequeDeposit.ChequeDepositConfirmationView   5      Flocal<clr-namespace:Omnia.Pie.Bdm.Bootstrapper.Views.ChequeDeposit  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Ap3clr-namespace:Omnia.Pie.Bdm.Bootstrapper.Properties     5      l	converterVclr-namespace:Omnia.Pie.Supervisor.Shell.Utilities;assembly=Omnia.Pie.Supervisor.Shell     5	         BaseView.Style#��[  5        � 	Resources  5      A  ;Omnia.Pie.Supervisor.Shell.Utilities.ConfigurationConverter  	Converter��%   &	         5      6%   5      .��5      �  � RowDefinitions 5      �� 5      $��* �6   �� 5      $
��auto �6   .+�5      � $��Center=�6	     5       �� 
TargetType  TextBlock.Info24.Style �� BasedOn �� Property �� Value  TextBlock.Detail.Style  TextBlock.Detail24.Style��%4  '��       7  &	 v     7  �� 5      " ��6   8   .�5      ��  � ��$
 12 6q�6    �� 5       � ��$ Right=�6-   5      �� 5      " ��6+   8   .�5      ��  � ��$
 12 6q�6    �� 5        � ��$
 Left=�6-   �� 5!       � ��$ LeftToRight=�6'   5"      5#       � ColumnDefinitions 5%      �� 5&      $��* �6   �� 5'      $��* �6    5*      �� 5+      $
��auto �6   �� 5,      $
��auto �6   �� 5-      $
��auto �6   �� 5.      $
��auto �6   �� 5/      $
��auto �6   �� 50      $
��auto �6   �� 51      $
��auto �6   .+�54      �� $��0��6   
�������5   /Omnia.Pie.Bdm.Bootstrapper.Properties.Resources   LabelAccountNameColon#��Z 6/   �� 55      $��1��6   
�������   LabelAccountNumberColon#��Z 6/   �� 57      $��2��6   
�������	   LabelPostingDate#��Z	 6/   y�6^   �� *DateVisibile+�� 58      $��3��6   
�������
   LabelAmount#��Z
 6/   �� 5:      $��0��6   
�������$��1��6/   #��[ 6?   ��6o   �� *SelectedAccount.Name+  ObjectToString �� 	Converter# [    NotSpecified �� TargetNullValue# Z �� 5;      $��1��6   
�������$��1��6/   #��[ 6?   ��6o   �� *SelectedAccount.Number+�� 5=      $��2��6   
�������$��1��6/   #��[ 6?   ��6o   �� *
ChequeDate+y�6�   �� *DateVisibile+�� 5>      $��3��6   
�������$��1��6/   #��[ 6?   ��5?      `� 5@       �� StringFormat$ 
{0} {1:N2}��6   .1�5A      ��    LabelAed �� Source# Z 6   �� 5B       �� Path$ ChequeAmount�6   5C      5E      �� 5G      -   5         ��Name$ lstVwImages��5G      $	��220��6   $��4��60   $��2��6=   $��Center=�6�   $��20q�6�   ��.  
������ $��0q�6  $��Disabled=�6(  ��6Q   �� *DepositedCheques+ �� UpdateSourceTrigger$ PropertyChanged=� �� Mode$ TwoWay=���5H      �� 5I      .R�5J      � $��10q�6    5K   	   �� 5L   
   $
��auto �6   �� 5M   
   $��* �6   .+�5O   	   �� $��Center=�6   $	��Top=�60   $��0 0 0 3q�6H      LabelRotateCheque#��Z 6Y     TextBlock.Info20.Style#��[ 6�   �� 5P   	   $��1��6   $��Center=�6   $	��Top=�6:   ��6R   ��  �� ElementName$ lstVwImages��$ DataContext.RotateCommand���5Q   
   �� 5R      $ MediaId�6   .��5U   
   ��  J� RenderTransformOrigin$ .5,.5&�6`   ��6   �� *
FrontImage+$ PropertyChanged=�$ TwoWay=�z�6~   �� *ChequeImageTransform+5V   
   5W   	   5X      ��5Z      �� 5[      .R�5\      ��  �� Orientation$ 
Horizontal=�6   6.   5]      5_      �� 5`      $��5��6   $��2��6   $��Center=�60   
�������    LabelChequeProcessingDays#��Z 6`   y�6�   �� *DaysVisible+�� 5a      $��6��6   $��2��6   $ Vertical=�61   $��Center=�6H   .+�5b      �� 
�������   LabelChequeConfirmation#��Z 6#   5d      5e      � 5g      $��1��6	   $��3��6   $��Bottom=�6*   $��	0 24 0 10q�6E    5h      �� 5i      $
��1.5* �6   �� 5j      $��* �6   �� 5k      $
��2.5* �6   �� 5l      $��* �6   �� 5m      $
��1.5* �6   .+�5p      ��   Button.Negative.Style#��[ 6      
ButtonBack#��Z 5q      ��62   �� *BackCommand+y�5r      �� *BackVisibility+  BooleanToVisibility# [  �� FallbackValue$ 	Collapsed���� 5t      $��2��6    	 Button.Positive.Style#��[	 6      ButtonContinue#��Z 5u      ��66   �� *DefaultCommand+y�5v      �� *DefaultVisibility+# [ $ 	Collapsed���� 5x      $��4��6   #��[ 6      ButtonCancel#��Z 5y      ��64   �� *CancelCommand+y�5z      �� *CancelVisibility+# [ $ 	Collapsed��5{      5|      5}      