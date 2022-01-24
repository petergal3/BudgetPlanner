Select dbo.userlogin.userId FROM dbo.userlogin
WHERE dbo.userlogin.username = 'peter' 
and dbo.userlogin.password = 'Prodev'

insert into dbo.userlogin(username, email, password) values('peter', '@', 'Prodev')