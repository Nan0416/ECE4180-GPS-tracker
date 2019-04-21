using Microsoft.EntityFrameworkCore.Migrations;

namespace web_code.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Trips",
                columns: table => new
                {
                    tripId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    devicenum = table.Column<int>(nullable: false),
                    status = table.Column<int>(nullable: false),
                    startTime = table.Column<long>(nullable: false),
                    endTime = table.Column<long>(nullable: false),
                    start_lat_ = table.Column<double>(nullable: false),
                    start_long_ = table.Column<double>(nullable: false),
                    end_lat_ = table.Column<double>(nullable: false),
                    end_long_ = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trips", x => x.tripId);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    timeStamp = table.Column<long>(nullable: false),
                    tripId = table.Column<int>(nullable: false),
                    lat_ = table.Column<double>(nullable: false),
                    long_ = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => new { x.tripId, x.timeStamp });
                    table.ForeignKey(
                        name: "FK_Locations_Trips_tripId",
                        column: x => x.tripId,
                        principalTable: "Trips",
                        principalColumn: "tripId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Trips");
        }
    }
}
