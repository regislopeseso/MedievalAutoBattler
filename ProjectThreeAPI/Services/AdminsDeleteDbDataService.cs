using MedievalAutoBattler.Models.Dtos.Request.Admin;
using MedievalAutoBattler.Models.Dtos.Response.Admin;
using Microsoft.EntityFrameworkCore;

namespace MedievalAutoBattler.Service
{
    public class AdminsDeleteDbDataService
    {
        private readonly ApplicationDbContext _daoDbContext;

        public AdminsDeleteDbDataService(ApplicationDbContext daoDbContext)
        {
            _daoDbContext = daoDbContext;
        }

        public async Task<(AdminsDeleteDbDataResponse?, string)> DeleteDbData(AdminsDeleteDbDataRequest response)
        {
            await using var transaction = await _daoDbContext.Database.BeginTransactionAsync();

            var message = "DB data deletion successful";

            try
            {
                await this._daoDbContext.Cards.ExecuteUpdateAsync(a => a.SetProperty(b => b.IsDeleted, true));
                
                await this._daoDbContext.NpcsDeckEntries.ExecuteUpdateAsync(a => a.SetProperty(b => b.IsDeleted, true));
                
                await this._daoDbContext.PlayersCardEntries.ExecuteUpdateAsync(a => a.SetProperty(b => b.IsDeleted, true));

                await this._daoDbContext.PlayersDeckEntries.ExecuteUpdateAsync(a => a.SetProperty(b => b.IsDeleted, true));

                await this._daoDbContext.Decks.ExecuteUpdateAsync(a => a.SetProperty(b => b.IsDeleted, true));

                await this._daoDbContext.Npcs.ExecuteUpdateAsync(a => a.SetProperty(b => b.IsDeleted, true));

                await this._daoDbContext.PlayersSaves.ExecuteUpdateAsync(a => a.SetProperty(b => b.IsDeleted, true));              

                await transaction.CommitAsync(); // ✅ Commit changes if everything is successful

                return (null, "DB data deletion successful");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); // ❌ Rollback in case of any failure

                return (null, $"DB data deletion failed: {ex.Message}");
            }
        }
        
    }
}
