   M S B A M L   `   `   `  ����Y  UOmnia.Pie.Supervisor.UI.Themes, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nulla5clr-namespace:Omnia.Pie.Supervisor.UI.Themes.Controls'Omnia.Pie.Supervisor.UI.Themes.Controls  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� MergedDictionaries �� Source $   TextBox.Input.Number.Popup.Style �� 
TargetType �� Property �� Value �� Triggers �� Binding �� RelativeSource �� AncestorType	 �� Path
 �� Value " TextBox.Input.Text.Popup.Style # TextBox.Input.Popup.Title.Style  TextBlock.Normal24.Style �� BasedOn  textBox 	 enter  
foreground;    5Omnia.Pie.Supervisor.UI.Themes.Controls.TextBoxHelper �� 
TargetType � RowDefinitions ��Name 
 corner �� CornerRadius J� SnapsToDevicePixels ��Name ��Name  TextBlock.Error12.Style �Name � 	StaysOpen 	 backgroundBrush J� Opacity 
 NullOrEmptyToVisibility �� 	Converter �� ConverterParameter �� TextAlignment ! TextBlock.Highlighted38.Style6   0Omnia.Pie.Supervisor.UI.Themes.Controls.Keyboard  Name �� Triggers P� Property P� Value �� 
TargetName  
enter.edit 	 error ]� 
Conditions �� Property  �� Value 	 hover  TextBox.Input.Device.Status�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      ;c5clr-namespace:Omnia.Pie.Supervisor.UI.Themes.Controls  5        5      �� 5      $ ../Resources/Main.xaml?�6   �� 5      $ TextBlock.xaml?�6   %}  &	        &	 `    &	 �    7  &	 I    7  7  7 
 7  7   '�� f    7  &	 �    7  �� 5
      " �62   .�5      ��  � ��$ Absolute=�6    �� 5       � ��$	 390��6   �� 5       � ��$	 530��6   �� 5       �@�HorizontalOffset$
 3261��6'   �� 5       �@�VerticalOffset$	 191��6%   �� 5       �@�VerticalOffset$	 119��6%   �� 5       �@�AllowsTransparency . 6)   �� 5       � ��$
 Fade=�6%   �� 5       � ��$ LeftToRight=�6$    5      �� 5       6   ��  �� " �$	 FlowDirection�
 RightToLeft6v   .a�5      ��  �@�HorizontalOffset$ 24��6)   5      5      �� 5      " �60   .�5      ��  � ��$ Absolute=�6    �� 5       � ��$	 976��6   �� 5       � ��$	 532��6   �� 5        �@�HorizontalOffset$ 24��6'   �� 5"       �@�VerticalOffset$
 3063��6%   �� 5#       �@�AllowsTransparency . 6)   �� 5$       � ��$
 Fade=�6%   �� 5%       � ��$ LeftToRight=�6$   5&      �� 5(      " ��61   8   .�5)      ��  �@��TextAlignment$ Center=�6$   5*      �� 5,      " ��6	   .�5-      ��  � ��$ 2q�6&   �� 5.       � ��$ 3,7,3,7q�6   �� 5/       � ��# �  6"   �� 50       � ��
 ���� 6!   �� 51       � ��# �  6!   �� 52       �@��
CaretBrush# �  6!   �� 53       � ��$ Center=�6/   �� 54       �@J�SnapsToDevicePixels . 6*   �� 55       �@  ClearWhenBackspace . 69   �� 56       �@  ClearFocusWhenEnter . 6:   �� 57       �@  HandleEnter . 62   �� 58       �@  DataType$ Numeric=�6/   �� 59       �@=�ErrorTemplate 5:      �� 5;      .R�5<      �� 5=      5?      �� 5@       � �� 5A      �� 5B      " ��6   .R�5C      �  5D      �� 5E   	   $��35 �6   �� 5F   	   $
��auto �6   .+�5I      �� $ PART_Border��6   # �  6$   #��z��6L   #��z��6�   #��z��6�    . ._�5J   	   �� $ PART_ContentHost��6   |�.  $��Hidden=�6B   $��Hidden=�6i   5K   	   �� 5M      $ 
PART_Error��6   $��1��6&   $y�	Collapsed=�63   8��  ��5N      ��  �� *TemplatedParent+$)	 #(Validation.Errors)[0].ErrorContent�� 5P      $ 
PART_Popup��6   #���  6"   ��.   . # z 5R      .��5S   	   � .+�5T   
   �� $ 12��6   #��� 	 6#   $
 0.95Q�6R   � 5U   
    5V      �� 5W      $
��auto �6   �� 5X      $
��auto �6   �� 5Y      $��* �6   .+�5\      �� $��12q�6   8�� ��5]      ��  �� *TemplatedParent+$	 (c:Keyboard.Title)�y�5^      ��  �� *Self+$
	 Text�8  $ False���� 5`      $ 
PART_Title��6   $��1��6)   $��
35 35 35 0q�66   $ Right=�6J   8�� ��5a      ��  �� *TemplatedParent+$
	 Text�  5c      $ PART_Keyboard��6   $��2��6-   $��0 0 0 33q�6:   $��Center=�6L   $��Bottom=�6i   5d      5e   
   5f   	   5g       5h      P� 5i       �@��
IsReadOnly .  . �5j   	   �� $ PART_Border��6   # �  6@    � ���� 5k   	   # �  6&    � ��5l   	   P� 5m       �@=�HasError . . �5n   	   �� $ PART_Border��6    � ��$ 2q�6D   �� 5o   	   $ PART_Border��6   # �  6@    � ���� 5p   	   $ 
PART_Error��6    � y�$ Visible=�6>   5q   	   ]� 5r       5s   	   �� 5t   
    �@J�IsKeyboardFocused  . 61   �� 5u   
    �@��
IsReadOnly  .  6*   /�5w   	   �� 5x   
   $ PART_Border��6   # �  6A    � ���� 5y   
   $ 
PART_Popup��6    � �� . 6;   5{   	   P� 5|       �@  DataType$ Numeric=�64   . �5}   	   �� $ 
PART_Popup��6   8   � ��69   �� 5~   	   $ 
PART_Title��6    � ��$ 
60 22 60 0q�6:   �� 5   	   $ PART_Keyboard��6    �@ KeyboardView$ Keypad=�6C   �� 5�   	   $ PART_Keyboard��6    � ��$ 0 0 0 58q�6=   5�   	   P� 5�       �@  DataType$ Alphanumeric=�64   . �5�   	   �� $ PART_Keyboard��6    �@ KeyboardView$ Alphanumeric=�6C   �� 5�   	   $ 
PART_Title��6    � ��$ 
35 12 35 0q�6:   5�   	   5�      5�      5�      �� 5�      " ��6	   8   6'   �� 5�      " ��60   8   .�5�   
   ��  � ��$ 20(�6%   �� 5�   
    � ��$ 30q�6#   �� 5�   
    � ��$ Center=�6.   5�      5�      