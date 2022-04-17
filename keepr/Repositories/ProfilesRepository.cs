using System.Data;
using System.Linq;
using Dapper;
using keepr.Models;

namespace keepr.Repositories
{
  public class ProfilesRepository
  {
    private readonly IDbConnection _db;

    public ProfilesRepository(IDbConnection db)
    {
      _db = db;
    }

    internal Profile GetById(string id)
    {
      string sql = @"
      SELECT
        *
        FROM accounts a
        WHERE a.id = @id
      ";
      return _db.Query<Profile, Profile, Profile>(sql, (profile, user) =>
      {
        profile.Id = user.Id;
        return profile;
      }).FirstOrDefault();
    }
  }
}