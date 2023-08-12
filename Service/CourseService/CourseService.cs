﻿using Azure.Core;
using BusinessObjects.Models;
using BusinessObjects.RequestModel;
using BusinessObjects.ResponseModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.CourseService
{
    public class CourseService : ICourseService
    {
        private readonly Swp391onGoingReportContext _context;

        public CourseService(Swp391onGoingReportContext context)
        {
            _context = context;
        }

        public async Task<int> AssignCourseToTeacher(AssignCourseRequest request)
        {
            var checkExist = await _context.UserCourses.SingleOrDefaultAsync(a => a.UserId == request.userID && a.CourseId == request.courseID);
            if (checkExist != null) return 1;

            var userCourse = new UserCourse
            {
                UserCourseId = Guid.NewGuid(),
                UserId = request.userID,
                CourseId = request.courseID
            };

            try
            {
                await _context.UserCourses.AddAsync(userCourse);
                await _context.SaveChangesAsync();
                return 2;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> UnassignCourseToTeacher(AssignCourseRequest request)
        {
            var checkExist = await _context.UserCourses.SingleOrDefaultAsync(a => a.UserId == request.userID && a.CourseId == request.courseID);
            if (checkExist == null) return 1;

            try
            {
                _context.UserCourses.Remove(checkExist);
                await _context.SaveChangesAsync();
                return 2;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> CreateCourse(CourseCreateRequest request)
        {
            var courses = await _context.Courses.SingleOrDefaultAsync(a => a.CourseName.ToLower() == request.CourseName.ToLower() && a.IsDelete == false);
            if (courses != null) return 1;

            var id = Guid.NewGuid();
            var course = new Course
            {
                CourseId = id,
                CourseCode = request.CourseCode,
                CourseName = request.CourseName,
                UserId = request.UserId,
                TimeCreated = DateTime.Now,
                IsActive = true,
                IsDelete = false
            };
            try
            {
                await _context.Courses.AddAsync(course);
                await _context.SaveChangesAsync();
                return 2;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<List<CourseResponse>> GetCourseForTeacher(Guid teacherID)
        {
            var result = new List<CourseResponse>();

            var uc = await _context.UserCourses.Where(a => a.UserId == teacherID).ToListAsync();
            if (uc == null) return null;

            foreach (var item in uc)
            {
                var course = await _context.Courses.FindAsync(item.CourseId);
                var user = await _context.Users.FindAsync(item.UserId);
                var courseTemp = new CourseResponse
                {
                    CourseId = item.CourseId,
                    CourseCode = course.CourseCode,
                    CourseName = course.CourseName,
                    UserId = course.UserId,
                    createdBy = user.FullName,
                    TimeCreated = course.TimeCreated
                };
                result.Add(courseTemp);
            }
            return result;
        }

        public async Task<int> ActiveCourse(Guid courseID)
        {
            var checkExist = await _context.Courses.FindAsync(courseID);
            if (checkExist == null) return 3;
            if (checkExist.IsActive == true) return 1;
            try
            {
                checkExist.IsActive = true;
                await _context.SaveChangesAsync();
                return 2;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> DeactiveCourse(Guid courseID)
        {
            var checkExist = await _context.Courses.FindAsync(courseID);
            if (checkExist == null) return 3;
            if (checkExist.IsActive == false) return 1;
            try
            {
                checkExist.IsActive = false;
                await _context.SaveChangesAsync();
                return 2;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> DeleteCourse(Guid courseID)
        {
            var checkExist = await _context.Courses.FindAsync(courseID);
            if (checkExist == null) return 1;
            try
            {
                checkExist.IsDelete = true;
                await _context.SaveChangesAsync();
                return 2;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<CourseResponse> GetCourseByID(Guid courseID)
        {
            var course = await _context.Courses.FindAsync(courseID);
            if (course == null) return null;
            var user = await _context.Users.FindAsync(course.UserId);
            var courseResponse = new CourseResponse
            {
                CourseId = courseID,
                CourseCode = course.CourseCode,
                CourseName = course.CourseName,
                TimeCreated = course.TimeCreated,
                UserId = course.UserId,
                createdBy = user.FullName
            };
            return courseResponse;
        }

        public async Task<List<CourseResponse>> SearchCourse(string searchText)
        {
            var result = new List<CourseResponse>();
            var courses = await _context.Courses.Where(a => a.IsDelete ==  false).ToListAsync();
            if (courses == null || courses.Count == 0) return null;

            foreach (var item in courses)
            {
                if (item.CourseName.ToLower().Contains(searchText.ToLower()))
                {
                    var creator = await _context.Users.FindAsync(item.UserId);
                    var tmp = new CourseResponse
                    {
                        CourseId = item.CourseId,
                        CourseName = item.CourseName,
                        CourseCode = item.CourseCode,
                        UserId = item.UserId,
                        createdBy = creator.FullName,
                        TimeCreated = item.TimeCreated,
                    };
                    result.Add(tmp);
                }
            }
            return result;
        }
    }
}
