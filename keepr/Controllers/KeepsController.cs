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
  public class KeepsController : ControllerBase
  {
    private readonly KeepsService _ks;

    public KeepsController(KeepsService ks)
    {
      _ks = ks;
    }
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Keep>> CreateAsync([FromBody] Keep data)
    {
      try
      {
        Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
        data.CreatorId = userInfo.Id;
        Keep keep = _ks.Create(data);
        keep.Creator = userInfo;
        return Created($"api/keeps/{keep.Id}", keep);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    [HttpGet]
    public ActionResult<List<Keep>> GetAll()
    {
      try
      {
        List<Keep> keeps = _ks.GetAll();
        return Ok(keeps);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    [HttpGet("{id}")]
    public ActionResult<Keep> GetById(int id)
    {
      try
      {
        Keep keep = _ks.GetById(id);
        if (keep == null)
        {
          throw new Exception("Invalid Id");
        }
        return Ok(keep);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<Keep>> UpdateAsync(int id, [FromBody] Keep data)
    {
      try
      {
        Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
        data.Id = id;
        data.CreatorId = userInfo.Id;
        Keep keep = _ks.Update(data);
        return Ok(keep);
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
        _ks.Remove(id, userInfo.Id);
        return Ok("Keep has been deleted");
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
  }
}