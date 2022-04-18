using System;
using keepr.Models;
using keepr.Repositories;

namespace keepr.Services
{
  public class VaultKeepsService
  {
    private readonly VaultKeepsRepository _repo;
    private readonly VaultsRepository _vrepo;
    private readonly KeepsRepository _krepo;

    public VaultKeepsService(VaultKeepsRepository repo, VaultsRepository vrepo, KeepsRepository krepo)
    {
      _repo = repo;
      _vrepo = vrepo;
      _krepo = krepo;
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
      Keep update = _krepo.GetById(data.KeepId);
      if (found != null)
      {
        // if (found.IsPrivate == false)
        // {
        if (found.CreatorId == data.CreatorId)
        {
          update.Kept++;
          _krepo.UpdateKept(update);
          return _repo.Create(data);
        }
        //   throw new Exception("Not your Vault");
        // }
        throw new Exception("Invalid Vault");
      }
      update.Kept++;
      _krepo.UpdateKept(update);
      return _repo.Create(data);
    }

    internal void Delete(int id, string userId)
    {
      VaultKeep found = GetById(id);
      Keep update = _krepo.GetById(found.KeepId);
      if (found.CreatorId != userId)
      {
        throw new Exception("You cannot delete this");
      }
      update.Kept -= 1;
      _krepo.UpdateKept(update);
      _repo.Delete(id);
    }
  }
}