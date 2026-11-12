using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ZentrixBarber.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "TreatmentCategories",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.InsertData(
                table: "Barbers",
                columns: new[] { "Id", "Bio", "FullName", "ImageUrl", "IsActive", "YearsOfExperience" },
                values: new object[,]
                {
                    { 1, "Experienced barber with a passion for classic cuts.", "Martin Atanasov", null, true, 10 },
                    { 2, "Specializes in modern styles and beard grooming.", "George Donchev", null, true, 8 },
                    { 3, "Known for precision cuts and excellent customer service.", "Lazar Stoychev", null, true, 12 }
                });

            migrationBuilder.InsertData(
                table: "TreatmentCategories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Classic haircuts for all hair types.", "Haircut" },
                    { 2, "Professional beard trimming and styling.", "Beard Grooming" },
                    { 3, "Combo treatment including haircut and beard grooming.", "Combo" }
                });

            migrationBuilder.InsertData(
                table: "Treatments",
                columns: new[] { "Id", "CategoryId", "Description", "DurationMinutes", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 1, "A traditional haircut for men.", 30, "Classic Haircut", 15.00m },
                    { 2, 1, "Precise fade down to the skin with a sharp line-up.", 45, "Skin Fade", 18.00m },
                    { 3, 1, "Haircut for kids under 12.", 30, "Kids Haircut", 12.00m },
                    { 4, 2, "Beard trim, shaping and hot towel finish.", 20, "Beard Trim & Shape", 10.00m },
                    { 5, 2, "Traditional straight razor shave with hot towels.", 30, "Hot Towel Shave", 14.00m },
                    { 6, 3, "Full haircut combined with beard trim and shaping.", 60, "Haircut & Beard", 22.00m }
                });

            migrationBuilder.InsertData(
                table: "BarberTreatments",
                columns: new[] { "BarberId", "TreatmentId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 1, 4 },
                    { 1, 5 },
                    { 1, 6 },
                    { 2, 1 },
                    { 2, 2 },
                    { 2, 3 },
                    { 2, 6 },
                    { 3, 1 },
                    { 3, 3 },
                    { 3, 4 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BarberTreatments",
                keyColumns: new[] { "BarberId", "TreatmentId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "BarberTreatments",
                keyColumns: new[] { "BarberId", "TreatmentId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "BarberTreatments",
                keyColumns: new[] { "BarberId", "TreatmentId" },
                keyValues: new object[] { 1, 4 });

            migrationBuilder.DeleteData(
                table: "BarberTreatments",
                keyColumns: new[] { "BarberId", "TreatmentId" },
                keyValues: new object[] { 1, 5 });

            migrationBuilder.DeleteData(
                table: "BarberTreatments",
                keyColumns: new[] { "BarberId", "TreatmentId" },
                keyValues: new object[] { 1, 6 });

            migrationBuilder.DeleteData(
                table: "BarberTreatments",
                keyColumns: new[] { "BarberId", "TreatmentId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "BarberTreatments",
                keyColumns: new[] { "BarberId", "TreatmentId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "BarberTreatments",
                keyColumns: new[] { "BarberId", "TreatmentId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "BarberTreatments",
                keyColumns: new[] { "BarberId", "TreatmentId" },
                keyValues: new object[] { 2, 6 });

            migrationBuilder.DeleteData(
                table: "BarberTreatments",
                keyColumns: new[] { "BarberId", "TreatmentId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "BarberTreatments",
                keyColumns: new[] { "BarberId", "TreatmentId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "BarberTreatments",
                keyColumns: new[] { "BarberId", "TreatmentId" },
                keyValues: new object[] { 3, 4 });

            migrationBuilder.DeleteData(
                table: "Barbers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Barbers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Barbers",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Treatments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Treatments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Treatments",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Treatments",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Treatments",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Treatments",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "TreatmentCategories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TreatmentCategories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TreatmentCategories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "Description",
                table: "TreatmentCategories");
        }
    }
}
