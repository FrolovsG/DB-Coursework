   M S B A M L   `   `   `  ????C  ?BBD-Kurs, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:OOP_KursOOP_Kurs  5   	   1clr-namespace:OOP_Kurs.ModelsOOP_Kurs.Models  5          OOP_Kurs.AddTourWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:OOP_Kurs     5   	   0modelsclr-namespace:OOP_Kurs.Models     5        ? Title$  AddTourWindow??5	   	   $??355.536??6   $??656.032??60    ? 
ResizeMode$ NoResize=?6@   .??5      ? .+?5      ?? $??_Tour Type:??6
   $
??Left=?6    $??	22,10,0,0q?6;   $	??Top=?6N   ?? 5      -   5         ??Name$ SearchButton??5      $??_Search??6!   $
??Left=?63   $??
356,14,0,0q?6N   $	??Top=?6b   $??75??6z   6?   ?? 5      -   5         ??Name$ SearchTextBox??5      $
??Left=?6#   $??23??6>   $??
191,13,0,0q?6J    ?? TextWrapping$
 Wrap=?6^   $	??Top=?6r   $	??160??6?   ?? 5      $
??_Id:??6
   $
??Left=?6   $??
457,58,0,0q?64   $	??Top=?6H    J? RenderTransformOrigin$ -0.243,0.156&?6`   ?? 5      -   5        $??	IDTextBox??5      $
??Left=?6   $??23??68   $??
457,84,0,0q?6D   $
 Wrap=?6X   $??TextBox??6l   $	??Top=?6{   $	??160??6?   $ 0.727,1.204&?6?    ?? 
IsReadOnly . ???	#FFD4D1D16?   ?? 5      $??Start _Time:??6
   $
??Left=?6!   $??457,206,0,0q?6<   $	??Top=?6Q   $ -0.243,0.156&?6i   ?? 5      -   5        $??StartTimeTextBox??5      $
??Left=?6$   $??23??6?   $??457,232,0,0q?6K   $
 Wrap=?6`   $?? ??6t   $	??Top=?6|   $	??160??6?   $ 0.727,1.204&?6?   6?   ?? 5      $??_Start Date:??6
   $
??Left=?6!   $??457,115,0,0q?6<   $	??Top=?6Q   $ -0.243,0.156&?6i   (  "System.Windows.Controls.DatePicker  5      -   5        $??StartDatePicker??5      $
??Left=?6&   $??457,140,0,0q?6A   $	??Top=?6V   $	??160??6n   |?.   ? 	Resources 5      :  4System.Windows.Controls.Primitives.DatePickerTextBox ?? 
TargetType	 ?? Property
 ?? Value??%[   '        ?? 5      "  6   .?5      ?? 	 ?@??
IsReadOnly
 . 6$   5      5      5      ?? 5      -   5        $??SubmitButton??5      $??S_ubmit??6   $
??Left=?61   $??457,285,0,0q?6L   $	??Top=?6a   $??75??6y   6?   ?? 5       -   5        $??CancelButton??5       $??_Cancel??6   $
??Left=?61   $??553,285,0,0q?6L   $	??Top=?6a   $??75??6y   $ 0.944,0.622&?6?   6?   ?? 5"      -   5        $??NewTourTypeButton??5"      $??_New Tour Type??6$   $
??Left=?6=   $??
22,289,0,0q?6X   $	??Top=?6l   $	??100??6?   6?   &   System.Windows.Controls.DataGrid  5$      -	   5        $??TourTypeGridView??5$         AutoGenerateColumns .  $
??Left=?6A      SelectionMode$ Single=?6\      CanUserReorderColumns .     RowHeaderWidth$ 20Q?6?      AlternatingRowBackground ?	#FFF0F0F0   RowDetailsVisibilityMode$ VisibleWhenSelected=?5%      $	??243??64   $??	22,41,0,0q?6A   $	??Top=?6T   $	??413??6l      Columns 5'      0  *System.Windows.Controls.DataGridTextColumn  5(      ,  &System.Windows.Controls.DataGridColumn   Header$ ID??6      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6)   ?? *Id+6@     5)      $
 Name??6    .  6*   ?? *Name+6C     5*      $ Duration(Hours)??6    .  62   ?? *DurationHours+6T     5+      $ Price??6    .  6+   ?? *PriceString+6K     5,      $ Max Participants??6    .  63   ?? *MaxParticipants+6W      RowDetailsTemplate 5/      ?? 50      .R?51      ?? ???	#FFF6F6F6 ?? Padding$ 10q?6%   ._?52      ? $??
5, 0, 0, 0q?6   $??80??6!   $
??Left=?60    ? ColumnDefinitions 53   	   ?? 54   
   $	??300 ?6    ? RowDefinitions 57   	   ?? 58   
   $??24 ?6   ?? 59   
   $??76 ?6   .+?5<   	   ?? $??Sites:??6   $
??Left=?6    $??0??6;   $??0??6H   ?? 5=   	    J? IsManipulationEnabled .  $
??Left=?6/   $	??300??6J   $??1??6V   $??0??6c   ??6s   ?? *
SiteList+5>   	   5?      5@      5B      ?? 5C      $E???Note: Start Date has to be at least
one day after current date??6
   $
??Left=?6\   $??457,169,0,0q?6w   $	??Top=?6?   $	??171??6?   $??10(?6?   $??37??6?   5D      5E      