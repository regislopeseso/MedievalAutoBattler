using MedievalAutoBattler.Models.Dtos.Request.Admin;
using MedievalAutoBattler.Models.Dtos.Response.Admin;
using MedievalAutoBattler.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedievalAutoBattler.Service.Admin
{
    public class AdminDeleteService
    {
        private readonly ApplicationDbContext _daoDbcontext;

        public AdminDeleteService(ApplicationDbContext daoDbcontext)
        {
            this._daoDbcontext = daoDbcontext;
        }

        public async Task<(AdminsDeleteDbDataResponse?, string)> Delete(AdminsDeleteDbDataRequest response)
        {
            //await this._daoDbcontext
            //          .Cards
            //          .ExecuteUpdateAsync(a => a.SetProperty(b => b.IsDeleted, true));

            //await this._daoDbcontext
            //          .Npcs
            //          .ExecuteUpdateAsync(a => a.SetProperty(b => b.IsDeleted, true));

            //await this._daoDbcontext
            //          .Saves
            //          .ExecuteUpdateAsync(a => a.SetProperty(b => b.IsDeleted, true));

            //await this._daoDbcontext
            //          .Decks
            //          .ExecuteUpdateAsync(a => a.SetProperty(b => b.IsDeleted, true));

            //await this._daoDbcontext
            //          .Battles
            //          .Where(a => a.IsFinished == false)
            //          .ExecuteDeleteAsync();

            await using var transaction = await this._daoDbcontext.Database.BeginTransactionAsync();
            var message = "DB data deletion successful";

            //try
            //{
            //    await this._daoDbcontext.Cards.ExecuteUpdateAsync(a => a.SetProperty(b => b.IsDeleted, true));
            //    await this._daoDbcontext.Npcs.ExecuteUpdateAsync(a => a.SetProperty(b => b.IsDeleted, true));
            //    await this._daoDbcontext.Saves.ExecuteUpdateAsync(a => a.SetProperty(b => b.IsDeleted, true));
            //    await this._daoDbcontext.Decks.ExecuteUpdateAsync(a => a.SetProperty(b => b.IsDeleted, true));

            //    await transaction.CommitAsync(); // ✅ Commit changes if everything is successful
            //    return (null, "DB data deletion successful");
            //}
            //catch (Exception ex)
            //{
            //    await transaction.RollbackAsync(); // ❌ Rollback in case of any failure
            //    return (null, $"DB data deletion failed: {ex.Message}");
            //}

            try
            {
                try
                {
                    await this._daoDbcontext.Cards.ExecuteUpdateAsync(a => a.SetProperty(b => b.IsDeleted, true));
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to delete Cards: {ex.Message}", ex);
                }

                try
                {
                    await this._daoDbcontext.Npcs.ExecuteUpdateAsync(a => a.SetProperty(b => b.IsDeleted, true));
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to delete NPCs: {ex.Message}", ex);
                }

                try
                {
                    await this._daoDbcontext.Saves.ExecuteUpdateAsync(a => a.SetProperty(b => b.IsDeleted, true));
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to delete Saves: {ex.Message}", ex);
                }

                try
                {
                    await this._daoDbcontext.Decks.ExecuteUpdateAsync(a => a.SetProperty(b => b.IsDeleted, true));
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to delete Decks: {ex.Message}", ex);
                }

                try
                {
                    await this._daoDbcontext
                              .Battles
                              .Where(a => a.IsFinished == false)
                              .ExecuteDeleteAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to hard delete Battles: {ex.Message}", ex);
                }


                await transaction.CommitAsync(); // ✅ Commit if everything is successful
                return (null, message);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); // ❌ Rollback in case of failure
                return (null, $"DB data deletion failed: {ex.Message}");
            }
        }
    }
}
