   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5      [ WOmnia.Pie.Vtm.Services.Interface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null�aclr-namespace:Omnia.Pie.Vtm.Services.Interface.Entities;assembly=Omnia.Pie.Vtm.Services.Interface)Omnia.Pie.Vtm.Services.Interface.Entities 5      U QOmnia.Pie.Bdm.Bootstrapper, Version=4.4.3.3, Culture=neutral, PublicKeyToken=nullG(clr-namespace:Omnia.Pie.Bdm.BootstrapperOmnia.Pie.Bdm.Bootstrapper 5
      R NOmnia.Pie.Vtm.Framework, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null�Wclr-namespace:Omnia.Pie.Vtm.Framework.ControlExtenders;assembly=Omnia.Pie.Vtm.Framework(Omnia.Pie.Vtm.Framework.ControlExtenders 5      ]3clr-namespace:Omnia.Pie.Bdm.Bootstrapper.Properties%Omnia.Pie.Bdm.Bootstrapper.Properties 5      1&clr-namespace:System;assembly=mscorlibSystem  5      Q MOmnia.Pie.Vtm.Workflow, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null�Vclr-namespace:Omnia.Pie.Vtm.Workflow.StatementPrinting;assembly=Omnia.Pie.Vtm.Workflow(Omnia.Pie.Vtm.Workflow.StatementPrinting 5      I2clr-namespace:System.Collections;assembly=mscorlibSystem.Collections  5      N   HOmnia.Pie.Bdm.Bootstrapper.Views.StatementPrinting.StatementPrintingView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R	 NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml  	   5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006  	   5      @d2http://schemas.microsoft.com/expression/blend/2008  	   5      4s&clr-namespace:System;assembly=mscorlib  	   5      oeaclr-namespace:Omnia.Pie.Vtm.Services.Interface.Entities;assembly=Omnia.Pie.Vtm.Services.Interface  	   5      c
 _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity 
 5	      0l(clr-namespace:Omnia.Pie.Bdm.Bootstrapper 
 5
      _cWclr-namespace:Omnia.Pie.Vtm.Framework.ControlExtenders;assembly=Omnia.Pie.Vtm.Framework 
 5      ;p3clr-namespace:Omnia.Pie.Bdm.Bootstrapper.Properties 
 5      0sys&clr-namespace:System;assembly=mscorlib 
 5      `stmVclr-namespace:Omnia.Pie.Vtm.Workflow.StatementPrinting;assembly=Omnia.Pie.Vtm.Workflow 
 5      DCollections2clr-namespace:System.Collections;assembly=mscorlib 
 5         BaseView.Style#��[  5        � 	Resources  5        Account.Preview.Template �� DataType7  1Omnia.Pie.Vtm.Services.Interface.Entities.Account � RowDefinitions J� Opacity  Account.Template �� 
TargetType  TextBlock.Normal.Base.Style �� BasedOn �� Property �� Value �� StringFormat	 �� Path & "SelectAccountView.Account.Template
 �� CornerRadius  ColorScheme.TopBorder �Name  TextBlock.Info20.Style  Key  TextBlock.Detail20.Style � ColumnDefinitions5  /Omnia.Pie.Bdm.Bootstrapper.Properties.Resources   LabelNameColon   LabelAccountTypeColon 	 TextBlock.Detail.Style ��Name �� TextAlignment"   LabelSelectAccountWatermark �� Triggers �� Binding �� Value �� 
TargetName *
 &SelectAccountView.AccountDetails.Style  StatementView.Style  ListBox.Inline.Style��%�  &	       &	     7  &	 �    7  7  7  7 	 7 	 7  &	
 =	    7  &	 �	    7  7  �� 5   
   "  68   .R�5      �  5      �� 5      $
��auto �6$   �� 5      $
��auto �6$   .+�5      �� ��6   �� *Number+�� 5      $��1��6   $��14(�6)   $	 0.8Q�67   $��Italic$�6E   ��6X   �� *Type+5      5      �� 5    
   "  60   .R�5!      � $��6q�6     5"      ���� 5#      '��       " ��6   8   .�5$      ��  � ��$ NoWrap=�69   5%      5&       5(      �� 5)      $
��auto �6$   �� 5*      $
��auto �6$   �� 5+      $
��auto �6$   .+�5-      �� $��18(�6   ��6*   �� *Number+�� 5.      $��1��6   $��13(�6)   $	 0.7Q�67   $��Italic$�6E   ��6X   �� *Type+�� 5/      $��2��6   $��16(�6)   $��Right=�67   ��50      `� 51      $ 
{0} {1:N2}��6'   .1�52      �� $	 Currency�6&   �� 53      $	 AvailableBalance�6&   54      56      57      58      �� 5:   
   "  6B   .R�5;      �� $
 6��6   $��0q�6&   8��  ._�5<      � .+�5=      � $ PART_Content��6   $��
12 6 12 10q�61     5>      ���� 5?      '��       " ��6$   8  .�5@   "   ��  � ��$	 5 3q�6;   �� 5A   "    � ��$
 Left=�6H   �� 5B   "    � ��$ NoWrap=�6A   5C      �� 5D      TextBlock.Detail.Style " ��6C   8  .�5E   "   ��  � ��$	 5 3q�6;   �� 5F   "    � ��$
 Left=�6H   �� 5G   "    � ��$ LeftToRight=�6B   �� 5H   "    � ��$ NoWrap=�6A   5I      5J       5K      �� 5L      $��* �6/   �� 5M      $��* �6/   �� 5N      $��* �6/   �� 5O      $��* �6/    5Q      �� 5R      $
��auto �6,   �� 5S      $
��auto �6,   �� 5T      $
��auto �6,   �� 5U      $
��auto �6,   .+�5X      �� $��0��6$   #��Z 61   �� 5Y      $��1��6$   #��Z 61   �� 5[      $��0��6$   $��1��61   $��3��6A   8�� ��6�   �� *Name+�� 5\      $��1��6$   $��1��61   $��3��6A   8�� ��6�   �� *Type+5]      �� 5_      $ PART_EmptyMessage��6    $��Center=�6;   $
��12 0q�6V   $y�	Collapsed=�6d   $ Center=�6{   8�� #��Z 5`      5a      5b       5c      �� 5d       6   ��  62   Z� .a�5e      �� $ PART_Content��6    � y�$ Hidden=�6M   �� 5f      $ PART_EmptyMessage��6    � y�$ Visible=�6R   5g      5i      �� 5k   
   " ��6?   .�5l      ��  �@=�ErrorTemplate 5m      �� 5n      .R�5o      �� 5p      5r      �� 5s       � ��8   60   5t      �� 5v   
   " ��6,   8   .�5w      ��  � �� 5x      �� 5y      .R�5z      � $	��175��6   $��Center=�6+   $��10q�6F    5{      �� 5|   "   $
��auto �60   �� 5}   "   $
��auto �60   .+�5      �� $y�Hidden=�6(   ��6<   �� *Number+�� 5�      $ Center=�6(   $��Center=�6?   $��26(�6Z   8�� ��5�      �� *Period+5�      5�      5�      5�      5�      .��5�      �   5�   
     TextBlock.Info24.Style  TextBlock.Detail24.Style��%4  '��       7  &		 v     7  �� 5�      " ��6   8   .�5�      ��  � ��$
 12 6q�6+   �� 5�       � ��$ Right=�68   5�      �� 5�      " ��63   8   .�5�      ��  � ��$
 12 6q�6+   �� 5�       � ��$
 Left=�68   �� 5�       � ��$ LeftToRight=�62   5�      5�       5�   
   �� 5�      $��* �6   6)   �� 5�      $
��auto �6   6,    5�   
   �� 5�      6!   .+�5�   
   � $��Center=�6   . 
 (System.Windows.Interactivity.Interaction   	Behaviors 5�      .  (Omnia.Pie.Bdm.Bootstrapper.FocusBehavior  5�       5�      �� 5�      $
��auto �6    �� 5�      $��* �6    �� 5�      $
��auto �6    �� 5�      $��* �6    �� 5�      $
��auto �6    �� 5�      $
��auto �6    �� 5�      $
��auto �6     5�      �� 5�      $��1.80* �6#   �� 5�      $��0.65* �6#   �� 5�      $
��0.5* �6#   �� 5�      $��0.95* �6#   .+�5�      �� $	��420��6     Button.Base.Style.Rectangle#��[ 6!   .��5�      � $	��440��6   $	��100��6#    5�      �� 5�      $��2* �6(   �� 5�      $��8* �6(    5�      �� 5�      $	��15* �6+   �� 5�      $	��73* �6+   .+�5�      �� $��0,0,0,1q�6   
���   �$��2��6K   ._�5�      �� $��Center=�6$   $	��0,0q�6A   $��0��6N   $	��Top=�6[      LabelSelectAccount#��Z 6s   #��[ 6�   5�      ?  9Omnia.Pie.Vtm.Framework.ControlExtenders.ComboBoxExtender  5�      $��2��6P   $��1��6d   $	��414��6q   $��0 0 0 0q�6~      Title# Z 6�   $��Center=�5�      $
��Left=�5�         ItemTemplate# [ 5�         SelectedItemTemplate# [ 5�      {�5�   )   �� *AccountSelection+   ItemsSource 5�      �� *
Accounts+   SelectedItem 5�      �� *SelectedAccount+ �� Mode$ TwoWay=�5�      5�      5�      �� 5�      $��1��6   $��0 0 0 3q�6(   $��Center=�69     TextBlock.Normal24.Style#��[ 6V      LabelCurrency#��Z 5�      �� 5�      $��0��6   $��1��6%   $��0 20 0 0q�65   $��Center=�6G   #��[ 6d   ��5�   	   �� *SelectedAccount.Currency+   NotSpecified  �� TargetNullValue#  Z ! �� FallbackValue#! Z �� 5�      $��2��6   $��0��6%   "   LabelStatementPeriod#��Z" 65    ! TextBlock.Highlighted30.Style#��[ 5�   	   $��25(�6@   $��4��6N   $��	45,25,0,0q�6b   $
��Left=�6u   $	��Top=�6�   �� 5�      $��3��6   #   LabelStartDate#��Z# 6%   #��[ 6R   $��25(�6�   $��Center=�6�   �� 5�      $��3��6   $��0��6%   $��Center=�65   $��	250,5,0,0q�6R   #��[	 6e   ��6�   �� *	StartDate+$ {0:yyyy-MM-dd}���� 5�      $��4��6   $   LabelEndDate#��Z$ 6%   #��[ 6P   $��25(�6�   $��Center=�6�   �� 5�      $��4��6   $��0��6%   $��Center=�65   $��	250,5,0,0q�6R   #��[	 6e   ��6�   �� *	EndDate+$ {0:yyyy-MM-dd}��(  "System.Windows.Controls.DatePicker  5�      -   5        %  Name$% dpStartDate��5�   &   $��3��6   $��0��6;   |�.  $��Right=�6�   $��10,5,0,0q�5�      $	��Top=�5�        CalendarStyle&   CalendarStyle#& [ 5�      $	��165��6H   '   SelectedDate' 5�   ]   �� *	StartDate+(   DisplayDateEnd( 5�      �� *	EndDate+  5�      :  4System.Windows.Controls.Primitives.DatePickerTextBox��%E   '        �� 5�      "  6   .�5�      -   5        5�      5�      5�        5�      -   5        $% 	dpEndDate��5�   &   $��4��6   $��0��69   |�.  $��Right=�6}   $��10,5,0,0q�5�      $	��Top=�5�      ) �� Today#( Z) 5�      #& [ 5�      $	��165��6H   ' 5�   I   �� *	EndDate+*   DisplayDateStart* 5�      �� *	StartDate+  5�      ��%E   '        �� 5�      "  6   .�5      -   5        5     5     5     5     � 5  
   $��1��6   $��3��6   $��Bottom=�60   $��0 34 0 0q�6K    5	     �� 5
     $
��1.5* �6#   �� 5     $
��3.0* �6#   �� 5     $
��2.0* �6#   �� 5     $
��2.5* �6#   �� 5     $
��1.5* �6#   .+�5     ��   Button.Negative.Style#��[ 6   +   
ButtonBack#��Z+ 5     ��62   �� *BackCommand+y�5     �� *BackVisibility+  BooleanToVisibility, �� 	Converter#, [ $! 	Collapsed���� 5     $��1��6     Button.Positive.Style#��[ 6%   -   ButtonContinue#��Z- 5     . �� 	IsDefault. . ��6G   �� *DefaultCommand+y�5     �� *DefaultVisibility+#, [ $! 	Collapsed���� 5     $��3��6   #��[ 6%   /   ButtonCancel#��Z/ 5     0 �� IsCancel0 . ��6D   �� *CancelCommand+y�5     �� *CancelVisibility+#, [ $! 	Collapsed��5     5     5     