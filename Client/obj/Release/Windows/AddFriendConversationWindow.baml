   M S B A M L   `   `   `  ���� A  =Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:Client.WindowsClient.Windows  0    *Client.Windows.AddFriendConversationWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     .localclr-namespace:Client.Windows       � Title$!  AddFriendConversationWindow��$	��300��$	��300�� � 	Resources    myHeaderStyle �� 
TargetType �� Property �� Value��%4   &	        �� " ��.���  � y�$ 	Collapsed=�.��� .+��� -    ��Name$ listViewFriendList��$
��Left=�$	��218��$	��Top=�$	��292������ *myHeaderStyle+ �� View �� ! �� ColumnHeaderContainerStyle# [  .P���  �� CellTemplate �� .R�A� $��30��$��30��	 �� Stretch$
	 Fill=�$��Center=�$��Center=����� 
 �� ImageSource
 �� *AvatarImage+�� $��Username�� �� DisplayMemberBinding �� *
Username+�� -    ��Name$ 	buttonAdd��$	��Add��$
��Left=�$��
40,232,0,0q�$	��Top=�$��75��#��[" �$��1q�
���   ��� -   $ buttonCancel��$��Cancel��$
��Left=�$��176,232,0,0q�$	��Top=�$��75��#��[" �$��1q�
���   �