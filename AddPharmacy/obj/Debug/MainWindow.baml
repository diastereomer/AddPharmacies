   M S B A M L   `   `   `  ����  AddPharmacy    AddPharmacy.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Add Pharmacy��5   	   $	��450��6   $	��600��6+    � WindowStartupLocation$ CenterScreen=�67   $	��450��6\   $	��600��6l   ���#fbfbfb � Icon$( "Resources/DSI Logo transparent.ico��6�   .��5      � .+�5   
   o� $��30��6   $��	0,0,100,0q�6   $	��Top=�6.   .@�5      m� -   5         m�Name$ SearchPharm��5      $��Search Pharmacy��6,   $��27��6E   $��15(�6Q   
������� J� BitmapEffect 5	      �� 5
      .��5      �� O� 5       O� 	GlowColor$ 	#FFAA8F00��60   F� 5       F� Color$ 	#FFF58E8E��61   ?� 5      5      5      m� 5      -   5        $ AddPharm��5      $��Add Pharmacy��6)   $��27��6?   $��15(�6K   
�������6l   m� 5      -   5        $ addTransfer��5      $��Add Transfer��6,   $��27��6B   $��15(�6N   
�������6o   5      �� 5   
   $��Right=�6   $��70��6,   $	��Top=�68   $	��100��6P   $��0q�6]   $��Resources/DSP_Logo.png��6h   �� 5   
   -   5         ��Name$ PharmTab��5      $��10,50,10,10q�6'   .@�5      �� -   5         ��Name$ 	SearchTab��5      $��Searching Result��6)   .��5      � ���	#FFE5E5E5.+�5      &   System.Windows.Controls.DataGrid  -   5        	  Name$	 
SearchGrid��5      
   CanUserAddRows
 .     
IsReadOnly . 5      5      5      �� 5      -   5        $ 	AddingTab��5      $��Adding Result��6)   .��5      � ���	#FFE5E5E5.+�5        -   5        $	 
AddingGrid��5   !   
 .   . 5       5!      5"      5#      5$      5%      