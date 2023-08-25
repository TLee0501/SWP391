use SWP391OnGoingReport
go

-- delete from [Course]
-- select * from [Course]

insert into [Course] (courseID, courseCode, courseName, timeCreated, isActive, isDelete, userID)
values (
    newid(),
    'SWP391',
    'Software Projects Development',
    getdate(),
    1,
    0,
    (select userID from [User] where email = 'teacher01@gmail.com')
)

insert into [Course] (courseID, courseCode, courseName, timeCreated, isActive, isDelete, userID)
values (
    newid(),
    'SWP411',
    'Capstone Projects',
    getdate(),
    1,
    0,
    (select userID from [User] where email = 'teacher01@gmail.com')
)