using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using keepr.Models;

namespace keepr.Repositories
{
  public class KeepsRepository
  {
    private readonly IDbConnection _db;

    public KeepsRepository(IDbConnection db)
    {
      _db = db;
    }

    public Keep Create(Keep data)
    {
      string sql = @"
      INSERT INTO keeps
      (name, description, img, views, kept, creatorId)
      VALUES
      (@Name, @Description, @Img, @Views, @Kept, @CreatorId);
      SELECT LAST_INSERT_ID();
      ";
      int id = _db.ExecuteScalar<int>(sql, data);
      data.Id = id;
      return data;

    }

    public List<Keep> GetAll()
    {
      string sql = @"
      SELECT
        k.*,
        a.*
        FROM keeps k
        JOIN accounts a ON k.creatorId = a.id;
      ";
      return _db.Query<Keep, Profile, Keep>(sql, (keep, prof) =>
      {
        keep.Creator = prof;
        return keep;
      }).ToList();
    }


    public Keep GetById(int id)
    {
      string sql = @"
      SELECT
        k.*,
        a.*
        FROM keeps k
        JOIN accounts a ON k.creatorId = a.id
        WHERE k.id = @id;
      ";
      return _db.Query<Keep, Profile, Keep>(sql, (keep, prof) =>
      {
        keep.Creator = prof;
        return keep;
      }, new { id }).FirstOrDefault();
    }

    internal List<KeepViewModel> GetVaultKeeps(int vaultId)
    {
      string sql = @"
        SELECT
          vk.*,
          k.*,
          a.*
          FROM vaultKeeps vk
          JOIN keeps k ON vk.keepId = k.id
          JOIN accounts a ON k.creatorId = a.id
          WHERE vk.vaultId = @vaultId;
        ";
      return _db.Query<VaultKeep, KeepViewModel, Profile, KeepViewModel>(sql, (vk, k, prof) =>
      {
        k.VaultKeepId = vk.Id;
        k.Creator = prof;
        return k;
      }, new { vaultId }).ToList();
    }

    internal List<Keep> GetProfileKeeps(string id)
    {
      string sql = @"
      SELECT
        k.*,
        a.*
        FROM keeps k
        JOIN accounts a
        ON k.creatorId = a.id
        WHERE k.creatorId = @id;
      ";
      return _db.Query<Keep, Profile, Keep>(sql, (k, prof) =>
      {
        k.Creator = prof;
        return k;
      }, new { id }).ToList();
    }


    public Keep Update(Keep original)
    {
      string sql = @"
      UPDATE keeps
      SET
        name = @Name,
        description = @Description,
        img = @Img
        WHERE id = @id;
      ";
      int rows = _db.Execute(sql, original);
      if (rows > 0)
      {
        return original;
      }
      throw new Exception("SQL error on update keeps, no rows affected");
    }
    public void Remove(int id)
    {
      string sql = "DELETE FROM keeps WHERE id = @id LIMIT 1;";
      _db.Execute(sql, new { id });

    }
  }
}