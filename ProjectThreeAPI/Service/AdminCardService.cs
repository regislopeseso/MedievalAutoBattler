using Microsoft.EntityFrameworkCore;
using ProjectThreeAPI.Models.Dtos.Request;
using ProjectThreeAPI.Models.Dtos.Response;
using ProjectThreeAPI.Models.Entities;
using System.Numerics;

namespace ProjectThreeAPI.Service
{
    public class AdminCardService
    {
        private readonly ApplicationDbContext _daoDBcontext;

        public AdminCardService(ApplicationDbContext daoDBcontext)
        {
            this._daoDBcontext = daoDBcontext;
        }

        public async Task<(CardCreateAdminResponse?, string)> Create(CardCreateAdminRequest card)
        {
            if (string.IsNullOrEmpty(card.Name) == true)
            {
                return (null, "Erro: nome da carta não pode ser vazio");
            }

            var exists = await this._daoDBcontext
                .Cards
                .Where(a => a.Name == card.Name && a.IsDeleted == false)
                .AnyAsync();
            if (exists == true)
            {
                return (null, $"Erro: a carta já existe - {card.Name}");
            }

            var newCard = new Card
            {
                Name = card.Name,
                Power = card.Power,
                UpperHand = card.UpperHand,
                Type = card.Type,
                IsDeleted = false,
            };

            _daoDBcontext.Add(newCard);
            
            await _daoDBcontext.SaveChangesAsync();

            var response = new CardCreateAdminResponse
            {
                Id = newCard.Id,
                Name = card.Name,
                Power = card.Power,
                UpperHand = card.UpperHand,
                Type = card.Type,
                //IsDeleted = false,
            };

            return (response, "Carta criada com sucesso!");
        }
    }
}
