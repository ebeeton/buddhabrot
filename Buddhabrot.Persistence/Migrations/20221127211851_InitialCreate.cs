using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Buddhabrot.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlotQueue",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QueuedUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlotParams = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlotQueue", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "Plots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PlotParams = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    CreatedUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlotBeginUTC = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PlotEndUTC = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ImageData = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plots", x => x.Id);
                });

			// Create enqueue and dequeue procs, and add check constraints for PlotType.
			// Hat tip to http://rusanu.com/2010/03/26/using-tables-as-queues/ for the
			// queue idea.
			migrationBuilder.Sql(@"CREATE PROCEDURE uspEnqueuePlot
	                                @PlotParams nvarchar(max)
                                AS
	                                SET NOCOUNT ON;
	                                INSERT INTO PlotQueue (QueuedUTC, PlotParams)
	                                VALUES (GETUTCDATE(), @PlotParams);
                                GO

                                CREATE PROCEDURE uspDequeuePlot
                                AS
	                                SET NOCOUNT ON;
	                                WITH cte AS
	                                (
		                                SELECT TOP 1 PlotParams
		                                FROM PlotQueue WITH (ROWLOCK, READPAST)
		                                ORDER BY Id
	                                )
	                                DELETE FROM cte
	                                OUTPUT deleted.PlotParams
                                GO

                                ALTER TABLE [Plots]
                                    ADD CONSTRAINT [CHK_Plots_Type]
                                    CHECK ([TYPE] IN ('Mandelbrot', 'Buddhabrot'));");
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql("DROP PROCEDURE uspDequeuePlot");
			migrationBuilder.Sql("DROP PROCEDURE uspEnqueuePlot");

			migrationBuilder.DropTable(
                name: "PlotQueue");

            migrationBuilder.DropTable(
                name: "Plots");
        }
    }
}
