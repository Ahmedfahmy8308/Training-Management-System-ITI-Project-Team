using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Training_Management_System_ITI_Project.Migrations
{
    /// <inheritdoc />
    public partial class UpdateToIdentityUserReferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // First, clear existing foreign key references that won't be valid with new schema
            migrationBuilder.Sql("UPDATE Courses SET InstructorId = NULL WHERE InstructorId IS NOT NULL");
            migrationBuilder.Sql("DELETE FROM Grades"); // Clear all grades since trainee IDs will be incompatible

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_AspNetUsers_ApplicationUserId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_LegacyUsers_InstructorId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_AspNetUsers_ApplicationUserId",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_LegacyUsers_TraineeId",
                table: "Grades");

            migrationBuilder.DropTable(
                name: "LegacyUsers");

            migrationBuilder.DropIndex(
                name: "IX_Grades_ApplicationUserId",
                table: "Grades");

            migrationBuilder.DropIndex(
                name: "IX_Courses_ApplicationUserId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Grades");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Courses");

            migrationBuilder.AlterColumn<string>(
                name: "TraineeId",
                table: "Grades",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "InstructorId",
                table: "Courses",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_AspNetUsers_InstructorId",
                table: "Courses",
                column: "InstructorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_AspNetUsers_TraineeId",
                table: "Grades",
                column: "TraineeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_AspNetUsers_InstructorId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_AspNetUsers_TraineeId",
                table: "Grades");

            migrationBuilder.AlterColumn<int>(
                name: "TraineeId",
                table: "Grades",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Grades",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "InstructorId",
                table: "Courses",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Courses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LegacyUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegacyUsers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Grades_ApplicationUserId",
                table: "Grades",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_ApplicationUserId",
                table: "Courses",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_LegacyUsers_Email",
                table: "LegacyUsers",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_AspNetUsers_ApplicationUserId",
                table: "Courses",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_LegacyUsers_InstructorId",
                table: "Courses",
                column: "InstructorId",
                principalTable: "LegacyUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_AspNetUsers_ApplicationUserId",
                table: "Grades",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_LegacyUsers_TraineeId",
                table: "Grades",
                column: "TraineeId",
                principalTable: "LegacyUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
