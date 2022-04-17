using System;
using System.Collections.Generic;
using keepr.Models;
using keepr.Repositories;

namespace keepr.Services
{
  public class KeepsService
  {
    private readonly KeepsRepository _repo;

    public KeepsService(KeepsRepository repo)
    {
      _repo = repo;
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
      return _repo.GetById(id);
    }

    internal Keep Update(Keep data)
    {
      Keep original = GetById(data.Id);
      ValidateOwner(data.CreatorId, original);
      original.Name = data.Name ?? original.Name;
      original.Description = data.Description ?? original.Description;
      original.Img = data.Img ?? original.Img;
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
        throw new Exception("You cannot edit someone else's Keep!");
      }
    }

    internal List<Keep> GetVaultKeeps(int vaultId)
    {
      return _repo.GetVaultKeeps(vaultId);
    }
  }
}
