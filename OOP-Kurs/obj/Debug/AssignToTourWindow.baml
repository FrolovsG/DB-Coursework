   M S B A M L   `   `   `  ????C  ?BBD-Kurs, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:OOP_KursOOP_Kurs  5   	   1clr-namespace:OOP_Kurs.ModelsOOP_Kurs.Models  5      !    OOP_Kurs.AssignToTourWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:OOP_Kurs     5   	   0modelsclr-namespace:OOP_Kurs.Models     5        ? Title$  AssignToTourWindow??5	   	   $??340.782??6$   $??770.891??65    ? 
ResizeMode$ NoResize=?6E   .??5
      ? .+?5      ?? $??Assigned to tours:??6
   $
??Left=?6'   $??	22,20,0,0q?6B   $	??Top=?6U    J? RenderTransformOrigin$ 0,0.115&?6m   ?? 5      -   5        $??TourListBox_Assigned??5   %    ?? SelectionMode$ Extended=?6   $
??Left=?6Z   $	??220??6u   $??	22,46,0,0q?6?   $	??Top=?6?   $	??210??6?   ?? 5      -   5        $??UnassignButton??5      $??	_Unassign??6!   $
??Left=?65   $??
34,275,0,0q?6P   $	??Top=?6d   $??75??6|   6?   ?? 5      ???	#FFF4F4F5$
??Left=?6   $	??271??6:   $??
256,20,0,0q?6G   
???   ?$	??Top=?6j   $??1??6?   ?? 5      $#??Tours with no guide assigned:??6
   $
??Left=?62   $??
283,20,0,0q?6M   $	??Top=?6a   $ 0,0.115&?6y   ?? 5      -   5         ??Name$ TourListBox_Unassigned??5   %   $ Extended=?6   $
??Left=?6^   $	??220??6y   $??
283,46,0,0q?6?   $	??Top=?6?   $	??210??6?   ?? 5      $??Rest of the tours:??6
   $
??Left=?6'   $??
526,20,0,0q?6B   $	??Top=?6V   $ 0,0.115&?6n   ?? 5      -   5        $ TourListBox_Other??5   %   $ Extended=?6   $
??Left=?6Y   $	??220??6t   $??
526,46,0,0q?6?   $	??Top=?6?   $	??210??6?   ?? 5      -   5        $??AssignButton??5      $??_Assign??6   $
??Left=?61   $??553,275,0,0q?6L   $	??Top=?6a   $??75??6y   6?   ?? 5      -   5        $??ReturnButton??5      $??_Return??6   $
??Left=?61   $??650,275,0,0q?6L   $	??Top=?6a   $??75??6y   6?   5      5      