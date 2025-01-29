using Microsoft.EntityFrameworkCore;
using ProjectThreeAPI.Models.Dtos.Request;
using ProjectThreeAPI.Models.Dtos.Response;
using ProjectThreeAPI.Models.Entities;
using ProjectThreeAPI.Utilities;

namespace ProjectThreeAPI.Service
{
    public class AdminCardService
    {
        private readonly ApplicationDbContext _daoDBcontext;

        public AdminCardService(ApplicationDbContext daoDBcontext)
        {
            this._daoDBcontext = daoDBcontext;
        }

        public async Task<(CardCreateAdminResponse, string)> Create(CardCreateAdminRequest card)
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
                Level = Helper.GetCardLevel(card),
                Type = card.Type,
                IsDeleted = false,
            };

            _daoDBcontext.Add(newCard);

            await _daoDBcontext.SaveChangesAsync();

            var response = new CardCreateAdminResponse
            {
                Id = newCard.Id,
                Name = newCard.Name,
                Power = newCard.Power,
                UpperHand = newCard.UpperHand,
                Type = newCard.Type,
            };

            return (response, "Carta criada com sucesso!");
        }

        public async Task<List<CardReadAdminResponse>> Read()
        {
            return await this._daoDBcontext
                .Cards
                .Where(a => a.IsDeleted == false)
                .Select(a => new CardReadAdminResponse
                {
                    Id = a.Id,
                    Name = a.Name,
                    Power = a.Power,
                    UpperHand = a.UpperHand,
                    Level = a.Level,
                    Type = a.Type,
                })
                .ToListAsync();
        }

        public async Task<(CardUpdateAdminResponse, string)> Update(CardUpdateAdminRequest card)
        {
            if (id == null || id <= 0)
            {
                return (null, "Id inexistente");
            }
            var card = await _daoDBcontext
                .Cards
                .AsNoTracking()
                .Where(a => a.Id == id && a.IsDeleted == false)
                .Select(b => new CardUpdateAdminResponse
                {
                    Id = b.Id,
                    Name = b.Name,
                    Power = b.Power,
                    UpperHand = b.UpperHand,
                })
                .FirstOrDefaultAsync();

            if (card == null)
            {
                return (null, "Carta inexistente");
            }

            return (card, string.);
        }


        private (bool, string) EditIsValid(CardUpdateAdminRequest card)
        {
            if(card == null)
            {
                return (false, "Nenhuma informação enviada");
            }

            if(string)
        }
    }
}
