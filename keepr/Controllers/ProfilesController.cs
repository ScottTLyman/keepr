using System;
using System.Collections.Generic;
using keepr.Models;
using keepr.Services;
using Microsoft.AspNetCore.Mvc;

namespace keepr.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ProfilesController : ControllerBase
  {
    private readonly ProfilesService _ps;
    private readonly VaultsService _vs;
    private readonly KeepsService _ks;

    public ProfilesController(ProfilesService ps, VaultsService vs, KeepsService ks)
    {
      _ps = ps;
      _vs = vs;
      _ks = ks;
    }

    [HttpGet("{id}")]
    public ActionResult<Profile> GetById(string id)
    {
      try
      {
        Profile profile = _ps.GetById(id);
        if (profile == null)
        {
          throw new Exception("Invalid Id");
        }
        return Ok(profile);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    [HttpGet("{id}/keeps")]
    public ActionResult<List<Keep>> GetProfileKeeps(string id)
    {
      try
      {
        List<Keep> keeps = _ks.GetProfileKeeps(id);
        return Ok(keeps);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    [HttpGet("{id}/vaults")]
    public ActionResult<List<Vault>> GetProfileVaults(string id)
    {
      try
      {
        // Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
        // if (userInfo != null){
        List<Vault> vaults = _vs.GetProfileVaults(id);
        return Ok(vaults);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
  }
}