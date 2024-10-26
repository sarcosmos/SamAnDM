using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SamAnDMBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    DocumentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsProtected = table.Column<bool>(type: "bit", nullable: false),
                    IsShared = table.Column<bool>(type: "bit", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TypeDocument = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocumentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocumentSize = table.Column<long>(type: "bigint", nullable: false),
                    DocumentContent = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.DocumentId);
                });

            migrationBuilder.CreateTable(
                name: "Historics",
                columns: table => new
                {
                    HistoricId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserModification = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateModification = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReferenceId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Historics", x => x.HistoricId);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    PermissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermissionName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.PermissionId);
                });

            migrationBuilder.CreateTable(
                name: "UserTypes",
                columns: table => new
                {
                    UserTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTypes", x => x.UserTypeId);
                });

            migrationBuilder.CreateTable(
                name: "DocumentsHistorics",
                columns: table => new
                {
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    HistoricId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentsHistorics", x => new { x.DocumentId, x.HistoricId });
                    table.ForeignKey(
                        name: "FK_DocumentsHistorics_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "DocumentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentsHistorics_Historics_HistoricId",
                        column: x => x.HistoricId,
                        principalTable: "Historics",
                        principalColumn: "HistoricId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_UserTypes_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "UserTypes",
                        principalColumn: "UserTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTypePermissions",
                columns: table => new
                {
                    UserTypeId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTypePermissions", x => new { x.UserTypeId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_UserTypePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "PermissionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTypePermissions_UserTypes_UserTypeId",
                        column: x => x.UserTypeId,
                        principalTable: "UserTypes",
                        principalColumn: "UserTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FamilyGroups",
                columns: table => new
                {
                    FamilyGroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Members = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UsersUserId = table.Column<int>(type: "int", nullable: false),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    DocumentsDocumentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyGroups", x => x.FamilyGroupId);
                    table.ForeignKey(
                        name: "FK_FamilyGroups_Documents_DocumentsDocumentId",
                        column: x => x.DocumentsDocumentId,
                        principalTable: "Documents",
                        principalColumn: "DocumentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FamilyGroups_Users_UsersUserId",
                        column: x => x.UsersUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    PersonId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<int>(type: "int", nullable: false),
                    DateRegistration = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProfilePicture = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountStatus = table.Column<bool>(type: "bit", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TokenRecovery = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UsersUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.PersonId);
                    table.ForeignKey(
                        name: "FK_Persons_Users_UsersUserId",
                        column: x => x.UsersUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentsHistorics_HistoricId",
                table: "DocumentsHistorics",
                column: "HistoricId");

            migrationBuilder.CreateIndex(
                name: "IX_FamilyGroups_DocumentsDocumentId",
                table: "FamilyGroups",
                column: "DocumentsDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_FamilyGroups_UsersUserId",
                table: "FamilyGroups",
                column: "UsersUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_UsersUserId",
                table: "Persons",
                column: "UsersUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserTypeId",
                table: "Users",
                column: "UserTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTypePermissions_PermissionId",
                table: "UserTypePermissions",
                column: "PermissionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentsHistorics");

            migrationBuilder.DropTable(
                name: "FamilyGroups");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "UserTypePermissions");

            migrationBuilder.DropTable(
                name: "Historics");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "UserTypes");
        }
    }
}
