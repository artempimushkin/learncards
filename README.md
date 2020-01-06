# learncards.org

<strong>Интервальные повторения</strong> – техника удержания информации в памяти, заключающаяся в повторении запомненного учебного материала по определённым, постоянно возрастающим интервалам.<br>
Наиболее популярный метод, основанный на принципе интервальных повторений, – система Лейтнера.<br>
<strong>Система Лейтнера</strong> – метод эффективного запоминания и повторения с помощью флэш-карточек. Система реализует принцип интервальных повторений, где карточки повторяются через увеличивающиеся временные интервалы.
<strong>Флэш-карточка</strong> – карточка, содержащая информацию на обеих сторонах (чаще всего в формате "вопрос-ответ"). 

Система позволяет пользователям создавать и редактировать карточки и колоды карточек, делиться ими с другими пользователями, повторять карточки, просматривать аналитику прогресса в обучении, предоставлять другим пользователям доступ к аналитике своего прогресса в обучении.

Программный продукт разработан в среде Microsoft Visual Studio 2019 Community на языке C# (.NET Core 3.1) с использованием ASP.NET MVC и Entity Framework Core с использованием СУБД PostgreSQL 10.6 (библиотека Npgsql). Клиентская часть реализована на языке JavaScript с использованием библиотеки jQuery 3.3.1.<br>
Для озвучивания слов используется API Amazon Polly.

# Диаграмма сущность-связь

![Alt text](EntityRelationship.png?raw=true "Диаграмма сущность-связь")

# Диаграмма развертывания

![Alt text](DeploymentDiagram.png?raw=true "Диаграмма развертывания")

# Инсталляция программного продукта (для AWS EC2, RDS и Amazon Polly)

Для инсталляции системы необходимо:
1.	Создать учетную запись в AWS (https://aws.amazon.com)/
2.	Согласно инструкции (https://docs.aws.amazon.com/en_us/AmazonRDS/latest/UserGuide/CHAP_GettingStarted.CreatingConnecting.PostgreSQL.html) создать экземпляр сервера базы данных PostgreSQL в Amazon RDS. Сохранить имя пользователя и пароль от базы данных в пункте «RDSConnection» в файле appsettings.json, находящемся в папке learncards.
3.	Создать базу данных «learncards» и таблицы (см. Приложение 7 в document.pdf).
4.	По инструкции (https://docs.aws.amazon.com/en_us/polly/latest/dg/setting-up.html) в учетной записи создать пользователя Amazon Polly, полученные Access Key и Secret Access Key сохранить в файле appsettings.json, находящемся в папке learncards, в пункте «AWS_Polly».
5.	Согласно инструкции (https://docs.aws.amazon.com/en_us/toolkit-for-visual-studio/latest/user-guide/deployment-ecs-aspnetcore-ec2.html) в учетной записи AWS создать экземпляр сервера EC2.
6.	По инструкции (https://medium.com/@setu677/how-to-host-asp-net-core-on-linux-using-nginx-85339560e929) установить .NET Core на сервер, загрузить на сервер learncards.dll, установить и настроить nginx.

Для запуска системы необходимо перейти в адресной строке браузера по ip-адресу, указанному в настройках сервера EC2 в учетной записи AWS.
