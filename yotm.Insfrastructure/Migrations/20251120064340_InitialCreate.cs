using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace yotm.Insfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Quota = table.Column<int>(type: "int", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Faculty = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Instructor = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OtpCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtpCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    StudentNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoursesApplication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ApplicationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProcessedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoursesApplication", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoursesApplication_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoursesApplication_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AdminUsers",
                columns: new[] { "Id", "CreatedAt", "FullName", "PasswordHash", "UpdatedAt", "UserName" },
                values: new object[] { 1, new DateTime(2025, 11, 20, 9, 43, 39, 958, DateTimeKind.Local).AddTicks(8766), "Sistem Yöneticisi", "$2a$11$zHZ6pJM.2BmwiIKA40/b7.AYxDMOGnOIdsSXXgQ/qTrumaE9SUO76", null, "admin" });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "Code", "CreatedAt", "Department", "Faculty", "Instructor", "IsActive", "Name", "Quota", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "CEN1003", new DateTime(2025, 11, 20, 9, 43, 39, 958, DateTimeKind.Local).AddTicks(9417), "Bilgisayar Mühendisliği", "Mühendislik Fakültesi", "Prof. Dr. Ahmet Yılmaz", true, "Computer Programming I", 30, null },
                    { 2, "CEN2001", new DateTime(2025, 11, 20, 9, 43, 39, 958, DateTimeKind.Local).AddTicks(9419), "Bilgisayar Mühendisliği", "Mühendislik Fakültesi", "Doç. Dr. Ayşe Demir", true, "Data Structures", 35, null },
                    { 3, "CEN2005", new DateTime(2025, 11, 20, 9, 43, 39, 958, DateTimeKind.Local).AddTicks(9420), "Bilgisayar Mühendisliği", "Mühendislik Fakültesi", "Dr. Öğr. Üyesi Mehmet Kaya", true, "Object Oriented Programming", 30, null },
                    { 4, "CEN2002", new DateTime(2025, 11, 20, 9, 43, 39, 958, DateTimeKind.Local).AddTicks(9423), "Bilgisayar Mühendisliği", "Mühendislik Fakültesi", "Prof. Dr. Zeynep Akar", true, "Analysis of Algorithms", 25, null },
                    { 5, "CEN3001", new DateTime(2025, 11, 20, 9, 43, 39, 958, DateTimeKind.Local).AddTicks(9434), "Bilgisayar Mühendisliği", "Mühendislik Fakültesi", "Doç. Dr. Can Öztürk", true, "Advanced Computer Programming", 28, null },
                    { 6, "CEN3007", new DateTime(2025, 11, 20, 9, 43, 39, 958, DateTimeKind.Local).AddTicks(9436), "Bilgisayar Mühendisliği", "Mühendislik Fakültesi", "Dr. Öğr. Üyesi Elif Şahin", true, "Database Systems I", 32, null },
                    { 7, "CEN3002", new DateTime(2025, 11, 20, 9, 43, 39, 958, DateTimeKind.Local).AddTicks(9438), "Bilgisayar Mühendisliği", "Mühendislik Fakültesi", "Prof. Dr. Ali Yıldırım", true, "Software Engineering", 30, null },
                    { 8, "CEN3004", new DateTime(2025, 11, 20, 9, 43, 39, 958, DateTimeKind.Local).AddTicks(9440), "Bilgisayar Mühendisliği", "Mühendislik Fakültesi", "Doç. Dr. Fatma Arslan", true, "Operating Systems", 30, null },
                    { 9, "CEN3006", new DateTime(2025, 11, 20, 9, 43, 39, 958, DateTimeKind.Local).AddTicks(9442), "Bilgisayar Mühendisliği", "Mühendislik Fakültesi", "Dr. Öğr. Üyesi Burak Tekin", true, "Computer Networks", 28, null },
                    { 10, "MTH3004", new DateTime(2025, 11, 20, 9, 43, 39, 958, DateTimeKind.Local).AddTicks(9444), "Bilgisayar Mühendisliği", "Mühendislik Fakültesi", "Prof. Dr. Selin Yıldız", true, "Numerical Analysis", 25, null },
                    { 11, "SEN1001", new DateTime(2025, 11, 20, 9, 43, 39, 958, DateTimeKind.Local).AddTicks(9446), "Yazılım Mühendisliği", "Mühendislik Fakültesi", "Prof. Dr. Emre Çelik", true, "Introduction to Software Engineering", 35, null },
                    { 12, "SEN2001", new DateTime(2025, 11, 20, 9, 43, 39, 958, DateTimeKind.Local).AddTicks(9588), "Yazılım Mühendisliği", "Mühendislik Fakültesi", "Doç. Dr. Deniz Koç", true, "Object Oriented Programming I", 30, null },
                    { 13, "SEN2002", new DateTime(2025, 11, 20, 9, 43, 39, 958, DateTimeKind.Local).AddTicks(9591), "Yazılım Mühendisliği", "Mühendislik Fakültesi", "Dr. Öğr. Üyesi Ceren Aydın", true, "Database Design and Management", 32, null },
                    { 14, "SEN3001", new DateTime(2025, 11, 20, 9, 43, 39, 958, DateTimeKind.Local).AddTicks(9592), "Yazılım Mühendisliği", "Mühendislik Fakültesi", "Doç. Dr. Kerem Polat", true, "Visual Programming", 28, null },
                    { 15, "SEN3003", new DateTime(2025, 11, 20, 9, 43, 39, 958, DateTimeKind.Local).AddTicks(9594), "Yazılım Mühendisliği", "Mühendislik Fakültesi", "Prof. Dr. Seda Güneş", true, "Software Design and Analysis", 25, null },
                    { 16, "SEN3002", new DateTime(2025, 11, 20, 9, 43, 39, 958, DateTimeKind.Local).AddTicks(9596), "Yazılım Mühendisliği", "Mühendislik Fakültesi", "Dr. Öğr. Üyesi Cem Karaca", true, "Internet & Web Programming", 35, null },
                    { 17, "SEN3004", new DateTime(2025, 11, 20, 9, 43, 39, 958, DateTimeKind.Local).AddTicks(9598), "Yazılım Mühendisliği", "Mühendislik Fakültesi", "Prof. Dr. Leyla Toprak", true, "Software Architecture", 25, null },
                    { 18, "SEN4001", new DateTime(2025, 11, 20, 9, 43, 39, 958, DateTimeKind.Local).AddTicks(9599), "Yazılım Mühendisliği", "Mühendislik Fakültesi", "Doç. Dr. Mert Erdoğan", true, "Software Testing", 30, null },
                    { 19, "SEN4010", new DateTime(2025, 11, 20, 9, 43, 39, 958, DateTimeKind.Local).AddTicks(9601), "Yazılım Mühendisliği", "Mühendislik Fakültesi", "Prof. Dr. Tuğba Akman", true, "Artificial Intelligence", 28, null },
                    { 20, "SEN4002", new DateTime(2025, 11, 20, 9, 43, 39, 958, DateTimeKind.Local).AddTicks(9603), "Yazılım Mühendisliği", "Mühendislik Fakültesi", "Dr. Öğr. Üyesi Berk Yılmaz", true, "Software Verification and Validation", 22, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminUsers_UserName",
                table: "AdminUsers",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_Code",
                table: "Courses",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoursesApplication_CourseId",
                table: "CoursesApplication",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CoursesApplication_StudentId_CourseId",
                table: "CoursesApplication",
                columns: new[] { "StudentId", "CourseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OtpCodes_PhoneNumber_Code_IsUsed",
                table: "OtpCodes",
                columns: new[] { "PhoneNumber", "Code", "IsUsed" });

            migrationBuilder.CreateIndex(
                name: "IX_Students_PhoneNumber",
                table: "Students",
                column: "PhoneNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminUsers");

            migrationBuilder.DropTable(
                name: "CoursesApplication");

            migrationBuilder.DropTable(
                name: "OtpCodes");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Students");
        }
    }
}
