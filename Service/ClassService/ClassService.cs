using BusinessObjects.Models;
using BusinessObjects.RequestModel;
using BusinessObjects.ResponseModel;
using Microsoft.EntityFrameworkCore;


namespace Service.ClassService
{
    public class ClassService : IClassService
    {
        private readonly Swp391onGoingReportContext _context;
        public ClassService(Swp391onGoingReportContext context)
        {
            _context = context;
        }

        public async Task<int> CreateClass(CreateClassRequest request)
        {
            try
            {
                var existingClass = await _context.Classes.SingleOrDefaultAsync(x => x.ClassName.ToLower() == request.ClassName.ToLower() && x.IsDeleted == false);
                if (existingClass != null) return 1;

                var newClass = new Class
                {
                    ClassId = Guid.NewGuid(),
                    UserId = request.TeacherId,
                    CourseId = request.CourseId,
                    ClassName = request.ClassName,
                    EnrollCode = request.EnrollCode,
                    IsDeleted = false,
                    SemesterId = request.SemesterId,
                };

                await _context.Classes.AddAsync(newClass);
                await _context.SaveChangesAsync();
                return 2;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> UpdateClass(UpdateClassRequest request)
        {
            try
            {
                var existingClass = await _context.Classes.FindAsync(request.ClassId);
                if (existingClass == null) return 1;

                existingClass.ClassName = request.ClassName;
                existingClass.EnrollCode = request.EnrollCode;
                existingClass.UserId = request.TeacherId;

                await _context.SaveChangesAsync();
                return 2;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> DeleteClass(Guid classId)
        {
            var check = await _context.Classes.FindAsync(classId);
            if (check == null) return 1;
            try
            {
                check.IsDeleted = true;
                await _context.SaveChangesAsync();
                return 2;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<bool> EnrollClass(Guid userId, Guid classId, string enrollCode)
        {
            var matchClass = await _context.Classes.Where(_ => _.EnrollCode == enrollCode && _.ClassId == classId).SingleOrDefaultAsync();
            if (matchClass == null)
            {
                return false;
            }

            var studentClass = new StudentClass
            {
                StudentClassId = Guid.NewGuid(),
                ClassId = classId,
                UserId = userId,
            };

            await _context.StudentClasses.AddAsync(studentClass);
            var result = await _context.SaveChangesAsync();
            return result == 1;
        }

        public async Task<ClassDetailResponse?> GetClassByID(Guid classId, Guid? userId)
        {
            var query = _context.Classes
                .Include(x => x.User)
                .Include(x => x.Course)
                .Include(x => x.Projects)
                .Include(x => x.Semester)
                .Where(x => x.ClassId == classId);
            var result = await query.SingleOrDefaultAsync();
            if (result == null) return null;

            var enrolledClasses = await _context.StudentClasses.Where(_ => _.UserId == userId).ToListAsync();

            var studentClasses = await _context.StudentClasses
                .Include(x => x.User)
                .Where(x => x.ClassId == classId)
                .ToListAsync();

            var projectTeamQuery = _context.ProjectTeams
                .Include(x => x.Project)
                .Include(x => x.TeamMembers)
                    .ThenInclude(x => x.User)
                .Where(x => x.Project.ClassId == classId);

            var projectTeams = await projectTeamQuery.ToListAsync();
            var teamLeaders = new List<ClassDetailResponseStudent>();
            foreach (var team in projectTeams)
            {
                var leader = await _context.Users.FindAsync(team.LeaderId);
                if (leader != null)
                {
                    teamLeaders.Add(new ClassDetailResponseStudent
                    {
                        Id = leader.UserId,
                        FullName = leader.FullName,
                        Code = leader.Mssv!,
                        Email = leader.Email,
                    });
                }
            }

            return new ClassDetailResponse
            {
                ClassId = classId,
                ClassName = result.ClassName,
                TeacherId = result.UserId,
                EnrollCode = result.EnrollCode,
                CourseCode = result.Course.CourseCode,
                CourseName = result.Course.CourseName,
                TeacherName = result.User?.FullName,
                Enrolled = enrolledClasses!.Find(_ => _.ClassId == classId) != null,
                Projects = result.Projects.Select(x => new ClassDetailResponseProject
                {
                    Id = x.ProjectId,
                    Name = x.ProjectName,
                    Description = x.Description,
                    FunctionalReq = x.FunctionalReq,
                    NonfunctionalReq = x.NonfunctionalReq
                }).ToList(),
                Students = studentClasses.Select(x => new ClassDetailResponseStudent
                {
                    Id = x.User.UserId,
                    FullName = x.User.FullName,
                    Email = x.User.Email,
                    Code = x.User.Mssv!
                }).ToList(),
                Teams = projectTeams.Select(x => new ClassDetailResponseTeam
                {
                    Id = x.ProjectTeamId,
                    Project = new ClassDetailResponseProject
                    {
                        Id = x.Project.ProjectId,
                        Name = x.Project.ProjectName,
                        Description = x.Project.Description,
                        FunctionalReq = x.Project.FunctionalReq,
                        NonfunctionalReq = x.Project.NonfunctionalReq,
                    },
                    Members = x.TeamMembers.Select(x => new ClassDetailResponseStudent
                    {
                        Id = x.User.UserId,
                        FullName = x.User.FullName,
                        Code = x.User.Mssv!,
                        Email = x.User.Email
                    }).ToList(),
                    Leader = teamLeaders.Single(item => item.Id == x.LeaderId)
                }).ToList(),
                Semester = new ClassDetailResponseSemester
                {
                    Id = result.Semester.SemesterId,
                    Name = result.Semester.SemeterName,
                    StartTime = result.Semester.StartTime,
                    EndTime = result.Semester.EndTime,
                },
            }; ;
        }

        public async Task<List<ClassListResponse>> GetClasses(Guid userId, Guid? semesterId, Guid? courseId, string? searchText)
        {
            var query = _context.Classes
                .Include(x => x.Course)
                .Include(x => x.User)
                .Include(x => x.Semester)
                .Where(x => !x.IsDeleted);

            var enrolledClasses = await _context.StudentClasses.Where(_ => _.UserId == userId).ToListAsync();

            if (courseId != null)
            {
                query = query.Where(item => item.CourseId == courseId);
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(item => item.ClassName.Contains(searchText));
            }

            var classes = await query.ToListAsync();
            var classResponses = classes.Select(item => new ClassListResponse
            {
                ClassId = item.ClassId,
                ClassName = item.ClassName,
                CourseCode = item.Course.CourseCode,
                CourseName = item.Course.CourseName,
                EnrollCode = item.EnrollCode,
                TeacherId = item.UserId,
                TeacherName = item.User?.FullName,
                Enrolled = enrolledClasses!.Find(_ => _.ClassId == item.ClassId) != null,
                SemesterId = item.Semester.SemesterId,
                SemesterName = item.Semester.SemeterName,
            });

            return classResponses.ToList();
        }

        public async Task<List<UserListResponse>> GetUsersInClass(Guid classId)
        {
            var studenClasses = await _context.StudentClasses.Where(_ => _.ClassId == classId).Include(_ => _.User).ToListAsync();
            var result = studenClasses.Select(item => new UserListResponse
            {
                UserId = item.UserId,
                FullName = item.User.FullName,
                Email = item.User.Email,
                Mssv = item.User.Mssv,
            }).ToList();

            return result;
        }

        public async Task<List<UserListResponse>> GetStudentsNotInProjectInClass(Guid classId)
        {
            //var checkRequest = await _context.TeamRequests.Where(x => x.ClassId == classId).ToListAsync();
            var studentsRequest = new List<Guid>();
            /*foreach (var item in checkRequest)
            {
                studentsRequest.Add(item.UserId);
            }*/

            var checkClass = await _context.StudentClasses.Where(x => x.ClassId == classId).ToListAsync();
            var studentsClass = new List<Guid>();
            var list = new List<UserListResponse>();
            foreach (var item in checkClass)
            {
                studentsClass.Add(item.UserId);
            }

            foreach (var item in studentsClass)
            {
                foreach (var item2 in studentsRequest)
                {
                    if (item == item2)
                    {
                        studentsClass.Remove(item);
                    }
                }
            }
            foreach (var item in studentsClass)
            {
                var user = await _context.Users.FindAsync(item);
                var result = new UserListResponse
                {
                    UserId = user.UserId,
                    FullName = user.FullName,
                    Email = user.Email,
                };
                list.Add(result);
            }
            return list;
        }

        public async Task<int> AssignClass(AssignClassRequest request)
        {
            var check = await _context.UserClasses.SingleOrDefaultAsync(x => x.UserId == request.UserID && x.ClassId == request.ClassID);
            if (check != null) return 1;
            else if (check == null) return 5;
            else if (check.ClassId.Equals(0)) return 3;
            else if (check.UserId.Equals(0)) return 4;
            var id = Guid.NewGuid();
            var userClass = new UserClass
            {
                UserClassId = id,
                ClassId = request.ClassID,
                UserId = request.UserID,
            };
            try
            {
                await _context.UserClasses.AddAsync(userClass);
                await _context.SaveChangesAsync();
                return 2;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> UnassignClass(AssignClassRequest request)
        {
            var check = await _context.UserClasses.SingleOrDefaultAsync(x => x.UserId == request.UserID && x.ClassId == request.ClassID);
            if (check == null) return 1;
            try
            {
                _context.UserClasses.Remove(check);
                _context.SaveChangesAsync();
                return 2;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public async Task<List<ClassListResponse>> GetClassForTeacher(Guid teacherId)
        {
            var checkClass = await _context.Classes.Where(x => x.UserId == teacherId).ToListAsync();
            var list = new List<ClassListResponse>();
            if (checkClass == null)
            {
                return null;
            }
            else
            {
                foreach (var c in checkClass)
                {
                    var courses = await _context.Courses.FindAsync(c.CourseId);
                    var users = await _context.Users.FindAsync(c.UserId);
                    var semesters = await _context.Semesters.FindAsync(c.SemesterId);
                    var teacherClass = new ClassListResponse
                    {
                        ClassId = c.ClassId,
                        ClassName = c.ClassName,
                        CourseCode = courses.CourseCode,
                        CourseName = courses.CourseName,
                        EnrollCode = c.EnrollCode,
                        SemesterId = c.SemesterId,
                        TeacherId = teacherId,
                        TeacherName = users.FullName,
                        SemesterName = semesters.SemeterName,
                    };
                    list.Add(teacherClass);
                }
            }
            return list;
        }
    }
}
