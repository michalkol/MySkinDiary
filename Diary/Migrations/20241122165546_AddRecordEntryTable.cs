using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Diary.Migrations
{
    /// <inheritdoc />
    public partial class AddRecordEntryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.CreateTable(
                name: "RecordEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhysicalState = table.Column<int>(type: "int", nullable: false),
                    PhysicalDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MentalState = table.Column<int>(type: "int", nullable: false),
                    MentalDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SkinState1 = table.Column<int>(type: "int", nullable: false),
                    SkinState2 = table.Column<int>(type: "int", nullable: false),
                    SkinState3 = table.Column<int>(type: "int", nullable: false),
                    SkinState4 = table.Column<int>(type: "int", nullable: false),
                    SkinState5 = table.Column<int>(type: "int", nullable: false),
                    SkinStateDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsSportActivity = table.Column<bool>(type: "bit", nullable: false),
                    SportActivityDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsSexActivity = table.Column<bool>(type: "bit", nullable: false),
                    IsAlcohol = table.Column<bool>(type: "bit", nullable: false),
                    Digesting = table.Column<int>(type: "int", nullable: false),
                    Menstruation = table.Column<int>(type: "int", nullable: false),
                    DietDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MedicationDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Photo = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecordEntries", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecordEntries");

           
        }
    }
}
