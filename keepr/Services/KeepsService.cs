using System;
using System.Collections.Generic;
using keepr.Models;
using keepr.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace keepr.Services
{
  public class KeepsService
  {
    private readonly KeepsRepository _repo;
    private readonly VaultsRepository _vrepo;

    public KeepsService(KeepsRepository repo, VaultsRepository vrepo)
    {
      _repo = repo;
      _vrepo = vrepo;
    }

    internal Keep Create(Keep data)
    {
      return _repo.Create(data);
    }

    internal List<Keep> GetAll()
    {
      return _repo.GetAll();
    }
    internal Keep GetById(int id)
    {
      Keep keep = _repo.GetById(id);
      keep.Views++;
      return keep;
    }

    internal Keep Update(Keep data)
    {
      Keep original = GetById(data.Id);
      ValidateOwner(data.CreatorId, original);
      original.Name = data.Name ?? original.Name;
      original.Description = data.Description ?? original.Description;
      original.Img = data.Img ?? original.Img;
      original.Kept = data.Kept;
      _repo.Update(original);
      return original;
    }
    private Keep UpdateKept(Keep data)
    {
      Keep original = GetById(data.Id);
      ValidateOwner(data.CreatorId, original);
      original.Kept = data.Kept;
      _repo.Update(original);
      return original;

    }
    internal void Remove(int id, string userId)
    {
      Keep original = GetById(id);
      ValidateOwner(userId, original);
      _repo.Remove(id);
    }


    private static void ValidateOwner(string userId, Keep data)
    {
      if (userId != data.CreatorId)
      {
        throw new Exception("Not your data!");
      }
    }

    internal List<Keep> GetProfileKeeps(string id)
    {
      return _repo.GetProfileKeeps(id);
    }


    internal ActionResult<List<KeepViewModel>> GetVaultKeeps(int id, string userId)
    {
      Vault found = _vrepo.GetById(id);
      if (found != null)
      {
        if (found.IsPrivate == true)
        {
          if (found.CreatorId == userId)
          {
            return _repo.GetVaultKeeps(id);
          }
          throw new Exception("Invalid vault");
        }
        return _repo.GetVaultKeeps(id);
      }
      throw new Exception("Invalid Id");
    }

    internal ActionResult<List<KeepViewModel>> GetVaultKeeps(int id)
    {
      Vault found = _vrepo.GetById(id);
      if (found != null)
      {
        if (found.IsPrivate == true)
        {
          throw new Exception("Invalid vault");
        }
        return _repo.GetVaultKeeps(id);
      }
      throw new Exception("Invalid Id");
    }
  }
}
