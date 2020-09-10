using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using VaporStore.Data.Models;
using VaporStore.Data.Models.Enums;
using VaporStore.DataProcessor.Dto;
using VaporStore.DataProcessor.Dto.Import;

namespace VaporStore.DataProcessor
{
	using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Data;

	public static class Deserializer
	{
        private const string ErrorMessage = "Invalid Data";
        private const string SuccessfullyAddedGame = "Added {0} ({1}) with {3} tags";

        public static string ImportGames(VaporStoreDbContext context, string jsonString)
		{
           
            var gamesDTOs = JsonConvert.DeserializeObject<ImportGamesDTO[]>(jsonString);
            var games = new List<Game>();
            var developers = new List<Developer>();
            var genres = new List<Genre>();
            var tags = new List<Tag>();
            var sb = new StringBuilder();

            foreach (var dto in gamesDTOs)
            {
                Developer dev;
                Genre genre;
                int tagCounter = 0;

                if (!IsValid(dto) || dto.Tags.Length == 0)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                dev = developers.FirstOrDefault(d => d.Name == dto.Developer);
                genre = genres.FirstOrDefault(g => g.Name == dto.Genre);

                var game = new Game()
                {
                    Name = dto.Name,
                    Price = dto.Price,
                    ReleaseDate = DateTime.ParseExact(dto.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Developer = dev == null ? new Developer() { Name = dto.Developer } : dev,
                    Genre = genre == null ? new Genre() { Name = dto.Genre } : genre
                };

                foreach (var tag in dto.Tags)
                {
                    GameTag searchTag;
                    if (tags.FirstOrDefault(t => t.Name == tag) == null)
                    {
                        var tagtoAdd = new Tag() { Name = tag };
                        game.GameTags.Add(new GameTag { Game = game, Tag = tagtoAdd });
                        tags.Add(tagtoAdd);
                    }
                    else
                    {
                        var foundTag = tags.FirstOrDefault(t => t.Name == tag);
                        game.GameTags.Add(new GameTag { Game = game, Tag = foundTag });
                    }
                    tagCounter++;
                }

                games.Add(game);
                if (dev == null)
                {
                    developers.Add(game.Developer);
                }
                if (genre == null)
                {
                    genres.Add(game.Genre);
                }

                sb.AppendLine(string.Format($"Added {game.Name} ({game.Genre.Name}) with {tagCounter} tags"));
            }

            context.Tags.AddRange(tags);
            context.Games.AddRange(games);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var userDTO = JsonConvert.DeserializeObject<ImportUsersDTO[]>(jsonString);

            var validUsers = new List<User>();

            foreach (var usersDto in userDTO)
            {
                if (!IsValid(usersDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var user = new User()
                {
                    FullName = usersDto.FullName,
                    Username = usersDto.Username,
                    Email = usersDto.Email,
                    Age = usersDto.Age
                };


                foreach (var cardsDto in usersDto.Cards)
                {
                    if (!IsValid(cardsDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue; ;
                    }

                    CardType validEnum;
                    var enumtype = Enum.TryParse(cardsDto.Type, out validEnum);

                    user.Cards.Add(new Card()
                    {
                        Number = cardsDto.Number,
                        Cvc = cardsDto.CVC,
                        Type = validEnum
                    });
                }
                validUsers.Add(user);
                sb.AppendLine($"Imported {usersDto.Username} with {user.Cards.Count} cards");
            }
            context.Users.AddRange(validUsers);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
		{
            var serializer = new XmlSerializer(typeof(ImportPurchaseDTO[]), new XmlRootAttribute("Purchases"));

            StringBuilder sb = new StringBuilder();


            using (StringReader sr = new StringReader(xmlString))
            {
                var purchasesDtos = (ImportPurchaseDTO[])serializer.Deserialize(sr);

                List<Purchase> purchases = new List<Purchase>();

                foreach (var purchaseDto in purchasesDtos)
                {
                    if (!IsValid(purchaseDto))
                    {
                        sb.AppendLine("Invalid Data");
                        continue;
                    }

                    var card = context.Cards.FirstOrDefault(x => x.Number == purchaseDto.Card);

                    var game = context.Games.FirstOrDefault(x => x.Name == purchaseDto.title);

                    Purchase purchase = new Purchase
                    {
                        Type = Enum.Parse<PurchaseType>(purchaseDto.Type),
                        ProductKey = purchaseDto.Key,
                        Card = card,
                        Date = DateTime.ParseExact(purchaseDto.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None),
                        Game = game
                    };

                    var username = context.Users.FirstOrDefault(x => x.Id == purchase.Card.UserId);

                    purchases.Add(purchase);

                    sb.AppendLine($"Imported {purchase.Game.Name} for {username.Username}");
                }

                context.Purchases.AddRange(purchases);
                context.SaveChanges();
            }

            return sb.ToString().TrimEnd();
        }

		private static bool IsValid(object dto)
		{
			var validationContext = new ValidationContext(dto);
			var validationResult = new List<ValidationResult>();

			return Validator.TryValidateObject(dto, validationContext, validationResult, true);
		}
	}
}