   M S B A M L   `   `   `  ����U  QOmnia.Pie.Bdm.Bootstrapper, Version=4.4.3.3, Culture=neutral, PublicKeyToken=nullS.clr-namespace:Omnia.Pie.Bdm.Bootstrapper.Views Omnia.Pie.Bdm.Bootstrapper.Views  5      R NOmnia.Pie.Vtm.Framework, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null�Wclr-namespace:Omnia.Pie.Vtm.Framework.ControlExtenders;assembly=Omnia.Pie.Vtm.Framework(Omnia.Pie.Vtm.Framework.ControlExtenders 5      5    /Omnia.Pie.Bdm.Bootstrapper.Views.TopVideoWindow   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @local.clr-namespace:Omnia.Pie.Bdm.Bootstrapper.Views     5      ecWclr-namespace:Omnia.Pie.Vtm.Framework.ControlExtenders;assembly=Omnia.Pie.Vtm.Framework     5         TopWindow.Style#��[  5
      .��5      �   � RowDefinitions  5      �� 5      $��* �6   �� 5      $��2 �6   �� 5      $
��auto �6   .+�5      r�  r� LoadedBehavior$ Manual=�5       r� Volume 5      �� *Volume+C  =Omnia.Pie.Vtm.Framework.ControlExtenders.MediaElementExtender   Sources 5      �� *	Sources+y�5      �� *	Sources+  NullToVisibility �� 	Converter# [  �� ConverterParameter$ False���� 5      $��1��6   
�������6   � 5      $��2��6	   $��3��6   $��Stretch=�6*   $��5,15,5,5q�6H   y�6Z   �� *Steps+# [ $ False�� � 	Resources 5        StepTemplate ��Name J� Opacity	 �� Orientation
 �� CornerRadius ��Name  TextBlock.Normal30.Style �� Path  TextBlock.Detail20.Style �� Triggers �� Value �� Binding �� 
TargetName �� Property �� Value��%*  &	       7  7  �� 5      .R�5      � $��Stretch=�6   .+�5      �� $ bdOuter��6   $	 0.5Q�6    $��5q�6.   ._�5   	   � $��Stretch=�6   $��Center=�6,     5   
   �� 5      $��* �6   �� 5      $��* �6   .+�5!   
   �� $��0��6   $��Stretch=�6"   $��10��6@   
�������$	 
Horizontal=�6_   �� 5"   
   $ bdInner��6   $��0��6   $
 30��6/   $��50��6A   $��50��6L   
��� ���$��2q�6l   
��� ���._�5#      �� $ number��6   $��20(�6F   $
��Bold"�6T   8��  $��Center=�5$      $��Center=�6*   ��5#   %   �� $ 
StepNumber�5$   *   5%      �� 5&   
   $	 txt��6!   $��1��6   8�� $��Center=�5'      $	��Top=�6*   ��5&   .   �� $ DisplayName�5'   *   5(   
   5)   	   5*       5+      �� 5,      $
 True��6;    6   �� $ IsCurrentStep�.a�5-   	   �� $	 txt��6    � ��$ 	ExtraBold"�67   �� 5.   	   $ bdInner��6    � ��
 � ���6;   �� 5/   	   $ bdOuter��6    �@J�Opacity$ 1Q�68   50   	   52      53      .+�54      �� -   5        $��steps��54      #��[ 6B   $��Stretch=�6o   ��6   �� $ Steps���55      �� 56      .R�57      A�  A� Rows$ 1��6   $��Stretch=�6   58      ��5:      �� 5;       �� 
TargetType" ��6   .�5<      ��  � ��$ Stretch=�6.   5=      5?      5@      5A      5B      