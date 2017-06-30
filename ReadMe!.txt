CLFtpManager

ftpmanager [-v] [-p] [-bin] [-buff 2048] [-u login:password] [-PutFtp <FromPATH> <ToPATH>]
Команды:	 
	-help		Помощь.
	-user, -u	Логин и пароль (-u Login:Password).
	-putftp		Копировать на сервер ftp (-PutFtp <FromPATH> <ToPATH>).
	-getftp		Копировать с сервера ftp (-GetFtp <FromPATH> <ToPATH>).
	-buff		Изменить размер буфера передачи (стандартно 2048 байт).
	-p		Использовать пасивный метод передачи файлов.
	-bin		Установка режима передачи файлов в двоичном формате.
	-v		Скривать сообщения при передедаче файлов.
	-save		Сохранить файл по ссилке (-Save <URL> <TargetPATH>).
	-dir		Посмотреть файлы на ftp.
	-run		Запустить файл с командами (-run <PATH_file>).
	-V		Версия.
	
Примеры: 
		 FTPManager -u login:password -To C:\1.txt ftp://hostname/forder/1.txt
	     FTPManager -save http://site.com/file D:\1.png
		 FTPManager -run D:\1.txt


Отправка Email
        -sendEmail              Команда для отправки Email.
        -to             Адресс получателя.
        -from           Адресс отправителя.
        -pw             Пароль на Email.
        -server         SMTP сервер.
        -subject                Тема письма.
        -body           Тело письма.
        -attach         Прикрепленный файл.
        -fromName               Имя отправителя.
        -toName         Имя получателя.
        -port           Порт.
        -login          Логин.
        -timeout                Таймаут.

Пример: CLManager -sendEmail -to emailto@mail.com -from emailfrom@mail.com -pw password
-subject thema


