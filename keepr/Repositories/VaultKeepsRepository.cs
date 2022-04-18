using System.Data;
using Dapper;
using keepr.Models;

namespace keepr.Repositories
{
  public class VaultKeepsRepository
  {
    private readonly IDbConnection _db;

    public VaultKeepsRepository(IDbConnection db)
    {
      _db = db;
    }
    internal VaultKeep GetById(int id)
    {
      string sql = "SELECT * FROM vaultKeeps WHERE id = @id;";
      return _db.QueryFirstOrDefault<VaultKeep>(sql, new { id });
    }

    internal VaultKeep Create(VaultKeep data)
    {
      string sql = @"
      INSERT INTO vaultKeeps
      (vaultId, keepId, creatorId)
      VALUES
      (@VaultId, @KeepId, @CreatorId);
      SELECT LAST_INSERT_ID();
      ";
      int id = _db.ExecuteScalar<int>(sql, data);
      data.Id = id;
      return data;
    }

    internal void Delete(int id)
    {
      string sql = "DELETE FROM vaultKeeps WHERE id = @id LIMIT 1;";
      _db.Execute(sql, new { id });
    }
  }
}