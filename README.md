# QuotesService

Сервис ASP.NET Core для загрузки русскоязычных цитат из PaperQuotes и сохранения их в базу данных SQL Server через Entity Framework Core.

## Настройка

1. Укажите строку подключения SQL Server в `appsettings.json` в секции `ConnectionStrings:DefaultConnection`.
2. При необходимости измените исходный URL API в секции `PaperQuotes:BaseUrl`.

## Запуск

1. В каталоге `QuotesService` выполните миграции и сборку:
   ```bash
   dotnet ef database update
   dotnet run
   ```
2. Для импорта цитат отправьте POST-запрос на `/api/quotes/import`. Полученные данные будут сохранены в таблицу `Quotes`.
3. Для просмотра сохранённых записей используйте GET-запрос на `/api/quotes`.

> Примечание: для получения данных из PaperQuotes требуется сетевой доступ к `https://api.paperquotes.com`.
