   M S B A M L   `   `   `  ����V  ROmnia.Pie.Supervisor.Shell, Version=2.8.10.3, Culture=neutral, PublicKeyToken=null_4clr-namespace:Omnia.Pie.Supervisor.Shell.Views.Pages&Omnia.Pie.Supervisor.Shell.Views.Pages  5      Y UOmnia.Pie.Supervisor.UI.Themes, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null�lclr-namespace:Omnia.Pie.Supervisor.UI.Themes.Controls.DateTimeHelper;assembly=Omnia.Pie.Supervisor.UI.Themes6Omnia.Pie.Supervisor.UI.Themes.Controls.DateTimeHelper 5      �]clr-namespace:Omnia.Pie.Supervisor.UI.Themes.Controls;assembly=Omnia.Pie.Supervisor.UI.Themes'Omnia.Pie.Supervisor.UI.Themes.Controls 5      [2clr-namespace:Omnia.Pie.Supervisor.Shell.Utilities$Omnia.Pie.Supervisor.Shell.Utilities  5	      >    8Omnia.Pie.Supervisor.Shell.Views.Pages.ConfigurationView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Flocal4clr-namespace:Omnia.Pie.Supervisor.Shell.Views.Pages     5      zclclr-namespace:Omnia.Pie.Supervisor.UI.Themes.Controls.DateTimeHelper;assembly=Omnia.Pie.Supervisor.UI.Themes     5      ke]clr-namespace:Omnia.Pie.Supervisor.UI.Themes.Controls;assembly=Omnia.Pie.Supervisor.UI.Themes     5      H	converter2clr-namespace:Omnia.Pie.Supervisor.Shell.Utilities     5	        � 	Resources  5       �� MergedDictionaries �� SourceA   ;Omnia.Pie.Supervisor.Shell.Utilities.ConfigurationConverter   	Converter�� 5       5      �� 5      $C =/Omnia.Pie.Supervisor.UI.Themes;component/Resources/Main.xaml?�6   �� 5      $@ :/Omnia.Pie.Supervisor.UI.Themes;component/Styles/Main.xaml?�6   %   &	          5      6&   5      .��5      � $	��450��6   $��10q�6    � ColumnDefinitions 5      �� 5      $��1* �6   �� 5      $��2* �6    � RowDefinitions 5      �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5       $
��auto �6   �� 5!      $
��auto �6   �� 5"      $
��auto �6   �� 5#      $
��auto �6   �� 5$      $
��auto �6   �� 5%      $
��auto �6   �� 5&      $
��auto �6   �� 5'      $
��auto �6   �� 5(      $
��auto �6     5*        textBox �� BasedOn �� 
TargetType �� Property �� Value 
 button 	 label��%�  '��       7  '�� R     7  '��     7  �� 5+      8   " ��6.   .�5,      ��  � ��$ 0 0 0 5q�6   5-      �� 5.      8   " ��6-   .�5/      ��  � ��$ 0 3 0 15q�6   �� 50       � ��$ Center=�6,   �� 51       � ��$ 0q�6    �� 52       � ��$ Apply��6    53      �� 54      8   " ��6,   .�55      ��  � ��$
 Left=�6,   �� 56       � ��$ Center=�6*   57      58      .+�5:      �� $��0��6
   $��0��6   $��	Server IP��6'   � 5;      $��0��6	   $��1��6    5<      �� 5=      $��3* �6   �� 5>      $��* �6   �� 5?      $��3* �6   �� 5@      $��* �6   �� 5A      $��3* �6   �� 5B      $��* �6   �� 5C      $��3* �6     5E      ��%�   '��       7  �� 5F      8   " ��6-   .�5G      ��  � ��$
 Left=�6-   �� 5H       � ��$ Bottom=�6+   �� 5I       � ��$ .��6!   5J      5K      .+�5L      �� -   5        	 ��Name$	 txtServerIP1��5L      $��0��6   J  DOmnia.Pie.Supervisor.UI.Themes.Controls.DateTimeHelper.TextBoxHelper
   DataType$
 Numeric=�6U   ��63   �� *ServerIpAddress1+6U   �� 5M      $��1��6   �� 5N      -   5        $	 txtServerIP2��5N      $��2��6   $
 Numeric=�6U   ��63   �� *ServerIpAddress2+6U   �� 5O      $��3��6   �� 5P      -   5        $	 txtServerIP3��5P      $��4��6   $
 Numeric=�6U   ��63   �� *ServerIpAddress3+6U   �� 5Q      $��5��6   �� 5R      -   5        $	 txtServerIP4��5R      $��6��6   $
 Numeric=�6U   ��63   �� *ServerIpAddress4+6U   5S      �� 5U      $��1��6
   $��0��6   $��Server Port��6'   �� 5V      -   5        $	 txtServerPort��5V      $��1��6#   $��1��60   $
 Numeric=�6\   ��6@   �� *
ServerPort+6\   �� 5X      $��2��6   $��1��6   ��6(   �� *ApplyServerSettings+��5Y      `� 5Z       `� 	Converter# [  6   .1�5[      ��  �� Path$
 Text�6    �� ElementName$ txtServerIP��6   �� 5\      $
 Text�6   $ txtServerPort��6   5]      5_      �� 5c      $��3��6
   $��0��6   $��Terminal IP��6'   � 5d      $��3��6	   $��1��6    5e      �� 5f      $��3* �6   �� 5g      $��* �6   �� 5h      $��3* �6   �� 5i      $��* �6   �� 5j      $��3* �6   �� 5k      $��* �6   �� 5l      $��3* �6     5n      ��%�   '��       7  �� 5o      8   " ��6-   .�5p      ��  � ��$
 Left=�6-   �� 5q       � ��$ Bottom=�6+   �� 5r       � ��$ .��6!   5s      5t      .+�5u      �� -   5        $	 txtMachineIP1��5u      $��0��6   $
 Numeric=�6U   ��64   �� *LocalIpAddress1+6U   �� 5v      $��1��6   �� 5w      -   5        $	 txtMachineIP2��5w      $��2��6   $
 Numeric=�6U   ��64   �� *LocalIpAddress2+6U   �� 5x      $��3��6   �� 5y      -   5        $	 txtMachineIP3��5y      $��4��6   $
 Numeric=�6U   ��64   �� *LocalIpAddress3+6U   �� 5z      $��5��6   �� 5{      -	   5        $	 txtMachineIP4��5{      $��6��6   $
 Numeric=�6U   ��64   �� *LocalIpAddress4+6U   5|      �� 5~      $��4��6   $��1��6   ��6(   �� *ApplyMachineIP+��5      `� 5�      # [  6   .1�5�      �� $
 Text�6   $ txtMachineIP��6   5�      5�      �� 5�      $��0��6
   $��5��6   $��	Time Zone��6'   �� 5�      -
   5         ��Name$ txtTimeZone��5�   *   $��1��6   $��5��6   $��Center=�5�      $��Center=�5�      $��0 0 0 5q�5�      ��5�      �� *	TimeZones+ �� DisplayMemberPath 5�      �� *StandardName+ �� SelectedItem 5�      �� *SelectedTimeZone+ �� UpdateSourceTrigger$ PropertyChanged=�5�      �� 5�      $��0��6
   $��6��6   $
��Date��6'   �� 5�      -   5        $	 txtSelectedDate��5�      $��6��5�      $��1��5�      6  0Omnia.Pie.Supervisor.UI.Themes.Controls.Keyboard   Title$ Date Selection��5�      $

 Date=�5�         HandleEnter .     SelectedDate 5�      �� *MachineDate+ �� Mode$ TwoWay=�$ PropertyChanged=�5�      �� 5�      $��7��6   $��1��6   ��6(   �� *ApplyMachineDateTime+��5�      `� 5�      # [  6   .1�5�      �� $ SelectedValue�6   $ txtTimeZone��6$   �� 5�      $ SelectedValue�6   $ txtSelectedDate��6$   5�      5�      �� 5�      $��8��6
   $��0��6   $��Terminal ID��6'   �� 5�      -   5        $	 txtTerminalID��5�      $��8��6#   $��1��60   ;  5Omnia.Pie.Supervisor.UI.Themes.Controls.TextBoxHelper   DataType$ Alphanumeric=�6\   ��6@   �� *
TerminalID+6\   �� 5�      $��9��6
   $��0��6   $��	Branch ID��6'   $
��Left=�6;   �� 5�      -   5        $	 txtBranchID��5�      $��9��6!   $��1��6.   $
 Numeric=�6X   ��6>   �� *
BranchID+6X   �� 5�      $��10��6   $��1��6   ��6)   �� *ApplyMachineCommon+��5�      `� 5�      # [  6   .1�5�      �� $
 Text�6   $ txtTerminalID��6   �� 5�      $
 Text�6   $ txtBranchID��6   5�      5�      5�      5�      