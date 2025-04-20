using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatSample.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    userId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Owner1 = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Owner2 = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    creation = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.userId);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    userId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsReply = table.Column<bool>(type: "bit", nullable: false),
                    RepliedMessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BelongtouserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    creation = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.userId);
                    table.ForeignKey(
                        name: "FK_Messages_Chats_BelongtouserId",
                        column: x => x.BelongtouserId,
                        principalTable: "Chats",
                        principalColumn: "userId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_BelongtouserId",
                table: "Messages",
                column: "BelongtouserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Chats");
        }
    }
}
