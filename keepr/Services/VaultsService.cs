using System;
using System.Collections.Generic;
using keepr.Models;
using keepr.Repositories;

namespace keepr.Services
{
  public class VaultsService
  {
    private readonly VaultsRepository _repo;

    public VaultsService(VaultsRepository repo)
    {
      _repo = repo;
    }

    internal Vault Create(Vault data)
    {
      return _repo.Create(data);
    }

    internal List<Vault> GetAll()
    {
      return _repo.GetAll();
    }

    internal Vault GetById(int id)
    {
      return _repo.GetById(id);
    }
    internal List<Vault> GetProfileVaults(string id)
    {
      return _repo.GetProfileVaults(id);
    }
    internal List<Vault> GetMyVaults(string id)
    {
      return _repo.GetMyVaults(id);
    }
    internal Vault Update(Vault data)
    {
      Vault original = GetById(data.Id);
      ValidateOwner(data.CreatorId, original);
      original.Name = data.Name ?? original.Name;
      original.Description = data.Description ?? original.Description;
      original.Img = data.Img ?? original.Img;
      original.IsPrivate = !original.IsPrivate;
      _repo.Update(original);
      return original;
    }
    internal void Remove(int id, string userId)
    {
      Vault original = GetById(id);
      ValidateOwner(userId, original);
      _repo.Remove(id);
    }


    private static void ValidateOwner(string userId, Vault data)
    {
      if (userId != data.CreatorId)
      {
        throw new Exception("Not your data!");
      }
    }
  }
}