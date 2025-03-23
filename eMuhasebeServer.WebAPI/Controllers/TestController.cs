using eMuhasebeServer.WebAPI.Abstractions;
using FluentEmail.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eMuhasebeServer.WebAPI.Controllers;

[AllowAnonymous]
public class TestController : ApiController
{
    private readonly IFluentEmail _fluentEmail;
    public TestController(IMediator mediator, IFluentEmail fluentEmail) : base(mediator)
    {
        _fluentEmail = fluentEmail;
    }

    [HttpGet]
    public async Task<IActionResult> SendTestEmail()
    {
        await _fluentEmail
            .To("ect@gmail.com")
            .Subject("Test maili")
            .Body("<h1>Mail Gönderme Testi</h1>", true)
            .SendAsync();

        return NoContent();
    }
}
