# DB-Coursework
A desktop application for managing database of tour agency. Written in C# .NET, using WPF for GUI, built using VS2022.

#### Prerequisites
- .NET Runtime 4.6.1
- Installed PostgreSQL Database

#### Features
- Retrieve and create records of tours, tour types, clients, guides and tour sites
- Retrieve records of tours planned within specific date frame
- Automatic expected revenue evaluation within specific date frame
- Retrieve records of clients/guides registered/assigned to tour
- Register/assign clients/guides to specific tour
- Prepare revenue report document in .docx format

#### How to use
On application startup, the user is prompted for password to the local PostgreSQL server. If the password is connect, a database connection to "tourdb" is established. If a database "tourdb" does not exist, it is created. If no other exceptions occur, the list of tours - main window of the application, - is shown.

Tours are created with a specific tour type in mind, which can be created from the tour form. Tour types include lists of tour sites that will be visited, in no particular order, as well as the max participant count and participant fee. Tour site form is available from the tour type form.

Once a tour has been created, clients should be registered to this tour, and guides must be assigned. Forms for creating clients/guides are accessible from their respective view tables (which are available from the main window). Created client records can be assigned from the main window, by first selecting a tour from the gridview, and using "Add/Manage Clients in Tour" button. The main window will expand to show additional GUI elements for adding/removing clients from a specific tour. The guides can be assigned from the guide view, likewise by first selecting guide from the gridview, and using "Assign/Unassign Guide" button. 

Additional readme info pending
