   M S B A M L   `   `   `  ����  UserChatManagement#    UserChatManagement.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  User Management��5   	   $	��600��6!   $
��1000��6.    � WindowStartupLocation$ CenterScreen=�6;    � 
ResizeMode$ NoResize=�5   	    � 	Resources 5         StyledButton �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius	 �� Triggers
 P� Property P� Value  ToolBarButtonStyle  MessageButtonStyle �� Triggers��%�  &	        &	 �    &	 v    �� 5   
   " ��6%   .�5      ��  � �� �#4A90E26+   �� 5	       � ��
 �����6+   �� 5
       � ��$ 16(�6)   �� 5       � ��$ 15,10q�6(   �� 5       � ��$
 15,0q�6'   �� 5       � ��$ 1q�60   �� 5       �@�Cursor$
 Hand��6'   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� #��z��6!   $ 10��5      #��z��5      #��z��5      ._�5      �� $��Center=�6/   $��Center=�6L   5      	 5      P� 5      
 �@J�IsMouseOver$
 True��6=   . �5   "   ��  � �� �#357ABD6?   5      P� 5      
 � ��$
 True��6;   . �5   "   ��  � �� �#285A8B6?   5      5       5"      5#      �� 5$   
   " ��6+   .�5%      ��  � ��
 ���� 6+   �� 5&       � ��
 �����6+   �� 5'       � ��$ 16(�6)   �� 5(       � ��$
 10,5q�6(   �� 5)       � ��$	 5,0q�6'   �� 5*       �@�Cursor$
 Hand��6'   �� 5+       � �� 5,      �� 5-      " ��6&   .R�5.      �� #��z��6!   $ 5��5/   !   ._�50      �� $��Center=�6/   $��Center=�6L   51      	 52      P� 53      
 �@J�IsMouseOver$
 True��6=   . �54   "   ��  � �� �#357ABD6?   55      P� 56      
 � ��$
 True��6;   . �57   "   ��  � �� �#285A8B6?   58      5:      5<      5=      �� 5>   
   " ��6+   .�5?      ��  � �� �#4A90E26+   �� 5@       � ��
 �����6+   �� 5A       � ��$
 10,5q�6(   �� 5B       � ��
 �����6,   �� 5C       � ��$ 1q�60   �� 5D       �@�Cursor$
 Hand��6'   �� 5E       � ��$ 14(�6)   �� 5F       � �� 5G      �� 5H      " ��6&   .R�5I      �� #��z��6!   $ 5��5J      #��z��5K      #��z��5L      ._�5M      �� $��Center=�6/   $��Center=�5N   +   #��z��5O   +   5P      5Q      5S       5T      P� 5U      
 �@J�IsMouseOver$
 True��61   . �5V      ��  � �� �#357ABD63   5W      P� 5X      
 � ��$
 True��6/   . �5Y      ��  � �� �#28609063   5Z      5\      5]      .��5^      ]� .+�5_   
   c� $	��Top=�6   ���#4A90E2
�������.@�5`      �� -   5        #��[ 5`      $��18(�6A   $
��Home��6O   6^   �� 5a      -   5        #��[ 5a      $��18(�6A   $��Event��6O   6_   �� 5b      -   5        #��[ 5b      $��18(�6A   $��Settings��6O   6b   �� 5c      $��	50,0,50,0q�6   �� 5e      -   5         ��Name$ AdminInfoPanel��5e      $��Right=�61    �� Orientation$ 
Horizontal=�6M   .+�5f      �� -   5         ��Name$ AdminNameTextBlock��5f      $
��Bold"�68   $��18(�6J   $��Center=�6X   $��0,0,20,0q�6s   A� 5g      -   5         A�Name$ AdminAvatarImage��5g      $��50��64   $��50��6?   $��0,0,10,0q�6K   ��5h      �� 5i      -   5        AdminAvatarBrush��5i   %   5k      � 5m      -   5         �Name$ LogoutPopup��5m       � AllowsTransparency$
 True��6f   $��Bottom=�6�    � PlacementTarget 6-   ��  �� ElementName$ AdminAvatarImage��.��5n      �� 
�������
�������$��1q�6C   $ 5��6W    �� Padding$ 10q�6h   ._�5o      �� .+�5p      �� -	   5        $��Logout��5p   (   ���#4A90E2 � Cursor$
 Hand��6K   6Y   5q      5r      5s      5t      5u      � 5w   
   $��	20,5,20,5q�6   .+�5x      � -
   5         �Name$ 	HomePanel��5x       � RowDefinitions 5y      �� 5z      $
��Auto �6    �� 5{      �� 5|      $
��Auto �6    $��49.04��6.    � ColumnDefinitions 5~      �� 5      $��0* �6#   �� 5�      $
��433* �6#   �� 5�      $
��527* �6#   .+�5�      �� $��0��6   
������ $
��Auto��6>   $��2��6n   y�6L   �� *
Visibility+._�5�      �� ���#F8F6F7$��40��6.   $ 25��6:   $	��Top=�6L   $��0,15,0,0q�6d   ._�5�      � $
��Auto��6    5�      �� 5�      $��60 �6/   �� 5�      $��* �6/   .+�5�      �� $��/Assets/search.png��6    $��25��6<   $��Uniform=�6G   �� 5�      -   5         ��Name$ searchQuery��5�   2   $��1��6"   $��16(�6G   $��Stretch=�6U   $��Stretch=�6s   $��Center=�6�    �� TextAlignment$
 Left=�6�   $��0q�6�   
������ $��0,0,15,0q�6�   5�      �� 5�      $��Enter to search...��6$   $��1��6>   $��SemiBold"�6N   $��16(�6d   $
��Left=�6r    �� TextAlignment$
 Left=�6�   $��Center=�6�   ���#B7B5B8 J� IsHitTestVisible$ False��6�   ��5�      �� 5�   "   " ��6(   .�5�   &   ��  � y�$ 	Collapsed=�6C    5�   &   �� 5�   *    �� Binding 66   �� *Text+$ searchQuery�� �� Value  6h   .a�5�   .   ��  � y�$ Visible=�6K   5�   +   5�   #   5�      5�      5�      5�      �� 5�      -   5          ��Name$  UserListBox��5�      $��1��6+   $��0��68   $��	0,25,0,24q�6x   $��2��6�   ��5�      �� 5�      .R�5�      �� $ 
Horizontal=�6%   $��Stretch=�6>   .+�5�      �� 
���   �$��Center=�6U   $��10,0,0,0q�6p   $
��Bold"�6�   ��6(   �� *
UserName+6�   �� 5�      -   5        $��	Chat now!��5�   %   $��10,0,0,0q�5�      #��[ 5�      $	��100��5�      ��5�      �� *
UserName+5�      5�      5�       5�      ! ��Name" �� 
TargetName��%�  '��       �� 5�      " ��6   .�5�      ��  � ��$ 5q�63   �� 5�       � ��$ 10q�64   �� 5�       � ��$ 40��66   �� 5�       � ��
 ���� 67   �� 5�       � �� 5�      �� 5�   "   " ��62   .R�5�   &   �� $! Border��6-   #��z��6=   
�������$��1q�6z   $ 5��6�   ._�5�   *   �� $��10q�6;   5�   '   	 5�   &   P� 5�   *   
 �@J�IsMouseOver$
 True��6I   . �5�   .   �� $" Border��65    � �� �#E0E0E06_   5�   +   P� 5�   *   
 �@��
IsSelected$
 True��6H   . �5�   .   �� $" Border��65    � �� �#4A90E26_   �� 5�   .   $" Border��65    � ��
 �����6`   �� 5�   .    � ��
 �����6K   5�   +   5�   #   5�      5�      5�      5�      �� 5�      -   5        $ UserDetailsPanel��5�      $��1��63   $��2��6@   $��Center=�6P   $	��392��6m   $��	0,25,0,10q�6y   .+�5�      �� -   5        # ��Name$# AvatarImage��5�      $��/Assets/LogoWhiteBack.png��6-   $	��100��6P   $	��100��6]   �� 5�      -   5        $ FullNameTextBlock��5�      $��0,10,0,0q�67   $ Center=�6I   $
��Bold"�6`   $��20(�6r   �� 5�      -   5        $ UserNameTextBlock��5�      $��0,6,0,0q�67   $ Center=�6H   $��17(�6_   ���#B7B5B86m   �� 5�      -   5        $ RolesTextBlock��5�      $��0,6,0,0q�64   $ Center=�6E   $��17(�6\   ���#B7B5B86j   A� 5�      $ A� Columns$$ 3��6   $��Center=�6*   $	��150��6G   $��0,15,0,0q�6S   .+�5�      A� $��40��6   $��40��6*   ��5�      �� 5�      % �� ImageSource$% /Assets/facebook.png��6)   & j� Stretch$& Uniform=�6L   5�      A� 5�      $��40��6   $��40��6*   ��5�      �� 5�      $% /Assets/twitter.png��6)   $& Uniform=�6K   5�      A� 5�      $��34��6   $��34��6*   ��5�      �� 5�      $% /Assets/insta.png��6)   5�      5�      �� 5�      $	��240��6   ���#B7B5B8$|�False��6=   $��0,15,0,0q�6O   �� 5�      $��	30,10,0,0q�6   .+�5�      �� �� 6!   $�Phone: ��6%   ���#B7B5B8$��14(�6I   �� 6Y   -   5        ' ��Name$' PhoneNumberTextBlock��5�   ]   $��14(�6{   6�   �� 5�      $ 
Horizontal=�6!   .+�5�      �� �� 6%   $�E-mail: ��6)   ���#B7B5B8$��14(�6N   �� 6^   -   5        $' EmailTextBlock��5�   b   $��14(�6z   6�   �� 5�      -   5        $# EmailConfirmedIcon��5�       $��16��6<   $��16��6G   $��5,0,0,0q�6S   5�      5�      5�      �� 5�      -   5        $ 	ChatPanel��5�      $��1��6,   $��2��69   $��Center=�6I   $	��400��6f   $��	0,25,0,10q�6r   $y�	Collapsed=�6�   .+�5�      �� $��Chat with User��6   $
��Bold"�62   $��20(�6D   $��0,0,0,10q�6R   �� 5�      $	��250��6   $��0,0,0,10q�6,   .��5�      �� -   5        $  MessageListBox��5�      $��Stretch=�66   5�      �� 5�      $��0��6   
������ $
��Auto��6B   $��2��6r   y�6P   �� *
Visibility+._�5�      �� ���#F8F6F7$��40��62   $ 25��6>   $	��Top=�6P   $��0,15,0,0q�6h   ._�5�      � $
��Auto��6    5�      �� 5�   "   $	��400 �63   �� 5   "   $��0* �63   .+�5     �� -   5        $ ChatInputBox��5  &   $��16(�6<   $��Center=�6J   $
 Left=�6l   $��0q�6�   
������ $��
20,0,110,0q�6�   ( �� AcceptsReturn$
( True��5     ") �� VerticalScrollBarVisibility$
) Auto=�6+   �� 5     -   5        $
��Send��5  %   $��76��6M   #��[  6X   $��Center=�6~   $��0,0,20,0q�6�   $��Right=�6�   �� 5     $��Type message here...��6(   $��16(�6D   ���#B7B5B8$ False��6g   $��	20,0,10,0q�6�   $��Center=�6�   ��5  "   �� 5  &   " ��6,   .�5	  *   ��  � y�$ 	Collapsed=�6G    5
  *   �� 5  .    6:   �� *Text+$ ChatInputBox��  6m   .a�5  2   ��  � y�$ Visible=�6O   5  /   5  '   5     5     5     5     5     �� 5     $ 
Horizontal=�6   $��Center=�62   $��	0,10,0,10q�6O   $��2��6b   $��1��6o   .+�5     �� -   5        #��[  5     $	��Add��6?   $��75��6M   6X   �� 5     -   5        #��[  5     $
��Edit��6?   $��75��6N   6Y   5     5     �� 5      -   5        $ 
EventPanel��5      $y�	Collapsed=�6-   $��	20,5,20,5q�6D   .+�5!     �� $��Create Event��6   $
��Bold"�60   $��20(�6B   $��0,0,0,10q�6P   �� 5$     -   5        $ EventSubjectTextBox��5$     $��16(�67   $��Stretch=�6E   $��Stretch=�6c   $��Center=�6   $
 Left=�6�   $��0q�6�   
������ $��0,0,15,0q�6�   �� 5%     $��Enter subject...��6   $��SemiBold"�64   $��16(�6J   $
��Left=�6X   $��Center=�6s   ���#B7B5B8$ False��6�   ��5&     �� 5'     " ��6    .�5(     ��  � y�$ 	Collapsed=�6;    5)     �� 5*  "    6.   �� *Text+$ EventSubjectTextBox��  6h   .a�5+  &   ��  � y�$ Visible=�6C   5,  #   5.     50     �� 53     -   5        $ EventMessageTextBox��53     $��16(�67   $��Stretch=�6E   $��Stretch=�6c   $��Center=�6   $
 Left=�6�   $��0q�6�   
������ $��0,0,15,0q�6�   �� 54     $��Enter message...��6   $��SemiBold"�64   $��16(�6J   $
��Left=�6X   $��Center=�6s   ���#B7B5B8$ False��6�   ��55     �� 56     " ��6    .�57     ��  � y�$ 	Collapsed=�6;    58     �� 59  "    6.   �� *Text+$ EventMessageTextBox��  6h   .a�5:  &   ��  � y�$ Visible=�6C   5;  #   5=     5?     �� 5B     -   5        $ EventDelayTimeTextBox��5B     $��16(�69   $��Stretch=�6G   $��Stretch=�6e   $��Center=�6�   $
 Left=�6�   $��0q�6�   
������ $��0,0,15,0q�6�   �� 5C     $��Enter delay time...��6   $��SemiBold"�67   $��16(�6M   $
��Left=�6[   $��Center=�6v   ���#B7B5B8$ False��6�   ��5D     �� 5E     " ��6    .�5F     ��  � y�$ 	Collapsed=�6;    5G     �� 5H  "    6.   �� *Text+$ EventDelayTimeTextBox��  6j   .a�5I  &   ��  � y�$ Visible=�6C   5J  #   5L     5N     5O     5Q     5T     5U     