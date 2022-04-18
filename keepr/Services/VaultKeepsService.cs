using System;
using keepr.Models;
using keepr.Repositories;

namespace keepr.Services
{
  public class VaultKeepsService
  {
    private readonly VaultKeepsRepository _repo;
    private readonly VaultsRepository _vrepo;

    public VaultKeepsService(VaultKeepsRepository repo, VaultsRepository vrepo)
    {
      _repo = repo;
      _vrepo = vrepo;
    }

    private VaultKeep GetById(int id)
    {
      VaultKeep found = _repo.GetById(id);
      if (found == null)
      {
        throw new Exception("Invalid Id");
      }
      return found;
    }

    internal VaultKeep Create(VaultKeep data)
    {
      Vault found = _vrepo.GetById(data.VaultId);
      if (found != null)
      {
        if (found.IsPrivate == false)
        {
          if (found.CreatorId == data.CreatorId)
          {
            return _repo.Create(data);
          }
          throw new Exception("Not your Vault");
        }
        // throw new Exception("Invalid Vault");
      }
      return _repo.Create(data);
    }

    internal void Delete(int id, string userId)
    {
      VaultKeep found = GetById(id);
      if (found.CreatorId != userId)
      {
        throw new Exception("You cannot delete this");
      }
      _repo.Delete(id);
    }
  }
}