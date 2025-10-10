using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BolomorzKeyManager.Model.Migrations
{
    /// <inheritdoc />
    public partial class KeyManager : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    UAID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: true),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    HashParameter = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.UAID);
                });

            migrationBuilder.CreateTable(
                name: "Keys",
                columns: table => new
                {
                    KID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    EncryptedData = table.Column<byte[]>(type: "BLOB", nullable: true),
                    UAID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keys", x => x.KID);
                    table.ForeignKey(
                        name: "FK_Keys_UserAccounts_UAID",
                        column: x => x.UAID,
                        principalTable: "UserAccounts",
                        principalColumn: "UAID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Passwords",
                columns: table => new
                {
                    PID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    EncryptedData = table.Column<byte[]>(type: "BLOB", nullable: true),
                    UAID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passwords", x => x.PID);
                    table.ForeignKey(
                        name: "FK_Passwords_UserAccounts_UAID",
                        column: x => x.UAID,
                        principalTable: "UserAccounts",
                        principalColumn: "UAID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Keys_UAID",
                table: "Keys",
                column: "UAID");

            migrationBuilder.CreateIndex(
                name: "IX_Passwords_UAID",
                table: "Passwords",
                column: "UAID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Keys");

            migrationBuilder.DropTable(
                name: "Passwords");

            migrationBuilder.DropTable(
                name: "UserAccounts");
        }
    }
}
