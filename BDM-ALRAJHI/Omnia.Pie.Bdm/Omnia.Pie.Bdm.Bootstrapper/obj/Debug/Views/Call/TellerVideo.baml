   M S B A M L   `   `   `  ����[  WSystem.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms  5      U QOmnia.Pie.Bdm.Bootstrapper, Version=4.4.3.3, Culture=neutral, PublicKeyToken=null]3clr-namespace:Omnia.Pie.Bdm.Bootstrapper.Properties%Omnia.Pie.Bdm.Bootstrapper.Properties 5      7   1Omnia.Pie.Bdm.Bootstrapper.Views.Call.TellerVideo   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      UWinForms@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms     5      Ap3clr-namespace:Omnia.Pie.Bdm.Bootstrapper.Properties     5      -   5          � Topmost  .  � WindowStyle$
 None=�5       � ShowInTaskbar .     TopWindow.Style#��[  5      .��5      �  � RowDefinitions 5      �� 5      $��* �6   �� 5      $��2 �6   �� 5      $	��150 �6   .+�5      � $��40 32q�6	    � 	Resources 5       �� 
TargetType  Button.Normal.Style �� BasedOn �� Property �� Value��%�   '��       7  �� 5      " ��6   8   .�5      ��  � ��$
 auto��6    �� 5       � ��$ 12q�6    5      5       5      �� 5      $��* �6   	 � ColumnDefinitions	 5!      �� 5"      $��2* �6   �� 5#      $��* �6   .+�5%      7  1System.Windows.Forms.Integration.WindowsFormsHost  -   5        $��	videoHost��5%   �   $��1��6   
   
Background

 �����y�6#   �� *OnHold+  BooleanInverseVisibility �� 	Converter# [  �� FallbackValue$ Visible��   Child. 5&          System.Windows.Forms.Panel  -   5        	video��"   System.Windows.Forms.Control   Dock$
 Fill=�5&   $   62   5'      � 5(      $��1��6
     ColorScheme.Background#��[ 6   y�5)      �� *OnHold+  BooleanToVisibility# [ $ 	Collapsed��.+�5*      �� 5  /Omnia.Pie.Bdm.Bootstrapper.Properties.Resources   LabelCallHold#��Z 6   $��60(�6<     TextBlock.Detail50.Style#��[ 6J   $��Center=�5+      $��Center=�6*   5,      � 5-      $��1��6
   .+�5.      �� $��1��6   $��1��6   $��Uniform=�6)    �� StretchDirection$
 Both=�6;   $G��A/Omnia.Pie.Bdm.Bootstrapper;component/Resources/Images/topAdd.png��6S   5/      50      �� 52      $��1��6   
�������6   � 53      $��2��6	   $��3��6   $��Stretch=�6*   $��5,15,5,5q�6H   y�6Z   �� *Steps+  NullToVisibility# [  �� ConverterParameter$ False�� 54        StepTemplate ��Name J� Opacity �� Orientation �� CornerRadius ��Name  TextBlock.Normal30.Style �� Path 	 TextBlock.Detail20.Style �� Triggers �� Value �� Binding �� 
TargetName��%*  &	       7  7 	 �� 55      .R�56      � $��Stretch=�6   .+�57      �� $ bdOuter��6   $	 0.5Q�6    $��5q�6.   ._�58   	   � $��Stretch=�6   $��Center=�6,    59   
   �� 5:      $��* �6   �� 5;      $��* �6   .+�5=   
   �� $��0��6   $��Stretch=�6"   $��10��6@   
�������$ 
Horizontal=�6_   �� 5>   
   $ bdInner��6   $��0��6   $ 30��6/   $��50��6A   $��50��6L   
��� ���$��2q�6l   
��� ���._�5?      �� $ number��6   $��20(�6F   $
��Bold"�6T   8��  $��Center=�5@      $��Center=�6*   ��5?   %   �� $ 
StepNumber�5@   *   5A      �� 5B   
   $	 txt��6!   $��1��6   8�� $��Center=�5C      $	��Top=�6*   ��5B   .   �� $ DisplayName�5C   *   5D   
   5E   	   5F       5G      �� 5H      $
 True��6;    6   �� $ IsCurrentStep�.a�5I   	   �� $	 txt��6    � ��$ 	ExtraBold"�67   �� 5J   	   $ bdInner��6    � ��
 � ���6;   �� 5K   	   $ bdOuter��6    �@J�Opacity$ 1Q�68   5L   	   5N      5O      .+�5P      �� -   5        $��steps��5P      #��[ 6B   $��Stretch=�6o   ��6   �� $ Steps���5Q      �� 5R      .R�5S      A�  A� Rows$ 1��6   $��Stretch=�6   5T      ��5V      �� 5W      " ��6   .�5X      ��  � ��$ Stretch=�6.   5Y      5[      5\      5]      5^      