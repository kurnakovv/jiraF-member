﻿using jiraF.Member.API.Infrastructure.Data.Contexts;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using jiraF.Member.API.Dtos.Member.Registration;
using System.Text.Json;
using System.Text;
using System.Collections.Generic;
using jiraF.Member.API.GlobalVariables;

namespace jiraF.Member.EndToEndTests.Controllers;

public class MemberControllerTests : IDisposable
{
    private readonly HttpClient _client;

    public MemberControllerTests()
    {
        TestVariables.IsWorkNow = true;
        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddDbContext<AppDbContext>(options =>
                    {
                        options.UseInMemoryDatabase(Guid.NewGuid().ToString());
                    });
                });
            });

        _client = application.CreateClient();
    }

    public void Dispose()
    {
        TestVariables.IsWorkNow = false;
    }

    [Theory]
    [InlineData("/Member/2f857708-6e97-413b-b495-f2161135616a")]
    [InlineData("/Member/2f857708-6e97-413b-b495-f2161135616b")]
    [InlineData("/Member/2f857708-6e97-413b-b495-f2161135616c")]
    [InlineData("/Member/IsExists/2f857708-6e97-413b-b495-f2161135616a")]
    [InlineData("/Member/IsExists/2f857708-6e97-413b-b495-f2161135616f")]
    public async Task CheckAllGETApiMethodsIsValid_StatusCode200(string url)
    {
        HttpResponseMessage response = await _client.GetAsync(url);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("application/json; charset=utf-8",
            response?.Content?.Headers?.ContentType?.ToString());
    }

    [Fact]
    public async Task GetByIds_CanGetDataByMemberIds_StatusCode200()
    {
        List<Guid> requestDto = new()
        {
            new Guid("2f857708-6e97-413b-b495-f2161135616a"),
            new Guid("2f857708-6e97-413b-b495-f2161135616b")
        };
        string jsonModel = JsonSerializer.Serialize(requestDto);
        var stringContent = new StringContent(jsonModel, UnicodeEncoding.UTF8, "application/json");
        HttpResponseMessage response = await _client.PostAsync("/Member/GetByIds", stringContent);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Registration_CanRegistrateValidMember_StatusCode200()
    {
        RegistrationMemberRequestDto requestDto = new()
        {
            Name = "TestName",
        };
        string jsonModel = JsonSerializer.Serialize(requestDto);
        var stringContent = new StringContent(jsonModel, UnicodeEncoding.UTF8, "application/json");

        HttpResponseMessage response = await _client.PostAsync("/Member", stringContent);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Bun_CanBunMemberIfMemberExists_StatusCode_200()
    {
        HttpResponseMessage response = await _client.DeleteAsync("/Member/2f857708-6e97-413b-b495-f2161135616a");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
