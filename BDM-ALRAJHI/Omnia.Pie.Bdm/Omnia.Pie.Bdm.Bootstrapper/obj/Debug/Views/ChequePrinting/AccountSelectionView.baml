   M S B A M L   `   `   `  ����[  WOmnia.Pie.Vtm.Services.Interface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null�aclr-namespace:Omnia.Pie.Vtm.Services.Interface.Entities;assembly=Omnia.Pie.Vtm.Services.Interface)Omnia.Pie.Vtm.Services.Interface.Entities  5      U QOmnia.Pie.Bdm.Bootstrapper, Version=4.4.3.3, Culture=neutral, PublicKeyToken=nullG(clr-namespace:Omnia.Pie.Bdm.BootstrapperOmnia.Pie.Bdm.Bootstrapper 5	      q=clr-namespace:Omnia.Pie.Bdm.Bootstrapper.Views.ChequePrinting/Omnia.Pie.Bdm.Bootstrapper.Views.ChequePrinting 5
      R NOmnia.Pie.Vtm.Framework, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null�Wclr-namespace:Omnia.Pie.Vtm.Framework.ControlExtenders;assembly=Omnia.Pie.Vtm.Framework(Omnia.Pie.Vtm.Framework.ControlExtenders 5      ]3clr-namespace:Omnia.Pie.Bdm.Bootstrapper.Properties%Omnia.Pie.Bdm.Bootstrapper.Properties 5      J   DOmnia.Pie.Bdm.Bootstrapper.Views.ChequePrinting.AccountSelectionView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      oeaclr-namespace:Omnia.Pie.Vtm.Services.Interface.Entities;assembly=Omnia.Pie.Vtm.Services.Interface     5      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5      0l(clr-namespace:Omnia.Pie.Bdm.Bootstrapper  5	      Ilocal=clr-namespace:Omnia.Pie.Bdm.Bootstrapper.Views.ChequePrinting  5
      _cWclr-namespace:Omnia.Pie.Vtm.Framework.ControlExtenders;assembly=Omnia.Pie.Vtm.Framework  5      ;p3clr-namespace:Omnia.Pie.Bdm.Bootstrapper.Properties  5         BaseView.Style#��[  5        � 	Resources  5        Account.Preview.Template �� DataType7   1Omnia.Pie.Vtm.Services.Interface.Entities.Account � RowDefinitions J� Opacity  Account.Template �� 
TargetType  TextBlock.Normal.Base.Style �� BasedOn �� Property �� Value �� StringFormat	 �� Path & "SelectAccountView.Account.Template
 �� CornerRadius  ColorScheme.TopBorder �Name  TextBlock.Info20.Style  Key  TextBlock.Detail20.Style � ColumnDefinitions5  /Omnia.Pie.Bdm.Bootstrapper.Properties.Resources   LabelNameColon   LabelAccountTypeColon 	 TextBlock.Detail.Style ��Name �� TextAlignment"   LabelSelectAccountWatermark �� Triggers �� Binding �� Value �� 
TargetName *
 &SelectAccountView.AccountDetails.Style  ChequesView.Style  ListBox.Inline.Style��%p  &	       &	     7  &	 �    7  7  7  7 	 7 	 7  &	
 =	    7  &	 �	    7  7  7  �� 5      "  62   .R�5      �  5      �� 5      $
��auto �6   �� 5      $
��auto �6   .+�5      �� ��6   �� *Number+�� 5      $��1��6   $��14(�6   $	 0.8Q�6+   $��Italic$�69   ��6L   �� *Type+5      5      �� 5      "  6*   .R�5      � $��6q�6
     5      ���� 5       '��       " ��6   8   .�5!      ��  � ��$ NoWrap=�6'   5"      5#       5%      �� 5&      $
��auto �6   �� 5'      $
��auto �6   �� 5(      $
��auto �6   .+�5*      �� $��18(�6   ��6   �� *Number+�� 5+      $��1��6   $��13(�6   $	 0.7Q�6+   $��Italic$�69   ��6L   �� *Type+�� 5,      $��2��6   $��16(�6   $��Right=�6+   ��5-      `� 5.      $ 
{0} {1:N2}��6   .1�5/   	   �� $	 Currency�6   �� 50   	   $	 AvailableBalance�6   51   	   53      54      55      �� 56      "  6<   .R�57      �� $
 6��6   $��0q�6   8��  ._�58      � .+�59      � $ PART_Content��6   $��
12 6 12 10q�6"     5:      ���� 5;   	   '��       " ��6   8  .�5<   
   ��  � ��$	 5 3q�6#   �� 5=   
    � ��$
 Left=�60   �� 5>   
    � ��$ NoWrap=�6)   5?   
   �� 5@   	   TextBlock.Detail.Style " ��6.   8  .�5A   
   ��  � ��$	 5 3q�6#   �� 5B   
    � ��$
 Left=�60   �� 5C   
    � ��$ LeftToRight=�6*   �� 5D   
    � ��$ NoWrap=�6)   5E   
   5F   	    5G      �� 5H   	   $��* �6   �� 5I   	   $��* �6   �� 5J   	   $��* �6   �� 5K   	   $��* �6    5M      �� 5N   	   $
��auto �6   �� 5O   	   $
��auto �6   �� 5P   	   $
��auto �6   �� 5Q   	   $
��auto �6   .+�5T      �� $��0��6   #��Z 6   �� 5U      $��1��6   #��Z 6   �� 5W      $��0��6   $��1��6   $��3��6/   8�� ��6s   �� *Name+�� 5X      $��1��6   $��1��6   $��3��6/   8�� ��6s   �� *Type+5Z      �� 5\      $ PART_EmptyMessage��6   $��Center=�6,   $
��12 0q�6G   $y�	Collapsed=�6U   $ Center=�6l   8�� #��Z 5]      5^      5_       5`      �� 5a       6   ��  6&   Z� .a�5b      �� $ PART_Content��6    � y�$ Hidden=�6>   �� 5c      $ PART_EmptyMessage��6    � y�$ Visible=�6C   5d      5f      �� 5h      " ��69   .�5i      ��  �@=�ErrorTemplate 5j      �� 5k      .R�5l      �� 5m      5o      �� 5p       � ��8   6'   5q      �� 5s      " ��6$   8   .�5t      ��  � �� 5u      �� 5v      .R�5w      � $	��175��6   $��Center=�6   $��10q�64    5x   	   �� 5y   
   $
��auto �6   �� 5z   
   $
��auto �6   .+�5|   	   �� $y�Hidden=�6   ��6'   �� *Number+�� 5}   	   $��10q�6   $ Center=�6   $��Center=�66   $��26(�6Q   8�� ��5~      �� *NumberOfCheque+�� 5   	   $��10q�6   $��1��6   $ Center=�6,   $��Center=�6C   $��26(�6^   8�� ��5�      �� *Amount+5�   	   5�      5�      5�      5�      .��5�      � .  (System.Windows.Interactivity.Interaction   	Behaviors 5�      .  (Omnia.Pie.Bdm.Bootstrapper.FocusBehavior  5�       5�      �� 5�      $��* �6   �� 5�      $
��auto �6   .+�5�      � $��Center=�6	    5�      �� 5�      $��1.35* �6   �� 5�      $��0.65* �6   �� 5�      $
��0.5* �6   �� 5�      $��0.95* �6    5�      �� 5�      $
��auto �6   �� 5�      $��* �6   �� 5�      $
��auto �6   �� 5�      $��* �6   �� 5�      $
��auto �6   .+�5�      �� $��Center=�6   $��2��6,   $��0 0 24 3q�6@     TextBlock.Normal24.Style#��[ 6R      LabelSelectAccount#��Z 5�   	   ?  9Omnia.Pie.Vtm.Framework.ControlExtenders.ComboBoxExtender  5�      $��2��6?   $��1��6S   $	��520��6`   $��0 0 24 0q�6l      Title# Z 6~   $��Center=�5�      $
��Left=�5�         ItemTemplate# [ 5�         SelectedItemTemplate# [ 5�      {�5�      �� *AccountSelection+   ItemsSource 5�      �� *
Accounts+   SelectedItem 5�      �� *SelectedAccount+ �� Mode$ TwoWay=�5�      �� 5�      $��2��6   $��0 0 0 3q�6   $��Center=�60   #��[ 6M      LabelCurrency#��Z 5�      �� 5�      $��1��6   $��2��6   $��0 20 0 0q�6,   $��Center=�6>   #��[ 6[   ��5�   	   �� *SelectedAccount.Currency+   NotSpecified  �� TargetNullValue#  Z ! �� FallbackValue#! Z �� 5�      $��3��6   $��0 0 0 3q�6   $��Center=�60   #��[ 6M   "   LabelBalance#��Z" 5�      �� 5�      $��1��6   $��3��6   $��0 20 0 0q�6,   $��Center=�6>    ! TextBlock.Highlighted24.Style#��[ 6[   ��5�   	   �� *" SelectedAccount.AvailableBalance+$ N2��#  Z #! Z �� 5�      $��2��6   $��4��6   $
��0 30q�60   $��Center=�6>   #��[ 5�   	   $N��HPlease select the number of cheques you want to print and press Proceed.��5�   	   �� 5�      $��3��6   $��4��6   $
��0 20q�6.   $��Center=�6<   #��[ 5�      # �� SelectedItem# 5�      �� *ChequeSelection+$ TwoWay=���5�      �� *ChequeSelections+�� 5�      $��4��6   $��4��6   $
��0 30q�60   $��Center=�6>    ! TextBlock.Highlighted20.Style#��[ 5�   	   $   MessageVATInclusive#��Z$ 5�   	   5�      � 5�      $��1��6	   $��3��6   $��Bottom=�6*   $��0 24 0 0q�6E    5�      �� 5�      $
��1.5* �6   �� 5�      $��* �6   �� 5�      $
��2.5* �6   �� 5�      $��* �6   �� 5�      $
��1.5* �6   .+�5�      ��   Button.Negative.Style#��[ 6   %   
ButtonBack#��Z% 5�      |�.  ��6D   �� *BackCommand+y�5�      �� *BackVisibility+  BooleanToVisibility& �� 	Converter#& [ $! 	Collapsed���� 5�      $��2��6     Button.Positive.Style#��[ 6   '   ButtonContinue#��Z' 5�      ( �� 	IsDefault( . |�.  ��6Y   �� *DefaultCommand+y�5�      �� *DefaultVisibility+#& [ $! 	Collapsed���� 5�      $��4��6   #��[ 6   )   ButtonCancel#��Z) 5�      * �� IsCancel* . |�.  ��6V   �� *CancelCommand+y�5�      �� *CancelVisibility+#& [ $! 	Collapsed��5�      5�      5�      