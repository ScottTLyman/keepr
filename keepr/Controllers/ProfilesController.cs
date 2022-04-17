using System;
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

    public ProfilesController(ProfilesService ps, VaultsService vs)
    {
      _ps = ps;
      _vs = vs;
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
  }
}