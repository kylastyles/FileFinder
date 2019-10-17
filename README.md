# FileFinder
LaunchCode's LC101/Liftoff capstone project.

Ten years before the Federal Mandate for Electronic Medical Records of 2014, Kyla was a file clerk at a human services organization. The organization provided day programs and residential assistance to consumers with varying degrees of disabilities. Case managers and therapists for the consumers created ample paperwork which Kyla was responsible for sorting into files. Kyla also performed quarterly audits of the files, and confidentially shredded the old files belonging to consumers who were no longer active in the system.

With thousands of file folders spread across multiple rooms in multiple buildings, Kyla wanted a way to keep better track of them all. She knew a relational database was the best way to go, but at the time she did not possess the technical skills to build one. So instead she made a very large, cumbersome spreadsheet. It did the job of cataloging and querying the files, but Kyla still dreamed of the day when she could have that database.

Now that Kyla has completed LaunchCode's LC101 program, she finally knows how to build the relational database and user interface that has haunted her for all these years. 

Please enjoy "FileFinder".

*NOTE: All data in the database is completely fictional. 

Features

    Authorized User Login: data is only accessible to file manager and admin.
    Daily Tasks:
        show inactive files due to be shredded based on current date and consumer end date
        show full or damaged files due for new file creation
    Data Queries: find files based on consumer name, case manager, program, room, or building
    Email Case Managers: inform them of paperwork discrepencies within the files.
    Audits: pull random list of files to perform quarterly audits.

Technologies

I will be creating an ASP.Net Core MVC application in C#, with persistent data stored in an Entity Framework SQLServer database, styled with Bootstrap.

What I'll Have to Learn

    Session logins with ASP.Net.
    Enhanced security and validation.
