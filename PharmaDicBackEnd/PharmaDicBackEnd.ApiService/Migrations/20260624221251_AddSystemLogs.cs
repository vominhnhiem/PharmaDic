using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmaDicBackEnd.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class AddSystemLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Diseases",
                columns: table => new
                {
                    DiseaseID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiseaseName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WarningSigns = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Diseases__69B533A9DF36CA37", x => x.DiseaseID);
                });

            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    IngredientID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IngredientName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Ingredie__BEAEB27A0F39FA2B", x => x.IngredientID);
                });

            migrationBuilder.CreateTable(
                name: "MedicineCategories",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Medicine__19093A2B366D8A04", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "Symptoms",
                columns: table => new
                {
                    SymptomID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SymptomName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Symptoms__D26ED8B6AE45016A", x => x.SymptomID);
                });

            migrationBuilder.CreateTable(
                name: "SystemActivityLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemActivityLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "Dược sĩ"),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__1788CCAC37887F71", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Medicines",
                columns: table => new
                {
                    MedicineID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicineName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CategoryID = table.Column<int>(type: "int", nullable: true),
                    DosageForm = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Strength = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Manufacturer = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Uses = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dosage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Contraindications = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SideEffects = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Storage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Medicine__4F2128F0486E5BDD", x => x.MedicineID);
                    table.ForeignKey(
                        name: "FK__Medicines__Categ__3F466844",
                        column: x => x.CategoryID,
                        principalTable: "MedicineCategories",
                        principalColumn: "CategoryID");
                });

            migrationBuilder.CreateTable(
                name: "SymptomDisease",
                columns: table => new
                {
                    SymptomID = table.Column<int>(type: "int", nullable: false),
                    DiseaseID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SymptomD__54F58B8CCE6612A6", x => new { x.SymptomID, x.DiseaseID });
                    table.ForeignKey(
                        name: "FK__SymptomDi__Disea__4CA06362",
                        column: x => x.DiseaseID,
                        principalTable: "Diseases",
                        principalColumn: "DiseaseID");
                    table.ForeignKey(
                        name: "FK__SymptomDi__Sympt__4BAC3F29",
                        column: x => x.SymptomID,
                        principalTable: "Symptoms",
                        principalColumn: "SymptomID");
                });

            migrationBuilder.CreateTable(
                name: "SearchHistory",
                columns: table => new
                {
                    SearchID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    Keyword = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SearchType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SearchDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SearchHi__21C53514CC980D92", x => x.SearchID);
                    table.ForeignKey(
                        name: "FK__SearchHis__UserI__5812160E",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "DiseaseMedicine",
                columns: table => new
                {
                    DiseaseID = table.Column<int>(type: "int", nullable: false),
                    MedicineID = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DiseaseM__7D4721264AE26E97", x => new { x.DiseaseID, x.MedicineID });
                    table.ForeignKey(
                        name: "FK__DiseaseMe__Disea__4F7CD00D",
                        column: x => x.DiseaseID,
                        principalTable: "Diseases",
                        principalColumn: "DiseaseID");
                    table.ForeignKey(
                        name: "FK__DiseaseMe__Medic__5070F446",
                        column: x => x.MedicineID,
                        principalTable: "Medicines",
                        principalColumn: "MedicineID");
                });

            migrationBuilder.CreateTable(
                name: "DrugInteractions",
                columns: table => new
                {
                    InteractionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicineID1 = table.Column<int>(type: "int", nullable: false),
                    MedicineID2 = table.Column<int>(type: "int", nullable: false),
                    Severity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Recommendation = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DrugInte__922C03766A653988", x => x.InteractionID);
                    table.ForeignKey(
                        name: "FK__DrugInter__Medic__534D60F1",
                        column: x => x.MedicineID1,
                        principalTable: "Medicines",
                        principalColumn: "MedicineID");
                    table.ForeignKey(
                        name: "FK__DrugInter__Medic__5441852A",
                        column: x => x.MedicineID2,
                        principalTable: "Medicines",
                        principalColumn: "MedicineID");
                });

            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    FavoriteID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    MedicineID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Favorite__CE74FAF5AF258C9F", x => x.FavoriteID);
                    table.ForeignKey(
                        name: "FK__Favorites__Medic__5CD6CB2B",
                        column: x => x.MedicineID,
                        principalTable: "Medicines",
                        principalColumn: "MedicineID");
                    table.ForeignKey(
                        name: "FK__Favorites__UserI__5BE2A6F2",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "MedicineIngredients",
                columns: table => new
                {
                    MedicineID = table.Column<int>(type: "int", nullable: false),
                    IngredientID = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Medicine__F4CBC3D7D7AA2604", x => new { x.MedicineID, x.IngredientID });
                    table.ForeignKey(
                        name: "FK__MedicineI__Ingre__44FF419A",
                        column: x => x.IngredientID,
                        principalTable: "Ingredients",
                        principalColumn: "IngredientID");
                    table.ForeignKey(
                        name: "FK__MedicineI__Medic__440B1D61",
                        column: x => x.MedicineID,
                        principalTable: "Medicines",
                        principalColumn: "MedicineID");
                });

            migrationBuilder.CreateTable(
                name: "MedicineWarnings",
                columns: table => new
                {
                    WarningID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicineID = table.Column<int>(type: "int", nullable: false),
                    WarningContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WarningLevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Medicine__214571B874D10CCC", x => x.WarningID);
                    table.ForeignKey(
                        name: "FK__MedicineW__Medic__5FB337D6",
                        column: x => x.MedicineID,
                        principalTable: "Medicines",
                        principalColumn: "MedicineID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiseaseMedicine_MedicineID",
                table: "DiseaseMedicine",
                column: "MedicineID");

            migrationBuilder.CreateIndex(
                name: "IX_DrugInteractions_MedicineID1",
                table: "DrugInteractions",
                column: "MedicineID1");

            migrationBuilder.CreateIndex(
                name: "IX_DrugInteractions_MedicineID2",
                table: "DrugInteractions",
                column: "MedicineID2");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_MedicineID",
                table: "Favorites",
                column: "MedicineID");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_UserID",
                table: "Favorites",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineIngredients_IngredientID",
                table: "MedicineIngredients",
                column: "IngredientID");

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_CategoryID",
                table: "Medicines",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineWarnings_MedicineID",
                table: "MedicineWarnings",
                column: "MedicineID");

            migrationBuilder.CreateIndex(
                name: "IX_SearchHistory_UserID",
                table: "SearchHistory",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_SymptomDisease_DiseaseID",
                table: "SymptomDisease",
                column: "DiseaseID");

            migrationBuilder.CreateIndex(
                name: "UQ__Users__A9D105347A0EE942",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiseaseMedicine");

            migrationBuilder.DropTable(
                name: "DrugInteractions");

            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.DropTable(
                name: "MedicineIngredients");

            migrationBuilder.DropTable(
                name: "MedicineWarnings");

            migrationBuilder.DropTable(
                name: "SearchHistory");

            migrationBuilder.DropTable(
                name: "SymptomDisease");

            migrationBuilder.DropTable(
                name: "SystemActivityLogs");

            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropTable(
                name: "Medicines");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Diseases");

            migrationBuilder.DropTable(
                name: "Symptoms");

            migrationBuilder.DropTable(
                name: "MedicineCategories");
        }
    }
}
