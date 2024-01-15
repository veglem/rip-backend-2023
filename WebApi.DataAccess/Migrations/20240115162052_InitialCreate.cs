using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebApi.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Status_pk", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnivesityUnit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ImgUrl = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    ParrentUnit = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("UnivesityUnit_pk", x => x.Id);
                    table.ForeignKey(
                        name: "UnivesityUnit_UnivesityUnit_Id_fk",
                        column: x => x.ParrentUnit,
                        principalTable: "UnivesityUnit",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Passord = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    IsModerator = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("User_pk", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UniversityEmployee",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullName = table.Column<string>(type: "character varying(70)", maxLength: 70, nullable: false),
                    Position = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Number = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    UnitId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("UniversityEmployee_pk", x => x.Id);
                    table.ForeignKey(
                        name: "UniversityEmployee_UnivesityUnit_Id_fk",
                        column: x => x.UnitId,
                        principalTable: "UnivesityUnit",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RectorOrder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    OrderBody = table.Column<string>(type: "text", nullable: false),
                    StatusId = table.Column<int>(type: "integer", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    FormationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ModeratorId = table.Column<int>(type: "integer", nullable: true),
                    CreatorId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("RectorOrder_pk", x => x.Id);
                    table.ForeignKey(
                        name: "RectorOrder_Status_Id_fk",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "RectorOrder_User_Id_fk",
                        column: x => x.ModeratorId,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "RectorOrder_User_Id_fk2",
                        column: x => x.CreatorId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Request",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UnitId = table.Column<int>(type: "integer", nullable: false),
                    OrderId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Request_pk", x => x.Id);
                    table.ForeignKey(
                        name: "Request_RectorOrder_Id_fk",
                        column: x => x.OrderId,
                        principalTable: "RectorOrder",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Request_UnivesityUnit_Id_fk",
                        column: x => x.UnitId,
                        principalTable: "UnivesityUnit",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RectorOrder_CreatorId",
                table: "RectorOrder",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_RectorOrder_ModeratorId",
                table: "RectorOrder",
                column: "ModeratorId");

            migrationBuilder.CreateIndex(
                name: "IX_RectorOrder_StatusId",
                table: "RectorOrder",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Request_OrderId",
                table: "Request",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Request_UnitId",
                table: "Request",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_UniversityEmployee_UnitId",
                table: "UniversityEmployee",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_UnivesityUnit_ParrentUnit",
                table: "UnivesityUnit",
                column: "ParrentUnit");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Request");

            migrationBuilder.DropTable(
                name: "UniversityEmployee");

            migrationBuilder.DropTable(
                name: "RectorOrder");

            migrationBuilder.DropTable(
                name: "UnivesityUnit");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
