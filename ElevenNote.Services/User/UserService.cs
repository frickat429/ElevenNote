using ElevenNote.Data;
using ElevenNote.Data.Entities;
using ElevenNote.Models.User;
using Microsoft.AspNetCore.Identity;

namespace ElevenNote.Services.User;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<UserEntity> _userManger;
    private readonly SignInManager<UserEntity> _signInManger;

    public UserService(ApplicationDbContext context,
    UserManager<UserEntity> userManager,
    SignInManager<UserEntity> signInManager)
    {
        _context = context;
        _userManger = userManager;
        _signInManger = signInManager;
    }
    public async Task<bool> RegisterUserAsync(UserRegister model)
    {
            if(await CheckEmailAvailability(model.Email) == false) {
                System.Console.WriteLine("Invaild email, already in use"); 
                return false;
            } 

            if(await CheckUserNameAvailability(model.UserName) == false) {
                System.Console.WriteLine("Invalid username, already in use"); 
                return false;
            }
        UserEntity entity = new()
        { 
            Email = model.Email,
            UserName = model.UserName,
            DateCreated = DateTime.Now

        };

        IdentityResult registarResult = await _userManger.CreateAsync(entity, model.Password);
        return registarResult.Succeeded;
        } 

        private async Task<bool> CheckUserNameAvailability(string userName)
        {
            UserEntity? existingUser = await _userManger.FindByNameAsync(userName);
            return existingUser is null;
        } 

        private async Task<bool> CheckEmailAvailability(string email) 
        {
            UserEntity? existingUser = await _userManger.FindByEmailAsync(email); 
            return existingUser is null;
        }
        }