   M S B A M L   `   `   `  ���� V  ROmnia.Pie.Supervisor.Shell, Version=2.8.10.3, Culture=neutral, PublicKeyToken=nullVclr-namespace:Omnia.Pie.Supervisor.Shell.Utilities;assembly=Omnia.Pie.Supervisor.Shell$Omnia.Pie.Supervisor.Shell.Utilities   Omnia.Pie.Vtm.BootstrapperM   GOmnia.Pie.Vtm.Bootstrapper.Views.ChequeDeposit.VerifyChequeSecurityView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     l	converterVclr-namespace:Omnia.Pie.Supervisor.Shell.Utilities;assembly=Omnia.Pie.Supervisor.Shell        BaseView.Style#��[    � 	Resources  A   ;Omnia.Pie.Supervisor.Shell.Utilities.ConfigurationConverter  	Converter��%   &	         .���  � RowDefinitions �� $
��auto �.+�� $��Center=�   �� 
TargetType  TextBlock.Info24.Style �� BasedOn �� Property �� Value  TextBlock.Detail.Style  TextBlock.Detail24.Style��%�   '��       7  &	 C     7  �� " ��8   .���  � ��$
 12 6q���  � ��$ Right=��� " ��8   .���  � ��$
 12 6q���  � ��$
 Left=���  � ��$ LeftToRight=� � ColumnDefinitions �� $��* ��� $��* � �� $
��auto �.+��� -    ��Name$ lstVwImages��$��
0,-680,0,0q�$	��900��$
��1300��$��0��$��2��$��Center=���.  
������ $��0q�$��Disabled=����� *DepositedCheques+ �� UpdateSourceTrigger$ PropertyChanged=�	 �� Mode$	 TwoWay=����� .R�� $��10q� �� $��* �.+��� $��0��$��Center=�$	��Top=�$	��900��$	��900������ 
 �� Path$
 MediaId�����  j� Stretch$
 Fill=� �� ImageSource �� *ValidationImage+$ PropertyChanged=�$	 TwoWay=����� .R���  �� Orientation$ 
Horizontal=�