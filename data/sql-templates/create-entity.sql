CREATE TABLE dbo.{TableName}
	(
	{PrimaryColumnName} {PrimaryColumnType} NOT NULL {PrimaryIsIdentity},
	)  ON [PRIMARY]
	 

ALTER TABLE dbo.{TableName} ADD CONSTRAINT
	PK_{TableName} PRIMARY KEY CLUSTERED
	(
	{PrimaryColumnName}
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]