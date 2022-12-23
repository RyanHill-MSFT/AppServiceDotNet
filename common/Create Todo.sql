CREATE TABLE dbo.ToDo (
    Id uniqueidentifier primary key,
    [Order] int null,
    Title nvarchar(200) not null,
    [Description] nvarchar(1000) null,
    DueDate datetime null,
    Created datetime not null CONSTRAINT DF_Todo_Created DEFAULT GETDATE(),
    [url] nvarchar(255) null,
    Completed bit null
);