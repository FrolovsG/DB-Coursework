   M S B A M L   `   `   `  ????C  ?BBD-Kurs, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:OOP_KursOOP_Kurs  5   	   1clr-namespace:OOP_Kurs.ModelsOOP_Kurs.Models  5          OOP_Kurs.ClientViewWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:OOP_Kurs     5   	   0modelsclr-namespace:OOP_Kurs.Models     5        ? Title$  ClientViewWindow??5	   	   $??480.63??6"   $??640.975??62    ? 
ResizeMode$ NoResize=?6B   .??5      ? .+?5      ?? -   5         ??Name$ SearchButton_Clients??5      $??_Search??6)   $
??Left=?6;   $??
525,20,0,0q?6V   $	??Top=?6j   $??75??6?   6?   ?? 5      -   5         ??Name$ SearchTextBox??5      $
??Left=?6#   $??23??6>   $??
349,19,0,0q?6J    ?? TextWrapping$
 Wrap=?6^   $	??Top=?6r   $	??160??6?   ?? 5      -   5        $ AddClientButton??5      $??_New Client??6$   $
??Left=?6:   $??
19,359,0,0q?6U   $	??Top=?6i   $	??120??6?   6?   ?? 5      -   5        $ ReturnButton_Clients??5      $??_Return??6)   $
??Left=?6;   $??
19,393,0,0q?6V   $	??Top=?6j   $	??120??6?   6?   &   System.Windows.Controls.DataGrid  5      -   5          Name$ ClientGridView??5         SelectionMode$ Single=?6%      CanUserResizeRows .     CanUserReorderColumns .  	   RowHeaderWidth$	 20Q?6t   
   AlternatingRowBackground
 ?	#FFF0F0F0$
??Left=?6?   $	??356??6?   $??
160,61,0,0q?6?   $	??Top=?6?   $	??457??6  ???	#FFC9C6C6   AutoGenerateColumns .     Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ ID??6      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6(   ?? *Id+6?     5      $
 Name??6    .  6)   ?? *Name+6B     5      $ Surname??6    .  6+   ?? *	Surname+6G     5      $ ID Code??6    .  6+   ?? *IDCode+6F     5      $ Status??6    .  6+   ?? *
StatusName+6J   5      5      5      