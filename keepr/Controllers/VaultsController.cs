using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeWorks.Auth0Provider;
using keepr.Models;
using keepr.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace keepr.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class VaultsController : ControllerBase
  {
    private readonly VaultsService _vs;
    private readonly KeepsService _ks;

    public VaultsController(VaultsService vs, KeepsService ks)
    {
      _vs = vs;
      _ks = ks;
    }
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Vault>> CreateAsync([FromBody] Vault data)
    {
      try
      {
        Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
        data.CreatorId = userInfo.Id;
        Vault vault = _vs.Create(data);
        data.Creator = userInfo;
        return Created($"api/vaults/{vault.Id}", vault);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    [HttpGet]
    public ActionResult<List<Vault>> GetAll()
    {
      try
      {
        List<Vault> vaults = _vs.GetAll();
        return Ok(vaults);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Vault>> GetById(int id)
    {
      try
      {
        Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
        Vault vault = _vs.GetById(id);
        if (vault == null)
        {
          throw new Exception("Invalid Id");
        }
        if (vault.IsPrivate == true && vault.CreatorId != userInfo.Id)
        {
          throw new Exception("Vault unavailable");
        }
        return Ok(vault);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    [HttpGet("{id}/keeps")]
    public ActionResult<List<Keep>> GetVaultKeeps(int id)
    {
      try
      {
        // Vault found = await GetById(id);

        List<Keep> keeps = _ks.GetVaultKeeps(id);
        return Ok(keeps);


      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<Vault>> UpdateAsync(int id, [FromBody] Vault data)
    {
      try
      {
        Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
        // if (data.CreatorId != userInfo.Id)
        // {
        //   throw new Exception("You are not authorized to edit this");
        // }
        data.Id = id;
        data.CreatorId = userInfo.Id;
        Vault vault = _vs.Update(data);
        return Ok(vault);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult<string>> DeleteAsync(int id)
    {
      try
      {
        Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
        _vs.Remove(id, userInfo.Id);
        return Ok("Vault has been deleted");
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
  }
}