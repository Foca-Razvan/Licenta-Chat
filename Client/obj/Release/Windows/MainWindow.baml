   M S B A M L   `   `   `  ���� A  =Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:ClientClient      Client.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     &localclr-namespace:Client     -     � Title$  
MainWindow��$��503.102��$��289.044�� � 	Resources    myHeaderStyle �� 
TargetType �� Property �� Value��%4   &	        �� " ��.���  � y�$ 	Collapsed=�.��� $��0,0,2,-1q�
������� � RowDefinitions �� $
��307* ��� $	��25* ��� $	��88* �.+�]� $��	0,0,0,319q�.+�o� $	��Top=�$��26��
�������.@�m� $��Application��.@�m� -   $��Logout��m� -   $��Close��A� -    A�Name$ avatar_image��$��10,32,224,275q� �� Stretch$
 Fill=� �� BitmapScalingMode$
 Fant=��� -   	 ��Name$	 laber_username��$�� ��$
��Left=�$��	55,33,0,0q�$	��Top=�
 J� RenderTransformOrigin$
 0.089,0.311&�$��90��$��14(�$
��Bold"��� -    ��Name$ 	buttonAdd��$
��Left=�$��
210,49,0,0q�$	��Top=�$��26��$��27��#��[" �.���� .+��� $��/Images/add-user.png��$
��Fill=��� -    ��Name$ listViewFriendList��$
��Left=�$	��396��$��0,81,0,0q�$	��Top=�$	��279��$��3��$��18(�
�����������    ContextMenuFriendList m� CommandParameter��%2   &	       �� .@�m� -   $��Remove�� �� ���� " ��.�-	   ��  �@�ContextMenu# [  �� Triggers P�  P� Property �@��
IsSelected P� Value . . ���  � ��
 ����� �� View �� ! �� ColumnHeaderContainerStyle# [  .P���  �� CellTemplate �� .R�A� $��30��$��30��$
 Fill=�$��Center=�$��Center=�����  �� ImageSource �� *AvatarImage+��  �� .R���  �� Orientation$ 
Horizontal=�.+��� $��12��$��12��$
��Fill=����� *StatusImage+�� $��Username�� �� DisplayMemberBinding �� *
Username+��  �� Width$ 50�� �� .R��� -
   #��[" �$��30��$��30������ .���� .+��� $��/Images/conversation.png��$
��Fill=���  �� .R��� -   #��[" �$��26��$��26������ .���� .+��� $��/Images/call.png��$
��Fill=���  �� .R��� -   #��[" �$��26��$��26������ .���� .+��� $��/Images/share-screen.png��$
��Fill=��� -   $ button��$
��Left=�$��
241,49,0,0q�$	��Top=�$��28��#��[" �
���   �$��0q�$��27��.���� .+��� $��/Images/create_group.png��$
��Fill=�$��19��$��20��