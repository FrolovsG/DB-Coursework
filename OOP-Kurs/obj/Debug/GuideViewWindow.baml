   M S B A M L   `   `   `  ????C  ?BBD-Kurs, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:OOP_KursOOP_Kurs  5   	   1clr-namespace:OOP_Kurs.ModelsOOP_Kurs.Models  5          OOP_Kurs.GuideViewWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:OOP_Kurs     5   	   0modelsclr-namespace:OOP_Kurs.Models     5        ? Title$  GuideViewWindow??5	   	   $??480.63??6!   $??759.975??61    ? 
ResizeMode$ NoResize=?6A   .??5      ? .+?5      ?? -   5         ??Name$ SearchButton??5      $??_Search??6!   $
??Left=?63   $??
657,21,0,0q?6N   $	??Top=?6b   $??75??6z   6?   ?? 5      -   5         ??Name$ SearchTextBox??5      $
??Left=?6#   $??23??6>   $??
481,20,0,0q?6J    ?? TextWrapping$
 Wrap=?6^   $	??Top=?6r   $	??160??6?   ?? 5      -   5        $??AssignToTourButton??5      $6??0Assign/Unassign
 Selected Guide
       To Tour??6%   $??Center=?6p   $
??Left=?6?   $??
19,246,0,0q?6?   $	??Top=?6?   $	??120??6?   $??59??6  ?? 5      -   5        $ AddGuideButton??5      $??
_New Guide??6#   $
??Left=?68   $??
19,322,0,0q?6S   $	??Top=?6g   $	??120??6   6?   ?? 5      -   5        $ ReturnButton_Clients??5      $??_Return??6)   $
??Left=?6;   $??
19,393,0,0q?6V   $	??Top=?6j   $	??120??6?   6?   &   System.Windows.Controls.DataGrid  5      -   5          Name$ GuideGridView??5         SelectionMode$ Single=?6$      CanUserResizeRows .     CanUserReorderColumns .  	   RowHeaderWidth$	 20Q?6s   
   AlternatingRowBackground
 ?	#FFF0F0F0   RowDetailsVisibilityMode$ VisibleWhenSelected=?6?   $
??Left=?6?   $	??356??6?   $??
160,61,0,0q?6  $	??Top=?6  $	??575??6/  ???	#FFC9C6C6   AutoGenerateColumns .     Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ ID??6      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6%   ?? *Id+6<     5      $
 Name??6    .  6)   ?? *Name+6B     5      $ Surname??6    .  6+   ?? *	Surname+6G     5      $ ID Code??6    .  6+   ?? *IDCode+6F   4  .System.Windows.Controls.DataGridCheckBoxColumn  5      $ 	Available??6      CanUserResize .   60   ?? *IsAvailable+6P   4  .System.Windows.Controls.DataGridTemplateColumn  5      $ 
Birth Date??6      CellTemplate 5      ?? 5      .R?5   	   (  "System.Windows.Controls.DatePicker     SelectedDate 6   ?? *	BirthDate+ ? 	Resources 5   
   :  4System.Windows.Controls.Primitives.DatePickerTextBox ?? 
TargetType ?? Property ?? Value??%[   '        ?? 5       "  6   .?5!      ??  ?@??
IsReadOnly . 6)   5"      5#      5$   
   5%   	   5'        5)      $ Employment Date??6    5*      ?? 5+      .R?5,   	      6   ?? *EmploymentDate+ 5-   
   ??%[   '        ?? 5.      "  6   .?5/      ??  ?@??
IsReadOnly . 6)   50      51      52   
   53   	   55      59      5:      5;      