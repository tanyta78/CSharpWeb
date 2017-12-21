namespace SocialNetwork
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Models.Logic;

    public class Program
    {
        private static Random random = new Random();

        public static void Main(string[] args)
        {
            using (var db = new SocialNetworkDbContext())
            {
                db.Database.Migrate();
                Console.WriteLine("Database migrated successfully!");
                // SeedUsers(db);
               // SeedAlbumsAndPictures(db);
               // SeedTags(db);
                Console.WriteLine("Database seeded successfully!");
                //ListAllUsersWithFriendsCount(db);
                //ListAllUsersWithMoreThan5Friends(db);
                //ListAlbumsWithTotalPictures(db);
                //ListPicturesWithMoreThanTwoAlbums(db);
                // var userId = int.Parse(Console.ReadLine());
                // ListAlbumsForUserIdWithInfo(db, userId);
               // ListAllAlbumsForTag(db);
                ListAllUsersWithAlbumWithMoreThan3Tags(db);
            }
        }

        private static void ListAllUsersWithAlbumWithMoreThan3Tags(SocialNetworkDbContext db)
        {
            var users = db.Users
                          .Where(u => u.Albums.Any(a => a.Tags.Count > 3))
                          .Select(u => new
                          {
                              Name = u.Username,
                              Albums= u.Albums
                                  .Where(a=>a.Tags.Count>3)
                                  .Select(a=>new
                                  {
                                      Title = a.Name,
                                      Tags = a.Tags.Select(t=>t.Tag.Name)
                                  })
                                  .ToList(),
                              TotalTags= u.Albums.Sum(a => a.Tags.Count())
                          })
                          .OrderByDescending(u=>u.Albums.Count())
                          .ThenByDescending(u=>u.TotalTags)
                          .ThenBy(u=>u.Name)
                          .ToList();
           
            var sb = new StringBuilder();
            foreach (var user in users)
            {
                sb.AppendLine($"{user.Name}");
                foreach (var album in user.Albums)
                {
                    sb.AppendLine($"===={album.Title}");
                    sb.AppendLine($"---Tags: {string.Join(", ",album.Tags)}");
                    sb.AppendLine(new string('-', 20));
                }
            }

            Console.WriteLine(sb.ToString());
        }

        private static void ListAllAlbumsForTag(SocialNetworkDbContext db)
        {
            var tag = "#tag1";

            var result = db.Albums
                .Where(a => a.Tags.Any(t => t.Tag.Name == tag))
                .Select(a =>new
                           {
                              Title= a.Name,
                              OwnerName= a.Owner.Username,
                              TotalTags = a.Tags.Count
                              
                           })
                .OrderByDescending(a=>a.TotalTags)
                .ThenBy(a=>a.Title)
                .ToList();

            Console.WriteLine(string.Join(Environment.NewLine,result.Select(a=>$"Title:{a.Title} Owner: {a.OwnerName}")));
        }
        
        private static void ListAlbumsForUserIdWithInfo(SocialNetworkDbContext db, int userId)
        {
            var user = db.Users.Find(userId);
            if (user==null)
            {
                throw new ArgumentException("User does not exist");
            }

            var result = db.Albums
                           .Where(a => a.OwnerId == userId)
                           .Select(a => new
                           {
                               Owner = a.Owner.Username,
                               a.IsPublic,
                               a.Name,
                               Pictures = a.Pictures.Select(p => new
                               {
                                   p.Picture.Title,
                                   p.Picture.Path
                               })
                           })
                           .OrderBy(a=>a.Name)
                           .ToList();

            var sb = new StringBuilder();

            foreach (var album in result)
            {
                sb.AppendLine(album.Name);
                if (album.IsPublic)
                {
                    foreach (var picture in album.Pictures)
                    {
                        sb.AppendLine($"---{picture.Title} -{picture.Path}");
                    }
                }
                else
                {
                    sb.AppendLine("Private content!");
                }
                sb.AppendLine(new string('=',20));
            }

            Console.WriteLine(sb.ToString());

        }

        private static void ListPicturesWithMoreThanTwoAlbums(SocialNetworkDbContext db)
        {
            var pictures = db.Pictures
                             .Where(p => p.Albums.Count > 2)
                             .Select(p => new
                             {
                                 Title = p.Title,
                                 NamesOfAlbums = p.Albums.Select(a => a.Album.Name).ToList(),
                                 NamesOfOwners = p.Albums.Select(a => a.Album.Owner.Username).ToList()
                             })
                             .OrderByDescending(e => e.NamesOfAlbums.Count)
                             .ThenBy(e => e.Title)
                             .ToList();

            var sb = new StringBuilder();
            foreach (var p in pictures)
            {
                sb.AppendLine($"Title: {p.Title}");
                for (int i = 0; i < p.NamesOfAlbums.Count; i++)
                {
                    sb.AppendLine($"==Album name: {p.NamesOfAlbums[i]}== Owner name: {p.NamesOfOwners[i]}");

                }
               
            }
            Console.WriteLine(sb.ToString());
        }

        private static void ListAlbumsWithTotalPictures(SocialNetworkDbContext db)
        {
            var albums = db.Albums
                           .Include(a => a.Owner)
                           .Select(a => new
                           {
                               Title = a.Name,
                               OwnerName = a.Owner.Username,
                               PicturesCount = a.Pictures.Count
                           })
                           .OrderByDescending(e=>e.PicturesCount)
                           .ThenBy(e=>e.OwnerName)
                           .ToList();
            Console.WriteLine(string.Join(Environment.NewLine, albums.Select(a => $"Title:{a.Title} Owner: {a.OwnerName} TotalPictures: {a.PicturesCount}")));
        }

      
        private static void ListAllUsersWithMoreThan5Friends(SocialNetworkDbContext db)
        {
            var users = db.Users
                          .Include(u => u.FromFriends)
                          .Include(u => u.ToFriends)
                          .Where(u => u.IsDeleted == false && (u.FromFriends.Count + u.ToFriends.Count) > 5)
                          .Select(u => new
                          {
                              Name = u.Username,
                              NumberOfFriends = u.FromFriends.Count + u.ToFriends.Count,
                              u.RegisteredOn,
                              Period = DateTime.Now.Subtract(u.RegisteredOn.Value)
                          })
                          .OrderBy(e => e.RegisteredOn)
                          .ThenByDescending(e => e.NumberOfFriends)
                          .ToList();

            Console.WriteLine(string.Join(Environment.NewLine, users.Select(a => $"Name:{a.Name} TotalFriends: {a.NumberOfFriends} Period: {a.Period}")));

        }

        private static void ListAllUsersWithFriendsCount(SocialNetworkDbContext db)
        {
            var users = db.Users
                .Include(u => u.FromFriends)
                .Include(u => u.ToFriends)
                .Select(u => new
                {
                    Name = u.Username,
                    NumberOfFriends = u.FromFriends.Count + u.ToFriends.Count,
                    Status = u.IsDeleted == false ? "Active" : "Inactive"
                })
                          .OrderByDescending(e => e.NumberOfFriends)
                          .ThenBy(e => e.Name)
                          .ToList();

            Console.WriteLine(string.Join(Environment.NewLine, users.Select(a => $"Name:{a.Name} TotalFriends: {a.NumberOfFriends} Status:{a.Status}")));

        }

        private static void SeedUsers(SocialNetworkDbContext db)
        {
            const int totalUsers = 50;

            var biggestUserId = db.Users
                .OrderByDescending(u => u.Id)
                .Select(u => u.Id)
                .FirstOrDefault() + 1;

            var allUsers = new List<User>();

            for (int i = biggestUserId; i < biggestUserId + totalUsers; i++)
            {
                var user = new User()
                {
                    Username = $"Username {i}",
                    Password = "Passw0rd#$",
                    Email = $"email@email{i}.com",
                    RegisteredOn = DateTime.Now.AddDays(-(100 + i * 10)),
                    LastTimeLoggedIn = DateTime.Now.AddDays(-i),
                    IsDeleted = false,
                    Age = i + 1
                };

                allUsers.Add(user);
            }

            db.Users.AddRange(allUsers);
            db.SaveChanges();
            Console.WriteLine("Users created successfully!");

            var userIds = allUsers.Select(u => u.Id).ToList();

            for (int i = 0; i < userIds.Count; i++)
            {
                var currentUserId = userIds[i];
                var totalFriends = random.Next(5, 11);

                for (int j = 0; j < totalFriends; j++)
                {
                    var friendId = userIds[random.Next(0, userIds.Count)];
                    var validFriendship = true;

                    //cannot be friend to myself
                    if (friendId == currentUserId)
                    {
                        validFriendship = false;
                    }

                    var friendshipExist = db
                        .Friendships
                        .Any(f =>
                        (f.FromUserId == currentUserId && f.ToUserId == friendId) ||
                        (f.FromUserId == friendId && f.ToUserId == currentUserId));

                    if (friendshipExist)
                    {
                        validFriendship = false;
                    }

                    if (!validFriendship)
                    {
                        j--;
                        continue;
                    }

                    db.Friendships.Add(new Friendship()
                    {
                        FromUserId = currentUserId,
                        ToUserId = friendId
                    });

                    db.SaveChanges();

                }
                Console.WriteLine("Frienships created successfully!");

            }
        }

        private static void SeedAlbumsAndPictures(SocialNetworkDbContext db)
        {
            const int totalAlbums = 100;
            const int totalPictures = 200;

            var biggestAlbumId = db.Albums
                                   .OrderByDescending(a => a.Id)
                                   .Select(a => a.Id)
                                   .FirstOrDefault() + 1;

            var userIds = db
                .Users
                .Select(u => u.Id)
                .ToList();

            var albums = new List<Album>();

            for (int i = biggestAlbumId; i < biggestAlbumId + totalAlbums; i++)
            {
                var album = new Album()
                {
                    Name = $"Album {i}",
                    BackgroundColor = $"Color {i}",
                    IsPublic = random.Next(0, 2) == 0 ? true : false,
                    OwnerId = userIds[random.Next(0, userIds.Count)]
                };

                db.Albums.Add(album);
                albums.Add(album);
            }

            db.SaveChanges();
            Console.WriteLine("Albums created successfully!");

            var biggestPictureId = db
                .Pictures
                .OrderByDescending(p => p.Id)
                .Select(p => p.Id)
                .FirstOrDefault() + 1;

            var pictures = new List<Picture>();

            for (int i = biggestPictureId; i < biggestPictureId + totalPictures; i++)
            {
                var picture = new Picture()
                {
                    Title = $"Picture {i}",
                    Caption = $"Caption {i}",
                    Path = $"Path {i}",
                };

                pictures.Add(picture);
                db.Pictures.Add(picture);
            }

            db.SaveChanges();
            Console.WriteLine("Pictures created successfully!");

            var albumsIds = albums.Select(a => a.Id).ToList();

            for (int i = 0; i < pictures.Count; i++)
            {
                var picture = pictures[i];
                var numberOfAlbums = random.Next(1, 20);

                for (int j = 0; j < numberOfAlbums; j++)
                {
                    var albumId = albumsIds[random.Next(0, albumsIds.Count)];

                    var pictureExistInAlbum = db
                        .Pictures
                        .Any(p => p.Id == picture.Id && p.Albums.Any(a => a.AlbumId == albumId));

                    if (pictureExistInAlbum)
                    {
                        j--;
                        continue;
                    }

                    picture.Albums.Add(new AlbumPicture()
                    {
                        AlbumId = albumId
                    });

                    db.SaveChanges();
                }
                Console.WriteLine("AlbumsPictures created successfully!");
            }
        }

        private static void SeedTags(SocialNetworkDbContext db)
        {
            int totalTags = db.Albums.Count()*20;

            var validtags = new List<Tag>();

            for (int i = 0; i < totalTags; i++)
            {
                var tag = new Tag
                {
                    Name = TagTransformer.Transform($"tag{i}")
                };

                validtags.Add(tag);
            }

            db.Tags.AddRange(validtags);

            db.SaveChanges();
            Console.WriteLine("Tags created successfully!");

            var albumIds = db.Albums.Select(a => a.Id).ToList();

            foreach (var tag in validtags)
            {
                var totalAlbum = random.Next(0, 20);

                for (int i = 0; i < totalAlbum; i++)
                {
                    var albumId = albumIds[random.Next(0, albumIds.Count)];

                    var tagExistForAlbums = db.Albums
                       .Any(a => a.Id == albumId && a.Tags.Any(t => t.TagId == tag.Id));

                    if (tagExistForAlbums)
                    {
                        i--;
                        continue;
                    }

                    var albumTag = new AlbumTag()
                    {
                        TagId = tag.Id,
                        AlbumId = albumId
                    };

                    tag.Albums.Add(albumTag);
                    db.SaveChanges();
                }
                Console.WriteLine("AlbumTags created successfully!");
            }
        }
    }
}
