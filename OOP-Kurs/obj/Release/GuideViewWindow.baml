   M S B A M L   `   `   `  ???? C  ?BBD-Kurs, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:OOP_KursOOP_Kurs  1clr-namespace:OOP_Kurs.ModelsOOP_Kurs.Models      OOP_Kurs.GuideViewWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     (localclr-namespace:OOP_Kurs     0modelsclr-namespace:OOP_Kurs.Models       ? Title$  GuideViewWindow??$??480.63??$??759.975?? ? 
ResizeMode$ NoResize=?.??? .+??? -    ??Name$ SearchButton??$??_Search??$
??Left=?$??
657,21,0,0q?$	??Top=?$??75???? -    ??Name$ SearchTextBox??$
??Left=?$??23??$??
481,20,0,0q? ?? TextWrapping$
 Wrap=?$	??Top=?$	??160???? -   $??AssignToTourButton??$6??0Assign/Unassign
 Selected Guide
       To Tour??$??Center=?$
??Left=?$??
19,246,0,0q?$	??Top=?$	??120??$??59???? -   $ AddGuideButton??$??
_New Guide??$
??Left=?$??
19,322,0,0q?$	??Top=?$	??120???? -   $ ReturnButton_Clients??$??_Return??$
??Left=?$??
19,393,0,0q?$	??Top=?$	??120??&   System.Windows.Controls.DataGrid  -     Name$ GuideGridView??   SelectionMode$ Single=?   CanUserResizeRows .     CanUserReorderColumns .  	   RowHeaderWidth$	 20Q?
   AlternatingRowBackground
 ?	#FFF0F0F0   RowDetailsVisibilityMode$ VisibleWhenSelected=?$
??Left=?$	??356??$??
160,61,0,0q?$	??Top=?$	??575?????	#FFC9C6C6   AutoGenerateColumns .     Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ ID??   
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding ?? *Id+  $
 Name?? .  ?? *Name+  $ Surname?? .  ?? *	Surname+  $ ID Code?? .  ?? *IDCode+4  .System.Windows.Controls.DataGridCheckBoxColumn  $ 	Available??   CanUserResize .   ?? *IsAvailable+4  .System.Windows.Controls.DataGridTemplateColumn  $ 
Birth Date??   CellTemplate ?? .R?(  "System.Windows.Controls.DatePicker     SelectedDate ?? *	BirthDate+ ? 	Resources :  4System.Windows.Controls.Primitives.DatePickerTextBox ?? 
TargetType ?? Property ?? Value??%6   '        ?? "  .???  ?@??
IsReadOnly .   $ Employment Date?? ?? .R?   ?? *EmploymentDate+ ??%6   '        ?? "  .???  ?@??
IsReadOnly . 