   M S B A M L   `   `   `  ???? C  ?BBD-Kurs, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:OOP_KursOOP_Kurs  1clr-namespace:OOP_Kurs.ModelsOOP_Kurs.Models      OOP_Kurs.ClientViewWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     (localclr-namespace:OOP_Kurs     0modelsclr-namespace:OOP_Kurs.Models       ? Title$  ClientViewWindow??$??480.63??$??640.975?? ? 
ResizeMode$ NoResize=?.??? .+??? -    ??Name$ SearchButton_Clients??$??_Search??$
??Left=?$??
525,20,0,0q?$	??Top=?$??75???? -    ??Name$ SearchTextBox??$
??Left=?$??23??$??
349,19,0,0q? ?? TextWrapping$
 Wrap=?$	??Top=?$	??160???? -   $ AddClientButton??$??_New Client??$
??Left=?$??
19,359,0,0q?$	??Top=?$	??120???? -   $ ReturnButton_Clients??$??_Return??$
??Left=?$??
19,393,0,0q?$	??Top=?$	??120??&   System.Windows.Controls.DataGrid  -     Name$ ClientGridView??   SelectionMode$ Single=?   CanUserResizeRows .     CanUserReorderColumns .  	   RowHeaderWidth$	 20Q?
   AlternatingRowBackground
 ?	#FFF0F0F0$
??Left=?$	??356??$??
160,61,0,0q?$	??Top=?$	??457?????	#FFC9C6C6   AutoGenerateColumns .     Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ ID??   
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding ?? *Id+  $
 Name?? .  ?? *Name+  $ Surname?? .  ?? *	Surname+  $ ID Code?? .  ?? *IDCode+  $ Status?? .  ?? *
StatusName+