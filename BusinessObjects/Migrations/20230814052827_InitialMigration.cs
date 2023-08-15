using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    roleID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    roleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.roleID);
                });

            migrationBuilder.CreateTable(
                name: "UserCourse",
                columns: table => new
                {
                    userCourseID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    courseID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    userID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCourse", x => x.userCourseID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    userID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    fullName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    roleID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    isBan = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.userID);
                    table.ForeignKey(
                        name: "FK_User_Role",
                        column: x => x.roleID,
                        principalTable: "Role",
                        principalColumn: "roleID");
                });

            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    courseID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    userID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    courseCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    courseName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    timeCreated = table.Column<DateTime>(type: "datetime", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false),
                    isDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.courseID);
                    table.ForeignKey(
                        name: "FK_Course_User",
                        column: x => x.userID,
                        principalTable: "User",
                        principalColumn: "userID");
                });

            migrationBuilder.CreateTable(
                name: "Class",
                columns: table => new
                {
                    classID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    userID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    courseID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    className = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    timeStart = table.Column<DateTime>(type: "datetime", nullable: false),
                    timeEnd = table.Column<DateTime>(type: "datetime", nullable: true),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false),
                    isCompleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Class", x => x.classID);
                    table.ForeignKey(
                        name: "FK_Class_Course",
                        column: x => x.courseID,
                        principalTable: "Course",
                        principalColumn: "courseID");
                    table.ForeignKey(
                        name: "FK_Class_User",
                        column: x => x.userID,
                        principalTable: "User",
                        principalColumn: "userID");
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    projectID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    classID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    projectName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.projectID);
                    table.ForeignKey(
                        name: "FK_Project_Class",
                        column: x => x.classID,
                        principalTable: "Class",
                        principalColumn: "classID");
                });

            migrationBuilder.CreateTable(
                name: "StudentClass",
                columns: table => new
                {
                    studentClassID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    userID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    classID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentClass", x => x.studentClassID);
                    table.ForeignKey(
                        name: "FK_StudentClass_Class",
                        column: x => x.classID,
                        principalTable: "Class",
                        principalColumn: "classID");
                    table.ForeignKey(
                        name: "FK_StudentClass_User",
                        column: x => x.userID,
                        principalTable: "User",
                        principalColumn: "userID");
                });

            migrationBuilder.CreateTable(
                name: "TeamRequest",
                columns: table => new
                {
                    requestID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    userID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    classID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    team = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    status = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamRequest", x => x.requestID);
                    table.ForeignKey(
                        name: "FK_TeamRequest_Class",
                        column: x => x.classID,
                        principalTable: "Class",
                        principalColumn: "classID");
                    table.ForeignKey(
                        name: "FK_TeamRequest_User",
                        column: x => x.userID,
                        principalTable: "User",
                        principalColumn: "userID");
                });

            migrationBuilder.CreateTable(
                name: "ProjectTeam",
                columns: table => new
                {
                    projectTeamID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    projectID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    teamName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    timeStart = table.Column<DateTime>(type: "datetime", nullable: false),
                    timeEnd = table.Column<DateTime>(type: "datetime", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTeam", x => x.projectTeamID);
                    table.ForeignKey(
                        name: "FK_ProjectTeam_Project",
                        column: x => x.projectID,
                        principalTable: "Project",
                        principalColumn: "projectID");
                });

            migrationBuilder.CreateTable(
                name: "Task",
                columns: table => new
                {
                    taskID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    projectID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    taskName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    status = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Task", x => x.taskID);
                    table.ForeignKey(
                        name: "FK_Task_Project",
                        column: x => x.projectID,
                        principalTable: "Project",
                        principalColumn: "projectID");
                });

            migrationBuilder.CreateTable(
                name: "TeamMember",
                columns: table => new
                {
                    teamMemberID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    projectTeamID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    userID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMember", x => x.teamMemberID);
                    table.ForeignKey(
                        name: "FK_TeamMember_ProjectTeam",
                        column: x => x.projectTeamID,
                        principalTable: "ProjectTeam",
                        principalColumn: "projectTeamID");
                    table.ForeignKey(
                        name: "FK_TeamMember_User",
                        column: x => x.userID,
                        principalTable: "User",
                        principalColumn: "userID");
                });

            migrationBuilder.CreateTable(
                name: "StudentTask",
                columns: table => new
                {
                    studentTaskID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    taskID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    userID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentTask", x => x.studentTaskID);
                    table.ForeignKey(
                        name: "FK_StudentTask_Task",
                        column: x => x.taskID,
                        principalTable: "Task",
                        principalColumn: "taskID");
                    table.ForeignKey(
                        name: "FK_StudentTask_User",
                        column: x => x.userID,
                        principalTable: "User",
                        principalColumn: "userID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Class_courseID",
                table: "Class",
                column: "courseID");

            migrationBuilder.CreateIndex(
                name: "IX_Class_userID",
                table: "Class",
                column: "userID");

            migrationBuilder.CreateIndex(
                name: "IX_Course_userID",
                table: "Course",
                column: "userID");

            migrationBuilder.CreateIndex(
                name: "IX_Project_classID",
                table: "Project",
                column: "classID");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTeam_projectID",
                table: "ProjectTeam",
                column: "projectID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentClass_classID",
                table: "StudentClass",
                column: "classID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentClass_userID",
                table: "StudentClass",
                column: "userID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTask_taskID",
                table: "StudentTask",
                column: "taskID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTask_userID",
                table: "StudentTask",
                column: "userID");

            migrationBuilder.CreateIndex(
                name: "IX_Task_projectID",
                table: "Task",
                column: "projectID");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMember_projectTeamID",
                table: "TeamMember",
                column: "projectTeamID");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMember_userID",
                table: "TeamMember",
                column: "userID");

            migrationBuilder.CreateIndex(
                name: "IX_TeamRequest_classID",
                table: "TeamRequest",
                column: "classID");

            migrationBuilder.CreateIndex(
                name: "IX_TeamRequest_userID",
                table: "TeamRequest",
                column: "userID");

            migrationBuilder.CreateIndex(
                name: "IX_User_roleID",
                table: "User",
                column: "roleID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentClass");

            migrationBuilder.DropTable(
                name: "StudentTask");

            migrationBuilder.DropTable(
                name: "TeamMember");

            migrationBuilder.DropTable(
                name: "TeamRequest");

            migrationBuilder.DropTable(
                name: "UserCourse");

            migrationBuilder.DropTable(
                name: "Task");

            migrationBuilder.DropTable(
                name: "ProjectTeam");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "Class");

            migrationBuilder.DropTable(
                name: "Course");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
