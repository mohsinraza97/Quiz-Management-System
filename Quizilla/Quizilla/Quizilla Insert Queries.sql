INSERT INTO [User] VALUES ('Bilal01', 'Bilal Vohra', 'bilal@gmail.com', 'bilal12345', 'Teacher')
INSERT INTO [User] VALUES ('Marouf02', 'Marouf Mehmood', 'marouf@gmail.com', 'marouf12345', 'Teacher')
INSERT INTO [User] VALUES ('Saba03', 'Saba Akhtar', 'saba@gmail.com', 'saba12345', 'Teacher')
INSERT INTO [User] VALUES ('Tanveer04', 'Tanveer Zahid Khan', 'tanveer@gmail.com', 'tanveer12345', 'Teacher')
INSERT INTO [User] VALUES ('Waqas05', 'Mirza Waqas', 'mirza@gmail.com', 'waqas12345', 'Teacher')
INSERT INTO [User] VALUES ('Mohsin06', 'Syed Mohsin Raza', 'mohsinsyed1997@gmail.com', 'mohsin12345', 'Student')
INSERT INTO [User] VALUES ('Umair07', 'Umair Bin Ali', 'umiiali4433@gmail.com', 'umair12345', 'Student')
INSERT INTO [User] VALUES ('Emad08', 'Emad-ud-din', 'emaduddin.1995@yahoo.com', 'emad12345', 'Student')
INSERT INTO [User] VALUES ('Afraz09', 'Afraz Afaq', 'afrazafaq96@gmail.com', 'afraz12345', 'Student')
INSERT INTO [User] VALUES ('Sufyan10', 'Sufyan Khan', 'saifissg@gmail.com', 'sufyan12345', 'Student')

INSERT INTO Course (Code, UserId, Title, Credits) VALUES ('CSC-468', 'Tanveer04', 'Data Warehousing', 3);
INSERT INTO Course (Code, UserId, Title, Credits) VALUES ('CSC-220', 'Tanveer04', 'Database Management Systems', 3);
INSERT INTO Course (Code, UserId, Title, Credits) VALUES ('CEN-222', 'Waqas05', 'Data Communication & Networking', 3);
INSERT INTO Course (Code, UserId, Title, Credits) VALUES ('CEN-120', 'Waqas05', 'Digital Logic Design', 3);
INSERT INTO Course (Code, UserId, Title, Credits) VALUES ('CSC-113', 'Bilal01', 'Computer Programming', 2);
INSERT INTO Course (Code, UserId, Title, Credits) VALUES ('CSC-313', 'Bilal01', 'Visual Programming', 2);
INSERT INTO Course (Code, UserId, Title, Credits) VALUES ('CSC-110', 'Saba03', 'Web Engineering', 2);
INSERT INTO Course (Code, UserId, Title, Credits) VALUES ('EET-313', 'Saba03', 'Data Structures & Algorithms', 3);
INSERT INTO Course (Code, UserId, Title, Credits) VALUES ('CSC-315', 'Marouf02', 'Object Oriented Programming', 3);

INSERT INTO Quiz (QuizId, Code, UserId, Title, Duration, [Date], Instructions, PassingMarks, MaximumMarks)
VALUES ('Dbms01', 'CSC-220', 'Tanveer04', 'DBMS Quiz 01 - ER Model & Relational Model', 30, '2018-05-10', 
'Please choose the correct answer and proceed to next. You have only 30 minutes to finish this quiz. Each question carry 1 mark.', 3, 10)

INSERT INTO Question (QuizId, [Description], [Type], Option1, Option2, Option3, Option4, Answer)
VALUES ('Dbms01', 'A logical schema', 'MCQS', 'Is the entire database', 'Describe data in terms of relational tables and columns, object-oriented classes, and XML tags', 'Describes how data is actually stored on disk', 'Both (A) and (C)', 'Is the entire database')

INSERT INTO Question (QuizId, [Description], [Type], Option1, Option2, Option3, Option4, Answer)
VALUES ('Dbms01', 'Related fields in a database are grouped to form a', 'MCQS', 'Data file', 'Data record', 'Menu', 'Bank', 'Data record')

INSERT INTO Question (QuizId, [Description], [Type], Option1, Option2, Option3, Option4, Answer)
VALUES ('Dbms01', 'The database environment has all of the following components except', 'MCQS', 'Users', 'Seperate files', 'Database', 'Database administrator', 'Seperate files')

INSERT INTO Question (QuizId, [Description], [Type], Option1, Option2, Option3, Option4, Answer)
VALUES ('Dbms01', 'The way a particular application views the data from the database that the application uses is a', 'MCQS', 'Module', 'Relational model', 'Schema', 'Sub schema', 'Sub schema')