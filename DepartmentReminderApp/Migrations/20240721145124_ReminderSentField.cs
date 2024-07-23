using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DepartmentReminderApp.Migrations
{
    /// <inheritdoc />
    public partial class ReminderSentField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Sent",
                table: "Reminders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sent",
                table: "Reminders");
        }
    }
}
