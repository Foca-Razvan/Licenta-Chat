   M S B A M L   `   `   `  ����A  =Client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:Client.WindowsClient.Windows  5   	   0    *Client.Windows.AddFriendConversationWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:Client.Windows     5   	     � Title$!  AddFriendConversationWindow��5   	   $	��300��6-   $	��300��6:    � 	Resources 5
         myHeaderStyle �� 
TargetType �� Property �� Value��%Y   &	        �� 5   
   " ��6&   .�5      ��  � y�$ 	Collapsed=�6+   5      5      .��5      � .+�5   
   �� -   5         ��Name$ listViewFriendList��5      $
��Left=�6/   $	��218��6J   $	��Top=�6W   $	��292��6o   ��6{   �� *myHeaderStyle+ �� View 5      �� 5      ! �� ColumnHeaderContainerStyle# [  6   .P�5      ��  �� CellTemplate 5      �� 5      .R�5   "   A� $��30��6*   $��30��65   	 �� Stretch$
	 Fill=�6A   $��Center=�6P   $��Center=�6m   ��5   &   �� 5   *   
 �� ImageSource
 65   �� *AvatarImage+5   #   5      5      �� 5      $��Username��6%    �� DisplayMemberBinding 67   �� *
Username+6c   5      5!      �� 5"   
   -   5         ��Name$ 	buttonAdd��5"      $	��Add��6$   $
��Left=�62   $��
40,232,0,0q�6M   $	��Top=�6a   $��75��6y   #��[" �6�   $��1q�6�   
���   �6�   �� 5#   
   -   5        $ buttonCancel��5#      $��Cancel��6'   $
��Left=�68   $��176,232,0,0q�6S   $	��Top=�6h   $��75��6�   #��[" �6�   $��1q�6�   
���   �6�   5%      5&      