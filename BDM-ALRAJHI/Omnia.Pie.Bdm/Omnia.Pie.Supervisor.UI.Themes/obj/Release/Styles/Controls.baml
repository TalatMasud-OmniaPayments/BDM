   M S B A M L   `   `   `  ���� O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� MergedDictionaries �� Source   bigFont  
normalFont  	smallFont 
 corner �� TopLeft �� TopRight �� 
BottomLeft �� BottomRight  corner.right 
 margin  margin.left  margin.right  margin.list 	 padding �� 
TargetType �� Property 
 backgroundBrush �� Value	 �� 
TargetType
 �� CornerRadius J� Opacity �� Orientation K� IsItemsHost �Name ��Name �� Triggers P� Property P� Value �� 
TargetName � RowDefinitions ]�Name ]� Thumb n�Name ]� IncreaseRepeatButton ��Name ]� DecreaseRepeatButton P� 
SourceName  ButtonSelectBrush  	DarkBrush �� Angle  listViewScrollBarStyle J� SnapsToDevicePixels J� IsMouseOver �� StrokeDashArray  ��Name! �� ContentStringFormat" �� ContentStringFormat# F�Name$ �� Stretch% ^� 
Conditions& �� Binding' �� ElementName( �� Value) �� 	Resources* � 	Resources+ �� Padding  gridListView  listViewItem  selected  listViewItem.left, �� BasedOn  listViewItem.list  off 
 border 	 enter- ��Name  
foreground 	 label  	darkLabel  foreground.dark  darkLabel.textBlock. �� Triggers 
 header 
 button3   -System.Windows.Media.Effects.DropShadowEffect/    ShadowDepth0    
BlurRadius1 J� RenderTransformOrigin  button.enabled  button.disabled  on2 �� ScaleX3 �� ScaleY   toggleButton ! supervisorMode " button.hover 	# alarm $ DeviceStatusProgressBar4 ��Name5 � ColumnDefinitions6 �� Path7 �� RelativeSource8 j� Stretch9 !� Visual�� D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     1system&clr-namespace:System;assembly=mscorlib    �� $ ../Resources/Main.xaml?��� $ ColorScheme.xaml?�%[.  &	        &	      &	      &	 $     &	 I     &	 n     &	 y     &	 �     &	 �     &		 �     ']� �     7 
 '�� �     'n� �    '�� S    &	 ]    '�� ]    &	 �    &	     7  7  &	     7  7  &	 �    7  7  7  '�� J    7  &	 v    7  'G� �    7   7  7  '�� [    7  7  &	 �    7  7  &	 �    7 	 7  7  &	 �    7 	 7   7  7  &	 	!    7 	 7 
 7  7  7  &	 �!    7  7  7  7  '�� X$    7  &	  g$    7  &	! v$    7  7  7  7 " 7  7  7 # &	$ [&    X�18Q�X�14Q�X�10Q��� $ 3Q�$ 3Q�$ 3Q�$ 3Q��� $ 3Q�$ 0Q�$ 3Q�$ 0Q�u�5q�u�0,2,2,2q�u�2,2,0,2q�u�	10 3 10 3q�u�6q��� " ]�.���  � ��8   �� " ��.���  �@�OverridesDefaultStyle . ��  �@J�SnapsToDevicePixels . ��  � �� �� "	 ��.R��� ���	#FF2453A2$
 5�����	#FFF19C00$��2q�$
 0.96Q�._��� }�. $ Vertical=� . $��5,4,5,4q��� " n�.���  � �� �� "	 n�.R�� $
 Grid��.+��� $��Stretch=�$��Stretch=�$
��Auto��$
��Auto��
������ �� $ CornerScrollBarRectangle��$
 4��$��Stretch=�$��Stretch=�$��10��$
��Auto��$��0,1,0,1q�
��� E�� P�  �@�Tag$ 
Horizontal��. ��� $ CornerScrollBarRectangle�� � ��$
 Auto���� $ CornerScrollBarRectangle�� � ��$ 6���� " ��.���  �@��IsFlicksEnabled .  ��  � �� �#ADABAB��  � ��
 ���� ��  � ��$ 10����  � �� �� "	 ��.R�� $ GridRoot��$��10��#��z�� �� $��0.00001* �.+�]� $ 
PART_Track��$��0����. |�.   n� $ Thumb��#��z�� �� $ PageUp��$��ScrollBar.PageDownCommand��$ 0Q�|�.   �� $ PageDown��$��ScrollBar.PageUpCommand��$ 0Q�|�.   P� $ Thumb�� �@J�IsMouseOver . . ��� # �  $ Thumb�� � ��P� $ Thumb�� �@n�
IsDragging . . ��� # �  $ Thumb�� � ��P�  � {� .  . ��� $ Thumb�� � y�$ 	Collapsed=�P�  �@��Orientation$ 
Horizontal=�. ��� $ GridRoot�� �@�LayoutTransform �� $	 -90Q��� $ 
PART_Track�� �@�LayoutTransform �� $	 -90Q���  � ��$
 Auto����  � ��$ 8���� $ Thumb�� �@�Tag$ 
Horizontal���� $ PageDown�� � ��$ ScrollBar.PageLeftCommand���� $ PageUp�� � ��$  ScrollBar.PageRightCommand���� " ��.���  � ��$ 6 0 0 0q���  � ��
 ���� ��  � ��$ 0q���  � ��$ 25����  � �� �� "	 ��.R�� $ Bg�� .  �� #��� q��� $��1E-05* ��� #��� q�.+��� #��z��#��z��#��z��$��1���� $ PART_LineUpButton��$y�Hidden=�$��ScrollBar.LineUpCommand��#{�z ���� " ��.���  �@�FocusVisualStyle �� .���  � �� �� .R��� $��2q� . #��� ��$��1��$	 1 2R���  � ��$ 1q���  � ��$ Center=���  � ��$ Center=���  � ��$ 1q���  � |� .  ��  � �� .  ��  � �� �� "	 ��.R��� $ border�����	#FFF0F0F0$��1q����	#FFF0F0F0 . ._��� $  contentPresenter��#��z��#��z��#" z! |�.  #��z��#��z��# z #��z�� P�  �@J�IsMouseOver . . ��� $ border�� � �� �	#FFDADADA�� $ border�� � �� �	#FFDADADAP�  � �� . . ��� $ border�� � �� �	#FF606060�� $ border�� � �� �	#FF606060P�  � {� .  . ��� $ contentPresenter�� �@J�Opacity$
 0.56Q��� $ border�� � �� �	#FFF0F0F0�� $ border�� � �� �	#FFF0F0F0.��F� $# ArrowTop�������     	= �     	= ��[ ��[ �    ��[ �g5 �%& �g5 �%& ��g5 �%& ��j ��[ ��j ��[ ���j ��[ ��j  	= ��j  	= ���j  	= �g5  � �g5  � ��g5  �  	=  	= 	���	#FF606060$��3,4,3,3q�$$ Uniform=�]� $ 
PART_Track����. #{�z $��1�� �� $��ScrollBar.PageUpCommand������ " ��.���  �@J�IsHitTestVisible .  ��  �@�OverridesDefaultStyle . ��  � ��
 ���� ��  � |� .  ��  � �� .  ��  � �� �� "	 ��.R��� #��z��#��z��#��z�� �� $��ScrollBar.PageDownCommand������ " ��.���  �@J�IsHitTestVisible .  ��  �@�OverridesDefaultStyle . ��  � ��
 ���� ��  � |� .  ��  � �� .  ��  � �� �� "	 ��.R��� #��z��#��z��#��z�� n� ���� " n�.���  �@�OverridesDefaultStyle . ��  � �� .  ��  � ��$ Stretch=���  � �� �� "	 n�.R��� $ border��$
 5�����	#FFA6A6A6#��z�� . #��z�� P�  �@J�IsMouseOver . . ��� $ border�� � �� �	#FFA6A6A6P�  �@n�
IsDragging . . ��� $ border�� � �� �	#FFD1D1D1�� $ PART_LineDownButton��$y�Hidden=�$��ScrollBar.LineDownCommand��#{�z $��2������ " ��.���  �@�FocusVisualStyle �� .���  � �� �� .R��� $��2q� . #��� ��$��1��$	 1 2R���  � ��$ 1q���  � ��$ Center=���  � ��$ Center=���  � ��$ 1q���  � |� .  ��  � �� .  ��  � �� �� "	 ��.R��� $ border�����	#FFF0F0F0$��1q����	#FFF0F0F0 . ._��� $  contentPresenter��#��z��#��z��#" z! |�.  #��z��#��z��# z #��z�� P�  �@J�IsMouseOver . . ��� $ border�� � �� �	#FFDADADA�� $ border�� � �� �	#FFDADADAP�  � �� . . ��� $ border�� � �� �	#FF606060�� $ border�� � �� �	#FF606060P�  � {� .  . ��� $ contentPresenter�� �@J�Opacity$
 0.56Q��� $ border�� � �� �	#FFF0F0F0�� $ border�� � �� �	#FFF0F0F0.��F� $# ArrowBottom�������    �%& �    �%&  �  � �     � �g5  	= �g5  	= ��g5  	= ��j  � ��j  � ���j  � ��j �%& ��j �%& ���j �%& �g5 ��[ �g5 ��[ ��g5 ��[ �%& �%& 	���	#FF606060$��3,4,3,3q�$$ Uniform=� ^� % �� & �� *IsMouseOver+$' PART_LineDownButton��( true�� & �� *	IsPressed+$' PART_LineDownButton��( true.0��� $ ArrowBottom�� � ��
 �����^� % �� & �� *IsMouseOver+$' PART_LineUpButton��( true�� & �� *	IsPressed+$' PART_LineUpButton��( true.0��� $ ArrowTop�� � ��
 �����^� % �� & �� *IsMouseOver+$' PART_LineDownButton��( true�� & �� *	IsPressed+$' PART_LineDownButton��	( false.0��� $ ArrowBottom�� � ��
 �   �^� % �� & �� *IsMouseOver+$' PART_LineUpButton��( true�� & �� *	IsPressed+$' PART_LineUpButton��	( false.0��� $ ArrowTop�� � ��
 �   �P�  � {� .  . ��� $ ArrowTop�� � �� �	#FFBFBFBF�� $ ArrowBottom�� � �� �	#FFBFBFBF�� " ��) ���� '��       " ��.���  �@��Padding$ 0q�.���  � ��
 ���� ��  � ��$ 0q���  � ��$ 0q���  � ��$
 Auto=���  � �� .  ��  � �� �� "	 ��* ��.R��� |�.  #��z��.���� 
������ $��0q�
������ $+ 0q� . ._��� # z �� " ��) ���� '��       " ��.���  �@��Padding$ 0q�.���  � ��
 ���� ��  � ��$ 0q���  � ��$ 0q��� " ��) ���� '��       " ��.���  �@��CornerRadius8   .���  � ��$ 0q���  � ��$ 0q���  � �� �� "	 ��.R��� #��z��# z ._���  P�  �@��
IsSelected . . ��� 8   � ���� " ��8,   ) ���� '��       " ��.���  �@��CornerRadius8  ��  �@��PanningMode$ VerticalFirst=��� " ��8,   .���  � ��$ Center=���  � ��$ Center=���  � �� �� "	 ��.R��� $ border��#��z��# z 8
  ._���  P�  �@��
IsSelected . . ��� 8   � ���� " ��.���  �@��CornerRadius8   �� " ��.���  �@��CornerRadius8   �� " G�.���  � ��8   ��  � ��$
 Bold"���  � ��$ 2q���  � ��8  ��  � ��
 ���� ��  � ��$ 10q���  � �� �� "	 G�.R��� $ border��#��z��#��z��#��z�� . 8
  ._��� $- PART_ContentHost��|�.  $��Hidden=�$��Hidden=� P�  � {� .  . ��� $ border�� �@J�Opacity$
 0.56Q�P�  �@J�IsKeyboardFocused . . ��� $ border�� � �� �	#FF569DE5�� " ��.���  � ��8   ��  � ��8  ��  � ��$ Center=��� " ��.���  � ��8   ��  � ��8  ��  � ��$ Center=��� " ��.���  � ��$ SemiBold"���  � �� �	#FFCED677��  � ��8   ��  � ��8  ��  � ��8  ��  � ��$ Center=���  � ��$ Stretch=���  � ��$ CharacterEllipsis=���  � ��$ Center=��� " ��.���  � �� �	#FFCED677��  �@��Padding8   ��  � ��8  ��  � ��8  ��  � ��$ Center=���  � ��$ Stretch=���  � ��$ CharacterEllipsis=�. P�  � �� Z� . ���  � y�$ 	Collapsed=�P�  � ��$ 0��. ���  � ��8  �� " ��.���  � ��$ SemiBold"���  � ��8   ��  � ��8  ��  � ��8  ��  � ��$ 4,8,4,8q���  � ��8  ��  � ��8  ��  � ��$ Stretch=���  � ��$ Center=��� " ��.���  � ��$ Center=���  � ��
 ���� ��  � ��# �  ��  � ��$ 80����  � ��$ 35����  � ��8   ��  � ��$ SemiBold"���  � ��$ 0q���  �@J�Effect    $/ 4Q�$0 5Q���  � �� �� "	 ��.R��� $ border��$
 6��$1 0.5 0.5&�$��Stretch=�$��Stretch=�#��z��._��� $��Center=�$��Center=� P�  � {� . . ��� $ border��8   � ��P�  � {� .  . ��� $ border��8   � ��P�  �@d�	IsChecked$
 TrueY�. ��� $ border��8   � ��P�  � �� . . ��� $ border�� � z� �� $	2 0.9Q�$	3 0.9Q��� " ��8,   �� " d�8,   �� " d�.���  � ��8   ��  � ��8  ��  � ��$	 160����  � ��$ 40����  � ��8  ��  � ��$ 0q���  � �� �� "	 ��.R��� $ border��._��� $��Center=�$��Center=� P�  �@J�IsMouseOver . . ��� $ border��8   � ��P�  � {� . . ��� $ border��8   � ���� 8   � ��P�  � {� .  . ��� $ border�� � ��
 ���� P�  �@d�	IsChecked$
 TrueY�. ��� $ border��8   � ���� " �.���  � ��$ 20,10,20,10q���  � ��$ 1q���  � ��
 �������  � ��
 �������  � ��
 � �����  �@�Orientation$ Vertical=���  � ��$ 30����  � ��$	 100����  � �� �� "	 �.R�� $ TemplateRoot��.+��� $��5,0,5,0q�$��0,1.2,0,1.2q�#��z�� . #��z���� $4 
PART_Track��� $ PART_Indicator��}�. $
��Left=�.+�� $ 	Indicator��5 �� $
��Auto ��� �� $
��Auto �.+��� $��0,0,-3,0q�$��1��#��z��A� $��2��#��z��#��z��$��8��#��z������ $6 Width�7 �� *TemplatedParent+A� #��z��#��z��$��10��#��z������ $6 Width�7 �� *TemplatedParent+�� $��5,0,0,0q�#��z���� $��1��#��z��$��	-5,0,-2,0q�$��0,1,0,1q�� $ 	Animation��5 �� $
��Auto ��� �� $
��Auto �.+�A� $��2��#��z��#��z��$��8��#��z������ $6 Width�7 �� *TemplatedParent+�� $��1��$1 0.5,0.5&�z��� ��!� $
8 None=�9 � #��z��.+��� $��2��$1 .5,.5&�
����������� $6 Value�7 �� *TemplatedParent+z��� $ 90Q�A� #��z��#��z��$��10��#��z������ $6 Width�7 �� *TemplatedParent+�� $��5,0,0,0q�#��z���� $��1��#��z��$��	-5,0,-2,0q�$��0,1,0,1q�A� $��Right=�#��z��#��z��$��8��#��z������ $6 Width�7 �� *TemplatedParent+ P�  �@�Orientation$ Vertical=�. ��� $ TemplateRoot�� �@�LayoutTransform �� $	 -90Q�P�  �@�IsIndeterminate . . ��� $ 	Indicator�� � y�$ 	Collapsed=�