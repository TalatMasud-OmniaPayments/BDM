   M S B A M L   `   `   `  ����U  QOmnia.Pie.Bdm.Bootstrapper, Version=4.4.3.3, Culture=neutral, PublicKeyToken=nullq=clr-namespace:Omnia.Pie.Bdm.Bootstrapper.Views.ChequePrinting/Omnia.Pie.Bdm.Bootstrapper.Views.ChequePrinting  5      ]3clr-namespace:Omnia.Pie.Bdm.Bootstrapper.Properties%Omnia.Pie.Bdm.Bootstrapper.Properties  5      M    GOmnia.Pie.Bdm.Bootstrapper.Views.ChequePrinting.ChargesConfirmationView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Olocal=clr-namespace:Omnia.Pie.Bdm.Bootstrapper.Views.ChequePrinting     5      Ap3clr-namespace:Omnia.Pie.Bdm.Bootstrapper.Properties     5         BaseView.Style#��[  5      .��5      �   � RowDefinitions  5      �� 5      $
��auto �6   �� 5      $��* �6   �� 5      $
��auto �6   .+�5      � $��Center=�6	   $��1��6$    � 	Resources 5       �� 
TargetType  TextBlock.Info24.Style �� BasedOn �� Property �� Value  TextBlock.Detail.Style  TextBlock.Detail24.Style��%4  '��       7  &	 v     7  �� 5      " ��6   8   .�5      ��  � ��$
 12 6q�6    �� 5       � ��$ Right=�6-   5      �� 5      " ��6+   8   .�5      ��  � ��$
 12 6q�6    �� 5       � ��$
 Left=�6-   �� 5       � ��$ LeftToRight=�6'   5      5       � ColumnDefinitions 5!      �� 5"      $��* �6   �� 5#      $��* �6     5&      �� 5'      $
��auto �6   �� 5(      $
��auto �6   �� 5)      $
��auto �6   �� 5*      $
��auto �6   �� 5+      $
��auto �6   �� 5,      $
��auto �6   �� 5-      $
��auto �6   �� 5.      $
��auto �6   �� 5/      $
��auto �6   �� 50      $
��auto �6   .+�53      �� $��0��6   5   /Omnia.Pie.Bdm.Bootstrapper.Properties.Resources   LabelAccountNameColon#��Z 6   �� 54      $��1��6      LabelAccountNumberColon#��Z 6   �� 55      $��2��6   	   LabelAccountTypeColon#��Z	 6   �� 56      $��3��6   !
   LabelAvailableBalanceColon#��Z
 6   �� 57      $��4��6       LabelNumberOfChequesColon#��Z 6   �� 58      $��5��6      LabelChargesColon#��Z 6   �� 5:      $��0��6   $��1��6   #��[ 6,   ��6\   �� *SelectedAccount.Name+�� 5;      $��1��6   $��1��6   #��[ 6,   ��6\   �� *SelectedAccount.Number+�� 5<      $��2��6   $��1��6   #��[ 6,   ��6\   �� *SelectedAccount.Type+�� 5=      $��3��6   $��1��6   #��[ 6,   ��5>      `� 5?       �� StringFormat$ 
{0} {1:N2}��6   .1�5@      ��    LabelAed �� Source# Z 6   �� 5A       �� Path$&  SelectedAccount.AvailableBalance�6   5B      5D      �� 5E      $��4��6   $��1��6   #��[ 6,   ��6\   �� *NumberOfCheques+�� 5F      $��5��6   $��1��6   #��[ 6,   ��5G      `� 5H      $ 
{0} {1:N2}��6   .1�5I      �� # Z 6   �� 5J      $ Charges�6   5K      5M      5N      � 5O      $��2��6	   $��3��6   $��Bottom=�6*   $��0 24 0 0q�6E    5P      �� 5Q      $
��1.5* �6   �� 5R      $��* �6   �� 5S      $
��2.5* �6   �� 5T      $��* �6   �� 5U      $
��1.5* �6   .+�5X      ��   Button.Negative.Style#��[ 6      
ButtonBack#��Z 5Y      |�.  ��6D   �� *BackCommand+y�5Z      �� *BackVisibility+  BooleanToVisibility �� 	Converter# [  �� FallbackValue$ 	Collapsed���� 5`      $��4��6   #��[ 6      ButtonCancel#��Z 5a       �� IsCancel . |�.  ��6V   �� *CancelCommand+y�5b      �� *CancelVisibility+# [ $ 	Collapsed��5c      5d      5e      