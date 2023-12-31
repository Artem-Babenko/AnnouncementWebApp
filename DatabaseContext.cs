﻿using AnnouncementWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AnnouncementWebApp;

/// <summary>
/// Клас, що представляє контекст бази даних для взаємодії з таблицями Users, Announcements та TemporaryPhotos.
/// Використовує SQLite як джерело даних.
/// </summary>
public class DatabaseContext : DbContext
{
    /// <summary>
    /// Набір даних користувачів.
    /// </summary>
    public DbSet<User> Users { get; set; } = null!;

    /// <summary>
    /// Набір даних оголошень.
    /// </summary>
    public DbSet<Announcement> Announcements { get; set; } = null!;

    /// <summary>
    /// Набір даних тимчасових фотографій.
    /// </summary>
    public DbSet<TemporaryPhoto> TemporaryPhotos { get; set; } = null!;

    /// <summary>
    /// Налаштування підключення до бази даних SQLite через Entity Framework Core.
    /// </summary>
    /// <param name="options">Об'єкт для налаштування параметрів підключення.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder options)
     => options.UseSqlite("Data Source=annoncementapp.db");

    /// <summary>
    /// Метод для визначення моделі даних та її відображення в базі даних.
    /// </summary>
    /// <param name="modelBuilder">Об'єкт для визначення моделі даних та її відображення.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Список коористувачів
        List<User> users = new List<User>()
        {
            new User(Guid.NewGuid().ToString(), "Артем", "Бабенко", "artem@gmail.com", "12345", "admin", DateTime.Now),
            new User(Guid.NewGuid().ToString(), "Том", "Томас", "tom@gmail.com", "00000", "user", DateTime.Now)
        };

        // Список оголошень
        List<Announcement> announcements = new List<Announcement>()
        {
            new Announcement(
                id: Guid.NewGuid().ToString(),
                photoLink: "/photos/red_flower_cold_snow.jpeg",
                name: "Різке похолодання",
                shortDescription: "У неділю, 8 жовтня, в західних областях місцями передбачаються​​​​​​​ заморозки -3 градуси. Ближче до вихідних, 7-8 жовтня, прийде відчутне похолодання. Очікується поривчастий вітер, місцями з невеликими дощами.",
                fullDescription: "У неділю, 8 жовтня, в західних областях місцями передбачаються заморозки -3 градуси.\r\n\r\nБлижче до вихідних, 7-8 жовтня, прийде відчутне похолодання. Очікується поривчастий вітер, місцями з невеликими дощами.\r\n\r\nТакий прогноз озвучив синоптик Ігор Кібальчич, передає meteoprog.\r\n\r\nЗа його словами, середньодобова температура в жовтні у більшості областей опуститься нижче +15 °С.\r\n\r\n\"Друга половина наступного тижня ознаменується прохолодною, вітряною погодою, місцями з невеликими дощами та суттєвим похолоданням. Очікувані умови погоди укладаються в кліматичну норму для цього сезону та не є аномальними\", – попередив він.\r\n\r\nЗниження температури очікується 5 жовтня. До України прийде антициклон із заходу. Очікується мінлива хмарність, без опадів. Температура повітря вночі коливатиметься в межах +6…+11°С, вдень +17…+22°С.\r\n\r\n\"У п'ятницю, 6 жовтня, у зв'язку з розвитком циклонічності над країнами Балтії значно знизиться тиск і пройдуть дощі в більшості областей України, крім західних регіонів. На Лівобережжі та в південних областях помірні дощі, місцями в супроводі гроз\", – йдеться у його прогнозі.\r\n\r\nПогода у вихідні\r\n\r\nВ суботу, 7 жовтня, в західних і місцями в північних областях пройдуть невеликі короткочасні дощі. На решті території країни без істотних опадів. Температура повітря вночі +3...+8 °С, вдень +10...+15 °С.\r\n\r\nВ неділю, 8 жовтня, у західних областях місцями передбачаються заморозки -3...0 °С; вдень +7…+12 °С. Вдень істотних опадів не очікується, лише на півночі Лівобережжя місцями пройде невеликий дощ. Вітер північно-західний, 7-12 м/с, місцями пориви, 15-20 м/с. Температура повітря вночі +2...+7 °С.",
                owner: "Артем Бабенко",
                ownerId: users[0].Id,
                tag: "Погода",
                startDate: Announcement.StringDateToDateTime("23.10.23"),
                endDate: Announcement.StringDateToDateTime("20.11.23"),
                telegram: "",
                email: "",
                address: "",
                phone: "",
                views: 23),
            new Announcement(
                id: Guid.NewGuid().ToString(),
                photoLink: "/photos/new_lunch-ov.jpg",
                name: "Нова страва від BUFET",
                shortDescription: "Ланч з овочами в складі якого: Запечене куряче м’ясо, броколі, спаржева квасоля, морква, солодкий перець, червона цибуля, зелень, кисло-солодкий соус Чилі. Вже чекає на тебе у всіх BUFET.",
                fullDescription: "Ласощі на ланч у ресторані BUFET! Сьогодні в нашому меню - справжня гастрономічна насолода. На обід вас очікує смачне поєднання інгредієнтів:\r\n\r\n1. Запечене куряче м'ясо: Ніжне куряче філе, обсмажене до золотистої скоринки та запечене в духмяних спеціях.\r\n    \r\n2. Броколі: Свіжі квітки броколі, які відмінно доповнюють смак страви своєю ароматною текстурою.\r\n    \r\n3. Спаржева квасоля: Додайте нотку елегантності зі спаржевою квасолею, яка надає страві особливий шарм.\r\n    \r\n4. Морква: Соковита морква, нарізана тонкими колечками, для свіжості та хрусткості.\r\n    \r\n5. Солодкий перець: Солодкуватий перець, який принесе приємну солодкість та аромат.\r\n    \r\n6. Червона цибуля: Цибуля, обсмажена до золотавої карамелізації, для глибшого смаку.\r\n    \r\n7. Зелень: Соковита зелень, яка додає свіжості та аромату.\r\n    \r\n8. Кисло-солодкий соус Чилі: Неабиякий соус, який додає страві виразний смак та гострість.\r\n    \r\nЦя страва - ідеальний вибір для тих, хто цінує смачну їжу та бажає отримати комбінацію смаків та текстур. Будь ласка, завітайте до BUFET та насолоджуйтесь цим смаковитим ланчем з овочами сьогодні!",
                owner: "Артем Бабенко",
                ownerId: users[0].Id,
                tag: "Їжа",
                startDate: Announcement.StringDateToDateTime("29.10.23"),
                endDate: Announcement.StringDateToDateTime("17.11.23"),
                telegram: "",
                email: "bufet-food@gmail.com",
                address: "пр. Володимира Івасюка 37",
                phone: "+380979242885",
                views: 57)
        };

        // Список коористувачів
        modelBuilder.Entity<User>().HasData(users);

        // Список оголошень
        modelBuilder.Entity<Announcement>().HasData(announcements);
            
    }
    // add-migration IntialMigration
    // update-database
}