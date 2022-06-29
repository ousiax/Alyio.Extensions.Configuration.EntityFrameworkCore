[![Alyio.Extensions.Configuration.EntityFrameworkCore](https://github.com/qqbuby/Alyio.Extensions.Configuration.EntityFrameworkCore/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/qqbuby/Alyio.Extensions.Configuration.EntityFrameworkCore/actions/workflows/ci.yml)

```ps
dotnet add package Alyio.Extensions.Configuration.EntityFrameworkCore --version 3.0.2
```

EntityFrameworkCore configuration provider implementation for Microsoft.Extensions.Configuration to load configuration arbitrary from database.

```console
> sqlite3 kv.db
SQLite version 3.38.5 2022-05-06 15:25:27
Enter ".help" for usage hints.
sqlite> .tables
KeyValuePairs
sqlite> .schema KeyValuePairs
CREATE TABLE IF NOT EXISTS "KeyValuePairs" (
    "pName" TEXT NOT NULL CONSTRAINT "PK_KeyValuePairs" PRIMARY KEY,
    "pValue" TEXT NULL
);
sqlite> select * from KeyValuePairs;
Blog1|http://blog1.com
Blog2|http://blog2.com
sqlite> .exit
```

```cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var connectString = "Data Source=kv.db";
var tableName = "KeyValuePairs";
var keyColumnName = "pName";
var valueColumeName = "pValue";

var builder = new ConfigurationBuilder()
    .AddEntityFrameworkCore(dbOpt => dbOpt.UseSqlite(connectString), tableName, keyColumnName, valueColumeName);

var configuration = builder.Build();

foreach (var conf in configuration.AsEnumerable())
{
    Console.WriteLine("{0} = {1}", conf.Key, conf.Value);
}
```

```console
> dotnet run
Blog2 = http://blog2.com
Blog1 = http://blog1.com
```