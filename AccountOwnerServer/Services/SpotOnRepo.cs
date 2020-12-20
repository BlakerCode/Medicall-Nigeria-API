using Microsoft.EntityFrameworkCore;
using SpotOnAccountServer.Dtos;
using SpotOnAccountServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotOnAccountServer.Services
{
    public class SpotOnRepo
    {
        public static async Task<List<DocUserDTO>> GetPaged<T>(int page, int pageSize) where T : class
        {
            using (var _context = new DB_A5DE44_RussellContext())
            {
                return await _context.Specialists
                    .Where(c => c.User.IsDoc.Equals(true))
                    .Select(x => DocUserToDTO(x))
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
        }

        public static async Task<List<DocUserDTO>> FindCallListPaged<T>() where T : class
        {
            using (var _context = new DB_A5DE44_RussellContext())
            {
                var lists = await _context.CallLists
                    .Join(_context.Specialists,
                                             e => e.DocId,
                                             d => d.UserId,
                                             (doc, special) => new
                                             {
                                                 doc.DocId,
                                                 doc.FirstName,
                                                 doc.LastName,
                                                 doc.LoginName,
                                                 special.Language,
                                                 special.Photo,
                                                 special.Specialization,
                                             })
                                //.Select(x => DocUserToDTO(x)) 
                                .ToListAsync();

                List<DocUserDTO> docUserDTOs = new List<DocUserDTO>();
                foreach (var i in lists)
                {
                    var _doc = new DocUserDTO
                    {
                        Id = (int)i.DocId,
                        FirstName = i.FirstName,
                        LastName = i.LastName,
                        LoginName = i.LoginName,
                        Language = i.Language,
                        Photo = i.Photo,
                        Special = i.Specialization

                    };
                    docUserDTOs.Add(_doc);
                }
                return docUserDTOs;
            }
        }

        public static async Task<List<DocUserDTO>> FindPaged<T>(int page, int pageSize) where T : class
        {
            using (var _context = new DB_A5DE44_RussellContext())
            {
                var lists = await _context.Users
                    .Join(_context.Specialists,
                                             e => e.Id,
                                             d => d.UserId,
                                             (user, special) => new
                                             {
                                                 user.Id,
                                                 user.FirstName,
                                                 user.LastName,
                                                 user.LoginName,
                                                 special.Language,
                                                 special.Photo,
                                                 special.Specialization,
                                             })
                                //.Select(x => DocUserToDTO(x))
                                .Skip(page * pageSize)
                                .Take(pageSize)
                                .ToListAsync();

                List<DocUserDTO> docUserDTOs = new List<DocUserDTO>();
                foreach (var i in lists)
                {
                    var _doc = new DocUserDTO
                    {
                        Id = i.Id,
                        FirstName = i.FirstName,
                        LastName = i.LastName,
                        LoginName = i.LoginName,
                        Language = i.Language,
                        Photo = i.Photo,
                        Special = i.Specialization

                    };
                    docUserDTOs.Add(_doc);
                }
                return docUserDTOs;
            }
        }


        public static async Task<List<DocUserDTO>> FindSpecialPaged<T>(string special, int page, int pageSize) where T : class
        {
            using (var _context = new DB_A5DE44_RussellContext())
            {
                var lists = await _context.Users
                    .Join(_context.Specialists,
                                             e => e.Id,
                                             d => d.UserId,
                                             (user, spec) => new
                                             {
                                                 user.Id,
                                                 user.FirstName,
                                                 user.LastName,
                                                 user.LoginName,
                                                 spec.Language,
                                                 spec.Photo,
                                                 spec.Specialization
                                             })
                                .Where(c => c.Specialization == special)
                                //.Select(x => DocUserToDTO(x))
                                .Skip(page * pageSize)
                                .Take(pageSize)
                                .ToListAsync();

                List<DocUserDTO> docUserDTOs = new List<DocUserDTO>();
                foreach (var i in lists)
                {
                    var _doc = new DocUserDTO
                    {
                        Id = i.Id,
                        FirstName = i.FirstName,
                        LastName = i.LastName,
                        LoginName = i.LoginName,
                        Language = i.Language,
                        Photo = i.Photo,
                        Special = i.Specialization

                    };
                    docUserDTOs.Add(_doc);
                }
                return docUserDTOs;
            }
        }

        private static DocUserDTO DocUserToDTO(Specialists user) =>
        new DocUserDTO
        {
            Id = user.Id,
            Special = user.Specialization,
            FirstName = user.User.FirstName,
            LastName = user.User.LastName,
            Language = user.Language,
            Photo = user.Photo
        };

        private static UserDTO UserToDTO(Users user) =>
       new UserDTO
       {
           Id = user.Id,
           FirstName = user.FirstName,
           LastName = user.LastName,
           Expired = DateTime.Now <= Convert.ToDateTime(user.SubscriptionExpires),
           StateOrigin = user.StateOrigin,
           EmailAddress = user.EmailAddress,
           DateRegistered = user.DateRegistered,
           LocalGovt = user.LocalGovt,
           LoginName = user.LoginName
       };

        private static UserDTO UserToDTO(Specialists user) =>
       new UserDTO
       {
           Id = user.Id,
           FirstName = user.User.FirstName,
           LastName = user.User.LastName,
           Expired = DateTime.Now <= Convert.ToDateTime(user.User.SubscriptionExpires)
       };


        internal static async Task<T> FindById<T>(int id) where T : class
        {
            using (var _context = new DB_A5DE44_RussellContext())
            {
                return await _context.Set<T>().FindAsync(id);
            }
        }


        internal static async Task<List<UserDTO>> AddLoginAsync<T>(Users model, string path) where T : class
        {
            using (var _context = new DB_A5DE44_RussellContext())
            {
                var _course = _context.Users.Where(c => c.EmailAddress.Equals(model.EmailAddress)).ToList();
                if (_course.Count() == 0)
                {
                    string passwordHash;
                    //CreatePasswordHash(password, out passwordHash, out passwordSalt);
                    PasswordHashing.CreatePasswordHash(model.Password, out passwordHash);

                    model.Password = passwordHash;
                    Random random = new Random();
                    string code = random.Next(1000, 9999).ToString();
                    model.DateRegistered = DateTime.Now.ToShortDateString();
                    model.IsDoc = false;
                    model.Busy = false;
                    //model.ConfirmCode = code; 
                    //model.EmailConfirmed = false;

                    _context.Users.Add(model);
                    await _context.SaveChangesAsync();

                    //Emailing.SendAdminMailAsync(model.EmailAddress, path, code);
                }
                return await _context.Users
                    .Where(c => c.EmailAddress.Equals(model.EmailAddress))
                    .Select(x => UserToDTO(x))
                    .ToListAsync();
            }
        }


        internal static async Task<List<DocUserDTO>> AddDocAsync<T>(Specialists model) where T : class
        {
            using (var _context = new DB_A5DE44_RussellContext())
            {
                var _course = _context.Specialists.Where(c => c.UserId.Equals(model.UserId));
                if (_course.Count() == 0)
                {
                    _context.Specialists.Add(model);

                    await _context.SaveChangesAsync();
                }

                return await _context.Specialists
                     .Where(c => c.UserId.Equals(model.UserId))
                     .Select(x => DocUserToDTO(x))
                     .ToListAsync();

            }
        }

        internal static async Task<List<T>> AddTreatmentAsync<T>(Treatments model) where T : class
        {
            using (var _context = new DB_A5DE44_RussellContext())
            {
                var _course = _context.Treatments.Where(c => c.UserId.Equals(model.UserId));
                if (_course.Count() == 0)
                {
                    _context.Treatments.Add(model);

                    await _context.SaveChangesAsync();
                }

                return await _context.Set<T>().ToListAsync();
                //return _context.LincolnSubjects.ToArray();
            }
        }

        internal static List<UserDTO> AuthenticateLogins<T>(Users model) where T : class
        {
            using (var _context = new DB_A5DE44_RussellContext())
            {
                var _user = _context.Users.FirstOrDefault(c => c.LoginName.Equals(model.LoginName));

                if (_user == null)
                {
                    List<UserDTO> userDTOs = new List<UserDTO>();

                    var testObjectOne = new UserDTO
                    {
                        FirstName = "Invalid"
                    };
                    userDTOs.Add(testObjectOne);

                    return userDTOs;
                }
                else
                {
                    if (!PasswordHashing.VerifyPasswordHash(model.Password, _user.Password))
                    {
                        List<UserDTO> userDTOs = new List<UserDTO>();

                        var testObjectOne = new UserDTO
                        {
                            FirstName = "Invalid"
                        };
                        userDTOs.Add(testObjectOne);

                        return userDTOs;
                    }
                    else
                    {
                        if ((bool)_user.IsDoc)
                        {
                            List<UserDTO> userDTOs = new List<UserDTO>();

                            var testObjectOne = new UserDTO
                            {
                                FirstName = _user.FirstName,
                                LastName = _user.LastName,
                                LoginName = _user.LoginName,
                                IsDoc = true
                            };
                            userDTOs.Add(testObjectOne);

                            return userDTOs;
                        }
                        else
                        {
                            List<UserDTO> userDTOs = new List<UserDTO>();

                            DateTime today = DateTime.Now;
                            DateTime dateexpire = Convert.ToDateTime(_user.SubscriptionExpires);

                            bool expired = today <= dateexpire;

                            var testObjectOne = new UserDTO
                            {
                                FirstName = _user.FirstName,
                                LastName = _user.LastName,
                                LoginName = _user.LoginName,
                                Expired = expired,
                                IsDoc = false
                            };
                            userDTOs.Add(testObjectOne);

                            return userDTOs;
                        }
                    }
                }
            }
        }


        internal static bool AddPhoto(int id, string fileName)
        {
            using (var _context = new DB_A5DE44_RussellContext())
            {
                var user = _context.Specialists.Find(id);

                if (user == null)
                    return false;

                // update user properties 
                user.Photo = fileName;

                _context.Specialists.Update(user);
                _context.SaveChanges();

                return true;
            }
        }

        internal static object SetUserBusy(int id)
        {
            using (var _context = new DB_A5DE44_RussellContext())
            {
                var user = _context.Users.Find(id);

                if (user == null)
                    throw new Exception("User not found");

                user.Busy = true;
                _context.Users.Update(user);
                _context.SaveChanges();

                return _context.Users.SingleOrDefault(c => c.Id == id);
            }
        }

        public static object UpdatePassword(string email, string password)
        {
            using (var _context = new DB_A5DE44_RussellContext())
            {
                var user = _context.Users.FirstOrDefault(c => c.EmailAddress.Equals(email));

                if (user == null)
                    throw new Exception("User not found");

                // update password if it was entered
                if (!string.IsNullOrWhiteSpace(password))
                {
                    string passwordHash;
                    PasswordHashing.CreatePasswordHash(password, out passwordHash);
                    user.Password = passwordHash;

                    _context.Users.Update(user);
                    _context.SaveChanges();
                    var usr = _context.Users.Select(c => new { code = "password changed" });
                    return "password changed";// usr.ToList();
                }

                var users = _context.Users.Select(c => new { code = "password not changed" });
                return "password not changed";// users.ToList();

            }
        }

    }
}
