using AnnouncementWebApp.Middlewares;
using AnnouncementWebApp.Models;
using AnnouncementWebApp.Services;

namespace AnnouncementWebApp.Extensions;

/// <summary>
/// Клас розширень для додавання проміжних програмних забезпечень до конвеєру обробки запитів.
/// </summary>
public static class Extensions
{

    /// <summary>
    /// Додає проміжне програмне забезпечення для встановлення типу контенту HTTP-відповідей на "text/html; charset=utf-8".
    /// </summary>
    /// <param name="builder">Об'єкт IApplicationBuilder для додавання проміжного програмного шару.</param>
    /// <returns>Засіб побудови програми з доданим проміжним ПЗ для встановленн типу контенту.</returns>
    public static IApplicationBuilder UseSetContentTypeHtml(this IApplicationBuilder builder)
        => builder.UseMiddleware<ContentTypeHtmlMiddleware>();

    /// <summary>
    /// Додає проміжне програмне забезпечення для обробки запитів, пов'язаних з головною сторінкою та оголошеннями.
    /// </summary>
    /// <param name="builder">Об'єкт IApplicationBuilder для додавання проміжного програмного шару.</param>
    /// <param name="tags">Список тегів для оголошень.</param>
    /// <returns>Засіб побудови програми з доданим проміжним ПЗ для головної сторінки.</returns>
    public static IApplicationBuilder UseMainPage(this IApplicationBuilder builder, List<Tag> tags)
        => builder.UseMiddleware<MainPageMiddleware>(tags);

    /// <summary>
    /// Додає проміжне програмне забезпечення для обробки запитів, пов'язаних з сторінкою для кожного оголошення.
    /// </summary>
    /// <param name="builder">Об'єкт IApplicationBuilder для додавання проміжного програмного шару.</param>
    /// <returns>Засіб побудови програми з доданим проміжним ПЗ для оголошень.</returns>
    public static IApplicationBuilder UseAnnouncementPage(this IApplicationBuilder builder)
        => builder.UseMiddleware<AnnouncementPageMiddleware>();

    /// <summary>
    /// Додає проміжне програмне забезпечення для обробки виходу користувача (логауту).
    /// </summary>
    /// <param name="builder">Об'єкт IApplicationBuilder для додавання проміжного програмного шару.</param>
    /// <returns>Засіб побудови програми з доданим проміжним ПЗ для сторінки логауту.</returns>
    public static IApplicationBuilder UseLogoutPage(this IApplicationBuilder builder)
        => builder.UseMiddleware<LogoutPageMiddleware>();

    /// <summary>
    /// Додає проміжне програмне забезпечення, яке надає сторінку входу (логіну) до покладеної в засоби побудови програми послідовності обробників запитів.
    /// </summary>
    /// <param name="builder">Засіб побудови програми, до якого додається проміжне ПЗ.</param>
    /// <returns>Засіб побудови програми з доданим проміжним ПЗ для сторінки входу.</returns>
    public static IApplicationBuilder UseLoginPage(this IApplicationBuilder builder)
        => builder.UseMiddleware<LoginPageMiddleware>();

    /// <summary>
    /// Додає проміжне програмне забезпечення для обробки сторінки профілю та інших пов'язаних шляхів.
    /// </summary>
    /// <param name="builder">Засіб побудови програми, до якого додається проміжне ПЗ.</param>
    /// <returns>Засіб побудови програми з доданим проміжним ПЗ для сторінки профілю.</returns>
    public static IApplicationBuilder UseProfilePage(this IApplicationBuilder builder)
        => builder.UseMiddleware<ProfilePageMiddleware>();

    /// <summary>
    /// Додає проміжне програмне забезпечення для обробки сторінки створення оголошень та інших пов'язаних URL-шляхів.
    /// </summary>
    /// <param name="builder">Засіб побудови програми, до якого додається проміжне ПЗ.</param>
    /// <param name="tags">Список тегів оголошень.</param>
    /// <returns>Засіб побудови програми з доданим проміжним ПЗ для сторінки створення оголошень.</returns>
    public static IApplicationBuilder UseCreatePage(this IApplicationBuilder builder, List<Tag> tags)
        => builder.UseMiddleware<CreatePageMiddleware>(tags);

    /// <summary>
    /// Додає проміжне програмне забезпечення для обробки сторінки оновлення оголошень та інших пов'язаних URL-шляхів.
    /// </summary>
    /// <param name="builder">Засіб побудови програми, до якого додається проміжне ПЗ.</param>
    /// <param name="tags">Список тегів оголошень.</param>
    /// <returns>Засіб побудови програми з доданим проміжним ПЗ для сторінки створення оголошень.</returns>
    public static IApplicationBuilder UseUpdatePage(this IApplicationBuilder builder, List<Tag> tags)
        => builder.UseMiddleware<UpdatePageMiddleware>(tags);

    /// <summary>
    /// Додає проміжне програмне забезпечення для обробки сторінки реєстрації користувачів та інших пов'язаних URL-шляхів.
    /// </summary>
    /// <param name="builder">Засіб побудови програми, до якого додається проміжне ПЗ.</param>
    /// <returns>Засіб побудови програми з доданим проміжним ПЗ для сторінки реєстрації користувачів.</returns>
    public static IApplicationBuilder UseRegistrationPage(this IApplicationBuilder builder)
        => builder.UseMiddleware<RegistrationPageMiddleware>();

    /// <summary>
    /// Запускає службу превірки активних оголошень, якщо вона зареєстрована в контейнері залежностей.
    /// </summary>
    /// <param name="provider">Постачальник служб, який містить службу активних оголошень.</param>
    public static void StartAnnouncementActiveService(this IServiceProvider provider)
        => provider.GetService<AnnouncementActiveService>()?.Start();

    /// <summary>
    /// Запускає службу, яка видаляє тимчасові фотографії.
    /// </summary>
    /// <param name="provider">Постачальник служб, в якому може бути доступна служба тимчасових фотографій.</param>
    public static void StartTemporaryPhotoService(this IServiceProvider provider)
        => provider.GetService<TemporaryPhotoService>()?.Start();

    /// <summary>
    /// Здійснює пошук тегів за назвою.
    /// </summary>
    /// <param name="tags">Список тегів.</param>
    /// <param name="tagName">Назва тегу.</param>
    /// <returns>Об'єкт тегу.</returns>
    public static Tag FindTag(this List<Tag> tags, string tagName)
        => tags.FirstOrDefault(tag => tag.Name == tagName) ?? new Tag("Null");
}
