using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastFood.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderType : Migration
    {
        /// <inheritdoc />

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. ساخت جدول انواع سفارش
            migrationBuilder.CreateTable(
                name: "OrderTypes",
                columns: table => new
                {
                    Id = table.Column<int>(
                        type: "int",
                        nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),

                    Name = table.Column<string>(
                        type: "nvarchar(max)",
                        nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTypes", x => x.Id);
                });

            // 2. ثبت انواع سفارش
            migrationBuilder.InsertData(
                table: "OrderTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
            { 1, "داخل رستوران" },
            { 2, "بیرون‌بر" }
                });

            // 3. افزودن ستون موقت و nullable برای سفارش‌ها
            migrationBuilder.AddColumn<int>(
                name: "OrderTypeId",
                table: "Orders",
                type: "int",
                nullable: true);

            // 4. اختصاص نوع بیرون‌بر به تمام سفارش‌های قبلی
            migrationBuilder.Sql(
                "UPDATE [Orders] SET [OrderTypeId] = 2;");

            // 5. اجباری کردن ستون پس از مقداردهی سفارش‌های قبلی
            migrationBuilder.AlterColumn<int>(
                name: "OrderTypeId",
                table: "Orders",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            // 6. ایجاد ایندکس
            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderTypeId",
                table: "Orders",
                column: "OrderTypeId");

            // 7. ایجاد رابطه یک‌به‌چند
            migrationBuilder.AddForeignKey(
                name: "FK_Orders_OrderTypes_OrderTypeId",
                table: "Orders",
                column: "OrderTypeId",
                principalTable: "OrderTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // حذف رابطه
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_OrderTypes_OrderTypeId",
                table: "Orders");

            // حذف ایندکس
            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderTypeId",
                table: "Orders");

            // حذف ستون از سفارش‌ها
            migrationBuilder.DropColumn(
                name: "OrderTypeId",
                table: "Orders");

            // حذف جدول انواع سفارش
            migrationBuilder.DropTable(
                name: "OrderTypes");
        }

    }
}
