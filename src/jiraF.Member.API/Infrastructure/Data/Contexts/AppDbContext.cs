﻿using jiraF.Member.API.GlobalVariables;
using jiraF.Member.API.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace jiraF.Member.API.Infrastructure.Data.Contexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        TestData testData = new(this);
        // TODO: Delete this check.
        if (!TestVariables.IsWorkNow)
        {
            testData.AddDefaultMember();
        }
#if DEBUG
        testData.Seed();
#endif
    }

    public DbSet<MemberEntity> Members { get; set; }
}
