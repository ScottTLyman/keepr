using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using keepr.Models;

namespace keepr.Repositories
{
  public class VaultsRepository
  {
    private readonly IDbConnection _db;

    public VaultsRepository(IDbConnection db)
    {
      _db = db;
    }

    internal Vault Create(Vault data)
    {
      string sql = @"
      INSERT INTO vaults
      (name, description, img, isPrivate, creatorId)
      VALUES
      (@Name, @Description, @Img, @IsPrivate, @CreatorId);
      SELECT LAST_INSERT_ID();
      ";
      int id = _db.ExecuteScalar<int>(sql, data);
      data.Id = id;
      return data;
    }

    internal List<Vault> GetAll()
    {
      string sql = @"
      SELECT
        v.*,
        a.*
        FROM vaults v
        JOIN accounts a ON v.creatorId = a.id;
      ";
      return _db.Query<Vault, Profile, Vault>(sql, (vault, prof) =>
      {
        vault.Creator = prof;
        return vault;
      }).ToList();
    }


    internal Vault GetById(int id)
    {
      string sql = @"
        SELECT
        v.*,
        a.*
        FROM vaults v
        JOIN accounts a ON v.creatorId = a.id
        WHERE v.id = @id;
      ";
      return _db.Query<Vault, Profile, Vault>(sql, (vault, prof) =>
      {
        vault.Creator = prof;
        return vault;
      }, new { id }).FirstOrDefault();
    }
    internal List<Vault> GetProfileVaults(string id)
    {
      string sql = @"
      SELECT
        v.*,
        a.*
        FROM vaults v
        JOIN accounts a
        ON v.creatorId = a.id
        WHERE v.creatorId = @id
        AND v.isPrivate = 0;
      ";
      return _db.Query<Vault, Profile, Vault>(sql, (v, prof) =>
      {
        v.Creator = prof;
        return v;
      }, new { id }).ToList();
    }
    internal List<Vault> GetMyVaults(string id)
    {
      string sql = @"
      SELECT
        v.*,
        a.*
        FROM vaults v
        JOIN accounts a
        ON v.creatorId = a.id
        WHERE v.creatorId = @id;
      ";
      return _db.Query<Vault, Profile, Vault>(sql, (v, prof) =>
      {
        v.Creator = prof;
        return v;
      }, new { id }).ToList();
    }

    internal Vault Update(Vault original)
    {
      string sql = @"
      UPDATE vaults
      SET
        name = @Name,
        description = @Description,
        img = @Img,
        isPrivate = @IsPrivate
        WHERE id = @id;
      ";
      int rows = _db.Execute(sql, original);
      if (rows > 0)
      {
        return original;
      }
      throw new Exception("SQL error on update vaults, now rows affected");
    }
    internal void Remove(int id)
    {
      string sql = @"
      DELETE FROM vaults
      WHERE id = @id LIMIT 1;
      ";
      _db.Execute(sql, new { id });
    }
  }
}