using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnnouncementWebApp.Migrations
{
    public partial class IntialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Announcements",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    PhotoLink = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ShortDescription = table.Column<string>(type: "TEXT", nullable: false),
                    FullDescription = table.Column<string>(type: "TEXT", nullable: false),
                    Owner = table.Column<string>(type: "TEXT", nullable: false),
                    OwnerId = table.Column<string>(type: "TEXT", nullable: false),
                    Tag = table.Column<string>(type: "TEXT", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    StartDateString = table.Column<string>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDateString = table.Column<string>(type: "TEXT", nullable: false),
                    Telegram = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: false),
                    Views = table.Column<int>(type: "INTEGER", nullable: false),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TemporaryPhotos",
                columns: table => new
                {
                    Path = table.Column<string>(type: "TEXT", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemporaryPhotos", x => x.Path);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Surname = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false),
                    Views = table.Column<int>(type: "INTEGER", nullable: false),
                    RegistrationDate = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Announcements",
                columns: new[] { "Id", "Active", "Address", "Email", "EndDate", "EndDateString", "FullDescription", "Name", "Owner", "OwnerId", "Phone", "PhotoLink", "ShortDescription", "StartDate", "StartDateString", "Tag", "Telegram", "Views" },
                values: new object[] { "12fb670b-5248-4a43-9dd3-9af45381cd76", false, "", "", new DateTime(2023, 10, 29, 23, 59, 0, 0, DateTimeKind.Unspecified), "29.10.23", "У неділю, 8 жовтня, в західних областях місцями передбачаються заморозки -3 градуси.\r\n\r\nБлижче до вихідних, 7-8 жовтня, прийде відчутне похолодання. Очікується поривчастий вітер, місцями з невеликими дощами.\r\n\r\nТакий прогноз озвучив синоптик Ігор Кібальчич, передає meteoprog.\r\n\r\nЗа його словами, середньодобова температура в жовтні у більшості областей опуститься нижче +15 °С.\r\n\r\n\"Друга половина наступного тижня ознаменується прохолодною, вітряною погодою, місцями з невеликими дощами та суттєвим похолоданням. Очікувані умови погоди укладаються в кліматичну норму для цього сезону та не є аномальними\", – попередив він.\r\n\r\nЗниження температури очікується 5 жовтня. До України прийде антициклон із заходу. Очікується мінлива хмарність, без опадів. Температура повітря вночі коливатиметься в межах +6…+11°С, вдень +17…+22°С.\r\n\r\n\"У п'ятницю, 6 жовтня, у зв'язку з розвитком циклонічності над країнами Балтії значно знизиться тиск і пройдуть дощі в більшості областей України, крім західних регіонів. На Лівобережжі та в південних областях помірні дощі, місцями в супроводі гроз\", – йдеться у його прогнозі.\r\n\r\nПогода у вихідні\r\n\r\nВ суботу, 7 жовтня, в західних і місцями в північних областях пройдуть невеликі короткочасні дощі. На решті території країни без істотних опадів. Температура повітря вночі +3...+8 °С, вдень +10...+15 °С.\r\n\r\nВ неділю, 8 жовтня, у західних областях місцями передбачаються заморозки -3...0 °С; вдень +7…+12 °С. Вдень істотних опадів не очікується, лише на півночі Лівобережжя місцями пройде невеликий дощ. Вітер північно-західний, 7-12 м/с, місцями пориви, 15-20 м/с. Температура повітря вночі +2...+7 °С.", "Різке похолодання", "Артем Бабенко", "5339b1de-f5ba-446c-a82f-1bcc3d992025", "", "/photos/red_flower_cold_snow.jpeg", "У неділю, 8 жовтня, в західних областях місцями передбачаються​​​​​​​ заморозки -3 градуси. Ближче до вихідних, 7-8 жовтня, прийде відчутне похолодання. Очікується поривчастий вітер, місцями з невеликими дощами.", new DateTime(2023, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "05.10.23", "Погода", "", 23 });

            migrationBuilder.InsertData(
                table: "Announcements",
                columns: new[] { "Id", "Active", "Address", "Email", "EndDate", "EndDateString", "FullDescription", "Name", "Owner", "OwnerId", "Phone", "PhotoLink", "ShortDescription", "StartDate", "StartDateString", "Tag", "Telegram", "Views" },
                values: new object[] { "a3a012b3-495a-46a9-b0a7-df0a0345508a", false, "пр. Володимира Івасюка 37", "bufet-food@gmail.com", new DateTime(2023, 10, 31, 23, 59, 0, 0, DateTimeKind.Unspecified), "31.10.23", "Ласощі на ланч у ресторані BUFET! Сьогодні в нашому меню - справжня гастрономічна насолода. На обід вас очікує смачне поєднання інгредієнтів:\r\n\r\n1. Запечене куряче м'ясо: Ніжне куряче філе, обсмажене до золотистої скоринки та запечене в духмяних спеціях.\r\n    \r\n2. Броколі: Свіжі квітки броколі, які відмінно доповнюють смак страви своєю ароматною текстурою.\r\n    \r\n3. Спаржева квасоля: Додайте нотку елегантності зі спаржевою квасолею, яка надає страві особливий шарм.\r\n    \r\n4. Морква: Соковита морква, нарізана тонкими колечками, для свіжості та хрусткості.\r\n    \r\n5. Солодкий перець: Солодкуватий перець, який принесе приємну солодкість та аромат.\r\n    \r\n6. Червона цибуля: Цибуля, обсмажена до золотавої карамелізації, для глибшого смаку.\r\n    \r\n7. Зелень: Соковита зелень, яка додає свіжості та аромату.\r\n    \r\n8. Кисло-солодкий соус Чилі: Неабиякий соус, який додає страві виразний смак та гострість.\r\n    \r\nЦя страва - ідеальний вибір для тих, хто цінує смачну їжу та бажає отримати комбінацію смаків та текстур. Будь ласка, завітайте до BUFET та насолоджуйтесь цим смаковитим ланчем з овочами сьогодні!", "Нова страва від BUFET", "Артем Бабенко", "5339b1de-f5ba-446c-a82f-1bcc3d992025", "+380979242885", "/photos/new_lunch-ov.jpg", "Ланч з овочами в складі якого: Запечене куряче м’ясо, броколі, спаржева квасоля, морква, солодкий перець, червона цибуля, зелень, кисло-солодкий соус Чилі. Вже чекає на тебе у всіх BUFET.", new DateTime(2023, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "15.10.23", "Їжа", "", 57 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "Password", "RegistrationDate", "Role", "Surname", "Views" },
                values: new object[] { "5339b1de-f5ba-446c-a82f-1bcc3d992025", "artem@gmail.com", "Артем", "12345", "01.11.23", "admin", "Бабенко", 0 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "Password", "RegistrationDate", "Role", "Surname", "Views" },
                values: new object[] { "c94f74fd-65a0-4d72-b759-899518ed1e0f", "tom@gmail.com", "Том", "00000", "01.11.23", "user", "Томас", 0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Announcements");

            migrationBuilder.DropTable(
                name: "TemporaryPhotos");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
