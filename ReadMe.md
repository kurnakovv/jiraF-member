[![Main repo](https://img.shields.io/static/v1?label=&message=MainRepo&color=orange)](https://github.com/KurnakovMaksim/jiraF/) 
![visitor badge](https://visitor-badge.glitch.me/badge?page_id=KurnakovMaksim.jiraF-member)
 [![](https://tokei.rs/b1/github/KurnakovMaksim/jiraF-member)](https://github.com/KurnakovMaksim/jiraF-member)
[![Help Wanted](https://img.shields.io/github/issues/KurnakovMaksim/jiraF-member/help%20wanted?color=green)](https://github.com/KurnakovMaksim/jiraF-member/issues?q=is%3Aissue+is%3Aopen+label%3A%22help+wanted%22)
[![Build/Test](https://github.com/KurnakovMaksim/jiraF-member/actions/workflows/build-test.yml/badge.svg)](https://github.com/KurnakovMaksim/jiraF-member/actions/workflows/build-test.yml)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=KurnakovMaksim_jiraF-member&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=KurnakovMaksim_jiraF-member) 
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=KurnakovMaksim_jiraF-member&metric=coverage)](https://sonarcloud.io/summary/new_code?id=KurnakovMaksim_jiraF-member) 
[![CodeQL](https://github.com/KurnakovMaksim/jiraF-member/workflows/CodeQL/badge.svg)](https://github.com/KurnakovMaksim/jiraF-member/actions?query=workflow%3ACodeQL) 
[![codecov](https://codecov.io/gh/KurnakovMaksim/jiraF-member/branch/main/graph/badge.svg?token=MXYQQKD940)](https://codecov.io/gh/KurnakovMaksim/jiraF-member)

# Member
Microservice with members logic. 

# How to setup db (not required)
* Install [postgreSQL](https://www.postgresql.org/) 
* Use this [script](https://github.com/KurnakovMaksim/jiraF/blob/main/Member/db.sql)
* Configure connection string
``` ps
dotnet user-secrets set "ConnectionString" "Server=localhost;Port=5432;Database=jiraf_member;User Id=postgres;Password=yourPassword;" --project ".\Member\src\jiraF.Member.API\"
```
* Edit program file from
``` cs
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase(TestVariables.IsWorkNow
        ? Guid.NewGuid().ToString()
        : "TestData");
    //options.UseNpgsql(builder.Configuration["ConnectionString"]);
});
```
to
``` cs
builder.Services.AddDbContext<AppDbContext>(options =>
{
    // options.UseInMemoryDatabase(TestVariables.IsWorkNow
    //     ? Guid.NewGuid().ToString()
    //     : "TestData");
    options.UseNpgsql(builder.Configuration["ConnectionString"]);
});
```
