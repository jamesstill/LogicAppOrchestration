-- -----------------------------------------------------------------------
-- Create database user svc_source_api_sql_user
-- Prereq in master: 
--     CREATE LOGIN svc_source_api_sql_user WITH PASSWORD='NotRealPassword';
-- -----------------------------------------------------------------------

/*
	-- Run first in master to create login:

	IF EXISTS (SELECT * FROM sys.sql_logins WHERE name  = N'svc_source_api_sql_user')
        DROP LOGIN svc_source_api_sql_user;
    GO

	CREATE LOGIN svc_source_api_sql_user WITH PASSWORD='NotRealPassword';

	select l.name as LoginName FROM sys.sql_logins l
*/


-- run second in database to assign roles:

IF EXISTS (SELECT * FROM sys.database_principals WHERE name = N'svc_source_api_sql_user')
	DROP USER svc_source_api_sql_user
GO

CREATE USER svc_source_api_sql_user FOR LOGIN svc_source_api_sql_user WITH DEFAULT_SCHEMA=[dbo]

GRANT EXECUTE TO svc_source_api_sql_user

EXEC sp_addrolemember 'db_datareader', 'svc_source_api_sql_user';
EXEC sp_addrolemember 'db_datawriter', 'svc_source_api_sql_user';

select u.name as UserName FROM sys.database_principals u WHERE name = N'svc_source_api_sql_user'