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
3. Для просмотра и управления сохранёнными записями доступны REST-эндпоинты `/api/quotes` (GET/POST), `/api/quotes/{id}` (GET/PUT/DELETE).

## Клиент (Angular)

1. Перейдите в каталог `quotes-ui` и установите зависимости:
   ```bash
   npm install
   ```
2. Запустите локальный дев-сервер Angular:
   ```bash
   npm start
   ```
   По умолчанию фронтенд обращается к API по адресу `http://localhost:5000/api`.

> Примечание: для получения данных из PaperQuotes требуется сетевой доступ к `https://api.paperquotes.com`.
