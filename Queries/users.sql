use SWP391OnGoingReport
go

-- delete from [User]

-- Students
insert into [User] (userID, MSSV, email, fullName, password, isBan, roleID)
values (
    newid(),
    'SE000001',
    'student01@gmail.com',
    N'Nguyễn Thế Vinh',
    '123',
    0,
    (select roleID from [Role] where roleName = 'Student')
)
insert into [User] (userID, MSSV, email, fullName, password, isBan, roleID)
values (
    newid(),
    'SE000002',
    'student02@gmail.com',
    N'Trần Vĩnh Phát',
    '123',
    0,
    (select roleID from [Role] where roleName = 'Student')
)
insert into [User] (userID, MSSV, email, fullName, password, isBan, roleID)
values (
    newid(),
    'SE000003',
    'student03@gmail.com',
    N'Mai Cát Tường',
    '123',
    0,
    (select roleID from [Role] where roleName = 'Student')
)
insert into [User] (userID, MSSV, email, fullName, password, isBan, roleID)
values (
    newid(),
    'SE000004',
    'student04@gmail.com',
    N'Lâm Quốc Bảo',
    '123',
    0,
    (select roleID from [Role] where roleName = 'Student')
)
insert into [User] (userID, MSSV, email, fullName, password, isBan, roleID)
values (
    newid(),
    'SE000005',
    'student05@gmail.com',
    N'Nguyễn Anh Lê',
    '123',
    0,
    (select roleID from [Role] where roleName = 'Student')
)

-- Teachers
insert into [User] (userID, email, fullName, password, isBan, roleID)
values (
    newid(),
    'teacher01@gmail.com',
    N'Nguyễn Thu Hoài',
    '123',
    0,
    (select roleID from [Role] where roleName = 'Teacher')
)
insert into [User] (userID, email, fullName, password, isBan, roleID)
values (
    newid(),
    'teacher02@gmail.com',
    N'Bùi Văn Lương',
    '123',
    0,
    (select roleID from [Role] where roleName = 'Teacher')
)
insert into [User] (userID, email, fullName, password, isBan, roleID)
values (
    newid(),
    'teacher03@gmail.com',
    N'Nguyễn Thế Bảo',
    '123',
    0,
    (select roleID from [Role] where roleName = 'Teacher')
)