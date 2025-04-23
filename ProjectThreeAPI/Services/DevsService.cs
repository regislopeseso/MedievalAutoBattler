using MedievalAutoBattler.Models.Dtos.Request.Devs;
using MedievalAutoBattler.Models.Dtos.Response.Devs;
using MedievalAutoBattler.Models.Entities;
using MedievalAutoBattler.Models.Enums;
using MedievalAutoBattler.Utilities;
using Microsoft.EntityFrameworkCore;

namespace MedievalAutoBattler.Services
{
    public class DevsService
    {
        private readonly ApplicationDbContext _daoDbContext;

        public DevsService(ApplicationDbContext daoDbContext)
        {
            _daoDbContext = daoDbContext;
        }

        public async Task<(DevsSeedResponse?, string)> Seed(DevsSeedRequest? request)
        {
            var (cardsSeedingResult, msg1) = await this.SeedCards();
            var (npcsSeedingResult, msg2) = await this.SeedNpcs();

            return (null, "Seeding was successful. " + msg1 +". "+ msg2);
        }

        private async Task<(DevsSeedCardsResponse?, string)> SeedCards()
        {
            var cardsSeed = new List<Card>();

            foreach (var cardType in new[] { CardType.Archer, CardType.Cavalry, CardType.Spearman })
            {
                for (int power = Constants.MinCardPower; power <= Constants.MaxCardPower; power++)
                {
                    for (int upperHand = Constants.MinCardUpperHand; upperHand <= Constants.MaxCardUpperHand; upperHand++)
                    {
                        var newCard = new Card
                        {
                            Name = cardType.ToString() + " *" + power + "|" + upperHand + "*",
                            Power = power,
                            UpperHand = upperHand,
                            Level = Helper.GetCardLevel(power, upperHand),
                            Type = cardType,
                            IsDeleted = false,
                            IsDummy = true
                        };
                        cardsSeed.Add(newCard);
                    }
                }
            }

            this._daoDbContext.AddRange(cardsSeed);

            await this._daoDbContext.SaveChangesAsync();

            return (null, $"{cardsSeed.Count} new cards haven been successfully seeded");
        }

        private async Task<(DevsSeedNpcsResponse?, string)> SeedNpcs()
        {
            var cardsDB = await _daoDbContext
                                    .Cards
                                    .Where(a => a.IsDeleted == false)
                                    .ToListAsync();

            if (cardsDB == null || cardsDB.Count == 0)
            {
                return (null, "Error: cards not found");
            }

            //Se não existir pelo menos uma carta de cada level não é possível fazer o seed dos NPCs.
            var countCardsLvl = cardsDB.GroupBy(a => a.Level).Count();
            if (countCardsLvl < Constants.MaxCardLevel - Constants.MinCardLevel)
            {
                return (null, "Error: not enough card variety for seeding NPCs. The existance of at least one card of each level is mandatory for seeding NPCs");
            }

            var npcsSeed = new List<Npc>();

            for (int level = Constants.MinCardLevel; level <= Constants.MaxCardLevel; level++)
            {
                npcsSeed.AddRange(GenerateRandomNpcs(level, cardsDB));
            }

            if (npcsSeed == null || npcsSeed.Count == 0)
            {
                return (null, "Error: seeding NPCs failed");
            }

            _daoDbContext.AddRange(npcsSeed);

            await _daoDbContext.SaveChangesAsync();

            return (null, "NPCs have been successfully seeded");
        }

        private static List<Npc> GenerateRandomNpcs(int level, List<Card> cardsDB)
        {
            var random = new Random();

            if (level == Constants.MinCardLevel || level == Constants.MaxCardLevel)
            {
                var countBotsLvlZero = 0;
                var countBotsLvlNine = 0;

                // Creating a list that will contain 10 NPCs of cardLvl 0 or cardLvl 9:
                var npcs = new List<Npc>();

                while (npcs.Count < 10)
                {
                    // Filtering all cards having cardLvl iguals to 0 or 9 (currently contains 4 cards npcLvl 0 or 10 npcLvl 9):
                    var cardsFiltered = cardsDB.Where(a => a.Level == level).ToList();

                    // Creating a new list of NpcDeckentries
                    var validNpcDeckEntries = new List<NpcDeckEntry>();
                    for (int countCards = 0; countCards < 5; countCards++)
                    {
                        // Obtaining one random request out of the list of filtered cards
                        var card = cardsFiltered.OrderBy(a => random.Next()).FirstOrDefault();

                        // "Converting" the random request into a new NPC DECK ENTRY and adding it to a new list of valid deck entries:
                        validNpcDeckEntries.Add
                        (
                            new NpcDeckEntry
                            {
                                Card = card
                            }
                        );
                    }

                    // Creating a new npc with 5 cards (npcLvl 0 or npcLvl 9) and adding it to a list of new NPCs:          
                    var npcName = "";
                    var npcDescription = "";
                    var npcLvl = Helper.GetNpcLevel(validNpcDeckEntries.Select(a => a.Card.Level).ToList());
                    if (level == 0)
                    {
                        countBotsLvlZero++;
                        npcName = "NPC-LVL" + npcLvl + "-" + countBotsLvlZero;
                        npcDescription = "(0, 0, 0, 0, 0)";
                    }
                    if (level == 9)
                    {
                        countBotsLvlNine++;
                        npcName = "NPC-LVL" + npcLvl + "-" + countBotsLvlNine;
                        npcDescription = "(9, 9, 9, 9, 9)";
                    }
                    npcs.Add(new Npc
                    {
                        Name = npcName,
                        Description = npcDescription,
                        Deck = validNpcDeckEntries,
                        Level = npcLvl,
                        IsDeleted = false,
                        IsDummy = true
                    });

                    //This while loop will stop when the list of NPCs has 10 NPCs
                }

                return npcs;
            }
            else if (level == 1)
            {
                var npcs = new List<Npc>();
                var countBotsLvlOne = 0;
                var count = 1;

                while (count <= 2)
                {
                    // Obtaining the lists of all unique sequences
                    // (for cardLvl 1 there are 5 unique possible combinations):
                    for (int i = 8; i <= 12; i++)
                    {
                        // Obtaining the list of all 5 unique sequence for cardLvl 1)
                        var levelSequence = Helper.GetPowerSequence(level, i);

                        // Criating a new list of valid NPC deck entries:
                        var validNpcDeckEntries = new List<NpcDeckEntry>();

                        // Obtaing a random request of cardLvl corresponding to its position in the sequence:
                        foreach (var cardLvl in levelSequence)
                        {
                            // Filtering all cards whose cardLvl is 1:
                            var cardsFiltered = cardsDB.Where(a => a.Level == cardLvl).ToList();

                            // Obtaining one random request out of the list of filtered cards
                            var card = cardsFiltered.OrderBy(a => random.Next()).Take(1).FirstOrDefault();

                            // "Converting" the random request into a new NPC DECK ENTRY and adding it to a new list of valid deck entries:                               
                            validNpcDeckEntries.Add
                            (
                                new NpcDeckEntry
                                {
                                    Card = card
                                }
                            );
                        }

                        // Creating a new npc with 5 cards npcLvl 1 and adding it to a list of new NPCs:
                        countBotsLvlOne++;
                        var npcLvl = Helper.GetNpcLevel(validNpcDeckEntries.Select(a => a.Card.Level).ToList());
                        var npcName = "NPC-LVL" + npcLvl + "-" + countBotsLvlOne;
                        npcs.Add(new Npc
                        {
                            Name = npcName,
                            Description = "( " + string.Join(", ", levelSequence) + " )",
                            Deck = validNpcDeckEntries,
                            Level = npcLvl,
                            IsDeleted = false,
                            IsDummy = true
                        });

                    }
                    //This while loop will stop when the count is 2.                       
                    count++;
                }

                return npcs;
            }
            else if (level == 8)
            {
                var npcs = new List<Npc>();
                var countBotsLvlEight = 0;
                var count = 1;


                while (count <= 4)
                {
                    // Obtaining the lists of all unique sequences
                    // (for cardLvl 8 there are 3 unique possible combinations):
                    for (int i = 5; i <= 12; i++)
                    {
                        if (i == 5 || i == 7 || i == 12)
                        {
                            // Obtaining the list of all 3 unique sequence for cardLvl 8)
                            var levelSequence = Helper.GetPowerSequence(level, i);

                            // Criating a new list of valid NPC deck entries:
                            var validNpcDeckEntries = new List<NpcDeckEntry>();

                            // Obtaing a random cards of cardLvl corresponding to its position in the sequence:
                            foreach (var cardLvl in levelSequence)
                            {
                                // Filtering all cards whose cardLvl is 8:
                                var cardsFiltered = cardsDB.Where(a => a.Level == cardLvl).ToList();

                                // Obtaining one random request out of the list of filtered cards
                                var card = cardsFiltered.OrderBy(a => random.Next()).FirstOrDefault();

                                // "Converting" the random request into a new NPC DECK ENTRY and adding it to a new list of valid deck entries:                               
                                validNpcDeckEntries.Add
                                (
                                    new NpcDeckEntry
                                    {
                                        Card = card
                                    }
                                );
                            }

                            // Creating a new npc with 5 cards npcLvl 1 and adding it to a list of new NPCs:                        
                            countBotsLvlEight++;
                            var npcLvl = Helper.GetNpcLevel(validNpcDeckEntries.Select(a => a.Card.Level).ToList());
                            var npcName = "NPC-LVL" + npcLvl + "-" + countBotsLvlEight;
                            npcs.Add(new Npc
                            {
                                Name = npcName,
                                Description = "( " + string.Join(", ", levelSequence) + " )",
                                Deck = validNpcDeckEntries,
                                Level = npcLvl,
                                IsDeleted = false,
                                IsDummy = true
                            });
                        }
                    }

                    count++;
                    //This while loop will stop when count is 4, since there 3 possible sequences looping 4 times will result in 12 npcs. 
                }

                //Returns only the first 10 npcs
                return npcs.Take(10).ToList();
            }
            else //(cardLvl != 0 && cardLvl != 1 && cardLvl != 8 && cardLvl != 9)
            {
                var countBotsLvlTwo = 0;
                var countBotsLvlThree = 0;
                var countBotsLvlFour = 0;
                var countBotsLvlFive = 0;
                var countBotsLvlSix = 0;
                var countBotsLvlSeven = 0;

                var validDecks = new List<List<NpcDeckEntry>>();
                var npcs = new List<Npc>();

                // Obtaining the lists of all unique sequences
                // (for cardLvl 2 up to 7 there are 12 unique possible combinations):
                for (int i = 1; i <= 12; i++)
                {
                    // Criating a new list of valid NPC deck entries:
                    var validNpcDeckEntries = new List<NpcDeckEntry>();

                    //ex.: cardLvl == 6 and  i == 1 => (4, 4, 6, 8, 8 )
                    var levelSequence = Helper.GetPowerSequence(level, i);

                    // Obtaing a random request of cardLvl corresponding to its position in the sequence:
                    foreach (var cardLvl in levelSequence)
                    {
                        // Filtering all cards by cardLvl (2, 3, 4, 5, 6 or 7):
                        var cardsFiltered = cardsDB.Where(a => a.Level == cardLvl).ToList();

                        // Obtaing a random request of cardLvl corresponding to its position in the sequence:
                        var card = cardsFiltered.OrderBy(a => random.Next()).FirstOrDefault();

                        // "Converting" the random request into a new NPC DECK ENTRY and adding it to a new list of valid deck entries:
                        validNpcDeckEntries.Add
                        (
                            new NpcDeckEntry
                            {
                                Card = card
                            }
                        );
                    }

                    // Creating a new npc with 5 cards and adding it to a list of new NPCs:   
                    var npcName = "";
                    var npcLvl = Helper.GetNpcLevel(validNpcDeckEntries.Select(a => a.Card.Level).ToList());

                    switch (level)
                    {
                        case 2:
                            countBotsLvlTwo++;
                            npcName = "NPC-LVL" + npcLvl + "-" + countBotsLvlTwo;
                            break;
                        case 3:
                            countBotsLvlThree++;
                            npcName = "NPC-LVL" + npcLvl + "-" + countBotsLvlThree;
                            break;
                        case 4:
                            countBotsLvlFour++;
                            npcName = "NPC-LVL" + npcLvl + "-" + countBotsLvlFour;
                            break;
                        case 5:
                            countBotsLvlFive++;
                            npcName = "NPC-LVL" + npcLvl + "-" + countBotsLvlFive;
                            break;
                        case 6:
                            countBotsLvlSix++;
                            npcName = "NPC-LVL" + npcLvl + "-" + countBotsLvlSix;
                            break;
                        case 7:
                            countBotsLvlSeven++;
                            npcName = "NPC-LVL" + npcLvl + "-" + countBotsLvlSeven;
                            break;
                    }

                    npcs.Add(new Npc
                    {
                        Name = npcName,
                        Description = "( " + string.Join(", ", levelSequence) + " )",
                        Deck = validNpcDeckEntries,
                        Level = npcLvl,
                        IsDeleted = false,
                        IsDummy = true
                    });

                    // By the end of this for loop, there will be 12 npcs                  
                }

                // Returns 10 random npcs out of the list of 12: 
                return npcs.OrderBy(a => random.Next()).Take(10).ToList(); ;
            }
        }

        public async Task<(DevsDeleteSeedResponse?, string)> DeleteSeed(DevsDeleteSeedRequest request)
        {
            await this._daoDbContext
                            .Npcs
                            .Where(a => a.IsDummy == true)
                            .ExecuteDeleteAsync();

            await this._daoDbContext
                .Cards
                .Where(a => a.IsDummy == true)
                .ExecuteDeleteAsync();

            return (null, "Dummies deleted successfully");
        }
    }
}
