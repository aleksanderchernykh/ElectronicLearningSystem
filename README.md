Electronic Learning System — система электронного обучения, ориентированная для внедрения в учебные учреждения. 
Предназначена для структурирования заданий, выдаваемых студентам, и улучшению взаимодействия между студентами и преподавателями.

| Назначение | Технология | Применение                            |
|----------- |------------|---------------------------------------|
|front-end|Angular 17|Основной фреймворк для веб-приложения|
|back-end|.NET 8 WebAPI|Предостовляет доступ к авторизации пользователя в системе и манипуляциями с сущностями системы|
|База данных|MS SQL|Хранит данные системы по основным сущностям|
|Маппинг объектов|AutoMapper|Используется для преобразования DTO схем в схемы сущностей|
|Очереди сообщений|Kafka|Используется для передачи информации от WebAPI к микросервису для отправки Email-сообщений|
|Контейнеризация|Docker|Используется для сборки всех модулей системы в контейнеры|
|Тестирование|xUnit, Moq|Юнит-тестирование сервисов и компонентов|
|Кеширование|Redis|Хранение кешированных данных (токены для восстановления пароля)|
|Безопасность|JWT|Аутентификация и управление доступом к API|
|CI/CD|Git Hub Actions|Автоматический билд и тестирование|

Базовые записи для тестирования системы

Роли:
|Id|Наименование|
|--|------------|
|02bc926f-9c56-4fb9-bc8e-68bbe2e87c17|Administrator|
|c0eb7e9a-b913-4cd0-bf70-146fc48764ba|Teacher|
|86b8ca0b-85ce-4aca-b911-28836645ebc7|Student|

Типы уведомлений:
|Id|Наименование|
|--|------------|
|2cbbcb67-fb42-4dcc-ae89-61f93a283d10|New comment sent|
|d569e3db-3daa-435c-9a02-2f21d19132f9|A new message has been sent|

Пользователи:
|Логин|Пароль|Роль|
|--|------------|
|Administrator|RsQOluzt|Administrator|
|Teacher|TZewZmJj|Teacher|
|Student|FXRCYoix|Student|
