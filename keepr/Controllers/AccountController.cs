using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using keepr.Models;
using keepr.Services;
using CodeWorks.Auth0Provider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace keepr.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class AccountController : ControllerBase
  {
    private readonly AccountService _accountService;
    private readonly VaultsService _vs;

    public AccountController(AccountService accountService, VaultsService vs)
    {
      _accountService = accountService;
      _vs = vs;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<Account>> Get()
    {
      try
      {
        Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        return Ok(_accountService.GetOrCreateProfile(userInfo));
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    [HttpGet("vaults")]
    [Authorize]
    public ActionResult<List<Vault>> GetMyVaults(string id)
    // public async Task<ActionResult<List<Vault>>> GetMyVaults(string id)
    {
      try
      {
        // Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
        // if (userInfo.Id != id)
        // {
        //   throw new Exception("Not your account");
        // }
        List<Vault> vaults = _vs.GetMyVaults(id);
        return Ok(vaults);

      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
  }


}