   M S B A M L   `   `   `  ����U  QOmnia.Pie.Bdm.Bootstrapper, Version=4.4.3.3, Culture=neutral, PublicKeyToken=nullk:clr-namespace:Omnia.Pie.Bdm.Bootstrapper.Views.CashDeposit,Omnia.Pie.Bdm.Bootstrapper.Views.CashDeposit  5      R NOmnia.Pie.Vtm.Framework, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null�Wclr-namespace:Omnia.Pie.Vtm.Framework.ControlExtenders;assembly=Omnia.Pie.Vtm.Framework(Omnia.Pie.Vtm.Framework.ControlExtenders 5      [ WOmnia.Pie.Vtm.Services.Interface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null�aclr-namespace:Omnia.Pie.Vtm.Services.Interface.Entities;assembly=Omnia.Pie.Vtm.Services.Interface)Omnia.Pie.Vtm.Services.Interface.Entities 5      ]3clr-namespace:Omnia.Pie.Bdm.Bootstrapper.Properties%Omnia.Pie.Bdm.Bootstrapper.Properties  5
      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5      R    LOmnia.Pie.Bdm.Bootstrapper.Views.CashDeposit.CashDepositAccountSelectionView   5      Dlocal:clr-namespace:Omnia.Pie.Bdm.Bootstrapper.Views.CashDeposit  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ecWclr-namespace:Omnia.Pie.Vtm.Framework.ControlExtenders;assembly=Omnia.Pie.Vtm.Framework     5      oeaclr-namespace:Omnia.Pie.Vtm.Services.Interface.Entities;assembly=Omnia.Pie.Vtm.Services.Interface     5      c	 _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity 	 5	      ;p3clr-namespace:Omnia.Pie.Bdm.Bootstrapper.Properties 	 5
      .s&clr-namespace:System;assembly=mscorlib 	 5         BaseView.Style#��[  5        � 	Resources  5        Account.Preview.Template �� DataType7  1Omnia.Pie.Vtm.Services.Interface.Entities.Account � RowDefinitions J� Opacity  Account.Template �� 
TargetType  TextBlock.Normal.Base.Style �� BasedOn �� Property �� Value �� StringFormat	 �� Path & "SelectAccountView.Account.Template
 �� CornerRadius  ColorScheme.TopBorder �Name  TextBlock.Info20.Style  Key  TextBlock.Detail20.Style � ColumnDefinitions5   /Omnia.Pie.Bdm.Bootstrapper.Properties.Resources   LabelNameColon   LabelAccountTypeColon 	 TextBlock.Detail.Style ��Name �� TextAlignment"   LabelSelectAccountWatermark �� Triggers �� Binding �� Value �� 
TargetName *
 &SelectAccountView.AccountDetails.Style  AccountTemplate �� Orientation��%�  &	       &	     7  &	 �    7  7  7  7 	 7 	 7  &	
 =	    7  &	 �	    7  7  7  7  �� 5      "  62   .R�5      �  5      �� 5      $
��auto �6   �� 5      $
��auto �6   .+�5      �� ��6   �� *Number+�� 5      $��1��6   $��14(�6   $	 0.8Q�6+   $��Italic$�69   ��6L   �� *Type+5      5      �� 5      "  6*   .R�5      � $��6q�6     5      ���� 5      '��       " ��6   8   .�5    	   ��  � ��$ NoWrap=�6(   5!   	   5"       5$      �� 5%      $
��auto �6   �� 5&      $
��auto �6   �� 5'      $
��auto �6   .+�5)      �� $��18(�6   ��6   �� *Number+�� 5*      $��1��6   $��13(�6   $	 0.7Q�6,   $��Italic$�6:   ��6M   �� *Type+�� 5+      $��2��6   $��16(�6   $��Right=�6,   ��5,      `� 5-   	   $ 
{0} {1:N2}��6   .1�5.   
   �� $	 Currency�6   �� 5/   
   $	 AvailableBalance�6   50   
   52      53      54      �� 55      "  6=   .R�56      �� $
 6��6   $��0q�6   8��  ._�57      � .+�58      � $ PART_Content��6   $��
12 6 12 10q�6#     59   	   ���� 5:   
   '��       " ��6   8  .�5;      ��  � ��$	 5 3q�6$   �� 5<       � ��$
 Left=�61   �� 5=       � ��$ NoWrap=�6*   5>      �� 5?   
   TextBlock.Detail.Style " ��6/   8  .�5@      ��  � ��$	 5 3q�6$   �� 5A       � ��$
 Left=�61   �� 5B       � ��$ LeftToRight=�6+   �� 5C       � ��$ NoWrap=�6*   5D      5E   
    5F   	   �� 5G   
   $��* �6   �� 5H   
   $��* �6   �� 5I   
   $��* �6   �� 5J   
   $��* �6    5L   	   �� 5M   
   $
��auto �6   �� 5N   
   $
��auto �6   �� 5O   
   $
��auto �6   �� 5P   
   $
��auto �6   .+�5S   	   �� $��0��6   #��Z 6    �� 5T   	   $��1��6   #��Z 6    �� 5V   	   $��0��6   $��1��6    $��3��60   8�� ��6t   �� *Name+�� 5W   	   $��1��6   $��1��6    $��3��60   8�� ��6t   �� *Type+5Y   	   �� 5[      $ PART_EmptyMessage��6   $��Center=�6-   $
��12 0q�6H   $y�	Collapsed=�6V   $ Center=�6m   8�� #��Z 5\      5]      5^       5_      �� 5`       6   ��  6'   Z� .a�5a      �� $ PART_Content��6    � y�$ Hidden=�6?   �� 5b      $ PART_EmptyMessage��6    � y�$ Visible=�6D   5c      5e      �� 5g      " ��6:   .�5h      ��  �@=�ErrorTemplate 5i      �� 5j      .R�5k   	   �� 5l   	   5n      �� 5o       � ��8   6(   5p      �� 5r      .R�5s      �� $��0 0 0 1q�6   ._�5t      � $
��12 6q�6    5u      �� 5v   	   $��* �6   �� 5w   	   $
��auto �6   .+�5z      �� .+�5{   	   �� $
��Left=�6   $��SemiBold"�6F   8��  ��6.   �� *Number+6\   �� 5|   	   $
��Left=�6   $��Italic$�6D   8�� ��6.   �� *Type+6W   5}   	   �� 5      $��1��6   $��Right=�6#   $��Center=�6?   $ 
Horizontal=�6Z   .+�5�   	   �� $��0 0 6 0q�6   8�� ��6$   �� *
Currency+6>   �� 5�   	   8�� ��6   �� *AvailableBalance+65   5�   	   5�      5�      5�      5�      .��5�      �  5�      �� 5�      $��0* �6   �� 5�      $��6* �6   �� 5�      $��* �6   �� 5�      $
��auto �6    5�      �� 5�      $��2* �6   �� 5�      $��8* �6   .+�5�      � $��Center=�6   $	��950��6)    � Orientation$ 
Horizontal=�65   $��1��6N   $��0��6[   $��2��6k   $��20q�6   $
��Left=�6�   .+�5�      �� $	��400��6     Button.Base.Style.Rectangle#��[ 6   .��5�      � $	��400��6   $	��130��6    5�      �� 5�      $��2* �6   �� 5�      $��8* �6    5�      �� 5�      $��* �6   .+�5�      �� $��0,0,0,1q�6   
���   �._�5�      �� $��Center=�6   $	��3,0q�6/   $��0��6<   $	��Top=�6I      LabelSelectAccount#��Z 6a   #��[ 6�   5�      ?  9Omnia.Pie.Vtm.Framework.ControlExtenders.ComboBoxExtender  5�      {�. $	��100��6+   $	��396��68   $��1��6D   $��	0,0.4,0,0q�6Q      Title# Z 6d   $	��Top=�5�      $
��Left=�5�         ItemTemplate# [ 5�         SelectedItemTemplate# [ 5�         ItemsSource 5�      �� *
Accounts+   SelectedItem 5�      �� *SelectedAccount+ �� Mode$ TwoWay=�5�      5�      5�      � 5�       5�      �� 5�      $��* �6   �� 5�      $��* �6    5�      �� 5�      $��* �6   �� 5�      $��* �6   .+�5�      �� $��0��6   $��	20 0 20 3q�6    $��Center=�63     TextBlock.Normal24.Style#��[ 6P      LabelCurrency#��Z 5�      �� 5�      $��1��6   $��0��6   $��0 20 0 0q�6-   $��Center=�6?   #��[ 6\   ��5�   	   �� *SelectedAccount.Currency+    NotSpecified! �� TargetNullValue#! Z  " �� FallbackValue#" Z  5�      5�      � 5�      $��3��6	   $��3��6   $��Bottom=�6*   $��	0 24 0 10q�6E    5�      �� 5�      $
��1.5* �6   �� 5�      $��* �6   �� 5�      $
��2.5* �6   �� 5�      $��* �6   �� 5�      $
��1.5* �6   .+�5�      ��   Button.Negative.Style#��[ 6   #   
ButtonBack#��Z# 5�      ��62   �� *BackCommand+y�5�      �� *BackVisibility+  BooleanToVisibility$ �� 	Converter#$ [ $" 	Collapsed���� 5�      $��2��6     Button.Positive.Style#��[ 6   %   ButtonContinue#��Z% 5�      & �� 	IsDefault& . |�.  ��6Y   �� *DefaultCommand+y�5�      �� *DefaultVisibility+#$ [ $" 	Collapsed���� 5�      $��4��6   #��[ 6   '   ButtonCancel#��Z' 5�      ( �� IsCancel( . |�.  ��6V   �� *CancelCommand+y�5�      �� *CancelVisibility+#$ [ $" 	Collapsed��5�      5�      5�      